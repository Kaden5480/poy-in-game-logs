using HarmonyLib;

namespace InGameLogs.Patches {
    /**
     * <summary>
     * A class for applying patches to
     * get things working.
     * </summary>
     */
    internal static class Patcher {
        /**
         * <summary>
         * Applies patches.
         * </summary>
         */
        internal static void Patch() {
            Harmony.CreateAndPatchAll(typeof(LogHooks));
            UnityHooks.Init();
        }
    }
}
