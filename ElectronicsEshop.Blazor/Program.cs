using Blazored.LocalStorage;
using ElectronicsEshop.Blazor;
using ElectronicsEshop.Blazor.Auth;
using ElectronicsEshop.Blazor.UI.Message;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped <MessageService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7259")
});

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
 
await builder.Build().RunAsync();
