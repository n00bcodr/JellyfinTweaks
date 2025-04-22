using System.Collections.ObjectModel;

namespace Jellyfin.Plugin.JellyTweaks.Data;

public class TweakFile(string path, Collection<TweakSearching> searchingValues)
{
    public string Path { get; } = path;

    public Collection<TweakSearching> SearchingValues { get; } = searchingValues;
}
