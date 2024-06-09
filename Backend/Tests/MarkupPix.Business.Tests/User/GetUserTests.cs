using FluentAssertions;

using MarkupPix.Business.Feature.User;
using MarkupPix.Core.Enums;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Tests.Common;

namespace MarkupPix.Business.Tests.User;

/// <summary>
/// Tests for getting user data.
/// </summary>
public class GetUserTests
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
    private GetUser.Handler _handler;

    /// <summary>
    /// Test settings.
    /// </summary>
    /// <returns>Save user in database.</returns>
    [SetUp]
    public async Task Setup()
    {
        _dbContext = MockHelper.CreateDbContextInMemory();
        _handler = new GetUser.Handler(
            _dbContext,
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
    /// Negative test for getting user data.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task NegativeGetUserTest()
    {
        // Assert
        await _handler
            .Awaiting(h => h.Handle(new GetUser.Command("test@gmail.com"), CancellationToken.None))
            .Should().ThrowAsync<Exception>();
    }

    /// <summary>
    /// Positive test for getting user data.
    /// </summary>
    /// <returns>Checking result test.</returns>
    [Test]
    public async Task PositiveGetUserTest()
    {
        // Act
        var result = await _handler.Handle(
                new GetUser.Command(
                    _existingUser.EmailAddress
                    ?? throw new Exception("User's email address empty.")),
                CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.UserId, Is.EqualTo(_existingUser.Id));
            Assert.That(result.EmailAddress, Is.EqualTo(_existingUser.EmailAddress));
            Assert.That(result.Block, Is.EqualTo(_existingUser.Block));
            Assert.That(result.BirthDate, Is.EqualTo(_existingUser.BirthDate));
            Assert.That(result.AccountType, Is.EqualTo(_existingUser.AccountType));
        });
    }
}