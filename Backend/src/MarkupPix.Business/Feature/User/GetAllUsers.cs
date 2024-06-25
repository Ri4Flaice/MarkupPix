using AutoMapper;

using MarkupPix.Core.Errors;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<Handler> _logger;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        /// <param name="logger">The event log.</param>
        public Handler(AppDbContext dbContext, IMapper mapper, ILogger<Handler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var usersResponse = await _dbContext.UsersEntities.ToListAsync(cancellationToken);

                if (usersResponse == null)
                    throw new BusinessException(nameof(Errors.MPX104), Errors.MPX104);

                var users = _mapper.Map<List<UserEntity>, List<GetUserResponse>>(usersResponse);

                return users;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX105, e.Message);
                throw;
            }
        }
    }
}