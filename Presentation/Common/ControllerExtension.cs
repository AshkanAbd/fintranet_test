namespace Presentation.Common;

using System.Net.Mime;
using Application.Common.Response;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/")]
public class ControllerExtension : Controller
{
    protected ActionResult<StdResponse<T>> FormatResponse<T>(StdResponse<T> response) where T : class
    {
        return ResponseFormat.Base(response);
    }
}