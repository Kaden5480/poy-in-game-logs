using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace InGameLogs.Patches {
    /**
     * <summary>
     * Applies a prefix patch to BepInEx's logger
     * to receive logs.
     * </summary>
     */
    internal static class LogHooks {
        /**
         * <summary>
         * Receives BepInEx logs.
         * </summary>
         * <param name="level">The log level of the log</param>
         * <param name="data">The data to log</param>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ManualLogSource), "Log")]
        private static void BepInLog(
            LogLevel level, object data
        ) {
            LogLevel l = level;

            // Error logs
            if (l.HasFlag(LogLevel.Fatal) == true
                || l.HasFlag(LogLevel.Error) == true
                || l.HasFlag(LogLevel.Warning) == true
            ) {
                Logs.AddError(data);
            }

            // Informational logs
            if (l.HasFlag(LogLevel.Message) == true
                || l.HasFlag(LogLevel.Info) == true
            ) {
                Logs.AddInfo(data);
            }

            // Debug logs
            if (l.HasFlag(LogLevel.Debug) == true) {
                Logs.AddDebug(data);
            }
        }
    }

    /**
     * <summary>
     * Adds a listener to receive Unity logs.
     * </summary>
     */
    internal static class UnityHooks {
        /**
         * <summary>
         * Adds the listener.
         * </summary>
         */
        internal static void Init() {
            Application.logMessageReceived += Handle;
        }

        /**
         * <summary>
         * Handles incoming logs.
         * </summary>
         * <param name="log">The log message</param>
         * <param name="stackTrace">The stack trace, if any</param>
         * <param name="logType">The type of log this is</param>
         */
        private static void Handle(
            string log,
            string stackTrace,
            LogType logType
        ) {
            // Stick everything into one big string
            string fullLog = log;
            if (string.IsNullOrEmpty(fullLog) == false) {
                fullLog = $"{fullLog}\n{stackTrace}";
            }

            switch (logType) {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Warning:
                case LogType.Exception:
                    Logs.AddError(fullLog);
                    break;

                case LogType.Log:
                    Logs.AddInfo(fullLog);
                    break;
            }
        }
    }
}
