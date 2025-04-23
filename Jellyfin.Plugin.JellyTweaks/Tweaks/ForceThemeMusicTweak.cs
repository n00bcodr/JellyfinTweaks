using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class ForceThemeMusicTweak(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "ForceEnableThemeMusic";

    // NOTE: These markers target main.jellyfin.bundle.js
    // Start marker ends just BEFORE the '!1'. End marker starts just AFTER it.
    // Replacement value will be '!0' (or '!1' to revert).
    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!,
        [
            // Find: enableThemeSongs:function(){return j}
            new TweakSearching("enableThemeSongs:function(){return ", "}")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var value = configuration.ForceEnableThemeMusic ? "!0" : "j"; // Use !0 for true, j for original
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}