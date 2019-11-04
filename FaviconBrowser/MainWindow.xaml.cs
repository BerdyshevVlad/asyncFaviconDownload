using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FaviconBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly List<string> s_Domains = new List<string>
                                                             {
                                                                 "google.com",
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
            foreach (string domain in s_Domains)
            {
                AddAFavicon(domain);
            }
        }

        ////ordered
        //private void GetButton_OnClick(object sender, RoutedEventArgs e)
        //{
        //    AddRemainingFavicons(s_Domains, 0);
        //}

        ////ordered
        //private void AddRemainingFavicons(List<string> domains, int i)
        //{
        //    WebClient webClient = new WebClient();
        //    webClient.DownloadDataCompleted += (o, args) =>
        //    {
        //        Image imageControl = MakeImageControl(args.Result);
        //        m_WrapPanel.Children.Add(imageControl);

        //        if (i + 1 < domains.Count)
        //        {
        //            AddRemainingFavicons(domains, i + 1);
        //        }
        //    };
        //    webClient.DownloadDataAsync(new Uri("http://" + domains[i] + "/favicon.ico"));
        //}


        //один поток для выполнения кода, второй поток - очередь выполнения. Сперва в очереди прогрузка всех картинок, а затем (последний в очереди - перерисовка элемемента.)
        private void AddAFavicon(string domain)
        {
            WebClient webClient = new WebClient();
            byte[] bytes = webClient.DownloadData("http://" + domain + "/favicon.ico");
            Image imageControl = MakeImageControl(bytes);
            m_WrapPanel.Children.Add(imageControl);
        }


        ////async 
        //private void AddAFavicon(string domain)
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
        //    WebClient webClient = new WebClient();
        //    webClient.DownloadDataCompleted += OnWebClientOnDownloadDataCompleted;
        //    webClient.DownloadDataAsync(new Uri("http://" + domain + "/favicon.ico"));
        //}

//        private void OnWebClientOnDownloadDataCompleted(object sender,
//DownloadDataCompletedEventArgs args)
//        {
//            Image imageControl = MakeImageControl(args.Result);
//            m_WrapPanel.Children.Add(imageControl);
//        }

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
