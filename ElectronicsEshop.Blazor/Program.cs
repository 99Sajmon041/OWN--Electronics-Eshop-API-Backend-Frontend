using Blazored.LocalStorage;
using ElectronicsEshop.Blazor;
using ElectronicsEshop.Blazor.Services.ApplicationUsers;
using ElectronicsEshop.Blazor.Services.Auth;
using ElectronicsEshop.Blazor.Services.Carts;
using ElectronicsEshop.Blazor.Services.Categories;
using ElectronicsEshop.Blazor.Services.Orders;
using ElectronicsEshop.Blazor.Services.Products;
using ElectronicsEshop.Blazor.Services.Roles;
using ElectronicsEshop.Blazor.UI.Message;
using ElectronicsEshop.Blazor.UI.State;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductAdminService, ProductAdminService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IApplicationUsersService, ApplicationUsersService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IOrderAdminService, OrderAdminService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IApplicationUsersAdminService, ApplicationUsersAdminService>();
builder.Services.AddScoped<ICartsAdminService, CartsAdminService>();
builder.Services.AddScoped<ICartsService, CartsService>();

builder.Services.AddScoped<TokenExpiryService>();
builder.Services.AddScoped<ClientSessionService>();
builder.Services.AddTransient<UnauthorizedHandler>();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7259");
})
.AddHttpMessageHandler<UnauthorizedHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
