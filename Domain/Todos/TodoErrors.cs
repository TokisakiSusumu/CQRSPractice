using SharedKernel;

namespace Domain.Todos;

public static class TodoErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Todo.NotFound", $"Todo with ID {id} was not found");

    public static Error AlreadyCompleted() =>
        Error.Conflict("Todo.AlreadyCompleted", "Todo is already completed");
}