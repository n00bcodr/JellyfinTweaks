using System.Collections.ObjectModel;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Utils;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Tweaks;

// TODO: Fix for 10.9.x
public class DefaultTitle(ILogger<Tweak> logger) : Tweak(Name, _files)
{
    private new const string Name = "DefaultTitle";

    private static readonly Collection<TweakFile> _files =
    [
        // new TweakFile(Paths.MainJs!,
        // [
        //     new TweakSearching("document.title=\"", "\"}"),
        //     new TweakSearching("document.title=e||\"", "\"}")
        // ]),
        new TweakFile(Paths.IndexHtml!,
        [
            new TweakSearching("<title>", "</title>"),
            //new TweakSearching("<meta property=\"og:title\" content=\"", "\">"),
            //new TweakSearching("<meta property=\"og:site_name\" content=\"", "\">"),
            new TweakSearching("<meta name=\"application-name\" content=\"", "\">")
        ])
    ];

    public override async Task Execute(PluginConfiguration configuration)
    {
        var value = configuration.DefaultTitle;
        await TweakUtils.ApplyTweakAsync(logger, this, value).ConfigureAwait(false);
    }
}
