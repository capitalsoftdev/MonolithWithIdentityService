using App.Application.Common.Interfaces;
using App.Infrastructure;
using App.WebUI;
using App.Infrastructure.Persistence;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddApplicationServices();
services.AddInfrastructureServices(configuration);
services.AddWebUIServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var seed = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeed>();
        await seed.InitialiseAsync();
        await seed.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();


if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}


app.UseAuthentication();
app.UseAuthorization();

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

    if (app.Environment.IsDevelopment())
    {
        spa.UseAngularCliServer(npmScript: "start");
        //spa.UseProxyToSpaDevelopmentServer(Configuration["SpaBaseUrl"] ?? "http://localhost:4200");
    }
});

app.Run();