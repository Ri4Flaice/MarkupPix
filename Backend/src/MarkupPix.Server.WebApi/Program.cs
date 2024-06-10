using MarkupPix.Business.Infrastructure;
using MarkupPix.Data.Data;
using MarkupPix.Server.WebApi.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(builder.Configuration.GetConnectionString("MarkupPixDB")
                             ?? throw new Exception("Connection string 'MarkupPixDB' is missing or empty.")));

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter the token.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });

        builder.Services.AddBusinessServices(builder.Configuration);
        builder.Services.AddWebApiServices();

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