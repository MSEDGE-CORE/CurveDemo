using CommunityToolkit.WinUI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using static CommunityToolkit.WinUI.Animations.Expressions.ExpressionValues;
using static CurveDemo.App;

namespace CurveDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        DispatcherTimer RoundCornerTimer;
        DispatcherTimer Timer;

        public System.TimeSpan TrDur05 = System.TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur06 = System.TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur07 = System.TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur075 = System.TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
        public System.TimeSpan TrDur085 = System.TimeSpan.FromSeconds(0.85 * (Application.Current as App).TransitionDurationTime);

        public static SystemUI MP
        {
            get { return (Window.Current.Content as Frame)?.Content as SystemUI; }
        }

        public MainPage()
        {
            TrDur05 = TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
            TrDur06 = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
            TrDur07 = TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);
            TrDur075 = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
            TrDur085 = TimeSpan.FromSeconds(0.85 * (Application.Current as App).TransitionDurationTime);

            InitializeComponent();

            RoundCornerTimer = new DispatcherTimer();
            RoundCornerTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            RoundCornerTimer.Tick += RoundCornerTick;
            GstBut.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.TranslateX | Windows.UI.Xaml.Input.ManipulationModes.TranslateY;

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Timer.Tick += Timer_Tick;
            Timer.Start();

        }

        private void Timer_Tick(object? sender, object e)
        {
            Page_SizeChanged(null, null);
            //Trace.WriteLine(ApFrTranslate.Y);
        }


        public int AppWindowMain_Target = -2, AppWindowMain_mIndex = -2, AppWindowState = 0, AppWindow2_Target = -2;
        private async void StartWindowAnimation(int isOnLaunching = 0, int AppTarget = -2)
        {
            AppWindowGesture.Visibility = Visibility.Visible; 

            if(isOnLaunching == -1)
            {
                if (AppWindowMain_Target >= 0)
                {
                    AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    AppWindow2Gesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
                    AppWindowState = 0;
                    AWGTransform.X = AWGTransform.Y = 0;
                    AWGScale.ScaleX = AWGScale.ScaleY = 1.0;
                }
                else
                {
                    AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    AppWindow2Gesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    AppWindowState = 0;
                    AWGTransform.X = AWGTransform.Y = 0;
                    AWGScale.ScaleX = AWGScale.ScaleY = 1.0;
                }
            }
            else if (isOnLaunching == 1)
            {
                GstBut.Visibility = Visibility.Visible;
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppWindowGesture.Opacity = 1;

                if (AppWindowMain_Target >= 0 && AppTarget != AppWindowMain_Target && AppWindowState == 2)
                {
                    //进入并行
                    //(DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;

                    if((Application.Current as App).EnableSideWindowAnimation == 1)
                    {
                        AppWindow2Gesture.Opacity = 1;
                        AppWindow2Gesture.Visibility = Visibility.Visible;

                        AW2GScaleFallBehind.CenterX = ActualWidth * 0.5;
                        AW2GScaleFallBehind.CenterY = ActualHeight * 0.5;
                        AW2GScaleFallBehind.ScaleX = AW2GScaleFallBehind.ScaleY = 1.0;
                        if((Application.Current as App).EnableBgScale == 1)
                        {
                            AW2GFallBehindStoryBoard.Begin();
                        }
                    }
                    else
                    {
                        Trace.WriteLine(1);
                        AppWindow2Gesture.Opacity = 0.01;
                        AppWindow2Gesture.Visibility = Visibility.Collapsed;
                        if (AppWindow2_Target >= 0)
                            (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
                        if (AppWindowMain_Target >= 0)
                            (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
                    }
                }

                if (AppTarget != -2 && AppTarget != -1)
                {
                    /*if (AppWindowMain_Target >= 0)
                        (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;*/
                    //AWABackStoryBoard.Stop();

                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    if (AppTarget != AppWindowMain_Target || AppWindowState == 0)
                    {
                        AWGGestureFlyStoryBoard.Stop();
                        AWAGestureBackStoryBoard.Stop();
                        AWAGestureBack2StoryBoard.Stop();
                        AWABackStoryBoard.Stop();
                        AWGTransform.X = AWGTransform.Y = 0;
                        AWGScale.ScaleX = AWGScale.ScaleY = 1.0;

                        AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                        AWATransform.X = 0;
                        AWATransform.Y = ActualHeight * FarPoint - ActualHeight * 0.4;

                        if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                            AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                        if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                        {
                            AWBackgIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Image).Source;
                            AWFrontIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[1] as Image).Source;
                        }
                        else if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                        {
                            (AWCardFrame.Content as DesktopCard).GetCardInfo = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo;
                            if ((((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo.Contains("Pic"))
                            {
                                (AWCardFrame.Content as DesktopCard).GetImage = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetImage;
                            }
                            AWBackgIcon.Source = null;
                            AWFrontIcon.Source = null;
                        }

                        AWFrontIcon.Opacity = 1;
                        AWBackgIcon.Opacity = 1;
                        if (DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight) <= (ActualWidth / ActualHeight))
                        {
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                            AppHeightAnimation.Height = ActualHeight;
                            AppHeightAnimation.Width = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                        }
                        else
                        {
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5;
                            AppHeightAnimation.Width = ActualWidth;
                            AppHeightAnimation.Height = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                        }

                        AWAScale.CenterX = ActualWidth * 0.5;
                        AWAScale.CenterY = ActualHeight * 0.5;
                        AWATransform.X = -(ActualWidth - ActualWidth * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.X - DesktopGridScrollViewer.HorizontalOffset + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + AWAScale.ScaleX * (AppHeightAnimation.Width - ActualWidth) / 2.0;
                        AWATransform.Y = -(ActualHeight - ActualHeight * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.Y - DesktopGridScrollViewer.VerticalOffset + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 + AWAScale.ScaleX * (AppHeightAnimation.Height - ActualHeight) / 2.0;
                        RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);

                        AWMultiTaskGrid.Opacity = 0;
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }
                    else
                    {
                        //AWGGestureFillStoryBoard.Begin();
                        double tX = (AWATransform.X + AWGTransform.X), tY = (AWATransform.Y + AWGTransform.Y), tS = AWAScale.ScaleX * AWGScale.ScaleX, tH = AppHeightAnimation.Height, tW = AppHeightAnimation.Width;
                        AWGGestureFlyStoryBoard.Stop();
                        AWABackStoryBoard.Stop();
                        AWAGestureBackStoryBoard.Stop();
                        AWAGestureBack2StoryBoard.Stop();
                        AWATransform.X = tX;
                        AWATransform.Y = tY;
                        AWAScale.ScaleX = AWAScale.ScaleY = tS;
                        AppHeightAnimation.Height = tH;
                        AppHeightAnimation.Width = tW;

                        AWGTransform.X = AWGTransform.Y = 0;
                        AWGScale.ScaleX = AWGScale.ScaleY = 1.0;

                        /*AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                        AWATransform.X = 0;
                        AWATransform.Y = ActualHeight * FarPoint - ActualHeight * 0.4;*/

                        if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                            AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                        if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                        {
                            AWBackgIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Image).Source;
                            AWFrontIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[1] as Image).Source;
                        }
                        else if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                        {
                            (AWCardFrame.Content as DesktopCard).GetCardInfo = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo;
                            if ((((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo.Contains("Pic"))
                            {
                                (AWCardFrame.Content as DesktopCard).GetImage = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetImage;
                            }
                            AWBackgIcon.Source = null;
                            AWFrontIcon.Source = null;
                        }

                        AWFrontIcon.Opacity = 1;
                        AWBackgIcon.Opacity = 1;
                    }
                    AppWindowState = 1;
                    AppWindowMain_Target = AppTarget;

                    AWAOpenKeyScaleX.Value = AWAOpenKeyScaleY.Value = 1.0;
                    AWAOpenKeyH.Value = ActualHeight;
                    AWAOpenKeyW.Value = ActualWidth;
                    AWAOpenKeyX.Value = 0;
                    AWAOpenKeyY.Value = 0;
                    //RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16;
                    RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerAnimation.To = 500 * (Application.Current as App).ScreenCornerRadius;
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);


                    AWAFrameOpacityDoubleAnimation.From = AWMultiTaskGrid.Opacity;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);

                    //await Task.Delay(0);
                    AWALaunchingStoryBoard.Begin();
                    AWAFrameOpacity.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();

                    (DesktopGrid.Children[AppTarget] as Grid).Opacity = 0.01;

                }
                else if (AppTarget == -1)
                {

                    AWABackStoryBoard.Stop();
                    AWAGestureBackStoryBoard.Stop();
                    AWAGestureBack2StoryBoard.Stop();
                    AWGGestureFlyStoryBoard.Stop();
                    AWGTransform.X = AWGTransform.Y = 0;
                    AWGScale.ScaleX = AWGScale.ScaleY = 1.0;

                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    AppHeightAnimation.Height = ActualHeight;
                    AppHeightAnimation.Width = ActualWidth;

                    AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                    AWATransform.X = 0;
                    AWATransform.Y = ActualHeight * -0.25;// FarPoint - ActualHeight * 0.4;
                    //Trace.WriteLine(AppRectGrid1Transform.Y);

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;

                    AppWindowMain_Target = AppTarget;

                    AWAOpenKeyScaleX.Value = AWAOpenKeyScaleY.Value = 1.0;
                    AWAOpenKeyH.Value = ActualHeight;
                    AWAOpenKeyW.Value = ActualWidth;
                    AWAOpenKeyX.Value = 0;
                    AWAOpenKeyY.Value = 0;
                    //RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16;
                    RoundCornerPointerAnimation.From = 500 * (Application.Current as App).ScreenCornerRadius;
                    RoundCornerPointerAnimation.To = 500 * (Application.Current as App).ScreenCornerRadius;
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                        AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                    (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";
                    AWFrontIcon.Source = null;
                    AWBackgIcon.Source = null;

                    AWAFrameOpacityDoubleAnimation.From = AWMultiTaskGrid.Opacity;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);

                    //AWSwipeBar.Opacity = 1;

                    AWALaunchingStoryBoard.Begin();
                    AWAFrameOpacity.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();
                    AppWindowState = 1;
                }
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppWindowGesture.Opacity = 1;


            }
            else if(isOnLaunching == 0)
            {
                AW2ABackStoryBoard.Stop();
                AW2AGestureBackStoryBoard.Stop();
                AW2AGestureBack2StoryBoard.Stop();
                AW2GGestureFlyStoryBoard.Stop();
                AppWindowState = 2;

                if (AppTarget != -1 && AppTarget != -2)
                {
                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;

                    //主
                    if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                        AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                    if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        AWBackgIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Image).Source;
                        AWFrontIcon.Source = ((DesktopGrid.Children[AppTarget] as Grid).Children[1] as Image).Source;
                    }
                    else if ((DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && (DesktopGrid.Children[AppTarget] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                    {
                        (AWCardFrame.Content as DesktopCard).GetCardInfo = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo;
                        if ((((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetCardInfo.Contains("Pic"))
                        {
                            (AWCardFrame.Content as DesktopCard).GetImage = (((DesktopGrid.Children[AppTarget] as Grid).Children[0] as Frame).Content as DesktopCard).GetImage;
                        }
                        AWBackgIcon.Source = null;
                        AWFrontIcon.Source = null;
                    }

                    if (DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight) <= (ActualWidth / ActualHeight))
                    {
                        AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                        AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                        AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                        AWABackKeyH.Value = ActualHeight;
                        AWABackKeyW.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                        AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                    }
                    else
                    {
                        AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                        AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                        AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5;
                        AWABackKeyW.Value = ActualWidth;
                        AWABackKeyH.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;

                        AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                    }


                    AppWindowMain_Target = AppTarget;

                    //AWABackKeyScaleX.Value = AWAOpenKeyScaleY.Value = 0;
                    //AWABackKeyH.Value = ActualHeight;
                    //AWABackKeyW.Value = ActualWidth;
                    AWABackKeyX.Value = -(ActualWidth - ActualWidth * AWABackKeyScaleX.Value) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + (AWABackKeyW.Value - ActualWidth) * AWABackKeyScaleX.Value / 2.0;
                    AWABackKeyY.Value = -(ActualHeight - ActualHeight * AWABackKeyScaleX.Value) * 0.5 + DesktopGrid.ActualOffset.Y - DesktopGridScrollViewer.VerticalOffset + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 +  (AWABackKeyH.Value - ActualHeight) * AWABackKeyScaleX.Value / 2.0;
                    RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerAnimation.To = 500 * 100 / 1920 * 0.8 / AWABackKeyScaleX.Value;
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);


                    AWAFrameOpacityDoubleAnimation.From = AWMultiTaskGrid.Opacity;
                    AWAFrameOpacityDoubleAnimation.To = 0;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.2 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.1 * (Application.Current as App).TransitionDurationTime);

                    //await Task.Delay(0);
                    AWABackStoryBoard.Begin();
                    AWAFrameOpacity.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();

                    AWReturnToApp.Visibility = Visibility.Visible;
                    (DesktopGrid.Children[AppTarget] as Grid).Opacity = 0.01;


                    //准备并行
                    if ((Application.Current as App).EnableSideWindowAnimation == 1)
                    {
                        if (AppWindow2_Target != AppWindowMain_Target && AppWindow2_Target >= 0)
                            (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;

                        StartSideAnimation();
                    }
                    else
                    {
                        AppWindow2Gesture.Opacity = 0.01;
                        AppWindow2Gesture.Visibility = Visibility.Collapsed;
                    }




                }
                else if (AppTarget == -1)
                {

                    if (AppWindow2_Target != AppWindowMain_Target && AppWindow2_Target >= 0)
                        (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;

                    AppWindow2Gesture.Visibility = Visibility.Collapsed;
                    //AWALaunchingStoryBoard.Stop();

                    GstBut.Visibility = Visibility.Visible;
                    AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    AppWindowGesture.Opacity = 1;

                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    //AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                    //AWATransform.X = 0;
                    //AWATransform.Y = ActualHeight * FarPoint - ActualHeight * 0.4;
                    //Trace.WriteLine(AppRectGrid1Transform.Y);

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1;

                    AppWindowMain_Target = AppTarget;

                    AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = 0.001;
                    AWABackKeyH.Value = ActualHeight;
                    AWABackKeyW.Value = ActualWidth;
                    AWABackKeyX.Value = 0;
                    AWABackKeyY.Value = ActualHeight * -0.25;// FarPoint - ActualHeight * 0.4;
                    RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerAnimation.To = 500 * (Application.Current as App).ScreenCornerRadius;
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                        AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                    (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";
                    AWFrontIcon.Source = null;
                    AWBackgIcon.Source = null;

                    AWAFrameOpacityDoubleAnimation.From = AWMultiTaskGrid.Opacity;
                    AWAFrameOpacityDoubleAnimation.To = 0;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.4 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.05 * (Application.Current as App).TransitionDurationTime);

                    //AWSwipeBar.Opacity = 1;

                    AWABackStoryBoard.Begin();
                    AWAFrameOpacity.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();
                }
            }
        }

        private void StartSideAnimation(int isGst = 0)
        {
            if((Application.Current as App).EnableSideWindowAnimation != 1)
            {
                return;
            }

            AW2GFallBehindStoryBoard.Stop();
            AW2GScaleFallBehind.ScaleX = AW2GScaleFallBehind.ScaleY = 1.0;
            AW2ABackStoryBoard.Stop();
            AW2AGestureBackStoryBoard.Stop();
            AW2GGestureFlyStoryBoard.Stop();

            if (AppWindow2_Target != AppWindowMain_Target && AppWindow2_Target >= 0)
            {
                (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
            }


            if (isGst == 1)
            {
                AppWindow2Gesture.Visibility = Visibility.Visible;
                AppWindow2Gesture.Opacity = 0.01;
                AW2ATransform.X = AWATransform.X;
                AW2ATransform.Y = AWATransform.Y;
                AW2AScale.ScaleX = AWAScale.ScaleX;
                AW2AScale.ScaleY = AWAScale.ScaleY;
                AW2AScale.CenterX = AWAScale.CenterX;
                AW2AScale.CenterY = AWAScale.CenterY;
                AW2GTransform.X = AWGTransform.X;
                AW2GTransform.Y = AWGTransform.Y;
                AW2GScale.ScaleX = AWGScale.ScaleX;
                AW2GScale.ScaleY = AWGScale.ScaleY;
                AW2GScale.CenterX = AWGScale.CenterX;
                AW2GScale.CenterY = AWGScale.CenterY;

                AppHeight2Animation.Height = AppHeightAnimation.Height;
                AppHeight2Animation.Width = AppHeightAnimation.Width;
                AppHeight2Animation.CornerRadius = AppHeightAnimation.CornerRadius;

                if (AW2CardFrame.Content == null || AW2CardFrame.Content.GetType() != typeof(DesktopCard))
                    AW2CardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                AW2BackgIcon.Source = AWBackgIcon.Source;
                AW2FrontIcon.Source = AWFrontIcon.Source;
                if (AW2CardFrame.Content is DesktopCard && AWCardFrame.Content is DesktopCard)
                {
                    (AW2CardFrame.Content as DesktopCard).GetCardInfo = (AWCardFrame.Content as DesktopCard).GetCardInfo;
                    (AW2CardFrame.Content as DesktopCard).GetImage = (AWCardFrame.Content as DesktopCard).GetImage;
                }
                AW2BackIconScale.ScaleX = AWBackIconScale.ScaleX;
                AW2BackIconScale.ScaleY = AWBackIconScale.ScaleY;
                AW2FrontIconScale.ScaleX = AWFrontIconScale.ScaleX;
                AW2FrontIconScale.ScaleY = AWFrontIconScale.ScaleY;
                AW2BackIconScale.CenterX = AWBackIconScale.CenterX;
                AW2BackIconScale.CenterY = AWBackIconScale.CenterY;
                AW2FrontIconScale.CenterX = AWFrontIconScale.CenterX;
                AW2FrontIconScale.CenterY = AWFrontIconScale.CenterY;

                AW2GBackKeyScaleX1.Value = AWGBackKeyScaleX1.Value;
                AW2GBackKeyScaleX2.Value = AWGBackKeyScaleX2.Value;
                AW2GBackKeyScaleY1.Value = AWGBackKeyScaleY1.Value;
                AW2GBackKeyScaleY2.Value = AWGBackKeyScaleY2.Value;
                AW2GBackKeyX2.Value = AWGBackKeyX2.Value;
                AW2GBackKeyY2.Value = AWGBackKeyY2.Value;

                AW2AGstBackKeyH2.Value = AWAGstBackKeyH2.Value;
                AW2AGstBackKeyW2.Value = AWAGstBackKeyW2.Value;
                AW2AGstBackKeyScaleX1.Value = AWAGstBackKeyScaleX1.Value;
                AW2AGstBackKeyScaleY1.Value = AWAGstBackKeyScaleY1.Value;
                AW2AGstBackKeyScaleX2.Value = AWAGstBackKeyScaleX2.Value;
                AW2AGstBackKeyScaleY2.Value = AWAGstBackKeyScaleY2.Value;
                AW2AGstBackKeyX1.Value = AWAGstBackKeyX1.Value;
                AW2AGstBackKeyX2.Value = AWAGstBackKeyX2.Value;
                AW2AGstBackKeyY1.Value = AWAGstBackKeyY1.Value;
                AW2AGstBackKeyY2.Value = AWAGstBackKeyY2.Value;

                AW2AGstBack2KeyH2.Value = AWAGstBack2KeyH2.Value;
                AW2AGstBack2KeyW2.Value = AWAGstBack2KeyW2.Value;
                AW2AGstBack2KeyScaleX2.Value = AWAGstBack2KeyScaleX2.Value;
                AW2AGstBack2KeyScaleY2.Value = AWAGstBack2KeyScaleY2.Value;
                AW2AGstBack2KeyX2.Value = AWAGstBack2KeyX2.Value;
                AW2AGstBack2KeyY2.Value = AWAGstBack2KeyY2.Value;
                AW2AGstBack2KeyX2BackEase.Amplitude = AWAGstBack2KeyX2BackEase.Amplitude;
                AW2AGstBack2KeyY2BackEase.Amplitude = AWAGstBack2KeyY2BackEase.Amplitude;

                RoundCornerPointer2Animation.From = RoundCornerPointerAnimation.From;
                RoundCornerPointer2Animation.To = RoundCornerPointerAnimation.To;
                AppHeightAnimation.CornerRadius = AppHeightAnimation.CornerRadius;

                AppWindow2_Target = AppWindowMain_Target;

                AW2GBackKeyX2.KeyTime = AWGBackKeyX2.KeyTime;
                AW2GBackKeyY2.KeyTime = AWGBackKeyY2.KeyTime;
                AW2GBackKeyScaleX1.KeyTime = AWGBackKeyScaleX1.KeyTime;
                AW2GBackKeyScaleY1.KeyTime = AWGBackKeyScaleY1.KeyTime;
                AW2GBackKeyScaleX2.KeyTime = AWGBackKeyScaleX2.KeyTime;
                AW2GBackKeyScaleY2.KeyTime = AWGBackKeyScaleY2.KeyTime;


                if ((Application.Current as App).CurveStyle == 0)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBack2StoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 1)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBackStoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
            }
            else
            {
                AppWindow2Gesture.Visibility = Visibility.Visible;
                AppWindow2Gesture.Opacity = 0.01;
                AW2ATransform.X = AWATransform.X;
                AW2ATransform.Y = AWATransform.Y;
                AW2AScale.ScaleX = AWAScale.ScaleX;
                AW2AScale.ScaleY = AWAScale.ScaleY;
                AW2AScale.CenterX = AWAScale.CenterX;
                AW2AScale.CenterY = AWAScale.CenterY;
                AW2GTransform.X = AWGTransform.X;
                AW2GTransform.Y = AWGTransform.Y;
                AW2GScale.ScaleX = AWGScale.ScaleX;
                AW2GScale.ScaleY = AWGScale.ScaleY;
                AW2GScale.CenterX = AWGScale.CenterX;
                AW2GScale.CenterY = AWGScale.CenterY;

                AppHeight2Animation.Height = AppHeightAnimation.Height;
                AppHeight2Animation.Width = AppHeightAnimation.Width;
                AppHeight2Animation.CornerRadius = AppHeightAnimation.CornerRadius;

                if (AW2CardFrame.Content == null || AW2CardFrame.Content.GetType() != typeof(DesktopCard))
                    AW2CardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                AW2BackgIcon.Source = AWBackgIcon.Source;
                AW2FrontIcon.Source = AWFrontIcon.Source;
                if (AW2CardFrame.Content is DesktopCard && AWCardFrame.Content is DesktopCard)
                {
                    (AW2CardFrame.Content as DesktopCard).GetCardInfo = (AWCardFrame.Content as DesktopCard).GetCardInfo;
                    (AW2CardFrame.Content as DesktopCard).GetImage = (AWCardFrame.Content as DesktopCard).GetImage;
                }
                AW2BackIconScale.ScaleX = AWBackIconScale.ScaleX;
                AW2BackIconScale.ScaleY = AWBackIconScale.ScaleY;
                AW2FrontIconScale.ScaleX = AWFrontIconScale.ScaleX;
                AW2FrontIconScale.ScaleY = AWFrontIconScale.ScaleY;
                AW2BackIconScale.CenterX = AWBackIconScale.CenterX;
                AW2BackIconScale.CenterY = AWBackIconScale.CenterY;
                AW2FrontIconScale.CenterX = AWFrontIconScale.CenterX;
                AW2FrontIconScale.CenterY = AWFrontIconScale.CenterY;

                AW2ABackKeyH.Value = AWABackKeyH.Value;
                AW2ABackKeyW.Value = AWABackKeyW.Value;
                AW2ABackKeyScaleX.Value = AWABackKeyScaleX.Value;
                AW2ABackKeyScaleY.Value = AWABackKeyScaleY.Value;
                AW2ABackKeyX.Value = AWABackKeyX.Value;
                AW2ABackKeyY.Value = AWABackKeyY.Value;

                RoundCornerPointer2Animation.From = RoundCornerPointerAnimation.From;
                RoundCornerPointer2Animation.To = RoundCornerPointerAnimation.To;
                AppHeightAnimation.CornerRadius = AppHeightAnimation.CornerRadius;

                AppWindow2_Target = AppWindowMain_Target;

                AW2ABackStoryBoard.Begin();
                RoundCornerPointer2StBo.Begin();
            }
        }

        private void AppIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) //图标按下
        {
            if (AppWindowState == 1)
            {
                return;
            }

            while (AWMultiTaskGrid.Children.Count > 0)
            {
                var OnBackgroundingApp = (AWMultiTaskGrid.Children[0] as Frame);
                AWMultiTaskGrid.Children.RemoveAt(0);
            }/*
            (Application.Current as App).MultiAppInfos[AppWindowMain_mIndex].AppFrame.Content = null;
            (Application.Current as App).MultiAppInfos[AppWindowMain_mIndex].AppFrame = null;
            (Application.Current as App).MultiAppInfos.RemoveAt(AppWindowMain_mIndex);
            AppWindowMain_mIndex = -1;*/

            int target = DesktopGrid.Children.IndexOf(((sender as Button).Parent as Grid));
            OnRunningAppInfo ToRunAppInfo = new OnRunningAppInfo { AppFrame = null, AppIconPos = target, AppPackageName = ((DesktopGrid.Children[target] as Grid)).Tag != null ? ((DesktopGrid.Children[target] as Grid)).Tag.ToString() : (DateTime.Now.ToString()) };
            int ToRunAppIndex = -1, AppWindowMain_ToIndex = 0;
            foreach (var RunningApp in (Application.Current as App).MultiAppInfos)
            {
                if (RunningApp != null && (RunningApp.AppIconPos == ToRunAppInfo.AppIconPos || RunningApp.AppPackageName == ToRunAppInfo.AppPackageName))
                {
                    ToRunAppIndex = (Application.Current as App).MultiAppInfos.IndexOf(RunningApp);
                    AppWindowMain_ToIndex = ToRunAppIndex;
                    break;
                }
            }
            if(ToRunAppIndex == -1)
            {
                ToRunAppInfo.AppFrame = new Frame();
                if (((DesktopGrid.Children[target] as Grid)).Tag != null && ((DesktopGrid.Children[target] as Grid)).Tag.ToString().Contains("com.android.settings"))
                {
                    ToRunAppInfo.AppFrame.Navigate(typeof(SettingsApp), null, new SuppressNavigationTransitionInfo());
                }
                else if (((DesktopGrid.Children[target] as Grid)).Tag != null && ((DesktopGrid.Children[target] as Grid)).Tag.ToString().Contains("com.android.camera"))
                {
                    ToRunAppInfo.AppFrame.Navigate(typeof(CameraApp), null, new SuppressNavigationTransitionInfo());
                }
                else
                {
                    ToRunAppInfo.AppFrame.Navigate(typeof(BlankPage), null, new SuppressNavigationTransitionInfo());
                }
                (Application.Current as App).MultiAppInfos.Add(ToRunAppInfo);
                AppWindowMain_ToIndex = (Application.Current as App).MultiAppInfos.IndexOf(ToRunAppInfo);
            }
            AppWindowMain_mIndex = AppWindowMain_ToIndex;

            AWMultiTaskGrid.Children.Add(ToRunAppIndex == -1 ? ToRunAppInfo.AppFrame : (Application.Current as App).MultiAppInfos[ToRunAppIndex].AppFrame);

            if (target == 8)
                target = -1 ;

            StartWindowAnimation(1, /*-1*/target);
            StartBackgroundAnimation(1);
        }

        private void DesktopGrid_Loaded(object sender, RoutedEventArgs e) // 放置桌面图标
        {
            GetCustomBackground();

            int DskIconSize = 500;
            DesktopGrid.ItemHeight = 0.16 * 1.4 * DskIconSize;
            DesktopGrid.ItemWidth = 0.16 * 1.4 * DskIconSize;
            DesktopGrid.Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.2 * DskIconSize, 0.16 * 0.8 * DskIconSize, 0.16 * 0.2 * DskIconSize, 0.16 * 0.2 * DskIconSize);

            var IconList = new List<DesktopIconInfo>
            {
                new DesktopIconInfo
                {
                    Tag = "com.android.deskclock",
                    BgSource = "",
                    FgSource = "com.android.deskclock.deskclock",
                    ColumnSpan = 4,
                    RowSpan = 2
                },
                new DesktopIconInfo { Tag = "com.android.settings", BgSource = "ms-appx:///Assets/IconRes/com.android.settings/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.settings/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.email/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.email/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.example.android.notepad/background.png", FgSource = "ms-appx:///Assets/IconRes/com.example.android.notepad/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.filemanager/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.filemanager/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.himovie/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.himovie/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.music/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.music/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.calculator2/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.calculator2/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.appmarket/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.appmarket/foreground.png" },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/230620-Wide.png",
                    ColumnSpan = 2,
                    RowSpan = 2
                },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.health/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.health/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.smarthome/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.smarthome/foreground.png" },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/img35.jpg",
                    ColumnSpan = 2,
                    RowSpan = 1
                },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.meetime/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.meetime/foreground.png" },
                new DesktopIconInfo { Tag = "com.android.camera", BgSource = "ms-appx:///Assets/IconRes/com.android.camera/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.camera/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.gallery3d/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.gallery3d/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.browser/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.browser/foreground.png" },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/03_gettyimages-591774121_super_resized.jpg",
                    ColumnSpan = 2,
                    RowSpan = 1
                },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.soundrecorder/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.soundrecorder/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.android.thememanager/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.android.thememanager/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.android.tips/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.android.tips/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.compass/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.compass/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.deskclock/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.deskclock/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.wallet/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.wallet/foreground.png" },

            };

            foreach (var IconInfo in IconList)
            {
                var grid = new Grid
                {
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    BorderThickness = new Thickness(IconInfo.ColumnSpan + IconInfo.RowSpan > 2 ? 1.0 : 0.5), //0.5是图标 1是卡片
                    Tag = IconInfo.Tag,
                    CornerRadius = new Windows.UI.Xaml.CornerRadius(DskIconSize * 100 / 1920 * 0.8),
                    Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.12 * DskIconSize),

                };
                VariableSizedWrapGrid.SetColumnSpan(grid, IconInfo.ColumnSpan);
                VariableSizedWrapGrid.SetRowSpan(grid, IconInfo.RowSpan);

                if (IconInfo.ColumnSpan + IconInfo.RowSpan > 2)
                {
                    var CardFrame = new Frame
                    {
                        Content = new DesktopCard(),
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                    };
                    grid.Children.Add(CardFrame);
                    (CardFrame.Content as DesktopCard).GetCardInfo = IconInfo.FgSource.Contains("ms-appx://") ? "Pic" : IconInfo.FgSource;
                    if (IconInfo.FgSource.Contains("ms-appx://"))
                    {
                        (CardFrame.Content as DesktopCard).GetImage = new Image { Source = new BitmapImage(new Uri(IconInfo.FgSource)) };
                    }
                }
                else
                {
                    var BackgroundIcon = new Image
                    {
                        Source = IconInfo.BgSource == "" ? null : (new BitmapImage(new Uri(IconInfo.BgSource))),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Stretch = Stretch.UniformToFill,
                        RenderTransform = new ScaleTransform { CenterX = 0, CenterY = 0, ScaleX = 1.2, ScaleY = 1.2 }
                    };
                    var ForegroundIcon = new Image
                    {
                        Source = IconInfo.FgSource == "" ? null : (new BitmapImage(new Uri(IconInfo.FgSource))),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Stretch = Stretch.UniformToFill,
                        RenderTransform = new ScaleTransform { CenterX = 0, CenterY = 0, ScaleX = 1.2, ScaleY = 1.2 }
                    };

                    grid.Children.Add(BackgroundIcon);
                    grid.Children.Add(ForegroundIcon);
                }

                var button = new Button
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(-4),
                    Opacity = 0.5,
                };
                button.Click += AppIcon_Click;

                grid.Children.Add(button);

                DesktopGrid.Children.Add(grid);
            }

            foreach (var AppIconGrid in DesktopGrid.Children)
            {
                //图标
                if (AppIconGrid.GetType() == typeof(Grid) && (AppIconGrid as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5))
                {
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterY = ((AppIconGrid as Grid).Children[0] as Image).ActualWidth * 0.5;
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.5;
                }
            }
        }

        private void AWReturnToApp_Click(object sender, RoutedEventArgs e)
        {
            if (AppWindowState == 1)
            {
                return;
            }
            AWReturnToApp.Visibility = Visibility.Collapsed;
            StartWindowAnimation(1, AppWindowMain_Target);
            StartBackgroundAnimation(1);
            SetSwipeBarColor(2);
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            foreach (var AppIconGrid in DesktopGrid.Children)
            {
                //图标
                if (AppIconGrid.GetType() == typeof(Grid) && (AppIconGrid as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5))
                {
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterY = ((AppIconGrid as Grid).Children[0] as Image).ActualWidth * 0.5;
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.5;
                }
            }
            WpScaleT.CenterX = Wallpaper.ActualWidth * 0.5;
            WpScaleT.CenterY = Wallpaper.ActualHeight * 0.5;
            DskIconScaleT.CenterX = PageOutline.ActualWidth * 0.5;
            DskIconScaleT.CenterY = PageOutline.ActualHeight * 0.5;

            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
            AWFrontIcon.Width = ActualWidth <= ActualHeight ? ActualWidth : ActualHeight;
            AWFrontIcon.Height = ActualWidth > ActualHeight ? ActualHeight : ActualWidth;
            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = AWFrontIcon.Width * 0.5;
            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = AWFrontIcon.Height * 0.5;

            if (sender != null)
            {
                AppWindowGesture.Width = AppHeightAnimation.Width = ActualWidth;
                AppWindowGesture.Height = AppHeightAnimation.Height = ActualHeight;
                //AWGScale.ScaleX = AWGScale.ScaleY = 1;
                AWFrame.Width = AWMultiTaskGrid.Width = ActualWidth;
                AWFrame.Height = AWMultiTaskGrid.Height = ActualHeight; 

                if (ActualWidth >= ActualHeight)
                {

                }
                else
                {

                }


                if((Application.Current as App).EnableSideWindowAnimation == 1)
                {
                    AW2BackIconScale.ScaleX = AW2BackIconScale.ScaleY = AW2FrontIconScale.ScaleX = AW2FrontIconScale.ScaleY = 1.5;
                    AW2FrontIcon.Width = ActualWidth <= ActualHeight ? ActualWidth : ActualHeight;
                    AW2FrontIcon.Height = ActualWidth > ActualHeight ? ActualHeight : ActualWidth;
                    AW2BackIconScale.CenterX = AW2FrontIconScale.CenterX = AW2FrontIcon.Width * 0.5;
                    AW2BackIconScale.CenterY = AW2FrontIconScale.CenterY = AW2FrontIcon.Height * 0.5;
                    AppWindow2Gesture.Width = AppHeight2Animation.Width = ActualWidth;
                    AppWindow2Gesture.Height = AppHeight2Animation.Height = ActualHeight;

                    AW2GScaleFallBehind.CenterX = ActualWidth * 0.5;
                    AW2GScaleFallBehind.CenterY = ActualHeight * 0.5;
                }
            }
        }

        public void StartBackgroundAnimation(int isOnLaunching = 0)
        {
            if (isOnLaunching == 1)
            {
                WpKeyFrameX.Value = (Application.Current as App).EnableBgScale == 1 ? 1.1 : 1;
                WpKeyFrameY.Value = (Application.Current as App).EnableBgScale == 1 ? 1.1 : 1;
                DskIconKeyFrameX.Value = (Application.Current as App).EnableBgScale == 1 ? 0.9 : 1;
                DskIconKeyFrameY.Value = (Application.Current as App).EnableBgScale == 1 ? 0.9 : 1;
                BlurAnSet.From = BlurPointerTransform.X;
                if((Application.Current as App).EnableBgBlur == 1)
                {
                    BlurAnSet.To = 20;
                    BlurPointerAnimation.To = 20;
                    if(BlurPointerTransform.X == 0)
                    {
                        BlurPointerTransform.X = 20;
                    }
                }
                else if ((Application.Current as App).EnableBgBlur == 0)
                {
                    BlurAnSet.To = 0;
                    BlurPointerAnimation.To = 0;

                    if (BlurPointerTransform.X == 20)
                    {
                        BlurPointerTransform.X = 0;
                    }
                }
                BlurAnSet.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                BlurPointerAnimation.From = BlurPointerTransform.X;
                BlurPointerAnimation.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                GstBut.Visibility = Visibility.Visible;
            }
            else if(isOnLaunching == 0)
            {
                WpKeyFrameX.Value = 1.0;
                WpKeyFrameY.Value = 1.0;
                DskIconKeyFrameX.Value = 1.0;
                DskIconKeyFrameY.Value = 1.0;
                BlurAnSet.From = BlurPointerTransform.X;
                BlurAnSet.To = 0;
                BlurAnSet.Duration = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                BlurPointerAnimation.From = BlurPointerTransform.X;
                BlurPointerAnimation.To = 0;
                BlurPointerAnimation.Duration = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
            }
            
            BlurPointerStoryBoard.Begin();
            BlurAnimation.Start();
            DskIconGridStoryBoard.Begin();
            WpStoryBoard.Begin();
            
        }


        private void GstBut_Click(object sender, RoutedEventArgs e)
        {
            if (AppWindowState == 2 || AppWindowState == 0)
            {
                return;
            }

            StartWindowAnimation(0, AppWindowMain_Target);
            AWGGestureFillStoryBoard.Begin();
            StartBackgroundAnimation(0);
            SetSwipeBarColor(0);
            return;
        }

        public async void SetSwipeBarColor(int i = 0) //0def 1white 2black
        {
            if (i == 0)
            {
                Frame.RequestedTheme = ElementTheme.Default;
                AWSwipeBar.RequestedTheme = ElementTheme.Default;
            }
            else
            {
                if(AWMultiTaskGrid.Children.Count != 0 && AWMultiTaskGrid.Children[0].GetType() == typeof(Frame))
                {
                    AWSwipeBar.RequestedTheme = (AWMultiTaskGrid.Children[0] as Frame).RequestedTheme;
                }
            }
            await Task.Delay(TimeSpan.FromSeconds((Application.Current as App).TransitionDurationTime * 0.5));
            (((MP.Content as Grid).Children[1] as Frame).Content as QuickControlPanel).SetStatusBarColor(AWSwipeBar.RequestedTheme);
        }

        //public ElementTheme GetSwipeBarColorTheme { get { return AWSwipeBar.RequestedTheme; } }

        private void RoundCornerTick(object? sender, object e)
        {
            if(AWAScale.ScaleX != 1.0)
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            else
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);

            if((Application.Current as App).EnableSideWindowAnimation == 1)
            {
                AppHeight2Animation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointer2Transform.X);
            }
        }

        private void AWALaunchingStoryBoard_Completed(object sender, object e)
        {
            if(AppWindowState == 1)
            {
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
                RoundCornerTimer.Stop();
                if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                    AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";
                AWBackgIcon.Opacity = 0;
                AWFrontIcon.Opacity = 0;
            }
        }

        private void AWABackStoryBoard_Completed(object sender, object e)
        {
            if (AppWindowState == 2 && AppWindowMain_Target >= 0)
            {
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
                AppWindowState = 0;
                AWGTransform.X = AWGTransform.Y = 0;
                AWGScale.ScaleX = AWGScale.ScaleY = 1.0;
            }
            else if(AppWindowState == 2)
            {
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AppWindowState = 0;
                AWGTransform.X = AWGTransform.Y = 0;
                AWGScale.ScaleX = AWGScale.ScaleY = 1.0;
            }
        }
        private void AW2ABackStoryBoard_Completed(object sender, object e)
        {
            if (AppWindow2_Target >= 0)
            {
                AppWindow2Gesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
            }
        }

        private void AWAFrameOpacity_Completed(object sender, object e)
        {
            if(AppWindowState == 1)
                AWReturnToApp.Visibility = Visibility.Collapsed;
        }

        private void GstBut_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        }


        double MouseDownX = -1, MouseDownY = -1;
        double MouseX = -1, MouseY = -1;
        double mH = 0;

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            AWMultiTaskGrid.Children.Clear();
        }

        private void DesktopGridScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if(AppWindowState == 2 && AppWindowMain_Target != -1)
                StartWindowAnimation(-1);
            AppWindow2Gesture.Visibility = Visibility.Collapsed;
        }

        double FarPoint = 0.2;
        private void GstBut_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (AppWindowState == 2)
            {
                return;
            }

            FarPoint = 0.15;
            if (true)
            {
                AWGScale.CenterX = ActualWidth / 2.0;
                AWGScale.CenterY = ActualHeight / 2.0;

                MouseX = e.Position.X;
                MouseY = e.Position.Y;
                double mY = ActualHeight + (MouseY - MouseDownY);
                mH = (mY - ActualHeight * FarPoint) * (ActualHeight / (ActualHeight - ActualHeight * FarPoint));
                double a = ActualHeight * 0.1;
                if (mH < ActualHeight * 0.4)
                {
                    a = ActualHeight * 0.3;
                    mH = -(a * (-1 / (Math.Abs(mH - ActualHeight * 0.4) / a + 1) + 1)) + ActualHeight * 0.4;
                }
                if (mH <= ActualHeight * 0.05)
                {
                    mH = ActualHeight * 0.05;
                }
                if (mH > ActualHeight * 1.0)
                {
                    a = ActualHeight * 0.1;
                    mH = (a * (-1 / (Math.Abs(mH - ActualHeight * 1.0) / a + 1) + 1)) + ActualHeight * 1.0;
                }
                if (mH >= ActualHeight * 1.1)
                {
                    mH = ActualHeight * 1.1;
                }
                AWGScale.ScaleX = AWGScale.ScaleY = mH / ActualHeight;
                AWGTransform.X = MouseX - (AWGScale.ScaleX) * (MouseDownX - 0.5 * ActualWidth) - 0.5 * ActualWidth;
                AWGTransform.Y = -0.5 * ActualHeight + FarPoint * ActualHeight + (0.5 - FarPoint) * mH;
            }
        }


        private void GstBut_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            if(AppWindowState != 1)
            {
                return;
            }

            if ((e.Velocities.Linear.Y <= -0.1))
            {
                SetSwipeBarColor(0);
                //GstBut.Visibility = Visibility.Collapsed;
                MouseX = e.Position.X;
                MouseY = e.Position.Y;
                try
                {
                    double dH = ((ActualHeight + (MouseY - MouseDownY) - Math.Pow(e.Velocities.Linear.Y, 1/5) * 1 * (Application.Current as App).FlyFar - ActualHeight * FarPoint) * (ActualHeight / (ActualHeight - ActualHeight * FarPoint)));
                    double a = ActualHeight * 0.1;
                    if (dH < ActualHeight * 0.4)
                    {
                        a = ActualHeight * 0.3;
                        dH = -(a * (-1 / (Math.Abs(dH - ActualHeight * 0.4) / a + 1) + 1)) + ActualHeight * 0.4;
                    }
                    if (dH <= ActualHeight * 0.05)
                    {
                        dH = ActualHeight * 0.05;
                    }
                    if (dH > ActualHeight * 1.0)
                    {
                        a = ActualHeight * 0.1;
                        dH = (a * (-1 / (Math.Abs(dH - ActualHeight * 1.0) / a + 1) + 1)) + ActualHeight * 1.0;
                    }
                    if (dH >= ActualHeight * 1.1)
                    {
                        dH = ActualHeight * 1.1;
                    }
                    //FarPoint = -0.2;
                    AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = AWGScale.ScaleX * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH);
                    if (AWGBackKeyScaleX1.Value < 0.01 && AppWindowMain_Target != -1)
                    {
                        AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = 0.01;
                    }
                }
                catch
                {
                    AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = AWGScale.ScaleX *1;
                    if (AWGBackKeyScaleX1.Value < 0.01 && AppWindowMain_Target != -1)
                    {
                        AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = 0.01;
                    }
                }

                AWGBackKeyScaleX2.Value = AWGBackKeyScaleY2.Value = 1.0;

                AWGBackKeyY2.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AWGBackKeyScaleY1.Value * (0.5 - FarPoint);
                try
                {
                    AWGBackKeyX2.Value = AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 1 / 2) * (Application.Current as App).FlyFar * 4;
                }
                catch { }
                
                if((Application.Current as App).CurveStyle == 0)
                {
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                }
                else if((Application.Current as App).CurveStyle == 1)
                {
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                }
                AWGGestureFlyStoryBoard.Begin();

                
                StartWindowAnimation(0, AppWindowMain_Target);
                StartBackgroundAnimation(0);

                if ((Application.Current as App).CurveStyle == 0)
                {
                    AWAGstBack2KeyX2.Value = AWABackKeyX.Value - AWGBackKeyX2.Value;
                    AWAGstBack2KeyY2.Value = AWABackKeyY.Value - AWGBackKeyY2.Value;
                    AWAGstBack2KeyH2.Value = AWABackKeyH.Value;
                    AWAGstBack2KeyW2.Value = AWABackKeyW.Value;

                    if (AppWindowMain_Target >= 0 && (DesktopGrid.Children[AppWindowMain_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppWindowMain_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        AWAGstBack2KeyX2BackEase.Amplitude = AWAGstBack2KeyY2BackEase.Amplitude = 0.15;
                    }
                    else if (AppWindowMain_Target >= 0) //卡片
                    {
                        AWAGstBack2KeyX2BackEase.Amplitude = AWAGstBack2KeyY2BackEase.Amplitude = 0.15;
                    }
                    else
                    {
                        AWAGstBack2KeyX2BackEase.Amplitude = AWAGstBack2KeyY2BackEase.Amplitude = 0.01;
                    }
                    AWAGstBack2KeyScaleX2.Value = AWABackKeyScaleX.Value;
                    AWAGstBack2KeyScaleY2.Value = AWABackKeyScaleY.Value;
                    AWAGestureBack2StoryBoard.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 1)
                {
                    double distance = Math.Sqrt(Math.Pow((AWABackKeyY.Value - AWGBackKeyY2.Value), 2) + Math.Pow((AWABackKeyX.Value - AWGBackKeyX2.Value), 2));
                    double b = ((Application.Current as App).BounceRadius);
                    double BackEaseV = (1 * Math.Sqrt(Math.Pow(e.Velocities.Linear.Y, 2) + Math.Pow(e.Velocities.Linear.X, 2))) * 10;
                    double BackEaseRound = (b * (-1 / (Math.Abs(BackEaseV - 0) / b + 1) + 1)) + 0;
                    AWAGstBackKeyX1.Value = (AWABackKeyX.Value - AWGBackKeyX2.Value) * (distance + BackEaseRound) / distance;
                    AWAGstBackKeyY1.Value = (AWABackKeyY.Value - AWGBackKeyY2.Value) * (distance + BackEaseRound) / distance;
                    AWAGstBackKeyX2.Value = AWABackKeyX.Value - AWGBackKeyX2.Value;
                    AWAGstBackKeyY2.Value = AWABackKeyY.Value - AWGBackKeyY2.Value;
                    AWAGstBackKeyH2.Value = AWABackKeyH.Value;
                    AWAGstBackKeyW2.Value = AWABackKeyW.Value;
                    if (AppWindowMain_Target >= 0 && b > 4 && (DesktopGrid.Children[AppWindowMain_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppWindowMain_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 15 / 16.0;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 15 / 16.0;
                    }
                    else if (AppWindowMain_Target >= 0) //卡片
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 16 / 16.0;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 16 / 16.0;
                    }
                    else
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 0.01;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 0.01;
                    }
                    AWAGstBackKeyScaleX2.Value = AWABackKeyScaleX.Value;
                    AWAGstBackKeyScaleY2.Value = AWABackKeyScaleY.Value;
                    AWAGestureBackStoryBoard.Begin();
                }

                AWABackStoryBoard.Stop();

                StartSideAnimation(1);
            }
            else if (e.Velocities.Linear.Y >= -2 || true)
            {
                GstBut.Visibility = Visibility.Visible;
                AWGScale.CenterX = ActualWidth * 0.5;
                AWGScale.CenterY = ActualHeight * 0.5;
                AWGGestureFillStoryBoard.Begin();
            }
        }


        private void GstBut_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            if (AppWindowState == 2)
            {
                return;
            }

            AWGGestureFillStoryBoard.Stop();
            GstBut.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.TranslateX | Windows.UI.Xaml.Input.ManipulationModes.TranslateY;

            if (true)
            {
                MouseDownX = e.Position.X;
                MouseDownY = e.Position.Y;
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

            }
        }

        private async void GetCustomBackground()
        {
            if ((Application.Current as App).iUseCustomBackground)
            {
                try
                {
                    Windows.Storage.StorageFolder StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    StorageFile file = await StorageFolder.GetFileAsync("Customize\\Background.png");
                    if (file != null)
                    {
                        using (IRandomAccessStream FileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            await bitmapImage.SetSourceAsync(FileStream);
                            Wallpaper.Source = bitmapImage;
                            bitmapImage = null;
                        }
                    }
                    file = null;
                }
                catch { }
            }
            else
            {
                //Wallpaper.Source = new BitmapImage(new Uri("ms-appx:///Assets/IconRes/home_wallpaper03.jpg"));
            }
        }
    }
}
