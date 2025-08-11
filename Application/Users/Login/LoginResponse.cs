namespace Application.Users.Login;

public sealed record LoginResponse(Guid UserId, string Email, string Role);