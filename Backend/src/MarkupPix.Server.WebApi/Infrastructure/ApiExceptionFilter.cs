using FluentValidation;

using MarkupPix.Core.Errors;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarkupPix.Server.WebApi.Infrastructure;

/// <summary>
/// A handler for standard exceptions.
/// </summary>
public sealed class ApiExceptionFilter : IExceptionFilter
{
    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiExceptionFilter>>();
        logger.LogError(context.Exception, context.Exception.Message);

        switch (context.Exception)
        {
            case ValidationException validationException:
                var validationError = new ErrorResponse(
                    nameof(Errors.MPX400),
                    string.Join("; ", validationException.Errors.Select(e => $"{e.ErrorMessage}")));
                context.Result = new BadRequestObjectResult(validationError);
                break;

            case BusinessException businessException:
                var businessError = new ErrorResponse(businessException.ErrorNumber, businessException.ErrorText);
                context.Result = new BadRequestObjectResult(businessError);
                break;

            default:
                var error = new ErrorResponse(Errors.MPX500, context.Exception.Message);
                context.Result = new ObjectResult(error) { StatusCode = StatusCodes.Status500InternalServerError };
                break;
        }
    }
}