using BlazorApp.Client.Pages;
using BlazorApp.Components;
using BlazorApp.Components.Account;
using BlazorApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        // Configure HttpClient with credentials
        builder.Services.AddScoped(sp =>
        {
            var client = new HttpClient(new HttpClientHandler
            {
                CookieContainer = new System.Net.CookieContainer(),
                UseCookies = true,
                UseDefaultCredentials = false
            })
            {
                BaseAddress = new Uri("https://localhost:7146/")
            };
            return client;
        });

        builder.Services.AddScoped<ApiAuthService>();
        builder.Services.AddAuthorizationCore();

        // Add dummy ApplicationUser for EmailSender
        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.Run();
    }
}