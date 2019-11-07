using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FaviconBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // book: Async https://777russia.ru/book/uploads/%D0%9F%D0%A0%D0%9E%D0%93%D0%A0%D0%90%D0%9C%D0%9C%D0%98%D0%A0%D0%9E%D0%92%D0%90%D0%9D%D0%98%D0%95/C%23/Async%20in%20C%23%205.0%2C%202012%20%28%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4%2C%20%D0%BD%D0%B0%20%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%BE%D0%BC%29.pdf
    // Rihter: https://viduus.net/wp-content/uploads/2018/02/Rihter-Dzh.-CLR-via-C.-Programmirovanie-na-platforme-Microsoft-.NET-Framework-4.5-na-yazyke-C-Master-klass-2013.pdf
    public partial class MainWindow : Window
    {
        private static readonly List<string> s_Domains = new List<string>
                                                             {
                                                                 "bing.com",
                                                                 "facebook.com",
                                                                 "reddit.com",
                                                                 "baidu.com",
                                                                 "bbc.co.uk"
                                                             };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetButton_OnClick(object sender, RoutedEventArgs e)
        {
            WebClient webClientGoogle = new WebClient();
            var googleImage = webClientGoogle.DownloadData(new Uri("http://google.com/favicon.ico"));
            Image imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);

            foreach (string domain in s_Domains)
            {
                AddAFavicon(domain);
            }
        }


        //async 
        //private async Task AddAFavicon(string domain)
        //{
        //    WebClient webClient = new WebClient();
        //    Task<byte[]> myTask = webClient.DownloadDataTaskAsync("http://" + domain + "/favicon.ico");

        //    // Здесь можно что-то сделать
        //    WebClient webClientGoogle = new WebClient();
        //    var googleImage = webClientGoogle.DownloadData(new Uri("http://google.com/favicon.ico"));
        //    Image imageControl = MakeImageControl(googleImage);
        //    m_WrapPanel.Children.Add(imageControl);

        //    byte[] page = await myTask;
        //    imageControl = MakeImageControl(page);
        //    m_WrapPanel.Children.Add(imageControl);
        //}

        private void AddAFavicon(string domain)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadDataCompleted += OnWebClientOnDownloadDataCompleted;

            // Здесь можно что-то сделать
            WebClient webClientGoogle = new WebClient();
            var googleImage = webClientGoogle.DownloadData(new Uri("http://google.com/favicon.ico"));
            Image imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);

            webClient.DownloadDataAsync(new Uri("http://" + domain + "/favicon.ico"));

            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
            m_WrapPanel.Children.Add(imageControl);
            imageControl = MakeImageControl(googleImage);
        }


        private void OnWebClientOnDownloadDataCompleted(object sender,
DownloadDataCompletedEventArgs args)
        {
            Image imageControl = MakeImageControl(args.Result);
            m_WrapPanel.Children.Add(imageControl);
        }

        private static Image MakeImageControl(byte[] bytes)
        {
            Image imageControl = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();
            imageControl.Source = bitmapImage;
            imageControl.Width = 16;
            imageControl.Height = 16;
            return imageControl;
        }
    }
}
