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
using Microsoft.Phone;

using System.Xml.Linq;
//using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging; 

namespace AcornPub
{
    public partial class Detail : PhoneApplicationPage
    {
        String id;
        List<Book> books = new List<Book>();

        public Detail()
        {
            InitializeComponent();
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            progressBar1.IsIndeterminate = true;
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
            var rssList = from rssTree in rssParser.Descendants("Result")
                          select new Book
                          {
                              Title = rssTree.Element("title").Value,

                              Contents = rssTree.Element("feature").Value,
                              Img = new Uri(rssTree.Element("imgpath").Value, UriKind.Absolute),
                              Url = rssTree.Element("bookid").Value

                          };


            //listBox1.ItemsSource = rssList;

            books = rssList.ToList();
            PageTitle.Text = books[0].Title;
            if (books[0].Contents.Length > 1300)
            {
                Contents1.Text = books[0].Contents.Substring(0, 1300);
                if (books[0].Contents.Length > 2600)
                {
                    Contents2.Text = books[0].Contents.Substring(1300, 1300);
                    if (books[0].Contents.Length > 3900)
                    {
                        Contents3.Text = books[0].Contents.Substring(2600, 1300);
                        if (books[0].Contents.Length > 5200)
                        {
                            Contents4.Text = books[0].Contents.Substring(3900, 1300);
                            if (books[0].Contents.Length > 6500)
                            {
                                Contents5.Text = books[0].Contents.Substring(5200, 1300);
                                if (books[0].Contents.Length > 7800)
                                {
                                    Contents6.Text = books[0].Contents.Substring(6500, 1300);
                                }
                                else
                                {
                                    Contents6.Text = books[0].Contents.Substring(6500);
                                }
                            }
                            else
                            {
                                Contents5.Text = books[0].Contents.Substring(5200);
                            }
                        }
                        else
                        {
                            Contents4.Text = books[0].Contents.Substring(3900);
                        }

                    }
                    else
                    {
                        Contents3.Text = books[0].Contents.Substring(2600);
                    }
                }
                else
                {
                    Contents2.Text = books[0].Contents.Substring(1300);
                }
            }
            else
            {
                Contents1.Text = books[0].Contents;
            }

            BookImage.Source = new BitmapImage(books[0].Img);
            progressBar1.Visibility = System.Windows.Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            id = NavigationContext.QueryString["id"];
            WebClient WC = new WebClient();
            WC.DownloadStringAsync(new Uri("http://www.acornpub.co.kr/API/detail.php?bid=" + id));

            WC.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler(Completed);

        } 
    }
}