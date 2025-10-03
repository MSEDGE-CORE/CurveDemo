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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CurveDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DesktopCard : Page
    {
        string CardInfo = "";
        double CardWidth = 1;
        double CardHeight = 1;
        public string GetCardInfo 
        { 
            get { return CardInfo; } 
            set 
            { 
                CardInfo = value;
                ShowDesktopCard(); 
            } 
        }
        public Image GetImage
        {
            get { return BackgroundImage; }
            set
            {
                BackgroundImage.Source = value.Source;
            }
        }
        DispatcherTimer Timer;

        public DesktopCard()
        {
            this.InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Timer.Tick += ClockUpdate_Timer;
        }

        public async void ShowDesktopCard()
        {
            if(CardInfo == "com.android.deskclock.deskclock")
            {
                ClockText.Visibility = Visibility.Visible;
                BackgroundImage.Visibility = Visibility.Collapsed;
                Timer.Stop();
                Timer.Start();

                
            }
            else if(CardInfo == "null")
            {
                Timer.Stop();
                BackgroundImage.Visibility = Visibility.Collapsed;
                ClockText.Visibility = Visibility.Collapsed;
            }
            else if(CardInfo == "Pic")
            {
                Timer.Stop();
                ClockText.Visibility = Visibility.Collapsed;
                BackgroundImage.Visibility = Visibility.Visible;
                try
                {
                    /*if(BackgroundImage.Source != new BitmapImage(new Uri(CardInfo)))
                        BackgroundImage.Source = new BitmapImage(new Uri(CardInfo));*/
                }
                catch { }
            }
        }

        private void ClockUpdate_Timer(object? sender, object e)
        {
            ClockText.Text = DateTime.Now.ToString("H:mm");

            //Trace.WriteLine((ActualWidth, ActualHeight));
        }

        private async void ClockText_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth != 0 && CardInfo == "com.android.deskclock.deskclock")
            {
                ClockScale.CenterX = ClockGrid.ActualWidth * 0.5;
                ClockScale.CenterY = ClockGrid.ActualHeight * 0.5;
                if (ActualWidth > 0 && (ActualWidth / ActualHeight >= 428 / 203.0))//宽
                {
                    ClockScale.ScaleX = ClockScale.ScaleY = ActualHeight / 203.0;
                }
                else
                {
                    ClockScale.ScaleX = ClockScale.ScaleY = ActualWidth / 428.0;
                }

                CardHeight = ClockGrid.ActualHeight;
                CardWidth = ClockGrid.ActualWidth;
                ClockText.FontSize = ((CardWidth - 80) / 5.0 * 2.0) > 0 ? (CardWidth - 80) / 5.0 * 2.0 : 1;
                ClockText.Margin = new Thickness(0, 0, 0, ((CardWidth - 80) / 5.0 * 0.15) > 0 ? (CardWidth - 80) / 5.0 * 0.15 : 1);
                ClockText.Text = DateTime.Now.ToString("H:mm"); 
            }
        }
    }
}
