using System.Linq;

using BepInEx;
using HarmonyLib;
using ModMenu;
using UILib;
using UnityEngine;

using InGameLogs.Patches;

namespace InGameLogs {
    [BepInDependency("com.github.Kaden5480.poy-ui-lib")]
    [BepInDependency(
        "com.github.Kaden5480.poy-mod-menu",
        BepInDependency.DependencyFlags.SoftDependency
    )]
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
            InGameLogs.Config.General.Init(this.Config);
            InGameLogs.Config.File.Init(this.Config);
            InGameLogs.Config.UI.Init(this.Config);

            // Start the file logger
            new File();

            // Register with mod menu
            if (AccessTools.AllAssemblies().FirstOrDefault(
                    a => a.GetName().Name == "ModMenu"
                ) != null
            ) {
                Register();
            }

            // Build the UI
            UIRoot.onInit.AddListener(() => {
                new UI();
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
            info.Add(typeof(InGameLogs.Config.General));
            info.Add(typeof(InGameLogs.Config.File));
            info.Add(typeof(InGameLogs.Config.UI));
        }
    }
}
