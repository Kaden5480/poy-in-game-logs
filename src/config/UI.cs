using BepInEx.Configuration;
using ModMenu.Config;
using UnityEngine;

using UI_ = InGameLogs.UI;

namespace InGameLogs.Config {
    /**
     * <summary>
     * Holds the UI's config.
     * </summary>
     */
    internal static class UI {
        [Field("Toggle Keybind")]
        internal static ConfigEntry<KeyCode> toggleKeybind { get; private set; }

        [Field("Enabled")]
        internal static ConfigEntry<bool> enabled { get; private set; }

        [Field("Debug History")]
        internal static ConfigEntry<bool> debugHistory { get; private set; }

        [Field("Info History")]
        internal static ConfigEntry<bool> infoHistory { get; private set; }

        [Field("Error History")]
        internal static ConfigEntry<bool> errorHistory { get; private set; }

        [Listener(typeof(UI_), nameof(UI_.SetDebugHistory))]
        [Field("Debug History Size", min=0)]
        internal static ConfigEntry<int> debugHistorySize { get; private set; }

        [Listener(typeof(UI_), nameof(UI_.SetInfoHistory))]
        [Field("Info History Size", min=0)]
        internal static ConfigEntry<int> infoHistorySize { get; private set; }

        [Listener(typeof(UI_), nameof(UI_.SetErrorHistory))]
        [Field("Error History Size", min=0)]
        internal static ConfigEntry<int> errorHistorySize { get; private set; }

        /**
         * <summary>
         * Initializes the UI's config.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            toggleKeybind = configFile.Bind(
                "UI", "toggleKeybind", KeyCode.Tab,
                "The keybind to toggle the UI."
            );

            enabled = configFile.Bind(
                "UI", "enabled", true,
                "Whether the logging UI should track logs."
            );

            // History types
            debugHistory = configFile.Bind(
                "UI", "debugHistory", true,
                "Whether debug logs should be tracked by the UI."
            );

            infoHistory = configFile.Bind(
                "UI", "infoHistory", true,
                "Whether info logs should be tracked by the UI."
            );

            errorHistory = configFile.Bind(
                "UI", "errorHistory", true,
                "Whether error logs should be tracked by the UI."
            );

            // History size
            debugHistorySize = configFile.Bind(
                "UI", "debugHistorySize", 100,
                "The maximum debug history size in the UI."
            );

            infoHistorySize = configFile.Bind(
                "UI", "infoHistorySize", 100,
                "The maximum info history size in the UI."
            );

            errorHistorySize = configFile.Bind(
                "UI", "errorHistorySize", 100,
                "The maximum error history size in the UI."
            );
        }
    }
}
