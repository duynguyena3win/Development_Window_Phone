using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using WAO_Player.Class;
using WAO_Player.Music_Online_NCT;

namespace WAO_Player.WindowChild
{
    public partial class Music_Page : PhoneApplicationPage, INotifyPropertyChanged
    {
        ListBox_Song LBS;
        private WebClient WebClient;
        ReadHTML Reader = new ReadHTML();
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Music_Page()
        {
            this.DataContext = this;
            InitializeComponent();
            WebClient = new WebClient();
            WebClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            LBS = new ListBox_Song();
        }


        void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            LBS.Items = Reader.GetListSong(e.Result);
            List_Search_Song.ItemsSource = LBS.Items;
        }

        private void Text_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text_Search.Text != String.Empty)
                Button_Search.IsEnabled = true;
            else
                Button_Search.IsEnabled = false;
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            string s = Static_String.Search_Songs(Text_Search.Text, 1);
            WebClient.DownloadStringAsync(new System.Uri(s));
        }

        private void List_Search_Song_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void List_Search_Song_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show(List_Search_Song.SelectedIndex.ToString());
        }
    }
}