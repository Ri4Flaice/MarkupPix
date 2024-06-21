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
/// Pages creation test.
/// </summary>
public class CreatePageTests
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

    private IEnumerable<IFormFile> _pages;
    private AppDbContext _dbContext;
    private CreatePage.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save document in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _pages = MockHelper.MockFormFilePages();
        _handler = new CreatePage.Handler(
            _dbContext,
            MockHelper.CreateMapper());

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
    /// Negative pages creation test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task CreatePageNegativeTest()
    {
        // Arrange
        var request = new CreatePageRequest
        {
            DocumentName = "Document",
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new CreatePage.Command(request, _pages), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive pages creation test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task CreatePagePositiveTest()
    {
        // Arrange
        var request = new CreatePageRequest
        {
            DocumentName = "TestDocument",
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new CreatePage.Command(request, _pages), CancellationToken.None))
            .Should().NotThrowAsync();
    }
}