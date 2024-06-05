using FluentValidation;

using MarkupPix.Core;
using MarkupPix.Server.ApiClient.Models.User;

namespace MarkupPix.Business.Feature.User;

/// <summary>
/// User's validator.
/// </summary>
public class UserValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="UserValidator"/>.
    /// </summary>
    public UserValidator()
    {
        RuleFor(d => d.EmailAddress)
            .NotEmpty()
            .Matches(RegularExpressions.Email)
            .WithMessage("The email is incorrect");

        RuleFor(d => d.Password)
            .NotEmpty()
            .Matches(RegularExpressions.Password)
            .WithMessage("The password is incorrect");

        RuleFor(d => d.BirthDate)
            .NotEmpty()
            .GreaterThan(new DateTime(1930, 01, 01))
            .WithMessage("Too old");

        RuleFor(d => d.BirthDate)
            .NotEmpty()
            .LessThan(DateTime.Today.AddYears(-18))
            .When(d => d.BirthDate != default)
            .WithMessage("Too young");
    }
}