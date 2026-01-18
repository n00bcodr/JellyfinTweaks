Jellyfin Tweaks - Jellyfin Plugin
====================

<p align="center">
  <img src="https://img.shields.io/github/last-commit/n00bcodr/JellyfinTweaks/main?logo=semantic-release&logoColor=white&label=Last%20Updated&labelColor=black&color=AA5CC3&cacheSeconds=3600" alt="Last Updated">
  <img src="https://img.shields.io/github/commit-activity/w/n00bcodr/JellyfinTweaks?logo=git&label=Commit%20Activity&labelColor=black&color=00A4DC&cacheSeconds=600" alt="Commit Activity">
  <img src="https://img.shields.io/badge/Jellyfin%20Version-10.10, 10.11-AA5CC3?logo=jellyfin&logoColor=00A4DC&labelColor=black" alt="Jellyfin Version">
  <br>  <br>
  <img alt="GitHub Downloads" src="https://img.shields.io/github/downloads/n00bcodr/JellyfinTweaks/latest/Jellyfin.Plugin.JellyfinTweaks_10.10.7.zip?displayAssetName=false&label=10.10%20Downloads%40Latest&labelColor=black&color=00A4DC&cacheSeconds=60">
  <img alt="GitHub Downloads" src="https://img.shields.io/github/downloads/n00bcodr/JellyfinTweaks/latest/Jellyfin.Plugin.JellyfinTweaks_10.11.0.zip?displayAssetName=false&label=10.11%20Downloads%40Latest&labelColor=black&color=AA5CC3&cacheSeconds=60">
  <br>  <br>
  <a href="https://discord.com/channels/1381737066366242896/1442128048873930762"><img alt="Discord" src="https://img.shields.io/badge/Jellyfin%20Enhanced%20-%20Jellyfin%20Community?&logo=discord&logoColor=white&style=for-the-badge&label=Jellyfin%20Community&labelColor=5865F2&color=black"></a>
  <br>  <br>
  <img alt="Logo" src="Jellyfin.Plugin.JellyTweaks\images\thumb.png" width="80%"  />
</p>
<br>


## About
A simple plugin that adds useful features, this is built on top of [gaam24/JellyTweaks](https://github.com/gaam24/JellyTweaks/) with more tweaks from [jellyfin-mods](https://github.com/BobHasNoSoul/jellyfin-mods)

## 🔧Tweaks
* Change default library page size
* Change Max Days for Next Up for all users
* Force Enable / Disable backdrops by default
* Force Enable / Disable Theme Music for all users
* Force Enable / Disable Theme Videos for all users
* Force Enable / Disable Rewatching in Next Up
* Force Enable / Disable Episode Images in Next Up
* and more

> [!Note]
> This plugin edits localStorage instead of editing index.html and main.jellyfin.bundle.js making it much safer and less intrusive!

~~<b>Warning</b>~~
* ~~Plugin edits "index.html" and "main.jellyfin.bundle.js" files, it is recommended to back up both files.~~
* ~~If user doesn't have the appropriate permissions, the plugin will not be able to change the settings.~~


## ⚙️ Installation

1.  In Jellyfin, go to **Dashboard** > **Plugins** > **Catalog** > ⚙️
2.  Click **➕** and give the repository a name (e.g., "Jellfin Tweaks Repo").
3.  Set the **Repository URL** to:

> [!IMPORTANT]
> **If you are on Jellyfin version 10.11**
> ``` 
> https://raw.githubusercontent.com/n00bcodr/jellyfin-plugins/main/10.11/manifest.json 
> ```
> If you are on 10.10.7
> ``` 
> https://raw.githubusercontent.com/n00bcodr/jellyfin-plugins/main/10.10/manifest.json 
> ```

4.  Click **Save**.
5.  Go to the **Catalog** tab, find **Jellfin Tweaks** in the list, and click **Install**.
6.  **Restart** your Jellyfin server to complete the installation.

#### 🐳 Docker Installation Notes

> [!IMPORTANT]
> If you are on a docker install it is highly advisable to have [file-transformation](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation) at least v2.2.1.0 installed. It helps avoid permission issues while modifying index.html


If you're running Jellyfin through Docker, the plugin may not have permission to modify jellyfin-web to inject the script. If you see permission errors such as `'System.UnauthorizedAccessException: Access to the path '/usr/share/jellyfin/web/index.html' is denied.` in your logs, you will need to map the `index.html` file manually:

1. Copy the index.html file from your container:

   ```bash
   docker cp jellyfin:/usr/share/jellyfin/web/index.html /path/to/your/jellyfin/config/index.html
   ```

2. Add a volume mapping to your Docker run command:

   ```yaml
   -v /path/to/your/jellyfin/config/index.html:/usr/share/jellyfin/web/index.html
   ```

3. Or for Docker Compose, add this to your volumes section:
   ```yaml
   services:
     jellyfin:
       # ... other config
       volumes:
         - /path/to/your/jellyfin/config:/config
         - /path/to/your/jellyfin/config/index.html:/usr/share/jellyfin/web/index.html
         # ... other volumes
   ```

This gives the plugin the necessary permissions to inject JavaScript into the web interface.

---

<div align="center">

**Made with 💜 for Jellyfin and the community**

### Enjoying Jellyfin Tweaks?

Checkout my other repos!

[Jellyfin-Enhanced](https://github.com/n00bcodr/Jellyfin-Enhanced) (javascript) • [Jellyfin-Elsewhere](https://github.com/n00bcodr/Jellyfin-Elsewhere) (javascript) • [Jellyfin-Tweaks](https://github.com/n00bcodr/JellyfinTweaks) (plugin) • [Jellyfin-JavaScript-Injector](https://github.com/n00bcodr/Jellyfin-JavaScript-Injector) (plugin) • [Jellyfish](https://github.com/n00bcodr/Jellyfish/) (theme)


</div>
