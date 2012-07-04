using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace AcornPub
{
    public partial class Blog : PhoneApplicationPage
    {
        public Blog()
        {
            InitializeComponent();
        }
        private void n1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Category.xaml", UriKind.Relative));
        }
        private void n2_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
        private void n3_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Search.xaml", UriKind.Relative));
        }
        private void n4_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Blog.xaml", UriKind.Relative));
        }
        
        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser1.Navigate(new Uri("http://acornpub.co.kr/blog/i", UriKind.Absolute));
        }
    }
}