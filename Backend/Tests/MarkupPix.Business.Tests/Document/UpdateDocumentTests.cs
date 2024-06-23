using FluentAssertions;

using MarkupPix.Business.Feature.Document;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.Document;
using MarkupPix.Tests.Common;

using Microsoft.AspNetCore.Http;

namespace MarkupPix.Business.Tests.Document;

/// <summary>
/// Document update tests.
/// </summary>
public class UpdateDocumentTests
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

    private IFormFile _documentFile;
    private AppDbContext _dbContext;
    private UpdateDocument.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save document in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _documentFile = MockHelper.MockFormFile();
        _handler = new UpdateDocument.Handler(
            _dbContext);

        _dbContext.UsersEntities.Add(_existingUser);
        _dbContext.DocumentsEntities.Add(_existingDocument);
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
    /// Negative update document test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task UpdateDocumentNegativeTest()
    {
        // Arrange
        var request = new UpdateDocumentRequest
        {
            DocumentName = "Document",
            DocumentDescription = "Test document",
            NumberPages = 12,
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new UpdateDocument.Command(request, _documentFile, _existingUser), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive update document test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task UpdateDocumentPositiveTest()
    {
        // Arrange
        var request = new UpdateDocumentRequest
        {
            DocumentName = "TestDocument",
            DocumentDescription = "Report for the future business plan",
            NumberPages = 17,
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new UpdateDocument.Command(request, _documentFile, _existingUser), CancellationToken.None))
            .Should().NotThrowAsync();
    }
}