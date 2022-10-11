using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quartz;
using OpeniddictServer.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using App.Application;
using App.Domain.Entities;
using App.Infrastructure;
using App.Infrastructure.Identity;
using App.Infrastructure.Persistence;
using IdentityService;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace IdentityService;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {


        //For Encryption Key
        //using var algorithm = RSA.Create(keySizeInBits: 2048);

        //var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
        //var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        //request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

        //var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        //File.WriteAllBytes("signing-certificate.pfx", certificate.Export(X509ContentType.Pfx, string.Empty));


        //For Encryption Certificate
        //using var algorithm = RSA.Create(keySizeInBits: 2048);

        //var subject = new X500DistinguishedName("CN=CryptoBot");
        //var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        //request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));

        //var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(5));

        //File.WriteAllBytes("encryption-certificate.pfx", certificate.Export(X509ContentType.Pfx, "Soft2020+Capital"));


       // var certificate = new X509Certificate2("encryption-certificate.pfx", "Soft2020+Capital");
        
       // services.AddApplicationServices();
        //services.AddInfrastructureServices(Configuration);

        services.AddControllers();
        // services.AddRazorPages();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Configure the context to use Microsoft SQL Server.
            options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need
            // to replace the default OpenIddict entities.
            options.UseOpenIddict();
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(2);
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
        });


        services.Configure<IdentityOptions>(options =>
        {
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
            options.ClaimsIdentity.EmailClaimType = Claims.Email;

            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
        });

        // OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
        // (like pruning orphaned authorizations/tokens from the database) at regular intervals.
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        //services.AddCors(options =>
        //{
        //    options.AddPolicy("Default",
        //        builder =>
        //        {
        //            builder
        //                .AllowCredentials()
        //                .WithOrigins(
        //                    new[]
        //                    {
        //                        "https://localhost:5001",
        //                        "http://bot.capitalsoft.am",
        //                        "https://bot.capitalsoft.am"
        //                    })
        //                .SetIsOriginAllowedToAllowWildcardSubdomains()
        //                .AllowAnyHeader()
        //                .AllowAnyMethod();
        //        });
        //});

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        //.AddOpenIdConnect();
        //.AddOpenIdConnect("KeyCloak", "KeyCloak", options =>
        //{
        //    options.SignInScheme = "Identity.External";
        //    //Keycloak server
        //    options.Authority = Configuration.GetSection("Keycloak")["ServerRealm"];
        //    //Keycloak client ID
        //    options.ClientId = Configuration.GetSection("Keycloak")["ClientId"];
        //    //Keycloak client secret in user secrets for dev
        //    options.ClientSecret = Configuration.GetSection("Keycloak")["ClientSecret"];
        //    //Keycloak .wellknown config origin to fetch config
        //    options.MetadataAddress = Configuration.GetSection("Keycloak")["Metadata"];
        //    //Require keycloak to use SSL

        //    options.GetClaimsFromUserInfoEndpoint = true;
        //    options.Scope.Add("openid");
        //    options.Scope.Add("profile");
        //    options.SaveTokens = true;
        //    options.ResponseType = OpenIdConnectResponseType.Code;
        //    options.RequireHttpsMetadata = false; //dev

        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        NameClaimType = "name",
        //        RoleClaimType = ClaimTypes.Role,
        //        ValidateIssuer = true
        //    };
        //});

        services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();

                // Enable Quartz.NET integration.
                options.UseQuartz();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the authorization, logout, token and userinfo endpoints.
                options.SetAuthorizationEndpointUris("/connect/authorize")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo")
                    .SetVerificationEndpointUris("/connect/verify");

                options.RequireProofKeyForCodeExchange();
                // options.Configure(configureOptions =>
                //     configureOptions.CodeChallengeMethods.Add(CodeChallengeMethods.Plain));

                // Note: this sample uses the code, device code, password and refresh token flows, but you
                // can enable the other flows if you need to support implicit or client credentials.
                options
                    .AllowAuthorizationCodeFlow()
                    .AllowHybridFlow()
                    .AllowRefreshTokenFlow();

                // Mark the "email", "profile", "roles" and "dataEventRecords" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "dataEventRecords");
                
                // options  .AddEncryptionKey(new SymmetricSecurityKey(
                //              Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")))
                //          .AddEphemeralSigningKey();
                
                options.AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey();
                
                //TODO: UNCOMMENT ON PRODUCTION
                // options
                //     .AddEncryptionCertificate(certificate)
                //     .AddEncryptionKey(new SymmetricSecurityKey(
                //         Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")))
                //     .AddEphemeralSigningKey();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableStatusCodePagesIntegration();
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        // Register the worker responsible of seeding the database.
        // Note: in a real world application, this step should be part of a setup script.
        services.AddHostedService<Worker>();

        services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        IdentityModelEventSource.ShowPII = true;

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseStatusCodePagesWithReExecute("~/error");
            //app.UseExceptionHandler("~/error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        //app.UseCors("Default");

        app.UseCors(builder =>
        {
            builder
                .WithOrigins(new[]
                {
                    "https://localhost:5001",
                    "https://bot.capitalsoft.am"
                })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
            //endpoints.MapRazorPages();
            //endpoints.MapFallbackToFile("index.html");
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseAngularCliServer(npmScript: "start");
                //spa.UseProxyToSpaDevelopmentServer(Configuration["SpaBaseUrl"] ?? "http://localhost:4200");
            }
        });
    }
}