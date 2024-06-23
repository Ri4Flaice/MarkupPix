using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

using MarkupPix.Business.Feature.User;
using MarkupPix.Core.Enums;
using MarkupPix.Server.ApiClient.Models.User;

namespace MarkupPix.Business.Tests.User;

/// <summary>
/// User's validator tests.
/// </summary>
[TestFixture]
public class UserValidatorTests
{
    private IValidator<CreateUserRequest> _userValidator;
    private IValidator<UpdateUserRequest> _updateUserValidator;

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidCreateUserData()
    {
        yield return new object[] { new CreateUserRequest { EmailAddress = "string", Password = "string", Block = false, BirthDate = DateTime.Now, AccountType = AccountType.Markup } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fakegmail.com", Password = "admin@323", Block = true, BirthDate = DateTime.Today.AddYears(-200), AccountType = AccountType.Admin } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fake@gmailcom", Password = "FileManager111", Block = true, BirthDate = DateTime.Today.AddYears(20), AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidCreateUserData()
    {
        yield return new object[] { new CreateUserRequest { EmailAddress = "admin@gmail.com", Password = "Admin@12300", Block = false, BirthDate = new DateTime(2000, 04, 15), AccountType = AccountType.Admin } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "markup@gmail.com", Password = "Markup%3@23", Block = true, BirthDate = new DateTime(1995, 12, 04), AccountType = AccountType.Markup } };
        yield return new object[] { new CreateUserRequest { EmailAddress = "fileManager@mail.ru", Password = "fileManager!!9284", Block = false, BirthDate = new DateTime(2005, 06, 20), AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidUpdateUserData()
    {
        yield return new object[] { new UpdateUserRequest { EmailAddress = "string", Block = false, AccountType = AccountType.Markup } };
        yield return new object[] { new UpdateUserRequest { EmailAddress = "fakegmail.com", AccountType = AccountType.Admin } };
        yield return new object[] { new UpdateUserRequest { EmailAddress = "fake@gmailcom", Block = true, AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidUpdateUserData()
    {
        yield return new object[] { new UpdateUserRequest { EmailAddress = "admin@gmail.com", Block = false, AccountType = AccountType.Admin } };
        yield return new object[] { new UpdateUserRequest { EmailAddress = "markup@gmail.com", Block = true, AccountType = AccountType.Markup } };
        yield return new object[] { new UpdateUserRequest { EmailAddress = "fileManager@mail.ru", Block = false, AccountType = AccountType.FileManager } };
    }

    /// <summary>
    /// Test settings.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _userValidator = new UserValidator();
        _updateUserValidator = new UpdateUserValidator();
    }

    /// <summary>
    /// Checking for an incorrect user creation request.
    /// </summary>
    /// <param name="request">User creation request.</param>
    [TestCaseSource(nameof(InvalidCreateUserData))]
    public void InvalidCreateUserRequestTest(CreateUserRequest request)
    {
        // Act
        var result = _userValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
        result.ShouldHaveValidationErrorFor(r => r.Password);
        result.ShouldHaveValidationErrorFor(r => r.BirthDate);
    }

    /// <summary>
    /// Checking the correct user creation request.
    /// </summary>
    /// <param name="request">User creation request.</param>
    [TestCaseSource(nameof(ValidCreateUserData))]
    public void ValidCreateUserRequestTest(CreateUserRequest request)
    {
        // Act
        var result = _userValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Checking for an incorrect user update request.
    /// </summary>
    /// <param name="request">User update request.</param>
    [TestCaseSource(nameof(InvalidUpdateUserData))]
    public void InvalidUpdateUserRequestTest(UpdateUserRequest request)
    {
        // Act
        var result = _updateUserValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.EmailAddress);
    }

    /// <summary>
    /// Checking the correct user update request.
    /// </summary>
    /// <param name="request">User update request.</param>
    [TestCaseSource(nameof(ValidUpdateUserData))]
    public void ValidUpdateUserRequestTest(UpdateUserRequest request)
    {
        // Act
        var result = _updateUserValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}