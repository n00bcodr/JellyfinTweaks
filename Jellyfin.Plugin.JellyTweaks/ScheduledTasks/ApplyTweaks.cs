using Jellyfin.Plugin.JellyTweaks.Data;
using Jellyfin.Plugin.JellyTweaks.Tweaks;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.ScheduledTasks;

public class ApplyTweaks(ILoggerFactory loggerFactory, ILocalizationManager localization) : IScheduledTask
{
    /// <inheritdoc />
    public string Name => "Apply Tweaks";

    /// <inheritdoc />
    public string Key => "ApplyTweaks";

    /// <inheritdoc />
    public string Description => "Apply tweaks enabled in settings.";

    /// <inheritdoc />
    public string Category => localization.GetLocalizedString("TasksLibraryCategory");

    public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger<Tweak>();
        var config = JellyTweaks.Instance!.Configuration;

        if (config == null)
        {
            throw new InvalidOperationException("Configuration cannot be null");
        }

        await new EnableBackdropsByDefault(logger).Execute(config);
        await new DefaultMaxPage(logger).Execute(config);
        await new DefaultTitle(logger).Execute(config);
        await new MaxDaysNextUpTweak(logger).Execute(config);
        await new DisableNextVideoInfoTweak(logger).Execute(config);
        await new ForceThemeMusicTweak(logger).Execute(config);

        cancellationToken.ThrowIfCancellationRequested();
    }

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        return
        [
            new TaskTriggerInfo { Type = TaskTriggerInfo.TriggerStartup }
        ];
    }
}
