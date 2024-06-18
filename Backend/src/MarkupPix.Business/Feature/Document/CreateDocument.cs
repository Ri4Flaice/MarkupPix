using AutoMapper;

using FluentValidation;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
    public record Command(CreateDocumentRequest CreateDocumentRequest, IFormFile File) : IRequest<long>;

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
        public async Task<long> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDocument = await _dbContext
                    .DocumentsEntities
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.DocumentName == request.CreateDocumentRequest.DocumentName, cancellationToken);

                if (existingDocument != default)
                    throw new Exception("A document with such an name already exists.");

                var user = await _dbContext
                    .UsersEntities
                    .AsNoTracking()
                    .SingleOrDefaultAsync(d => d.EmailAddress == request.CreateDocumentRequest.UserEmailAddress, cancellationToken);

                if (user == null)
                    throw new Exception("There is no such user.");

                var document = _mapper.Map<DocumentEntity>(request.CreateDocumentRequest);

                document.UserId = user.Id;

                using var documentStream = new MemoryStream();
                await request.File.CopyToAsync(documentStream, cancellationToken);
                document.File = documentStream.ToArray();

                _dbContext.DocumentsEntities.Add(document);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return document.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}