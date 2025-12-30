using BepInEx.Configuration;
using ModMenu.Config;

namespace InGameLogs.Config {
    /**
     * <summary>
     * Holds the general config.
     * </summary>
     */
    internal static class General {
        [Field("Enabled")]
        internal static ConfigEntry<bool> enabled { get; private set; }

        [Field("More Precision")]
        internal static ConfigEntry<bool> morePrecision { get; private set; }

        /**
         * <summary>
         * Initializes the general config.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            enabled = configFile.Bind(
                "General", "enabled", true,
                "Whether logging is enabled (controls both file and UI logging)."
            );

            morePrecision = configFile.Bind(
                "General", "morePrecision", false,
                "Whether logs should also include milliseconds."
            );
        }
    }
}
