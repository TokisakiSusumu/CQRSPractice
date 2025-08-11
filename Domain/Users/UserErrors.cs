using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("User.NotFound", $"User with ID {userId} was not found");

    public static Error EmailAlreadyExists() =>
        Error.Conflict("User.EmailExists", "Email already exists");

    public static Error InvalidCredentials() =>
        Error.Validation("User.InvalidCredentials", "Invalid email or password");

    public static Error Unauthorized() =>
        Error.Failure("User.Unauthorized", "You don't have permission to perform this action");
}