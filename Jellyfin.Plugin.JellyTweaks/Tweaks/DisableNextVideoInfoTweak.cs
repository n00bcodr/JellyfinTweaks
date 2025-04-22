using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class DisableNextVideoInfoTweak(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "ForceDisableNextVideoInfo";

    // NOTE: These markers target main.jellyfin.bundle.js
    // Start marker ends just BEFORE the 'j'. End marker IS the '}'.
    // Replacement value will be 'ne' (or 'j' to revert).
    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!,
        [
            new TweakSearching("enableNextVideoInfoOverlay:function(){return ", "}")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        // If checkbox is checked, replace 'j' with 'ne' (disable).
        // If checkbox is unchecked, replace 'ne' with 'j' (enable/revert).
        // The value passed to ApplyTweakAsync is what goes BETWEEN the markers.
        var value = configuration.ForceDisableNextVideoInfo ? "ne" : "j";
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}