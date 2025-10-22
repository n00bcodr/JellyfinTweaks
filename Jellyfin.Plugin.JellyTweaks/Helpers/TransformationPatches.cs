using Jellyfin.Plugin.JellyTweaks.Model;

namespace Jellyfin.Plugin.JellyTweaks.Helpers
{
    public static class TransformationPatches
    {
        public static string IndexHtml(PatchRequestPayload content)
        {
            // Return original content if it's null or empty to avoid issues.
            if (string.IsNullOrEmpty(content.Contents))
            {
                return content.Contents ?? string.Empty;
            }

            var pluginVersion = JellyTweaks.Instance?.Version.ToString() ?? "unknown";
            var scriptUrl = "JellyTweaks/script";
            var scriptTag = $"<script plugin=\"JellyTweaks\" version=\"{pluginVersion}\" src=\"{scriptUrl}\" defer></script>";

            if (content.Contents.Contains("</body>"))
            {
                return content.Contents.Replace("</body>", $"{scriptTag}</body>");
            }

            // Fallback in case </body> tag isn't found
            return content.Contents;
        }
    }
}