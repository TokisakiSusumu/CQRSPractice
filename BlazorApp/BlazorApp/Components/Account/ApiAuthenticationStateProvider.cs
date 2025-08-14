using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorApp.Components.Account;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiAuthenticationStateProvider> _logger;

    public ApiAuthenticationStateProvider(HttpClient httpClient, ILogger<ApiAuthenticationStateProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("yardify/auth/me");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserInfo>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user.UserId),
                        new(ClaimTypes.Name, user.Email),
                        new(ClaimTypes.Email, user.Email),
                        new(ClaimTypes.Role, user.Role)
                    };

                    var identity = new ClaimsIdentity(claims, "apiauth");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
        }
        catch { }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication(string email)
    {
        var authState = GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }

    private class UserInfo
    {
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }
}