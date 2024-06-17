using System.Text.Json;

using AutoMapper;

using FluentValidation;

using MarkupPix.Business.Infrastructure;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// Create user.
/// </summary>
public static class CreateUser
{
    /// <summary>
    /// Command for create user.
    /// </summary>
    /// <param name="CreateUserRequest">Model for create user.</param>
    public record Command(CreateUserRequest CreateUserRequest) : IRequest<long>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="userValidator">Checking the request description.</param>
        public Validator(IValidator<CreateUserRequest> userValidator)
        {
            RuleFor(q => q.CreateUserRequest)
                .NotNull()
                .SetValidator(userValidator);
        }
    }

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, long>
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userManager">User status management interface.</param>
        /// <param name="mapper">The AutoMapper.</param>
        /// <param name="cache">Distributed cache.</param>
        public Handler(
            AppDbContext dbContext,
            UserManager<UserEntity> userManager,
            IMapper mapper,
            IDistributedCache cache)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<long> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _dbContext
                    .UsersEntities
                    .AsNoTracking()
                    .SingleOrDefaultAsync(e => e.EmailAddress == request.CreateUserRequest.EmailAddress, cancellationToken);

                if (existingUser != default)
                    throw new Exception("A user with such an email already exists.");

                var user = _mapper.Map<UserEntity>(request.CreateUserRequest);
                var result = await _userManager.CreateAsync(user, request.CreateUserRequest.Password);

                if (!result.Succeeded)
                    throw new Exception("Error during user registration.");

                var role = UserRoles.RolesList[request.CreateUserRequest.AccountType];
                await _userManager.AddToRoleAsync(user, role);

                _dbContext.UsersEntities.Add(user);

                var cacheKey = $"users:{request.CreateUserRequest.EmailAddress}";
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), cancellationToken);

                return user.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}