using FluentValidation;

using MarkupPix.Core.Errors;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Update document.
/// </summary>
public static class UpdateDocument
{
    /// <summary>
    /// Command for update document.
    /// </summary>
    /// <param name="UpdateDocumentRequest">Request update document.</param>
    /// <param name="File">Document.</param>
    public record Command(UpdateDocumentRequest UpdateDocumentRequest, IFormFile? File, UserEntity CurrentUser) : IRequest<bool>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="documentValidator">Checking the request description.</param>
        public Validator(IValidator<UpdateDocumentRequest> documentValidator)
        {
            RuleFor(q => q.UpdateDocumentRequest)
                .NotNull()
                .SetValidator(documentValidator);
        }
    }

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, bool>
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
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocument = await _dbContext
                    .DocumentsEntities
                    .SingleOrDefaultAsync(d => d.DocumentName == request.UpdateDocumentRequest.DocumentName, cancellationToken);

                if (existingDocument == null)
                    throw new BusinessException(nameof(Errors.MPX202), Errors.MPX202);

                if (existingDocument.NumberPages == request.UpdateDocumentRequest.NumberPages &&
                    existingDocument.DocumentDescription == request.UpdateDocumentRequest.DocumentDescription)
                    return true;

                existingDocument.UserId = request.CurrentUser.Id;

                if (request.UpdateDocumentRequest.NumberPages < existingDocument.NumberPages)
                    throw new BusinessException(nameof(Errors.MPX211), Errors.MPX211);

                if (request.UpdateDocumentRequest.NumberPages != null)
                    existingDocument.NumberPages = (int)request.UpdateDocumentRequest.NumberPages;

                if (request.UpdateDocumentRequest.DocumentDescription != null)
                    existingDocument.DocumentDescription = request.UpdateDocumentRequest.DocumentDescription;

                if (request.File != null)
                {
                    using var documentStream = new MemoryStream();
                    await request.File.CopyToAsync(documentStream, cancellationToken);
                    existingDocument.File = documentStream.ToArray();
                }

                _dbContext.DocumentsEntities.Update(existingDocument);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX206, e.Message);
                throw;
            }
        }
    }
}