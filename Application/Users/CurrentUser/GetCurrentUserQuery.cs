using Application.Abstractions.Messaging;

namespace Application.Users.CurrentUser;

public sealed record GetCurrentUserQuery() : IQuery<CurrentUserResponse>;