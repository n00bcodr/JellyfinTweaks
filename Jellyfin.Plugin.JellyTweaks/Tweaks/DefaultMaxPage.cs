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
            new TweakSearching("this.get(\"libraryPageSize\",!1),10);return 0===t?0:t||", "}")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var value = Math.Abs(configuration.DefaultLibraryPageSize).ToString(CultureInfo.InvariantCulture);
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}
