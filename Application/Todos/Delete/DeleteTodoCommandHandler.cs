using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Todos.GetById;
using Domain.Data;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.Delete;

public sealed class DeleteTodoCommandHandler(IUserContext userContext, ApplicationDbContext context) : ICommandHandler<DeleteTodoCommand>
{
    public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }
        var todo = await context.TodoItems
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (todo is null)
        {
            return Result.Failure(TodoErrors.NotFound(command.Id));
        }

        if (todo.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        context.TodoItems.Remove(todo);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}