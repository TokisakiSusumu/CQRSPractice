using Application.Abstractions.Messaging;
using Domain.Todos;

namespace Application.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid Id,
    string Title,
    string Description,
    DateTime? DueDate,
    Priority Priority) : ICommand;