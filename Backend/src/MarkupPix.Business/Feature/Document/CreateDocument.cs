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

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Create document.
/// </summary>
public static class CreateDocument
{
    /// <summary>
    /// Command for create document.
    /// </summary>
    /// <param name="CreateDocumentRequest">Request create document.</param>
    /// <param name="File">Document.</param>
    public record Command(CreateDocumentRequest CreateDocumentRequest, IFormFile File, UserEntity CurrentUser) : IRequest<long>;

    /// <inheritdoc />
    public class Validator : AbstractValidator<Command>
    {
        /// <summary>
        /// Initializes a new instance of the class <see cref="Validator"/>.
        /// </summary>
        /// <param name="documentValidator">Checking the request description.</param>
        public Validator(IValidator<CreateDocumentRequest> documentValidator)
        {
            RuleFor(q => q.CreateDocumentRequest)
                .NotNull()
                .SetValidator(documentValidator);

            RuleFor(q => q.File)
                .NotNull()
                .Must(file => file.Length > 0)
                .WithMessage("File is not provided or empty.");
        }
    }

    /// <inheritdoc />
    public class Handler : IRequestHandler<Command, long>
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
        public async Task<long> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocument = await _dbContext
                    .DocumentsEntities
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.DocumentName == request.CreateDocumentRequest.DocumentName, cancellationToken);

                if (existingDocument != default)
                    throw new BusinessException(nameof(Errors.MPX201), Errors.MPX201);

                var document = _mapper.Map<DocumentEntity>(request.CreateDocumentRequest);

                document.UserId = request.CurrentUser.Id;

                using var documentStream = new MemoryStream();
                await request.File.CopyToAsync(documentStream, cancellationToken);
                document.File = documentStream.ToArray();

                _dbContext.DocumentsEntities.Add(document);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return document.Id;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, message: Errors.MPX200, e.Message);
                throw;
            }
        }
    }
}