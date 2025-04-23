using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.JellyTweaks;

public class JellyTweaks : BasePlugin<PluginConfiguration>, IHasWebPages
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JellyTweaks" /> class.
    /// </summary>
    /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths" /> interface.</param>
    /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer" /> interface.</param>
    public JellyTweaks(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;

        // Check if webPath is not null
        if (string.IsNullOrWhiteSpace(applicationPaths.WebPath))
        {
            return;
        }

        Paths.IndexHtml = Path.Combine(applicationPaths.WebPath, "index.html");
        Paths.MainJs = Path.Combine(applicationPaths.WebPath, "main.jellyfin.bundle.js");
    }

    /// <inheritdoc />
    public override string Name => "Jellyfin Tweaks";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("dfee3828-01df-49df-85b1-5c2b75e5ea1a");

    /// <summary>
    ///     Gets the current plugin instance.
    /// </summary>
    public static JellyTweaks? Instance { get; private set; }

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo { Name = Name, EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.configPage.html", GetType().Namespace) }
        ];
    }
}
