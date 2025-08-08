using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
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

        public ApplicationDataContainer LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public int EnableBgBlur = 1;
        public double ScreenCornerRadius = 100.0 / 1920;
        public double TransitionDurationTime = 1.0;
        public double FlyFar = 1.0;


        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;

            if (LocalSettings.Values["EnableBgBlur"] != null && ((int)LocalSettings.Values["EnableBgBlur"] == 1 || (int)LocalSettings.Values["EnableBgBlur"] == 0))
            {
                EnableBgBlur = (int)LocalSettings.Values["EnableBgBlur"];
            }
            if(LocalSettings.Values["EnableCornerRadius"] != null && ((bool)LocalSettings.Values["EnableCornerRadius"] == false))
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
        }

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
