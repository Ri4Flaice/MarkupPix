using FluentAssertions;

using MarkupPix.Business.Feature.Page;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;
using MarkupPix.Tests.Common;

using Microsoft.AspNetCore.Http;

namespace MarkupPix.Business.Tests.Page;

/// <summary>
/// Page update tests.
/// </summary>
public class UpdatePageTests
{
    /// <summary>
    /// An existing document in the database.
    /// </summary>
    private readonly DocumentEntity _existingDocument = new()
    {
        Id = 1,
        DocumentName = "TestDocument",
        DocumentDescription = "Test document",
        UserId = 1,
        NumberPages = 2,
        File = [],
    };

    /// <summary>
    /// An existing user in the database.
    /// </summary>
    private readonly UserEntity _existingUser = new()
    {
        Id = 1,
        EmailAddress = "admin@gmail.com",
        Block = false,
        BirthDate = new DateTime(1999, 02, 11),
        AccountType = AccountType.Admin,
    };

    /// <summary>
    /// An existing page in the database.
    /// </summary>
    private readonly PageEntity _existingPage = new()
    {
        Id = 1,
        DocumentId = 1,
        NumberPage = 1,
        IsRecognize = null,
        DateRecognize = null,
        RecognizeUser = null,
        Page = [],
    };

    private IFormFile _page;
    private AppDbContext _dbContext;
    private UpdatePage.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save document in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _page = MockHelper.MockFormFilePage();
        _handler = new UpdatePage.Handler(
            _dbContext);

        _dbContext.UsersEntities.Add(_existingUser);
        _dbContext.DocumentsEntities.Add(_existingDocument);
        _dbContext.PageEntities.Add(_existingPage);

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Clearing database after tests.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    /// <summary>
    /// Negative update page test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task UpdatePageNegativeTest()
    {
        // Arrange
        var request = new UpdatePageRequest
        {
            DocumentName = "Document",
            NumberPage = 1,
            UserEmailAddress = "admin@gmail.com",
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new UpdatePage.Command(request, _page), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive update page test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task UpdatePagePositiveTest()
    {
        // Arrange
        var request = new UpdatePageRequest
        {
            DocumentName = "TestDocument",
            NumberPage = 1,
            UserEmailAddress = "admin@gmail.com",
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new UpdatePage.Command(request, _page), CancellationToken.None))
            .Should().NotThrowAsync();
    }
}