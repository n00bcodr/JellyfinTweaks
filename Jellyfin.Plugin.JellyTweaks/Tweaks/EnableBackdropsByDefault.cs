using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class EnableBackdropsByDefault(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "EnableBackdropsByDefault";

    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!, [
            new TweakSearching("enableBackdrops:function(){return ", "}")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var value = configuration.EnableBackdropsByDefault ? "P" : "_";
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}
