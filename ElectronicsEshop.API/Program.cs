using ElectronicsEshop.API.Extensions;
using ElectronicsEshop.Application.Extensions;
using ElectronicsEshop.Domain.Entities;
using ElectronicsEshop.Infrastructure.Extensions;
using ElectronicsEshop.Infrastructure.Seeders;
    
var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
builder.Configuration.AddUserSecrets<Program>();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDefaultDataSeeder>();
    await seeder.SeedData();
}
    
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

app.Run();
