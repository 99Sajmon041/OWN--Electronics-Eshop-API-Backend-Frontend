using ElectronicsEshop.API.Extensions;
using ElectronicsEshop.API.Interfaces;
using ElectronicsEshop.API.Middlewares;
using ElectronicsEshop.API.Services;
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

const string BlazorClientOrigin = "BlazorClient";

builder.Services.AddCors(options =>
{
    options.AddPolicy(BlazorClientOrigin, policy =>
    {
        policy
        .WithOrigins("https://localhost:7060")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Host.UseSerilog();

builder.Services.AddAutoMapper(cfg => { }, typeof(ProductMappingProfile).Assembly); // pro všechny mappery v této Assebly, které dědí z Profile

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

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
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElectronicsEshop.API v1");
});

app.UseHttpsRedirection();
app.UseCors(BlazorClientOrigin);
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
