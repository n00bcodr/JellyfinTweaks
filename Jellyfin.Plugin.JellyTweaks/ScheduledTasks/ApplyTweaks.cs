using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.JellyTweaks.ScheduledTasks;

public class ApplyTweaks : IScheduledTask
{
    private readonly ILogger<ApplyTweaks> _logger;
    private readonly ILocalizationManager _localization;

    public ApplyTweaks(ILoggerFactory loggerFactory, ILocalizationManager localization)
    {
        _logger = loggerFactory.CreateLogger<ApplyTweaks>();
        _localization = localization;
    }

    /// <inheritdoc />
    public string Name => "Apply Tweaks";
    public string Key => "ApplyTweaks";
    public string Description => "Injects Jellyfin Tweaks script into the web client.";
    public string Category => _localization.GetLocalizedString("TasksLibraryCategory");

    public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var instance = JellyTweaks.Instance;
        if (instance == null)
        {
            _logger.LogError("JellyTweaks instance is not available.");
            return;
        }
        var indexPath = instance.IndexHtmlPath;
        if (string.IsNullOrEmpty(indexPath) || !File.Exists(indexPath))
        {
            _logger.LogError("Could not find index.html at path: {Path}", indexPath);
            return;
        }
        var scriptUrl = "/JellyTweaks/script";
        var scriptTag = $"<script defer src=\"{scriptUrl}\"></script>";

        try
        {
            var content = await File.ReadAllTextAsync(indexPath, cancellationToken);
            var regex = new Regex("<script.*(applyTweaks\\.js|JellyfinTweaksStatic|JellyTweaks/script).*</script>");

            if (regex.IsMatch(content))
            {
                 // If a correct or old version of the tag exists, remove it before adding the new one.
                 content = regex.Replace(content, string.Empty);
            }
            var closingBodyTag = "</body>";
            if (content.Contains(closingBodyTag))
            {
                content = content.Replace(closingBodyTag, $"{scriptTag}\n{closingBodyTag}");
                await File.WriteAllTextAsync(indexPath, content, cancellationToken);
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
