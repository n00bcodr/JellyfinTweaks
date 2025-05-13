using System.Collections.ObjectModel;
using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class MaxDaysNextUpTweak(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "MaxDaysNextUp";

    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!,
        [
            new TweakSearching(
                // Start marker: Includes the function definition up to the point before the value to change
                "maxDaysForNextUp:function(){var e=parseInt(this.get(\"maxDaysForNextUp\",!1),10);return 0===e?0:e||",
                // End marker: The closing braces of the function
                "}}"
            )
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var valueToSet = configuration.MaxDaysNextUp.HasValue && configuration.MaxDaysNextUp.Value >= 0
                        ? configuration.MaxDaysNextUp.Value
                        : 365; // Default if not set or invalid

        // The valueString will replace the original default (e.g., 365) in the JS.
        var valueString = valueToSet.ToString(CultureInfo.InvariantCulture);
        await TweakUtils.ApplyTweakAsync(logger, this, valueString).ConfigureAwait(false);
    }
}