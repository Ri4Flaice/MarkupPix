using MarkupPix.Business.Feature.User;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests.User;

/// <summary>
/// Testing the receipt of all users.
/// </summary>
public class GetAllUsersTests
{
    private AppDbContext _dbContext;
    private GetAllUsers.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save users in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _handler = new GetAllUsers.Handler(
            _dbContext,
            MockHelper.CreateMapper());

        _dbContext.UsersEntities.AddRange(ExistingUsers());
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
    /// Testing the receipt of all users.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task GetAllUsersTest()
    {
        // Act
        var result = await _handler.Handle(new GetAllUsers.Command(), CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    /// <summary>
    /// Existing users in the test database.
    /// </summary>
    /// <returns>Data test.</returns>
    private static List<UserEntity> ExistingUsers()
    {
        var users = new List<UserEntity>
        {
            new() { EmailAddress = "admin@gmail.com", Block = false, BirthDate = new DateTime(2000, 04, 15), AccountType = AccountType.Admin },
            new() { EmailAddress = "markup@gmail.com", Block = true, BirthDate = new DateTime(1995, 12, 04), AccountType = AccountType.Markup },
            new() { EmailAddress = "fileManager@mail.ru", Block = false, BirthDate = new DateTime(2005, 06, 20), AccountType = AccountType.FileManager },
        };
        return users;
    }
}