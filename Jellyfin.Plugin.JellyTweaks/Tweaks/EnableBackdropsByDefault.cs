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
            new TweakSearching(
                "value:function(e){return void 0!==e?this.set(\"enableBackdrops\",e.toString(),!1):(0,i.G4)(this.get(\"enableBackdrops\",",
                "))},")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var value = configuration.EnableBackdropsByDefault ? "!0),!0" : "!1),!1";
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}