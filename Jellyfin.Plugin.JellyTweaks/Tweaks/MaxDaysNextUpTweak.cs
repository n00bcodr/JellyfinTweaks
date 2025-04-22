using System.Collections.ObjectModel;
using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class MaxDaysNextUpTweak(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "MaxDaysNextUp"; // Name for logging

    // Define which file and what text markers to search for
    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!, // Target file from Paths.cs
        [
            // TweakSearching uses start/end markers
            // These markers are based on the JS snippet you provided earlier.
            // IMPORTANT: Verify these markers against your current main.jellyfin.bundle.js!
            new TweakSearching("parseInt(this.get(\"maxDaysForNextUp\",!1),10);return 0===t?0:t||", "}}")
        ])
    ];

    // This method executes the tweak
    public override async Task Execute(PluginConfiguration configuration)
    {
        // Default to 365 if null, otherwise use the configured value (must be >= 0)
        var valueToSet = configuration.MaxDaysNextUp.HasValue && configuration.MaxDaysNextUp.Value >= 0
                        ? configuration.MaxDaysNextUp.Value
                        : 365;

        // Convert the number to a string for replacement
        var valueString = valueToSet.ToString(CultureInfo.InvariantCulture);

        // Call the utility function to apply the tweak
        await TweakUtils.ApplyTweakAsync(logger, this, valueString).ConfigureAwait(false);
    }
}