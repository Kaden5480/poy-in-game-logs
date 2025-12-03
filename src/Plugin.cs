using BepInEx;
using UILib;
using UnityEngine;

using InGameLogs.Patches;

namespace InGameLogs {
    [BepInPlugin("com.github.Kaden5480.poy-in-game-logs", "In Game Logs", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        private void Awake() {
            Patcher.Patch();

            UIRoot.onInit.AddListener(() => {
                new Logs();
            });
        }
    }
}
