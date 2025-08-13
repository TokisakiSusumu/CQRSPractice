using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Feature;

[ApiController]
public abstract class OperationController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : HandleError(result.Error);

    protected IActionResult HandleResult(Result result) =>
        result.IsSuccess ? NoContent() : HandleError(result.Error);

    private IActionResult HandleError(Error error) => error.Code switch
    {
        var code when code.Contains("NotFound") => NotFound(error),
        var code when code.Contains("Validation") => BadRequest(error),
        var code when code.Contains("Conflict") => Conflict(error),
        var code when code.Contains("Unauthorized") => Unauthorized(error),
        _ => StatusCode(500, "Server failure")
    };
}