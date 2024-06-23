using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Get document.
/// </summary>
public static class GetDocument
{
    /// <summary>
    /// Command for get document.
    /// </summary>
    /// <param name="DocumentName">Document name, which needs get.</param>
    public record Command(string DocumentName) : IRequest<GetDocumentResponse>;

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, GetDocumentResponse>
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
        public async Task<GetDocumentResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocument = await _dbContext
                    .DocumentsEntities
                    .SingleOrDefaultAsync(d => d.DocumentName == request.DocumentName, cancellationToken);

                if (existingDocument == null)
                    throw new Exception("A document with that name does not exist.");

                var existingPages = await _dbContext.PageEntities
                    .Where(p => p.DocumentId == existingDocument.Id).Include(pageEntity => pageEntity.User)
                    .ToListAsync(cancellationToken);

                if (existingPages.Count == 0)
                    throw new Exception("No pages found for the document");

                var existingUser = await _dbContext
                    .UsersEntities
                    .SingleOrDefaultAsync(u => u.Id == existingDocument.UserId, cancellationToken);

                if (existingUser == null)
                    throw new Exception("The user was not found.");

                var response = new GetDocumentResponse
                {
                    UserEmailAddress = existingUser.EmailAddress,
                    DocumentName = existingDocument.DocumentName,
                    DocumentDescription = existingDocument.DocumentDescription,
                    NumberPages = existingDocument.NumberPages,
                    File = existingDocument.File,
                    Pages = existingPages.Select(p => new GetPageResponse
                    {
                        UserEmailAddress = p.User?.EmailAddress,
                        IsRecognize = p.IsRecognize ?? false,
                        DateRecognize = p.DateRecognize ?? DateTime.MinValue,
                        NumberPage = p.NumberPage,
                        Page = p.Page,
                    }).ToList(),
                };

                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}