using FluentValidation;

using MarkupPix.Core.Errors;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                var pageToUpdate = await _dbContext.PageEntities
                    .Include(p => p.Document)
                    .SingleOrDefaultAsync(
                        p =>
                            p.Document != null &&
                            p.Document.DocumentName == request.UpdatePageRequest.DocumentName &&
                            p.NumberPage == request.UpdatePageRequest.NumberPage,
                        cancellationToken);

                if (pageToUpdate == null)
                    throw new BusinessException(nameof(Errors.MPX209), Errors.MPX209);

                pageToUpdate.IsRecognize = true;
                pageToUpdate.DateRecognize = DateTime.Now;
                pageToUpdate.RecognizeUser = request.CurrentUser.Id;

                if (request.Page != null)
                {
                    using var pageStream = new MemoryStream();
                    await request.Page.CopyToAsync(pageStream, cancellationToken);
                    pageToUpdate.Page = pageStream.ToArray();
                }

                _dbContext.PageEntities.Update(pageToUpdate);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX210, e.Message);
                throw;
            }
        }
    }
}