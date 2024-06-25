using MarkupPix.Core.Errors;
using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<Handler> _logger;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="logger">The event log.</param>
        public Handler(AppDbContext dbContext, ILogger<Handler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<GetDocumentResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var documentResponse = await _dbContext.DocumentsEntities
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

                if (documentResponse == null)
                    throw new BusinessException(nameof(Errors.MPX202), Errors.MPX202);

                if (documentResponse.Pages == null || documentResponse.Pages.Count == 0)
                    throw new BusinessException(nameof(Errors.MPX204), Errors.MPX204);

                return documentResponse;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX205, e.Message);
                throw;
            }
        }
    }
}