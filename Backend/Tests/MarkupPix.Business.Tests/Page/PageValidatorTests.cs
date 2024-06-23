using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

using MarkupPix.Business.Feature.Page;
using MarkupPix.Server.ApiClient.Models.Document;

namespace MarkupPix.Business.Tests.Page;

/// <summary>
/// Page's validator tests.
/// </summary>
[TestFixture]
public class PageValidatorTests
{
    private IValidator<CreatePageRequest> _pageValidator;
    private IValidator<UpdatePageRequest> _updatePageValidator;

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidCreatePageData()
    {
        yield return new object[] { new CreatePageRequest { DocumentName = "doc-test" } };
        yield return new object[] { new CreatePageRequest { DocumentName = "doc?? test" } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidCreatePageData()
    {
        yield return new object[] { new CreatePageRequest { DocumentName = "TestDocument.docx" } };
        yield return new object[] { new CreatePageRequest { DocumentName = "document" } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> InvalidUpdatePageData()
    {
        yield return new object[] { new UpdatePageRequest { DocumentName = "doc-test" } };
        yield return new object[] { new UpdatePageRequest { DocumentName = "doc?? test" } };
    }

    /// <summary>
    /// Data source for test.
    /// </summary>
    /// <returns>Data test.</returns>
    public static IEnumerable<object> ValidUpdatePageData()
    {
        yield return new object[] { new UpdatePageRequest { DocumentName = "TestDocument.docx" } };
        yield return new object[] { new UpdatePageRequest { DocumentName = "document" } };
    }

    /// <summary>
    /// Test settings.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _pageValidator = new PageValidator();
        _updatePageValidator = new UpdatePageValidator();
    }

    /// <summary>
    /// Checking for an incorrect page creation request.
    /// </summary>
    /// <param name="request">Page creation request.</param>
    [TestCaseSource(nameof(InvalidCreatePageData))]
    public void InvalidCreatePageRequestTest(CreatePageRequest request)
    {
        // Act
        var result = _pageValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.DocumentName);
    }

    /// <summary>
    /// Checking the correct page creation request.
    /// </summary>
    /// <param name="request">Page creation request.</param>
    [TestCaseSource(nameof(ValidCreatePageData))]
    public void ValidCreatePageRequestTest(CreatePageRequest request)
    {
        // Act
        var result = _pageValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    /// <summary>
    /// Checking for an incorrect page updating request.
    /// </summary>
    /// <param name="request">Page updating request.</param>
    [TestCaseSource(nameof(InvalidUpdatePageData))]
    public void InvalidCreateUserRequestTest(UpdatePageRequest request)
    {
        // Act
        var result = _updatePageValidator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(r => r.DocumentName);
    }

    /// <summary>
    /// Checking the correct page updating request.
    /// </summary>
    /// <param name="request">Page updating request.</param>
    [TestCaseSource(nameof(ValidUpdatePageData))]
    public void ValidCreateUserRequestTest(UpdatePageRequest request)
    {
        // Act
        var result = _updatePageValidator.TestValidate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}