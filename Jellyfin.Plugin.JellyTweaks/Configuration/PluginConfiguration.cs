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
        EnableBackdropsByDefault = false;
        EnableDetailsBannerByDefault = false;
        ForceEnableThemeMusic = false;
        ForceEnableThemeVideos = false;
        ForceDisableNextVideoInfo = false;
        ForceEnableRewatchingInNextUp = false;
        ForceEnableEpisodeImagesInNextUp = false;
        DisplayMode = "";
        MaxVideoWidth = 0;
        LimitSupportedVideoResolution = false;
    }

    public int DefaultLibraryPageSize { get; set; }
    public int? MaxDaysNextUp { get; set; }
    public bool EnableBackdropsByDefault { get; set; }
    public bool EnableDetailsBannerByDefault { get; set; }
    public bool ForceEnableThemeMusic { get; set; }
    public bool ForceEnableThemeVideos { get; set; }
    public bool ForceDisableNextVideoInfo { get; set; }
    public bool ForceEnableRewatchingInNextUp { get; set; }
    public bool ForceEnableEpisodeImagesInNextUp { get; set; }
    public string DisplayMode { get; set; }
    public int MaxVideoWidth { get; set; }
    public bool LimitSupportedVideoResolution { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the request-time script injection
    /// middleware (<see cref="Services.ScriptInjectionStartupFilter"/>) is disabled.
    /// When disabled, the plugin falls back to registering with the File
    /// Transformation plugin (if installed) or writing directly to index.html.
    /// Off by default -- the middleware is the primary injection path.
    /// </summary>
    public bool DisableScriptInjectionMiddleware { get; set; }
}
