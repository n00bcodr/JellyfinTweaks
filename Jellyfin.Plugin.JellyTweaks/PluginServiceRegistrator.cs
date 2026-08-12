using Jellyfin.Plugin.JellyTweaks.Services;
using MediaBrowser.Controller.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MediaBrowser.Controller;

namespace Jellyfin.Plugin.JellyTweaks
{
    public class PluginServiceRegistrator : IPluginServiceRegistrator
    {
        public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
        {
            serviceCollection.AddSingleton<StartupService>();

            // Request-time injection (Jellyfin 10.11 & 12): injects the loader
            // <script> into web index.html on every request via ASP.NET middleware.
            // Primary injection path -- see ScriptInjectionStartupFilter for details.
            // Kill-switchable via DisableScriptInjectionMiddleware, which falls back
            // to StartupService's File Transformation / on-disk write path.
            serviceCollection.AddSingleton<IStartupFilter, ScriptInjectionStartupFilter>();
        }
    }
}