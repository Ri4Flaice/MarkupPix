using AutoMapper;

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
/// Create page.
/// </summary>
public static class CreatePage
{
    /// <summary>
    /// Command for create page.
    /// </summary>
    /// <param name="CreatePageRequest">Request create page.</param>
    /// <param name="Pages">Pages.</param>
    public record Command(CreatePageRequest CreatePageRequest, IEnumerable<IFormFile> Pages) : IRequest<bool>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="pageValidator">Checking the request description.</param>
        public Validator(IValidator<CreatePageRequest> pageValidator)
        {
            RuleFor(q => q.CreatePageRequest)
                .NotNull()
                .SetValidator(pageValidator);

            RuleFor(q => q.Pages)
                .NotNull()
                .Must(page => page.Any())
                .WithMessage("Pages is not provided or empty.");
        }
    }

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<Handler> _logger;

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        /// <param name="logger">The event log.</param>
        public Handler(AppDbContext dbContext, IMapper mapper, ILogger<Handler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocument = await _dbContext
                    .DocumentsEntities
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.DocumentName == request.CreatePageRequest.DocumentName, cancellationToken);

                if (existingDocument == null)
                    throw new BusinessException(nameof(Errors.MPX202), Errors.MPX202);

                if (existingDocument.NumberPages != request.Pages.Count())
                    throw new BusinessException(nameof(Errors.MPX207), Errors.MPX207);

                using var memoryStream = new MemoryStream();
                var pagesEntity = new List<PageEntity>();

                foreach (var (file, numberPage) in request.Pages.Select((file, index) => (file, index + 1)))
                {
                    var pageEntity = _mapper.Map<PageEntity>(request.CreatePageRequest);

                    pageEntity.DocumentId = existingDocument.Id;
                    pageEntity.NumberPage = numberPage;

                    await file.CopyToAsync(memoryStream, cancellationToken);
                    pageEntity.Page = memoryStream.ToArray();

                    pagesEntity.Add(pageEntity);
                }

                await _dbContext.PageEntities.AddRangeAsync(pagesEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX208, e.Message);
                throw;
            }
        }
    }
}