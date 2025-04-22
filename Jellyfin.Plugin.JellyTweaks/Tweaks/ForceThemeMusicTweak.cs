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
             // Searching for: this.get("enableThemeSongs",!1),!1);
             // Replace !1 with !0 if checked
            new TweakSearching("this.get(\"enableThemeSongs\",!1),", ");")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        // If checkbox is checked, replace '!1' with '!0' (force enable).
        // If checkbox is unchecked, replace '!0' with '!1' (revert to default logic).
        // The value passed to ApplyTweakAsync is what goes BETWEEN the markers.
        var value = configuration.ForceEnableThemeMusic ? "!0" : "!1";
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}