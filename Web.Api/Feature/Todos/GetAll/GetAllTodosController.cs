using Application.Abstractions.Messaging;
using Application.Todos.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.GetAll;

[Route("yardify/todos")]
[Authorize]
public class GetAllTodosController(ISender Sender) : OperationController
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Sender.SendAsync(new GetAllTodosQuery());
        return HandleResult(result);
    }
}
