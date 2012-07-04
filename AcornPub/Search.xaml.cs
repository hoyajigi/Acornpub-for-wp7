﻿using System;
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

// EUCKR <--> Unicode String conversion library for Windows Phone 7 Mango
// Written by Kyle Koh
// Oct. 26, 2011
// kyle at hcil dot snu dot ac dot kr
//
// All codes are provided 'as-is', with NO warranty.
//
// ConversionTable.dat is made from cp949.txt written by
// Shawn.Steele at microsoft dot com
// and has been modified into a binary form to be read faster in a mobile environment
//
// You are free to modify, use, or distribute this code for both commercial and non-commercial use.
// However, if you want to distribute the source code of the modified version to the public,
//		please indicate the original author and briefly explain how you modified the original code for educational purpose.
// If you don't want to distribute your modified code, then you are free to distribute the program w/o credit to the original author

//using System;
//using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
//zusing System.Windows;
using System.Windows.Resources;

namespace EUCKR_Unicode_Library
{
    public class EUCKR_Unicode_Converter
    {
        private static List<Byte[]> unicodeToEucKrTable;
        private static List<Char> eucKrToUnicodeTable;

        /// <summary>
        /// Initialize the conversion table
        /// </summary>
        private static void Initialize()
        {
            if (unicodeToEucKrTable == null)
            {
                unicodeToEucKrTable = new List<Byte[]>(65536);
            }
            else
            {
                unicodeToEucKrTable.Clear();
            }

            if (eucKrToUnicodeTable == null)
            {
                eucKrToUnicodeTable = new List<Char>(65536);
            }
            else
            {
                eucKrToUnicodeTable.Clear();
            }


            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            StreamResourceInfo streamInfo = Application.GetResourceStream(new Uri("ConversionTable.dat", UriKind.Relative));
            BinaryReader br = new BinaryReader(streamInfo.Stream, Encoding.Unicode);

            for (Int32 i = 0; i < 65536; ++i)
            {
                Byte[] newData = new Byte[2];
                newData[1] = br.ReadByte();
                newData[0] = br.ReadByte();
                Char c = BitConverter.ToChar(newData, 0);
                eucKrToUnicodeTable.Add(c);

            }
            for (Int32 i = 0; i < 65536; ++i)
            {
                Byte[] newData = new Byte[2];
                newData[0] = br.ReadByte();
                newData[1] = br.ReadByte();
                unicodeToEucKrTable.Add(newData);
            }

            streamInfo.Stream.Close();
            br.Close();
        }

        /// <summary>
        /// Take an EUC-KR byte array and return a corresponding unicode string
        /// </summary>
        /// <param name="euckrStringBytes">EUC-KR byte array</param>
        /// <returns>Unicode String</returns>
        public static String GetUnicodeString(Byte[] euckrStringBytes)
        {
            if (eucKrToUnicodeTable == null)
            {
                Initialize();
            }
            StringBuilder stringBuilder = new StringBuilder();

            Int32 movingIndex = 0;
            while (movingIndex < euckrStringBytes.Length)
            {
                if (euckrStringBytes[movingIndex] < 129)
                {
                    Byte[] euckrChar = { euckrStringBytes[movingIndex] };
                    stringBuilder.Append(GetUnicodeChar(euckrChar));
                    movingIndex += 1;
                }
                else
                {
                    Byte[] euckrChar = { euckrStringBytes[movingIndex], euckrStringBytes[movingIndex + 1] };
                    stringBuilder.Append(GetUnicodeChar(euckrChar));
                    movingIndex += 2;
                }
            }

            return stringBuilder.ToString();
        }

        private static Char GetUnicodeChar(Byte[] euckrBytes)
        {
            Byte[] newBytes = new Byte[2];
            if (euckrBytes.Length == 1)
            {
                newBytes[0] = euckrBytes[0];
                newBytes[1] = 0;
            }
            else
            {
                newBytes[0] = euckrBytes[1];
                newBytes[1] = euckrBytes[0];
            }
            Int32 lookupIndex = (Int32)BitConverter.ToUInt16(newBytes, 0);

            return eucKrToUnicodeTable[lookupIndex];
        }

        /// <summary>
        /// Take a unicode string and return a corresponding EUC-KR byte array
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static Byte[] GetEucKRString(String unicodeString)
        {
            if (unicodeToEucKrTable == null)
            {
                Initialize();
            }

            List<Byte> euckrStringBytes = new List<Byte>(unicodeString.Length * 2);

            foreach (Char c in unicodeString)
            {
                foreach (Byte b in GetEucKR(c))
                {
                    euckrStringBytes.Add(b);
                }
            }

            return euckrStringBytes.ToArray();
        }

        private static Byte[] GetEucKR(Char unicodeChar)
        {
            Byte[] unicodeBytes = BitConverter.GetBytes(unicodeChar);

            Int32 lookupIndex = (Int32)BitConverter.ToUInt16(unicodeBytes, 0);

            Byte[] euckrBytes = unicodeToEucKrTable[lookupIndex];

            if (lookupIndex < 33024)
            { // if leading header is less than 0x81
                Byte[] newReturnValue = { euckrBytes[1] };
                return newReturnValue;
            }
            else
            {
                return euckrBytes;
            }
        }
    }
}


namespace AcornPub
{
    public partial class Search : PhoneApplicationPage
    {
        List<Book> books = new List<Book>();
        public Search()
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

        private void query_LostFocus(object sender, RoutedEventArgs e)
        {
            go();
        }

        private void query_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void query_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //     WebBrowserTask blogViewer = new WebBrowserTask();
            //   blogViewer.URL = books[listBox1.SelectedIndex].Url;
            //   blogViewer.Show();
            try
            {
                NavigationService.Navigate(new Uri("/Detail.xaml?id=" + books[listBox1.SelectedIndex].Url, UriKind.Relative));
            }
            catch (Exception ex)
            {
            }
        }

        private void go()
        {
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            progressBar1.IsIndeterminate = true;
            WebClient WC = new WebClient();
 //           System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding("cp949"),query.Text);
//            WC.Encoding=System.Text.Encoding.GetEncoding("cp949");
            var temp = EUCKR_Unicode_Library.EUCKR_Unicode_Converter.GetEucKRString(
                             query.Text
                         );
            var sb=new StringBuilder();
            foreach (byte i in temp)
            {
                sb.Append("%");
                sb.Append(i.ToString("X"));
            }
            var temp2 = System.Text.Encoding.UTF8.GetString(
                        temp, 0, temp.Length
                    );
            var temp3 = System.Net.HttpUtility.UrlEncode(
                    sb.ToString()
                );
            WC.DownloadStringAsync(new Uri("http://www.acornpub.co.kr/API/search.php?page=1&pageSize=25&keyword=" + 
                sb.ToString()
           ));

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

                              Contents = rssTree.Element("subtitle").Value,
                              Img = new Uri(rssTree.Element("imgpath").Value, UriKind.Absolute),
                              Url = rssTree.Element("bookid").Value

                          };


            listBox1.ItemsSource = rssList;

            books = rssList.ToList();

            progressBar1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void query_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode == 13)
            {
                this.Focus();
               
            }
        }

    }
}