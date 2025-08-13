using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Logout;

public sealed class LogoutCommandHandler(IUserContext UserContext, SignInManager<User> SignInManager) : ICommandHandler<LogoutCommand>
{

    public async Task<Result> HandleAsync(LogoutCommand command, CancellationToken cancellationToken)
    {
        if (!UserContext.IsAuthenticated)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        await SignInManager.SignOutAsync();

        return Result.Success();
    }
}