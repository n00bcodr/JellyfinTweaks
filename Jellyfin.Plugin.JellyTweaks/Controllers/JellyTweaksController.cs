using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Jellyfin.Plugin.JellyTweaks.Controllers;

[Route("JellyTweaks")]
[ApiController]
public class JellyTweaksController : ControllerBase
{
    [HttpGet("script")]
    [Produces("application/javascript")]
    public ActionResult GetScript()
    {
        // Get the embedded applyTweaks.js file from the assembly
        var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Jellyfin.Plugin.JellyTweaks.Configuration.applyTweaks.js");

        if (stream == null)
        {
            return NotFound();
        }

        // Return the file with the correct content type
        return new FileStreamResult(stream, "application/javascript");
    }
}
