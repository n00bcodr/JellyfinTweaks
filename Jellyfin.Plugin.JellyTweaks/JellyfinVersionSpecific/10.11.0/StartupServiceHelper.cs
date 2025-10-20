using MediaBrowser.Model.Tasks;
using System.Collections.Generic;

namespace Jellyfin.Plugin.JellyTweaks.JellyfinVersionSpecific
{
    public static class StartupServiceHelper
    {
        public static IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            // Trigger at startup
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfoType.StartupTrigger
            };

            // Trigger every 12 hours
            yield return new TaskTriggerInfo
            {
                Type = TaskTriggerInfoType.DailyTrigger,
                TimeOfDayTicks = System.TimeSpan.FromHours(12).Ticks
            };
        }
    }
}