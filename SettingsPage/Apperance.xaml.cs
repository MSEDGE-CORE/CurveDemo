using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CurveDemo.SettingsPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Apperance : Page
    {
        public static SystemUI MP
        {
            get { return (Window.Current.Content as Frame)?.Content as SystemUI; }
        }

        public Apperance()
        {
            this.InitializeComponent();
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
            SPanel.Width = (ActualWidth >= 800) ? 720 : ActualWidth - 80;
        }

        private void ResetBg_Click(object sender, RoutedEventArgs e)
        {

            (Application.Current as App).iUseCustomBackground = false;
            (Application.Current as App).LocalSettings.Values["iUseCustomBackground"] = (Application.Current as App).iUseCustomBackground;

            if (true)
            {

                ((MP.Content as Grid).Children[0] as Frame).Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());
            }
            //BgPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/IconRes/home_wallpaper03.jpg"));
         }

        private async void SetBgFile_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).iUseCustomBackground = true;
            (Application.Current as App).LocalSettings.Values["iUseCustomBackground"] = (Application.Current as App).iUseCustomBackground;
            FileCustomBackground();
        }

        private async void FileCustomBackground()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            var inputFile = await fileOpenPicker.PickSingleFileAsync();
            if (inputFile == null)
                return;
            SoftwareBitmap SoftwareBitmap;
            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap = await decoder.GetSoftwareBitmapAsync();
            }
            Windows.Storage.StorageFolder StorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var OutputFile = await StorageFolder.CreateFileAsync("Customize\\Background.png", CreationCollisionOption.OpenIfExists);

            using (IRandomAccessStream stream = await OutputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetSoftwareBitmap(SoftwareBitmap);
                encoder.BitmapTransform.ScaledWidth = (uint)SoftwareBitmap.PixelWidth;
                encoder.BitmapTransform.ScaledHeight = (uint)SoftwareBitmap.PixelHeight;
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;
                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }
                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }

            ((MP.Content as Grid).Children[0] as Frame).Navigate(typeof(MainPage), null, new SuppressNavigationTransitionInfo());

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
                            BgPreview.Source = bitmapImage;
                            bitmapImage = null;
                        }
                    }
                    file = null;

                    OStoryBoardDoubleAnimation.From = 0;
                    OStoryBoardDoubleAnimation.To = 1;
                    OStoryBoard.Begin();
                }
                catch { }
            }
            else
            {
                BgPreview.Source = new BitmapImage(new Uri("ms-appx:///Assets/IconRes/home_wallpaper03.jpg"));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetCustomBackground();
        }
    }
}
