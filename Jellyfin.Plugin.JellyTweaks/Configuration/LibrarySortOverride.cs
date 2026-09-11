namespace Jellyfin.Plugin.JellyTweaks.Configuration;

public class LibrarySortOverride
{
    public string LibraryId { get; set; } = string.Empty;
    public string ViewSuffix { get; set; } = string.Empty;
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
}
