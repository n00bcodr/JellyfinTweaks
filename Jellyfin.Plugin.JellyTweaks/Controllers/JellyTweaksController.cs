using System.Linq;
using MediaBrowser.Controller.Library;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Jellyfin.Plugin.JellyTweaks.Controllers;

[Route("JellyTweaks")]
[ApiController]
public class JellyTweaksController : ControllerBase
{
    private readonly ILibraryManager _libraryManager;

    public JellyTweaksController(ILibraryManager libraryManager)
    {
        _libraryManager = libraryManager;
    }

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

    [HttpGet("configPage.css")]
    [Produces("text/css")]
    public ActionResult GetConfigPageStyles()
    {
        // Served separately since the dashboard discards <style> tags in the config page's <head>.
        var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Jellyfin.Plugin.JellyTweaks.Configuration.configPage.css");

        if (stream == null)
        {
            return NotFound();
        }

        return new FileStreamResult(stream, "text/css");
    }

    [HttpGet("public-config")]
    public ActionResult GetPublicConfig()
    {
        var config = JellyTweaks.Instance?.Configuration;
        if (config == null)
        {
            return StatusCode(503);
        }

        return new JsonResult(new
        {
            config.DefaultLibraryPageSize,
            config.MaxDaysNextUp,
            config.EnableBackdropsByDefault,
            config.EnableDetailsBannerByDefault,
            config.ForceEnableThemeMusic,
            config.ForceEnableThemeVideos,
            config.ForceDisableNextVideoInfo,
            config.ForceEnableRewatchingInNextUp,
            config.ForceEnableEpisodeImagesInNextUp,
            config.DisplayMode,
            config.MaxVideoWidth,
            config.LimitSupportedVideoResolution,
            config.ForceEnablePgsSubtitleRendering,
            config.ForceAudioLanguagePreference,
            config.ForcePlayDefaultAudioTrack,
            config.ForceSubtitleLanguagePreference,
            config.ForceSubtitleMode,
            config.ForceRememberAudioSelections,
            config.ForceRememberSubtitleSelections,
            config.LibrarySortOverrides
        });
    }

    [HttpGet("libraries")]
    public ActionResult GetLibraries()
    {
        var libraries = _libraryManager.GetVirtualFolders()
            .Where(folder => folder.CollectionType != null)
            .Select(folder => new
            {
                Id = folder.ItemId,
                folder.Name,
                CollectionType = folder.CollectionType.ToString()
            });

        return new JsonResult(libraries);
    }
}
