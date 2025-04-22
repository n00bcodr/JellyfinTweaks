namespace Jellyfin.Plugin.JellyTweaks.Data;

public class TweakSearching(string start, string end)
{
    public string Start { get; } = start;

    public string End { get; } = end;
}
