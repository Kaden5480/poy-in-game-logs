using System;

using UILib;
using UILib.Components;
using UILib.Layouts;
using UIButton = UILib.Components.Button;
using UnityEngine;

namespace InGameLogs {
    internal class Logs {
        private static Logs instance;

        private Window window;

        // All of the history areas
        private QueueArea errorHistory;
        private QueueArea infoHistory;
        private QueueArea debugHistory;

        // The currently active area
        private QueueArea current;

        // Maximum number of logs to store
        // on each area
        private int logMaxSize = 100;

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
        internal Logs() {
            instance = this;

            Theme theme = new Theme();
            theme.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");

            window = new Window("Logs", 1000f, 700f);

            errorHistory = CreateHistory(theme);
            infoHistory = CreateHistory(theme);
            debugHistory = CreateHistory(theme);

            window.Add(errorHistory);
            window.Add(infoHistory);
            window.Add(debugHistory);

            SetCurrentArea("Error", errorHistory);

#region Switcher

            Area switcher = new Area();
            switcher.SetAnchor(AnchorType.BottomMiddle);
            switcher.SetFill(FillType.Horizontal);
            switcher.SetSize(-20f, switcherHeight);
            switcher.SetOffset(-10f, 0f);
            window.AddDirect(switcher);

            // Fix scrollbar
            window.scrollView.scrollBarH.SetOffset(-10f, switcherHeight);

            Image switcherBg = new Image(Colors.RGB(10, 10, 10));
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

#endregion

        }

        /**
         * <summary>
         * Toggles the visibility of the log window.
         * </summary>
         */
        internal void Toggle() {
            window.ToggleVisibility();
        }

#region History

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
         */
        private QueueArea CreateHistory(Theme theme) {
            QueueArea area = new QueueArea(logMaxSize);
            area.SetContentLayout(LayoutType.Vertical);
            area.SetAnchor(AnchorType.BottomLeft);
            area.SetElementAlignment(TextAnchor.LowerLeft);
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
            window.ScrollToBottom();
        }

#endregion

#region Logs

        /**
         * <summary>
         * Adds a log to one of the areas.
         * </summary>
         * <param name="area">The area to add to</param>
         * <param name="data">The data to add</param>
         */
        internal void AddLog(QueueArea area, object data) {
            if (area == null) {
                return;
            }

            string log = data.ToString();
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (string line in log.Split(
                new [] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            )) {
                if (string.IsNullOrEmpty(line) == true) {
                    continue;
                }

                Label label = new Label($"[{now}] {line.Trim()}", fontSize);
                label.SetSize(0f, fontSize+10);
                label.SetFill(FillType.Horizontal);

                // Make sure the label inherits the theme
                // (this is why `true` is used)
                area.Add(label, true);
            }
        }

        /**
         * <summary>
         * Adds a log to the error area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddError(object data) {
            if (instance == null) {
                return;
            }

            instance.AddLog(instance.errorHistory, data);
        }

        /**
         * <summary>
         * Adds a log to the info area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddInfo(object data) {
            if (instance == null) {
                return;
            }

            instance.AddLog(instance.infoHistory, data);
        }

        /**
         * <summary>
         * Adds a log to the debug area.
         * </summary>
         * <param name="data">The data to add</param>
         */
        internal static void AddDebug(object data) {
            if (instance == null) {
                return;
            }

            instance.AddLog(instance.debugHistory, data);
        }

#endregion

    }
}
