using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Stremio
{
    /// <summary>
    /// Core display page. Hosts the Stremio web app in a WebView2 control.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Detect Xbox at startup — drives all platform-specific behaviour below.
        public static string DeviceForm { get; } =
            Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox"
                ? "Xbox" : "Desktop";

        // Script injected into every page the WebView2 loads.
        // On Xbox, navigator.gamepadInputEmulation = "keyboard" makes the D-pad emit
        // arrow-key events and the A/B buttons emit Enter/Escape, so the Stremio web
        // UI (which is fully keyboard-navigable) works with the controller.
        private const string GamepadShimScript =
            "if (typeof navigator !== 'undefined' && 'gamepadInputEmulation' in navigator) {" +
            "    navigator.gamepadInputEmulation = 'keyboard';" +
            "}";

        public MainPage()
        {
            // Set WebView2 background colour BEFORE the control is created so there
            // is no white flash during startup. Format must be ARGB hex (8 chars).
            Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "ff151330");

            this.InitializeComponent();

            if (DeviceForm == "Xbox")
            {
                // At Xbox's default 200 % UI scaling, the mouse pointer is confined to
                // the top-left 25 % of the WebView2 area (confirmed MS bug #4133).
                // Disabling layout scaling drops to 100 % and the pointer reaches the
                // full 1920×1080 viewport — the documented workaround.
                Windows.UI.ViewManagement.ApplicationViewScaling.TrySetDisableLayoutScaling(true);

                // Extend the app content behind the title bar so the WebView2 fills
                // the entire screen (TV / 10-foot UX).
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView()
                    .SetDesiredBoundsMode(
                        Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
            }

            // ── WebView2 initialised ────────────────────────────────────────────────
            // AddScriptToExecuteOnDocumentCreatedAsync persists across all navigations,
            // so the gamepad shim is present even if the SPA hard-navigates internally.
            WebView2.CoreWebView2Initialized += (sender, args) =>
            {
                if (sender.CoreWebView2 == null) return;
                _ = sender.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(GamepadShimScript);
            };

            // ── Navigation complete ─────────────────────────────────────────────────
            WebView2.NavigationCompleted += (sender, args) =>
            {
                // Dismiss the loading overlay once the first page finishes loading.
                LoadingOverlay.Visibility = Visibility.Collapsed;

                // Belt-and-suspenders: also execute the shim immediately on the live
                // page in case CoreWebView2Initialized fired after the first navigation.
                _ = sender.ExecuteScriptAsync(GamepadShimScript);
            };

            // ── Back / B-button navigation ──────────────────────────────────────────
            // The Xbox B button (and Windows Back gesture) fires BackRequested.
            // If the web app has history to go back through, do that; otherwise let
            // the system handle it (shows the Xbox "leave app?" prompt).
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested +=
                (s, e) =>
                {
                    if (WebView2.CanGoBack)
                    {
                        WebView2.GoBack();
                        e.Handled = true;
                    }
                };
        }
    }
}
