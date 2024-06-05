using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarkupPix.Server.WebApi.Infrastructure;

/// <summary>
/// Validation exception filter.
/// </summary>
public class ValidationExceptionFilter : IExceptionFilter
{
    /// <inheritdoc />
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException validationException) return;

        var errors = validationException.Errors
            .Select(e => new { e.PropertyName, e.ErrorMessage })
            .ToList();

        context.Result = new BadRequestObjectResult(new { Errors = errors });
        context.ExceptionHandled = true;
    }
}