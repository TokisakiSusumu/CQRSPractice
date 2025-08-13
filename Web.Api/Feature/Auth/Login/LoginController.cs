using Application.Abstractions.Messaging;
using Application.Users.Login;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Auth.Login;

[Route("yardify/auth/login")]
public class LoginController(ISender Sender) : OperationController
{

    [HttpPost]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await Sender.SendAsync(command);
        return HandleResult(result);
    }
}