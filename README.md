Jellyfin Tweaks - Jellyfin Plugin
====================

<p align="center">
  <img src="https://img.shields.io/github/last-commit/n00bcodr/JellyfinTweaks/main?logo=semantic-release&logoColor=white&label=Last%20Updated&labelColor=black&color=AA5CC3&cacheSeconds=3600" alt="Last Updated">
  <img src="https://img.shields.io/github/commit-activity/w/n00bcodr/JellyfinTweaks?logo=git&label=Commit%20Activity&labelColor=black&color=00A4DC&cacheSeconds=600" alt="Commit Activity">
  <img src="https://img.shields.io/badge/Jellyfin%20Version-10.11, 12-AA5CC3?logo=jellyfin&logoColor=00A4DC&labelColor=black" alt="Jellyfin Version">
  <br>  <br>
  <img alt="GitHub Downloads" src="https://img.shields.io/github/downloads/n00bcodr/JellyfinTweaks/latest/Jellyfin.Plugin.JellyTweaks_10.11.0.zip?displayAssetName=false&label=10.11%20Downloads%40Latest&labelColor=black&color=00A4DC&cacheSeconds=60">
  <img alt="GitHub Downloads" src="https://img.shields.io/github/downloads/n00bcodr/JellyfinTweaks/latest/Jellyfin.Plugin.JellyTweaks_12.0.0.zip?displayAssetName=false&label=12%20Downloads%40Latest&labelColor=black&color=AA5CC3&cacheSeconds=60">
  <br>  <br>
  <a href="https://discord.com/channels/1381737066366242896/1442128048873930762"><img alt="Discord" src="https://img.shields.io/badge/Jellyfin%20Enhanced%20-%20Jellyfin%20Community?&logo=discord&logoColor=white&style=for-the-badge&label=Jellyfin%20Community&labelColor=5865F2&color=black"></a>
  <br>  <br>
  <img alt="Logo" src="Jellyfin.Plugin.JellyTweaks\images\thumb.png" width="80%"  />
</p>
<br>


## About
A simple plugin that adds useful features, this is built on top of [gaam24/JellyTweaks](https://github.com/gaam24/JellyTweaks/) with more tweaks from [jellyfin-mods](https://github.com/BobHasNoSoul/jellyfin-mods)

## 🔧Tweaks
* Change default library page size and Max Days in 'Next Up'
* Force enable/disable backdrops, details banner, theme songs, theme videos, next-video info during playback, rewatching in Next Up, and episode images in Next Up / Continue Watching
* Force a display mode, maximum video transcoding resolution, and experimental PGS/VobSub subtitle rendering
* Force default audio/subtitle language, subtitle playback mode, and remember-selection behavior
* Force a default sort order per library (Movies, TV Shows, Music), picked from your server's real libraries, no IDs to enter
* Every setting is opt-in: leave it on "Don't manage" and users keep control of it themselves

> [!Note]
> This plugin never edits index.html or main.jellyfin.bundle.js on disk. Settings are applied per user through a combination of localStorage and the Jellyfin API (for account-level preferences like audio/subtitle defaults), injected at request time instead of writing to files, making it much safer and less intrusive!


## ⚙️ Installation

1.  In Jellyfin, go to **Dashboard** > **Plugins** > **Repositories**.
2.  Click **➕** and add the repository:

> [!NOTE]
> ```
> https://raw.githubusercontent.com/n00bcodr/jellyfin-plugins/main/manifest.json
> ```

4.  Click **Save**.
5.  Go to the **Catalog** tab, find **Jellyfin Tweaks** in the list, and click **Install**.
6.  **Restart** your Jellyfin server to complete the installation.

---

<div align="center">

**Made with 💜 for Jellyfin and the community**

### Enjoying Jellyfin Tweaks?

Checkout my other repos!

[Jellyfin-Enhanced](https://github.com/n00bcodr/Jellyfin-Enhanced) (javascript) • [Jellyfin-Elsewhere](https://github.com/n00bcodr/Jellyfin-Elsewhere) (javascript) • [Jellyfin-Tweaks](https://github.com/n00bcodr/JellyfinTweaks) (plugin) • [Jellyfin-JavaScript-Injector](https://github.com/n00bcodr/Jellyfin-JavaScript-Injector) (plugin) • [Jellyfish](https://github.com/n00bcodr/Jellyfish/) (theme)


</div>
