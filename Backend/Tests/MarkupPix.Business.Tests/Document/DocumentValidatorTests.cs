using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

using MarkupPix.Business.Feature.Document;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Tests.Document;

/// <summary>
/// Document validator tests.
/// </summary>
[TestFixture]
public class DocumentValidatorTests
{
    private IValidator<CreateDocumentRequest> _documentValidator;
    private IValidator<UpdateDocumentRequest> _updateDocumentValidator;

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidCreateDocumentData()
    {
        yield return new object[] { new CreateDocumentRequest { UserEmailAddress = "fake@gmailcom", DocumentName = "!!string^_^", DocumentDescription = "desc^_^", NumberPages = -1 } };
        yield return new object[] { new CreateDocumentRequest { UserEmailAddress = "gmail", DocumentName = "!!string^_^", DocumentDescription = "desc^_^", NumberPages = 0 } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidCreateDocumentData()
    {
        yield return new object[] { new CreateDocumentRequest { UserEmailAddress = "admin@gmail.com", DocumentName = "Report", DocumentDescription = "Report from analytics 2024 year", NumberPages = 22 } };
        yield return new object[] { new CreateDocumentRequest { UserEmailAddress = "markup@mail.ru", DocumentName = "Report", DocumentDescription = "Plan on 2024 year", NumberPages = 1 } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidUpdateDocumentData()
    {
        yield return new object[] { new UpdateDocumentRequest { UserEmailAddress = "fake@gmailcom", DocumentName = "!!string^_^", DocumentDescription = "desc^_^", NumberPages = -1 } };
        yield return new object[] { new UpdateDocumentRequest { UserEmailAddress = "gmail", DocumentName = "!!string^_^", DocumentDescription = "desc^_^", NumberPages = 0 } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidUpdateDocumentData()
    {
        yield return new object[] { new UpdateDocumentRequest { UserEmailAddress = "admin@gmail.com", DocumentName = "Report", DocumentDescription = "Report from analytics 2024 year", NumberPages = 22 } };
        yield return new object[] { new UpdateDocumentRequest { UserEmailAddress = "markup@mail.ru", DocumentName = "Report", DocumentDescription = "Plan on 2024 year", NumberPages = 1 } };
    }

    /// <summary>
    /// Test settings.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _documentValidator = new DocumentValidator();
        _updateDocumentValidator = new UpdateDocumentValidator();
    }

    /// <summary>
    /// Checking for an incorrect document creation request.
    /// </summary>
    /// <param name="request">Document creation request.</param>
    [TestCaseSource(nameof(InvalidCreateDocumentData))]
    public void InvalidCreateDocumentRequestTest(CreateDocumentRequest request)
    {
        // Act
        var result = _documentValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.UserEmailAddress);
        result.ShouldHaveValidationErrorFor(r => r.DocumentName);
        result.ShouldHaveValidationErrorFor(r => r.DocumentDescription);
        result.ShouldHaveValidationErrorFor(r => r.NumberPages);
    }

    /// <summary>
    /// Checking the correct document creation request.
    /// </summary>
    /// <param name="request">Document creation request.</param>
    [TestCaseSource(nameof(ValidCreateDocumentData))]
    public void ValidCreateDocumentRequestTest(CreateDocumentRequest request)
    {
        // Act
        var result = _documentValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Checking for an incorrect document update request.
    /// </summary>
    /// <param name="request">Document update request.</param>
    [TestCaseSource(nameof(InvalidUpdateDocumentData))]
    public void InvalidUpdateUserRequestTest(UpdateDocumentRequest request)
    {
        // Act
        var result = _updateDocumentValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.UserEmailAddress);
        result.ShouldHaveValidationErrorFor(r => r.DocumentName);
        result.ShouldHaveValidationErrorFor(r => r.DocumentDescription);
        result.ShouldHaveValidationErrorFor(r => r.NumberPages);
    }

    /// <summary>
    /// Checking the correct document update request.
    /// </summary>
    /// <param name="request">Document update request.</param>
    [TestCaseSource(nameof(ValidUpdateDocumentData))]
    public void ValidUpdateUserRequestTest(UpdateDocumentRequest request)
    {
        // Act
        var result = _updateDocumentValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}