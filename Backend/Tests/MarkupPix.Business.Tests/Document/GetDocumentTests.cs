using FluentAssertions;

using MarkupPix.Business.Feature.Document;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests.Document;

/// <summary>
/// Get document tests.
/// </summary>
public class GetDocumentTests
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
        NumberPages = 12,
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

    private AppDbContext _dbContext;
    private GetDocument.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save document in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _handler = new GetDocument.Handler(
            _dbContext,
            MockHelper.CreateLogger<GetDocument.Handler>());

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
    /// Negative get document test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task GetDocumentNegativeTest()
    {
        await _handler
            .Awaiting(h => h.Handle(new GetDocument.Command("Document"), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive get document test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task GetDocumentPositiveTest()
    {
        // Assert
        await _handler
            .Awaiting(h => h.Handle(new GetDocument.Command(_existingDocument.DocumentName ?? string.Empty), CancellationToken.None))
            .Should().NotThrowAsync();
    }
}