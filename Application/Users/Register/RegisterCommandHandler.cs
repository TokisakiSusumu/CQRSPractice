using Application.Abstractions.Messaging;
using Domain.Data;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application.Users.Register;
internal sealed class RegisterCommandHandler(UserManager<User> userManager, ApplicationDbContext context) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<Result<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // Check if email exists
        var existingUser = await userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
        {
            return Result.Failure<RegisterResponse>(UserErrors.EmailAlreadyExists());
        }

        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            UserName = command.Email, // ASP.NET Identity requires UserName
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        // Create user with password
        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<RegisterResponse>(Error.Validation("User.ValidationFailed", errors));
        }

        // Add to default role
        await userManager.AddToRoleAsync(user, RoleNames.User);

        return Result.Success(new RegisterResponse(user.Id, user.Email));
    }
}
