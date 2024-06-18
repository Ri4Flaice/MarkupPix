using AutoMapper;

using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

using Moq;

namespace MarkupPix.Tests.Common;

/// <summary>
/// Configuring the database for tests.
/// </summary>
public static class MockHelper
{
    /// <summary>
    /// Creating database context in memory.
    /// </summary>
    /// <returns>App database context.</returns>
    public static AppDbContext CreateDbContextInMemory()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase($"DB_{Guid.NewGuid()}");
        var appDbContext = new TestAppDbContext(optionsBuilder.Options);

        SeedDatabase(appDbContext);

        return appDbContext;
    }

    /// <summary>
    /// Create AutoMapper with configuration.
    /// </summary>
    /// <returns>AutoMapper with configuration.</returns>
    public static IMapper CreateMapper()
    {
        var mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<Business.Feature.User.AutoMapperProfile>();
            c.AddProfile<Business.Feature.Document.AutoMapperProfile>();
        });

        mapper.AssertConfigurationIsValid();
        return mapper.CreateMapper();
    }

    /// <summary>
    /// Creating mock for user manager.
    /// </summary>
    /// <param name="users">List users.</param>
    /// <returns>User manager object.</returns>
    public static UserManager<UserEntity> MockUserManager(List<UserEntity> users)
    {
        var userManagerMock = new Mock<UserManager<UserEntity>>(Mock.Of<IUserStore<UserEntity>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        userManagerMock.Object.UserValidators.Add(new UserValidator<UserEntity>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<UserEntity>());

        userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<UserEntity, string>((x, _) => users.Add(x));
        userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(IdentityResult.Success);

        return userManagerMock.Object;
    }

    /// <summary>
    /// Creating mock for cache.
    /// </summary>
    /// <returns>Cache object.</returns>
    public static IDistributedCache MockDistributedCache()
    {
        var mockDistributedCache = new Mock<IDistributedCache>();

        mockDistributedCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[])null!);

        mockDistributedCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        mockDistributedCache.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return mockDistributedCache.Object;
    }

    /// <summary>
    /// Creating mock for <see cref="IFormFile"/>.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <param name="content">File content.</param>
    /// <returns>File object.</returns>
    public static IFormFile MockFormFile(string fileName = "TestDocument.docx", string content = "File content in test document")
    {
        var fileMock = new Mock<IFormFile>();

        var documentStream = new MemoryStream();
        var writer = new StreamWriter(documentStream);

        writer.Write(content);
        writer.Flush();
        documentStream.Position = 0;

        fileMock.Setup(f => f.OpenReadStream()).Returns(documentStream);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(documentStream.Length);
        fileMock.Setup(f => f.ContentType).Returns("application/octet-stream");

        return fileMock.Object;
    }

    /// <summary>
    /// Filling in the database with values.
    /// </summary>
    /// <param name="appDbContext">App database context.</param>
    private static void SeedDatabase(TestAppDbContext appDbContext)
    {
    }
}