using AutoMapper;

using FluentValidation;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Business.Feature.Page;

/// <summary>
/// Create page.
/// </summary>
public class CreatePage
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

        /// <summary>
        /// Initializes a new instance of the class <see cref="Handler"/>.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="mapper">The AutoMapper.</param>
        public Handler(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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
                    throw new Exception("Document not found.");

                using var memoryStream = new MemoryStream();

                if (existingDocument.NumberPages != request.Pages.Count())
                    throw new Exception("The number of pages does not match.");

                foreach (var file in request.Pages)
                {
                    var pageEntity = _mapper.Map<PageEntity>(request.CreatePageRequest);

                    pageEntity.DocumentId = existingDocument.Id;

                    await file.CopyToAsync(memoryStream, cancellationToken);
                    pageEntity.Page = memoryStream.ToArray();

                    _dbContext.PageEntities.Add(pageEntity);
                }

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