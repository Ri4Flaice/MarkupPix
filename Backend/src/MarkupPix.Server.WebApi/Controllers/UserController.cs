using MarkupPix.Business.Feature.User;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;
using MarkupPix.Server.WebApi.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkupPix.Server.WebApi.Controllers;

/// <summary>
/// Controller for works with user data.
/// </summary>
[ApiController]
[Route("api/user")]
public class UserController : BaseController<UserController>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="UserController"/>.
    /// </summary>
    /// <param name="mediator">The AutoMediator.</param>
    /// <param name="userManager">User status management interface.</param>
    public UserController(IMediator mediator, UserManager<UserEntity> userManager)
        : base(mediator, userManager)
    {
    }

    /// <summary>
    /// Create user.
    /// </summary>
    /// <param name="request">Create user request.</param>
    /// <returns>User ID.</returns>
    [HttpPost("create")]
    public async Task<long> CreateUser([FromBody] CreateUserRequest request) =>
        await Mediator.Send(new CreateUser.Command(request));

    /// <summary>
    /// User login to the system.
    /// </summary>
    /// <param name="request">Login user request.</param>
    /// <returns>The success of the completed operation.</returns>
    [HttpPost("login")]
    public async Task<bool> Login([FromBody] LoginUserRequest request)
    {
        var token = await Mediator.Send(new LoginUser.Command(request));

        if (string.IsNullOrEmpty(token)) return false;

        HttpContext.Response.Cookies.Append("secretCookies", token);

        return true;
    }

    /// <summary>
    /// Get user by id.
    /// </summary>
    /// <param name="emailAddress">User email address.</param>
    /// <returns>Response with data user.</returns>
    [Authorize]
    [HttpGet("{emailAddress}")]
    public async Task<GetUserResponse> GetUser(string emailAddress) =>
        await Mediator.Send(new GetUser.Command(emailAddress));
}