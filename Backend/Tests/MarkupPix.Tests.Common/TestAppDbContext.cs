using MarkupPix.Data.Data;

using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Tests.Common;

/// <summary>
/// Testing app database context.
/// </summary>
public class TestAppDbContext : AppDbContext
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="TestAppDbContext"/>.
    /// </summary>
    /// <param name="options">Context parameters.</param>
    public TestAppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}