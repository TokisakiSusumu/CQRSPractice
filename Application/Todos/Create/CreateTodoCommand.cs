using Application.Abstractions.Messaging;
using Domain.Todos;

namespace Application.Todos.Create;

public sealed record CreateTodoCommand(
    string Title,
    string Description,
    DateTime? DueDate,
    Priority Priority) : ICommand<CreateTodoResponse>;