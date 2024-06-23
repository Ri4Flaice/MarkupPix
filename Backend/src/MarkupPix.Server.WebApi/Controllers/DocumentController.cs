using MarkupPix.Business.Feature.Document;
using MarkupPix.Business.Infrastructure;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;
using MarkupPix.Server.WebApi.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkupPix.Server.WebApi.Controllers;

/// <summary>
/// Controller for works with document data.
/// </summary>
[ApiController]
[Route("api/document")]
public class DocumentController : BaseController<DocumentController>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="DocumentController"/>.
    /// </summary>
    /// <param name="mediator">The AutoMapper.</param>
    /// <param name="userManager">User status management interface.</param>
    public DocumentController(IMediator mediator, UserManager<UserEntity> userManager)
        : base(mediator, userManager)
    {
    }

    /// <summary>
    /// Create document.
    /// </summary>
    /// <param name="request">Create document request.</param>
    /// <param name="file">Document.</param>
    /// <returns>Document ID.</returns>
    [Authorize(Roles = UserRoles.AtFileManager)]
    [HttpPost("create")]
    [FileUpload]
    public async Task<long> CreateDocument([FromForm] CreateDocumentRequest request, [FromForm] IFormFile file) =>
        await Mediator.Send(new CreateDocument.Command(request, file, await GetCurrentUser()));

    /// <summary>
    /// Update document.
    /// </summary>
    /// <param name="request">Update document request.</param>
    /// <param name="file">Document.</param>
    /// <returns>The success of the operation.</returns>
    [Authorize(Roles = UserRoles.AtFileManager)]
    [HttpPatch("update")]
    [FileUpload]
    public async Task<bool> UpdateDocument([FromForm] UpdateDocumentRequest request, [FromForm] IFormFile? file) =>
        await Mediator.Send(new UpdateDocument.Command(request, file, await GetCurrentUser()));

    /// <summary>
    /// Get document.
    /// </summary>
    /// <param name="documentName">Document name, which needs get.</param>
    /// <returns>Document and pages data.</returns>
    [Authorize(Roles = UserRoles.AtFileManager)]
    [HttpGet("{documentName}")]
    public async Task<GetDocumentResponse> GetDocument(string documentName) =>
        await Mediator.Send(new GetDocument.Command(documentName));

    /// <summary>
    /// Get all documents.
    /// </summary>
    /// <returns>All documents and pages data.</returns>
    [Authorize(Roles = UserRoles.AtFileManager)]
    [HttpGet("alldocuments")]
    public async Task<IEnumerable<GetDocumentResponse>> GetAllDocuments() =>
        await Mediator.Send(new GetAllDocuments.Command());
}