namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }
    string Email { get; }
    List<string> Roles { get; }
    bool IsAuthenticated { get; }
}