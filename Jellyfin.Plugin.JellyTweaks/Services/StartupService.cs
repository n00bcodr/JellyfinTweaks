using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.JellyTweaks.Helpers;
using Jellyfin.Plugin.JellyTweaks.JellyfinVersionSpecific;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Jellyfin.Plugin.JellyTweaks.Services
{
    public class StartupService : IScheduledTask
    {
        private readonly ILogger<StartupService> _logger;
        private readonly IApplicationPaths _applicationPaths;

        public string Name => "Jellyfin Tweaks Startup";
        public string Key => "JellyTweaksStartup";
        public string Description => "Injects the Jellyfin Tweaks script using the File Transformation plugin and performs necessary cleanups.";
        public string Category => "Startup Services";

        public StartupService(ILogger<StartupService> logger, IApplicationPaths applicationPaths)
        {
            _logger = logger;
            _applicationPaths = applicationPaths;
        }

        public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                CleanupOldScript();
                RegisterFileTransformation();
            }, cancellationToken);
        }

        private void CleanupOldScript()
        {
            try
            {
                var indexPath = Path.Combine(_applicationPaths.WebPath, "index.html");
                if (!File.Exists(indexPath))
                {
                    _logger.LogWarning("Could not find index.html at path: {Path}. Unable to perform cleanup.", indexPath);
                    return;
                }

                var content = File.ReadAllText(indexPath);
                var regex = new Regex($"<script[^>]*src=[\"'][^\"']*JellyTweaks/script[\"'][^>]*>\\s*</script>\\n?");

                if (regex.IsMatch(content))
                {
                    _logger.LogInformation("Found old Jellyfin Tweaks script tag in index.html. Removing it now.");
                    content = regex.Replace(content, string.Empty);
                    File.WriteAllText(indexPath, content);
                    _logger.LogInformation("Successfully removed old script tag.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cleanup of old script from index.html.");
            }
        }

        private void RegisterFileTransformation()
        {
            Assembly? fileTransformationAssembly =
                AssemblyLoadContext.All.SelectMany(x => x.Assemblies).FirstOrDefault(x =>
                    x.FullName?.Contains(".FileTransformation") ?? false);

            if (fileTransformationAssembly != null)
            {
                Type? pluginInterfaceType = fileTransformationAssembly.GetType("Jellyfin.Plugin.FileTransformation.PluginInterface");

                if (pluginInterfaceType != null)
                {
                    var payload = new JObject
                    {
                        { "id", "dfee3828-01df-49df-85b1-5c2b75e5ea1a" }, // Using the plugin's GUID as a unique ID
                        { "fileNamePattern", "index.html" },
                        { "callbackAssembly", GetType().Assembly.FullName },
                        { "callbackClass", typeof(TransformationPatches).FullName },
                        { "callbackMethod", nameof(TransformationPatches.IndexHtml) }
                    };

                    pluginInterfaceType.GetMethod("RegisterTransformation")?.Invoke(null, new object?[] { payload });
                    _logger.LogInformation("Successfully registered Jellyfin Tweaks script injection with the File Transformation plugin.");
                }
                else
                {
                    _logger.LogWarning("Could not find PluginInterface in FileTransformation assembly. Using fallback injection method.");
                    JellyTweaks.Instance?.InjectScript();
                }
            }
            else
            {
                _logger.LogWarning("File Transformation plugin not found. Using fallback injection method.");
                JellyTweaks.Instance?.InjectScript();
            }
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers() => StartupServiceHelper.GetDefaultTriggers();
    }
}