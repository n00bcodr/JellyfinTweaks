Jellyfin Tweaks - Jellyfin Plugin
====================

<p align="center">
  <img src="https://img.shields.io/badge/Jellyfin%20Version-10.10.x-AA5CC3?logo=jellyfin&logoColor=00A4DC&labelColor=black" alt="Jellyfin Version">
  <br/><br/>
    <img alt="Logo" src="Jellyfin.Plugin.JellyTweaks\images\thumb.png" width="80%"  />
<br>
</p>


## About
A simple plugin that adds useful features, this is built on top of [gaam24/JellyTweaks](https://github.com/gaam24/JellyTweaks/) with more tweaks from [jellyfin-mods](https://github.com/BobHasNoSoul/jellyfin-mods)

## Tweaks
* Change default library page size
* Change Max Days for Next Up for all users
* Force Enable / Disable backdrops by default
* Force Enable / Disable Theme Music for all users
* Force Enable / Disable Theme Videos for all users
* Force Enable / Disable Rewatching in Next Up
* Force Enable / Disable Episode Images in Next Up


> [!Note]
> This plugin edits localStorage instead of editing index.html and main.jellyfin.bundle.js making it much safer and less intrusive!

~~<b>Warning</b>~~
* ~~Plugin edits "index.html" and "main.jellyfin.bundle.js" files, it is recommended to back up both files.~~
* ~~If user doesn't have the appropriate permissions, the plugin will not be able to change the settings.~~


## Installation
1. Add "https://raw.githubusercontent.com/n00bcodr/JellyfinTweaks/main/manifest.json" to Jellyfin plugin repository
2. Install JellyfinTweaks from the repository
3. Restart the Jellyfin server
