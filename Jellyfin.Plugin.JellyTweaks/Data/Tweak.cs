using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;

namespace Jellyfin.Plugin.JellyTweaks.Data;

public class Tweak(string name, Collection<TweakFile> files)
{
    public string Name { get; } = name;

    public Collection<TweakFile> Files { get; } = files;

    public virtual async Task Execute(PluginConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
