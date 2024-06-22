using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Get all documents.
/// </summary>
public class GetAllDocuments
{
    /// <summary>
    /// Command for get all documents.
    /// </summary>
    public record Command : IRequest<IEnumerable<GetDocumentResponse>>;

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, IEnumerable<GetDocumentResponse>>
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetDocumentResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocuments = await _dbContext.DocumentsEntities.ToListAsync(cancellationToken);

                if (existingDocuments == null)
                    throw new Exception("A documents does not exist.");

                var documentsResponse = new List<GetDocumentResponse>();

                foreach (var document in existingDocuments)
                {
                    var existingPages = await _dbContext.PageEntities
                        .Where(p => p.DocumentId == document.Id)
                        .Include(p => p.User)
                        .ToListAsync(cancellationToken);

                    if (existingPages.Count == 0)
                        throw new Exception("No pages found for the document");

                    var existingUser = await _dbContext.UsersEntities
                        .SingleOrDefaultAsync(u => u.Id == document.UserId, cancellationToken);

                    if (existingUser == null)
                        throw new Exception("The user was not found.");

                    var response = new GetDocumentResponse
                    {
                        UserEmailAddress = existingUser.EmailAddress,
                        DocumentName = document.DocumentName,
                        DocumentDescription = document.DocumentDescription,
                        NumberPages = document.NumberPages,
                        File = document.File,
                        Pages = existingPages.Select(p => new GetPageResponse
                        {
                            UserEmailAddress = p.User?.EmailAddress,
                            IsRecognize = p.IsRecognize ?? false,
                            DateRecognize = p.DateRecognize ?? DateTime.MinValue,
                            NumberPage = p.NumberPage,
                            Page = p.Page,
                        }).ToList(),
                    };

                    documentsResponse.Add(response);
                }

                return documentsResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}