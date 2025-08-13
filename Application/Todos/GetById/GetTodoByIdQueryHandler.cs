using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Data;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.GetById;

public sealed class GetTodoByIdQueryHandler(IUserContext userContext, ApplicationDbContext context) : IQueryHandler<GetTodoByIdQuery, TodoResponse>
{
    public async Task<Result<TodoResponse>> HandleAsync(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            return Result.Failure<TodoResponse>(UserErrors.Unauthorized());
        }
        var todo = await context.TodoItems
            .AsNoTracking()
            .Where(t => t.Id == query.Id)
            .Select(t => new TodoResponse(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.IsCompleted,
                t.Priority,
                t.CreatedAt,
                t.CompletedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (todo is null)
        {
            return Result.Failure<TodoResponse>(TodoErrors.NotFound(query.Id));
        }

        // Check if user owns this todo
        var ownsTodo = await context.TodoItems
            .AnyAsync(t => t.Id == query.Id && t.UserId == userContext.UserId, cancellationToken);

        if (!ownsTodo && !userContext.Roles.Contains(RoleNames.Admin))
        {
            return Result.Failure<TodoResponse>(UserErrors.Unauthorized());
        }

        return Result.Success(todo);
    }
}