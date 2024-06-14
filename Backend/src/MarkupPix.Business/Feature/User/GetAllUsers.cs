using System.Text.Json;

using AutoMapper;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// Get all users.
/// </summary>
public static class GetAllUsers
{
    /// <summary>
    /// Command for get users.
    /// </summary>
    public record Command : IRequest<IEnumerable<GetUserResponse>>;

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, IEnumerable<GetUserResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        /// <param name="cache">Distributed cache.</param>
        public Handler(AppDbContext dbContext, IMapper mapper, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheUsers = await _cache.GetStringAsync("users", cancellationToken);

                if (cacheUsers != null)
                {
                    return JsonSerializer.Deserialize<IEnumerable<GetUserResponse>>(cacheUsers) ??
                           throw new InvalidOperationException("Failed to deserialize cached user data.");
                }

                var usersResponse = _dbContext.Users.ToList();

                if (usersResponse == null)
                    throw new Exception("There are no users in the database.");

                var users = _mapper.Map<List<UserEntity>, List<GetUserResponse>>(usersResponse);

                await _cache.SetStringAsync(
                    "users",
                    JsonSerializer.Serialize(users),
                    cancellationToken);

                return users;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}