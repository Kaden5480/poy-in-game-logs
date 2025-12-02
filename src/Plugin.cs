using BepInEx;
using UILib;
using UnityEngine;

using InGameLogs.Patches;

namespace InGameLogs {
    [BepInPlugin("com.github.Kaden5480.poy-in-game-logs", "In Game Logs", PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {
        private Logs logs;

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        private void Awake() {
            UIRoot.onInit.AddListener(() => {
                logs = new Logs();
            });

            Patcher.Patch();
        }

        /**
         * <summary>
         * Executes each frame.
         * </summary>
         */
        private void Update() {
            if (logs == null) {
                return;
            }

            if (InputOverlay.waitingForInput == true) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab) == true) {
                logs.Toggle();
            }
        }
    }
}
