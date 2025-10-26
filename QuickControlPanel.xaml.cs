using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int isControlPanelToOpen = 0;

        public System.TimeSpan TrDur005 = System.TimeSpan.FromSeconds(0.05 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur010 = System.TimeSpan.FromSeconds(0.10 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur015 = System.TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur03 = System.TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur04 = System.TimeSpan.FromSeconds(0.4 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur05 = System.TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur06 = System.TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur07 = System.TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);

        public QuickControlPanel()
        {
            this.InitializeComponent();

            ClockTime.Text = DateTime.Now.ToString("H:mm");
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Timer.Tick += Timer_Tick;
            Timer.Start();

            CG0Scale.ScaleX = CG0Scale.ScaleY = CG1Scale.ScaleX = CG1Scale.ScaleY = CG2Scale.ScaleX = CG2Scale.ScaleY = CG3Scale.ScaleX = CG3Scale.ScaleY = 0.8;
            ControlsGrid0.Opacity = ControlsGrid1.Opacity = ControlsGrid2.Opacity = ControlsGrid3.Opacity = 0;
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

        private async void StartControlAnimation(int isOpening, int isPointerMoving = 0)
        {
            if ((Application.Current as App).CtrPnelCurveStyle == 0)
            {
                CG0Scale.CenterX = 210;
                CG1Scale.CenterX = CG2Scale.CenterX = CG3Scale.CenterX = 250;
                CGSScaleKeyScaleX.Value = CGSScaleKeyScaleY.Value = 1;
                if (CGSScale.ScaleX != 1)
                {
                    CGSScale.ScaleX = CGSScale.ScaleY = 1;
                }
            }
            else if((Application.Current as App).CtrPnelCurveStyle == 1)
            {
                CG0Scale.CenterX = 400;
                CG1Scale.CenterX = CG2Scale.CenterX = CG3Scale.CenterX = 400;
                CGSScaleKeyScaleX.Value = CGSScaleKeyScaleY.Value = 0.2;
                if (CGSScale.ScaleX == 1 && CG0Scale.ScaleX == 0.8)
                {
                    CGSScale.ScaleX = CGSScale.ScaleY = 0.2;
                }
            }

            if (isOpening == 1)
            {
                ControlsScrollViewer.ScrollToVerticalOffset(0);
                GridControlPanel.Visibility = Visibility.Visible;
                BackBoard.Visibility = Visibility.Visible;
                StatusBar.RequestedTheme = ElementTheme.Dark;

                if (isPointerMoving == 0)
                {
                    BlurAnSet.To = maxBlur;
                    BlurPointerAnimation.To = maxBlur;

                    BlurAnSet.From = BlurPointerTransform.Y;
                    BlurPointerAnimation.From = BlurPointerTransform.Y;
                    BlurAnSet.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                    BlurPointerAnimation.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                    BlurAnimation.Start();
                    BlurPointerStoryBoard.Begin();

                }
                if(isControlPanelOpen == 0)
                {
                    ShowControlsStoryBoard.Begin();
                }
                isControlPanelOpen = 1;
            }
            else if (isOpening == 0)
            {
                StatusBar.RequestedTheme = statusBarColor;

                if (isPointerMoving == 0)
                {
                    BlurAnSet.To = 0;
                    BlurPointerAnimation.To = 0;

                    BlurAnSet.From = BlurPointerTransform.Y;
                    BlurPointerAnimation.From = BlurPointerTransform.Y;
                    BlurAnSet.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.4);
                    BlurPointerAnimation.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.4);
                    BlurAnimation.Start();
                    BlurPointerStoryBoard.Begin();
                }

                if (isControlPanelOpen == 1)
                {
                    HideControlsStoryBoard.Begin();
                }
                isControlPanelOpen = 0;

            }
        }

        double MouseDownX = -1, MouseDownY = -1;
        double MouseX = -1, MouseY = -1;
        int maxBlur = 40, maxDY = 100;
        double firstBlur = 0;
        private void StartDownPull(double eY, int currentPanel = 0)
        {
            MouseDownY = eY;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(100);
            firstBlur = BlurPointerTransform.Y;
            BlurPointerStoryBoard.Stop();
            BlurPointerTransform.Y = firstBlur;
            if (MoveBackControlCardsStoryBoard.GetCurrentState() == Windows.UI.Xaml.Media.Animation.ClockState.Active)
                MoveBackControlCardsStoryBoard.Stop();
        }
        private void StartDownPulling(double dY)
        {
            BlurPointerStoryBoard.Stop();

            if (dY <= 0)
                dY = 0;
            else if (dY > maxDY)
                dY = maxDY;

            BlurPointerTransform.Y = (dY / maxDY) * maxBlur;
            BlurAnSet.From = BlurPointerTransform.Y;
            BlurAnSet.To = BlurPointerTransform.Y;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(100);
            BlurAnimation.Start();

            if ((dY >= 0.55 * maxDY && isControlPanelOpen == 0))
            {
                StartControlAnimation(1, 1);
            }
            else if (dY <= 0.45 * maxDY && isControlPanelOpen == 1)
            {
                StartControlAnimation(0, 1);
            }

            if (true) //卡片间距
            {
                dY = MouseY - MouseDownY + (firstBlur / maxBlur) * maxDY;
                if (dY <= 1 * maxDY)
                {
                    CG0Translate.Y = dY - maxDY;
                    CG1Translate.Y = dY - maxDY;
                    CG2Translate.Y = dY - maxDY;
                    CG3Translate.Y = dY - maxDY;

                }
                else if (dY > maxDY * 1)
                {
                    double ddY = dY - maxDY;
                    CG0Translate.Y = GetSpaceDeltaY(ddY, 150);
                    CG1Translate.Y = GetSpaceDeltaY(ddY, 250);
                    CG2Translate.Y = GetSpaceDeltaY(ddY, 350);
                    CG3Translate.Y = GetSpaceDeltaY(ddY, 450);
                }
            }
        }
        private void StartDownPulled(double vY)
        {
            MouseX = -1;
            MouseY = -1;
            if ((BlurPointerTransform.Y >= 0 * maxBlur && vY >= -1))
            {
                StartControlAnimation(1);
                MBCC0KeyY.Value = MBCC1KeyY.Value = MBCC2KeyY.Value = MBCC3KeyY.Value = 0;
                MoveBackControlCardsStoryBoard.Begin();
            }
            else
            {
                StartControlAnimation(0);
                MBCC0KeyY.Value = MBCC1KeyY.Value = MBCC2KeyY.Value = MBCC3KeyY.Value = -100;
                MoveBackControlCardsStoryBoard.Begin();
            }
        }

        private double GetSpaceDeltaY(double y, double a)
        {
            y = (a * (-1 / (Math.Abs(y) / a + 1) + 1));
            return y;
        }


        private void ControlBar_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            MouseX = e.Position.X;
            MouseY = e.Position.Y;
            isControlPanelToOpen = 1;
            StartDownPull(e.Position.Y, 1);
        }

        private void ControlBar_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            MouseX = e.Position.X;
            MouseY = e.Position.Y;
            double dY = MouseY - MouseDownY + (firstBlur / maxBlur) * maxDY;
            StartDownPulling(dY);
        }

        private async void ControlBar_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            StartDownPulled(e.Velocities.Linear.Y);
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
            MouseX = e.Position.X;
            MouseY = e.Position.Y;

            double dY = MouseY - MouseDownY + (firstBlur / maxBlur) * maxDY;
            StartDownPulling(dY);
        }

        private void GstBut_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            StartDownPulled(e.Velocities.Linear.Y);
        }

        private void GstBut_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            MouseDownX = e.Position.X;
            MouseDownY = e.Position.Y;
            StartDownPull(MouseDownY);
        }

        private async void GstBut_Click(object sender, RoutedEventArgs e)
        {
            if (CG0Translate.Y <= 0)
            {
                StartControlAnimation(0);
                MBCC0KeyY.Value = MBCC1KeyY.Value = MBCC2KeyY.Value = MBCC3KeyY.Value = -100;
                MoveBackControlCardsStoryBoard.Begin();
            }
        }

        private void HideControlsStoryBoard_Completed(object sender, object e)
        {
            if(ControlsGrid0.Opacity == 0 && MouseY == -1)
            {
                GridControlPanel.Visibility = Visibility.Collapsed;
                BackBoard.Visibility = Visibility.Collapsed;
            }
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
