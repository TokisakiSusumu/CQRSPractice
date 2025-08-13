using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Data;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.Complete;

public sealed class CompleteTodoCommandHandler(IUserContext userContext, ApplicationDbContext context) : ICommandHandler<CompleteTodoCommand>
{
    public async Task<Result> HandleAsync(CompleteTodoCommand command, CancellationToken cancellationToken)
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

        if (todo.IsCompleted)
        {
            return Result.Failure(TodoErrors.AlreadyCompleted());
        }

        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}