using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WAO_Player.Resources;
using WAO_Player.Window_Child;
using Windows.Storage;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using WAO_Player.WindowChild;
using WAO_Player.Class;
using WAO_Player.Music_Online_NCT;
using System.ComponentModel;
using System.Windows.Threading;

namespace WAO_Player
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        WebClient WebClient;
        WebClient WebClient_GetMoreInfo;
        ReadHTML Reader_NCT;
        // Các danh sách bài hát, video:
        ListBox_Song List_Online;
        int Current_NowPlaying = -1;
        DispatcherTimer Timer;
        // Các biến giao diện:
        string _namenowartist;

        public string NameNowArtist
        {
            get { return _namenowartist; }
            set { _namenowartist = value; NotifyPropertyChanged("NameNowArtist"); }
        }
        string _namenowsong;

        public string NameNowSong
        {
            get { return _namenowsong; }
            set { _namenowsong = value; NotifyPropertyChanged("NameNowSong"); }
        }
        string _status;
       
        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropertyChanged("Status"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // Constructor
        public MainPage()
        {
            this.DataContext = this;
            InitializeComponent();
            Init_Other();
            Init_WebClient();
            Init_MediaElement();
            Reader_NCT = new ReadHTML();
            Init_List_Collection();
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void Init_Other()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Slider_Time.Value = Player.Position.TotalSeconds;
        }
        void Init_List_Collection()
        {
            List_Online = new ListBox_Song();
        }
        void Init_WebClient()
        {
            WebClient = new WebClient();
            WebClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
            WebClient_GetMoreInfo = new WebClient();
            WebClient_GetMoreInfo.DownloadStringCompleted += WebClient_GetMoreInfo_DownloadStringCompleted;
        }
        void Init_MediaElement()
        {
            Player.MediaOpened += Player_MediaOpened;
            Player.MediaEnded += Player_MediaEnded;
        }

        void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            
            Timer.Stop();
            Slider_Time.Value = 0;
        }

        void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            Slider_Time.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
        }
        void WebClient_GetMoreInfo_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if(List_Online.Items[List_Song_Online.SelectedIndex].Stream == null)
                List_Online.Items[List_Song_Online.SelectedIndex] = Reader_NCT.GetMoreSongInfo(e.Result, List_Online.Items[List_Song_Online.SelectedIndex]);
            Init_Open(List_Online.Items[List_Song_Online.SelectedIndex]);
            Btn_Play_Click(null, null);
        }

        void Init_Open(Song_W item)
        {
            NameNowArtist = item.Artist;
            NameNowSong = item.Name;
            Player.Source = new Uri(List_Online.Items[List_Song_Online.SelectedIndex].Stream);
        }

        void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Status = "";
            List_Online.Items = Reader_NCT.GetListSong(e.Result);
            List_Song_Online.ItemsSource = List_Online.Items;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            string s = Static_String.Search_Songs(Text_Search.Text, 1);
            Status = "Searching '" + Text_Search.Text +"' . . . ";
            WebClient.DownloadStringAsync(new System.Uri(s));
        }

        private void Text_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text_Search.Text != String.Empty)
                Button_Search.IsEnabled = true;
            else
                Button_Search.IsEnabled = false;
        }

        private void List_Song_Online_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Status = "Buffering '" + List_Online.Items[List_Song_Online.SelectedIndex].Name + "'";

            string temp = List_Online.Items[List_Song_Online.SelectedIndex].URL;
            List_Online.Items[List_Song_Online.SelectedIndex].URL = temp.Replace("http://www", "http://m");
            Current_NowPlaying = List_Song_Online.SelectedIndex;
            WebClient_GetMoreInfo.DownloadStringAsync(new Uri(List_Online.Items[List_Song_Online.SelectedIndex].URL));
        }

        private void Slider_Time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == Slider_Time.Maximum)
            {
                Slider_Time.Value = 0;
                NameNowSong = NameNowArtist = "";
            }
        }

        private void Slider_Time_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int pos = Convert.ToInt32(Slider_Time.Value);
            Player.Position = new TimeSpan(0, 0, pos);
        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Status = "";
            string key = Btn_Play.Content.ToString();
            if (key == "4")
            {
                if (Current_NowPlaying == -1)
                    return;
                Btn_Play.Content = ";";
                Player.Play();
                Timer.Start();
            }
            else
            {
                Btn_Play.Content = "4";
                Player.Pause();
                Timer.Stop();
            }
        }
        
               
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}