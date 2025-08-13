using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Data;
using Domain.Todos;
using Domain.Users;
using SharedKernel;

namespace Application.Todos.Create;

public sealed class CreateTodoCommandHandler(IUserContext userContext, ApplicationDbContext context) : ICommandHandler<CreateTodoCommand, CreateTodoResponse>
{
    public async Task<Result<CreateTodoResponse>> HandleAsync(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            return Result.Failure<CreateTodoResponse>(UserErrors.Unauthorized());
        }

        var todo = new TodoItem
        {
            UserId = userContext.UserId,
            Title = command.Title,
            Description = command.Description,
            DueDate = command.DueDate,
            Priority = command.Priority,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        context.TodoItems.Add(todo);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateTodoResponse(todo.Id));
    }
}