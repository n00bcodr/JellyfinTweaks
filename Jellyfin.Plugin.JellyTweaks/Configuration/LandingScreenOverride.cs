namespace Jellyfin.Plugin.JellyTweaks.Configuration;

/// <summary>
///     Forced default landing tab for one library (or "livetv"), written to the legacy
///     "usersettings" DisplayPreferences as a "landing-{LibraryId}" CustomPrefs key
///     (see jellyfin-web's homeScreenSettings.js).
/// </summary>
public class LandingScreenOverride
{
    public string LibraryId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
