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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace CurveDemo
{
    public sealed partial class QuickControlPanel : Page
    {
        public static SystemUI SYS
        {
            get { return (Window.Current.Content as Frame)?.Content as SystemUI; }
        }

        DispatcherTimer Timer;
        int isControlPanelOpen = 0;

        public System.TimeSpan TrDur01 = System.TimeSpan.FromSeconds(0.10 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur02 = System.TimeSpan.FromSeconds(0.20 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur03 = System.TimeSpan.FromSeconds(0.30 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur04 = System.TimeSpan.FromSeconds(0.4 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur05 = System.TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur06 = System.TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur07 = System.TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur08 = System.TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);

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
                GridStatusClockMotion.RequestedTheme = StatusBar.RequestedTheme;
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
                NG1Scale.CenterX = NG2Scale.CenterX = 250;
                CGSScaleKeyScaleX.Value = CGSScaleKeyScaleY.Value = 1;
                NGSScaleKeyScaleX.Value = NGSScaleKeyScaleY.Value = 1;
                if (CGSScale.ScaleX != 1)
                {
                    CGSScale.ScaleX = CGSScale.ScaleY = 1;
                    NGSScale.ScaleX = NGSScale.ScaleY = 1;
                }
            }
            else if((Application.Current as App).CtrPnelCurveStyle == 1)
            {/*
                CG0Scale.CenterX = 400;
                CG1Scale.CenterX = CG2Scale.CenterX = CG3Scale.CenterX = 400;*/
                CG0Scale.CenterX = 210;
                CG1Scale.CenterX = CG2Scale.CenterX = CG3Scale.CenterX = 250;
                NG1Scale.CenterX = NG2Scale.CenterX = 250;
                CGSScaleKeyScaleX.Value = CGSScaleKeyScaleY.Value = 0.2;
                NGSScaleKeyScaleX.Value = NGSScaleKeyScaleY.Value = 0.2;
                if (CGSScale.ScaleX == 1 && CG0Scale.ScaleX == 0.8)
                {
                    CGSScale.ScaleX = CGSScale.ScaleY = 0.2;
                    NGSScale.ScaleX = NGSScale.ScaleY = 0.2;
                }
            }

            if (isOpening == 1)
            {
                NotificationsScrollViewer.ScrollToVerticalOffset(0);
                ControlsScrollViewer.ScrollToVerticalOffset(0);
                GridControlPanel.Visibility = Visibility.Visible;
                BackBoard.Visibility = Visibility.Visible;
                StatusBar.RequestedTheme = BackBoard.ActualTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
                GridStatusClockMotion.RequestedTheme = StatusBar.RequestedTheme;

                if (isPointerMoving == 0)
                {
                    //BlurAnSet.To = maxBlur;
                    BlurPointerAnimation.To = maxBlur;

                    //BlurAnSet.From = BlurPointerTransform.Y;
                    BlurPointerAnimation.From = BlurPointerTransform.Y;
                    //BlurAnSet.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                    BlurPointerAnimation.Duration = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                    //BlurAnimation.Start();
                    BlurPointerStoryBoard.Begin();

                    GSCOpenDAHeight.Value = GridClockToH;
                    GSCOpenDAHeight.KeyTime = TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5);
                    GridStatusClockOpenStoryBoard.Begin();

                }
                if(isControlPanelOpen == 0)
                {
                    AWAControlScale.ScaleX = AWAControlScale.ScaleY = 6.0;
                    ShowControlsStoryBoard.Begin();
                }
                isControlPanelOpen = 1;
            }
            else if (isOpening == 0)
            {
                StatusBar.RequestedTheme = statusBarColor;
                GridStatusClockMotion.RequestedTheme = StatusBar.RequestedTheme;

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

                    GSCOpenDAHeight.Value = GridClockToH;
                    GridStatusClockCloseStoryBoard.Begin();
                }

                if (isControlPanelOpen == 1)
                {
                    HideControlsStoryBoard.Begin();
                }
                else if (ControlsGrid0.Opacity < 0.1 && MouseX == -100000 && isPointerMoving == 0)
                {
                    GridControlPanel.Visibility = Visibility.Collapsed;
                    BackBoard.Visibility = Visibility.Collapsed;
                }
                    isControlPanelOpen = 0;

            }
        }

        double MouseDownX = -1, MouseDownY = -1;
        double MouseX = -100000, MouseY = -1;
        int maxBlur = 40, maxDY = 100;
        int GridClockFromH = 48, GridClockToH = 128;
        double firstBlur = 0;
        private void StartDownPull(double eY, int currentPanel = 0)
        {

            GridControlPanel.Visibility = Visibility.Visible;
            MouseDownY = eY;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(100);
            firstBlur = BlurPointerTransform.Y;
            BlurPointerStoryBoard.Stop();
            BlurPointerTransform.Y = firstBlur;
            if (MoveBackControlCardsStoryBoard.GetCurrentState() == Windows.UI.Xaml.Media.Animation.ClockState.Active)
            {
                MoveBackControlCardsStoryBoard.Begin();
                MoveBackControlCardsStoryBoard.Stop();
            }
        }
        private void StartDownPulling(double dY)
        {
            BlurPointerStoryBoard.Stop();

            if (dY <= 0)
                dY = 0;
            else if (dY > maxDY)
                dY = maxDY;

            BlurPointerTransform.Y = (dY / maxDY) * maxBlur;

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
                
                if(GridStatusClockOpenStoryBoard.GetCurrentState() == Windows.UI.Xaml.Media.Animation.ClockState.Active)
                {
                    GridStatusClockOpenStoryBoard.Begin();
                    GridStatusClockOpenStoryBoard.Stop();
                }
                else if (GridStatusClockCloseStoryBoard.GetCurrentState() == Windows.UI.Xaml.Media.Animation.ClockState.Active)
                {
                    GridStatusClockCloseStoryBoard.Begin();
                    GridStatusClockCloseStoryBoard.Stop();
                }

                double SCMHeight = ((dY * 1.0 / maxDY) * (GridClockToH - GridClockFromH) + GridClockFromH);
                if(SCMHeight < GridClockFromH)
                    GridStatusClockMotion.Height = GridClockFromH;
                else if(SCMHeight > GridClockToH)
                    GridStatusClockMotion.Height = GridClockToH;
                else
                    GridStatusClockMotion.Height = SCMHeight;

                if (dY <= 1 * maxDY)
                {
                    double ddY = -dY + maxDY;
                    CG0Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    CG1Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    CG2Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    CG3Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    NG1Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    NG2Translate.Y = -GetSpaceDeltaY(ddY, 100);
                    ClockTimeDownPullTranslate.Y = 0;

                }
                else if (dY > maxDY * 1)
                {
                    double ddY = dY - maxDY;
                    CG0Translate.Y = GetSpaceDeltaY(ddY, 150);
                    CG1Translate.Y = GetSpaceDeltaY(ddY, 250);
                    CG2Translate.Y = GetSpaceDeltaY(ddY, 350);
                    CG3Translate.Y = GetSpaceDeltaY(ddY, 450);
                    ClockTimeDownPullTranslate.Y = ControlHorizontalScrollViewer.HorizontalOffset == 0 ? GetSpaceDeltaY(ddY, 150) : 0;
                    NG1Translate.Y = GetSpaceDeltaY(ddY, 250);
                    NG2Translate.Y = GetSpaceDeltaY(ddY, 350);
                }
            }
        }
        private void StartDownPulled(double vY,int sender = 0)
        {
            MouseX = -100000;
            MouseY = -1;
            if ((BlurPointerTransform.Y >= 0 * maxBlur && vY >= 0 && sender == 0) || (BlurPointerTransform.Y >= 0 * maxBlur && vY >= 1 && sender == 1))
            {
                StartControlAnimation(1);
                MBCC0KeyY.Value = MBCC1KeyY.Value = MBCC2KeyY.Value = MBCC3KeyY.Value = MBCC4KeyY.Value = MBCC5KeyY.Value = 0;
                MoveBackControlCardsStoryBoard.Begin();
            }
            else
            {
                StartControlAnimation(0);
                MBCC0KeyY.Value = MBCC1KeyY.Value = MBCC2KeyY.Value = MBCC3KeyY.Value = MBCC4KeyY.Value = MBCC5KeyY.Value = CG0Translate.Y -0;
                MoveBackControlCardsStoryBoard.Begin();
            }
        }

        private double GetSpaceDeltaY(double y, double a)
        {
            y = (a * (-1 / (Math.Abs(y) / a + 1) + 1));
            return y;
        }


        private async void ControlBar_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            GridControlPanel.Visibility = Visibility.Visible;
            if (isControlPanelOpen == 0 || true)
            {
                ControlHorizontalScrollViewer.ScrollToHorizontalOffset(ActualWidth);
                while (!(Application.Current as App).CombineControlCenterWhenWide && (ControlHorizontalScrollViewer.ScrollableWidth != ActualWidth || ControlHorizontalScrollViewer.HorizontalOffset != ControlHorizontalScrollViewer.ScrollableWidth))
                {
                    await Task.Delay(10);
                    ControlHorizontalScrollViewer.ScrollToHorizontalOffset(ActualWidth);
                }
            }
            MouseX = e.Position.X;
            MouseY = e.Position.Y;
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
            if (isControlPanelOpen == 0 || true)
            {
                ControlHorizontalScrollViewer.ScrollToHorizontalOffset(0);
            }
            MouseX = e.Position.X;
            MouseY = e.Position.Y;
            StartDownPull(e.Position.Y, 1);
        }

        private void NotificationBar_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            MouseX = e.Position.X;
            MouseY = e.Position.Y;
            double dY = MouseY - MouseDownY + (firstBlur / maxBlur) * maxDY;
            StartDownPulling(dY);
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
            StartDownPulled(e.Velocities.Linear.Y,(sender.GetType() == typeof(Button) ? 1:0));
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
            if(ControlsGrid0.Opacity < 0.1 && ShowControlsStoryBoard.GetCurrentState() != ClockState.Active && MouseX == -100000)
            {
                AWAControlScale.ScaleX = AWAControlScale.ScaleY = 6.0;
                GridControlPanel.Visibility = Visibility.Collapsed;
                BackBoard.Visibility = Visibility.Collapsed;
            }
        }

        private void NotificationsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            MoveStatusClock();
        }

        private void ControlHorizontalScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            MoveStatusClock();

            if(ControlHorizontalScrollViewer.HorizontalOffset == 0)
            {
                ControlsScrollViewer.ScrollToVerticalOffset(0);
            }
            else if(ControlHorizontalScrollViewer.HorizontalOffset == ControlHorizontalScrollViewer.ScrollableWidth)
            {
                NotificationsScrollViewer.ScrollToVerticalOffset(0);
            }
        }

        //已定义GridClockFromH
        double NotificationScrollFromOf = 100, NotificationScrollToOf = 0;
        double HorizontalScrollFromOf = 10, HorizontalScrollToOf = 0;
        double FontSizeFrom = 22, FontSizeTo = 80;



        private void CP_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            (((SYS.Content as Grid).Children[0] as Frame).Content as MainPage).StartBackgroundAnimation(1);
            if ((((SYS.Content as Grid).Children[0] as Frame).Content as MainPage).AppLauncher("com.android.settings") != -4)
                (((SYS.Content as Grid).Children[0] as Frame).Content as MainPage).StartWindowAnimation(1, -3, [GridCP20.ActualOffset.X + ControlsGrid0.ActualOffset.X + ControlsScrollViewer.ActualOffset.X + (ControlsGrid0.Children[1] as Grid).ActualOffset.X + 24 , GridCP20.ActualOffset.Y + ControlsScrollViewer.ActualOffset.Y - ControlsScrollViewer.VerticalOffset + ControlsGrid0.ActualOffset.Y + (ControlsGrid0.Children[1] as Grid).ActualOffset.Y + CG0Translate.Y + 24 + 52, 48,48]);
            //(((SYS.Content as Grid).Children[0] as Frame).Content as MainPage).StartWindowAnimation(1, -3, [ActualWidth / 2.0, 0, 48, 48]);
            StartControlAnimation(0);
        }

        double ClockXFrom = 0, ClockXTo = 1, ClockYFrom = 0, ClockYTo = 32;
        public void MoveStatusClock()
        {
            double Pointer1 = Math.Max(Math.Min(((GridStatusClockMotion.Height - GridClockFromH) * 1.0 / (GridClockToH - GridClockFromH)), 1), 0);
            double Pointer2 = Math.Max(Math.Min(((ControlHorizontalScrollViewer.HorizontalOffset - HorizontalScrollFromOf) * 1.0 / (HorizontalScrollToOf - HorizontalScrollFromOf)), 1), 0);
            double Pointer3 = Math.Max(Math.Min(((NotificationsScrollViewer.VerticalOffset - NotificationScrollFromOf) * 1.0 / (NotificationScrollToOf - NotificationScrollFromOf)), 1), 0);
            double FinalPointer = Math.Min(Math.Min(Pointer2, Pointer3), Pointer1);

            FontSizeTo = 80;
            if (ActualWidth >= 1000 && (Application.Current as App).CombineControlCenterWhenWide)
            {
                GridCP1.Width = ActualWidth - 500;
                GridCP2.Width = 500;
            }
            else
            {
                GridCP1.Width = GridCP2.Width = ActualWidth;
            }
            if ((Application.Current as App).NotificationCenterAlignment == 1 || ActualWidth >= 1000 && (Application.Current as App).CombineControlCenterWhenWide)
            {
                ClockXTo = 32;
                ClockYTo = 20;
                (NotificationsScrollViewer.Content as StackPanel).HorizontalAlignment = HorizontalAlignment.Left;

            }
            else if ((Application.Current as App).NotificationCenterAlignment == 0)
            {
                ClockXTo = ActualWidth / 2.0 - 110 - 14;
                ClockYTo = 20;
                (NotificationsScrollViewer.Content as StackPanel).HorizontalAlignment = HorizontalAlignment.Center;

            }



            ClockTime.FontSize = FinalPointer * (FontSizeTo - FontSizeFrom) + FontSizeFrom;
            if(ClockTime.FontSize >= 40)
            {
                ClockTime.FontWeight = Windows.UI.Text.FontWeights.Bold;
            }
            else
            {
                ClockTime.FontWeight = Windows.UI.Text.FontWeights.Medium;
            }
                ClockTimeTranslate.X = FinalPointer * (ClockXTo - ClockXFrom) + ClockXFrom;
            ClockTimeTranslate.Y = FinalPointer * (ClockYTo - ClockYFrom) + ClockYFrom;
        }

        private void GridStatusClockMotion_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveStatusClock();
            BlurAnSet.From = BlurPointerTransform.Y;
            BlurAnSet.To = BlurPointerTransform.Y;
            BlurAnSet.Duration = TimeSpan.FromMilliseconds(50);
            BlurAnimation.Start();
        }

        private void ControlHorizontalScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ControlBar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationBar_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            StartDownPulled(e.Velocities.Linear.Y);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth >= 1000 && (Application.Current as App).CombineControlCenterWhenWide)
            {
                GridCP1.Width = ActualWidth - 500;
                GridCP2.Width = 500;
                GridCP20.HorizontalAlignment = HorizontalAlignment.Right;
                CGSScale.CenterX = 460;
                NGSScale.CenterX = 20;
                HorizontalScrollFromOf = 10;
                HorizontalScrollToOf = 00;
                ClockXTo = 1;
            }
            else if (ActualWidth > 0)
            {
                GridCP1.Width = GridCP2.Width = ActualWidth;

                if (ActualWidth <= 720)
                {
                    GridCP20.HorizontalAlignment = HorizontalAlignment.Center;
                    CGSScale.CenterX = 230 + ActualWidth / 2;
                    NGSScale.CenterX = 20;
                    HorizontalScrollFromOf = ActualWidth;
                    HorizontalScrollToOf = 0;
                }
                else
                {
                    GridCP20.HorizontalAlignment = HorizontalAlignment.Right;
                    CGSScale.CenterX = 460;
                    NGSScale.CenterX = 20;
                    if((Application.Current as App).NotificationCenterAlignment == 0)
                    {
                        HorizontalScrollFromOf = ActualWidth / 2.0 + 360;
                        HorizontalScrollToOf = ActualWidth / 2.0 - 360;
                    }
                    else if((Application.Current as App).NotificationCenterAlignment == 1)
                    {
                        HorizontalScrollFromOf = 720;
                        HorizontalScrollToOf = 0;
                    }
                }
            }

        }
    }
}
