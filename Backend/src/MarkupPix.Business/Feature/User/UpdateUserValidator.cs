using FluentValidation;

using MarkupPix.Core;
using MarkupPix.Server.ApiClient.Models.User;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// User's validator.
/// </summary>
public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="UpdateUserValidator"/>.
    /// </summary>
    public UpdateUserValidator()
    {
        RuleFor(d => d.EmailAddress)
            .NotEmpty()
            .Matches(RegularExpressions.Email)
            .WithMessage("The email is incorrect");
    }
}