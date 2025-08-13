using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Todos.GetById;
using Domain.Data;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Todos.GetAll;

public sealed class GetAllTodosQueryHandler(IUserContext userContext, ApplicationDbContext context) : IQueryHandler<GetAllTodosQuery, List<TodoResponse>>
{
    public async Task<Result<List<TodoResponse>>> HandleAsync(GetAllTodosQuery query, CancellationToken cancellationToken)
    {
        if (!userContext.IsAuthenticated)
        {
            return Result.Failure<List<TodoResponse>>(UserErrors.Unauthorized());
        }
        // Admins see all todos, users see only their own
        var todosQuery = context.TodoItems.AsNoTracking();

        if (!userContext.Roles.Contains(RoleNames.Admin))
        {
            todosQuery = todosQuery.Where(t => t.UserId == userContext.UserId);
        }

        var todos = await todosQuery
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TodoResponse(
                t.Id,
                t.Title,
                t.Description,
                t.DueDate,
                t.IsCompleted,
                t.Priority,
                t.CreatedAt,
                t.CompletedAt))
            .ToListAsync(cancellationToken);

        return Result.Success(todos);
    }
}