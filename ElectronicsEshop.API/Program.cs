using ElectronicsEshop.API.Extensions;
using ElectronicsEshop.API.Middlewares;
using ElectronicsEshop.Application.Extensions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Infrastructure.Extensions;
using ElectronicsEshop.Infrastructure.Seeders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();


builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<UserLogEnricherMiddleware>();

var app = builder.Build();

Log.Information("Application starting up.");

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDefaultDataSeeder>();
    await seeder.SeedData();
}

app.UseMiddleware<UserLogEnricherMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElectronicsEshop.API v1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/identity").MapIdentityApi<ApplicationUser>();
app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
