namespace Application.Users.CurrentUser;

public sealed record CurrentUserResponse(string UserId, string Email, string Role);