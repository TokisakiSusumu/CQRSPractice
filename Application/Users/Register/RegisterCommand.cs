using Application.Abstractions.Messaging;

namespace Application.Users.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<RegisterResponse>;