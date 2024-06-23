using AutoMapper;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MediatR;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        public Handler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var usersResponse = await _dbContext.UsersEntities.ToListAsync(cancellationToken);

                if (usersResponse == null)
                    throw new Exception("There are no users in the database.");

                var users = _mapper.Map<List<UserEntity>, List<GetUserResponse>>(usersResponse);

                return users;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}