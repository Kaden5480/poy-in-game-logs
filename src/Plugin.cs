using System.Linq;

using BepInEx;
using HarmonyLib;
using ModMenu;
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

            // Initialize the config
            InGameLogs.Config.Init(this.Config);

            // Register with mod menu
            if (AccessTools.AllAssemblies().FirstOrDefault(
                    a => a.GetName().Name == "ModMenu"
                ) != null
            ) {
                Register();
            }

            // Build the UI
            UIRoot.onInit.AddListener(() => {
                new Logs();
            });
        }

        /**
         * <summary>
         * Registers with Mod Menu.
         * </summary>
         */
        private void Register() {
            ModInfo info = ModManager.Register(this);
            info.license = "GPL-3.0";
            info.Add(typeof(InGameLogs.Config));
        }
    }
}
