using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.CurrentUser;

public sealed class GetCurrentUserQueryHandler(IUserContext userContext) : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    public async Task<Result<CurrentUserResponse>> HandleAsync(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
            return Result.Failure<CurrentUserResponse>(UserErrors.Unauthorized());

        var role = userContext.Roles.FirstOrDefault() ?? RoleNames.User;
        return Result.Success(new CurrentUserResponse(
            userContext.UserId.ToString(),
            userContext.Email,
            role));
    }
}