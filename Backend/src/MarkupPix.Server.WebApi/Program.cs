using MarkupPix.Business.Infrastructure;
using MarkupPix.Data.Data;
using MarkupPix.Data.Entities;
using MarkupPix.Server.WebApi.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarkupPix.Server.WebApi;

/// <summary>
/// App startup.
/// </summary>
public static class Program
{
    /// <summary>
    /// The entry point to the program.
    /// </summary>
    /// <param name="args">Cmd parameters.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var env = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
        });

        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("MarkupPixDB")
                             ?? throw new Exception("Connection string 'MarkupPixDB' is missing or empty.")));

        builder.Services.AddBusinessServices(builder.Configuration);

        builder.Services.AddWebApiServices();

        builder.Services.AddIdentity<UserEntity, IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            serviceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
            serviceProvider.ConfigureDefaultDb();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}