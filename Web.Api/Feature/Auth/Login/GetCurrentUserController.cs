using Application.Abstractions.Messaging;
using Application.Users.CurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Feature.Auth.Login;


[Route("yardify/auth/me")]
[Authorize]
public class GetCurrentUserController(ISender Sender) : OperationController
{
    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await Sender.SendAsync(new GetCurrentUserQuery());
        return HandleResult(result);
    }
}