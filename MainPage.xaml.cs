using CommunityToolkit.WinUI.Animations;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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

        public System.TimeSpan TrDur05 { get { return System.TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime); } }
        public System.TimeSpan TrDur07 { get { return System.TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime); } }
        public System.TimeSpan TrDur075 { get { return System.TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime); } }
        public Frame GetAppFrame { get { return AppFrame; } }


        public MainPage()
        {
            /*
            TrDur05 = TimeSpan.FromSeconds(0.5 * (Application.Current as App).TransitionDurationTime);
            TrDur07 = TimeSpan.FromSeconds(0.7 * (Application.Current as App).TransitionDurationTime);
            TrDur075 = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);*/

            InitializeComponent();
            AppWindowGrid0.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

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

            if(BlurAnSet.To >= 20)
            {
                GstBut.Visibility = Visibility.Visible;
                AppWindowGrid0.Visibility = Visibility.Visible;
            }
            else
            {
            }
        }

        private void Sc_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(AppRect_AnState == 0 || AppRect_AnState == 2)
            {
                AppRect_AnState = 0;
                int target = vDesktop.Children.IndexOf(((sender as Button).Parent as Grid));

                if (((vDesktop.Children[target] as Grid)).Tag != null && ((vDesktop.Children[target] as Grid)).Tag.ToString().Contains("com.android.settings"))
                {
                    AppFrame.Navigate(typeof(SettingsApp), null, new SuppressNavigationTransitionInfo());
                }
                else if (((vDesktop.Children[target] as Grid)).Tag != null && ((vDesktop.Children[target] as Grid)).Tag.ToString().Contains("com.android.camera"))
                {
                    AppFrame.Navigate(typeof(CameraApp), null, new SuppressNavigationTransitionInfo());
                }
                else
                {
                    AppFrame.Navigate(typeof(BlankPage), null, new SuppressNavigationTransitionInfo());
                }
                if (target == 9)
                    target = -1;

                //(Application.Current as App).EnableBgBlur = 1;
                //(Application.Current as App).ScreenCornerRadius = 100.0 / 1920;// 0.001;

                StartRectAnimation(1, target);
                StartBgAnimation(1);
            }
        }

        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            PageOutline.CornerRadius = new Windows.UI.Xaml.CornerRadius(0.1);
            if (ActualWidth >= ActualHeight * 9/16)
            {
                PageOutline.Width = PageOutline.MaxWidth = ActualHeight * 9 / 16;
                PageOutline.Height = PageOutline.MaxHeight = ActualHeight;
            }
            else
            {
                PageOutline.Width = PageOutline.MaxWidth = ActualWidth;
                PageOutline.Height = PageOutline.MaxHeight = ActualWidth * 16 / 9;
            }

            //iconSc1.CornerRadius = new Windows.UI.Xaml.CornerRadius(PageOutline.Height * 100 / 1920 * 0.4);
            vDesktop.ItemHeight = 0.16 * 1.4 * PageOutline.ActualWidth;
            vDesktop.ItemWidth = 0.16 * 1.4 * PageOutline.ActualWidth;
            vDesktop.Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.2 * PageOutline.ActualWidth, 0.16 * 0.8 * PageOutline.ActualWidth, 0.16 * 0.2 * PageOutline.ActualWidth, 0.16 * 0.2 * PageOutline.ActualWidth);
            //AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(PageOutline.Height * 100 / 1920);
            //RoundCornerPointerTransform.X = PageOutline.Height * 100 / 1920 / 0.16;
            WpScaleT.CenterX = Wallpaper.ActualWidth * 0.5;
            WpScaleT.CenterY = Wallpaper.ActualHeight * 0.5;
            dskIconScaleT.CenterX = PageOutline.ActualWidth * 0.5;
            dskIconScaleT.CenterY = PageOutline.ActualHeight * 0.5;
            BlurBorder.Margin = new Windows.UI.Xaml.Thickness(0);// = new Windows.UI.Xaml.Thickness(-ActualWidth * 0.5, -ActualHeight * 0.5, -ActualWidth * 0.5, -ActualHeight * 0.5);

            iconFgTop.Height = AppWindowGrid0.Width;
            if (AppRect_Target >= 0 && (vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
            {
                iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
            }
            else if (AppRect_Target >= 0 && (vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
            {
                iconBgTopScaleT.CenterX = iconBgTop.ActualWidth * 0.5;
                iconBgTopScaleT.CenterY = iconBgTop.ActualHeight * 0.5;
                iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = 1.1;
            }

            foreach (var aIgrid in vDesktop.Children)
            {
                //图标
                if(aIgrid.GetType() == typeof(Grid) && (aIgrid as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5))
                {
                    (aIgrid as Grid).CornerRadius = new Windows.UI.Xaml.CornerRadius(PageOutline.Height * 100 / 1920 * 0.5);
                    (aIgrid as Grid).Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.12 * PageOutline.ActualWidth);
                    (((aIgrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterX = (((aIgrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).CenterY = (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterX = (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterY = ((aIgrid as Grid).Children[0] as Image).ActualWidth * 0.5;
                    (((aIgrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleX = (((aIgrid as Grid).Children[0] as Image).RenderTransform as ScaleTransform).ScaleY = (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.55;
                    ((aIgrid as Grid).Children[2] as Button).Opacity = 0.5;
                }
                //卡片
                if (aIgrid.GetType() == typeof(Grid) && (aIgrid as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1.0))
                {
                    (aIgrid as Grid).CornerRadius = new Windows.UI.Xaml.CornerRadius(PageOutline.Height * 100 / 1920 * 0.5);
                    (aIgrid as Grid).Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.12 * PageOutline.ActualWidth);
                    (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterX = ((aIgrid as Grid).Children[1] as Image).ActualWidth * 0.5;
                    (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).CenterY = ((aIgrid as Grid).Children[1] as Image).ActualHeight * 0.5;
                    (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleX = (((aIgrid as Grid).Children[1] as Image).RenderTransform as ScaleTransform).ScaleY = 1.1;
                    ((aIgrid as Grid).Children[2] as Button).Opacity = 0.5;
                }
            }

            GstGrid.Height = 0.05 * ActualHeight;
            SwipeBar.Height = ActualHeight * 0.006;
            SwipeBar.Width = ActualHeight * 0.006 * 18;
            SwipeBar.Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, ActualHeight * 0.01);
            SwipeBar.CornerRadius = new CornerRadius(SwipeBar.Height / 2);
            //SwipeBar.Opacity = 1;

            if (sender != null)
            {
                AppWindowGrid0.Width = AppRectGrid1.Width = AppRectGrid2.Width = AppRectGrid3.Width = ActualWidth;
                AppWindowGrid0.Height = AppRectGrid1.Height = AppRectGrid2.Height = AppRectGrid3.Height = ActualHeight;
                AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;
                AppFrame.Height = ActualHeight;
            }
        }

        public void StartBgAnimation(int isOnLaunching = 0)
        {


            if (isOnLaunching == 1)
            {
                WpKeyFrameX.Value = 1.1;
                WpKeyFrameY.Value = 1.1;
                dskIconKeyFrameX.Value = 0.9;
                dskIconKeyFrameY.Value = 0.9;
                BlurAnSet.From = BlurPointerTransform.X;
                if((Application.Current as App).EnableBgBlur == 1)
                {
                    BlurAnSet.To = 20;
                    BlurPointerDA.To = 20;
                }
                else if ((Application.Current as App).EnableBgBlur == 0)
                {
                    BlurAnSet.To = 0;
                    BlurPointerDA.To = 0;
                }
                BlurAnSet.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                BlurPointerDA.From = BlurPointerTransform.X;
                BlurPointerDA.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                GstBut.Visibility = Visibility.Visible;
            }
            else if(isOnLaunching == 0)
            {
                WpKeyFrameX.Value = 1.0;
                WpKeyFrameY.Value = 1.0;
                dskIconKeyFrameX.Value = 1.0;
                dskIconKeyFrameY.Value = 1.0;
                BlurAnSet.From = BlurPointerTransform.X;
                BlurAnSet.To = 0;
                BlurAnSet.Duration = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                BlurPointerDA.From = BlurPointerTransform.X;
                BlurPointerDA.To = 0;
                BlurPointerDA.Duration = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
            }
            
            BlurPointerStBo.Begin();
            BlurAnimation.Start();
            dskIconGridStoryBoard.Begin();
            WpStoryBoard.Begin();
            
        }

        public int AppRect_AnState = 0, AppRect_Target = -2; //0无 1开 2关 10全屏 9手势


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
                        (vDesktop.Children[AppRect_Target] as Grid).Opacity = 1;



                    if (target != AppRect_Target)
                    {
                        iconFgTop.Opacity = 1; iconBgTop.Opacity = 1;
                        AppWindowGrid0.Height = (vDesktop.Children[target] as Grid).ActualHeight / (vDesktop.Children[target] as Grid).ActualWidth * AppWindowGrid0.Width;
                        AppWindowGrid0Scale.CenterX = ActualWidth * 0.5;
                        AppWindowGrid0Scale.CenterY = ActualHeight * 0.5;

                        AppRectGrid2TransformSplineX.Value = AppRectGrid2TransformSplineY.Value = 0;
                        AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;

                        AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = (vDesktop.Children[target] as Grid).ActualWidth / ActualWidth;
                        AppRectGrid1Transform.X = -(ActualWidth - ActualWidth * AppWindowGrid0Scale.ScaleX) * 0.5 + vDesktop.ActualOffset.X + (vDesktop.Children[target]).ActualOffset.X - 0;
                        ApFrTranslate.Y = 0;
                        if (AppWindowGrid0Scale.ScaleX > 0.2 || true)
                        {
                            ApFrTranslate.Y = 0.5 * (ActualHeight - AppWindowGrid0.Height);
                        }
                        AppRectGrid1Transform.Y = -(ActualHeight - ActualHeight * AppWindowGrid0Scale.ScaleX) * 0.5 + vDesktop.ActualOffset.Y + (vDesktop.Children[target]).ActualOffset.Y - 0 - ApFrTranslate.Y * AppWindowGrid0Scale.ScaleX;
                        RoundCornerPointerDA.From = PageOutline.Height * 100 / 1920 * 3 * 0.16 / ((vDesktop.Children[target] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = PageOutline.Height * 100 / 1920 * 3 * 0.16 / ((vDesktop.Children[target] as Grid).ActualWidth / ActualWidth);
                       


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
                        RoundCornerPointerDA.From = RoundCornerPointerTransform.X;

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
                        RoundCornerPointerTransform.X = PageOutline.Height * 100 / 1920 * 1 * 0.16 / ((vDesktop.Children[target] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerDA.From = RoundCornerPointerTransform.X;
                    }
                    AppRect_AnState = 1;
                    AppRect_Target = target;

                    AppWindowGrid0ScaleSplineX.Value = AppWindowGrid0ScaleSplineY.Value = 1.0;
                    AppWindowGrid0HeightSpline.Value = ActualHeight;
                    AppRectGrid1TransformSplineX.Value = AppRectGrid1TransformSplineY.Value = 0;
                    AppWindowGrid0OfYSpline.Value = 0;
                    RoundCornerPointerDA.To = PageOutline.Height * (Application.Current as App).ScreenCornerRadius;
                    AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if ((vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                        iconBgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconFgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else if ((vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.CenterY = iconBgTop.ActualHeight * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = 1.1;
                        iconFgTop.Source = null;//((vDesktop.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconBgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else
                    {
                        iconFgTop.Source = null;
                        iconBgTop.Source = null;
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

                    (vDesktop.Children[target] as Grid).Opacity = 0.01;
                }
                else if (target == -1)
                {

                    if(AppRect_Target >= 0)
                        (vDesktop.Children[AppRect_Target] as Grid).Opacity = 1;



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
                        RoundCornerPointerDA.From = PageOutline.Height * 100 / 1920 * 3 * 0.16;
                        RoundCornerPointerTransform.X = PageOutline.Height * 100 / 1920 * 3 * 0.16;
                    AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;



                        AppRectGrid2Transform.X = 0; AppRectGrid2Transform.Y = 0;
                        AppWindowGrid0Scale.CenterY = cy;
                        RoundCornerPointerDA.From = RoundCornerPointerTransform.X;


                        AppRectGrid1BackCurveBounce.Stop();
                        AppRectGrid1Back.Stop();
                        AppWindowGrid0Back.Stop();
                        AppRectGrid3StoryBoardC1.Stop();
                        AppRectGrid2StoryBoard.Stop();
                        AppRectGrid2StoryBoard2.Stop();

                        frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1.2 / AppWindowGrid0Scale.ScaleX;

                        if (AppRect_AnState == 9)
                        {
                            RoundCornerPointerTransform.X = PageOutline.Height * 100 / 1920 * 1 * 0.16;
                            RoundCornerPointerDA.From = RoundCornerPointerTransform.X;
                        }
                        AppRect_AnState = 1;
                        AppRect_Target = target;

                        AppWindowGrid0ScaleSplineX.Value = AppWindowGrid0ScaleSplineY.Value = 1.0;
                        AppWindowGrid0HeightSpline.Value = ActualHeight;
                        AppRectGrid1TransformSplineX.Value = AppRectGrid1TransformSplineY.Value = 0;
                        AppWindowGrid0OfYSpline.Value = 0;
                        RoundCornerPointerDA.To = PageOutline.Height * (Application.Current as App).ScreenCornerRadius;
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
                    AppWindowGrid0Back.Stop();*/
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

                    AppWindowGrid0BackScaleSplineX.Value = AppWindowGrid0BackScaleSplineY.Value = (vDesktop.Children[target] as Grid).ActualWidth / AppWindowGrid0.Width;
                    AppWindowGrid0BackHeightSpline.Value = (vDesktop.Children[target] as Grid).ActualHeight / (vDesktop.Children[target] as Grid).ActualWidth * AppWindowGrid0.Width;
                    AppWindowGrid0BackOfYSpline.Value = 0;
                    if (AppWindowGrid0BackScaleSplineX.Value > 0.2 || true)
                    {
                        AppWindowGrid0BackOfYSpline.Value = 0.5 * (ActualHeight - AppWindowGrid0BackHeightSpline.Value);
                    }
                    AppRectGrid1BackTransformSplineX.Value = -(ActualWidth - ActualWidth * AppWindowGrid0BackScaleSplineX.Value) * 0.5 + vDesktop.ActualOffset.X + (vDesktop.Children[target]).ActualOffset.X;
                    AppRectGrid1BackTransformSplineY.Value = -(ActualHeight - ActualHeight * AppWindowGrid0BackScaleSplineY.Value) * 0.5 + vDesktop.ActualOffset.Y + (vDesktop.Children[target]).ActualOffset.Y - AppWindowGrid0BackOfYSpline.Value * AppWindowGrid0BackScaleSplineX.Value;
                    ApFrTranslate.Y = oy;

                    AppWindowGrid0BackScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackHeightSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AppWindowGrid0BackOfYSpline.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    PowerEase3.Power = 5.5;
                    PowerEase4.Power = 5.5;
                    PowerEase5.Power = 5.5;
                    PowerEase6.Power = 5.5;

                    RoundCornerPointerDA.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerDA.To = PageOutline.Height * 100 / 1920 * 3 * 0.16 / AppWindowGrid0BackScaleSplineX.Value;
                    AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);

                    if ((vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        iconBgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconFgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                        iconBgTopScaleT.CenterX = iconBgTopScaleT.CenterY = iconFgTopScaleT.CenterX = iconFgTopScaleT.CenterY = iconFgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = iconFgTopScaleT.ScaleX = iconFgTopScaleT.ScaleY = 1.5;
                    }
                    else if ((vDesktop.Children[AppRect_Target].GetType() == typeof(Grid) && (vDesktop.Children[AppRect_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(1)))
                    {
                        iconBgTopScaleT.CenterX = iconBgTop.ActualWidth * 0.5;
                        iconBgTopScaleT.CenterY = iconBgTop.ActualHeight * 0.5;
                        iconBgTopScaleT.ScaleX = iconBgTopScaleT.ScaleY = 1.1;
                        iconFgTop.Source = null;//((vDesktop.Children[AppRect_Target] as Grid).Children[0] as Image).Source;
                        iconBgTop.Source = ((vDesktop.Children[AppRect_Target] as Grid).Children[1] as Image).Source;
                    }
                    else
                    {
                        iconFgTop.Source = null;
                        iconBgTop.Source = null;
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
                    (vDesktop.Children[target] as Grid).Opacity = 0.01;
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
                    AppWindowGrid0Back.Stop();*/
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

                    RoundCornerPointerDA.From = RoundCornerPointerTransform.X;
                    RoundCornerPointerDA.To = PageOutline.Height * 100 / 1920 * 3 * 0.16;
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

        }
        private void RoundCornerTick(object? sender, object e)
        {
            AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
        }

        private void AppWindowGrid0Back_Completed(object sender, object e)
        {
            AppWindowGrid0.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            if (AppRect_Target != -2 && AppRect_Target != -1)
                (vDesktop.Children[AppRect_Target] as Grid).Opacity = 1;
            AppRect_Target = -2;
            AppRect_AnState = 0;


        }


        private void AppWindowGrid0StoryBoard_Completed(object sender, object e)
        {
            AppRect_AnState = 10;
        }

        private void TopLayerOpacityStBo_Completed(object sender, object e)
        {
            if (AppRect_AnState == 1)
            {
                RetToApp.Visibility = Visibility.Collapsed;
            }
        }

        private void GstBut_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
        }

        private void RetToApp_Click(object sender, RoutedEventArgs e)
        {
            if (AppRect_Target != -2 && (AppRect_AnState == 2 || AppRect_AnState == 0))
            {
                GstBut.Visibility = Visibility.Visible;
                StartRectAnimation(1, AppRect_Target);
                StartBgAnimation(1);
            }
        }

        private void AppWindowGrid0Scale_Completed(object sender, object e)
        {
            RoundCornerTimer.Stop();
            AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            if ((AppRect_AnState == 10 || AppRect_AnState == 1))
            {
                AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
                iconFgTop.Opacity = 0;
                iconBgTop.Opacity = 0;
            }
        }










        double MouseDownX = -1, MouseDownY = -1;
        double MouseX = -1, MouseY = -1;
        double mH = 0;
        double FarPoint = 0.2;
        private void GstBut_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (AppRect_Target != -2 && (AppRect_AnState == 10 || AppRect_AnState == 9 || AppRect_AnState == 1) && AppRect_AnState != 8)
            {
                AppRect_AnState = 9;
                AppRectGrid3Scale.CenterX = ActualWidth / 2;
                AppRectGrid3Scale.CenterY = ActualHeight / 2;

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
                AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = mH / ActualHeight;
                AppRectGrid2Transform.X = MouseX - (AppRectGrid3Scale.ScaleX) * (MouseDownX - 0.5 * ActualWidth) - 0.5 * ActualWidth;
                AppRectGrid2Transform.Y = -0.5 * ActualHeight + FarPoint * ActualHeight + (0.5 - FarPoint) * mH;
            }
            /*
            AppRect[ApRecTag].AppRect1_ToHeight = mH;
            AppRect[ApRecTag].AppRect1_ToWidth = AppRect[ApRecTag].AppRect1_ToHeight * ActualWidth / ActualHeight;
            AppRect[ApRecTag].AppRect1_ToY = 0.2 * ActualHeight + 0.3 * AppRect[ApRecTag].AppRect1_ToHeight;
            AppRect[ApRecTag].AppRect1_ToX = MouseX - (AppRect[ApRecTag].AppRect1_ToWidth / ActualWidth) * (MouseDownX - 0.5 * ActualWidth);
        */
            // Trace.WriteLine(MouseDownX.ToString() + " " + MouseX.ToString());
        }
        
        
        private void GstBut_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            double ty = ActualHeight * FarPoint - ActualHeight * 0.5; 
            if (AppRect_Target >= 0)
            {
                ty = -(ActualHeight - ActualHeight * AppWindowGrid0BackScaleSplineY.Value) * 0.5 + vDesktop.ActualOffset.Y + (vDesktop.Children[AppRect_Target]).ActualOffset.Y - AppWindowGrid0BackOfYSpline.Value * AppWindowGrid0BackScaleSplineX.Value;
            }

            //Trace.WriteLine((e.Velocities.Linear.Y, AppRect_Target, AppRect_AnState));
            if ((e.Velocities.Linear.Y <= -0.1) && AppRect_Target != -2 && (AppRect_AnState == 10 || AppRect_AnState == 9 || AppRect_AnState == 1))
            {
                SetSwBarBk(0);
                GstBut.Visibility = Visibility.Collapsed;
                AppRect_AnState = 8;
                AppRect_AnState = 8;
                MouseX = e.Position.X;
                MouseY = e.Position.Y;
                AppRectGrid2TransformSplineX.Value = AppRectGrid2Transform.X + e.Velocities.Linear.X * 20 * Math.Pow((Application.Current as App).FlyFar, 1 / 2);
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
                    AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = AppRectGrid3Scale.ScaleX * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH);
                    if (AppRectGrid3ScaleSplineX.Value < 0.01)
                    {
                        AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                    }
                    if (ty <= -ActualHeight * 1 / 6 && AppRectGrid3ScaleSplineX.Value < 0.2)
                    {
                        AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.2;
                    }
                }
                catch 
                {
                    AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = AppRectGrid3Scale.ScaleX * 1;
                    if (AppRectGrid3ScaleSplineX.Value < 0.01)
                    {
                        AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                    }
                }
                //Trace.WriteLine(AppRectGrid3ScaleSplineX.Value);
                //AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value =1;
                if (AppRectGrid3ScaleSplineX.Value < 0.01)
                {
                    AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = 0.01;
                }


                    //Trace.WriteLine(AppRectGrid3ScaleSplineX.Value);
                AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);

                if (AppRect_Target == -1)
                {
                    AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + 0*ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);
                }

                StartRectAnimation(0, AppRect_Target);
                StartBgAnimation(0);

                AppWindowGrid0Back.Stop();
                AppRectGrid1Back.Stop();

                AppWindowGrid0BackScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppWindowGrid0BackScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppWindowGrid0BackHeightSpline.KeyTime = TimeSpan.FromSeconds(0.8 * (Application.Current as App).TransitionDurationTime);
                AppRectGrid2TransformSplineY.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.1 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.1 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));

                if (AppRectGrid1BackTransformSplineY.Value >= ActualHeight * 1 / 6)
                {
                    AppRectGrid2TransformSplineY.Value += 0;
                    BackEase1.Amplitude = 0.13; 
                    BackEase2.Amplitude = 0.13;
                    PowerEase3.Power = 4.0;
                    PowerEase4.Power = 4.0;
                    PowerEase5.Power = 4.0;
                }
                else if (AppRectGrid1BackTransformSplineY.Value <= -ActualHeight * 1 / 6)
                {
                    AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                    AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 4));
                    AppRectGrid2Transform2SplineX.Value = AppRectGrid2TransformSplineX.Value * 2.0;// - ActualHeight * (0.5 - FarPoint * 1.0) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint * 1.0);

                    AppRectGrid2TransformSplineY.Value = 0 + (FarPoint - 0.5) * ActualHeight; 
                    BackEase1.Amplitude = 0.12; 
                    BackEase2.Amplitude = 0.15;
                    PowerEase3.Power = 3.8;
                    PowerEase4.Power = 3.8;
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
                    AppRectGrid3ScaleSplineX.Value = AppRectGrid3ScaleSplineY.Value = (AppRectGrid3Scale.ScaleX * 0.7 + AppRectGrid3ScaleSplineY.Value * 0.3);
                    AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint);

                    AppRectGrid3ScaleSplineY.KeyTime = TimeSpan.FromSeconds(0.07 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1/4));
                    AppRectGrid3ScaleSplineX.KeyTime = TimeSpan.FromSeconds(0.07 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1/4));
                    BackEase1.Amplitude = 0.1;
                    BackEase2.Amplitude = 0.1;
                    PowerEase3.Power = 3.6;
                    PowerEase4.Power = 3.6;
                    PowerEase5.Power = 3.6;
                }

                if (AppRectGrid1BackTransformSplineY.Value <= -ActualHeight * 1 / 6 && AppRectGrid1BackTransformSplineY.Value != ActualHeight * FarPoint - ActualHeight * 0.5)
                {
                    AppRectGrid2StoryBoard2.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2Transform2SplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2Transform2SplineY.Value;

                    /*
                    AppRectGrid2TransformSplineY.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AppRectGrid3ScaleSplineY.Value * (0.5 - FarPoint) - 0.5 * ActualHeight * e.Velocities.Linear.Y * -0.2;
                    AppRectGrid2StoryBoard.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2TransformSplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2TransformSplineY.Value;*/
                }
                else
                {
                    AppRectGrid2StoryBoard.Begin();
                    AppRectGrid1Back2TransformSplineX.Value = AppRectGrid1BackTransformSplineX.Value - AppRectGrid2TransformSplineX.Value;
                    AppRectGrid1Back2TransformSplineY.Value = AppRectGrid1BackTransformSplineY.Value - AppRectGrid2TransformSplineY.Value;
                }
                AppRectGrid3StoryBoardC1.Begin();
                AppWindowGrid0Back.Begin();
                AppRectGrid1BackCurveBounce.Begin();
            }
            else if(e.Velocities.Linear.Y >= -2 || true)
            {
                frameTopScaleT.ScaleX = frameTopScaleT.ScaleY = 1;
                GstBut.Visibility = Visibility.Visible;
                AppRect_AnState = 10;
                AppRect_AnState = 10;
                AppRectGrid3StoryBoardFill.Begin();
                AppRectGrid2Fill.Begin();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StartRectAnimation(1, -1);
            StartRectAnimation(0, -1);
        }

        private void AppRectGrid3StoryBoardFill_Completed(object sender, object e)
        {
            RoundCornerTimer.Stop();
            AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            if ((AppRect_AnState == 10 || AppRect_AnState == 1))
            {
                AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
            }
            AppRect_AnState = 10;
        }

        private void GstBut_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {/*
            RoundCornerPointerStBo.Stop();
            RoundCornerTimer.Stop();*/
            if(AppRectGrid2StoryBoard.GetCurrentState() == ClockState.Active && AppRect_AnState != 8)
            {
                AppRectGrid3StoryBoardC1.Stop();
                AppRectGrid2StoryBoard.Stop();
            }
            if(AppRectGrid3StoryBoardFill.GetCurrentState() == ClockState.Active && AppRect_AnState != 8)
            {
                AppRectGrid3StoryBoardFill.Stop();
                AppRectGrid2Fill.Stop();
                AppRectGrid2Fill.Stop();
            }
            GstBut.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.TranslateX | Windows.UI.Xaml.Input.ManipulationModes.TranslateY;

            if (AppRect_Target != -2 && (AppRect_AnState == 10 || AppRect_AnState == 1) && AppRect_AnState != 8)
            {
                AppRect_AnState = 9;
                MouseDownX = e.Position.X;
                MouseDownY = e.Position.Y;
                AppWindowGrid0.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
                /*
                double x = AppRectGrid1Transform.X, y = AppRectGrid1Transform.Y, sx = AppWindowGrid0Scale.ScaleX, h = AppWindowGrid0.ActualHeight, cy = AppWindowGrid0Scale.CenterY;
                AppRectGrid1StoryBoard.Stop();
                AppWindowGrid0StoryBoard.Stop();
                AppRectGrid1Transform.X = x; AppRectGrid1Transform.Y = y; AppWindowGrid0Scale.ScaleX = AppWindowGrid0Scale.ScaleY = sx; AppWindowGrid0.Height = h;
                */

            }
        }

        private void GstBut_Click(object sender, RoutedEventArgs e)
        {
            if (AppRect_Target != -2 && (AppRect_AnState == 10 || AppRect_AnState == 1))
            {
                SetSwBarBk(0);
                AppRectGrid3Scale.ScaleX = AppRectGrid3Scale.ScaleY = 1;
                AppRectGrid2Transform.X = AppRectGrid2Transform.Y = 0;
                StartRectAnimation(0, AppRect_Target);
                StartBgAnimation(0);
                return;
            }
        }

        public void SetSwBarBk(int i = 0) //0def 1white 2black
        {
            if(i == 0)
            {
                Frame.RequestedTheme = ElementTheme.Default;
                SwipeBar.RequestedTheme = ElementTheme.Default;
            }
            else if(i == 1)
            {
                Frame.RequestedTheme = ElementTheme.Light;
                SwipeBar.RequestedTheme = ElementTheme.Light;
            }
            else if(i == 2)
            {
                Frame.RequestedTheme = ElementTheme.Dark;
                SwipeBar.RequestedTheme = ElementTheme.Dark;
            }
        }

    }
}
