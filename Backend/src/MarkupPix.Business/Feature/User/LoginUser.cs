using System.Text.Json;

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
/// User login to the system.
/// </summary>
public static class LoginUser
{
    /// <summary>
    /// Command for user login to the system.
    /// </summary>
    /// <param name="LoginUserRequest">Login user request.</param>
    public record Command(LoginUserRequest LoginUserRequest) : IRequest<string>;

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, string>
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly JwtProvider _jwtProvider;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="userManager">User status management interface.</param>
        /// <param name="jwtProvider">JWT provider.</param>
        /// <param name="cache">Distributed cache.</param>
        public Handler(
            AppDbContext dbContext,
            UserManager<UserEntity> userManager,
            JwtProvider jwtProvider,
            IDistributedCache cache)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var cacheKey = $"users:{request.LoginUserRequest.Email}";
            var cachedUser = await _cache.GetStringAsync(cacheKey, cancellationToken);

            var user = !string.IsNullOrEmpty(cachedUser)
                ? JsonSerializer.Deserialize<UserEntity>(cachedUser)
                : await _dbContext.UsersEntities.FirstOrDefaultAsync(
                    u => u.EmailAddress == request.LoginUserRequest.Email, cancellationToken);

            if (user == null)
                throw new Exception("The email or password is incorrect.");

            if (string.IsNullOrEmpty(cachedUser))
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(user), cancellationToken);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.LoginUserRequest.Password ?? throw new Exception("User's password empty."));

            if (!isPasswordValid)
                throw new Exception("The email or password is incorrect.");

            var token = await _jwtProvider.GenerateToken(user, _userManager);

            return token;
        }
    }
}