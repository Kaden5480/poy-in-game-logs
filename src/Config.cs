using BepInEx.Configuration;
using ModMenu.Config;
using UnityEngine;

namespace InGameLogs {
    /**
     * <summary>
     * Holds the in game logs config.
     * </summary>
     */
    internal static class Config {
        [Field("Toggle Keybind")]
        internal static ConfigEntry<KeyCode> toggleKeybind;

        [Field("Max History", min=0)]
        [Listener(typeof(Logs), "SetMaxHistory")]
        internal static ConfigEntry<int> maxHistory;

        /**
         * <summary>
         * Initializes the config.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            toggleKeybind = configFile.Bind(
                "General", "toggleKeybind", KeyCode.Tab,
                "The keybind to toggle the UI."
            );
            maxHistory = configFile.Bind(
                "General", "maxHistory", 100,
                "The maximum history size per log level."
            );
        }
    }
}
