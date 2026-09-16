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
            config.DisableAllTweaks,
            config.DefaultLibraryPageSize,
            config.MaxDaysNextUp,
            config.EnableBackdropsByDefault,
            config.EnableDetailsBannerByDefault,
            config.ForceEnableThemeMusic,
            config.ForceEnableThemeVideos,
            config.ForceDisableNextVideoInfo,
            config.ForceEnableRewatchingInNextUp,
            config.ForceEnableEpisodeImagesInNextUp,
            config.DesktopDisplayMode,
            config.MobileDisplayMode,
            config.MaxVideoWidth,
            config.LimitSupportedVideoResolution,
            config.ForceEnablePgsSubtitleRendering,
            config.ForceAudioLanguagePreference,
            config.ForcePlayDefaultAudioTrack,
            config.ForceSubtitleLanguagePreference,
            config.ForceSubtitleMode,
            config.ForceRememberAudioSelections,
            config.ForceRememberSubtitleSelections,
            config.LibrarySortOverrides,

            config.ForceAllowedAudioChannels,
            config.ForcePreferFmp4HlsContainer,
            config.ForceLimitSegmentLength,
            config.SubtitleAppearance,

            config.ForceEnableCinemaMode,
            config.ForceEnableVideoRemainingTime,
            config.ForceEnableFastFadein,
            config.ForceEnableBlurhash,
            config.ForceSelectAudioNormalization,
            config.ForceStillWatchingPrompt,
            config.ForceSkipBackLength,
            config.ForceSkipForwardLength,
            config.ForceEnableNextEpisodeAutoPlay,
            config.ForceHidePlayedInLatest,
            config.ForceDisplayMissingEpisodes,

            config.ForceAppTheme,
            config.ForceDashboardTheme,
            config.ForceScreensaver,
            config.ForceScreensaverTime,
            config.ForceBackdropScreensaverInterval,
            config.ForceSlideshowInterval,
            config.ForceLanguage,
            config.ForceDateTimeLocale,
            config.ForceDisableCustomCss,
            config.ForceCustomCss,

            config.ForceGuideColorCodedBackgrounds,
            config.ForceLiveTvFavoriteChannelsAtTop,
            config.ForceLiveTvChannelOrder,
            config.GuideIndicatorHd,
            config.GuideIndicatorLive,
            config.GuideIndicatorNew,
            config.GuideIndicatorPremiere,
            config.GuideIndicatorRepeat,

            config.ForceEnableLocalPassword,
            config.ManageLatestItemsExcludes,
            config.LatestItemsExcludes,
            config.ManageMyMediaExcludes,
            config.MyMediaExcludes,
            config.ManageOrderedViews,
            config.OrderedViews,
            config.ManageGroupedFolders,
            config.GroupedFolders,
            config.HomeSections,
            config.LandingScreens,

            config.ForceEnableAutoLogin,
            config.ForceEnableGamepad,
            config.ForceEnableSmoothScroll,
            config.ForceEnableSystemExternalPlayers,
            config.ForcePreferredTranscodeVideoCodec,
            config.ForcePreferredTranscodeVideoAudioCodec,
            config.ForceAlwaysBurnInSubtitleWhenTranscoding,
            config.ForceSubtitleBurnInMode,
            config.ForceEnableDts,
            config.ForceEnableTrueHd,
            config.ForceEnableHi10p,
            config.ForceDisableVbrAudio,
            config.ForceAlwaysRemuxFlac,
            config.ForceAlwaysRemuxMp3,
            config.ForceAspectRatio,
            config.ForceMaxStaticMusicBitrate,
            config.ForceMaxChromecastBitrate,
            config.BitrateOverrides
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
