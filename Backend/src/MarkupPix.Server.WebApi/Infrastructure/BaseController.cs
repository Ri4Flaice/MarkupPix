using MarkupPix.Data.Entities;

using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkupPix.Server.WebApi.Infrastructure;

/// <summary>
/// Base controller.
/// </summary>
/// <typeparam name="T">Controller type.</typeparam>
public class BaseController<T> : Controller
    where T : BaseController<T>
{
    internal readonly IMediator Mediator;
    internal readonly UserManager<UserEntity> UserManager;

    /// <summary>
    /// Initializes a new instance of the class <see cref="BaseController{T}"/>
    /// </summary>
    /// <param name="mediator">The AutoMediator.</param>
    /// <param name="userManager">User status management interface.</param>
    protected BaseController(IMediator mediator, UserManager<UserEntity> userManager)
    {
        Mediator = mediator;
        UserManager = userManager;
    }

    /// <summary>
    /// Get current user.
    /// </summary>
    /// <returns>Current user.</returns>
    protected async Task<UserEntity> GetCurrentUser() =>
        await UserManager.GetUserAsync(User) ??
            throw new AuthenticationFailureException("User not found.");
}