using Jellyfin.Plugin.JellyTweaks.Data;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JellyTweaks.Utils;

public static class TweakUtils
{
    public static async Task ApplyTweakAsync(ILogger logger, Tweak tweak, string value)
    {
        var isChanged = false;

        foreach (var file in tweak.Files)
        {
            var path = file.Path;
            if (!File.Exists(path))
            {
                logger.LogError($"Cannot find path {path} for apply {tweak.Name}!");
                return;
            }

            var originalContent = await File.ReadAllTextAsync(path);
            var content = originalContent;

            // Find and change values
            foreach (var values in file.SearchingValues)
            {
                var start = values.Start;
                var end = values.End;

                try
                {
                    var startIndex = content.IndexOf(start, StringComparison.Ordinal);
                    var endIndex = content.IndexOf(end, startIndex, StringComparison.Ordinal);

                    if (startIndex == -1 || endIndex == -1)
                    {
                        logger.LogError($"Cannot find values '{start}xxx{end}'!");
                        continue;
                    }

                    var original = content[startIndex..(endIndex + end.Length)];
                    var changed = $"{start}{value}{end}";

                    content = content.Replace(original, changed, StringComparison.Ordinal);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    logger.LogError($"Cannot find values '{start}xxx{end}'! | Error: {ex.Message}");
                }
            }

            // Continue if no modifications
            if (originalContent.Equals(content, StringComparison.Ordinal))
            {
                continue;
            }

            // Save changes to file
            try
            {
                await File.WriteAllTextAsync(path, content).ConfigureAwait(false);
                isChanged = true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Encountered exception while applying tweak {tweak.Name}: {ex}");
                return;
            }
        }

        logger.LogInformation(isChanged ? $"Finished applying tweak: {tweak.Name}!" : $"Tweak {tweak.Name} no need to apply!");
    }
}
