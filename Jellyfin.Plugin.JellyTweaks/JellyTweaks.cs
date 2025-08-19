using System.Globalization;
using Jellyfin.Plugin.JellyTweaks.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System.IO;
using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks;

public class JellyTweaks : BasePlugin<PluginConfiguration>, IHasWebPages
{
    private readonly IApplicationPaths _applicationPaths;
    private readonly ILogger<JellyTweaks> _logger;

    public JellyTweaks(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, ILoggerFactory loggerFactory) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
        _applicationPaths = applicationPaths;
        _logger = loggerFactory.CreateLogger<JellyTweaks>();
    }
    /// <inheritdoc />
    public override string Name => "Jellyfin Tweaks";
    /// <inheritdoc />
    public override Guid Id => Guid.Parse("dfee3828-01df-49df-85b1-5c2b75e5ea1a");
    public static JellyTweaks? Instance { get; private set; }
    public string IndexHtmlPath => Path.Combine(_applicationPaths.WebPath, "index.html");

    public void InjectScript()
    {
        try
        {
            var indexPath = IndexHtmlPath;
            if (string.IsNullOrEmpty(indexPath) || !File.Exists(indexPath))
            {
                _logger.LogError("Could not find index.html at path: {Path}", indexPath);
                return;
            }

            var pluginVersion = Version.ToString();
            var scriptUrl = "/JellyTweaks/script";
            var scriptTag = $"<script plugin=\"JellyTweaks\" version=\"{pluginVersion}\" src=\"{scriptUrl}\" defer></script>";

            var content = File.ReadAllText(indexPath);

            if (content.Contains(scriptTag))
            {
                _logger.LogInformation("JellyTweaks script is already correctly injected. No changes needed.");
                return;
            }

            var regex = new Regex($"<script[^>]*src=[\"']{scriptUrl}[\"'][^>]*>\\s*</script>\\n?");

            if (regex.IsMatch(content))
            {
                _logger.LogInformation("Removing old JellyTweaks script tag.");
                content = regex.Replace(content, string.Empty);
            }

            var closingBodyTag = "</body>";
            if (content.Contains(closingBodyTag))
            {
                content = content.Replace(closingBodyTag, $"{scriptTag}\n{closingBodyTag}");
                File.WriteAllText(indexPath, content);
                _logger.LogInformation("Successfully injected/updated the JellyTweaks script in index.html.");
            }
            else
            {
                _logger.LogWarning("Could not find </body> tag in index.html. Script not injected.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to inject script into index.html.");
        }
    }

    public override void OnUninstalling()
    {
        // Final cleanup on uninstall to be safe
        try
        {
            var indexPath = IndexHtmlPath;
            if (string.IsNullOrEmpty(indexPath) || !File.Exists(indexPath))
            {
                _logger.LogError("Could not find index.html at path: {Path}", indexPath);
                return;
            }

            var content = File.ReadAllText(indexPath);
            var regex = new Regex($"<script plugin=\"{Name}\".*?></script>\\n?");
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, string.Empty);
                File.WriteAllText(indexPath, content);
                _logger.LogInformation("Successfully removed the {Name} script from index.html during uninstall.", Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to remove script from index.html during uninstall.");
        }

        base.OnUninstalling();
    }

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