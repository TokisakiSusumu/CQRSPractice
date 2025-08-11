using Application.Abstractions.Messaging;
using Application.Todos.GetById;

namespace Application.Todos.GetAll;

public sealed record GetAllTodosQuery() : IQuery<List<TodoResponse>>;