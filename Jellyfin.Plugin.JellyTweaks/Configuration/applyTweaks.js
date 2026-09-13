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

            if (config.DisableAllTweaks) {
                console.log('[JellyTweaks] Disabled via the master switch. Skipping.');
                return;
            }

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
            // NOTE: useEpisodeImagesInNextUpAndResume is intentionally NOT written to
            // localStorage here. jellyfin-web's userSettings.js reads/writes this one
            // specific setting with enableOnServer=true, meaning it comes from the
            // server-side "usersettings" DisplayPreferences (CustomPrefs), not
            // localStorage, so a localStorage write here would silently do nothing.
            // It's applied below in applyForcedDisplayPreferences() instead.

            // Playback Behavior (per-user localStorage, enableOnServer:false)
            if (config.ForceEnableCinemaMode != null) {
                setStorageItem(userId, 'enableCinemaMode', config.ForceEnableCinemaMode);
            }
            if (config.ForceEnableVideoRemainingTime != null) {
                setStorageItem(userId, 'enableVideoRemainingTime', config.ForceEnableVideoRemainingTime);
            }
            if (config.ForceEnableFastFadein != null) {
                setStorageItem(userId, 'fastFadein', config.ForceEnableFastFadein);
            }
            if (config.ForceEnableBlurhash != null) {
                setStorageItem(userId, 'blurhash', config.ForceEnableBlurhash);
            }
            if (config.ForceSelectAudioNormalization) {
                setStorageItem(userId, 'selectAudioNormalization', config.ForceSelectAudioNormalization);
            }
            if (config.ForceStillWatchingPrompt) {
                setStorageItem(userId, 'stillWatchingPrompt', config.ForceStillWatchingPrompt);
            }

            // Audio & Subtitle (additional; per-user localStorage)
            if (config.ForceAllowedAudioChannels) {
                setStorageItem(userId, 'allowedAudioChannels', config.ForceAllowedAudioChannels);
            }
            if (config.ForcePreferFmp4HlsContainer != null) {
                setStorageItem(userId, 'preferFmp4HlsContainer', config.ForcePreferFmp4HlsContainer);
            }
            if (config.ForceLimitSegmentLength != null) {
                setStorageItem(userId, 'limitSegmentLength', config.ForceLimitSegmentLength);
            }

            // Branding & Screensaver (per-user localStorage)
            if (config.ForceAppTheme) {
                setStorageItem(userId, 'appTheme', config.ForceAppTheme);
            }
            if (config.ForceScreensaver) {
                setStorageItem(userId, 'screensaver', config.ForceScreensaver);
            }
            if (config.ForceScreensaverTime != null) {
                setStorageItem(userId, 'screensaverTime', config.ForceScreensaverTime);
            }
            if (config.ForceBackdropScreensaverInterval != null) {
                setStorageItem(userId, 'backdropScreensaverInterval', config.ForceBackdropScreensaverInterval);
            }
            if (config.ForceSlideshowInterval != null) {
                setStorageItem(userId, 'slideshowInterval', config.ForceSlideshowInterval);
            }
            if (config.ForceLanguage) {
                setStorageItem(userId, 'language', config.ForceLanguage);
            }
            if (config.ForceDateTimeLocale) {
                setStorageItem(userId, 'datetimelocale', config.ForceDateTimeLocale);
            }
            if (config.ForceDisableCustomCss != null) {
                setStorageItem(userId, 'disableCustomCss', config.ForceDisableCustomCss);
            }
            if (config.ForceCustomCss) {
                setStorageItem(userId, 'customCss', config.ForceCustomCss);
            }

            // Empty string means "unmanaged".
            if (config.DisplayMode) {
                localStorage.setItem('layout', config.DisplayMode);
            }
            if (config.MaxVideoWidth != null) {
                localStorage.setItem('maxVideoWidth', config.MaxVideoWidth);
            }
            localStorage.setItem('limitSupportedVideoResolution', config.LimitSupportedVideoResolution);

            // Device Defaults (appSettings.js: plain localStorage, no userId prefix)
            if (config.ForceEnablePgsSubtitleRendering != null) {
                localStorage.setItem('subtitlerenderpgs', config.ForceEnablePgsSubtitleRendering);
            }
            if (config.ForceEnableAutoLogin != null) {
                localStorage.setItem('enableAutoLogin', config.ForceEnableAutoLogin);
            }
            if (config.ForceEnableGamepad != null) {
                localStorage.setItem('enableGamepad', config.ForceEnableGamepad);
            }
            if (config.ForceEnableSmoothScroll != null) {
                localStorage.setItem('enableSmoothScroll', config.ForceEnableSmoothScroll);
            }
            if (config.ForceEnableSystemExternalPlayers != null) {
                localStorage.setItem('enableSystemExternalPlayers', config.ForceEnableSystemExternalPlayers);
            }
            if (config.ForcePreferredTranscodeVideoCodec) {
                localStorage.setItem('preferredTranscodeVideoCodec', config.ForcePreferredTranscodeVideoCodec);
            }
            if (config.ForcePreferredTranscodeVideoAudioCodec) {
                localStorage.setItem('preferredTranscodeVideoAudioCodec', config.ForcePreferredTranscodeVideoAudioCodec);
            }
            if (config.ForceAlwaysBurnInSubtitleWhenTranscoding != null) {
                localStorage.setItem('alwaysBurnInSubtitleWhenTranscoding', config.ForceAlwaysBurnInSubtitleWhenTranscoding);
            }
            // subtitleburnin has no dedicated appSettings method; it's read/written as a raw
            // key by subtitlesettings.js (appSettings.get('subtitleburnin')). '' means "Auto".
            if (config.ForceSubtitleBurnInMode) {
                localStorage.setItem('subtitleburnin', config.ForceSubtitleBurnInMode);
            }
            if (config.ForceEnableDts != null) {
                localStorage.setItem('enableDts', config.ForceEnableDts);
            }
            if (config.ForceEnableTrueHd != null) {
                localStorage.setItem('enableTrueHd', config.ForceEnableTrueHd);
            }
            if (config.ForceEnableHi10p != null) {
                localStorage.setItem('enableHi10p', config.ForceEnableHi10p);
            }
            if (config.ForceDisableVbrAudio != null) {
                localStorage.setItem('disableVbrAudio', config.ForceDisableVbrAudio);
            }
            if (config.ForceAlwaysRemuxFlac != null) {
                localStorage.setItem('alwaysRemuxFlac', config.ForceAlwaysRemuxFlac);
            }
            if (config.ForceAlwaysRemuxMp3 != null) {
                localStorage.setItem('alwaysRemuxMp3', config.ForceAlwaysRemuxMp3);
            }
            if (config.ForceAspectRatio) {
                localStorage.setItem('aspectRatio', config.ForceAspectRatio);
            }
            if (config.ForceMaxStaticMusicBitrate != null) {
                localStorage.setItem('maxStaticMusicBitrate', config.ForceMaxStaticMusicBitrate);
            }
            if (config.ForceMaxChromecastBitrate != null) {
                // appSettings.js's own (historical) key name for this setting.
                localStorage.setItem('chromecastBitrate1', config.ForceMaxChromecastBitrate);
            }
            (config.BitrateOverrides || []).forEach(function (override) {
                if (!override.MediaType) {
                    return;
                }
                var suffix = override.MediaType + '-' + (override.InNetwork ? 'true' : 'false');
                if (override.AutoDetect != null) {
                    localStorage.setItem('enableautobitratebitrate-' + suffix, override.AutoDetect);
                }
                if (override.MaxBitrate != null) {
                    localStorage.setItem('maxbitrate-' + suffix, override.MaxBitrate);
                }
            });

            console.log('[JellyTweaks] Finished applying tweaks to localStorage.');

            applyForcedSubtitleAppearance(userId, config);
            applyForcedUserConfig(userId, config);
            applyForcedDisplayPreferences(userId, config);
        }).catch(error => {
            console.error('[JellyTweaks] Failed to fetch public plugin configuration:', error);
        });
    }

    // Subtitle appearance is a single JSON blob stored per-user in localStorage
    // (userSettings.js's getSubtitleAppearanceSettings/setSubtitleAppearanceSettings
    // both pass enableOnServer:false), keyed 'localplayersubtitleappearance3'.
    function applyForcedSubtitleAppearance(userId, config) {
        const override = config.SubtitleAppearance;
        if (!override || !override.Enabled) {
            return;
        }

        const blob = {};
        if (override.SubtitleStyling) blob.subtitleStyling = override.SubtitleStyling;
        if (override.TextSize != null) blob.textSize = override.TextSize;
        if (override.TextWeight) blob.textWeight = override.TextWeight;
        if (override.DropShadow != null) blob.dropShadow = override.DropShadow;
        if (override.Font != null) blob.font = override.Font;
        if (override.TextBackground) blob.textBackground = override.TextBackground;
        if (override.TextColor) blob.textColor = override.TextColor;
        if (override.VerticalPosition != null) blob.verticalPosition = override.VerticalPosition;
        if (override.AspectMode) blob.aspectMode = override.AspectMode;

        setStorageItem(userId, 'localplayersubtitleappearance3', JSON.stringify(blob));
        console.log('[JellyTweaks] Applied forced subtitle appearance settings:', blob);
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
        const hasEnableNextEpisodeAutoPlay = config.ForceEnableNextEpisodeAutoPlay != null;
        const hasHidePlayedInLatest = config.ForceHidePlayedInLatest != null;
        const hasDisplayMissingEpisodes = config.ForceDisplayMissingEpisodes != null;
        const hasEnableLocalPassword = config.ForceEnableLocalPassword != null;
        // Gated on the explicit "Manage*" flag, not list.length, since an empty list is
        // itself a valid forced value.
        const hasLatestItemsExcludes = !!config.ManageLatestItemsExcludes;
        const hasMyMediaExcludes = !!config.ManageMyMediaExcludes;
        const hasOrderedViews = !!config.ManageOrderedViews;
        const hasGroupedFolders = !!config.ManageGroupedFolders;

        if (!hasAudioLanguage && !hasSubtitleLanguage && !hasSubtitleMode &&
            !hasPlayDefaultAudioTrack && !hasRememberAudioSelections && !hasRememberSubtitleSelections &&
            !hasEnableNextEpisodeAutoPlay && !hasHidePlayedInLatest && !hasDisplayMissingEpisodes &&
            !hasEnableLocalPassword &&
            !hasLatestItemsExcludes && !hasMyMediaExcludes && !hasOrderedViews && !hasGroupedFolders) {
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
            if (hasEnableNextEpisodeAutoPlay && userConfig.EnableNextEpisodeAutoPlay !== config.ForceEnableNextEpisodeAutoPlay) {
                userConfig.EnableNextEpisodeAutoPlay = config.ForceEnableNextEpisodeAutoPlay;
                changed = true;
            }
            if (hasHidePlayedInLatest && userConfig.HidePlayedInLatest !== config.ForceHidePlayedInLatest) {
                userConfig.HidePlayedInLatest = config.ForceHidePlayedInLatest;
                changed = true;
            }
            if (hasDisplayMissingEpisodes && userConfig.DisplayMissingEpisodes !== config.ForceDisplayMissingEpisodes) {
                userConfig.DisplayMissingEpisodes = config.ForceDisplayMissingEpisodes;
                changed = true;
            }
            if (hasEnableLocalPassword && userConfig.EnableLocalPassword !== config.ForceEnableLocalPassword) {
                userConfig.EnableLocalPassword = config.ForceEnableLocalPassword;
                changed = true;
            }
            if (hasLatestItemsExcludes && JSON.stringify(userConfig.LatestItemsExcludes || []) !== JSON.stringify(config.LatestItemsExcludes || [])) {
                userConfig.LatestItemsExcludes = config.LatestItemsExcludes || [];
                changed = true;
            }
            if (hasMyMediaExcludes && JSON.stringify(userConfig.MyMediaExcludes || []) !== JSON.stringify(config.MyMediaExcludes || [])) {
                userConfig.MyMediaExcludes = config.MyMediaExcludes || [];
                changed = true;
            }
            if (hasOrderedViews && JSON.stringify(userConfig.OrderedViews || []) !== JSON.stringify(config.OrderedViews || [])) {
                userConfig.OrderedViews = config.OrderedViews || [];
                changed = true;
            }
            if (hasGroupedFolders && JSON.stringify(userConfig.GroupedFolders || []) !== JSON.stringify(config.GroupedFolders || [])) {
                userConfig.GroupedFolders = config.GroupedFolders || [];
                changed = true;
            }

            if (!changed) {
                return;
            }

            return ApiClient.updateUserConfiguration(userId, userConfig).then(() => {
                console.log('[JellyTweaks] Applied forced user configuration defaults.');
            });
        }).catch(error => {
            console.error('[JellyTweaks] Failed to apply forced user configuration:', error);
        });
    }

    // Sort storage differs by jellyfin-web app: Jellyfin 12's "modern" client keeps it in
    // localStorage (key `${LibraryTab} - ${libraryId}`, SortBy as an array), while the
    // legacy (10.11) client stores it server-side in "usersettings" DisplayPreferences
    // (key `${libraryId}-${viewSuffix}`, SortBy as a comma-joined string). Both writes run
    // unconditionally; the one the current client doesn't use is a harmless no-op.
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

    // The only 5 real "guide-indicator-*" DisplayPreferences keys. The guide's separate
    // "categories" filter (movies/kids/news/sports) is not one of these keys.
    const GUIDE_INDICATOR_TYPES = ['hd', 'live', 'new', 'premiere', 'repeat'];
    const GUIDE_INDICATOR_CONFIG_FIELDS = {
        hd: 'GuideIndicatorHd',
        live: 'GuideIndicatorLive',
        new: 'GuideIndicatorNew',
        premiere: 'GuideIndicatorPremiere',
        repeat: 'GuideIndicatorRepeat'
    };

    function applyForcedDisplayPreferences(userId, config) {
        console.log('[JellyTweaks:sort] Raw LibrarySortOverrides from config:', config.LibrarySortOverrides);

        const overrides = (config.LibrarySortOverrides || []).filter(o => o.SortBy || o.SortOrder);
        console.log('[JellyTweaks:sort] Overrides after filtering out empty entries:', overrides);

        // v12's "modern" client keeps sort in localStorage; unrelated to the legacy
        // DisplayPreferences fetch/write below, so it can run independently.
        applyForcedSortToV12LocalStorage(overrides);

        // These all live in the same "usersettings" DisplayPreferences document, so they're
        // combined into one fetch-modify-write to avoid two concurrent writes clobbering
        // each other.
        const guideIndicatorOverrides = {};
        GUIDE_INDICATOR_TYPES.forEach(function (type) {
            const value = config[GUIDE_INDICATOR_CONFIG_FIELDS[type]];
            if (value != null) {
                guideIndicatorOverrides[type] = value;
            }
        });

        // homesection0..9, keyed by slot index; empty entries are "don't manage".
        const homeSectionOverrides = {};
        (config.HomeSections || []).forEach(function (value, index) {
            if (value) {
                homeSectionOverrides[index] = value;
            }
        });

        // landing-{LibraryId}, one entry per configured library.
        const landingScreenOverrides = {};
        (config.LandingScreens || []).forEach(function (override) {
            if (override.LibraryId && override.Value) {
                landingScreenOverrides[override.LibraryId] = override.Value;
            }
        });

        const documentFields = {
            useEpisodeImagesInNextUpAndResume: config.ForceEnableEpisodeImagesInNextUp,
            skipBackLength: config.ForceSkipBackLength,
            skipForwardLength: config.ForceSkipForwardLength,
            dashboardTheme: config.ForceDashboardTheme,
            guideColorCodedBackgrounds: config.ForceGuideColorCodedBackgrounds,
            liveTvFavoriteChannelsAtTop: config.ForceLiveTvFavoriteChannelsAtTop,
            liveTvChannelOrder: config.ForceLiveTvChannelOrder,
            guideIndicators: guideIndicatorOverrides,
            homeSections: homeSectionOverrides,
            landingScreens: landingScreenOverrides
        };

        const hasAnyDocumentField = documentFields.useEpisodeImagesInNextUpAndResume != null ||
            documentFields.skipBackLength != null ||
            documentFields.skipForwardLength != null ||
            !!documentFields.dashboardTheme ||
            documentFields.guideColorCodedBackgrounds != null ||
            documentFields.liveTvFavoriteChannelsAtTop != null ||
            !!documentFields.liveTvChannelOrder ||
            Object.keys(guideIndicatorOverrides).length > 0 ||
            Object.keys(homeSectionOverrides).length > 0 ||
            Object.keys(landingScreenOverrides).length > 0;

        if (overrides.length === 0 && !hasAnyDocumentField) {
            console.log('[JellyTweaks:sort] Nothing to do for legacy DisplayPreferences: no sort overrides or other DisplayPreferences-backed settings are configured.');
            return;
        }

        applyForcedDisplayPreferencesDocument(userId, overrides, documentFields);
    }

    function applyForcedSortToV12LocalStorage(overrides) {
        overrides.forEach(override => {
            const tab = VIEW_SUFFIX_TO_V12_TAB[override.ViewSuffix];
            if (!tab) {
                console.warn('[JellyTweaks:sort] No v12 tab mapping for ViewSuffix "' + override.ViewSuffix + '", skipping localStorage write for this override.', override);
                return;
            }

            const key = `${tab} - ${override.LibraryId}`;
            const existingRaw = localStorage.getItem(key);
            console.log('[JellyTweaks:sort] v12 localStorage key "' + key + '", existing value:', existingRaw);

            let existing = null;
            try {
                existing = existingRaw ? JSON.parse(existingRaw) : null;
            } catch (e) {
                console.warn('[JellyTweaks:sort] Existing value for "' + key + '" was not valid JSON, discarding it:', existingRaw, e);
                existing = null;
            }

            // No prior entry: seed full defaults so the view never gets a partial settings object.
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
                console.log('[JellyTweaks:sort] "' + key + '" already matches the desired value, no write needed.');
                return;
            }

            localStorage.setItem(key, desiredRaw);
            const readBack = localStorage.getItem(key);
            console.log('[JellyTweaks:sort] Wrote "' + key + '":', desiredRaw, 'read back:', readBack, readBack === desiredRaw ? '(confirmed)' : '(write did not stick)');
        });
    }

    // Writes a single CustomPrefs[key] entry if it differs from what's already there.
    // Returns true if a write was staged (i.e. prefs.CustomPrefs was mutated).
    function stageCustomPref(prefs, key, desiredRaw) {
        const existingRaw = prefs.CustomPrefs[key];
        console.log('[JellyTweaks:sort] Legacy CustomPrefs key "' + key + '", existing:', existingRaw, 'desired:', desiredRaw);
        if (desiredRaw === existingRaw) {
            return false;
        }
        prefs.CustomPrefs[key] = desiredRaw;
        return true;
    }

    function applyForcedDisplayPreferencesDocument(userId, overrides, documentFields) {
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

                if (stageCustomPref(prefs, key, JSON.stringify(desired))) {
                    changed = true;
                }
            });

            // Booleans and numbers are stored as their raw .toString() form, matching how
            // userSettings.set() writes them.
            if (documentFields.useEpisodeImagesInNextUpAndResume != null && stageCustomPref(prefs, 'useEpisodeImagesInNextUpAndResume', String(documentFields.useEpisodeImagesInNextUpAndResume))) {
                changed = true;
            }
            if (documentFields.skipBackLength != null && stageCustomPref(prefs, 'skipBackLength', String(documentFields.skipBackLength))) {
                changed = true;
            }
            if (documentFields.skipForwardLength != null && stageCustomPref(prefs, 'skipForwardLength', String(documentFields.skipForwardLength))) {
                changed = true;
            }
            if (documentFields.dashboardTheme && stageCustomPref(prefs, 'dashboardTheme', documentFields.dashboardTheme)) {
                changed = true;
            }
            if (documentFields.guideColorCodedBackgrounds != null && stageCustomPref(prefs, 'guide-colorcodedbackgrounds', String(documentFields.guideColorCodedBackgrounds))) {
                changed = true;
            }
            if (documentFields.liveTvFavoriteChannelsAtTop != null && stageCustomPref(prefs, 'livetv-favoritechannelsattop', String(documentFields.liveTvFavoriteChannelsAtTop))) {
                changed = true;
            }
            if (documentFields.liveTvChannelOrder && stageCustomPref(prefs, 'livetv-channelorder', documentFields.liveTvChannelOrder)) {
                changed = true;
            }
            Object.keys(documentFields.guideIndicators || {}).forEach(function (type) {
                if (stageCustomPref(prefs, 'guide-indicator-' + type, String(documentFields.guideIndicators[type]))) {
                    changed = true;
                }
            });
            Object.keys(documentFields.homeSections || {}).forEach(function (index) {
                if (stageCustomPref(prefs, 'homesection' + index, documentFields.homeSections[index])) {
                    changed = true;
                }
            });
            Object.keys(documentFields.landingScreens || {}).forEach(function (libraryId) {
                if (stageCustomPref(prefs, 'landing-' + libraryId, documentFields.landingScreens[libraryId])) {
                    changed = true;
                }
            });

            if (!changed) {
                console.log('[JellyTweaks:sort] Legacy DisplayPreferences already match, no update sent.');
                return;
            }

            return ApiClient.updateDisplayPreferences(DISPLAY_PREFERENCES_ID, prefs, userId, DISPLAY_PREFERENCES_CLIENT).then(() => {
                console.log('[JellyTweaks:sort] Applied forced sort/display settings to legacy user display preferences.');
            });
        }).catch(error => {
            console.error('[JellyTweaks:sort] Failed to apply forced sort/display settings (legacy DisplayPreferences path):', error);
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
