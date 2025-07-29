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
        ForceEnableThemeMusic = false;
        ForceEnableThemeVideos = false;
        ForceDisableNextVideoInfo = false;
        ForceEnableRewatchingInNextUp = false;
        ForceEnableEpisodeImagesInNextUp = false;
    }

    public int DefaultLibraryPageSize { get; set; }
    public int? MaxDaysNextUp { get; set; }
    public bool EnableBackdropsByDefault { get; set; }
    public bool ForceEnableThemeMusic { get; set; }
    public bool ForceEnableThemeVideos { get; set; }
    public bool ForceDisableNextVideoInfo { get; set; }
    public bool ForceEnableRewatchingInNextUp { get; set; }
    public bool ForceEnableEpisodeImagesInNextUp { get; set; }
}
