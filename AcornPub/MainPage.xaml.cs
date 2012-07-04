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

using System.Xml.Linq;
//using Microsoft.Phone.Tasks;


namespace AcornPub
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<Book> books = new List<Book>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            progressBar1.IsIndeterminate = true;

            WebClient WC = new WebClient();
            WC.DownloadStringAsync(new Uri("http://www.acornpub.co.kr/API/booklist.php?kind=NEW&page=1&pageSize=25&tid=(null)"));
            
            WC.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(Completed);

        }

        void Completed(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            string rssContent;
            rssContent = "hello";
            try
            {
                rssContent = e.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Internet Connection");
                return;
            }
            XDocument rssParser = XDocument.Parse(rssContent);
            //LINQ
            var rssList = from rssTree in rssParser.Descendants("Book")
                          select new Book
                          {
                              Title = rssTree.Element("title").Value,

                           Contents = rssTree.Element("subtitle").Value ,
                              Img = new Uri(rssTree.Element("imgpath").Value, UriKind.Absolute),
                                Url = rssTree.Element("bookid").Value

                            };

             
             listBox1.ItemsSource = rssList;

              books = rssList.ToList();

            progressBar1.Visibility = System.Windows.Visibility.Collapsed;
          }
 
         private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
          {
         //     WebBrowserTask blogViewer = new WebBrowserTask();
         //   blogViewer.URL = books[listBox1.SelectedIndex].Url;
         //   blogViewer.Show();
              NavigationService.Navigate(new Uri("/Detail.xaml?id="+books[listBox1.SelectedIndex].Url, UriKind.Relative));
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

   
    }

    public class Book
    {
        public string Title { get; set; }
        public string Contents { get; set; }
        public Uri Img { get; set; }
        public string Url { get; set; }
    }
}