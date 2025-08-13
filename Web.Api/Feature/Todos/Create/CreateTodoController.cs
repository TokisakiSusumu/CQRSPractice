using Application.Abstractions.Messaging;
using Application.Todos.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.Create;
[Route("yardify/todos")]
[Authorize]
public class CreateTodoController(ISender Sender) : OperationController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoCommand createTodoCommand)
    {
        var result = await Sender.SendAsync(createTodoCommand);
        return HandleResult(result);
    }
}
