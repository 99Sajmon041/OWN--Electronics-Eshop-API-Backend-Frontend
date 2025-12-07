using Blazored.LocalStorage;
using ElectronicsEshop.Blazor;
using ElectronicsEshop.Blazor.Services.ApplicationUsers;
using ElectronicsEshop.Blazor.Services.Auth;
using ElectronicsEshop.Blazor.Services.Categories;
using ElectronicsEshop.Blazor.Services.Products;
using ElectronicsEshop.Blazor.Services.Roles;
using ElectronicsEshop.Blazor.UI.Message;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductAdminService, ProductAdminService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IApplicationUsersService, ApplicationUsersService>();
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7259")
});

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
 
await builder.Build().RunAsync();
