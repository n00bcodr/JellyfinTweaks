namespace Jellyfin.Plugin.JellyTweaks.Configuration;

/// <summary>
///     Forced defaults for one (MediaType, InNetwork) combination of jellyfin-web's
///     device-scoped bitrate settings (<c>appSettings.js</c>: <c>enableautobitratebitrate-*</c> /
///     <c>maxbitrate-*</c> keys). Exactly 4 combinations exist: MediaType is "Audio" or "Video",
///     crossed with InNetwork true/false.
/// </summary>
public class BitrateOverride
{
    public string MediaType { get; set; } = string.Empty;
    public bool InNetwork { get; set; }
    public bool? AutoDetect { get; set; }
    public int? MaxBitrate { get; set; }
}
