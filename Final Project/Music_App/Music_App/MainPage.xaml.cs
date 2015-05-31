using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Music_App.Resources;
using Microsoft.Xna.Framework.Media;
using Music_App.Process;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.Windows.Threading;
using Music_App.Class;
using Microsoft.Phone.BackgroundAudio;
using System.Collections.ObjectModel;
using Music_App.MyFacebook;
using System.Xml.Linq;
using System.Windows.Resources;
using System.IO.IsolatedStorage;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
//using vservWindowsPhone;

namespace Music_App
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        #region
        // Variable
        public event PropertyChangedEventHandler PropertyChanged;
        DispatcherTimer DTimer;
        bool Bool_Play;
        // + Biến xữ lý:
        MusicOffline MOffline;
        MusicOnline mOnline;

        //VservAdControl VSB = VservAdControl.Instance;
        string zoneId = "20846";//8deaea04"; 
        bool bool_close = false;
        bool bool_QC = false;

        public MusicOnline MOnline
        {
            get { return mOnline; }
            set { mOnline = value; NotifyPropertyChanged("MOnline"); }
        }

        // + Biến Binding:
        string _namenowartist;
        public string NameNowArtist
        {
            get { return _namenowartist; }
            set { _namenowartist = value; NotifyPropertyChanged("NameNowArtist"); }
        }

        string _timecurrent;
        public string TimeCurrent
        {
            get { return _timecurrent; }
            set { _timecurrent = value; NotifyPropertyChanged("TimeCurrent"); }
        }

        string _timeend;
        public string TimeEnd
        {
            get { return _timeend; }
            set { _timeend = value; NotifyPropertyChanged("TimeEnd"); }
        }

        ObservableCollection<MySong> list_NowPlaying;
        List<HistorySong> ListHistory = new List<HistorySong>();

        public ObservableCollection<MySong> List_NowPlaying
        {
            get { return list_NowPlaying; }
            set { list_NowPlaying = value; NotifyPropertyChanged("List_NowPlaying"); }
        }
        
        int current_NowPlaying;
        public int Current_NowPlaying
        {
            get { return current_NowPlaying; }
            set { 
                current_NowPlaying = value;
                if (current_NowPlaying < 0)
                    current_NowPlaying = List_NowPlaying.Count - 1;
                if (current_NowPlaying >= List_NowPlaying.Count)
                    current_NowPlaying = 0;
                }
            
        }

        private void SetProgressIndicator(string status, bool value)
        {
            
            SystemTray.ProgressIndicator.Text = status;
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }
        string _namenowsong;
        public string NameNowSong
        {
            get { return _namenowsong; }
            set { _namenowsong = value; NotifyPropertyChanged("NameNowSong"); }
        }

        string _imagenowsong;
        public string ImageNowSong
        {
            get { return _imagenowsong; }
            set { _imagenowsong = value; NotifyPropertyChanged("ImageNowSong"); }
        }

        string _lyric;

        public string Lyric
        {
            get { return _lyric; }
            set { _lyric = value; NotifyPropertyChanged("Lyric"); }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = this;
            //QuangCao();

            Init_Variable();
            Init_Timer();
            Init_AppBar();
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            Radio_VPOP.IsChecked = true;
        }
        // Init for Program . . .
        void Init_Timer()
        {
            DTimer = new DispatcherTimer();
            DTimer.Interval = TimeSpan.Parse("00:00:01");
            DTimer.Tick += DTimer_Tick;
        }

        //void QuangCao()
        //{
        //    if (App.Bool_Load == false)
        //    {
        //        App.Bool_Load = true;
        //        VSB.SetRequestTimeOut(10);
        //        //VSB.DisplayAd(zoneId, LayoutRoot);
        //        // Xử lí khi hiển thị tắt quảng cáo 
        //        VSB.VservAdClosed += new EventHandler(VACCallback_OnVservAdClosing);
        //        // Xử lí khi không có kết nối mạng 
        //        VSB.VservAdError += new EventHandler(VACCallback_OnVservAdNetworkError);
        //        // HET QUANG CAO --- 
        //    }
        //    else
        //    {
        //        Init_AppBar();
        //    }
        //}

        //protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        //{
        //    if (bool_QC == false)
        //        return;
        //    bool_close = true;
        //    if (add.Visibility == Visibility.Collapsed)
        //    {
        //        // tích hợp nếu dùng thoát app có quảng cáo 
        //        e.Cancel = true;
        //        //VSB.DisplayAd(zoneId, LayoutRoot);
        //        add.Visibility = Visibility.Visible;

        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //    }
        //}

        private void VACCallback_OnVservAdNetworkError(object sender, EventArgs e)
        {
            MessageBox.Show("Data connection not available", "No Data",
                    MessageBoxButton.OKCancel);
            Init_AppBar();
        }

        private void VACCallback_OnVservAdClosing(object sender, EventArgs e)
        {
            if (bool_close == true)
                Application.Current.Terminate();
            Init_AppBar();
        }

        void Init_AppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBarIconButton Button_Lyric = new ApplicationBarIconButton();
            Button_Lyric.IconUri = new Uri("/Assets/Picture/edit.png", UriKind.Relative);
            Button_Lyric.Text = "Show lyric";
            Button_Lyric.Click +=Button_Lyric_Click;
            ApplicationBar.Buttons.Add(Button_Lyric);

            ApplicationBarIconButton Button_Info = new ApplicationBarIconButton();
            Button_Info.IconUri = new Uri("/Assets/Picture/about.png", UriKind.Relative);
            Button_Info.Text = "Information";
            Button_Info.Click +=Button_Info_Click;
            ApplicationBar.Buttons.Add(Button_Info);

            ApplicationBarMenuItem menuItem_face = new ApplicationBarMenuItem();
            menuItem_face.Text = "share on Facebook";
            menuItem_face.Click += menuItem_face_Click;
            ApplicationBar.MenuItems.Add(menuItem_face);
            ApplicationBar.Opacity = 0.6f;
        }

        void Button_Lyric_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(Lyric, "Lyric current Song", MessageBoxButton.OK);
            }
            catch { }
        }

        void Button_Info_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Music Room Ver 1.0 \r\nDeveloper Nguyễn Duy Nguyên\r\nEmail: duynguyena3win@gmail.com", "Information", MessageBoxButton.OK);
            }
            catch { }
        }

        void menuItem_face_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MyFacebook/FacebookLoginPage.xaml", UriKind.Relative));
            }
            catch { }
        }
        void Init_Songs(MySong Item)
        {
            if (Item.bool_on == false)
            {
                Slider_Time.Maximum = Item.OfflineSong.Duration.TotalSeconds;
                TimeEnd = Get_String_Time(Item.OfflineSong.Duration.Minutes, Item.OfflineSong.Duration.Seconds);
            }
            NameNowSong = Item.Name;
            NameNowArtist = Item.Artist.Name;
            ImageNowSong = Item.Image;
        }

        string Get_String_Time(int min, int sec)
        {
            string temp;
            if (min <= 9)
                temp = "0" + min.ToString();
            else
                temp = min.ToString();
            
            if (sec <= 9)
                temp += ":0" +  sec.ToString();
            else
                temp += ":" + sec.ToString();
            return temp;
        }

        void Init_Songs(AudioTrack Item)
        {
            Slider_Time.Maximum = Item.Duration.TotalSeconds;
            TimeEnd = Get_String_Time(Item.Duration.Minutes, Item.Duration.Seconds);
            NameNowSong = Item.Title;
            NameNowArtist = Item.Artist;
            ImageNowSong = Item.Tag;
        }

        string Get_String_Image(string str)
        {
            string s = null;
            int index = str.IndexOf("^_%11*2199");
            for (int i = index + 10; i < str.Length; i++)
                s += str[i];
            return s;
        }

        void Init_Variable()
        {
            MOffline = new MusicOffline();
            Init_ListOffline();

            MOnline = new MusicOnline();
            MOnline.LoadingSongCompleted += MOnline_LoadingSongCompleted;
            MOnline.LoadingListCompleted += MOnline_LoadingListCompleted;
            BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
            TimeCurrent = TimeEnd = "";
        }

        void MOnline_LoadingListCompleted()
        {
            Block_Control_Online(true);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            bool_QC = false;
            Write_XML();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool_QC = true;
            Read_XML();
            if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState || PlayState.Paused == BackgroundAudioPlayer.Instance.PlayerState)
            {
                try
                {
                    Bool_Play = false;
                    if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState)
                        Btn_Play.Content = ";";
                    else
                        Btn_Play.Content = "4";
                    DTimer.Start();
                    Lyric = List_NowPlaying[Current_NowPlaying].Lyric;
                    Init_Songs(BackgroundAudioPlayer.Instance.Track);
                }
                catch { }
            }

            if (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused)
            {
                try
                {
                    Bool_Play = true;
                    if (MediaPlayer.State == MediaState.Playing)
                        Btn_Play.Content = ";";
                    else
                        Btn_Play.Content = "4";
                    DTimer.Start();
                    Slider_Time.Maximum = MediaPlayer.Queue.ActiveSong.Duration.TotalSeconds;
                    TimeEnd = Get_String_Time(MediaPlayer.Queue.ActiveSong.Duration.Minutes, MediaPlayer.Queue.ActiveSong.Duration.Seconds);
                    NameNowSong = MediaPlayer.Queue.ActiveSong.Name;
                    NameNowArtist = MediaPlayer.Queue.ActiveSong.Artist.Name;
                    ImageNowSong = MOffline.OffL_Current_Songs[MediaPlayer.Queue.ActiveSongIndex].Image;
                }
                catch { }
            }
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    
                    DTimer.Start();
                    Btn_Play.Content = ";";
                    break;
                case PlayState.Stopped:
                case PlayState.Paused:
                    DTimer.Stop();
                    Btn_Play.Content = "4";
                    break;
                case PlayState.BufferingStarted:
                    SetProgressIndicator("Buffering", true);
                    break;
                case PlayState.BufferingStopped:
                    SetProgressIndicator("", false);
                    break;
            }

            if (null != BackgroundAudioPlayer.Instance.Track)
            {
                Init_Songs(BackgroundAudioPlayer.Instance.Track);
            }
        }

        void MOnline_LoadingSongCompleted(MySong Output)
        {
            Block_Control_Online(true);            
            Add_Song_NowPlaying(Output);
            Current_NowPlaying = List_NowPlaying.Count - 1;
            PlaySong();
        }

        void MOnline_WebDownloadCompleted()
        {
            Block_Control_Online(true);
        }

        void Read_XML()
        {
            List_NowPlaying = new ObservableCollection<MySong>();
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("DataHistory.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<HistorySong>));

                        ListHistory = (List<HistorySong>)serializer.Deserialize(stream);
                        
                        for(int i=0;i<ListHistory.Count;i++)
                            List_NowPlaying.Add(new MySong(ListHistory[i]));
                        if (ListHistory.Count > 0)
                            Current_NowPlaying = ListHistory[0].Current_Index;
                    }
                }
            }
            catch (Exception ex)
            {
                //add some code here
            }
        }
        void Write_XML()
        {
            try
            {
                // Write to the Isolated Storage
                ListHistory = new List<HistorySong>();
                for (int i = 0; i < List_NowPlaying.Count; i++)
                {
                    HistorySong HS = new HistorySong();
                    HS.CopySong(List_NowPlaying[i]);
                    HS.Current_Index = Current_NowPlaying;
                    ListHistory.Add(HS);
                }
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("DataHistory.xml", FileMode.Create))
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<HistorySong>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            serializer.Serialize(xmlWriter, ListHistory);
                        }
                    }
                }
            }
            catch { MessageBox.Show("Có lổi khi lưu vào dữ liệu lịch sử hệ thống!"); }
        }
        void Init_ListOffline()
        {
            Radio_Song_Offline.IsChecked = true;
        }

        // Process 
        void PlaySong()
        {
            if (List_NowPlaying.Count() == 0)
                return;
            if (Current_NowPlaying < 0 || Current_NowPlaying > List_NowPlaying.Count - 1)
                return;
            Init_Songs(List_NowPlaying[Current_NowPlaying]);
            DTimer.Start();

            DTO_Class.CopySong(List_NowPlaying[Current_NowPlaying]);
            Lyric = List_NowPlaying[Current_NowPlaying].Lyric;

            if (List_NowPlaying[Current_NowPlaying].Stream != null)
            {
                FrameworkDispatcher.Update();
                MediaPlayer.Stop();
                Bool_Play = false;
                
                BackgroundAudioPlayer.Instance.Track = List_NowPlaying[Current_NowPlaying].Offline_Track;
                BackgroundAudioPlayer.Instance.Play();
            }
            else
            {
                FrameworkDispatcher.Update();
                BackgroundAudioPlayer.Instance.Close();
                Bool_Play = true;
                MediaPlayer.Play(List_NowPlaying[Current_NowPlaying].OfflineSong);
            }
        }
        void Add_Song_NowPlaying(Song Item)
        {
            MySong temp = new MySong(Item);
            List_NowPlaying.Add(temp);
        }
        void Add_Song_NowPlaying(MySong Item)
        {
            MySong temp = new MySong(Item);
            List_NowPlaying.Add(temp);
        }
        void Remove_Song_NowPlaying(int index)
        {
            List_NowPlaying.RemoveAt(index);
        }
        void Remove_From_List(ObservableCollection<MySong> List_Data, ListBox List_Control, int indexRemove)
        {
            List_Data.RemoveAt(indexRemove);

            List_Control.ItemsSource = null;
            List_Control.Items.Clear();
            List_Control.ItemsSource = List_Data;
        }

        // Event of Main Page . . . .
        void DTimer_Tick(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
            if (Bool_Play == true)
            {
                Slider_Time.Value = MediaPlayer.PlayPosition.TotalSeconds;
                TimeCurrent = Get_String_Time(MediaPlayer.PlayPosition.Minutes, MediaPlayer.PlayPosition.Seconds);
            }
            else
            {
                Slider_Time.Value = BackgroundAudioPlayer.Instance.Position.TotalSeconds;
                TimeCurrent = Get_String_Time(BackgroundAudioPlayer.Instance.Position.Minutes, BackgroundAudioPlayer.Instance.Position.Seconds);
            }
        }
        void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            //zif (Checkbox_Reve.IsChecked == true)
            //{
            //    if (MediaPlayer.State == MediaState.Stopped)
            //    {
            //        PlaySong();
            //    }
            //    return;
            //}

            if (Checkbox_Round.IsChecked == true)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    Current_NowPlaying++;
                    PlaySong();
                }
            }
        }

        // PanoramaItem 1 : Main Program
        private void Checkbox_Round_Checked(object sender, RoutedEventArgs e)
        {
            FrameworkDispatcher.Update();
            MediaPlayer.IsShuffled = true;
        }
        private void Checkbox_Reve_Checked(object sender, RoutedEventArgs e)
        {
            FrameworkDispatcher.Update();
            Random rand = new Random();
            Current_NowPlaying = rand.Next() % List_NowPlaying.Count;
            if (Bool_Play == true)
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Paused:
                    case MediaState.Playing:
                        Btn_Play.Content = ";";
                        PlaySong();
                        break;

                    case MediaState.Stopped:
                        break;
                }
            }
            else
            {
                switch (BackgroundAudioPlayer.Instance.PlayerState)
                {
                    case PlayState.Paused:
                    case PlayState.Playing:
                        Btn_Play.Content = ";";
                        DTimer.Start();
                        PlaySong();
                        break;
                    case PlayState.Stopped:
                        break;
                }
            }
        }

        private void Btn_Pre_Click(object sender, RoutedEventArgs e)
        {
            if (Bool_Play == true)
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Paused:
                    case MediaState.Playing:
                        Btn_Play.Content = ";";
                        DTimer.Start();
                        Current_NowPlaying--;
                        PlaySong();
                        break;

                    case MediaState.Stopped:
                        Current_NowPlaying--;
                        break;
                }
            }
            else
            {
                switch (BackgroundAudioPlayer.Instance.PlayerState)
                {
                    case PlayState.Paused:
                    case PlayState.Playing:
                        Btn_Play.Content = ";";
                        Current_NowPlaying--;
                        DTimer.Start();
                        PlaySong();
                        break;
                    case PlayState.Stopped:
                        Current_NowPlaying--;
                        break;
                }
            }
        }
        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            string key = Btn_Play.Content.ToString();
            if (Bool_Play == true)
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Playing:
                        Btn_Play.Content = "4";
                        MediaPlayer.Pause();
                        DTimer.Stop();
                        break;
                    case MediaState.Paused:
                        Btn_Play.Content = ";";
                        MediaPlayer.Resume();
                        DTimer.Start();
                        break;
                    case MediaState.Stopped:
                        break;
                }
            }
            else
            {
                switch(BackgroundAudioPlayer.Instance.PlayerState)
                {
                    case PlayState.Paused:
                        Btn_Play.Content = ";";
                        BackgroundAudioPlayer.Instance.Play();
                        DTimer.Start();
                        break;
                    case PlayState.Playing:
                        Btn_Play.Content = "4";
                        BackgroundAudioPlayer.Instance.Pause();
                        DTimer.Stop();
                        break;
                    case PlayState.Stopped:
                        break;
                }
            }
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            if (Bool_Play == true)
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Paused:
                    case MediaState.Playing:
                        Btn_Play.Content = ";";
                        Current_NowPlaying++;
                        DTimer.Start();
                        PlaySong();
                        break;

                    case MediaState.Stopped:
                        Current_NowPlaying++;
                        break;
                }
            }
            else
            {
                switch (BackgroundAudioPlayer.Instance.PlayerState)
                {
                    case PlayState.Paused:
                    case PlayState.Playing:
                        Btn_Play.Content = ";";
                        Current_NowPlaying++;
                        DTimer.Start();
                        PlaySong();
                        break;
                    case PlayState.Stopped:
                        Current_NowPlaying++;
                        break;
                }
            }
        }

        private void Slider_Time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue > Slider_Time.Maximum-1 && Slider_Time.Maximum != 0)
            { 
                Slider_Time.Value = 0;
                TimeCurrent = TimeEnd = "";
                NameNowSong = NameNowArtist = "";
                DTimer.Stop();
                if (Checkbox_Round.IsChecked == false)
                {
                    Btn_Next_Click(null, null);
                }
            }
        }
        private void Slider_Time_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DTimer.Start();
            try
            {
                int pos = Convert.ToInt32(Slider_Time.Value);
                if (Bool_Play == false)
                {
                    TimeSpan temp = new TimeSpan(pos / 3600, pos / 60, pos % 60);
                    BackgroundAudioPlayer.Instance.Position = temp;
                }
                
            }
            catch { }
        }

        // PanoramaItem 2 : List Song Offline 
        private void List_Artist_Offline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MOffline.Load_Songs_Artist(List_Artist_Offline.SelectedIndex);

                List_Song_Offline.Visibility = System.Windows.Visibility.Visible;
                List_Artist_Offline.Visibility = System.Windows.Visibility.Collapsed;

                List_Song_Offline.ItemsSource = null;
                List_Song_Offline.Items.Clear();

                List_Song_Offline.ItemsSource = MOffline.OffL_Current_Songs;
            }
            catch
            {
            }
        }
        private void List_Album_Offline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MOffline.Load_Songs_Album(List_Album_Offline.SelectedIndex);

                List_Song_Offline.Visibility = System.Windows.Visibility.Visible;
                List_Album_Offline.Visibility = System.Windows.Visibility.Collapsed;

                List_Song_Offline.ItemsSource = null;
                List_Song_Offline.Items.Clear();

                List_Song_Offline.ItemsSource = MOffline.OffL_Current_Songs;
            }
            catch
            { }
        }
        private void List_Song_Offline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                MediaLibrary ML = new MediaLibrary();
                if (List_Song_Offline.SelectedIndex == -1)
                    return;

                Btn_Play.Content = ";";
                Add_Song_NowPlaying(MOffline.List_Current_Song[List_Song_Offline.SelectedIndex]);
                Current_NowPlaying = List_NowPlaying.Count - 1;
                PlaySong();
            }
            catch { }
        }

        private void List_Song_NowPlaying_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Current_NowPlaying = List_Song_NowPlaying.SelectedIndex;

                PlaySong();
            }
            catch { }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            List_Song_Offline.Visibility = System.Windows.Visibility.Visible;
            List_Album_Offline.Visibility = System.Windows.Visibility.Collapsed;
            List_Artist_Offline.Visibility = System.Windows.Visibility.Collapsed;

            List_Song_Offline.ItemsSource = null;
            List_Song_Offline.Items.Clear();

            //MOffline.Load_Songs();
            List_Song_Offline.ItemsSource = MOffline.OffL_Current_Songs;
        }
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            List_Song_Offline.Visibility = System.Windows.Visibility.Collapsed;
            List_Artist_Offline.Visibility = System.Windows.Visibility.Visible;
            List_Album_Offline.Visibility = System.Windows.Visibility.Collapsed;
            List_Artist_Offline.ItemsSource = MOffline.OffL_Artist;
        }
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            List_Song_Offline.Visibility = System.Windows.Visibility.Collapsed;
            List_Artist_Offline.Visibility = System.Windows.Visibility.Collapsed;
            List_Album_Offline.Visibility = System.Windows.Visibility.Visible;
            List_Album_Offline.ItemsSource = MOffline.OffL_Album;
        }

        // Context Menu for List Now Playing:
        private void MenuItem_Play_ListNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            Current_NowPlaying = List_Song_NowPlaying.Items.IndexOf((sender as MenuItem).DataContext); ;
            PlaySong();
        }
        private void MenuItem_Remove_ListNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = List_Song_NowPlaying.Items.IndexOf((sender as MenuItem).DataContext);
            Remove_From_List(List_NowPlaying, List_Song_NowPlaying, selectedIndex);
        }

        private void MenuItem_Play_ListOffline_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = List_Song_Offline.Items.IndexOf((sender as MenuItem).DataContext);
            Add_Song_NowPlaying(MOffline.List_Current_Song[selectedIndex]);

            Btn_Play.Content = ";";
            Current_NowPlaying = List_NowPlaying.Count - 1;
            PlaySong();
        }
        private void MenuItem_Add_ListOffline_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = List_Song_Offline.Items.IndexOf((sender as MenuItem).DataContext);
            Add_Song_NowPlaying(MOffline.List_Current_Song[selectedIndex]);
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
            Block_Control_Online(false);
            MOnline.Button_Search_Click(Text_Search.Text);
        }

        void Block_Control_Online(bool state)
        {
            if (state == true)
                SetProgressIndicator("",false);
            else
                SetProgressIndicator("Loading", true);
            Text_Search.IsEnabled = Button_Search.IsEnabled = Radio_NEW.IsEnabled = Radio_KPOP.IsEnabled = Radio_USUK.IsEnabled = Radio_VPOP.IsEnabled = state;
        }

        private void List_Song_Online_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Block_Control_Online(false);
            if(List_Song_Online.SelectedIndex < 0 || MOnline.State == false)
                return;
            MOnline.GetSongSelected(List_Song_Online.SelectedIndex);
        }

        private void Radio_NEW_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Block_Control_Online(false);
                MOnline.Search_New_Song();
            }
            catch { }
        }

        private void Radio_VPOP_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Block_Control_Online(false);
                MOnline.Search_Top_Song(0);
            }
            catch { }
        }

        private void Radio_USUK_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Block_Control_Online(false);
                MOnline.Search_Top_Song(1);
            }
            catch { }
        }

        private void Radio_KPOP_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Block_Control_Online(false);
                MOnline.Search_Top_Song(2);
            }
            catch { }
        }

        private void Slider_Time_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DTimer.Stop();
            
        }

        // PanoramaItem 2 : List Song Online 
        
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