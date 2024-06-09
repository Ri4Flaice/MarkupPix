using MarkupPix.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Data.Data;

/// <summary>
/// Connecting to the database.
/// </summary>
public class AppDbContext : IdentityDbContext<UserEntity, IdentityRole<long>, long>
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="AppContext"/>.
    /// </summary>
    /// <param name="options">Context parameters.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// User data.
    /// </summary>
    public DbSet<UserEntity> UsersEntities { get; set; }
}