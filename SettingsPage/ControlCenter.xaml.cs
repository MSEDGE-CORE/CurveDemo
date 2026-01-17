using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace CurveDemo.SettingsPage
{

    public sealed partial class ControlCenter : Page
    {
        public ControlCenter()
        {
            this.InitializeComponent();
            CombineControlCenterWhenWideSwitch.IsOn = (Application.Current as App).CombineControlCenterWhenWide;
            NotifiPnelAlignmentSelection.SelectedIndex = (Application.Current as App).NotificationCenterAlignment <= 1 ? (Application.Current as App).NotificationCenterAlignment : 0;
        }

        private void NotifiPnelAlignmentSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (Application.Current as App).NotificationCenterAlignment = NotifiPnelAlignmentSelection.SelectedIndex;
            (Application.Current as App).LocalSettings.Values["NotificationCenterAlignment"] = (Application.Current as App).NotificationCenterAlignment;

        }

        private void CombineControlCenterWhenWideSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).CombineControlCenterWhenWide = CombineControlCenterWhenWideSwitch.IsOn;
            (Application.Current as App).LocalSettings.Values["CombineControlCenterWhenWide"] = (Application.Current as App).CombineControlCenterWhenWide;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void TransitionPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage.Transition), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
