using Application.Abstractions.Messaging;
using Application.Todos.Complete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Todos.Complete;

[Route("yardify/todos/{id}/complete")]
[Authorize]
public class CompleteTodoController(ISender Sender) : OperationController
{
    [HttpPost]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await Sender.SendAsync(new CompleteTodoCommand(id));
        return HandleResult(result);
    }
}