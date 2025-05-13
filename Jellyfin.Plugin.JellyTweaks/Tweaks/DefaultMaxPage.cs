using System.Collections.ObjectModel;
using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

public class DefaultMaxPage(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "DefaultLibraryPageSize";

    private static readonly Collection<TweakFile> _files =
    [
        new TweakFile(Paths.MainJs!,
        [
            new TweakSearching(
                // Start marker: Includes the function definition up to the point before the value to change
                "libraryPageSize:function(){var e=parseInt(this.get(\"libraryPageSize\",!1),10);return 0===e?0:e||",
                // End marker: The closing braces of the function
                "}}"
            )
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        // The value to insert is the configured page size.
        // It replaces the original default (e.g., 100) in the JS.
        var value = Math.Abs(configuration.DefaultLibraryPageSize).ToString(CultureInfo.InvariantCulture);
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}