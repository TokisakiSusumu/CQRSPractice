using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Todos.GetById;
using Domain.Data;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.Update;

public sealed class UpdateTodoCommandHandler(IUserContext userContext, ApplicationDbContext context) : ICommandHandler<UpdateTodoCommand>
{
    public async Task<Result> HandleAsync(UpdateTodoCommand command, CancellationToken cancellationToken)
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

        // Check ownership
        if (todo.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        // Update fields
        todo.Title = command.Title;
        todo.Description = command.Description;
        todo.DueDate = command.DueDate;
        todo.Priority = command.Priority;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}