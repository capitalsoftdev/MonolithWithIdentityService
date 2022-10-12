using App.Application.Common.Interfaces;
using App.Infrastructure.Files;
using App.Infrastructure.Identity;
using App.Infrastructure.Persistence;
using App.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace App.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitectureDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                }

            );
            
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextSeed>();
        
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
        
        services.AddOpenIddict()
            .AddValidation(options =>
            {
                // Note: the validation handler uses OpenID Connect discovery
                // to retrieve the address of the introspection endpoint.
                options.SetIssuer("https://localhost:6001/");  //debug "https://localhost:6001/"
                options.AddAudiences("rs_dataEventRecordsApi");
                    
                    

                    
                // options.Configure(opt =>
                //
                //     opt.TokenValidationParameters.IssuerSigningKey =
                //             new SymmetricSecurityKey(Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY="))
                // );

                // options.AddEncryptionCertificate("18f443ee80337ad86d79b3968bc06c23fb0b9ac1");

                // Configure the validation handler to use introspection and register the client
                // credentials used when communicating with the remote introspection endpoint.
                options.UseIntrospection()
                    .SetClientId("rs_dataEventRecordsApi")
                    .SetClientSecret("dataEventRecordsSecret");

                //options.us

                // Register the System.Net.Http integration.
                options.UseSystemNetHttp();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
        
        services.AddScoped<IAuthorizationHandler, RequireScopeHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("dataEventRecordsPolicy", policyUser =>
            {
                policyUser.Requirements.Add(new RequireScope());
            });
        });


        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        return services;
    }
}
