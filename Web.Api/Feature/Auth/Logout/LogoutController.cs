using Application.Abstractions.Messaging;
using Application.Users.Logout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Auth.Logout;
[Route("yardify/auth/logout")]
[Authorize]
public class LogoutController(ISender Sender) : OperationController
{
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var result = await Sender.SendAsync(new LogoutCommand());
        return HandleResult(result);
    }
}