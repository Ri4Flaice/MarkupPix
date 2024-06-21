using MarkupPix.Business.Feature.Page;
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
/// Controller for works with page data.
/// </summary>
[ApiController]
[Route("api/page")]
public class PageController : BaseController<PageController>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="PageController"/>.
    /// </summary>
    /// <param name="mediator">The AutoMapper.</param>
    /// <param name="userManager">User status management interface.</param>
    public PageController(IMediator mediator, UserManager<UserEntity> userManager)
        : base(mediator, userManager)
    {
    }

    /// <summary>
    /// Create pages.
    /// </summary>
    /// <param name="request">Create pages request.</param>
    /// <param name="pages">Pages.</param>
    /// <returns>The success of the operation.</returns>
    [Authorize(Roles = UserRoles.AtFileManager)]
    [HttpPost("create")]
    [FileUpload]
    public async Task<bool> CreatePages([FromForm] CreatePageRequest request, [FromForm] IEnumerable<IFormFile> pages)
    {
        var formFiles = pages.ToList();
        if (formFiles.Any(file => file.ContentType != "image/png"))
        {
            return false;
        }

        return await Mediator.Send(new CreatePage.Command(request, formFiles));
    }
}