(function() {
    'use strict';

    const pluginId = 'dfee3828-01df-49df-85b1-5c2b75e5ea1a';
    let checkInterval;

    // Helper function to set a localStorage item
    function setStorageItem(userId, key, value) {
        localStorage.setItem(`${userId}-${key}`, value);
    }

    function runTweaks(userId) {
        console.log(`[JellyTweaks] User ID found: ${userId}. Applying settings...`);
        ApiClient.ajax({
            type: 'GET',
            url: ApiClient.getUrl('/JellyTweaks/public-config'),
            dataType: 'json'
        }).then(config => {
            console.log('[JellyTweaks] Fetched public configuration:', config);

            if (config.DefaultLibraryPageSize != null) {
                setStorageItem(userId, 'libraryPageSize', config.DefaultLibraryPageSize);
            }
            if (config.MaxDaysNextUp != null) {
                setStorageItem(userId, 'maxDaysForNextUp', config.MaxDaysNextUp);
            }
            // Tri-state: null means "unmanaged", leave it alone.
            if (config.EnableBackdropsByDefault != null) {
                setStorageItem(userId, 'enableBackdrops', config.EnableBackdropsByDefault);
            }
            if (config.EnableDetailsBannerByDefault != null) {
                setStorageItem(userId, 'detailsBanner', config.EnableDetailsBannerByDefault);
            }
            if (config.ForceEnableThemeMusic != null) {
                setStorageItem(userId, 'enableThemeSongs', config.ForceEnableThemeMusic);
            }
            if (config.ForceEnableThemeVideos != null) {
                setStorageItem(userId, 'enableThemeVideos', config.ForceEnableThemeVideos);
            }
            if (config.ForceDisableNextVideoInfo != null) {
                setStorageItem(userId, 'enableNextVideoInfoOverlay', !config.ForceDisableNextVideoInfo);
            }
            if (config.ForceEnableRewatchingInNextUp != null) {
                setStorageItem(userId, 'enableRewatchingInNextUp', config.ForceEnableRewatchingInNextUp);
            }
            if (config.ForceEnableEpisodeImagesInNextUp != null) {
                setStorageItem(userId, 'useEpisodeImagesInNextUpAndResume', config.ForceEnableEpisodeImagesInNextUp);
            }

            // Empty string means "unmanaged".
            if (config.DisplayMode) {
                localStorage.setItem('layout', config.DisplayMode);
            }
            if (config.MaxVideoWidth != null) {
                localStorage.setItem('maxVideoWidth', config.MaxVideoWidth);
            }
            localStorage.setItem('limitSupportedVideoResolution', config.LimitSupportedVideoResolution);

            // Device-scoped (no userId prefix), same as maxVideoWidth above.
            if (config.ForceEnablePgsSubtitleRendering != null) {
                localStorage.setItem('subtitlerenderpgs', config.ForceEnablePgsSubtitleRendering);
            }

            console.log('[JellyTweaks] Finished applying tweaks to localStorage.');

            applyForcedUserConfig(userId, config);
            applyForcedSortSettings(userId, config);
        }).catch(error => {
            console.error('[JellyTweaks] Failed to fetch public plugin configuration:', error);
        });
    }

    // Audio/subtitle defaults live server-side on the user's Configuration, so they're
    // pushed through the user-configuration API instead of localStorage.
    function applyForcedUserConfig(userId, config) {
        const hasAudioLanguage = !!config.ForceAudioLanguagePreference;
        const hasSubtitleLanguage = !!config.ForceSubtitleLanguagePreference;
        const hasSubtitleMode = !!config.ForceSubtitleMode;
        const hasPlayDefaultAudioTrack = config.ForcePlayDefaultAudioTrack != null;
        const hasRememberAudioSelections = config.ForceRememberAudioSelections != null;
        const hasRememberSubtitleSelections = config.ForceRememberSubtitleSelections != null;

        if (!hasAudioLanguage && !hasSubtitleLanguage && !hasSubtitleMode &&
            !hasPlayDefaultAudioTrack && !hasRememberAudioSelections && !hasRememberSubtitleSelections) {
            return;
        }

        ApiClient.getUser(userId).then(user => {
            const userConfig = user.Configuration || {};
            let changed = false;

            if (hasAudioLanguage && userConfig.AudioLanguagePreference !== config.ForceAudioLanguagePreference) {
                userConfig.AudioLanguagePreference = config.ForceAudioLanguagePreference;
                changed = true;
            }
            if (hasSubtitleLanguage && userConfig.SubtitleLanguagePreference !== config.ForceSubtitleLanguagePreference) {
                userConfig.SubtitleLanguagePreference = config.ForceSubtitleLanguagePreference;
                changed = true;
            }
            if (hasSubtitleMode && userConfig.SubtitleMode !== config.ForceSubtitleMode) {
                userConfig.SubtitleMode = config.ForceSubtitleMode;
                changed = true;
            }
            if (hasPlayDefaultAudioTrack && userConfig.PlayDefaultAudioTrack !== config.ForcePlayDefaultAudioTrack) {
                userConfig.PlayDefaultAudioTrack = config.ForcePlayDefaultAudioTrack;
                changed = true;
            }
            if (hasRememberAudioSelections && userConfig.RememberAudioSelections !== config.ForceRememberAudioSelections) {
                userConfig.RememberAudioSelections = config.ForceRememberAudioSelections;
                changed = true;
            }
            if (hasRememberSubtitleSelections && userConfig.RememberSubtitleSelections !== config.ForceRememberSubtitleSelections) {
                userConfig.RememberSubtitleSelections = config.ForceRememberSubtitleSelections;
                changed = true;
            }

            if (!changed) {
                return;
            }

            return ApiClient.updateUserConfiguration(userId, userConfig).then(() => {
                console.log('[JellyTweaks] Applied forced audio/subtitle defaults to user configuration.');
            });
        }).catch(error => {
            console.error('[JellyTweaks] Failed to apply forced user configuration:', error);
        });
    }

    // Sort storage differs by jellyfin-web app: Jellyfin 12's "modern" client keeps it
    // in localStorage, key `${LibraryTab} - ${libraryId}`, SortBy as an array (see
    // apps/modern/features/libraries/utils/settings.ts:getSettingsKey). The legacy
    // (10.11) client stores it server-side in each user's "usersettings"
    // DisplayPreferences, key `${libraryId}-${viewSuffix}`, SortBy as a comma-joined
    // string. Both writes below are harmless no-ops on the client that doesn't use
    // them, so both always run rather than detecting which app is active.
    const DISPLAY_PREFERENCES_ID = 'usersettings';
    const DISPLAY_PREFERENCES_CLIENT = 'emby';

    // Maps a stored ViewSuffix (legacy naming) to its Jellyfin 12 LibraryTab value.
    const VIEW_SUFFIX_TO_V12_TAB = {
        movies: 'movies',
        moviecollections: 'collections',
        series: 'series',
        musicalbums: 'albums',
        songs: 'songs'
    };

    function applyForcedSortSettings(userId, config) {
        console.log('[JellyTweaks:sort] Raw LibrarySortOverrides from config:', config.LibrarySortOverrides);

        const overrides = (config.LibrarySortOverrides || []).filter(o => o.SortBy || o.SortOrder);
        console.log('[JellyTweaks:sort] Overrides after filtering out empty entries:', overrides);

        if (overrides.length === 0) {
            console.log('[JellyTweaks:sort] Nothing to do -- no library has a SortBy/SortOrder configured. Check the Sorting tab on the config page.');
            return;
        }

        applyForcedSortToV12LocalStorage(overrides);
        applyForcedSortToLegacyDisplayPreferences(userId, overrides);
    }

    function applyForcedSortToV12LocalStorage(overrides) {
        overrides.forEach(override => {
            const tab = VIEW_SUFFIX_TO_V12_TAB[override.ViewSuffix];
            if (!tab) {
                console.warn('[JellyTweaks:sort] No v12 tab mapping for ViewSuffix "' + override.ViewSuffix + '" -- skipping localStorage write for this override.', override);
                return;
            }

            const key = `${tab} - ${override.LibraryId}`;
            const existingRaw = localStorage.getItem(key);
            console.log('[JellyTweaks:sort] v12 localStorage key "' + key + '" -- existing value:', existingRaw);

            let existing = null;
            try {
                existing = existingRaw ? JSON.parse(existingRaw) : null;
            } catch (e) {
                console.warn('[JellyTweaks:sort] Existing value for "' + key + '" was not valid JSON, discarding it:', existingRaw, e);
                existing = null;
            }

            // No prior entry -- seed full defaults (matches getDefaultLibraryViewSettings)
            // so the view never gets a partial settings object.
            const desired = existing || {
                ShowTitle: true,
                ShowYear: true,
                ViewMode: tab === 'songs' ? 'list' : 'grid',
                ImageType: 'Primary',
                CardLayout: false,
                StartIndex: 0
            };

            if (override.SortBy) {
                desired.SortBy = override.SortBy.split(',');
            }
            if (override.SortOrder) {
                desired.SortOrder = override.SortOrder;
            }

            const desiredRaw = JSON.stringify(desired);
            if (desiredRaw === existingRaw) {
                console.log('[JellyTweaks:sort] "' + key + '" already matches the desired value -- no write needed.');
                return;
            }

            localStorage.setItem(key, desiredRaw);
            const readBack = localStorage.getItem(key);
            console.log('[JellyTweaks:sort] Wrote "' + key + '":', desiredRaw, '-- read back immediately after write:', readBack, readBack === desiredRaw ? '(write confirmed)' : '(!!! WRITE DID NOT STICK !!!)');
        });
    }

    function applyForcedSortToLegacyDisplayPreferences(userId, overrides) {
        ApiClient.getDisplayPreferences(DISPLAY_PREFERENCES_ID, userId, DISPLAY_PREFERENCES_CLIENT).then(prefs => {
            console.log('[JellyTweaks:sort] Fetched legacy DisplayPreferences ("' + DISPLAY_PREFERENCES_ID + '"):', prefs);
            prefs.CustomPrefs = prefs.CustomPrefs || {};
            let changed = false;

            overrides.forEach(override => {
                const key = `${override.LibraryId}-${override.ViewSuffix}`;
                const existingRaw = prefs.CustomPrefs[key];
                let existing = {};
                try {
                    existing = existingRaw ? JSON.parse(existingRaw) : {};
                } catch (e) {
                    existing = {};
                }

                const desired = Object.assign({}, existing);
                if (override.SortBy) {
                    desired.SortBy = override.SortBy;
                }
                if (override.SortOrder) {
                    desired.SortOrder = override.SortOrder;
                }

                const desiredRaw = JSON.stringify(desired);
                console.log('[JellyTweaks:sort] Legacy CustomPrefs key "' + key + '" -- existing:', existingRaw, '-- desired:', desiredRaw);
                if (desiredRaw !== existingRaw) {
                    prefs.CustomPrefs[key] = desiredRaw;
                    changed = true;
                }
            });

            if (!changed) {
                console.log('[JellyTweaks:sort] Legacy DisplayPreferences already match -- no update sent.');
                return;
            }

            return ApiClient.updateDisplayPreferences(DISPLAY_PREFERENCES_ID, prefs, userId, DISPLAY_PREFERENCES_CLIENT).then(() => {
                console.log('[JellyTweaks:sort] Applied forced sort settings to legacy user display preferences.');
            });
        }).catch(error => {
            console.error('[JellyTweaks:sort] Failed to apply forced sort settings (legacy DisplayPreferences path):', error);
        });
    }

    function initialize() {
        checkInterval = setInterval(() => {
            if (typeof window.ApiClient?.getCurrentUserId === 'function') {
                const userId = window.ApiClient.getCurrentUserId();
                if (userId) {
                    clearInterval(checkInterval);
                    runTweaks(userId);
                }
            }
        }, 300);

        setTimeout(() => clearInterval(checkInterval), 30000);
    }

    initialize();

})();
