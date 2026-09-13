namespace Jellyfin.Plugin.JellyTweaks.Configuration;

/// <summary>
///     Forced defaults for jellyfin-web's subtitle appearance settings, written as a single JSON blob
///     to the <c>localplayersubtitleappearance3</c> localStorage key (see
///     jellyfin-web's <c>subtitleappearancehelper.js</c>/<c>subtitlesettings.js</c>).
/// </summary>
public class SubtitleAppearanceOverride
{
    /// <summary>
    ///     Gets or sets a value indicating whether this override is active. When false, none of the
    ///     other fields are written, since the whole section behaves as one "Don't manage" toggle
    ///     mapping to a single JSON blob rather than independently-managed keys.
    /// </summary>
    public bool Enabled { get; set; }

    public string? SubtitleStyling { get; set; }
    public string? TextSize { get; set; }
    public string? TextWeight { get; set; }
    public string? DropShadow { get; set; }
    public string? Font { get; set; }
    public string? TextBackground { get; set; }
    public string? TextColor { get; set; }
    public int? VerticalPosition { get; set; }
    public string? AspectMode { get; set; }
}
