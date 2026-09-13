using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JellyTweaks.Configuration;

/// <summary>
///     Plugin configuration.
/// </summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PluginConfiguration" /> class.
    /// </summary>
    public PluginConfiguration()
    {
        DefaultLibraryPageSize = 100;
        MaxDaysNextUp = 365;
        EnableBackdropsByDefault = null;
        EnableDetailsBannerByDefault = null;
        ForceEnableThemeMusic = null;
        ForceEnableThemeVideos = null;
        ForceDisableNextVideoInfo = null;
        ForceEnableRewatchingInNextUp = null;
        ForceEnableEpisodeImagesInNextUp = null;
        DisplayMode = "";
        MaxVideoWidth = 0;
        LimitSupportedVideoResolution = false;
        ForceEnablePgsSubtitleRendering = null;
        ForceAudioLanguagePreference = null;
        ForcePlayDefaultAudioTrack = null;
        ForceSubtitleLanguagePreference = null;
        ForceSubtitleMode = null;
        ForceRememberAudioSelections = null;
        ForceRememberSubtitleSelections = null;
        LibrarySortOverrides = new List<LibrarySortOverride>();

        // Audio & Subtitle (additional)
        ForceAllowedAudioChannels = null;
        ForcePreferFmp4HlsContainer = null;
        ForceLimitSegmentLength = null;
        SubtitleAppearance = new SubtitleAppearanceOverride();

        // Playback Behavior
        ForceEnableCinemaMode = null;
        ForceEnableVideoRemainingTime = null;
        ForceEnableFastFadein = null;
        ForceEnableBlurhash = null;
        ForceSelectAudioNormalization = null;
        ForceStillWatchingPrompt = null;
        ForceSkipBackLength = null;
        ForceSkipForwardLength = null;
        ForceEnableNextEpisodeAutoPlay = null;
        ForceHidePlayedInLatest = null;
        ForceDisplayMissingEpisodes = null;

        // Branding & Screensaver
        ForceAppTheme = null;
        ForceDashboardTheme = null;
        ForceScreensaver = null;
        ForceScreensaverTime = null;
        ForceBackdropScreensaverInterval = null;
        ForceSlideshowInterval = null;
        ForceLanguage = null;
        ForceDateTimeLocale = null;
        ForceDisableCustomCss = null;
        ForceCustomCss = null;

        // Live TV & Guide
        ForceGuideColorCodedBackgrounds = null;
        ForceLiveTvFavoriteChannelsAtTop = null;
        ForceLiveTvChannelOrder = null;
        GuideIndicatorHd = null;
        GuideIndicatorLive = null;
        GuideIndicatorNew = null;
        GuideIndicatorPremiere = null;
        GuideIndicatorRepeat = null;

        // Home & Library
        ForceEnableLocalPassword = null;
        ManageLatestItemsExcludes = false;
        LatestItemsExcludes = new List<string>();
        ManageMyMediaExcludes = false;
        MyMediaExcludes = new List<string>();
        ManageOrderedViews = false;
        OrderedViews = new List<string>();
        ManageGroupedFolders = false;
        GroupedFolders = new List<string>();
        HomeSections = new List<string> { "", "", "", "", "", "", "", "", "", "" };
        LandingScreens = new List<LandingScreenOverride>();

        // Device Defaults
        ForceEnableAutoLogin = null;
        ForceEnableGamepad = null;
        ForceEnableSmoothScroll = null;
        ForceEnableSystemExternalPlayers = null;
        ForcePreferredTranscodeVideoCodec = null;
        ForcePreferredTranscodeVideoAudioCodec = null;
        ForceAlwaysBurnInSubtitleWhenTranscoding = null;
        ForceSubtitleBurnInMode = null;
        ForceEnableDts = null;
        ForceEnableTrueHd = null;
        ForceEnableHi10p = null;
        ForceDisableVbrAudio = null;
        ForceAlwaysRemuxFlac = null;
        ForceAlwaysRemuxMp3 = null;
        ForceAspectRatio = null;
        ForceMaxStaticMusicBitrate = null;
        ForceMaxChromecastBitrate = null;
        BitrateOverrides = new List<BitrateOverride>();
    }

    public int DefaultLibraryPageSize { get; set; }
    public int? MaxDaysNextUp { get; set; }
    public bool? EnableBackdropsByDefault { get; set; }
    public bool? EnableDetailsBannerByDefault { get; set; }
    public bool? ForceEnableThemeMusic { get; set; }
    public bool? ForceEnableThemeVideos { get; set; }
    public bool? ForceDisableNextVideoInfo { get; set; }
    public bool? ForceEnableRewatchingInNextUp { get; set; }
    public bool? ForceEnableEpisodeImagesInNextUp { get; set; }
    public string DisplayMode { get; set; }
    public int MaxVideoWidth { get; set; }
    public bool LimitSupportedVideoResolution { get; set; }
    public bool? ForceEnablePgsSubtitleRendering { get; set; }
    public string? ForceAudioLanguagePreference { get; set; }
    public bool? ForcePlayDefaultAudioTrack { get; set; }
    public string? ForceSubtitleLanguagePreference { get; set; }
    public string? ForceSubtitleMode { get; set; }
    public bool? ForceRememberAudioSelections { get; set; }
    public bool? ForceRememberSubtitleSelections { get; set; }
    public List<LibrarySortOverride> LibrarySortOverrides { get; set; }

    // ===================== Audio & Subtitle (additional) =====================
    public string? ForceAllowedAudioChannels { get; set; }
    public bool? ForcePreferFmp4HlsContainer { get; set; }
    public bool? ForceLimitSegmentLength { get; set; }
    public SubtitleAppearanceOverride SubtitleAppearance { get; set; }

    // ===================== Playback Behavior =====================
    public bool? ForceEnableCinemaMode { get; set; }
    public bool? ForceEnableVideoRemainingTime { get; set; }
    public bool? ForceEnableFastFadein { get; set; }
    public bool? ForceEnableBlurhash { get; set; }
    public string? ForceSelectAudioNormalization { get; set; }
    public string? ForceStillWatchingPrompt { get; set; }
    public int? ForceSkipBackLength { get; set; }
    public int? ForceSkipForwardLength { get; set; }
    public bool? ForceEnableNextEpisodeAutoPlay { get; set; }
    public bool? ForceHidePlayedInLatest { get; set; }
    public bool? ForceDisplayMissingEpisodes { get; set; }

    // ===================== Branding & Screensaver =====================
    public string? ForceAppTheme { get; set; }
    public string? ForceDashboardTheme { get; set; }
    public string? ForceScreensaver { get; set; }
    public int? ForceScreensaverTime { get; set; }
    public int? ForceBackdropScreensaverInterval { get; set; }
    public int? ForceSlideshowInterval { get; set; }
    public string? ForceLanguage { get; set; }
    public string? ForceDateTimeLocale { get; set; }
    public bool? ForceDisableCustomCss { get; set; }
    public string? ForceCustomCss { get; set; }

    // ===================== Live TV & Guide =====================
    public bool? ForceGuideColorCodedBackgrounds { get; set; }
    public bool? ForceLiveTvFavoriteChannelsAtTop { get; set; }
    public string? ForceLiveTvChannelOrder { get; set; }

    // Only these 5 correspond to real "guide-indicator-*" DisplayPreferences keys.
    // The guide's separate "categories" filter (Movies/Kids/News/Sports) is not exposed here.
    public bool? GuideIndicatorHd { get; set; }
    public bool? GuideIndicatorLive { get; set; }
    public bool? GuideIndicatorNew { get; set; }
    public bool? GuideIndicatorPremiere { get; set; }
    public bool? GuideIndicatorRepeat { get; set; }

    // ===================== Home & Library =====================
    public bool? ForceEnableLocalPassword { get; set; }

    // Each of these 4 lists has a companion "Manage*" flag, since an empty list is itself a
    // valid forced value (e.g. "force nothing grouped") and must be distinguished from
    // "don't manage this".
    public bool ManageLatestItemsExcludes { get; set; }

    /// <summary>Library (folder) GUIDs, as strings, excluded from the home "Latest" rows.</summary>
    public List<string> LatestItemsExcludes { get; set; }

    public bool ManageMyMediaExcludes { get; set; }

    /// <summary>Library (folder) GUIDs, as strings, excluded from the home "My Media" section.</summary>
    public List<string> MyMediaExcludes { get; set; }

    public bool ManageOrderedViews { get; set; }

    /// <summary>Library (folder) GUIDs, as strings, in the forced home-screen display order.</summary>
    public List<string> OrderedViews { get; set; }

    public bool ManageGroupedFolders { get; set; }

    /// <summary>Library (folder) GUIDs, as strings, grouped into a single home-screen entry.</summary>
    public List<string> GroupedFolders { get; set; }

    /// <summary>The 10 configurable home screen section slots. An empty entry means "don't manage".</summary>
    public List<string> HomeSections { get; set; }

    /// <summary>Forced default landing tab per library (or "livetv").</summary>
    public List<LandingScreenOverride> LandingScreens { get; set; }

    // ===================== Device Defaults =====================
    public bool? ForceEnableAutoLogin { get; set; }
    public bool? ForceEnableGamepad { get; set; }
    public bool? ForceEnableSmoothScroll { get; set; }
    public bool? ForceEnableSystemExternalPlayers { get; set; }
    public string? ForcePreferredTranscodeVideoCodec { get; set; }
    public string? ForcePreferredTranscodeVideoAudioCodec { get; set; }
    public bool? ForceAlwaysBurnInSubtitleWhenTranscoding { get; set; }
    public string? ForceSubtitleBurnInMode { get; set; }
    public bool? ForceEnableDts { get; set; }
    public bool? ForceEnableTrueHd { get; set; }
    public bool? ForceEnableHi10p { get; set; }
    public bool? ForceDisableVbrAudio { get; set; }
    public bool? ForceAlwaysRemuxFlac { get; set; }
    public bool? ForceAlwaysRemuxMp3 { get; set; }
    public string? ForceAspectRatio { get; set; }
    public int? ForceMaxStaticMusicBitrate { get; set; }
    public int? ForceMaxChromecastBitrate { get; set; }
    public List<BitrateOverride> BitrateOverrides { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request-time script injection
    /// middleware (<see cref="Services.ScriptInjectionStartupFilter"/>) is disabled.
    /// When disabled, the plugin falls back to registering with the File
    /// Transformation plugin (if installed) or writing directly to index.html.
    /// Off by default: the middleware is the primary injection path.
    /// </summary>
    public bool DisableScriptInjectionMiddleware { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all tweaks are turned off. When true, the
    /// loader script is not injected into the web client (neither via the middleware nor
    /// the File Transformation/on-disk fallback) and the applied script, if already
    /// loaded in a browser, no-ops. The plugin itself stays installed and registered.
    /// Off by default.
    /// </summary>
    public bool DisableAllTweaks { get; set; }
}
