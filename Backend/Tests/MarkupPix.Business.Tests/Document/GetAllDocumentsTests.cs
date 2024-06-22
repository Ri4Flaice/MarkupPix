using FluentAssertions;

using MarkupPix.Business.Feature.Document;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests.Document;

/// <summary>
/// Get all documents tests.
/// </summary>
public class GetAllDocumentsTests
{
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

    private AppDbContext _dbContext;
    private GetAllDocuments.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save users in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _handler = new GetAllDocuments.Handler(
            _dbContext);

        _dbContext.UsersEntities.Add(_existingUser);
        _dbContext.DocumentsEntities.AddRange(ExistingDocuments());
        _dbContext.PageEntities.AddRange(ExistingPages());

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
    /// Positive get all documents test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task GetAllDocumentsPositiveTest()
    {
        await _handler
            .Awaiting(h => h.Handle(new GetAllDocuments.Command(), CancellationToken.None))
            .Should().NotThrowAsync();
    }

    /// <summary>
    /// Existing documents in the test database.
    /// </summary>
    /// <returns>Data test.</returns>
    private static List<DocumentEntity> ExistingDocuments()
    {
        var documents = new List<DocumentEntity>
        {
            new() { DocumentName = "test", DocumentDescription = "test document", NumberPages = 1, UserId = 1, File = [] },
            new() { DocumentName = "report", DocumentDescription = "report document", NumberPages = 1, UserId = 1, File = [] },
        };

        return documents;
    }

    /// <summary>
    /// Existing pages in the test database.
    /// </summary>
    /// <returns>Data test.</returns>
    private static List<PageEntity> ExistingPages()
    {
        var pages = new List<PageEntity>
        {
            new() { DocumentId = 1, NumberPage = 1, IsRecognize = true, RecognizeUser = 1, DateRecognize = DateTime.Now, Page = [] },
            new() { DocumentId = 2, NumberPage = 1, IsRecognize = false, RecognizeUser = null, DateRecognize = null, Page = [] },
        };

        return pages;
    }
}