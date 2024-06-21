using FluentValidation;

using MarkupPix.Data.Data;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
    public record Command(UpdateDocumentRequest UpdateDocumentRequest, IFormFile? File) : IRequest<bool>;

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

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public Handler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
                    throw new Exception("A document with that name does not exist.");

                var user = await _dbContext
                    .UsersEntities
                    .SingleOrDefaultAsync(d => d.EmailAddress == request.UpdateDocumentRequest.UserEmailAddress, cancellationToken);

                if (user == null)
                    throw new Exception("There is no such user.");

                existingDocument.UserId = user.Id;

                if (request.UpdateDocumentRequest.NumberPages != null && request.UpdateDocumentRequest.NumberPages > existingDocument.NumberPages)
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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}