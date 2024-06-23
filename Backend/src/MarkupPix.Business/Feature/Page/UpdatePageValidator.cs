using FluentValidation;

using MarkupPix.Core;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Feature.Page;

/// <summary>
/// Page's validator.
/// </summary>
public class UpdatePageValidator : AbstractValidator<UpdatePageRequest>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="UpdatePageValidator"/>.
    /// </summary>
    public UpdatePageValidator()
    {
        RuleFor(d => d.DocumentName)
            .NotEmpty()
            .Matches(RegularExpressions.EnglishTitleDocument)
            .WithMessage("The name should consist of English words");
    }
}