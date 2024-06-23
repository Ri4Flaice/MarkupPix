using FluentValidation;

using MarkupPix.Core;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Feature.Page;

/// <summary>
/// Page's validator.
/// </summary>
public class PageValidator : AbstractValidator<CreatePageRequest>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="PageValidator"/>.
    /// </summary>
    public PageValidator()
    {
        RuleFor(d => d.DocumentName)
            .NotEmpty()
            .Matches(RegularExpressions.EnglishTitleDocument)
            .WithMessage("Incorrect document name");
    }
}