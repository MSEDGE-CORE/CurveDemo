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
using Windows.UI.Input;
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

    class AppAnimationRectProperties
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

        int SpringCurveStyleCount = 3;

        public double SpringAnimationInterpolator(double damping, long durationTick, double velocity0, double value0, double value1, long nowTick)
        {
            double amplitude = value1 - value0, w = 4 * Math.PI / durationTick;
            if (damping <= 0)
            {
                return 0;
            }
            if(amplitude == 0 && velocity0 == 0 || durationTick == 0)
            {
                return value1;
            }
            if(nowTick < 0)
            {
                return value0;
            }
            
            if (damping < 1) // 欠阻尼
            {
                double wd = w * Math.Sqrt((1 - damping * damping));
                return value0 + amplitude * (1 - Math.Exp(-damping * w * nowTick) * (Math.Cos(wd * nowTick) + damping * (w / wd * Math.Sin(wd * nowTick)))) + velocity0 / wd * Math.Exp(-damping * w * nowTick) * Math.Sin(wd * nowTick);
            }
            else if (damping == 1) // 临界阻尼
            {
                return value0 + amplitude * (1 - Math.Exp(-w * nowTick) * (1 + w * nowTick)) + velocity0 * nowTick * Math.Exp(-w * nowTick);
            }
            else if (damping > 1) //过阻尼
            {
                double l1 = w * (damping - Math.Sqrt(damping * damping - 1));
                double l2 = w * (damping + Math.Sqrt(damping * damping - 1));
                return value0 + amplitude * (1 + (l1 * Math.Exp(-l2 * nowTick) - l2 * Math.Exp(-l1 * nowTick)) / (l2 - l1)) + (velocity0 / (l2 - l1)) * (Math.Exp(-l1 * nowTick) - Math.Exp(-l2 * nowTick));
            }
            return 0;
        }

        public double GetSpringAnimationVisibleTick(double damping, long durationTick, double velocity0, double value0, double value1, double thresholdPercent = 0.0001)
        {
            if (damping <= 0)
            {
                return 0;
            }
            double amplitude = value1 - value0;
            double w = 4 * Math.PI / durationTick;

            // 如果幅度为0且速度为0，瞬间结束
            if (Math.Abs(amplitude) < 1e-12 && Math.Abs(velocity0) < 1e-12)
                return 0;

            // 计算等效初始振幅（更精确）
            double A0;
            if (damping < 1) // 欠阻尼
            {
                double wd = w * Math.Sqrt(1 - damping * damping);
                // 初始包络幅度（初位移和初速度的综合影响）
                double x0 = amplitude; // 实际初始位移偏移
                double v0 = velocity0;
                // 包络幅度 = sqrt( x0^2 + ((v0 + damping*w*x0)/wd)^2 )，这是欠阻尼系统初态下的振幅
                double b = v0 + damping * w * x0;
                A0 = Math.Sqrt(x0 * x0 + (b * b) / (wd * wd));
                // 如果 amplitude 为0，但速度不为0，则 A0 = |v0|/wd
                if (Math.Abs(amplitude) < 1e-12) A0 = Math.Abs(v0) / wd;
            }
            else if (damping >= 1) // 临界或过阻尼
            {
                // 过阻尼无振荡，包络不是纯指数，但通常衰减很快
                // 可以近似用阻尼比=1时的包络，或者直接取 1 / (damping*w) 的倍数
                // 这里用临界阻尼的衰减时间常数: 1/w
                A0 = Math.Abs(amplitude) + Math.Abs(velocity0) / w; // 粗略估计
                                                                    // 对于过阻尼，衰减主导项是 exp(-w*(damping - sqrt(damping^2-1))*t)
                                                                    // 我们取较小的时间常数
                double lambda1 = w * (damping - Math.Sqrt(damping * damping - 1));
                // 如果 lambda1 很小（接近0），则衰减很慢，但通常 lambda1 > 0
                // 对于临界阻尼，lambda = w
                double effectiveW = (damping > 1) ? lambda1 : w;
                // 使用指数衰减计算
                if (effectiveW > 1e-12)
                {
                    double t = -Math.Log(thresholdPercent) / effectiveW;
                    // 但还要考虑初始速度，简单加一个安全系数
                    return t * 1.5; // 保守估算
                }
                else
                    return durationTick * 2; // 后备
            }
            else
                return 0;

            // 欠阻尼：包络衰减到 thresholdPercent 所需时间
            double dampingW = damping * w;
            if (dampingW < 1e-12) return durationTick * 10; // 无阻尼，永不停止

            double tVisible = -Math.Log(thresholdPercent) / dampingW;
            // 如果 A0 比 |amplitude| 大，说明速度贡献使振幅增大，可适当增加时间
            // 但为了简洁，直接返回 tVisible
            return tVisible;
        }

        private long ComputeVisibleDurationTick(AppAnimationRectProperties rect)
        {
            if (rect == null) return 0;
            double maxTick = 0;

            // X, Y, Scale, Width, Height, Rotation, CornerRadius
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.XDamping, rect.durationTick, rect.VX, rect.FromX, rect.ToX));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.YDamping, rect.durationTick, rect.VY, rect.FromY, rect.ToY));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.ScaleDamping, rect.durationTick, rect.VScale, rect.FromScale, rect.ToScale));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.WidthDamping, rect.durationTick, rect.VWidth, rect.FromWidth, rect.ToWidth));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.HeightDamping, rect.durationTick, rect.VHeight, rect.FromHeight, rect.ToHeight));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.RotationDamping, rect.durationTick, rect.VRotaion, rect.FromRotaion, rect.ToRotaion));
            maxTick = Math.Max(maxTick, GetSpringAnimationVisibleTick(rect.RotationDamping, rect.durationTick, rect.VCornerRadius, rect.FromCornerRadius, rect.ToCornerRadius));

            // opacity may have its own duration and begin offset
            double opTick = GetSpringAnimationVisibleTick(rect.OpacityDamping, rect.OpacityDurationTick, rect.VOpacity, rect.FromOpacity, rect.ToOpacity);
            opTick += rect.OpacityBeginTick; // account for delayed start
            maxTick = Math.Max(maxTick, opTick);

            if (maxTick < 0) maxTick = 0;
            return (long)Math.Ceiling(maxTick);
        }


        AppAnimationRectProperties MainAppRect = new AppAnimationRectProperties();
        AppAnimationRectProperties FallBehindAppRect = new AppAnimationRectProperties();


        //打开1，关闭2
        private void StartWindowSpringAnimation(int anDirection, AppAnimationRectProperties comingAppRect)
        {
            //X,Y,Scale,Height/Width,Opacity,Rotation以及并行

            MainAppRect.anStatus = true;
            MainAppRect.anDirection = anDirection;


            MainAppRect.FromX = AWATransform.X + AWGTransform.X;
            MainAppRect.FromY = AWATransform.Y + AWGTransform.Y;
            MainAppRect.FromScale = AWAScale.ScaleX * AWGScale.ScaleX;
            MainAppRect.FromWidth = AppHeightAnimation.Width;
            MainAppRect.FromHeight = AppHeightAnimation.Height;
            MainAppRect.FromOpacity = AWMultiTaskGrid.Opacity;
            MainAppRect.FromRotaion = AWARotate.Angle;
            MainAppRect.FromCornerRadius = AppHeightAnimation.CornerRadius.TopLeft;

            MainAppRect.durationTick = comingAppRect.durationTick;
            MainAppRect.VX = comingAppRect.VX;
            MainAppRect.VY = comingAppRect.VY;
            MainAppRect.VScale = comingAppRect.VScale;
            MainAppRect.ToX = comingAppRect.ToX;
            MainAppRect.ToY = comingAppRect.ToY;
            MainAppRect.ToWidth = comingAppRect.ToWidth;
            MainAppRect.ToHeight = comingAppRect.ToHeight;
            MainAppRect.ToScale = comingAppRect.ToScale;
            MainAppRect.ToOpacity = comingAppRect.ToOpacity;
            MainAppRect.ToRotaion = comingAppRect.ToRotaion;
            MainAppRect.ToCornerRadius = comingAppRect.ToCornerRadius;

            MainAppRect.XDamping = comingAppRect.XDamping;
            MainAppRect.YDamping = comingAppRect.YDamping;
            MainAppRect.ScaleDamping = comingAppRect.ScaleDamping;
            MainAppRect.WidthDamping = comingAppRect.WidthDamping;
            MainAppRect.HeightDamping = comingAppRect.HeightDamping;
            MainAppRect.RotationDamping = comingAppRect.RotationDamping;
            MainAppRect.CornerRadiusDamping = comingAppRect.CornerRadiusDamping;
            MainAppRect.OpacityDamping = comingAppRect.OpacityDamping;

            MainAppRect.OpacityBeginTick = comingAppRect.OpacityBeginTick;
            MainAppRect.OpacityDurationTick = comingAppRect.OpacityDurationTick;
            // 计算可视化持续时间（ticks），用于判断何时认为动画已停止
            MainAppRect.VisibleDurationTick = ComputeVisibleDurationTick(MainAppRect);
            MainAppRect.startTick = DateTime.Now.Ticks;

            if (anDirection == 2)
            {

                FallBehindAppRect.anStatus = true;
                FallBehindAppRect.anDirection = 2;

                FallBehindAppRect.FromX = AWATransform.X + AWGTransform.X;
                FallBehindAppRect.FromY = AWATransform.Y + AWGTransform.Y;
                FallBehindAppRect.FromScale = AWAScale.ScaleX * AWGScale.ScaleX;
                FallBehindAppRect.FromWidth = AppHeightAnimation.Width;
                FallBehindAppRect.FromHeight = AppHeightAnimation.Height;
                FallBehindAppRect.FromOpacity = AWMultiTaskGrid.Opacity;
                FallBehindAppRect.FromRotaion = AWARotate.Angle;

                FallBehindAppRect.FromCornerRadius = AppHeightAnimation.CornerRadius.TopLeft;
                FallBehindAppRect.durationTick = comingAppRect.durationTick;
                FallBehindAppRect.VX = comingAppRect.VX;
                FallBehindAppRect.VY = comingAppRect.VY;
                FallBehindAppRect.VScale = comingAppRect.VScale;
                FallBehindAppRect.ToX = comingAppRect.ToX;
                FallBehindAppRect.ToY = comingAppRect.ToY;
                FallBehindAppRect.ToWidth = comingAppRect.ToWidth;
                FallBehindAppRect.ToHeight = comingAppRect.ToHeight;
                FallBehindAppRect.ToScale = comingAppRect.ToScale;
                FallBehindAppRect.ToOpacity = comingAppRect.ToOpacity;
                FallBehindAppRect.ToRotaion = comingAppRect.ToRotaion;
                FallBehindAppRect.ToCornerRadius = comingAppRect.ToCornerRadius;
                
                FallBehindAppRect.XDamping = comingAppRect.XDamping;
                FallBehindAppRect.YDamping = comingAppRect.YDamping;
                FallBehindAppRect.ScaleDamping = comingAppRect.ScaleDamping;
                FallBehindAppRect.WidthDamping = comingAppRect.WidthDamping;
                FallBehindAppRect.HeightDamping = comingAppRect.HeightDamping;
                FallBehindAppRect.RotationDamping = comingAppRect.RotationDamping;
                FallBehindAppRect.CornerRadiusDamping = comingAppRect.CornerRadiusDamping;
                FallBehindAppRect.OpacityDamping = comingAppRect.OpacityDamping;

                FallBehindAppRect.OpacityBeginTick = comingAppRect.OpacityBeginTick;
                FallBehindAppRect.OpacityDurationTick = comingAppRect.OpacityDurationTick;
                FallBehindAppRect.VisibleDurationTick = ComputeVisibleDurationTick(FallBehindAppRect);
                FallBehindAppRect.startTick = DateTime.Now.Ticks;


            }

            Windows.UI.Xaml.Media.CompositionTarget.Rendering -= CompositionTarget_RenderingSpringAnimation;
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += CompositionTarget_RenderingSpringAnimation;
        }



        private void CompositionTarget_RenderingSpringAnimation(object? sender, object e)
        {
            //X,Y,Scale,Height/Width,Opacity,Rotation以及并行

            long nowTick = DateTime.Now.Ticks;

            // 根据可视化持续时间判断每个 rect 是否已停止（使用 GetSpringAnimationVisibleTick 计算）
            if (MainAppRect.anStatus)
            {
                if (nowTick - MainAppRect.startTick >= MainAppRect.VisibleDurationTick)
                {
                    // 标记已结束并把最终值应用到 UI（执行原来 if 分支的结尾处理）
                    MainAppRect.anStatus = false;
                    // 确保最终状态为目标值
                    AWATransform.X = MainAppRect.ToX;
                    AWATransform.Y = MainAppRect.ToY;
                    AWAScale.ScaleX = AWAScale.ScaleY = MainAppRect.ToScale;
                    AWMultiTaskGrid.Opacity = MainAppRect.ToOpacity;
                    AWARotate.Angle = MainAppRect.ToRotaion;
                    AppHeightAnimation.Width = MainAppRect.ToWidth;
                    AppHeightAnimation.Height = MainAppRect.ToHeight;
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(MainAppRect.ToCornerRadius);

                    if (MainAppRect.anDirection == 1)
                    {
                        AWALaunchingStoryBoard_Completed(null, null);
                        if(AWGScale.ScaleX == 1 && AWGTransform.X == 0 && AWGTransform.Y == 0)
                            AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
                    }
                    else if(MainAppRect.anDirection == 2)
                    {
                        AWABackStoryBoard_Completed(null, null);
                    }
                    return;
                }
                else if(MainAppRect.anDirection == 1 && nowTick - MainAppRect.startTick >= MainAppRect.VisibleDurationTick * 0.2)
                {
                    AWReturnToApp.Visibility = Visibility.Collapsed;
                }
            }

            if (FallBehindAppRect.anStatus)
            {
                if (nowTick - FallBehindAppRect.startTick >= FallBehindAppRect.VisibleDurationTick)
                {
                    FallBehindAppRect.anStatus = false;
                    AW2GTransform.X = FallBehindAppRect.ToX;
                    AW2GTransform.Y = FallBehindAppRect.ToY;
                    AW2GScale.ScaleX = AW2GScale.ScaleY = FallBehindAppRect.ToScale;
                    AW2ARotate.Angle = FallBehindAppRect.ToRotaion;
                    AppHeight2Animation.Width = FallBehindAppRect.ToWidth;
                    AppHeight2Animation.Height = FallBehindAppRect.ToHeight;
                    AppHeight2Animation.CornerRadius = new Windows.UI.Xaml.CornerRadius(FallBehindAppRect.ToCornerRadius);

                    AW2ABackStoryBoard_Completed(null, null);
                    return;
                }
            }

            // 如果两个都结束则结束整个动画并取消订阅
            if (!MainAppRect.anStatus && !FallBehindAppRect.anStatus)
            {
                MainAppRect.anStatus = false;
                FallBehindAppRect.anStatus = false;
                Windows.UI.Xaml.Media.CompositionTarget.Rendering -= CompositionTarget_RenderingSpringAnimation;
                return;
            }

            //Trace.WriteLine((nowTick - MainAppRect.startTick).ToString());
            //Trace.WriteLine((AWATransform.X, AWAScale.ScaleX, AppHeightAnimation.ActualWidth, AppHeightAnimation.ActualHeight, AWMultiTaskGrid.Opacity));
            //Trace.WriteLine( nowTick-MainAppRect.OpacityBeginTick - MainAppRect.startTick);

            AWATransform.X = SpringAnimationInterpolator(MainAppRect.XDamping, MainAppRect.durationTick, MainAppRect.VX, MainAppRect.FromX, MainAppRect.ToX, nowTick - MainAppRect.startTick);
            AWATransform.Y = SpringAnimationInterpolator(MainAppRect.YDamping, MainAppRect.durationTick, MainAppRect.VY, MainAppRect.FromY, MainAppRect.ToY, nowTick - MainAppRect.startTick);
            double preparingScale = SpringAnimationInterpolator(MainAppRect.ScaleDamping, MainAppRect.durationTick, MainAppRect.VScale, MainAppRect.FromScale, MainAppRect.ToScale, nowTick - MainAppRect.startTick);
            //Trace.WriteLine(preparingScale);
            double minScale = (MainAppRect.ToScale * ActualWidth <= 4 && MainAppRect.ToX == 0) ? 0.1 : (MainAppRect.ToScale < MainAppRect.FromScale ? MainAppRect.ToScale : MainAppRect.FromScale);
            if (MainAppRect.anDirection == 2 && preparingScale < minScale)
            {
                //Trace.WriteLine((minScale));
                double a = minScale;
                preparingScale = -(a * (-1 / (Math.Abs(preparingScale - minScale) / a + 1) + 1)) + minScale;
            }
            AWAScale.ScaleX = AWAScale.ScaleY = preparingScale;
            AWSwipeBar.Opacity = AWMultiTaskGrid.Opacity = SpringAnimationInterpolator(MainAppRect.OpacityDamping, MainAppRect.OpacityDurationTick, MainAppRect.VOpacity, MainAppRect.FromOpacity, MainAppRect.ToOpacity, nowTick - MainAppRect.OpacityBeginTick - MainAppRect.startTick < 0 ? 0 : nowTick - MainAppRect.OpacityBeginTick - MainAppRect.startTick);
            AWARotate.Angle = SpringAnimationInterpolator(MainAppRect.RotationDamping, MainAppRect.durationTick, MainAppRect.VRotaion, MainAppRect.FromRotaion, MainAppRect.ToRotaion, nowTick - MainAppRect.startTick);
            AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(SpringAnimationInterpolator(MainAppRect.CornerRadiusDamping, MainAppRect.durationTick, MainAppRect.VCornerRadius, MainAppRect.FromCornerRadius, MainAppRect.ToCornerRadius, nowTick - MainAppRect.startTick));
            AppHeightAnimation.Width = Math.Max(0,SpringAnimationInterpolator(MainAppRect.WidthDamping, MainAppRect.durationTick, MainAppRect.VWidth, MainAppRect.FromWidth, MainAppRect.ToWidth, nowTick - MainAppRect.startTick));
            AppHeightAnimation.Height = Math.Max(0,SpringAnimationInterpolator(MainAppRect.HeightDamping, MainAppRect.durationTick, MainAppRect.VHeight, MainAppRect.FromHeight, MainAppRect.ToHeight, nowTick - MainAppRect.startTick));
            RoundCornerPointerTransform.X = AppHeightAnimation.CornerRadius.TopLeft;
            

            if (FallBehindAppRect.anStatus)
            {

                AW2ATransform.X = SpringAnimationInterpolator(FallBehindAppRect.XDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VX, FallBehindAppRect.FromX, FallBehindAppRect.ToX, nowTick - FallBehindAppRect.startTick);
                AW2ATransform.Y = SpringAnimationInterpolator(FallBehindAppRect.YDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VY, FallBehindAppRect.FromY, FallBehindAppRect.ToY, nowTick - FallBehindAppRect.startTick);
                
                preparingScale = SpringAnimationInterpolator(FallBehindAppRect.ScaleDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VScale, FallBehindAppRect.FromScale, FallBehindAppRect.ToScale, nowTick - FallBehindAppRect.startTick);
                minScale = (MainAppRect.ToScale * ActualWidth <= 4 && MainAppRect.ToX == 0) ? 0.1 : (FallBehindAppRect.ToScale < FallBehindAppRect.FromScale ? FallBehindAppRect.ToScale : FallBehindAppRect.FromScale);
                if (preparingScale < minScale)
                {
                    double a = minScale;
                    preparingScale = -(a * (-1 / (Math.Abs(preparingScale - minScale) / a + 1) + 1)) + minScale;
                }
                AW2AScale.ScaleX = AW2AScale.ScaleY = preparingScale;

                AW2ARotate.Angle = SpringAnimationInterpolator(FallBehindAppRect.RotationDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VRotaion, FallBehindAppRect.FromRotaion, FallBehindAppRect.ToRotaion, nowTick - FallBehindAppRect.startTick);
                AppHeight2Animation.CornerRadius = new Windows.UI.Xaml.CornerRadius(SpringAnimationInterpolator(FallBehindAppRect.CornerRadiusDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VCornerRadius, FallBehindAppRect.FromCornerRadius, FallBehindAppRect.ToCornerRadius, nowTick - FallBehindAppRect.startTick));
                AppHeight2Animation.Width = Math.Max(0,SpringAnimationInterpolator(FallBehindAppRect.WidthDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VWidth, FallBehindAppRect.FromWidth, FallBehindAppRect.ToWidth, nowTick - FallBehindAppRect.startTick));
                AppHeight2Animation.Height = Math.Max(0,SpringAnimationInterpolator(FallBehindAppRect.HeightDamping, FallBehindAppRect.durationTick, FallBehindAppRect.VHeight, FallBehindAppRect.FromHeight, FallBehindAppRect.ToHeight, nowTick - FallBehindAppRect.startTick));
                RoundCornerPointer2Transform.X = AppHeightAnimation.CornerRadius.TopLeft;
            }

        }

        private void Timer_Tick(object? sender, object e)
        {
            Page_SizeChanged(null, null);
            //Trace.WriteLine(ApFrTranslate.Y);
        }


        public int AppWindowMain_Target = -2, AppWindowMain_mIndex = -2, AppWindowState = 0, AppWindow2_Target = -2;
        double RvX = 0, RvY = 0, RvScale = 0;//swipingVelocity 
        public async void StartWindowAnimation(int isOnLaunching = 0, int AppTarget = -2, double[] WindowPositionFrom = null) // X Y 宽 高
        {
            AppWindowGesture.Visibility = Visibility.Visible;
            AppAnimationRectProperties comingAppRect = new AppAnimationRectProperties();

            if (isOnLaunching == -1)
            {
                DesktopIconGridWhenClosingAppScale.ScaleX = 0;
                Windows.UI.Xaml.Media.CompositionTarget.Rendering -= CompositionTarget_RenderingSpringAnimation;
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
                if (AppWindow2_Target >= 0)
                {
                    AppWindow2Gesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    DesktopIconGridWhenClosingAppScale.ScaleX = 0;
                    (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
                }
            }
            else if (isOnLaunching == 1)
            {
                DesktopIconGridWhenClosingAppScale.ScaleX = 0;
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

                        /*
                        BlurPointerStoryBoard.Stop();
                        BlurPointerAnimation.From = BlurPointerTransform.X = 1;
                        StartBackgroundAnimation(1);*/
                    }
                    else if (AppWindowMain_Target == -3)
                    {
                        AppWindow2Gesture.Opacity = 0.01;
                        AppWindow2Gesture.Visibility = Visibility.Collapsed;
                        if (AppWindow2_Target >= 0)
                            (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
                        if (AppWindowMain_Target >= 0)
                            (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
                    }
                    else 
                    {
                        AppWindow2Gesture.Opacity = 0.01;
                        AppWindow2Gesture.Visibility = Visibility.Collapsed;
                        if (AppWindow2_Target >= 0)
                            (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
                        if (AppWindowMain_Target >= 0)
                            (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
                    }
                }

                comingAppRect.anDirection = 1;

                if (AppTarget != -2 && AppTarget != -1 && AppTarget != -3 && AppTarget != -4)
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
                        AWABackStoryBoard.Stop();
                        AWAGestureBackStoryBoard.Stop();
                        AWAGestureBack2StoryBoard.Stop();
                        AWGTransform.X = AWGTransform.Y = 0;
                        AWGScale.ScaleX = AWGScale.ScaleY = 1.0;

                        AWAScale.ScaleX = AWAScale.ScaleY = 0.01;
                        AWATransform.X = 0;
                        AWATransform.Y = 0;

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
                            AWBackIconRotate.CenterX = ActualHeight / 2.0;
                            AWBackIconRotate.CenterY = ActualHeight / 2.0;
                            AWFrontIconRotate.CenterX = ActualHeight / 2.0;
                            AWFrontIconRotate.CenterY = ActualHeight / 2.0;

                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                            AppHeightAnimation.Height = ActualHeight;
                            AppHeightAnimation.Width = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                        }
                        else
                        {

                            AWBackIconRotate.CenterX = ActualWidth / 2.0;
                            AWBackIconRotate.CenterY = ActualWidth / 2.0;
                            AWFrontIconRotate.CenterX = ActualWidth / 2.0;
                            AWFrontIconRotate.CenterY = ActualWidth / 2.0;

                            AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                            AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualWidth * 0.5;
                            AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualWidth * 0.5;
                            AppHeightAnimation.Width = ActualWidth;
                            AppHeightAnimation.Height = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / (DesktopGrid.Children[AppTarget] as Grid).ActualWidth * AppHeightAnimation.Width;
                            

                            AWAScale.ScaleX = AWAScale.ScaleY = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth;
                        }
                        AWARotate.Angle = 0;
                        AWBackIconRotate.Angle = 0;
                        AWFrontIconRotate.Angle = 0;


                        AWAScale.CenterX = ActualWidth * 0.5;
                        AWAScale.CenterY = ActualHeight * 0.5;
                        AWATransform.X = -(ActualWidth - ActualWidth * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.X - DesktopGridScrollViewer.HorizontalOffset + (DesktopGrid.Children[AppTarget]).ActualOffset.X - 0 + AWAScale.ScaleX * (AppHeightAnimation.Width - ActualWidth) / 2.0;
                        AWATransform.Y = -(ActualHeight - ActualHeight * AWAScale.ScaleX) * 0.5 + DesktopGrid.ActualOffset.Y - DesktopGridScrollViewer.VerticalOffset + (DesktopGrid.Children[AppTarget]).ActualOffset.Y - 0 + AWAScale.ScaleX * (AppHeightAnimation.Height - ActualHeight) / 2.0;
                        RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);
                        RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / ((DesktopGrid.Children[AppTarget] as Grid).ActualWidth / ActualWidth);

                        AWMultiTaskGrid.Opacity = 0;
                        if((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                        {
                            AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                        }
                        else
                        {
                            AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                        }



                        if (AppTarget == 5 && (Application.Current as App).CurveStyle < SpringCurveStyleCount) //特殊待遇
                        {
                            AWBackIconRotate.Angle = 90;
                            AWFrontIconRotate.Angle = 90;
                            AWARotate.Angle = -90;

                        }
                        if (AppTarget == 23 && (Application.Current as App).CurveStyle < SpringCurveStyleCount) //特殊待遇
                        {
                            AWBackIconRotate.Angle = -90;
                            AWFrontIconRotate.Angle = -90;
                            AWARotate.Angle = 90;

                        }
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

                    comingAppRect.ToHeight = ActualHeight;
                    comingAppRect.ToWidth = ActualWidth;
                    comingAppRect.ToX = 0;
                    comingAppRect.ToY = 0;
                    comingAppRect.ToOpacity = 1.0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = 1.0;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.2 * (Application.Current as App).TransitionDurationTime * 10000000);


                    

                    AWAFrameOpacityDoubleAnimation.From = AWMultiTaskGrid.Opacity;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);



                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        StartWindowSpringAnimation(1,comingAppRect);
                    }
                    else
                    {
                        //await Task.Delay(0);
                        AWALaunchingStoryBoard.Begin();
                        AWAFrameOpacity.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();
                    }

                    (DesktopGrid.Children[AppTarget] as Grid).Opacity = 0.01;

                }
                else if(AppTarget == -3 && WindowPositionFrom != null)
                {

                    AWARotate.Angle = 0;
                    AWBackIconRotate.Angle = 0;
                    AWFrontIconRotate.Angle = 0;

                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;
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
                    (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";

                    AWBackgIcon.Source = null;
                    AWFrontIcon.Source = null;
                    

                    if ((WindowPositionFrom[2] / WindowPositionFrom[3]) <= (ActualWidth / ActualHeight))
                    {
                        AppHeightAnimation.Height = ActualHeight;
                        AppHeightAnimation.Width = WindowPositionFrom[2] / WindowPositionFrom[3] * AppHeightAnimation.Height;

                        AWAScale.ScaleX = AWAScale.ScaleY = WindowPositionFrom[3] / ActualHeight;
                    }
                    else
                    {
                        AppHeightAnimation.Width = ActualWidth;
                        AppHeightAnimation.Height = WindowPositionFrom[3] / WindowPositionFrom[2] * AppHeightAnimation.Width;

                        AWAScale.ScaleX = AWAScale.ScaleY = WindowPositionFrom[2] / ActualWidth;
                    }

                    AWFrontIcon.Opacity = 0;
                    AWBackgIcon.Opacity = 0;
                    AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                    AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                    AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;

                    AWAFrameOpacityDoubleAnimation.From = 0;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);

                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;
                    AWATransform.X = -(ActualWidth) * 0.5 + WindowPositionFrom[0] - 0 * WindowPositionFrom[2];
                    AWATransform.Y = -(ActualHeight) * 0.5 + WindowPositionFrom[1] - 0 * WindowPositionFrom[3];
                    /*RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / (WindowPositionFrom[2] / ActualWidth);
                    RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / (WindowPositionFrom[2] / ActualWidth);
                    */
                    AWMultiTaskGrid.Opacity = 0;
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                    }
                    else
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }
                    AppWindowState = 1;
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

                    comingAppRect.ToHeight = ActualHeight;
                    comingAppRect.ToWidth = ActualWidth;
                    comingAppRect.ToX = 0;
                    comingAppRect.ToY = 0;
                    comingAppRect.ToOpacity = 1.0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = 1.0;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);


                    //await Task.Delay(0);
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        StartWindowSpringAnimation(1, comingAppRect);
                    }
                    else
                    {
                        AWALaunchingStoryBoard.Begin();
                        AWAFrameOpacity.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();
                    }
                }
                else if (AppTarget == -4 && WindowPositionFrom != null)
                {

                    AWARotate.Angle = 0;
                    AWBackIconRotate.Angle = 0;
                    AWFrontIconRotate.Angle = 0;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;
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
                    (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";

                    AWBackgIcon.Source = null;
                    AWFrontIcon.Source = null;


                    if ((WindowPositionFrom[2] / WindowPositionFrom[3]) <= (ActualWidth / ActualHeight))
                    {
                        AppHeightAnimation.Height = ActualHeight;
                        AppHeightAnimation.Width = WindowPositionFrom[2] / WindowPositionFrom[3] * AppHeightAnimation.Height;

                        AWAScale.ScaleX = AWAScale.ScaleY = WindowPositionFrom[3] / ActualHeight;
                    }
                    else
                    {
                        AppHeightAnimation.Width = ActualWidth;
                        AppHeightAnimation.Height = WindowPositionFrom[3] / WindowPositionFrom[2] * AppHeightAnimation.Width;

                        AWAScale.ScaleX = AWAScale.ScaleY = WindowPositionFrom[2] / ActualWidth;
                    }

                    AWFrontIcon.Opacity = 0;
                    AWBackgIcon.Opacity = 0;
                    AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                    AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                    AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;

                    AWAFrameOpacityDoubleAnimation.From = 1;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);

                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;
                    AWATransform.X = -(ActualWidth) * 0.5 + WindowPositionFrom[0] - 0 * WindowPositionFrom[2];
                    AWATransform.Y = -(ActualHeight) * 0.5 + WindowPositionFrom[1] - 0 * WindowPositionFrom[3];
                    RoundCornerPointerAnimation.From = 500 * 100 / 1920 * 3 * 0.16 / (WindowPositionFrom[2] / ActualWidth);
                    RoundCornerPointerTransform.X = 500 * 100 / 1920 * 3 * 0.16 / (WindowPositionFrom[2] / ActualWidth);

                    AWMultiTaskGrid.Opacity = 0;
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                    }
                    else
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }
                    AppWindowState = 1;
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


                    comingAppRect.ToHeight = ActualHeight;
                    comingAppRect.ToWidth = ActualWidth;
                    comingAppRect.ToX = 0;
                    comingAppRect.ToY = 0;
                    comingAppRect.ToOpacity = 1.0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = 1.0;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.2 * (Application.Current as App).TransitionDurationTime * 10000000);


                    //await Task.Delay(0);
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        StartWindowSpringAnimation(1, comingAppRect);
                    }
                    else
                    {
                        //await Task.Delay(0);
                        AWALaunchingStoryBoard.Begin();
                        AWAFrameOpacity.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();
                    }
                }
                else if (AppTarget == -1 || true)
                {

                    AWARotate.Angle = 0;
                    AWBackIconRotate.Angle = 0;
                    AWFrontIconRotate.Angle = 0;
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

                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                    }
                    else
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }
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

                    AWAFrameOpacityDoubleAnimation.From = 0;
                    AWAFrameOpacityDoubleAnimation.To = 1;
                    AWAFrameOpacityDoubleAnimation.Duration = TimeSpan.FromSeconds(0.3 * (Application.Current as App).TransitionDurationTime);
                    AWAFrameOpacityDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0.0);


                    comingAppRect.ToHeight = ActualHeight;
                    comingAppRect.ToWidth = ActualWidth;
                    comingAppRect.ToX = 0;
                    comingAppRect.ToY = 0;
                    comingAppRect.ToOpacity = 1.0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = 1.0;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                    

                    //AWSwipeBar.Opacity = 1;
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        StartWindowSpringAnimation(1, comingAppRect);
                    }
                    else
                    {
                        AWALaunchingStoryBoard.Begin();
                        AWAFrameOpacity.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();
                    }
                    AppWindowState = 1;
                }
                    AppWindowGesture.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppWindowGesture.Opacity = 1;


            }
            else if(isOnLaunching == 0)
            {
                comingAppRect.anDirection = 2;
                comingAppRect.VX = RvX * ((Application.Current as App).CurveStyle == 2 ? (Application.Current as App).XVelocity : 1);
                comingAppRect.VY = RvY * ((Application.Current as App).CurveStyle == 2 ? (Application.Current as App).YVelocity : 1);
                comingAppRect.VScale = RvScale * ((Application.Current as App).CurveStyle == 2 ? (Application.Current as App).ScaleVelocity : 1);

                AppTarget = FindAppTarget(AppTarget);
                DesktopIconGridWhenClosingAppScale.ScaleX = 1;
                DesktopGridScrollViewer2.ScrollToVerticalOffset(DesktopGridScrollViewer.VerticalOffset);

                AW2ABackStoryBoard.Stop();
                AW2AGestureBackStoryBoard.Stop();
                AW2AGestureBack2StoryBoard.Stop();
                AW2GGestureFlyStoryBoard.Stop();
                AppWindowState = 2;

                if (AppTarget != -1 && AppTarget != -2 && AppTarget != -3 && AppTarget != -4)
                {
                    AWBackgIcon.Opacity = 1; AWFrontIcon.Opacity = 1;
                    AWAScale.CenterX = ActualWidth * 0.5;
                    AWAScale.CenterY = ActualHeight * 0.5;

                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                    }
                    else
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }

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

                        AWBackIconRotate.CenterX = ActualHeight / 2.0;
                        AWBackIconRotate.CenterY = ActualHeight / 2.0;
                        AWFrontIconRotate.CenterX = ActualHeight / 2.0;
                        AWFrontIconRotate.CenterY = ActualHeight / 2.0;

                        AWBackIconScale.ScaleX = AWBackIconScale.ScaleY = AWFrontIconScale.ScaleX = AWFrontIconScale.ScaleY = 1.5;
                        AWBackIconScale.CenterX = AWFrontIconScale.CenterX = ActualHeight * 0.5;
                        AWBackIconScale.CenterY = AWFrontIconScale.CenterY = ActualHeight * 0.5;
                        AWABackKeyH.Value = ActualHeight;
                        AWABackKeyW.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualWidth / (DesktopGrid.Children[AppTarget] as Grid).ActualHeight * AppHeightAnimation.Height;

                        AWABackKeyScaleX.Value = AWABackKeyScaleY.Value = (DesktopGrid.Children[AppTarget] as Grid).ActualHeight / ActualHeight;
                    }
                    else
                    {

                        AWBackIconRotate.CenterX = ActualWidth / 2.0;
                        AWBackIconRotate.CenterY = ActualWidth/ 2.0;
                        AWFrontIconRotate.CenterX = ActualWidth / 2.0;
                        AWFrontIconRotate.CenterY = ActualWidth / 2.0;

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


                    comingAppRect.ToHeight = AWABackKeyH.Value;
                    comingAppRect.ToWidth = AWABackKeyW.Value;
                    comingAppRect.ToX = AWABackKeyX.Value;
                    comingAppRect.ToY = AWABackKeyY.Value;
                    comingAppRect.ToOpacity = 0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = AWABackKeyScaleX.Value;
                    comingAppRect.ToCornerRadius = 500 * 100 / 1920 * 0.8 / AWABackKeyScaleX.Value;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                    if (AppTarget == 5 && (Application.Current as App).CurveStyle < SpringCurveStyleCount) //特殊待遇
                    {
                        comingAppRect.ToHeight = AWABackKeyW.Value;
                        comingAppRect.ToWidth = AWABackKeyH.Value;
                        comingAppRect.ToRotaion = -90;

                    }
                    if (AppTarget == 23 && (Application.Current as App).CurveStyle < SpringCurveStyleCount) //特殊待遇
                    {
                        comingAppRect.ToHeight = AWABackKeyW.Value;
                        comingAppRect.ToWidth = AWABackKeyH.Value;
                        comingAppRect.ToRotaion = 90;

                    }

                    if (RvScale != 0)
                    {
                        if((Application.Current as App).CurveStyle == 0)
                        {

                            comingAppRect.durationTick = (long)(1.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = 0.82;
                            comingAppRect.XDamping = 0.82;
                            comingAppRect.YDamping = 0.82;
                            comingAppRect.WidthDamping = 0.9;
                            comingAppRect.HeightDamping = 0.9;
                            comingAppRect.RotationDamping = 0.9;
                        }
                        else if((Application.Current as App).CurveStyle == 1)
                        {

                            comingAppRect.durationTick = (long)(1.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = 1.0 - comingAppRect.ToY / ActualHeight * 0.3;
                            comingAppRect.XDamping = 0.82;
                            comingAppRect.YDamping = 0.82;
                            comingAppRect.WidthDamping = 0.9;
                            comingAppRect.HeightDamping = 0.9;
                            comingAppRect.RotationDamping = 0.9;
                        }
                        else if ((Application.Current as App).CurveStyle == 2)
                        {
                            comingAppRect.durationTick = (long)((Application.Current as App).DurationTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)((Application.Current as App).OpacityBeginTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)((Application.Current as App).OpacityDurationTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = (Application.Current as App).ScaleDamping;
                            comingAppRect.XDamping = (Application.Current as App).XDamping;
                            comingAppRect.YDamping = (Application.Current as App).YDamping;
                            comingAppRect.WidthDamping = (Application.Current as App).WidthDamping;
                            comingAppRect.HeightDamping = (Application.Current as App).HeightDamping;
                            comingAppRect.RotationDamping = (Application.Current as App).RotationDamping;
                            comingAppRect.CornerRadiusDamping = (Application.Current as App).CornerRadiusDamping;
                            comingAppRect.OpacityDamping = (Application.Current as App).OpacityDamping;
                        }
                    }
                    


                    if(comingAppRect.ToY >= -ActualHeight * 0.5 && comingAppRect.ToY <= ActualHeight * 0.5 && comingAppRect.ToX >= -ActualWidth * 0.5 && comingAppRect.ToX <= ActualWidth * 0.5)
                    {
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


                        if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                        {
                            StartWindowSpringAnimation(2, comingAppRect);
                        }
                        else
                        {

                            AWABackStoryBoard.Begin();
                            AWAFrameOpacity.Begin();
                            RoundCornerPointerStBo.Begin();
                            RoundCornerTimer.Start();


                        }
                    }
                    else
                    {
                        AppTarget = -1;
                    }
                    


                }
                if (AppTarget == -1 || AppTarget == -3 || AppTarget == -4)
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

                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.0;
                    }
                    else
                    {
                        AWAMultiTaskScale.ScaleX = AWAMultiTaskScale.ScaleY = 1.2 / AWAScale.ScaleX;
                    }
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
                    
                    comingAppRect.ToHeight = AWABackKeyH.Value;
                    comingAppRect.ToWidth = AWABackKeyW.Value;
                    comingAppRect.ToX = AWABackKeyX.Value;
                    comingAppRect.ToY = AWABackKeyY.Value;
                    comingAppRect.ToOpacity = 0;
                    comingAppRect.ToRotaion = 0;
                    comingAppRect.ToScale = AWABackKeyScaleX.Value;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.durationTick = (long)(0.7 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                    comingAppRect.OpacityDurationTick = (long)(0.6 * (Application.Current as App).TransitionDurationTime * 10000000);
                    if (RvScale != 0)
                    {
                        if ((Application.Current as App).CurveStyle == 0)
                        {

                            comingAppRect.durationTick = (long)(1.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = 0.81;
                            comingAppRect.XDamping = 0.82;
                            comingAppRect.YDamping = 0.82;
                            comingAppRect.WidthDamping = 0.9;
                            comingAppRect.HeightDamping = 0.9;
                            comingAppRect.RotationDamping = 0.9;
                        }
                        else if ((Application.Current as App).CurveStyle == 1)
                        {

                            comingAppRect.durationTick = (long)(1.0 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)(0.1 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)(0.3 * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = 1.0 - comingAppRect.ToY / ActualHeight * 0.3;
                            comingAppRect.XDamping = 0.82;
                            comingAppRect.YDamping = 0.82;
                            comingAppRect.WidthDamping = 0.9;
                            comingAppRect.HeightDamping = 0.9;
                            comingAppRect.RotationDamping = 0.9;
                        }
                        else if ((Application.Current as App).CurveStyle == 2)
                        {
                            comingAppRect.durationTick = (long)((Application.Current as App).DurationTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityBeginTick = (long)((Application.Current as App).OpacityBeginTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.OpacityDurationTick = (long)((Application.Current as App).OpacityDurationTime * (Application.Current as App).TransitionDurationTime * 10000000);
                            comingAppRect.ScaleDamping = (Application.Current as App).ScaleDamping;
                            comingAppRect.XDamping = (Application.Current as App).XDamping;
                            comingAppRect.YDamping = (Application.Current as App).YDamping;
                            comingAppRect.WidthDamping = (Application.Current as App).WidthDamping;
                            comingAppRect.HeightDamping = (Application.Current as App).HeightDamping;
                            comingAppRect.RotationDamping = (Application.Current as App).RotationDamping;
                            comingAppRect.CornerRadiusDamping = (Application.Current as App).CornerRadiusDamping;
                            comingAppRect.OpacityDamping = (Application.Current as App).OpacityDamping;
                        }
                    }
                    //AWSwipeBar.Opacity = 1;
                    if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                    {
                        StartWindowSpringAnimation(2, comingAppRect);
                    }
                    else
                    {

                        AWABackStoryBoard.Begin();
                        AWAFrameOpacity.Begin();
                        RoundCornerPointerStBo.Begin();
                        RoundCornerTimer.Start();


                    }
                }

                RvX = RvY = RvScale = 0;
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
                AW2FrontIconRotate.Angle = AWFrontIconRotate.Angle;
                AW2BackIconRotate.Angle = AWBackIconRotate.Angle;
                AW2FrontIconRotate.CenterX = AWFrontIconRotate.CenterX;
                AW2FrontIconRotate.CenterY = AWFrontIconRotate.CenterY;
                AW2BackIconRotate.CenterX = AWBackIconRotate.CenterX;
                AW2BackIconRotate.CenterY = AWBackIconRotate.CenterY;
                AppWindow2Gesture.Visibility = Visibility.Visible;
                AppWindow2Gesture.Opacity = 0.001;
                //AppWindowGesture.Opacity = 0.001;
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

                AW2GBackKeyY2PowerEase.Power = AWGBackKeyY2PowerEase.Power;
                AW2AGstBackKeyX1PowerEase.Power = AWAGstBackKeyX1PowerEase.Power;
                AW2AGstBackKeyY1PowerEase.Power = AWAGstBackKeyY1PowerEase.Power;
                AW2AGstBackKeyX1.KeyTime = AWAGstBackKeyX1.KeyTime;
                AW2AGstBackKeyY1.KeyTime = AWAGstBackKeyY1.KeyTime;
                AW2AGstBackKeyX2.KeyTime = AWAGstBackKeyX2.KeyTime;
                AW2AGstBackKeyY2.KeyTime = AWAGstBackKeyY2.KeyTime;

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


                if((Application.Current as App).CurveStyle < SpringCurveStyleCount) //SpringAnimation
                {
                    AW2GTransform.X = AW2GTransform.Y = 0;
                    AW2GScale.ScaleX = AW2GScale.ScaleY = 1;
                }
                else if ((Application.Current as App).CurveStyle == SpringCurveStyleCount)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBack2StoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 1 + SpringCurveStyleCount)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBackStoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 2 + SpringCurveStyleCount)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBackStoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 3 + SpringCurveStyleCount)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBack2StoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
                else if ((Application.Current as App).CurveStyle == 4 + SpringCurveStyleCount)
                {
                    AW2GGestureFlyStoryBoard.Begin();
                    AW2AGestureBack2StoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
            }
            else
            {
                AW2FrontIconRotate.Angle = AWFrontIconRotate.Angle;
                AW2BackIconRotate.Angle = AWBackIconRotate.Angle;
                AW2FrontIconRotate.CenterX = AWFrontIconRotate.CenterX;
                AW2FrontIconRotate.CenterY = AWFrontIconRotate.CenterY;
                AW2BackIconRotate.CenterX = AWBackIconRotate.CenterX;
                AW2BackIconRotate.CenterY = AWBackIconRotate.CenterY;
                AppWindow2Gesture.Visibility = Visibility.Visible;
                AppWindow2Gesture.Opacity = 0.001;
                //AppWindowGesture.Opacity = 0.001;
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

                if ((Application.Current as App).CurveStyle < SpringCurveStyleCount) //SpringAnimation
                {

                }
                else
                {

                    AW2ABackStoryBoard.Begin();
                    RoundCornerPointer2StBo.Begin();
                }
            }
        }

        public int AppLauncher(string PackageName = "", int target = -1)
        {/*
            (Application.Current as App).MultiAppInfos[AppWindowMain_mIndex].AppFrame.Content = null;
            (Application.Current as App).MultiAppInfos[AppWindowMain_mIndex].AppFrame = null;
            (Application.Current as App).MultiAppInfos.RemoveAt(AppWindowMain_mIndex);
            AppWindowMain_mIndex = -1;*/

            if(PackageName == "" && target != -1)
            {
                PackageName = ((DesktopGrid.Children[target] as Grid)).Tag != null ? ((DesktopGrid.Children[target] as Grid)).Tag.ToString() : (DateTime.Now.ToString());
            }
            OnRunningAppInfo ToRunAppInfo = new OnRunningAppInfo { AppFrame = null, AppIconPos = target, AppPackageName = PackageName };
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
            if (ToRunAppIndex == -1)
            {
                ToRunAppInfo.AppFrame = new Frame();
                if (PackageName.Contains("com.android.settings"))
                {
                    ToRunAppInfo.AppFrame.Navigate(typeof(SettingsApp), null, new SuppressNavigationTransitionInfo());
                }
                else if (PackageName.Contains("com.android.camera"))
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

            int toShowAnimation = 1;
            while (AWMultiTaskGrid.Children.Count > 0)
            {
                var OnBackgroundingApp = (AWMultiTaskGrid.Children[0] as Frame);
                if (ToRunAppIndex != -1 && (AWMultiTaskGrid.Children[0] as Frame).Content.GetType() == (Application.Current as App).MultiAppInfos[ToRunAppIndex].AppFrame.Content.GetType())
                    toShowAnimation = 0;
                AWMultiTaskGrid.Children.RemoveAt(0);
            }
            AWMultiTaskGrid.Children.Add(ToRunAppIndex == -1 ? ToRunAppInfo.AppFrame : (Application.Current as App).MultiAppInfos[ToRunAppIndex].AppFrame);

            if (target == 8)
                target = -1;


            return (toShowAnimation == 1 || AppWindowState != 1) ? target : -4;
        }

        public int FindAppTarget(int index = -1)
        {
            if (((AWMultiTaskGrid.Children[0] as Frame).Content.GetType() == typeof(SettingsApp)))
            {
                return 1;
            }
            return index;
        }

        private void AppIcon_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) //图标按下
        {
            if (AppWindowState == 1)
            {
                return;
            }

            int target = DesktopGrid.Children.IndexOf(((sender as Button).Parent as Grid)) != -1 ? DesktopGrid.Children.IndexOf(((sender as Button).Parent as Grid)) : TouchIconGrid.Children.IndexOf(((sender as Button).Parent as Grid));
            target = AppLauncher("", target);

            if(target != -4)
                StartWindowAnimation(1, target);
            StartBackgroundAnimation(1);
            DesktopIconGridWhenClosingAppScale.ScaleX = 0;

        }

        private void DesktopGrid_Loaded(object sender, RoutedEventArgs e) // 放置桌面图标
        {
            GetCustomBackground();

            int DskIconSize = 500;
            DesktopGrid.ItemHeight = 0.16 * 1.4 * DskIconSize;
            DesktopGrid.ItemWidth = 0.16 * 1.4 * DskIconSize;
            DesktopGrid.Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.2 * DskIconSize, 0.16 * 0.8 * DskIconSize, 0.16 * 0.2 * DskIconSize, 0.16 * 0.8 * DskIconSize);

            TouchIconGrid.ItemHeight = 0.16 * 1.4 * DskIconSize;
            TouchIconGrid.ItemWidth = 0.16 * 1.4 * DskIconSize;
            TouchIconGrid.Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.2 * DskIconSize, 0.16 * 0.8 * DskIconSize, 0.16 * 0.2 * DskIconSize, 0.16 * 0.8 * DskIconSize);


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
                    ColumnSpan = 4,
                    RowSpan = 2
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


                //打断层
                var grid2 = new Grid
                {
                    BorderBrush = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    BorderThickness = new Thickness(IconInfo.ColumnSpan + IconInfo.RowSpan > 2 ? 1.0 : 0.5), //0.5是图标 1是卡片
                    Tag = IconInfo.Tag,
                    CornerRadius = new Windows.UI.Xaml.CornerRadius(DskIconSize * 100 / 1920 * 0.8),
                    Margin = new Windows.UI.Xaml.Thickness(0.16 * 0.12 * DskIconSize),

                };
                VariableSizedWrapGrid.SetColumnSpan(grid2, IconInfo.ColumnSpan);
                VariableSizedWrapGrid.SetRowSpan(grid2, IconInfo.RowSpan);

                if (IconInfo.ColumnSpan + IconInfo.RowSpan > 2)
                {
                }
                else
                {
                }

                var button2 = new Button
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(-4),
                    Opacity = 0.5,
                };
                button2.Click += AppIcon_Click;
                grid2.Children.Add(button2);
                TouchIconGrid.Children.Add(grid2);
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


            AWARotate.CenterX = ActualWidth * 0.5;
            AWARotate.CenterY = ActualHeight * 0.5;
            AW2ARotate.CenterX = ActualWidth * 0.5;
            AW2ARotate.CenterY = ActualHeight * 0.5;

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

            if(ActualWidth != 0 && ActualWidth <= 720)
            {
                DesktopGrid.MaximumRowsOrColumns = 4;
                TouchIconGrid.MaximumRowsOrColumns = 4;
            }
            else
            {
                DesktopGrid.MaximumRowsOrColumns = -1;
                TouchIconGrid.MaximumRowsOrColumns = -1;
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

        }

        public void GstBut_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            if((e == null || e.HoldingState == HoldingState.Started))
            {
                if (AppWindowState == 2 || AppWindowState == 0)
                {
                    return;
                }

                StartWindowAnimation(0, AppWindowMain_Target);
                if((Application.Current as App).CurveStyle >= SpringCurveStyleCount) AWGGestureFillStoryBoard.Begin();
                StartBackgroundAnimation(0);
                SetSwipeBarColor(0);
                return;
            }
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
            if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
            {
                return;
            }
            if(AWAScale.ScaleX != 1.0)
                AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(RoundCornerPointerTransform.X);
            else if(AWAScale.ScaleX == 1.0 && AWGScale.ScaleX == 1.0)
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
                if(AWAScale.ScaleX == 1.0 && AWGScale.ScaleX == 1.0)
                    AppHeightAnimation.CornerRadius = new Windows.UI.Xaml.CornerRadius(1);
                RoundCornerTimer.Stop();
                if (AWCardFrame.Content == null || AWCardFrame.Content.GetType() != typeof(DesktopCard))
                    AWCardFrame.Navigate(typeof(DesktopCard), null, new SuppressNavigationTransitionInfo());
                (AWCardFrame.Content as DesktopCard).GetCardInfo = "null";
                AWBackgIcon.Opacity = 0;
                AWFrontIcon.Opacity = 0;
                if(AppWindowMain_Target >= 0 && AWAScale.ScaleX == 1.0 && AWATransform.X == 0 && AWATransform.Y == 0)
                    (DesktopGrid.Children[AppWindowMain_Target] as Grid).Opacity = 1;
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
                DesktopIconGridWhenClosingAppScale.ScaleX = 0;
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
            if(AppWindowState == 2 && AppWindowMain_Target != -1 && DesktopGridScrollViewer.VerticalOffset != DesktopGridScrollViewer2.VerticalOffset)
                StartWindowAnimation(-1);
            if((sender as ScrollViewer).Name == "DesktopGridScrollViewer2" && DesktopGridScrollViewer.VerticalOffset != DesktopGridScrollViewer2.VerticalOffset)
            {
                DesktopGridScrollViewer.ScrollToVerticalOffset(DesktopGridScrollViewer2.VerticalOffset);
            }

            if (AppWindow2_Target >= 0)
            {
                AppWindow2Gesture.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                DesktopIconGridWhenClosingAppScale.ScaleX = 0;
                (DesktopGrid.Children[AppWindow2_Target] as Grid).Opacity = 1;
            }
            AppWindow2Gesture.Visibility = Visibility.Collapsed;
        }

        double FarPoint = 0.2;
        private void GstBut_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (AppWindowState == 2)
            {
                return;
            }

            FarPoint = 0.2;
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
                AWGTransform.X = (MouseX - (AWGScale.ScaleX) * (MouseDownX - 0.5 * ActualWidth) - 0.5 * ActualWidth) * AWAScale.ScaleY;
                AWGTransform.Y = (-0.5 * ActualHeight + FarPoint * ActualHeight + (0.5 - FarPoint) * mH) * AWAScale.ScaleY;
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
                        double dH = ((ActualHeight + (MouseY - MouseDownY) - Math.Pow(e.Velocities.Linear.Y, 1 / 5) * 1 * (Application.Current as App).FlyFar - ActualHeight * FarPoint) * (ActualHeight / (ActualHeight - ActualHeight * FarPoint)));
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
                    if ((Application.Current as App).CurveStyle == 4 + SpringCurveStyleCount)
                    {
                        FarPoint = -0;
                    }
                    else
                    {
                        FarPoint = 0.2;
                    }
                    AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = AWGScale.ScaleX * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH) * (dH / mH);
                        if (AWGBackKeyScaleX1.Value < 0.01 && AppWindowMain_Target != -1)
                        {
                            AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = 0.01;
                        }
                    }
                    catch
                    {
                        AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = AWGScale.ScaleX * 1;
                        if (AWGBackKeyScaleX1.Value < 0.01 && AppWindowMain_Target != -1)
                        {
                            AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = 0.01;
                        }
                    }

                AWGBackKeyScaleX2.Value = AWGBackKeyScaleY2.Value = 1.0;


                if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                {

                }
                else if ((Application.Current as App).CurveStyle == SpringCurveStyleCount)
                {
                    AWGBackKeyY2.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AWGBackKeyScaleY1.Value * (0.5 - FarPoint) * AWAScale.ScaleY;
                    try
                    {
                        AWGBackKeyX2.Value = AWAScale.ScaleY * (AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 0.8) * (Application.Current as App).FlyFar * 8);
                    }
                    catch { }
                    AWGBackKeyY2PowerEase.Power = 50;
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                }
                else if ((Application.Current as App).CurveStyle == 1 + SpringCurveStyleCount)
                {
                    AWGBackKeyY2.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AWGBackKeyScaleY1.Value * (0.5 - FarPoint) * AWAScale.ScaleY;
                    try
                    {
                        AWGBackKeyX2.Value = AWAScale.ScaleY * (AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 0.8) * (Application.Current as App).FlyFar * 8);
                    }
                    catch { }

                    AWGBackKeyY2PowerEase.Power = 50;
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.15 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));

                    AWAGstBackKeyX1PowerEase.Power = 3;
                    AWAGstBackKeyY1PowerEase.Power = 3;
                    AWAGstBackKeyX1.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                    AWAGstBackKeyY1.KeyTime = TimeSpan.FromSeconds(0.6 * (Application.Current as App).TransitionDurationTime);
                }
                else if ((Application.Current as App).CurveStyle == 2 + SpringCurveStyleCount)
                {
                    AWGBackKeyY2.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AWGBackKeyScaleY1.Value * (0.5 - FarPoint) * AWAScale.ScaleY;
                    try
                    {
                        AWGBackKeyX2.Value = AWAScale.ScaleY * (AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 0.8) * (Application.Current as App).FlyFar * 8);
                    }
                    catch { }
                    AWGBackKeyY2PowerEase.Power = 30;
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.54 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.54 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.05 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.05 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));

                    AWAGstBackKeyX1PowerEase.Power = 5;
                    AWAGstBackKeyY1PowerEase.Power = 5;
                    AWAGstBackKeyX1.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWAGstBackKeyY1.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                }
                else if ((Application.Current as App).CurveStyle == 3 + SpringCurveStyleCount)
                {
                    AWGBackKeyScaleX1.Value = AWGBackKeyScaleY1.Value = AWGScale.ScaleX * 1;
                    AWGBackKeyY2.Value = AWAScale.ScaleY * (AWGTransform.Y + e.Velocities.Linear.Y / Math.Abs(e.Velocities.Linear.Y) * Math.Pow(Math.Abs(e.Velocities.Linear.Y), 0.8) * (Application.Current as App).FlyFar * 8);
                    try
                    {
                        AWGBackKeyX2.Value = AWAScale.ScaleY * (AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 0.8) * (Application.Current as App).FlyFar * 8);
                    }
                    catch { }
                    AWGBackKeyY2PowerEase.Power = 20;
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.00 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.00 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));

                }
                else if ((Application.Current as App).CurveStyle == 4 + SpringCurveStyleCount)
                {
                    AWGBackKeyY2.Value = 0 - ActualHeight * (0.5 - FarPoint) + ActualHeight * AWGBackKeyScaleY1.Value * (0.5 - FarPoint) * AWAScale.ScaleY;
                    try
                    {
                        AWGBackKeyX2.Value = AWAScale.ScaleY * (AWGTransform.X + e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X) * Math.Pow(Math.Abs(e.Velocities.Linear.X), 0.8) * (Application.Current as App).FlyFar * 8);
                    }
                    catch { }
                    AWGBackKeyY2PowerEase.Power = 20;
                    AWGBackKeyX2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyY2.KeyTime = TimeSpan.FromSeconds(0.75 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleY2.KeyTime = TimeSpan.FromSeconds(0.65 * (Application.Current as App).TransitionDurationTime);
                    AWGBackKeyScaleX1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));
                    AWGBackKeyScaleY1.KeyTime = TimeSpan.FromSeconds(0.08 * (Application.Current as App).TransitionDurationTime * Math.Pow((Application.Current as App).FlyFar, 1 / 6));

                }

                if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                {
                    RvX = e.Velocities.Linear.X / 10000 / 20.0 * (Application.Current as App).FlyFar / Math.Pow((Application.Current as App).TransitionDurationTime, 0.5);
                    RvY = (e.Velocities.Linear.Y / 10000) / (1 - FarPoint) * (0.5 - FarPoint) / 30.0 * (Application.Current as App).FlyFar / Math.Pow((Application.Current as App).TransitionDurationTime,0.5);
                    RvScale = (e.Velocities.Linear.Y / 10000) * (1 - FarPoint) / (ActualHeight * AWGScale.ScaleY) / 30.0 * (Application.Current as App).FlyFar / Math.Pow((Application.Current as App).TransitionDurationTime, 0.5);

                    StartWindowAnimation(0, AppWindowMain_Target);
                    AWGScale.ScaleX = AWGScale.ScaleY = 1;
                    AWGTransform.X = AWGTransform.Y = 0;
                    StartBackgroundAnimation(0);
                }
                else
                {

                    AWGGestureFlyStoryBoard.Begin();
                    StartWindowAnimation(0, AppWindowMain_Target);
                    StartBackgroundAnimation(0);
                }

                if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                {
                    
                }
                else if ((Application.Current as App).CurveStyle == SpringCurveStyleCount)
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
                else if ((Application.Current as App).CurveStyle == 1 + SpringCurveStyleCount)
                {
                    double distance = Math.Sqrt(Math.Pow((AWABackKeyY.Value - AWGBackKeyY2.Value), 2) + Math.Pow((AWABackKeyX.Value - AWGBackKeyX2.Value), 2));
                    double b = 6;// ((Application.Current as App).BounceRadius);
                    double BackEaseV = (1 * Math.Sqrt(Math.Pow(e.Velocities.Linear.Y, 2) + Math.Pow(e.Velocities.Linear.X, 2))) * 10;
                    double BackEaseRound = (b * (-1 / (Math.Abs(BackEaseV - 0) / b + 1) + 1)) + 0;
                    AWAGstBackKeyX1.Value = (AWABackKeyX.Value - AWGBackKeyX2.Value) * (distance * 1.02) / distance;
                    AWAGstBackKeyY1.Value = (AWABackKeyY.Value - AWGBackKeyY2.Value) * (distance * 1.02/* + BackEaseRound*/) / distance;
                    AWAGstBackKeyX2.Value = AWABackKeyX.Value - AWGBackKeyX2.Value;
                    AWAGstBackKeyY2.Value = AWABackKeyY.Value - AWGBackKeyY2.Value;
                    AWAGstBackKeyH2.Value = AWABackKeyH.Value;
                    AWAGstBackKeyW2.Value = AWABackKeyW.Value;
                    if (AppWindowMain_Target >= 0 && b > 4 && (DesktopGrid.Children[AppWindowMain_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppWindowMain_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 15.4 / 16.0;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 15.4 / 16.0;
                    }
                    else if (AppWindowMain_Target >= 0) //卡片
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 15.6 / 16.0;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 15.6 / 16.0;
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
                else if ((Application.Current as App).CurveStyle == 2 + SpringCurveStyleCount)
                {
                    double distance = Math.Sqrt(Math.Pow((AWABackKeyY.Value - AWGBackKeyY2.Value), 2) + Math.Pow((AWABackKeyX.Value - AWGBackKeyX2.Value), 2));
                    double b = 0;// ((Application.Current as App).BounceRadius);
                    double BackEaseV = (1 * Math.Sqrt(Math.Pow(e.Velocities.Linear.Y, 2) + Math.Pow(e.Velocities.Linear.X, 2))) * 10;
                    double BackEaseRound = (b * (-1 / (Math.Abs(BackEaseV - 0) / b + 1) + 1)) + 0;
                    AWAGstBackKeyX1.Value = (AWABackKeyX.Value - AWGBackKeyX2.Value) * (distance + 0*BackEaseRound) / distance;
                    AWAGstBackKeyY1.Value = (AWABackKeyY.Value - AWGBackKeyY2.Value) * (distance + 0*BackEaseRound) / distance;
                    AWAGstBackKeyX2.Value = AWABackKeyX.Value - AWGBackKeyX2.Value;
                    AWAGstBackKeyY2.Value = AWABackKeyY.Value - AWGBackKeyY2.Value;
                    AWAGstBackKeyH2.Value = AWABackKeyH.Value;
                    AWAGstBackKeyW2.Value = AWABackKeyW.Value;
                    if (AppWindowMain_Target >= 0 && b > 4 && (DesktopGrid.Children[AppWindowMain_Target].GetType() == typeof(Grid) && (DesktopGrid.Children[AppWindowMain_Target] as Grid).BorderThickness == new Windows.UI.Xaml.Thickness(0.5)))
                    {
                        AWAGstBackKeyScaleX1.Value = AWABackKeyScaleX.Value * 16 / 16.0;
                        AWAGstBackKeyScaleY1.Value = AWABackKeyScaleY.Value * 16 / 16.0;
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
                else if ((Application.Current as App).CurveStyle == 3 + SpringCurveStyleCount)
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
                else if ((Application.Current as App).CurveStyle == 4 + SpringCurveStyleCount)
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

                    AWABackStoryBoard.Stop();

                StartSideAnimation(1);
            }
            else if (e.Velocities.Linear.Y >= -2 || true)
            {
                GstBut.Visibility = Visibility.Visible;
                AWGScale.CenterX = ActualWidth * 0.5;
                AWGScale.CenterY = ActualHeight * 0.5;
                if ((Application.Current as App).CurveStyle < SpringCurveStyleCount)
                {
                    AppAnimationRectProperties comingAppRect = new AppAnimationRectProperties();
                    comingAppRect.ToHeight = ActualHeight;
                    comingAppRect.ToWidth = ActualWidth;
                    comingAppRect.ToX = 0;
                    comingAppRect.ToY = 0;
                    comingAppRect.ToScale = 1.0;
                    comingAppRect.ToOpacity = 1;
                    comingAppRect.ToCornerRadius = 500 * (Application.Current as App).ScreenCornerRadius;
                    comingAppRect.XDamping = 0.8;
                    comingAppRect.YDamping = 0.8;
                    comingAppRect.ScaleDamping = 0.8;
                    comingAppRect.VX = e.Velocities.Linear.X / 10000 / 20.0 * (Application.Current as App).FlyFar;
                    comingAppRect.VY = (e.Velocities.Linear.Y / 10000) / (1 - FarPoint) * (0.5 - FarPoint) / 30.0 * (Application.Current as App).FlyFar;
                    comingAppRect.VScale = (e.Velocities.Linear.Y / 10000) * (1 - FarPoint) / (ActualHeight * AWGScale.ScaleY) / 30.0 * (Application.Current as App).FlyFar;
                    
                    comingAppRect.durationTick = (long)(0.5 * 10000000);
                    StartWindowSpringAnimation(1, comingAppRect);
                    AWGTransform.X = AWGTransform.Y = 0;
                    AWGScale.ScaleX = AWGScale.ScaleY = 1;
                }
                else
                {
                    AWGGestureFillStoryBoard.Begin();
                }
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
