using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CurveDemo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default <see cref="Application"/> class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 


        public class DesktopIconInfo
        {
            public string Tag { get; set; }
            public string BgSource { get; set; }
            public string FgSource { get; set; }
            public int ColumnSpan { get; set; } = 1;
            public int RowSpan { get; set; } = 1;
            public string AppName { get; set; } = "";
        }

        public class OnRunningAppInfo
        {
            public string AppPackageName { get; set; }
            public int AppIconPos { get; set; }
            public Frame AppFrame { get; set; }
        }

        public ObservableCollection<OnRunningAppInfo> MultiAppInfos { get; set; } = new ObservableCollection<OnRunningAppInfo>();


        public ApplicationDataContainer LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public int EnableBgBlur = 1;
        public int EnableBgScale = 1;
        public double ScreenCornerRadius = 100.0 / 1920;
        public double TransitionDurationTime = 1.0;
        public double FlyFar = 5.0;
        public double BounceRadius = 6;
        public int EnableSideWindowAnimation = 1;
        public bool iUseCustomBackground = false;
        public int CurveStyle = 0;
        public int CtrPnelCurveStyle = 0;


        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;

            if (LocalSettings.Values["EnableBgBlur"] != null && ((int)LocalSettings.Values["EnableBgBlur"] == 1 || (int)LocalSettings.Values["EnableBgBlur"] == 0))
            {
                EnableBgBlur = (int)LocalSettings.Values["EnableBgBlur"];
            }
            if (LocalSettings.Values["EnableBgScale"] != null && ((int)LocalSettings.Values["EnableBgScale"] == 1 || (int)LocalSettings.Values["EnableBgScale"] == 0))
            {
                EnableBgScale = (int)LocalSettings.Values["EnableBgScale"];
            }
            if (LocalSettings.Values["EnableCornerRadius"] != null && ((bool)LocalSettings.Values["EnableCornerRadius"] == false))
            {
                ScreenCornerRadius = 0.000000001;
            }
            if(LocalSettings.Values["TransitionDurationTime"] != null && ((double)LocalSettings.Values["TransitionDurationTime"] >= 0.01))
            {
                TransitionDurationTime = (double)LocalSettings.Values["TransitionDurationTime"];
            }

            if (LocalSettings.Values["FlyFar"] != null && ((double)LocalSettings.Values["FlyFar"] >= 0))
            {
                FlyFar = (double)LocalSettings.Values["FlyFar"];
            }
            if (LocalSettings.Values["EnableSideWindowAnimation"] != null && ((int)LocalSettings.Values["EnableSideWindowAnimation"] == 1 || (int)LocalSettings.Values["EnableSideWindowAnimation"] == 0))
            {
                EnableSideWindowAnimation = (int)LocalSettings.Values["EnableSideWindowAnimation"];
            }
            if (LocalSettings.Values["BounceRadius"] != null && ((double)LocalSettings.Values["BounceRadius"] >= 0))
            {
                BounceRadius = (double)LocalSettings.Values["BounceRadius"];
            }
            if (LocalSettings.Values["iUseCustomBackground"] == null)
            {
                LocalSettings.Values["iUseCustomBackground"] = false;
            }
            else
            {
                iUseCustomBackground = (bool)LocalSettings.Values["iUseCustomBackground"];
            }

            if (LocalSettings.Values["CurveStyle"] != null && ((int)LocalSettings.Values["CurveStyle"] <= 1))
            {
                CurveStyle = (int)LocalSettings.Values["CurveStyle"];
            }
        }

        public Frame rootFrame;
        /// <inheritdoc/>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (Window.Current.Content is not Frame rootFrame)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page, configuring
                    // the new page by passing required information as a navigation parameter.
                    rootFrame.Navigate(typeof(SystemUI), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails.
        /// </summary>
        /// <param name="sender">The Frame which failed navigation.</param>
        /// <param name="e">Details about the navigation failure.</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load page '{e.SourcePageType.FullName}'.");
        }

        /// <summary>
        /// Invoked when application execution is being suspended. Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
