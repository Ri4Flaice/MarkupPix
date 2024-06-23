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
                var response = await _dbContext.DocumentsEntities
                    .Where(d => d.DocumentName == request.DocumentName)
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
                    .SingleOrDefaultAsync(cancellationToken);

                if (response == null)
                    throw new Exception("A document with that name does not exist.");

                if (response.Pages == null || response.Pages.Count == 0)
                    throw new Exception("No pages found for the document");

                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}