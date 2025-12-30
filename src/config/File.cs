using BepInEx.Configuration;
using ModMenu.Config;

namespace InGameLogs.Config {
    /**
     * <summary>
     * Holds the file logging config.
     * </summary>
     */
    [Category("File Logging")]
    internal static class File {
        [Field("Enabled")]
        internal static ConfigEntry<bool> enabled { get; private set; }

        [Field("Separate Files")]
        internal static ConfigEntry<bool> separateFiles { get; private set; }

        [Field("Append Logs")]
        internal static ConfigEntry<bool> appendLogs { get; private set; }

        [Field("BepInEx Logs")]
        internal static ConfigEntry<bool> bepinLogs { get; private set; }

        [Field("Debug History")]
        internal static ConfigEntry<bool> debugHistory { get; private set; }

        [Field("Info History")]
        internal static ConfigEntry<bool> infoHistory { get; private set; }

        [Field("Error History")]
        internal static ConfigEntry<bool> errorHistory { get; private set; }

        /**
         * <summary>
         * Initializes the file logging config.
         * </summary>
         * <param name="configFile">The config file to bind to</param>
         */
        internal static void Init(ConfigFile configFile) {
            enabled = configFile.Bind(
                "File", "enabled", true,
                "Whether logging to a file should be enabled."
            );

            separateFiles = configFile.Bind(
                "File", "separateFiles", false,
                "Whether a separate file should be made for each game session."
            );

            appendLogs = configFile.Bind(
                "File", "appendLogs", false,
                "If `Separate Files` is disabled, whether logs should be appended to the log file."
                + " If this is disabled, the log file will be overwritten for each game session."
            );

            bepinLogs = configFile.Bind(
                "File", "bepinLogs", false,
                "Whether BepInEx logs should also be written to log files."
            );

            // Debug
            debugHistory = configFile.Bind(
                "File", "debugHistory", true,
                "Whether debug logs should be logged to a file."
            );

            // Info
            infoHistory = configFile.Bind(
                "File", "infoHistory", true,
                "Whether info logs should be logged to a file."
            );

            // Error
            errorHistory = configFile.Bind(
                "File", "errorHistory", true,
                "Whether error logs should be logged to a file."
            );
        }
    }
}
