using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MarkupPix.Data.Data;

/// <summary>
/// Connecting to the database.
/// </summary>
public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the class <see cref="AppContext"/>.
    /// </summary>
    /// <param name="configuration">The configuration settings for the application.</param>
    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Configures the database context options.
    /// </summary>
    /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("MarkupPixDB") ??
            throw new InvalidOperationException("Connection string 'MarkupPixDB' is missing or empty."));
    }
}