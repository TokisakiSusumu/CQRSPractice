using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Components.Account;

public class ApiAuthService(HttpClient _httpClient, AuthenticationStateProvider _authStateProvider)
{

    //public async Task<(bool Success, string? Error)> LoginAsync(string email, string password)
    //{
    //    var response = await _httpClient.PostAsJsonAsync("yardify/auth/login", new { email, password });

    //    if (response.IsSuccessStatusCode)
    //    {
    //        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
    //        return (true, null);
    //    }

    //    var error = await response.Content.ReadAsStringAsync();
    //    return (false, error);
    //}

    public async Task<(bool Success, string? Error)> RegisterAsync(string email, string password, string firstName, string lastName)
    {
        var response = await _httpClient.PostAsJsonAsync("yardify/auth/register",
            new { email, password, firstName, lastName });

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    public async Task<(bool Success, string? Error)> LoginAsync(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("yardify/auth/login", new { email, password });

        if (response.IsSuccessStatusCode)
        {
            ((ApiAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(email);
            return (true, null);
        }

        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("yardify/auth/logout", null);
        ((ApiAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("yardify/auth/me");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfo>();
            }
        }
        catch { }
        return null;
    }

    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}