using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.JellyTweaks;

public class JellyTweaks : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly IApplicationPaths _applicationPaths;

    public JellyTweaks(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
        _applicationPaths = applicationPaths;
    }
    /// <inheritdoc />
    public override string Name => "Jellyfin Tweaks";
    /// <inheritdoc />
    public override Guid Id => Guid.Parse("dfee3828-01df-49df-85b1-5c2b75e5ea1a");
    public static JellyTweaks? Instance { get; private set; }
    public string IndexHtmlPath => Path.Combine(_applicationPaths.WebPath, "index.html");
    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = this.Name, // "Jellyfin Tweaks"
                EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.configPage.html", GetType().Namespace)
            }
        ];
    }
}
