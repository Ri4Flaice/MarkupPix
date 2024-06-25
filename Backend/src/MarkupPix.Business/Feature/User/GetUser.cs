using System.Text.Json;

using AutoMapper;

using MarkupPix.Core.Errors;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// Get user.
/// </summary>
public static class GetUser
{
    /// <summary>
    /// Command for get user.
    /// </summary>
    /// <param name="UserEmailAddress">User, which needs to get.</param>
    public record Command(string UserEmailAddress) : IRequest<GetUserResponse>;

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, GetUserResponse>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<Handler> _logger;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        /// <param name="cache">Distributed cache.</param>
        /// <param name="logger">The event log.</param>
        public Handler(
            AppDbContext dbContext,
            IMapper mapper,
            IDistributedCache cache,
            ILogger<Handler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<GetUserResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"user:{request.UserEmailAddress}";
                var cachedUser = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (cachedUser != null)
                {
                    return JsonSerializer.Deserialize<GetUserResponse>(cachedUser) ??
                           throw new BusinessException(nameof(Errors.MPX300), Errors.MPX300);
                }

                var userResponse = await _dbContext.UsersEntities.SingleOrDefaultAsync(u => u.EmailAddress == request.UserEmailAddress, cancellationToken);

                if (userResponse == null)
                    throw new BusinessException(nameof(Errors.MPX106), Errors.MPX106);

                var user = _mapper.Map<UserEntity, GetUserResponse>(userResponse);

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), cancellationToken);

                return user;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX107, e.Message);
                throw;
            }
        }
    }
}