using Application.Abstractions.Messaging;
using Application.Todos.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.Update;

[Route("yardify/todos/{id}")]
[Authorize]
public class UpdateTodoController(ISender Sender) : OperationController
{
    [HttpPut]
    public async Task<IActionResult> Update(Guid id, UpdateTodoCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await Sender.SendAsync(command);
        return HandleResult(result);
    }
}