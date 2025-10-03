using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CurveDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsApp : Page
    {
        public SettingsApp()
        {
            this.InitializeComponent();
            FramePage.Navigate(typeof(SettingsPage.Home), null, new SuppressNavigationTransitionInfo());
        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {/*
            PageOutline.Width = 420;
            PageOutline.Height = 420.0 * (ActualHeight / ActualWidth);
            PageScale.ScaleX = PageScale.ScaleY = ActualWidth / 420.0;
            PageScale.CenterX = PageScale.CenterY = 0;*/
        }
    }
}
