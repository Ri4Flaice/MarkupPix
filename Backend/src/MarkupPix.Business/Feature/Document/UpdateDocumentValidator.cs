using FluentValidation;

using MarkupPix.Core;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Feature.Document;

/// <summary>
/// Document's validator.
/// </summary>
public class UpdateDocumentValidator : AbstractValidator<UpdateDocumentRequest>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="UpdateDocumentValidator"/>.
    /// </summary>
    public UpdateDocumentValidator()
    {
        RuleFor(d => d.UserEmailAddress)
            .NotEmpty()
            .Matches(RegularExpressions.Email)
            .WithMessage("Incorrect email");

        RuleFor(d => d.DocumentName)
            .NotEmpty()
            .Matches(RegularExpressions.EnglishTitleDocument)
            .WithMessage("The name should consist of English words");

        RuleFor(d => d.DocumentDescription)
            .Matches(RegularExpressions.EnglishTitleDocument)
            .WithMessage("It must contain only English words and spaces.");

        RuleFor(d => d.NumberPages)
            .GreaterThan(0)
            .WithMessage("The number of pages must not be less than zero or equal to zero.");
    }
}