using FluentValidation;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Business.Feature.Page;

/// <summary>
/// Update page.
/// </summary>
public static class UpdatePage
{
    /// <summary>
    /// Command for update page.
    /// </summary>
    /// <param name="UpdatePageRequest">Update page request.</param>
    /// <param name="Page">Page.</param>
    public record Command(UpdatePageRequest UpdatePageRequest, IFormFile? Page, UserEntity CurrentUser) : IRequest<bool>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="pageValidator">Checking the request description.</param>
        public Validator(IValidator<UpdatePageRequest> pageValidator)
        {
            RuleFor(q => q.UpdatePageRequest)
                .NotEmpty()
                .SetValidator(pageValidator);
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
                    .SingleOrDefaultAsync(d => d.DocumentName == request.UpdatePageRequest.DocumentName, cancellationToken);

                if (existingDocument == null)
                    throw new Exception("The document was not found.");

                var existingPage = await _dbContext
                    .PageEntities
                    .SingleOrDefaultAsync(p => p.DocumentId == existingDocument.Id && p.NumberPage == request.UpdatePageRequest.NumberPage, cancellationToken);

                if (existingPage == null)
                    throw new Exception("No pages found for the document");

                existingPage.IsRecognize = true;
                existingPage.DateRecognize = DateTime.Now;
                existingPage.RecognizeUser = request.CurrentUser.Id;

                if (request.Page != null)
                {
                    using var pageStream = new MemoryStream();
                    await request.Page.CopyToAsync(pageStream, cancellationToken);
                    existingPage.Page = pageStream.ToArray();
                }

                _dbContext.PageEntities.Update(existingPage);

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