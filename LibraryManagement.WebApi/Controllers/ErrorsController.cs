using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class ErrorsController : ControllerBase
{
    [HttpGet("/error")]
    public IActionResult Error()
    {
        Exception? exception =
            HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        return Problem(
            title: exception?.Message,
            statusCode: 400);
    }
}