using FluentAssertions;

using MarkupPix.Business.Feature.User;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.ApiClient.Models.User;

using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests.User;

/// <summary>
/// User creation tests.
/// </summary>
public class CreateUserTests
{
    /// <summary>
    /// An existing user in the database.
    /// </summary>
    private readonly UserEntity _existingUser = new()
    {
        EmailAddress = "nazarbaev@gmail.com",
        Block = false,
        BirthDate = new DateTime(1940, 06, 07),
        AccountType = AccountType.Admin,
    };

    private AppDbContext _dbContext;
    private CreateUser.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save user in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _handler = new CreateUser.Handler(
            _dbContext,
            MockHelper.MockUserManager(new List<UserEntity>()),
            MockHelper.CreateMapper(),
            MockHelper.MockDistributedCache());

        _dbContext.UsersEntities.Add(_existingUser);
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
    /// Negative user creation test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task CreateUserNegativeTest()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            EmailAddress = "nazarbaev@gmail.com",
            Password = "Nazarbaev$$19400607",
            Block = false,
            BirthDate = new DateTime(1940, 06, 07),
            AccountType = AccountType.Admin,
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new CreateUser.Command(request), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive user creation test.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task CreateUserPositiveTest()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            EmailAddress = "tokaev@gmail.com",
            Password = "Tokaev%%19230210",
            Block = false,
            BirthDate = new DateTime(1923, 02, 10),
            AccountType = AccountType.Admin,
        };

        // Assert
        await _handler
            .Awaiting(h => h.Handle(new CreateUser.Command(request), CancellationToken.None))
            .Should().NotThrowAsync();
    }
}