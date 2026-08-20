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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CurveDemo.SettingsPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Transition : Page
    {
        

        public static SystemUI MP
        {
            get { return (Window.Current.Content as Frame)?.Content as SystemUI; }
        }

        bool isLoaded = false;
        public Transition()
        {
            this.InitializeComponent();

            this.Slider_SpringSettings_Duration.Value = (Application.Current as App).DurationTime;
            this.Slider_SpringSettings_XVelocity.Value = (Application.Current as App).XVelocity;
            this.Slider_SpringSettings_YVelocity.Value = (Application.Current as App).YVelocity;
            this.Slider_SpringSettings_ScaleVelocity.Value = (Application.Current as App).ScaleVelocity;
            this.Slider_SpringSettings_XDamping.Value = (Application.Current as App).XDamping;
            this.Slider_SpringSettings_YDamping.Value = (Application.Current as App).YDamping;
            this.Slider_SpringSettings_WidthDamping.Value = (Application.Current as App).WidthDamping;
            this.Slider_SpringSettings_ScaleDamping.Value = (Application.Current as App).ScaleDamping;
            this.Slider_SpringSettings_OpacityDamping.Value = (Application.Current as App).OpacityDamping;
            this.Slider_SpringSettings_RotationDamping.Value = (Application.Current as App).RotationDamping;
            this.Slider_SpringSettings_OpacityBeginTime.Value = (Application.Current as App).OpacityBeginTime;
            this.Slider_SpringSettings_OpacityDurationTime.Value = (Application.Current as App).OpacityDurationTime;
            this.Slider_SpringSettings_CornerRadiusDamping.Value = (Application.Current as App).CornerRadiusDamping;

            BgBlur_Switch.IsOn = (Application.Current as App).EnableBgBlur == 1 ? true : false;
            BgScale_Switch.IsOn = (Application.Current as App).EnableBgScale == 1 ? true : false;
            ScrCornerRadius_Switch.IsOn = (Application.Current as App).ScreenCornerRadius == 100.0 / 1920 ? true : false;
            SideWindowAnimation_Switch.IsOn = (Application.Current as App).EnableSideWindowAnimation == 1 ? true : false;
            CurveStyleSelection.SelectedIndex = (Application.Current as App).CurveStyle <= 6 ? (Application.Current as App).CurveStyle : 0;
            CtrlPnCurveSelection.SelectedIndex = (Application.Current as App).CtrPnelCurveStyle <= 1 ? (Application.Current as App).CtrPnelCurveStyle : 0;
            if ((Application.Current as App).TransitionDurationTime < 0.1)
            {
                TimeDurSelection.SelectedIndex = 0;
            }
            else if((Application.Current as App).TransitionDurationTime == 0.5)
            {
                TimeDurSelection.SelectedIndex = 1;
            }
            else if((Application.Current as App).TransitionDurationTime == 1.0)
            {
                TimeDurSelection.SelectedIndex = 2;
            }
            else if ((Application.Current as App).TransitionDurationTime == 1.5)
            {
                TimeDurSelection.SelectedIndex = 3;
            }
            else if ((Application.Current as App).TransitionDurationTime == 2.0)
            {
                TimeDurSelection.SelectedIndex = 4;
            }
            else if ((Application.Current as App).TransitionDurationTime == 5.0)
            {
                TimeDurSelection.SelectedIndex = 5;
            }
            else if ((Application.Current as App).TransitionDurationTime == 10.0)
            {
                TimeDurSelection.SelectedIndex = 6;
            }
            else
            {
                TimeDurSelection.SelectedIndex = -1;
            }

            //Trace.WriteLine((Application.Current as App).FlyFar);
            if ((Application.Current as App).FlyFar == 0)
            {
                FlyFarSelection.SelectedIndex = 0;
            }
            else if ((Application.Current as App).FlyFar == 1)
            {
                FlyFarSelection.SelectedIndex = 1;
            }
            else if ((Application.Current as App).FlyFar == 3)
            {
                FlyFarSelection.SelectedIndex = 2;
            }
            else if ((Application.Current as App).FlyFar == 5)
            {
                FlyFarSelection.SelectedIndex = 3;
            }
            else if ((Application.Current as App).FlyFar == 6)
            {
                FlyFarSelection.SelectedIndex = 4;
            }
            else if ((Application.Current as App).FlyFar == 8)
            {
                FlyFarSelection.SelectedIndex = 5;
            }
            else if ((Application.Current as App).FlyFar == 10)
            {
                FlyFarSelection.SelectedIndex = 6;
            }
            else
            {
                FlyFarSelection.SelectedIndex = -1;
            }

            if ((Application.Current as App).BounceRadius == 0)
            {
                BounceRadiusSelection.SelectedIndex = 0;
            }
            else if ((Application.Current as App).BounceRadius == 2)
            {
                BounceRadiusSelection.SelectedIndex = 1;
            }
            else if ((Application.Current as App).BounceRadius == 4)
            {
                BounceRadiusSelection.SelectedIndex = 2;
            }
            else if ((Application.Current as App).BounceRadius == 6)
            {
                BounceRadiusSelection.SelectedIndex = 3;
            }
            else if ((Application.Current as App).BounceRadius == 8)
            {
                BounceRadiusSelection.SelectedIndex = 4;
            }
            else if ((Application.Current as App).BounceRadius == 10)
            {
                BounceRadiusSelection.SelectedIndex = 5;
            }
            else if ((Application.Current as App).BounceRadius == 12)
            {
                BounceRadiusSelection.SelectedIndex = 6;
            }
            else
            {
                BounceRadiusSelection.SelectedIndex = -1;
            }

            isLoaded = true;

            StackPanel_SpringSettings.Visibility = CurveStyleSelection.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private async void TimeDurSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeDurSelection.SelectedIndex == 0)
            {
                (Application.Current as App).TransitionDurationTime = 0.01;
            }
            else if (TimeDurSelection.SelectedIndex == 1)
            {
                (Application.Current as App).TransitionDurationTime = 0.5;
            }
            else if (TimeDurSelection.SelectedIndex == 2)
            {
                (Application.Current as App).TransitionDurationTime = 1.0;
            }
            else if (TimeDurSelection.SelectedIndex == 3)
            {
                (Application.Current as App).TransitionDurationTime = 1.5;
            }
            else if (TimeDurSelection.SelectedIndex == 4)
            {
                (Application.Current as App).TransitionDurationTime = 2.0;
            }
            else if (TimeDurSelection.SelectedIndex == 5)
            {
                (Application.Current as App).TransitionDurationTime = 5.0;
            }
            else if (TimeDurSelection.SelectedIndex == 6)
            {
                (Application.Current as App).TransitionDurationTime = 10.0;
            }

            (Application.Current as App).LocalSettings.Values["TransitionDurationTime"] = (Application.Current as App).TransitionDurationTime;

            if ( isLoaded)
            {
               ((MP.Content as Grid).Children[0] as Frame).Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());
                ((MP.Content as Grid).Children[1] as Frame).Navigate(typeof(QuickControlPanel), null, new SuppressNavigationTransitionInfo());

            }
        }

        private void BgBlur_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).EnableBgBlur = (bool)((sender as ToggleSwitch).IsOn) ? 1 : 0;
            (Application.Current as App).LocalSettings.Values["EnableBgBlur"] = (Application.Current as App).EnableBgBlur;
        }

        private void ScrCornerRadius_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).ScreenCornerRadius = (bool)((sender as ToggleSwitch).IsOn) ? 100.0 / 1920 : 0.000000001;
            (Application.Current as App).LocalSettings.Values["EnableCornerRadius"] = (bool)((sender as ToggleSwitch).IsOn);
        }

        private void FlyFarSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FlyFarSelection.SelectedIndex == 0)
            {
                (Application.Current as App).FlyFar = 0;
            }
            else if (FlyFarSelection.SelectedIndex == 1)
            {
                (Application.Current as App).FlyFar = 1;
            }
            else if (FlyFarSelection.SelectedIndex == 2)
            {
                (Application.Current as App).FlyFar = 3;
            }
            else if (FlyFarSelection.SelectedIndex == 3)
            {
                (Application.Current as App).FlyFar = 5;
            }
            else if (FlyFarSelection.SelectedIndex == 4)
            {
                (Application.Current as App).FlyFar = 6;
            }
            else if (FlyFarSelection.SelectedIndex == 5)
            {
                (Application.Current as App).FlyFar = 8;
            }
            else if (FlyFarSelection.SelectedIndex == 6)
            {
                (Application.Current as App).FlyFar = 10;
            }

            (Application.Current as App).LocalSettings.Values["FlyFar"] = (Application.Current as App).FlyFar;
        }

        private void BounceRadiusSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BounceRadiusSelection.SelectedIndex == 0)
            {
                (Application.Current as App).BounceRadius = 0;
            }
            else if (BounceRadiusSelection.SelectedIndex == 1)
            {
                (Application.Current as App).BounceRadius = 2;
            }
            else if (BounceRadiusSelection.SelectedIndex == 2)
            {
                (Application.Current as App).BounceRadius = 4;
            }
            else if (BounceRadiusSelection.SelectedIndex == 3)
            {
                (Application.Current as App).BounceRadius = 6;
            }
            else if (BounceRadiusSelection.SelectedIndex == 4)
            {
                (Application.Current as App).BounceRadius = 8;
            }
            else if (BounceRadiusSelection.SelectedIndex == 5)
            {
                (Application.Current as App).BounceRadius = 10;
            }
            else if (BounceRadiusSelection.SelectedIndex == 6)
            {
                (Application.Current as App).BounceRadius = 12;
            }

            (Application.Current as App).LocalSettings.Values["BounceRadius"] = (Application.Current as App).BounceRadius;
        }

        private void SideWindowAnimation_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).EnableSideWindowAnimation = (bool)((sender as ToggleSwitch).IsOn) ? 1:0;
            (Application.Current as App).LocalSettings.Values["EnableSideWindowAnimation"] = (bool)((sender as ToggleSwitch).IsOn) ? 1 : 0;
        }

        private void CurveStyleSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            StackPanel_SpringSettings.Visibility = CurveStyleSelection.SelectedIndex == 2 ? Visibility.Visible : Visibility.Collapsed;


            (Application.Current as App).CurveStyle = (sender as ComboBox).SelectedIndex;
            (Application.Current as App).LocalSettings.Values["CurveStyle"] = (Application.Current as App).CurveStyle;
            /*
            else if ((Application.Current as App).CurveStyle == 1)
            {
                //GridBounceRadius.Visibility = Visibility.Visible;
            }*/
        }

        private void BgScale_Switch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).EnableBgScale = (bool)((sender as ToggleSwitch).IsOn) ? 1 : 0;
            (Application.Current as App).LocalSettings.Values["EnableBgScale"] = (Application.Current as App).EnableBgScale;
        }

        private void CtrlPnCurveSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            (Application.Current as App).CtrPnelCurveStyle = (sender as ComboBox).SelectedIndex;
            (Application.Current as App).LocalSettings.Values["CtrPnelCurveStyle"] = (Application.Current as App).CtrPnelCurveStyle;
            
        }

        private void Slider_SpringSettings_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (isLoaded)
            {

            }
            else return;

            (Application.Current as App).DurationTime = this.Slider_SpringSettings_Duration.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_DurationTime"] = (Application.Current as App).DurationTime;

            (Application.Current as App).XVelocity = this.Slider_SpringSettings_XVelocity.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_XVelocity"] = (Application.Current as App).XVelocity;

            (Application.Current as App).YVelocity = this.Slider_SpringSettings_YVelocity.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_YVelocity"] = (Application.Current as App).YVelocity;
            
            (Application.Current as App).ScaleVelocity = this.Slider_SpringSettings_ScaleVelocity.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_ScaleVelocity"] = (Application.Current as App).ScaleVelocity;


            (Application.Current as App).XDamping = this.Slider_SpringSettings_XDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_XDamping"] = (Application.Current as App).XDamping;

            (Application.Current as App).YDamping = this.Slider_SpringSettings_YDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_YDamping"] = (Application.Current as App).YDamping;

            (Application.Current as App).WidthDamping = this.Slider_SpringSettings_WidthDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_WidthDamping"] = (Application.Current as App).WidthDamping;

            (Application.Current as App).HeightDamping = this.Slider_SpringSettings_WidthDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_HeightDamping"] = (Application.Current as App).HeightDamping;

            (Application.Current as App).ScaleDamping = this.Slider_SpringSettings_ScaleDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_ScaleDamping"] = (Application.Current as App).ScaleDamping;

            (Application.Current as App).OpacityDamping = this.Slider_SpringSettings_OpacityDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_OpacityDamping"] = (Application.Current as App).OpacityDamping;

            (Application.Current as App).RotationDamping = this.Slider_SpringSettings_RotationDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_RotationDamping"] = (Application.Current as App).RotationDamping;

            (Application.Current as App).OpacityBeginTime = this.Slider_SpringSettings_OpacityBeginTime.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_OpacityBeginTime"] = (Application.Current as App).OpacityBeginTime;

            (Application.Current as App).OpacityDurationTime = this.Slider_SpringSettings_OpacityDurationTime.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_OpacityDurationTime"] = (Application.Current as App).OpacityDurationTime;
        
            (Application.Current as App).CornerRadiusDamping = this.Slider_SpringSettings_CornerRadiusDamping.Value;
            (Application.Current as App).LocalSettings.Values["SpringSettings_CornerRadiusDamping"] = (Application.Current as App).CornerRadiusDamping;
        }

        private void SpringSettings_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            


            this.Slider_SpringSettings_Duration.Value = 1.0;
            this.Slider_SpringSettings_XVelocity.Value = 1.0;
            this.Slider_SpringSettings_YVelocity.Value = 1.0;
            this.Slider_SpringSettings_ScaleVelocity.Value = 1.0;
            this.Slider_SpringSettings_XDamping.Value = 0.82;
            this.Slider_SpringSettings_YDamping.Value = 0.82;
            this.Slider_SpringSettings_WidthDamping.Value = 0.9;
            this.Slider_SpringSettings_ScaleDamping.Value = 0.9;
            this.Slider_SpringSettings_OpacityDamping.Value = 1.0;
            this.Slider_SpringSettings_RotationDamping.Value = 0.9;
            this.Slider_SpringSettings_OpacityBeginTime.Value = 0.1;
            this.Slider_SpringSettings_OpacityDurationTime.Value = 0.3;
            this.Slider_SpringSettings_CornerRadiusDamping.Value = 1.0;
        }
    }
}
