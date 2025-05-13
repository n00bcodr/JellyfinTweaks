using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class ForceThemeMusicTweak(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "ForceEnableThemeMusic";

    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!,
        [
            new TweakSearching(
                // Start marker:
                "enableThemeSongs:function(){return(0,i.G4)(this.get(\"enableThemeSongs\",",
                // End marker:
                "))}}"
            )
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        // This 'value' will replace the original e.g., "!1),!1" part in the JS
        var value = configuration.ForceEnableThemeMusic ? "!0),!0" : "!1),!1";
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}