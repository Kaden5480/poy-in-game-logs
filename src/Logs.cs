using System;

namespace InGameLogs {
    /**
     * <summary>
     * Handles adding logs to different sources.
     * </summary>
     */
    internal static class Logs {
        internal const string timeFormat = "yyyy-MM-dd HH:mm:ss";
        internal const string timeFormatSafe = "yyyy-MM-dd_HH-mm-ss";

        internal static string timeNow {
            get => DateTime.Now.ToString(timeFormat);
        }

        internal static string timeNowSafe {
            get => DateTime.Now.ToString(timeNowSafe);
        }

        /**
         * <summary>
         * Adds debug logs.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddDebug(object data, bool bepin = false) {
            if (Config.General.enabled.Value == false) {
                return;
            }

            UI.AddDebug(data);
            File.AddDebug(data, bepin);
        }

        /**
         * <summary>
         * Adds info logs.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddInfo(object data, bool bepin = false) {
            if (Config.General.enabled.Value == false) {
                return;
            }

            UI.AddInfo(data);
            File.AddInfo(data, bepin);
        }

        /**
         * <summary>
         * Adds error logs.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddError(object data, bool bepin = false) {
            if (Config.General.enabled.Value == false) {
                return;
            }

            UI.AddError(data);
            File.AddError(data, bepin);
        }
    }
}
