using BlazorApp.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp.Client;
internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
        // In BlazorApp.Client/Program.cs

        // Add the custom DelegatingHandler
        builder.Services.AddTransient<CookieDelegatingHandler>();

        // Configure the HttpClient to use the handler
        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7146/"); // Your API's address
        })
        .AddHttpMessageHandler<CookieDelegatingHandler>();

        // This makes the named HttpClient available for injection
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));
        await builder.Build().RunAsync();
    }
}
