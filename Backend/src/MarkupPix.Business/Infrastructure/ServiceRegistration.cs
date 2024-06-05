using FluentValidation;

using MediatR;
using MediatR.Pipeline;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarkupPix.Business.Infrastructure;

/// <summary>
/// An extension for registering business layer services.
/// </summary>
public static class ServiceRegistration
{
    /// <summary>
    /// Registration business services.
    /// </summary>
    /// <param name="services">App services collection.</param>
    /// <param name="configuration">App configuration.</param>
    /// <returns>Changed collection.</returns>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(BusinessLayer));
        services.AddValidatorsFromAssembly(typeof(BusinessLayer).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(BusinessLayer).Assembly)
                .AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        return services;
    }

    /// <summary>
    /// Configuration user roles.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public static void ConfigureDefaultDb(this IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        var existingRoles = roleManager.Roles.Select(t => t.Name).ToListAsync().GetAwaiter().GetResult();
        var rolesList = UserRoles.RolesList.Values;

        foreach (var role in rolesList.Where(l => !existingRoles.Contains(l)))
        {
            roleManager.CreateAsync(new IdentityRole<long>(role)).GetAwaiter().GetResult();
        }
    }
}