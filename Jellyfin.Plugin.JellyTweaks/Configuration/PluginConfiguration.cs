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
        EnableBackdropsByDefault = false;
        DefaultLibraryPageSize = 100;
        DefaultTitle = "Jellyfin";
        MaxDaysNextUp = 365;
        ForceDisableNextVideoInfo = false;
        ForceEnableThemeMusic = false;
    }

    /// <summary>
    ///     Gets or sets a value of default title.
    /// </summary>
    public string DefaultTitle { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether backdrops is enabled by default.
    /// </summary>
    public bool EnableBackdropsByDefault { get; set; }

    /// <summary>
    ///     Gets or sets a value of default page size.
    /// </summary>
    public int DefaultLibraryPageSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum days items stay in Next Up. Null means use default.
    /// </summary>
    public int? MaxDaysNextUp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force disable the next video info overlay.
    /// </summary>
    public bool ForceDisableNextVideoInfo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to force enable theme music.
    /// </summary>
    public bool ForceEnableThemeMusic { get; set; }
}
