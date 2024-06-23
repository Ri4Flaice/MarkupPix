using MarkupPix.Business.Infrastructure;
using MarkupPix.Data.Data;
using MarkupPix.Server.WebApi.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MarkupPix.Server.WebApi;

/// <summary>
/// Configuration at the start of the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// Initializes a new instance of the class <see cref="Startup"/>.
    /// </summary>
    /// <param name="configuration">App configuration.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// App configuration.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services">The collection of services to be configured.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Configuration.GetConnectionString("Redis");
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("MarkupPixDB")
                             ?? throw new Exception("Connection string 'MarkupPixDB' is missing or empty.")));

        services.AddSwaggerGen(c =>
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

            c.OperationFilter<FileUploadOperationFilter>();
        });

        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });

        services.AddBusinessServices(Configuration);
        services.AddWebApiServices();
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            serviceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
            serviceProvider.ConfigureDefaultDb();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}