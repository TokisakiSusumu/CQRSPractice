using Application.Abstractions.Messaging;
using Application.Todos.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.Delete;
[Route("yardify/todos/{id}")]
[Authorize]
public class DeleteTodoController(ISender Sender) : OperationController
{
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Sender.SendAsync(new DeleteTodoCommand(id));
        return HandleResult(result);
    }
}