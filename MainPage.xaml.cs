using CommunityToolkit.WinUI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
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

        public System.TimeSpan TrDur05 { get { return System.TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime); } set { } }
        public System.TimeSpan TrDur07 { get { return System.TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime); } set { } }
        public System.TimeSpan TrDur075 { get { return System.TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime); } set { } }
        public Frame GetAppFrame { get { return AWFrame; } }

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


        public MainPage()
        {
            TrDur05 = TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
            TrDur07 = TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);
            TrDur075 = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);

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


        public int AppWindowMain_Target = -2, AppWindowMain_mIndex = -2, AppWindowState = 0;
        private async void StartWindowAnimation(int isOnLaunching = 0, int AppTarget = -2)
        {
            AppWindowGesture.Visibility = Visibility.Visible; 
            if (AppWindowMain_Target >= 0 && AppTarget != AppWindowMain_Target)
            {
                //进入并行
                (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;

            }

            if (isOnLaunching == 1)
            {
                GstBut.Visibility = Visibility.Visible;
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppWindowGesture.Opacity = 1;

                if (AppWindowMain_Target >= 0 && AppTarget != AppWindowMain_Target)
                {
                    //进入并行
                    (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;

                }
                AppWindowState = 1;

                if (AppTarget != -2 && AppTarget != -1)
                {
                    /*if (AppRect_Target >= 0)
                        (DesktopGrid.Children[AppRect_Target] as Grid).Opacity = 1;*/
                    //AWABackStoryBoard.Stop();

                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    if(AppTarget != AppWindowMain_Target)
                    {
                        AWABackStoryBoard.Stop();

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
                            AWFrontIcon.Source = null ;
                        }

                        AWFrontIcon.Opacity = 1; 
                        AWBackgIcon.Opacity = 1;
                        if (DesktopGrid.Children[AppTarget].GetType() == typeof(Grid) && ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth/(DesktopGrid.Children[AppTarget] as Grid).ActualHeight) <= (ActualWidth / ActualHeight))
                        {
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AppHeightAnimation.Height = ActualHeight;
                            AppHeightAnimation.Width = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;
                            
                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                        }
                        else
                        {
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5; 
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AppHeightAnimation.Width = ActualWidth;
                            AppHeightAnimation.Height = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;
                            
                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                        }

                        AWAScale.CenterX = ActualWidth * 0.5;
                        AWAScale.CenterY = ActualHeight * 0.5;
                        AWATransform.X = -(ActualWidth - ActualWidth * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + AWAScale.ScaleX * (AppHeightAnimation.Width - ActualWidth) / 2.0;
                        AWATransform.Y = -(ActualHeight - ActualHeight * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 + AWAScale.ScaleX * (AppHeightAnimation.Height - ActualHeight) / 2.0;
                        RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);

                        AWMultiTaskGrid.Opacity = 0;
                    }
                    else
                    {
                        
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
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AppHeightAnimation.Height = ActualHeight;
                            AppHeightAnimation.Width = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                        }
                        else
                        {
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5;
                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AppHeightAnimation.Width = ActualWidth;
                            AppHeightAnimation.Height = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                        }

                        AWAScale.CenterX = ActualWidth * 0.5;
                        AWAScale.CenterY = ActualHeight * 0.5;
                        AWATransform.X = -(ActualWidth - ActualWidth * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + AWAScale.ScaleX * (AppHeightAnimation.Width - ActualWidth) / 2.0;
                        AWATransform.Y = -(ActualHeight - ActualHeight * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 + AWAScale.ScaleX * (AppHeightAnimation.Height - ActualHeight) / 2.0;
                        RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);

                        AWMultiTaskGrid.Opacity = 0;
                    }

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;

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
                else if(AppTarget == -1)
                {
                    /*if (AppRect_Target >= 0) //并行要写这个
                        (DesktopGrid.Children[AppRect_Target] as Grid).Opacity = 1;*/
                    AWABackStoryBoard.Stop();

                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                    AWATransform.X = 0;
                    AWATransform.Y = ActualHeight * FarPoint - ActualHeight * 0.4;
                    //Trace.WriteLine(AppRectGrid1Transform.Y);

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;

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
                }


            }
            else if(isOnLaunching == 0)
            {
                AppWindowState = 2;
                if (AppTarget != -1 && AppTarget != -2)
                {
                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;

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
                        AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                        AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                        AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                        AWABackKeyH.Value = ActualHeight;
                        AWABackKeyW.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                        AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                    }
                    else
                    {
                        AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                        AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5;
                        AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                        AWABackKeyW.Value = ActualWidth;
                        AWABackKeyH.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;

                        AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                    } 


                    AppWindowMain_Target = AppTarget;

                    //AWABackKeyScaleX.Value = AWAOpenKeyScaleY.Value = 0;
                    //AWABackKeyH.Value = ActualHeight;
                    //AWABackKeyW.Value = ActualWidth;
                    AWABackKeyX.Value = -(ActualWidth - ActualWidth * AWABackKeyScaleX.Value) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + (AWABackKeyW.Value - ActualWidth) * AWABackKeyScaleX.Value / 2.0;
                    AWABackKeyY.Value = -(ActualHeight - ActualHeight * AWABackKeyScaleX.Value) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 +  (AWABackKeyH.Value - ActualHeight) * AWABackKeyScaleX.Value / 2.0;
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
                }
                else if (AppTarget == -1)
                {
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

                    AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = 0.01;
                    AWABackKeyH.Value = ActualHeight;
                    AWABackKeyW.Value = ActualWidth;
                    AWABackKeyX.Value = 0;
                    AWABackKeyY.Value = ActualHeight * FarPoint - ActualHeight * 0.4;
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
            MultiAppInfos[AppWindowMain_mIndex].AppFrame.Content = null;
            MultiAppInfos[AppWindowMain_mIndex].AppFrame = null;
            MultiAppInfos.RemoveAt(AppWindowMain_mIndex);
            AppWindowMain_mIndex = -1;*/

            int target = DesktopGrid.Children.IndexOf(((sender as Button).Parent as Grid));
            OnRunningAppInfo ToRunAppInfo = new OnRunningAppInfo { AppFrame = null, AppIconPos = target, AppPackageName = ((DesktopGrid.Children[target] as Grid)).Tag != null ? ((DesktopGrid.Children[target] as Grid)).Tag.ToString() : (DateTime.Now.ToString()) };
            int ToRunAppIndex = -1, AppWindowMain_ToIndex = 0;
            foreach (var RunningApp in MultiAppInfos)
            {
                if (RunningApp != null && (RunningApp.AppIconPos == ToRunAppInfo.AppIconPos || RunningApp.AppPackageName == ToRunAppInfo.AppPackageName))
                {
                    ToRunAppIndex = MultiAppInfos.IndexOf(RunningApp);
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
                MultiAppInfos.Add(ToRunAppInfo);
                AppWindowMain_ToIndex = MultiAppInfos.IndexOf(ToRunAppInfo);
            }
            AppWindowMain_mIndex = AppWindowMain_ToIndex;

            AWMultiTaskGrid.Children.Add(ToRunAppIndex == -1 ? ToRunAppInfo.AppFrame : MultiAppInfos[ToRunAppIndex].AppFrame);

            if (target == 0)
                target = 0;//-1 ;//做不出来

            StartWindowAnimation(1, /*-1*/target);
            StartBackgroundAnimation(1);
        }

        private void DesktopGrid_Loaded(object sender, RoutedEventArgs e) // 放置桌面图标
        {
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
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.health/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.health/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.smarthome/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.smarthome/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.huawei.meetime/background.png", FgSource = "ms-appx:///Assets/IconRes/com.huawei.meetime/foreground.png" },
                new DesktopIconInfo { Tag = "com.android.camera", BgSource = "ms-appx:///Assets/IconRes/com.android.camera/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.camera/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.gallery3d/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.gallery3d/foreground.png" },
                new DesktopIconInfo { BgSource = "ms-appx:///Assets/IconRes/com.android.browser/background.png", FgSource = "ms-appx:///Assets/IconRes/com.android.browser/foreground.png" },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/230620-Wide.png",
                    ColumnSpan = 2,
                    RowSpan = 2
                },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/img35.jpg",
                    ColumnSpan = 2,
                    RowSpan = 1
                },
                new DesktopIconInfo
                {
                    BgSource = "",
                    FgSource = "ms-appx:///Assets/CardRes/03_gettyimages-591774121_super_resized.jpg",
                    ColumnSpan = 4,
                    RowSpan = 2
                },
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
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.55;
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
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            foreach (var AppIconGrid in DesktopGrid.Children)
            {
                //图标
                if (AppIconGrid.GetType() == typeof(Grid) && (AppIconGrid as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5))
                {
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterY = ((AppIconGrid as Grid).Children[0] as Image).ActualWidth * 0.5;
                    (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleY = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((AppIconGrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.55;
                }
            }
            WpScaleT.CenterX = Wallpaper.ActualWidth * 0.5;
            WpScaleT.CenterY = Wallpaper.ActualHeight * 0.5;
            DskIconScaleT.CenterX = PageOutline.ActualWidth * 0.5;
            DskIconScaleT.CenterY = PageOutline.ActualHeight * 0.5;

            if (AppRect_Target >= 0 && (DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
            {
                AWBackIconScale.CenterX = AWFrontIconScale.CenterX = AWBackgIcon.ActualWidth * 0.5;
                AWBackIconScale.CenterY = AWFrontIconScale.CenterY = AWBackgIcon.ActualHeight * 0.5;
                AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
            }
            else if (AppRect_Target >= 0 && (DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
            {
                AWBackIconScale.CenterX = AWFrontIconScale.CenterX = AWBackgIcon.ActualWidth * 0.5;
                AWBackIconScale.CenterY = AWFrontIconScale.CenterY = AWBackgIcon.ActualHeight * 0.5;
                AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
            }

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
            }
        }

        public void StartBackgroundAnimation(int isOnLaunching = 0)
        {
            if (isOnLaunching == 1)
            {
                WpKeyFrameX.Value = 1.1;
                WpKeyFrameY.Value = 1.1;
                DskIconKeyFrameX.Value = 0.9;
                DskIconKeyFrameY.Value = 0.9;
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
            StartWindowAnimation(0, AppWindowMain_Target);
            StartBackgroundAnimation(0);
            SetSwipeBarColor(0);
            return;
        }

        public void SetSwipeBarColor(int i = 0) //0def 1white 2black
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
        }

        private void RoundCornerTick(object? sender, object e)
        {
            if(AWAScale.ScaleX != 1.0)
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            else
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
        }

        private void AWALaunchingStoryBoard_Completed(object sender, object e)
        {
            if(AWAScale.ScaleX == 1.0)
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
            if (AWFrame.Opacity == 0 && AppWindowMain_Target >= 0)
            {
                AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
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
        double FarPoint = 0.2;
        private void GstBut_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (true)
            {
                AppRect_AnState = 9;
                AWGScale.CenterX = ActualWidth / 2;
                AWGScale.CenterY = ActualHeight / 2;

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

            AWGScale.ScaleX = AWGScale.ScaleY = 1;
            AWGTransform.X = 0;
            AWGTransform.Y = 0;

            double ty = ActualHeight * FarPoint - ActualHeight * 0.5;
            if (AppRect_Target >= 0)
            {
               // ty = -(ActualHeight - ActualHeight * AppWindowGrid0BackScaleSplineY.Value) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[AppRect_Target]).ActualOffset.Y - AppWindowGrid0BackOfYSpline.Value * AppWindowGrid0BackScaleSplineX.Value;
            }
            //ty = -(ActualHeight - ActualHeight * AppWindowGrid0BackScaleSplineY.Value) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[AppRect_Target]).ActualOffset.Y - AppWindowGrid0BackOfYSpline.Value * AppWindowGrid0BackScaleSplineX.Value;

            
            //Trace.WriteLine((e.Velocities.Linear.Y, AppRect_Target, AppRect_AnState));
            if ((e.Velocities.Linear.Y <= -0.1) && true)
            {
                SetSwipeBarColor(0);
                //GstBut.Visibility = Visibility.Collapsed;
                MouseX = e.Position.X;
                MouseY = e.Position.Y;
                try
                {
                    double dH = ((ActualHeight + (MouseY - MouseDownY) + e.Velocities.Linear.Y * (Application.Current as App).FlyFar - ActualHeight * FarPoint) * (ActualHeight / (ActualHeight - ActualHeight * FarPoint)));
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
                    //Trace.WriteLine((e.Velocities.Linear.Y,dH, mH));
                    /*AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = AppRectGrid3Scale.ScaleX * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH);
                    if (AppRectGrid3ScaleSplineX.Value < 0.01 && AppRect_Target != -1)
                    {
                        AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                    }
                    if (ty <= -ActualHeight * 1 / 6 && AppRectGrid3ScaleSplineX.Value < 0.2)
                    {
                        //AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.2;
                    }*/
                }
                catch
                {
                    /*AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = AppRectGrid3Scale.ScaleX * 1;
                    if (AppRectGrid3ScaleSplineX.Value < 0.01)
                    {
                        AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                    }*/
                }
                //Trace.WriteLine(AppRectGrid3ScaleSplineX.Value);
                //AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value =1;
                /*if (AppRectGrid3ScaleSplineX.Value < 0.01)
                {
                    AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                }*/


                //Trace.WriteLine(AppRectGrid3ScaleSplineX.Value);
                //AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);

                if (AppRect_Target == -1)
                {
                    //AppRectGrid2TransformSplineY.Value = 0;// 0 - ActualHeight * (0.5 - FarPoint) + 0*ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);
                }
            
                StartWindowAnimation(0, AppWindowMain_Target);
                StartBackgroundAnimation(0);
                
               /* AppWindowGrid0Back.Stop();
                AppRectGrid1Back.Stop();

                AppWindowGrid0BackScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppWindowGrid0BackScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppWindowGrid0BackHeightSpline.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppRectGrid2TransformSplineY.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.12 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.12 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));

                if (AppRectGrid1BackTransformSplineY.Value >= ActualHeight * 1 / 6)
                {
                    AppRectGrid2TransformSplineY.Value += 0;
                    BackEase1.Amplitude = 0.13;
                    BackEase2.Amplitude = 0.13;
                    PowerEase3.Power = 4.0;
                    PowerEase4.Power = 4.0;
                    PowerEase5.Power = 4.0;
                }
                else if (AppRectGrid1BackTransformSplineY.Value <= -ActualHeight * 1 / 6 && AppRect_Target != -1)
                {/*
                    AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.12 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                    AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.12 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                  */  //AppRectGrid2Transform2SplineX.Value = AppRectGrid2TransformSplineX.Value * 1.0;// - ActualHeight * (0.5 - FarPoint * 1.0) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint * 1.0);

                    //AppRectGrid2Transform2SplineY.Value = AppRectGrid2TransformSplineY.Value;// 0 + (FarPoint - 0.5) * (0) * ActualHeight; 
                   /* BackEase1.Amplitude = 0.13;
                    BackEase2.Amplitude = 0.2;
                    PowerEase3.Power = 4.0;
                    PowerEase4.Power = 4.0;
                    PowerEase5.Power = 4.0;
                }
                else
                {
                    BackEase1.Amplitude = 0.1;
                    BackEase2.Amplitude = 0.13;
                    PowerEase3.Power = 4.0;
                    PowerEase4.Power = 4.0;
                    PowerEase5.Power = 5.0;
                }
                if (AppWindowGrid0BackScaleSplineX.Value > 0.2)
                {
                    //AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = (AppRectGrid3Scale.ScaleX * 0 + AppRectGrid3ScaleSplineY.Value * 1);
                    //AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);

                    AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.07 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                    AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.07 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                    BackEase1.Amplitude = 0.1;
                    BackEase2.Amplitude = 0.1;
                    PowerEase3.Power = 3.6;
                    PowerEase4.Power = 3.6;
                    PowerEase5.Power = 3.6;
                }
                /*
                if (AppRectGrid1BackTransformSplineY.Value <= -ActualHeight * 1 / 6 && AppRect_Target != -1 && false)
                {
                    AppRectGrid2StoryBoard2.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2Transform2SplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2Transform2SplineY.Value;

                    /*
                    AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint) - 0.5 * ActualHeight * e.Velocities.Linear.Y * -0.2;
                    AppRectGrid2StoryBoard.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2TransformSplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2TransformSplineY.Value;
                }
                else
                {
                    AppRectGrid2StoryBoard.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2TransformSplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2TransformSplineY.Value;
                }


                AppRectGrid2StoryBoard.Begin();
                AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2TransformSplineX.Value;
                AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2TransformSplineY.Value;

                AppRectGrid3StoryBoardC1.Begin();
                AppWindowGrid0Back.Begin();
                AppRectGrid1BackCurveBounce.Begin();*/
            }
            else if (e.Velocities.Linear.Y >= -2 || true)
            {/*
                frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1;*/
                GstBut.Visibility = Visibility.Visible;
                /*AppRect_AnState = 10;
                AppRect_AnState = 10;
                AppRectGrid3StoryBoardFill.Begin();
                AppRectGrid2Fill.Begin();*/
            }
        }


        private void GstBut_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {/*
            RoundCornerPointerStBo.Stop();
            RoundCornerTimer.Stop();*//*
            if (AppRectGrid2StoryBoard.GetCurrentState() == ClockState.Active && AppRect_AnState != 8)
            {
                AppRectGrid3StoryBoardC1.Stop();
                AppRectGrid2StoryBoard.Stop();
            }
            if (AppRectGrid3StoryBoardFill.GetCurrentState() == ClockState.Active && AppRect_AnState != 8)
            {
                AppRectGrid3StoryBoardFill.Stop();
                AppRectGrid2Fill.Stop();
                AppRectGrid2Fill.Stop();
            }*/
            GstBut.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.TranslateX | Windows.UI.Xaml.Input.ManipulationModes.TranslateY;

            if (true)
            {
                AppRect_AnState = 9;
                MouseDownX = e.Position.X;
                MouseDownY = e.Position.Y;
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
                /*
                double x = AppRectGrid1Transform.X, y = AppRectGrid1Transform.Y, sx = AppWindowGrid0Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY;
                AppRectGrid1StoryBoard.Stop();
                AppWindowGrid0StoryBoard.Stop();
                AppRectGrid1Transform.X = x; AppRectGrid1Transform.Y = y; AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx; AppWindowGrid0.Height = h;
                */

            }
        }
























        //

        private void AppWindowGrid0Scale_Completed(object sender, object e)
        {/*
            RoundCornerTimer.Stop();
            AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            if ((AppRect_AnState == 10 || AppRect_AnState == 1))
            {
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
                AWBackgIcon.Opacity = 0;
                AWFrontIcon.Opacity = 0;
            }*/
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public int AppRect_AnState = 0, AppRect_Target = -2; //0无 1开 2关 10全屏 9手势

        /*
        public void StartRectAnimation(int isOnLaunching = 0, int target = -2)
        {
            if (isOnLaunching == 1)
            {

                GstBut.Visibility = Visibility.Visible;
                AppWindowGrid0.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppWindowGrid0.Opacity = 1;

                if (target != -2 && target != -1)
                {

                    iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                    iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                    double x = AppRectGrid1Transform.X, x2 = AppRectGrid2Transform.X, y = AppRectGrid1Transform.Y, y2 = AppRectGrid2Transform.Y, sx = AppWindowGrid0Scale.ScaleX, s2 = AppRectGrid3Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY, oy = ApFrTranslate.Y;

                    AppRectGrid1StoryBoard.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    AppWindowGrid0Back.Stop();
                    RoundCornerTimer.Stop();

                    if (AppRect_Target != -2 && AppRect_Target != -1)
                        (DesktopGrid.Children[AppRect_Target] as Grid).Opacity = 1;



                    if (target != AppRect_Target)
                    {
                        iconFgTop.Opacity = 1; iconBgTop.Opacity = 1;
                        AppWindowGrid0.Height = (DesktopGrid.Children[target] as Grid).ActualHeight / (DesktopGrid.Children[target] as Grid).ActualWidth * AppWindowGrid0.Width;
                        AppWindowGrid0Scale.CenterX = ActualWidth * 0.5;
                        AppWindowGrid0Scale.CenterY = ActualHeight * 0.5;

                        AppRectGrid2TransformSplineX.Value = AppRectGrid2TransformSplineY.Value = 0;
                        AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;

                        AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = (DesktopGrid.Children[target] as Grid).ActualWidth / ActualWidth;
                        AppRectGrid1Transform.X = -(ActualWidth - ActualWidth * AppWindowGrid0Scale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[target]).ActualOffset.X - 0;
                        ApFrTranslate.Y = 0;
                        if (AppWindowGrid0Scale.ScaleX > 0.2 || true)
                        {
                            ApFrTranslate.Y = 0.5 * (ActualHeight - AppWindowGrid0.Height);
                        }
                        AppRectGrid1Transform.Y = -(ActualHeight - ActualHeight * AppWindowGrid0Scale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[target]).ActualOffset.Y - 0 - ApFrTranslate.Y * AppWindowGrid0Scale.ScaleX;
                        RoundCornerPointerAnimation.From = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[target] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[target] as Grid).ActualWidth / ActualWidth);
                       


                        if ((AppRectGrid1Back.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard2.GetCurrentState() == ClockState.Active) && target == AppRect_Target)
                        {
                            AppRectGrid1Transform.X = x + x2; AppRectGrid1Transform.Y = y + y2 - oy * sx * s2;
                            AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx * s2;
                            AppRectGrid1BackCurveBounce.Stop();
                            AppWindowGrid0Back.Stop();
                            AppRectGrid3StoryBoardC1.Stop();
                            AppRectGrid2StoryBoard.Stop();
                            AppRectGrid2StoryBoard2.Stop();
                            AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;
                            AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;
                            ApFrTranslate.Y = oy;
                        }
                        else if ((AppRectGrid1Back.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard2.GetCurrentState() == ClockState.Active) && target != AppRect_Target)
                        {
                            AppRectGrid1BackCurveBounce.Stop();
                            AppRectGrid1Back.Stop();
                            AppWindowGrid0Back.Stop();
                            AppRectGrid3StoryBoardC1.Stop();
                            AppRectGrid2StoryBoard.Stop();
                            AppRectGrid2StoryBoard2.Stop();
                            AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;
                            AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;
                            ApFrTranslate.Y = 0;
                            if (AppWindowGrid0Scale.ScaleX > 0.2 || true)
                            {
                                ApFrTranslate.Y = 0.5 * (ActualHeight - AppWindowGrid0.Height);
                            }
                        }
                        AppFrame.Opacity = 0;
                    }
                    else
                    {
                        AppRectGrid1Transform.X = x + x2; AppRectGrid1Transform.Y = y + y2; AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx; AppWindowGrid0.Height = h;
                        AppRectGrid2Transform.X = 0; AppRectGrid2Transform.Y = 0;
                        AppWindowGrid0Scale.CenterY = cy;
                        RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;

                        if ((AppRectGrid1Back.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard.GetCurrentState() == ClockState.Active || AppRectGrid2StoryBoard2.GetCurrentState() == ClockState.Active))
                        {
                            AppRectGrid1Transform.X = AppRectGrid1Transform.X + AppRectGrid2Transform.X; AppRectGrid1Transform.Y = AppRectGrid1Transform.Y + AppRectGrid2Transform.Y;
                            AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = AppWindowGrid0Scale.ScaleX * AppRectGrid3Scale.ScaleX;
                            AppRectGrid1BackCurveBounce.Stop();
                            AppRectGrid1Back.Stop();
                            AppWindowGrid0Back.Stop();
                            AppRectGrid3StoryBoardC1.Stop();
                            AppRectGrid2StoryBoard.Stop();
                            AppRectGrid2StoryBoard2.Stop();
                            AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;
                            AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;
                            ApFrTranslate.Y = oy;
                        }
                    }
                    frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1.2 / AppWindowGrid0Scale.ScaleX;
                    SwBarScale.ScaleX = SwBarScale.ScaleY = 1.2 / AppWindowGrid0Scale.ScaleX;
                    SwipeBar.Opacity = 0;

                    if (AppRect_AnState == 9)
                    {
                        RoundCornerPointerTransform.X = PageOutline.ActualHeight * 100 / 1920 * 1 * 0.16 / ((DesktopGrid.Children[target] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    }
                    AppRect_AnState = 1;
                    AppRect_Target = target;

                    AppWindowGrid0ScaleSplineX.Value = AppWindowGrid0ScaleSplineY.Value = 1.0;
                    AppWindowGrid0HeightSpline.Value = ActualHeight;
                    AppRectGrid1TransformSplineX.Value = AppRectGrid1TransformSplineY.Value = 0;
                    AppWindowGrid0OfYSpline.Value = 0;
                    RoundCornerPointerAnimation.To = PageOutline.ActualHeight * (Application.Current as App).ScreenCornerRadius;
                    AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if ((DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                        iconBgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconFgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else if ((DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.CenterY = iconBgTop.ActualHeight * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = 1.1;
                        iconFgTop.Source = null;//((DesktopGrid.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconBgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else
                    {
                        iconFgTop.Source = null;
                        iconBgTop.Source = null;
                    }



                    if (ActualWidth >= ActualHeight)
                    {
                        iconFgTop.HorizontalAlignment = HorizontalAlignment.Left;
                        iconFgTop.VerticalAlignment = VerticalAlignment.Stretch;
                        AppFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                        AppFrame.VerticalAlignment = VerticalAlignment.Stretch;
                    }
                    else
                    {
                        iconFgTop.HorizontalAlignment = HorizontalAlignment.Stretch;
                        iconFgTop.VerticalAlignment = VerticalAlignment.Top;
                        AppFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                        AppFrame.VerticalAlignment = VerticalAlignment.Stretch;
                    }

                    TopLayerOpacityDA1.From = AppFrame.Opacity;
                    TopLayerOpacityDA1.To = 1;
                    TopLayerOpacityDA1.Duration = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
                    TopLayerOpacityDA1.BeginTime = TimeSpan.FromSeconds(0.0);

                    AppRectGrid1Back.Stop();
                    AppWindowGrid0Back.Stop();

                    AppRectGrid1StoryBoard.Begin();
                    AppWindowGrid0StoryBoard.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();
                    TopLayerOpacityStBo.Begin();

                    (DesktopGrid.Children[target] as Grid).Opacity = 0.01.01;
                }
                else if (target == -1)
                {

                    if(AppRect_Target >= 0)
                        (DesktopGrid.Children[AppRect_Target] as Grid).Opacity = 1;



                    iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                    iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                    double x = AppRectGrid1Transform.X, x2 = AppRectGrid2Transform.X, y = AppRectGrid1Transform.Y, y2 = AppRectGrid2Transform.Y, sx = AppWindowGrid0Scale.ScaleX, s2 = AppRectGrid3Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY, oy = ApFrTranslate.Y;

                    AppRectGrid1StoryBoard.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    AppWindowGrid0Back.Stop();
                    RoundCornerTimer.Stop();




                    
                        iconFgTop.Opacity = 1; iconBgTop.Opacity = 1;
                        AppWindowGrid0.Height = ActualHeight;
                        AppWindowGrid0Scale.CenterX = ActualWidth * 0.5;
                        AppWindowGrid0Scale.CenterY = ActualHeight * 0.5;

                        AppRectGrid2TransformSplineX.Value = AppRectGrid2TransformSplineY.Value = 0;
                        AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;

                        AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = 0.01;
                        AppRectGrid1Transform.X = 0;
                        ApFrTranslate.Y = 0;
                        AppRectGrid1Transform.Y = ActualHeight * FarPoint - ActualHeight * 0.4;
                    //Trace.WriteLine(AppRectGrid1Transform.Y);
                        RoundCornerPointerAnimation.From = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16;
                        RoundCornerPointerTransform.X = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16;
                    AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;



                        AppRectGrid2Transform.X = 0; AppRectGrid2Transform.Y = 0;
                        AppWindowGrid0Scale.CenterY = cy;
                        RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;


                        AppRectGrid1BackCurveBounce.Stop();
                        AppRectGrid1Back.Stop();
                        AppWindowGrid0Back.Stop();
                        AppRectGrid3StoryBoardC1.Stop();
                        AppRectGrid2StoryBoard.Stop();
                        AppRectGrid2StoryBoard2.Stop();

                        frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1.2 / AppWindowGrid0Scale.ScaleX;

                        if (AppRect_AnState == 9)
                        {
                            RoundCornerPointerTransform.X = PageOutline.ActualHeight * 100 / 1920 * 1 * 0.16;
                            RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                        }
                        AppRect_AnState = 1;
                        AppRect_Target = target;

                        AppWindowGrid0ScaleSplineX.Value = AppWindowGrid0ScaleSplineY.Value = 1.0;
                        AppWindowGrid0HeightSpline.Value = ActualHeight;
                        AppRectGrid1TransformSplineX.Value = AppRectGrid1TransformSplineY.Value = 0;
                        AppWindowGrid0OfYSpline.Value = 0;
                        RoundCornerPointerAnimation.To = PageOutline.ActualHeight * (Application.Current as App).ScreenCornerRadius;
                        AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);


                        iconFgTop.Source = null;
                        iconBgTop.Source = null;

                        TopLayerOpacityDA1.From = AppFrame.Opacity;
                        TopLayerOpacityDA1.To = 1;
                        TopLayerOpacityDA1.Duration = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
                        TopLayerOpacityDA1.BeginTime = TimeSpan.FromSeconds(0.0);

                        AppRectGrid1Back.Stop();
                        AppWindowGrid0Back.Stop();
                    SwipeBar.Opacity = 1;

                    AppRectGrid1StoryBoard.Begin();
                        AppWindowGrid0StoryBoard.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();
                        TopLayerOpacityStBo.Begin();



                }
            }
            else if (isOnLaunching == 0)
            {
                if (target != -2 && target != -1)
                {

                    frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1;
                    SwBarScale.ScaleX = SwBarScale.ScaleY = 1;

                    iconFgTop.Opacity = 1;
                    iconBgTop.Opacity = 1;
                    GstBut.Visibility = Visibility.Collapsed;
                    AppRect_AnState = 2;
                    /*
                    AppRectGrid1StoryBoard.Stop();
                    AppRectGrid1Back.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    AppWindowGrid0Back.Stop();
                    double x = AppRectGrid1Transform.X, y = AppRectGrid1Transform.Y, sx = AppWindowGrid0Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY, oy = ApFrTranslate.Y;

                    AppRectGrid1StoryBoard.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    RoundCornerTimer.Stop();
                    SwipeBar.Opacity = 1;

                    AppRect_Target = target;
                    AppRect_AnState = 2;

                    AppWindowGrid0.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    AppWindowGrid0.Opacity = 1;
                    AppWindowGrid0Scale.CenterX = ActualWidth * 0.5;
                    AppWindowGrid0Scale.CenterY = ActualHeight * 0.5;


                    AppRectGrid1Transform.X = x; AppRectGrid1Transform.Y = y; AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx; AppWindowGrid0.Height = h;

                    AppWindowGrid0BackScaleSplineX.Value = AppWindowGrid0BackScaleSplineY.Value = (DesktopGrid.Children[target] as Grid).ActualWidth / AppWindowGrid0.Width;
                    AppWindowGrid0BackHeightSpline.Value = (DesktopGrid.Children[target] as Grid).ActualHeight / (DesktopGrid.Children[target] as Grid).ActualWidth * AppWindowGrid0.Width;
                    AppWindowGrid0BackOfYSpline.Value = 0;
                    if (AppWindowGrid0BackScaleSplineX.Value > 0.2 || true)
                    {
                        AppWindowGrid0BackOfYSpline.Value = 0.5 * (ActualHeight - AppWindowGrid0BackHeightSpline.Value);
                    }
                    AppRectGrid1BackTransformSplineX.Value = -(ActualWidth - ActualWidth * AppWindowGrid0BackScaleSplineX.Value) * 0.5 + DesktopGrid.ActualOffset.X + (DesktopGrid.Children[target]).ActualOffset.X;
                    AppRectGrid1BackTransformSplineY.Value = -(ActualHeight - ActualHeight * AppWindowGrid0BackScaleSplineY.Value) * 0.5 + DesktopGrid.ActualOffset.Y + (DesktopGrid.Children[target]).ActualOffset.Y - AppWindowGrid0BackOfYSpline.Value * AppWindowGrid0BackScaleSplineX.Value;
                    ApFrTranslate.Y = oy;

                    AppWindowGrid0BackScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackHeightSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackOfYSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    PowerEase3.Power = 5.5;
                    PowerEase4.Power = 5.5;
                    PowerEase5.Power = 5.5;
                    PowerEase6.Power = 5.5;

                    RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerAnimation.To = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16 / AppWindowGrid0BackScaleSplineX.Value;
                    AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if ((DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        iconBgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconFgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                        iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                    }
                    else if ((DesktopGrid.Children[AppRect_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.CenterY = iconBgTop.ActualHeight * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = 1.1;
                        iconFgTop.Source = null;//((DesktopGrid.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconBgTop.Source = ((DesktopGrid.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else
                    {
                        iconFgTop.Source = null;
                        iconBgTop.Source = null;
                    }



                    if (ActualWidth >= ActualHeight)
                    {
                        iconFgTop.HorizontalAlignment = HorizontalAlignment.Left;
                        iconFgTop.VerticalAlignment = VerticalAlignment.Stretch;
                        AppFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                        AppFrame.VerticalAlignment = VerticalAlignment.Stretch;
                    }
                    else
                    {
                        iconFgTop.HorizontalAlignment = HorizontalAlignment.Stretch;
                        iconFgTop.VerticalAlignment = VerticalAlignment.Top;
                        AppFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                        AppFrame.VerticalAlignment = VerticalAlignment.Stretch;
                    }

                    TopLayerOpacityDA1.From = AppFrame.Opacity;
                    TopLayerOpacityDA1.To = 0;
                    TopLayerOpacityDA1.Duration = TimeSpan.FromSeconds(0.2 * (Application.Current as App).TransitionDurationTime);
                    TopLayerOpacityDA1.BeginTime = TimeSpan.FromSeconds(0.1 * (Application.Current as App).TransitionDurationTime);


                    AppRectGrid1Back.Begin();
                    AppWindowGrid0Back.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();
                    TopLayerOpacityStBo.Begin();

                    RetToApp.Visibility = Visibility.Visible;
                    (DesktopGrid.Children[target] as Grid).Opacity = 0.01.01;
                }
                else if(target == -1)
                {
                    frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1;
                    SwipeBar.Opacity = 1;

                    iconFgTop.Opacity = 1;
                    iconBgTop.Opacity = 1;
                    GstBut.Visibility = Visibility.Collapsed;
                    AppRect_AnState = 2;
                    /*
                    AppRectGrid1StoryBoard.Stop();
                    AppRectGrid1Back.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    AppWindowGrid0Back.Stop();
                    double x = AppRectGrid1Transform.X, y = AppRectGrid1Transform.Y, sx = AppWindowGrid0Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY, oy = ApFrTranslate.Y;

                    AppRectGrid1StoryBoard.Stop();
                    AppWindowGrid0StoryBoard.Stop();
                    RoundCornerTimer.Stop();

                    AppRect_Target = target;
                    AppRect_AnState = 2;

                    AppWindowGrid0.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    AppWindowGrid0.Opacity = 1;
                    AppWindowGrid0Scale.CenterX = ActualWidth * 0.5;
                    AppWindowGrid0Scale.CenterY = ActualHeight * 0.5;


                    AppRectGrid1Transform.X = x; AppRectGrid1Transform.Y = y; AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx; AppWindowGrid0.Height = h;

                    AppWindowGrid0BackScaleSplineX.Value = AppWindowGrid0BackScaleSplineY.Value = 0.01;
                    AppWindowGrid0BackHeightSpline.Value = ActualHeight;
                    AppWindowGrid0BackOfYSpline.Value = 0;
                    if (AppWindowGrid0BackScaleSplineX.Value > 0.2)
                    {
                        AppWindowGrid0BackOfYSpline.Value = 0.5 * (ActualHeight - AppWindowGrid0BackHeightSpline.Value);
                    }
                    AppRectGrid1BackTransformSplineX.Value = 0;
                    AppRectGrid1BackTransformSplineY.Value = ActualHeight * FarPoint - ActualHeight * 0.4;
                    ApFrTranslate.Y = oy;

                    AppWindowGrid0BackScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackHeightSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackOfYSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    PowerEase3.Power = 5.5;
                    PowerEase4.Power = 5.5;
                    PowerEase5.Power = 5.5;
                    PowerEase6.Power = 5.5;

                    RoundCornerPointerAnimation.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerAnimation.To = PageOutline.ActualHeight * 100 / 1920 * 3 * 0.16;
                    AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);


                    iconFgTop.Source = null;
                    iconBgTop.Source = null;

                    TopLayerOpacityDA1.From = AppFrame.Opacity;
                    TopLayerOpacityDA1.To = 0;
                    TopLayerOpacityDA1.Duration = TimeSpan.FromSeconds(0.4 * (Application.Current as App).TransitionDurationTime);
                    TopLayerOpacityDA1.BeginTime = TimeSpan.FromSeconds(0.1 * (Application.Current as App).TransitionDurationTime);


                    AppRectGrid1Back.Begin();
                    AppWindowGrid0Back.Begin();
                    RoundCornerPointerStBo.Begin();
                    RoundCornerTimer.Start();
                    TopLayerOpacityStBo.Begin();

                    RetToApp.Visibility = Visibility.Visible;
                }

            }

        }*/


    }
}
