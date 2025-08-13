using Application.Abstractions.Messaging;
using Application.Todos.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.GetById;
[Route("yardify/todos/{id}")]
[Authorize]
public class GetTodoByIdController(ISender Sender) : OperationController
{
    [HttpGet(Name = "GetTodoById")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Sender.SendAsync(new GetTodoByIdQuery(id));
        return HandleResult(result);
    }
}
