using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI;
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

        public class AppAnimationRectProperties
        {
            public bool anStatus { get; set; } = false;
            public int anDirection { get; set; } = 1;
            public double XDamping { get; set; } = 1.0;
            public double YDamping { get; set; } = 1.0;
            public double WidthDamping { get; set; } = 1;
            public double HeightDamping { get; set; } = 1;
            public double ScaleDamping { get; set; } = 1.0;
            public double OpacityDamping { get; set; } = 1.0;
            public double RotationDamping { get; set; } = 1.0;
            public double CornerRadiusDamping { get; set; } = 1.0;

            public long startTick { get; set; } = 0;
            public long durationTick { get; set; } = 1;
            public long OpacityBeginTick { get; set; } = 0;
            public long OpacityDurationTick { get; set; } = 1;

            public long VisibleDurationTick { get; set; } = 1;

            public double FromX { get; set; } = 0;
            public double FromY { get; set; } = 0;
            public double FromWidth { get; set; } = 0;
            public double FromHeight { get; set; } = 0;
            public double FromScale { get; set; } = 0;
            public double FromOpacity { get; set; } = 0;
            public double FromRotaion { get; set; } = 0;
            public double FromCornerRadius { get; set; } = 0;

            public double ToX { get; set; } = 0;
            public double ToY { get; set; } = 0;
            public double ToWidth { get; set; } = 0;
            public double ToHeight { get; set; } = 0;
            public double ToScale { get; set; } = 0;
            public double ToOpacity { get; set; } = 0;
            public double ToRotaion { get; set; } = 0;
            public double ToCornerRadius { get; set; } = 0;

            public double VX { get; set; } = 0;
            public double VY { get; set; } = 0;
            public double VWidth { get; set; } = 0;
            public double VHeight { get; set; } = 0;
            public double VScale { get; set; } = 0;
            public double VOpacity { get; set; } = 0;
            public double VRotaion { get; set; } = 0;
            public double VCornerRadius { get; set; } = 0;
        }
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
        public int NotificationCenterAlignment = 0;
        public bool CombineControlCenterWhenWide = false;

        public double DurationTime = 1.1, XDamping = 0.82, YDamping = 0.82, WidthDamping = 0.9, HeightDamping = 0.9, ScaleDamping = 0.82, OpacityDamping = 1.0, RotationDamping = 0.9, CornerRadiusDamping = 1.0, OpacityBeginTime = 0.1, OpacityDurationTime = 0.3;
        public double XVelocity = 1, YVelocity = 1, ScaleVelocity = 1;

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;

            
        }

        public Frame RootFrame;
        /// <inheritdoc/>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            RootFrame = Window.Current.Content as Frame;

            if (RootFrame == null)
            {
                RootFrame = new Frame();
                RootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {

                }
                Window.Current.Content = RootFrame;
            }
            /*
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (Window.Current.Content is not Frame rootFrame)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                this.RootFrame = rootFrame;
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }*/
            GetSettings();

            if (e.PrelaunchActivated == false)
            {
                if (RootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page, configuring
                    // the new page by passing required information as a navigation parameter.
                    RootFrame.Navigate(typeof(SystemUI), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        public void GetSettings()
        {
            if (LocalSettings.Values["Theme"] == null || (int)LocalSettings.Values["Theme"] == 0)
            {
                (Application.Current as App).RootFrame.RequestedTheme = ElementTheme.Default;
                LocalSettings.Values["Theme"] = 0;
            }
            else if ((int)LocalSettings.Values["Theme"] == 1)
            {
                (Application.Current as App).RootFrame.RequestedTheme = ElementTheme.Light;
            }
            else if ((int)LocalSettings.Values["Theme"] == 2)
            {
                (Application.Current as App).RootFrame.RequestedTheme = ElementTheme.Dark;
            }

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
            if (LocalSettings.Values["TransitionDurationTime"] != null && ((double)LocalSettings.Values["TransitionDurationTime"] >= 0.01))
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

            if (LocalSettings.Values["CurveStyle"] != null && ((int)LocalSettings.Values["CurveStyle"] >= 0))
            {
                CurveStyle = (int)LocalSettings.Values["CurveStyle"];
            }
            if (LocalSettings.Values["CtrPnelCurveStyle"] != null && ((int)LocalSettings.Values["CtrPnelCurveStyle"] >= 0))
            {
                CtrPnelCurveStyle = (int)LocalSettings.Values["CtrPnelCurveStyle"];
            }

            if (LocalSettings.Values["NotificationCenterAlignment"] != null && ((int)LocalSettings.Values["NotificationCenterAlignment"] >= 0))
            {
                NotificationCenterAlignment = (int)LocalSettings.Values["NotificationCenterAlignment"];
            }
            if (LocalSettings.Values["CombineControlCenterWhenWide"] != null)
            {
                CombineControlCenterWhenWide = (bool)LocalSettings.Values["CombineControlCenterWhenWide"];
            }

            if (LocalSettings.Values["SpringSettings_DurationTime"] != null)
            {
                DurationTime = (double)LocalSettings.Values["SpringSettings_DurationTime"];
            }
            if (LocalSettings.Values["SpringSettings_XDamping"] != null)
            {
                XDamping = (double)LocalSettings.Values["SpringSettings_XDamping"];
            }
            if (LocalSettings.Values["SpringSettings_YDamping"] != null)
            {
                YDamping = (double)LocalSettings.Values["SpringSettings_YDamping"];
            }
            if (LocalSettings.Values["SpringSettings_WidthDamping"] != null)
            {
                WidthDamping = (double)LocalSettings.Values["SpringSettings_WidthDamping"];
            }
            if (LocalSettings.Values["SpringSettings_HeightDamping"] != null)
            {
                HeightDamping = (double)LocalSettings.Values["SpringSettings_HeightDamping"];
            }
            if (LocalSettings.Values["SpringSettings_ScaleDamping"] != null)
            {
                ScaleDamping = (double)LocalSettings.Values["SpringSettings_ScaleDamping"];
            }
            if (LocalSettings.Values["SpringSettings_OpacityDamping"] != null)
            {
                OpacityDamping = (double)LocalSettings.Values["SpringSettings_OpacityDamping"];
            }
            if (LocalSettings.Values["SpringSettings_RotationDamping"] != null)
            {
                RotationDamping = (double)LocalSettings.Values["SpringSettings_RotationDamping"];
            }
            if (LocalSettings.Values["SpringSettings_OpacityBeginTime"] != null)
            {
                OpacityBeginTime = (double)LocalSettings.Values["SpringSettings_OpacityBeginTime"];
            }
            if (LocalSettings.Values["SpringSettings_OpacityDurationTime"] != null)
            {
                OpacityDurationTime = (double)LocalSettings.Values["SpringSettings_OpacityDurationTime"];
            }
            if (LocalSettings.Values["SpringSettings_XVelocity"] != null)
            {
                XVelocity = (double)LocalSettings.Values["SpringSettings_XVelocity"];
            }
            if (LocalSettings.Values["SpringSettings_YVelocity"] != null)
            {
                YVelocity = (double)LocalSettings.Values["SpringSettings_YVelocity"];
            }
            if (LocalSettings.Values["SpringSettings_ScaleVelocity"] != null)
            {
                ScaleVelocity = (double)LocalSettings.Values["SpringSettings_ScaleVelocity"];
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
