using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Login;

public sealed class LoginCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials());
        }

        // Sign in with password
        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            command.Password,
            isPersistent: false, // Remember me
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials());
        }

        // Get user roles
        var roles = await userManager.GetRolesAsync(user);

        return Result.Success(new LoginResponse(user.Id, user.Email!, roles.FirstOrDefault() ?? RoleNames.User));
    }
}