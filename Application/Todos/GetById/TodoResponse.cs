using Domain.Todos;

namespace Application.Todos.GetById;

public sealed record TodoResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime? DueDate,
    bool IsCompleted,
    Priority Priority,
    DateTime CreatedAt,
    DateTime? CompletedAt);