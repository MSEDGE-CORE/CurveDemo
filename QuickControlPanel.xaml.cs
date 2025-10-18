using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CurveDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class QuickControlPanel : Page
    {
        public static SystemUI SYS
        {
            get { return (Window.Current.Content as Frame)?.Content as SystemUI; }
        }

        DispatcherTimer Timer;
        int isControlPanelOpen = 0;

        public System.TimeSpan TrDur005 = System.TimeSpan.FromSeconds(0.05 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur010 = System.TimeSpan.FromSeconds(0.10 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur015 = System.TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur03 = System.TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);

        public QuickControlPanel()
        {
            this.InitializeComponent();

            ClockTime.Text = DateTime.Now.ToString("H:mm");
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        ElementTheme statusBarColor = ElementTheme.Default;
        public void SetStatusBarColor(ElementTheme color)
        {
            statusBarColor = color;
            if(isControlPanelOpen == 0)
            {
                StatusBar.RequestedTheme = statusBarColor;
            }
        }

        private void Timer_Tick(object? sender, object e)
        {
            ClockTime.Text = DateTime.Now.ToString("H:mm");
        }

        private async void StartControlAnimation(int isOpening)
        {
            if(isOpening == 1)
            {
                isControlPanelOpen = 1;
                BlurAnSet.To = maxBlur;
                BlurPointerAnimation.To = maxBlur;

                BlurAnSet.From = BlurPointerTransform.Y;
                BlurPointerAnimation.From = BlurPointerTransform.Y;
                BlurAnSet.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                BlurPointerAnimation.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                BlurAnimation.Start();
                BlurPointerStoryBoard.Begin();

                ShowControlsStoryBoard.Begin();
            }
            else if(isOpening == 0)
            {
                isControlPanelOpen = 0;
                BlurAnSet.To = 0;
                BlurPointerAnimation.To = 0;

                BlurAnSet.From = BlurPointerTransform.Y;
                BlurPointerAnimation.From = BlurPointerTransform.Y;
                BlurAnSet.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.4);
                BlurPointerAnimation.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.4);
                BlurAnimation.Start();
                BlurPointerStoryBoard.Begin();

                HideControlsStoryBoard.Begin();

                await Task.Delay(TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.4));
                SYS.ShowQuickControlFullScreen(0);
            }
        }


        double MouseDownX = -1, MouseDownY = -1;
        double MouseX = -1, MouseY = -1;
        int maxBlur = 40, maxDY = 100;
        private void ControlBar_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            SYS.ShowQuickControlFullScreen(1);
            MouseDownX = e.Position.X;
            MouseDownY = e.Position.Y;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(100);
        }

        private void ControlBar_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            MouseX = e.Position.X;
            MouseY = e.Position.Y;

            double dY = MouseY - MouseDownY;
            if (dY <= 0)
                dY = 0;
            else if (dY > maxDY)
                dY = maxDY;

            if(isControlPanelOpen == 0)
                BlurPointerTransform.Y = (dY / maxDY) * maxBlur;
            BlurAnSet.From = BlurPointerTransform.Y;
            BlurAnSet.To = BlurPointerTransform.Y;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(100);
            BlurAnimation.Start();
        }

        private async void ControlBar_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            if((BlurPointerTransform.Y >= 0 * maxBlur && e.Velocities.Linear.Y >= 0))
            {
                StartControlAnimation(1);
            }
            else
            {
                StartControlAnimation(0);
            }

        }

        private void NotificationBar_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {

        }

        private void NotificationBar_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {

        }

        private void NotificationBar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ControlsGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GstBut_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

        }

        private void GstBut_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if(e.Velocities.Linear.Y < 0)
            {

                StartControlAnimation(0);
            }
        }

        private void GstBut_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }

        private async void GstBut_Click(object sender, RoutedEventArgs e)
        {
            StartControlAnimation(0);
        }

        private void ControlBar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationBar_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
