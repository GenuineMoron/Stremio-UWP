using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                // args.Exception is non-null when the underlying browser process
                // failed to start — bail out so we don't crash on a null CoreWebView2.
                if (args.Exception != null || sender.CoreWebView2 == null) return;

                _ = sender.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(GamepadShimScript);
            };

            // ── Navigation complete ─────────────────────────────────────────────────
            WebView2.NavigationCompleted += (sender, args) =>
            {
                // Always dismiss the loading spinner first.
                LoadingOverlay.Visibility = Visibility.Collapsed;

                if (!args.IsSuccess)
                {
                    // Show the error overlay so the user knows what happened and can
                    // retry without relaunching the app.
                    ErrorOverlay.Visibility = Visibility.Visible;
                    return;
                }

                // Successful navigation — make sure the error overlay is gone.
                ErrorOverlay.Visibility = Visibility.Collapsed;

                // Belt-and-suspenders: also execute the shim immediately on the live
                // page in case CoreWebView2Initialized fired after the first navigation.
                _ = sender.ExecuteScriptAsync(GamepadShimScript);

                // Keep the Desktop title-bar back button in sync with web history so
                // the user always has a visible way to navigate backwards.
                if (DeviceForm == "Desktop")
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                        sender.CanGoBack
                            ? AppViewBackButtonVisibility.Visible
                            : AppViewBackButtonVisibility.Collapsed;
                }
            };

            // ── Back / B-button navigation ──────────────────────────────────────────
            // The Xbox B button (and Windows Back gesture / title-bar back button)
            // fires BackRequested.  If the web app has history to go back through,
            // do that; otherwise let the system handle it (shows the Xbox
            // "leave app?" prompt, or does nothing on Desktop).
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                if (WebView2.CanGoBack)
                {
                    WebView2.GoBack();
                    e.Handled = true;
                }
            };
        }

        // ── Retry handler ───────────────────────────────────────────────────────────
        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorOverlay.Visibility = Visibility.Collapsed;
            LoadingOverlay.Visibility = Visibility.Visible;
            WebView2.Reload();
        }
    }
}
