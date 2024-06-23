using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Get all documents.
/// </summary>
public static class GetAllDocuments
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
                var documentsResponse = await _dbContext.DocumentsEntities
                    .Include(d => d.Pages)!
                    .ThenInclude(p => p.User)
                    .Select(document => new GetDocumentResponse
                    {
                        UserEmailAddress = document.User!.EmailAddress,
                        DocumentName = document.DocumentName,
                        DocumentDescription = document.DocumentDescription,
                        NumberPages = document.NumberPages,
                        File = document.File,
                        Pages = document.Pages!.Select(p => new GetPageResponse
                        {
                            UserEmailAddress = p.User!.EmailAddress,
                            IsRecognize = p.IsRecognize ?? false,
                            DateRecognize = p.DateRecognize ?? DateTime.MinValue,
                            NumberPage = p.NumberPage,
                            Page = p.Page,
                        }).ToList(),
                    })
                    .ToListAsync(cancellationToken);

                if (documentsResponse == null || documentsResponse.Count == 0)
                    throw new Exception("No documents found.");

                return documentsResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}