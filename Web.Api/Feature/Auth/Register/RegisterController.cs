using Application.Abstractions.Messaging;
using Application.Users.Register;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Auth.Register;

[Route("yardify/auth/register")]
public class RegisterController(ISender Sender) : OperationController
{

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await Sender.SendAsync(command);
        return HandleResult(result);
    }
}