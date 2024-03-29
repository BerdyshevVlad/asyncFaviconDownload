﻿using System;
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
    /// 

    // book: Async https://777russia.ru/book/uploads/%D0%9F%D0%A0%D0%9E%D0%93%D0%A0%D0%90%D0%9C%D0%9C%D0%98%D0%A0%D0%9E%D0%92%D0%90%D0%9D%D0%98%D0%95/C%23/Async%20in%20C%23%205.0%2C%202012%20%28%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4%2C%20%D0%BD%D0%B0%20%D1%80%D1%83%D1%81%D1%81%D0%BA%D0%BE%D0%BC%29.pdf
    // Rihter: https://viduus.net/wp-content/uploads/2018/02/Rihter-Dzh.-CLR-via-C.-Programmirovanie-na-platforme-Microsoft-.NET-Framework-4.5-na-yazyke-C-Master-klass-2013.pdf
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


        //один поток для выполнения кода, второй поток - очередь выполнения. Идет выполнение в одном потоке, когда код проходит строку 73- m_WrapPanel.Children.Add(imageControl); в очередь выполнения добавляется изменения по отрисовке компонентов. Но т.к. это синхронное выполнение, то необходимо дождаться завершения кода (загрузки всех картинок), а потом отработает отрисовка(из очереди выполнения).
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
