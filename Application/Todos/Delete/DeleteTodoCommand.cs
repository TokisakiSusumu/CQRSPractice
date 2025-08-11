using Application.Abstractions.Messaging;

namespace Application.Todos.Delete;

public sealed record DeleteTodoCommand(Guid Id) : ICommand;