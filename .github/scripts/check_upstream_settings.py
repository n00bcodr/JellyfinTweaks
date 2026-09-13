#!/usr/bin/env python3
"""
Detects upstream jellyfin-web / jellyfin drift that would silently break this
plugin -- the exact class of bug hit once already (useEpisodeImagesInNextUpAndResume
was assumed to be a plain localStorage setting, but jellyfin-web actually reads/writes
it via server-side DisplayPreferences; the plugin's write was a silent no-op for
years).

This script fetches the upstream source files this plugin's applyTweaks.js depends
on, extracts the facts that matter (which localStorage/DisplayPreferences key each
setting uses, and whether jellyfin-web treats it as a plain per-user localStorage
value or a server-side DisplayPreferences document field), and diffs those facts
against a checked-in manifest describing what this plugin currently assumes.

Storage-bucket rule (see PluginConfiguration.cs / applyTweaks.js for the full
picture): userSettings.js's `get(name, enableOnServer)` reads a per-user
localStorage key ONLY when `enableOnServer` is the literal `false`. Omitted or
`true` means it reads/writes the server-side "usersettings" DisplayPreferences
document instead. That single rule is why this script buckets everything as
LOCAL vs DISPLAYPREFS vs DEVICE vs USERCONFIG.

No third-party dependencies -- stdlib only (urllib, re, json) -- so this runs on
a bare `ubuntu-latest` runner with no pip install step.
"""

import json
import re
import sys
import urllib.request
from pathlib import Path

RAW_BASE_WEB = "https://raw.githubusercontent.com/jellyfin/jellyfin-web/master"
RAW_BASE_SERVER = "https://raw.githubusercontent.com/jellyfin/jellyfin/master"

SOURCES = {
    "userSettings.js": f"{RAW_BASE_WEB}/src/scripts/settings/userSettings.js",
    "appSettings.js": f"{RAW_BASE_WEB}/src/scripts/settings/appSettings.js",
    "guide-settings.js": f"{RAW_BASE_WEB}/src/components/guide/guide-settings.js",
    "subtitlesettings.js": f"{RAW_BASE_WEB}/src/components/subtitlesettings/subtitlesettings.js",
    "UserConfiguration.cs": f"{RAW_BASE_SERVER}/MediaBrowser.Model/Configuration/UserConfiguration.cs",
}

MANIFEST_PATH = Path(__file__).resolve().parent.parent / "upstream-snapshots" / "expected-settings-manifest.json"

# Matches `this.get('key')` / `this.get('key', true)` / `this.get('key', false)` --
# the literal third argument (or its absence) is what decides LOCAL vs DISPLAYPREFS.
USERSETTINGS_GET_RE = re.compile(r"""\.get\(\s*['"]([\w\-]+)['"]\s*(?:,\s*(true|false))?\s*\)""")

# Matches any literal-keyed appSettings/userSettings get/set call, e.g.
# `appSettings.get('subtitleburnin')` or `userSettings.set('guide-colorcodedbackgrounds', ...)`.
# Dynamic keys built via string concatenation (bitrate grid, guide-indicator-<type>)
# don't match this -- they're checked separately via KNOWN_DYNAMIC_PATTERNS below.
LITERAL_KEY_CALL_RE = re.compile(r"""(?:this|appSettings|userSettings)\.(?:get|set)\(\s*['"]([\w\-]+)['"]""")

# Substrings that must still be present verbatim for a *dynamically constructed* key
# to be considered unchanged (can't be captured by a key-literal regex).
KNOWN_DYNAMIC_PATTERNS = {
    "appSettings.js: bitrate auto-detect key template": (
        "appSettings.js",
        "'enableautobitratebitrate-' + mediaType + '-' + isInNetwork",
    ),
    "appSettings.js: max bitrate key template": (
        "appSettings.js",
        "'maxbitrate-' + mediaType + '-' + isInNetwork",
    ),
    "guide-settings.js: guide indicator key template": (
        "guide-settings.js",
        "'guide-indicator-' + type",
    ),
    "subtitlesettings.js: subtitle appearance object includes aspectMode": (
        "subtitlesettings.js",
        "aspectMode: context.querySelector('#selectBitmapSubtitleAspectMode').value",
    ),
    "userSettings.js: subtitle appearance key": (
        "userSettings.js",
        "key = key || 'localplayersubtitleappearance3'",
    ),
    "userSettings.js: subtitle appearance is still LOCAL (enableOnServer:false)": (
        "userSettings.js",
        "this.get(key, false) || '{}'",
    ),
}

USERCONFIG_PROPERTY_RE = re.compile(r"""public\s+[\w<>\[\],? ]+?\s+(\w+)\s*{\s*get;\s*set;\s*}""")


def fetch(url: str) -> str:
    with urllib.request.urlopen(url, timeout=30) as response:  # noqa: S310 (public, read-only, pinned hosts)
        return response.read().decode("utf-8")


def extract_userSettings_buckets(content: str) -> dict[str, str]:
    buckets: dict[str, str] = {}
    for key, enable_on_server_literal in USERSETTINGS_GET_RE.findall(content):
        bucket = "LOCAL" if enable_on_server_literal == "false" else "DISPLAYPREFS"
        # A key can appear more than once (get + set, or reused across methods); if
        # any occurrence disagrees on bucket, that's itself worth flagging loudly.
        if key in buckets and buckets[key] != bucket:
            buckets[key] = "AMBIGUOUS"
        else:
            buckets.setdefault(key, bucket)
    return buckets


def extract_literal_keys(content: str) -> set[str]:
    return set(LITERAL_KEY_CALL_RE.findall(content))


def extract_userconfig_properties(content: str) -> set[str]:
    return set(USERCONFIG_PROPERTY_RE.findall(content))


def main() -> int:
    manifest = json.loads(MANIFEST_PATH.read_text())

    print("Fetching upstream source files...")
    raw_sources = {}
    try:
        for name, url in SOURCES.items():
            raw_sources[name] = fetch(url)
            print(f"  OK  {name} ({len(raw_sources[name])} bytes)")
    except Exception as exc:  # noqa: BLE001
        print(f"::error::Failed to fetch upstream source: {exc}")
        return 2

    user_settings_buckets = extract_userSettings_buckets(raw_sources["userSettings.js"])
    guide_settings_buckets = {k: "DISPLAYPREFS" for k in extract_literal_keys(raw_sources["guide-settings.js"])}
    app_settings_keys = extract_literal_keys(raw_sources["appSettings.js"])
    subtitle_settings_keys = extract_literal_keys(raw_sources["subtitlesettings.js"])
    userconfig_properties = extract_userconfig_properties(raw_sources["UserConfiguration.cs"])

    # DISPLAYPREFS keys can come from either userSettings.js's own methods or the
    # ad-hoc keys guide-settings.js writes through the same exported get/set.
    displayprefs_buckets = {**user_settings_buckets, **guide_settings_buckets}

    failures: list[str] = []
    informational: list[str] = []

    for entry in manifest["settings"]:
        name = entry["name"]
        key = entry["key"]
        expected_bucket = entry["storage"]

        if expected_bucket in ("LOCAL", "DISPLAYPREFS"):
            actual_bucket = displayprefs_buckets.get(key)
            if actual_bucket is None:
                failures.append(f"[{name}] key '{key}' no longer found in userSettings.js/guide-settings.js")
            elif actual_bucket == "AMBIGUOUS":
                failures.append(f"[{name}] key '{key}' has inconsistent enableOnServer usage across call sites upstream")
            elif actual_bucket != expected_bucket:
                failures.append(
                    f"[{name}] key '{key}' storage changed: plugin assumes {expected_bucket}, "
                    f"upstream now resolves to {actual_bucket} -- this is the exact bug class that broke "
                    f"useEpisodeImagesInNextUpAndResume; the plugin's write is likely now a silent no-op"
                )
        elif expected_bucket == "DEVICE":
            if key not in app_settings_keys and key not in subtitle_settings_keys:
                failures.append(f"[{name}] device-scoped key '{key}' no longer found in appSettings.js/subtitlesettings.js")
        elif expected_bucket == "USERCONFIG":
            if key not in userconfig_properties:
                failures.append(f"[{name}] UserConfiguration property '{key}' no longer found in UserConfiguration.cs")
        else:
            failures.append(f"[{name}] unknown expected storage bucket '{expected_bucket}' in manifest. Fix the manifest.")

    for description, (source, needle) in KNOWN_DYNAMIC_PATTERNS.items():
        if needle not in raw_sources[source]:
            failures.append(f"[dynamic pattern] {description}: exact snippet no longer found in {source}. Review manually.")

    # Informational only: settings that exist upstream but aren't in our manifest yet.
    # Keys we've deliberately decided not to track (see ignoredUnmanifestedKeys in the
    # manifest) are excluded so they don't re-flag on every run.
    known_keys = {e["key"] for e in manifest["settings"]}
    ignored_keys = set(manifest.get("ignoredUnmanifestedKeys", {}).get("keys", []))
    for key in sorted(set(user_settings_buckets) - known_keys - ignored_keys):
        informational.append(f"userSettings.js has an unmanifested key '{key}' (bucket {user_settings_buckets[key]}). Possible new tweak candidate.")

    print("\n--- Drift check results ---")
    if failures:
        print(f"\n{len(failures)} problem(s) found:\n")
        for f in failures:
            print(f"::error::{f}")
    else:
        print("\nNo drift detected against the checked-in manifest.")

    if informational:
        print(f"\n{len(informational)} informational note(s):\n")
        for note in informational:
            print(f"::notice::{note}")

    summary_path = __import__("os").environ.get("GITHUB_STEP_SUMMARY")
    if summary_path:
        with open(summary_path, "a", encoding="utf-8") as f:
            f.write("## Upstream settings drift check\n\n")
            if failures:
                f.write(f"**{len(failures)} problem(s) found:**\n\n")
                for item in failures:
                    f.write(f"- {item}\n")
            else:
                f.write("No drift detected against the checked-in manifest.\n")
            if informational:
                f.write(f"\n**{len(informational)} informational note(s):**\n\n")
                for item in informational:
                    f.write(f"- {item}\n")

    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main())
