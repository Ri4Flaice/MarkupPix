using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

using MarkupPix.Business.Feature.User;
using MarkupPix.Core.Enums;
using MarkupPix.Server.ApiClient.Models.User;

namespace MarkupPix.Business.Tests.User;

/// <summary>
/// User validator tests.
/// </summary>
[TestFixture]
public class UserValidatorTests
{
    private IValidator<CreateUserRequest> _validator;

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidData()
    {
        yield return new object[] { new CreateUserRequest { EmailAddress = "string", Password = "string", Block = false, BirthDate = DateTime.Now, AccountType = AccountType.Markup } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fakegmail.com", Password = "admin@323", Block = true, BirthDate = DateTime.Today.AddYears(-200), AccountType = AccountType.Admin } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fake@gmailcom", Password = "FileManager111", Block = true, BirthDate = DateTime.Today.AddYears(20), AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidData()
    {
        yield return new object[] { new CreateUserRequest { EmailAddress = "admin@gmail.com", Password = "Admin@12300", Block = false, BirthDate = new DateTime(2000, 04, 15), AccountType = AccountType.Admin } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "markup@gmail.com", Password = "Markup%3@23", Block = true, BirthDate = new DateTime(1995, 12, 04), AccountType = AccountType.Markup } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fileManager@mail.ru", Password = "fileManager!!9284", Block = false, BirthDate = new DateTime(2005, 06, 20), AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Test settings.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _validator = new UserValidator();
    }

    /// <summary>
    /// Checking for an incorrect user creation request.
    /// </summary>
    /// <param name="request">User creation request.</param>
    [TestCaseSource(nameof(InvalidData))]
    public void InvalidCreateUserRequestTest(CreateUserRequest request)
    {
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
        result.ShouldHaveValidationErrorFor(r => r.Password);
        result.ShouldHaveValidationErrorFor(r => r.BirthDate);
    }

    /// <summary>
    /// Checking the correct user creation request.
    /// </summary>
    /// <param name="request">User creation request.</param>
    [TestCaseSource(nameof(ValidData))]
    public void ValidCreateUserRequestTest(CreateUserRequest request)
    {
        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}