using System.Text.Json;

using FluentValidation;

using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// Update user.
/// </summary>
public static class UpdateUser
{
    /// <summary>
    /// Command for update user.
    /// </summary>
    /// <param name="UpdateUserRequest">User, which needs to update.</param>
    public record Command(UpdateUserRequest UpdateUserRequest) : IRequest<bool>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="userValidator">Checking the request description.</param>
        public Validator(IValidator<UpdateUserRequest> userValidator)
        {
            RuleFor(q => q.UpdateUserRequest)
                .NotNull()
                .SetValidator(userValidator);
        }
    }

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly AppDbContext _dbContext;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="cache">Distributed cache.</param>
        public Handler(AppDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _dbContext.UsersEntities.SingleOrDefaultAsync(u => u.EmailAddress == request.UpdateUserRequest.EmailAddress, cancellationToken);

                if (existingUser == null)
                    throw new Exception("This user has not been found.");

                if (request.UpdateUserRequest.Block == existingUser.Block && request.UpdateUserRequest.AccountType == existingUser.AccountType) return true;

                var cacheKey = $"user:{request.UpdateUserRequest.EmailAddress}";
                var cacheValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (cacheValue == null)
                    throw new Exception("User not found in the cache.");

                var userToUpdate = JsonSerializer.Deserialize<GetUserResponse>(cacheValue) ?? throw new Exception("Failed to deserialize user data from cache.");

                var previousBlock = existingUser.Block;
                var previousAccountType = existingUser.AccountType;

                if (request.UpdateUserRequest.Block != null && request.UpdateUserRequest.Block != previousBlock)
                {
                    existingUser.Block = request.UpdateUserRequest.Block.Value;
                    userToUpdate.Block = request.UpdateUserRequest.Block.Value;
                }

                if (request.UpdateUserRequest.AccountType != null && request.UpdateUserRequest.AccountType != previousAccountType)
                {
                    existingUser.AccountType = request.UpdateUserRequest.AccountType.Value;
                    userToUpdate.AccountType = request.UpdateUserRequest.AccountType.Value;
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(userToUpdate), cancellationToken);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}