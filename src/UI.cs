using System;

using UILib;
using UILib.Components;
using UILib.Layouts;
using UIButton = UILib.Components.Button;
using UnityEngine;

namespace InGameLogs {
    internal class UI {
        private static UI instance;

        private Window window;

        // All of the history areas
        private QueueArea debugHistory;
        private QueueArea infoHistory;
        private QueueArea errorHistory;

        // The currently active area
        private QueueArea current;

        // Font size of the logs
        private int fontSize = 16;

        // How big the control area for
        // switching between histories should be
        private float switcherHeight = 80f;

        /**
         * <summary>
         * Initializes the log UI.
         * </summary>
         */
        internal UI() {
            instance = this;

            Theme theme = Theme.GetTheme();
            theme.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");

            window = new Window("Logs", 1000f, 700f);

            debugHistory = CreateHistory(theme, Config.UI.debugHistorySize.Value);
            infoHistory = CreateHistory(theme, Config.UI.infoHistorySize.Value);
            errorHistory = CreateHistory(theme, Config.UI.errorHistorySize.Value);

            window.Add(debugHistory);
            window.Add(infoHistory);
            window.Add(errorHistory);

            SetCurrentArea("Error", errorHistory);

            // Switcher
            Area switcher = new Area();
            switcher.SetAnchor(AnchorType.BottomMiddle);
            switcher.SetFill(FillType.Horizontal);
            switcher.SetSize(-20f, switcherHeight);
            switcher.SetOffset(-10f, 0f);
            window.AddDirect(switcher);

            // Fix scrollbar
            window.scrollView.scrollBarH.SetOffset(-10f, switcherHeight);

            Image switcherBg = new Image(theme.accent);
            switcherBg.SetFill(FillType.All);
            switcher.Add(switcherBg);

            // Controls
            Area controls = new Area();
            controls.SetContentLayout(LayoutType.Horizontal);
            controls.SetElementSpacing(20);
            switcher.Add(controls);

            controls.Add(MakeSwitchButton("Debug", debugHistory));
            controls.Add(MakeSwitchButton("Info", infoHistory));
            controls.Add(MakeSwitchButton("Error", errorHistory));

            // Register toggle shortcut
            Shortcut shortcut = new Shortcut(new[] { Config.UI.toggleKeybind });
            shortcut.onTrigger.AddListener(() => {
                window.ToggleVisibility();
            });
            UIRoot.AddShortcut(shortcut);
        }

        /**
         * <summary>
         * Updates the current history size.
         * </summary>
         * <param name="max">The maximum number of logs to retain</param>
         */
        internal static void SetDebugHistory(int max) {
            if (instance != null) {
                instance.debugHistory.SetLimit(max);
            }
        }

        /**
         * <summary>
         * Updates the current history size.
         * </summary>
         * <param name="max">The maximum number of logs to retain</param>
         */
        internal static void SetInfoHistory(int max) {
            if (instance != null) {
                instance.infoHistory.SetLimit(max);
            }
        }

        /**
         * <summary>
         * Updates the current history size.
         * </summary>
         * <param name="max">The maximum number of logs to retain</param>
         */
        internal static void SetErrorHistory(int max) {
            if (instance != null) {
                instance.errorHistory.SetLimit(max);
            }
        }

        /**
         * <summary>
         * Helper method for creating a button
         * for switching to different areas.
         * </summary>
         * <param name="name">The name of the area this button is for</param>
         * <param name="area">The area this button should switch to</param>
         */
        private UIButton MakeSwitchButton(string name, QueueArea area) {
            UIButton button = new UIButton(name, 20);
            button.SetSize(100f, 40f);
            button.onClick.AddListener(() => {
                SetCurrentArea(name, area);
            });
            return button;
        }

        /**
         * <summary>
         * Helper method for creating an area
         * for storing log history.
         * </summary>
         * <param name="theme">The theme to set on the area</param>
         * <param name="limit">The limit to apply to this history</param>
         */
        private QueueArea CreateHistory(Theme theme, int limit) {
            QueueArea area = new QueueArea(limit);
            area.SetContentLayout(LayoutType.Vertical);
            area.SetAnchor(AnchorType.BottomLeft);
            area.SetElementAlignment(AnchorType.BottomLeft);
            area.SetContentPadding(
                10, 10, 10, 20 + (int) switcherHeight
            );
            area.SetTheme(theme);
            area.Hide();

            return area;
        }

        /**
         * <summary>
         * Switches the window to display a different area.
         * </summary>
         * <param name="name">The name of the area (displayed in title bar)</param>
         * <param name="area">The area to switch to</param>
         */
        private void SetCurrentArea(string name, QueueArea area) {
            if (current != null) {
                current.Hide();
            }

            current = area;

            // Make sure the scroll view scrolls over
            // the area which was set
            window.scrollView.SetContent(current);

            current.Show();

            window.SetName($"{name} Logs");
        }

        /**
         * <summary>
         * Adds a log to one of the areas.
         * </summary>
         * <param name="area">The area to add to</param>
         * <param name="data">The data to add</param>
         */
        internal void AddLog(QueueArea area, object data) {
            if (Config.UI.enabled.Value == false) {
                return;
            }

            if (area == null) {
                return;
            }

            string log = data.ToString();
            string timeNow = Logs.timeNow;

            foreach (string line in log.Split(
                new [] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            )) {
                if (string.IsNullOrEmpty(line) == true) {
                    continue;
                }

                Label label = new Label($"[{timeNow}] {line.Trim()}", fontSize);
                label.SetSize(0f, fontSize+10);
                label.SetFill(FillType.Horizontal);

                // Make sure the label inherits the theme
                // (this is why `true` is used)
                area.Add(label, true);
            }
        }

        /**
         * <summary>
         * Adds a log to the debug area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddDebug(object data) {
            if (Config.UI.debugHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog(instance.debugHistory, data);
        }

        /**
         * <summary>
         * Adds a log to the info area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddInfo(object data) {
            if (Config.UI.infoHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog(instance.infoHistory, data);
        }

        /**
         * <summary>
         * Adds a log to the error area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddError(object data) {
            if (Config.UI.errorHistory.Value == false || instance == null) {
                return;
            }

            instance.AddLog(instance.errorHistory, data);
        }
    }
}
