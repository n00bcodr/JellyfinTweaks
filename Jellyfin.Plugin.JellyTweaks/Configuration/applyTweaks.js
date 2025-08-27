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
            setStorageItem(userId, 'enableBackdrops', config.EnableBackdropsByDefault);
            setStorageItem(userId, 'detailsBanner', config.EnableDetailsBannerByDefault);
            setStorageItem(userId, 'enableThemeSongs', config.ForceEnableThemeMusic);
            setStorageItem(userId, 'enableThemeVideos', config.ForceEnableThemeVideos);
            setStorageItem(userId, 'enableNextVideoInfoOverlay', !config.ForceDisableNextVideoInfo);
            setStorageItem(userId, 'enableRewatchingInNextUp', config.ForceEnableRewatchingInNextUp);
            setStorageItem(userId, 'useEpisodeImagesInNextUpAndResume', config.ForceEnableEpisodeImagesInNextUp);

            console.log('[JellyTweaks] Finished applying tweaks to localStorage.');
        }).catch(error => {
            console.error('[JellyTweaks] Failed to fetch public plugin configuration:', error);
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
