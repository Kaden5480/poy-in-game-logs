using System.IO;

using BepInEx;
using UnityEngine;

namespace InGameLogs {
    /**
     * <summary>
     * Holds the file logging source.
     * </summary>
     */
    internal class File {
        private static File instance;

        // Log location
        private string directory;
        private string filePath;

        // Writer
        private StreamWriter writer;

        /**
         * <summary>
         * Initializes the file logging source.
         * </summary>
         */
        internal File() {
            instance = this;

            directory = Path.Combine(
                Paths.GameRootPath, "InGameLogs"
            );

            MakeWriter();
        }

        /**
         * <summary>
         * Creates a file writer.
         * </summary>
         */
        private void MakeWriter() {
            // Make the directory if it doesn't exist
            if (Directory.Exists(directory) == false) {
                Directory.CreateDirectory(directory);
            }

            // Determine whether to use separate files
            if (Config.File.separateFiles.Value == true) {
                filePath = Path.Combine(
                    directory, $"{Logs.timeNowSafe}.log"
                );
            }
            else {
                filePath = Path.Combine(
                    directory, $"InGameLogs.log"
                );
            }

            // And create the writer
            writer = new StreamWriter(
                filePath, Config.File.appendLogs.Value
            ) {
                AutoFlush = true,
            };
        }

        /**
         * <summary>
         * Adds a log to the file.
         * </summary>
         * <param name="level">The log level</param>
         * <param name="data">The log data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        private void AddLog(string level, object data, bool bepin) {
            if (data == null) {
                return;
            }

            // Don't log if disabled
            if (Config.File.enabled.Value == false) {
                return;
            }

            // Ignore BepInEx logs
            if (Config.File.bepinLogs.Value == false && bepin == true) {
                return;
            }

            writer.WriteLine($"[{Logs.timeNow} | {level,5}] {data.ToString().Trim()}");
        }

        /**
         * <summary>
         * Adds a debug log.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddDebug(object data, bool bepin) {
            // Don't log if debug history is disabled
            if (Config.File.debugHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog("Debug", data, bepin);
        }

        /**
         * <summary>
         * Adds an info log.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddInfo(object data, bool bepin) {
            // Don't log if info history is disabled
            if (Config.File.infoHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog("Info", data, bepin);
        }

        /**
         * <summary>
         * Adds an error log.
         * </summary>
         * <param name="data">The data to add</param>
         * <param name="bepin">Whether the log originated from BepInEx</param>
         */
        internal static void AddError(object data, bool bepin) {
            // Don't log if error history is disabled
            if (Config.File.errorHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog("Error", data, bepin);
        }
    }
}
