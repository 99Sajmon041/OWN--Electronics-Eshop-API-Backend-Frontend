using ElectronicsEshop.API.Extensions;
using ElectronicsEshop.API.Middlewares;
using ElectronicsEshop.Application.Abstractions;
using ElectronicsEshop.Application.Extensions;
using ElectronicsEshop.Application.Products.Mapping;
using ElectronicsEshop.Infrastructure.Email;
using ElectronicsEshop.Infrastructure.Extensions;
using ElectronicsEshop.Infrastructure.Seeders;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly); // pro všechny mappery v této Assebly, které dědí z Profile

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, IEmailService>();

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

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
