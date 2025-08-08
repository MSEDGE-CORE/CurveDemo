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

            BgBlur_Switch.IsOn = (Application.Current as App).EnableBgBlur == 1 ? true : false;
            ScrCornerRadius_Switch.IsOn = (Application.Current as App).ScreenCornerRadius == 100.0 / 1920 ? true : false;
            if((Application.Current as App).TransitionDurationTime < 0.1)
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

            Trace.WriteLine((Application.Current as App).FlyFar);
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

            isLoaded = true;
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
                await Task.Delay(100);
                (((MP.Content as Grid).Children[0] as Frame).Content as MainPage).StartBgAnimation(1);
                (((MP.Content as Grid).Children[0] as Frame).Content as MainPage).StartRectAnimation(1, -1);
                (((MP.Content as Grid).Children[0] as Frame).Content as MainPage).AppRect_Target = 0;
                //(((((((((((MP.Content as Grid).Children[0] as Frame).Content as MainPage).Content as Grid).Children[1] as Grid).Children[4] as Grid).Children[0] as Grid).Children[0] as Grid).Children[0] as Grid).Children[0] as Grid).Children[2] as Frame).Navigate(typeof(SettingsPage.Home), null, new SuppressNavigationTransitionInfo());
                //(((((((((((MP.Content as Grid).Children[0] as Frame).Content as MainPage).Content as Grid).Children[1] as Grid).Children[4] as Grid).Children[0] as Grid).Children[0] as Grid).Children[0] as Grid).Children[0] as Grid).Children[2] as Frame).Navigate(typeof(SettingsPage.Transition), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                (((MP.Content as Grid).Children[0] as Frame).Content as MainPage).GetAppFrame.Navigate(typeof(BlankPage), null, new SuppressNavigationTransitionInfo());
                await Task.Delay((int)(0.7 * (Application.Current as App).TransitionDurationTime * 1100));
                (((MP.Content as Grid).Children[0] as Frame).Content as MainPage).GetAppFrame.Navigate(typeof(SettingsApp), null, new SuppressNavigationTransitionInfo());
                (((((((MP.Content as Grid).Children[0] as Frame).Content as MainPage).GetAppFrame.Content as SettingsApp).Content as Grid).Children[0] as Grid).Children[0] as Frame).Navigate(typeof(SettingsPage.Transition), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                
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
    }
}
