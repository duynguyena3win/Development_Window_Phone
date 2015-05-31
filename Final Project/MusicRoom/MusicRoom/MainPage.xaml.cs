using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MusicRoom.Resources;
using System.ComponentModel;
using System.Windows.Threading;
using Music_App.Process;
using System.Collections.ObjectModel;
using Music_App.Class;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Music_App.MyFacebook;
using System.Xml;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using vservWindowsPhone;
using System.Threading;
using RateMyApp.Controls;

//using System.Xml.Serialization;

namespace MusicRoom
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        #region Variable
        public event PropertyChangedEventHandler PropertyChanged;
        DispatcherTimer DTimer;
        DispatcherTimer DTimerDownload;
        DispatcherTimer DTimerWait;
        bool Bool_Play;
        VservAdControl VSB = VservAdControl.Instance;
        string zoneId = "8deaea04"; //"20846";//
        bool bool_close = false;
        bool bool_statusDownload = false;
        #endregion

        #region Biến xữ lý:
        MusicOffline mOffline;

        public MusicOffline MOffline
        {
            get { return mOffline; }
            set { mOffline = value; NotifyPropertyChanged("MOffline"); }
        }
        MusicOnline mOnline;        

        public MusicOnline MOnline
        {
            get { return mOnline; }
            set { mOnline = value; NotifyPropertyChanged("MOnline"); }
        }
        int current_NowPlaying;
        
        public int Current_NowPlaying
        {
            get { return current_NowPlaying; }
            set
            {
                current_NowPlaying = value;
                if (current_NowPlaying < 0)
                    current_NowPlaying = List_NowPlaying.Count - 1;
                if (current_NowPlaying >= List_NowPlaying.Count)
                    current_NowPlaying = 0;
            }

        }
        ObservableCollection<MySong> list_NowPlaying;
        List<HistorySong> ListHistory = new List<HistorySong>();

        ObservableCollection<MySong> listDownload = new ObservableCollection<MySong>();
        public ObservableCollection<MySong> ListDownload
        {
            get { return listDownload; }
            set { listDownload = value; NotifyPropertyChanged("ListDownload"); }
        }
        #endregion

        #region Biến Binding       
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

        public ObservableCollection<MySong> List_NowPlaying
        {
            get { return list_NowPlaying; }
            set { list_NowPlaying = value; NotifyPropertyChanged("List_NowPlaying"); }
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
        #endregion
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = this;
            
            try
            {
                Init_Variable();
                Init_Timer();
                
                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
                QuangCao();
            }
            catch { }
            finally
            {
                Storyboard1.Begin();
            }
        }

        void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = (FeedbackOverlay.Visibility != Visibility.Visible);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            Radio_USUK.IsChecked = true;
            FeedbackOverlay.VisibilityChanged += FeedbackOverlay_VisibilityChanged;
            DataContext = RateMyApp.Helpers.FeedbackHelper.Default;
            this.DataContext = this;
        }

        #region Quảng Cáo 
        void QuangCao()
        {
            try
            {
                if (App.Bool_Load == false)
                {
                    App.Bool_Load = true;
                    VSB.SetRequestTimeOut(10);
                    VSB.DisplayAd(zoneId, LayoutRoot);
                    // Xử lí khi hiển thị tắt quảng cáo 
                    VSB.VservAdClosed += new EventHandler(VACCallback_OnVservAdClosing);
                    // Xử lí khi không có kết nối mạng 
                    VSB.VservAdError += new EventHandler(VACCallback_OnVservAdNetworkError);
                    // HET QUANG CAO --- 
                }
                else
                {
                }
            }
            catch { }
            finally
            {
                Init_AppBar();
            }
        }
        private void VACCallback_OnVservAdNetworkError(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Data connection not available", "No Data",
                        MessageBoxButton.OK);
            }
            catch { }
            finally
            {
                Init_AppBar();
            }
        }

        private void VACCallback_OnVservAdClosing(object sender, EventArgs e)
        {
            try
            {
                if (bool_close == true)
                    Application.Current.Terminate();
            }
            catch { }
            finally
            {
                Init_AppBar();
            }
        }
        #endregion

        #region Init Apps
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        void Init_AppBar()
        {
            try
            {
                ApplicationBar = new ApplicationBar();

                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Opacity = 1.0;
                ApplicationBar.IsVisible = true;
                ApplicationBar.IsMenuEnabled = true;

                ApplicationBarIconButton Button_Lyric = new ApplicationBarIconButton();
                Button_Lyric.IconUri = new Uri("/Assets/Picture/edit.png", UriKind.Relative);
                Button_Lyric.Text = "Show lyric";
                Button_Lyric.Click += Button_Lyric_Click;
                ApplicationBar.Buttons.Add(Button_Lyric);

                ApplicationBarIconButton Button_Download = new ApplicationBarIconButton();
                Button_Download.IconUri = new Uri("/Assets/Picture/download.png", UriKind.Relative);
                Button_Download.Text = "Download";
                Button_Download.Click += Button_Download_Click;
                ApplicationBar.Buttons.Add(Button_Download);

                ApplicationBarIconButton Button_Info = new ApplicationBarIconButton();
                Button_Info.IconUri = new Uri("/Assets/Picture/about.png", UriKind.Relative);
                Button_Info.Text = "Information";
                Button_Info.Click += Button_Info_Click;
                ApplicationBar.Buttons.Add(Button_Info);

                ApplicationBarMenuItem menuItem_face = new ApplicationBarMenuItem();
                menuItem_face.Text = "share on Facebook";
                menuItem_face.Click += menuItem_face_Click;
                ApplicationBar.MenuItems.Add(menuItem_face);
                ApplicationBar.Opacity = 0.6f;
            }
            catch { }
        }

        void Button_Download_Click(object sender, EventArgs e)
        {
            try
            {
                if(CheckSongDownload())
				{
                    Status_Download.Text = "Downloading '" + List_NowPlaying[Current_NowPlaying].Name + "' of " + List_NowPlaying[Current_NowPlaying].Artist.Name + " . . . wait some minute";
                    MOnline.Download(List_NowPlaying[Current_NowPlaying]);
				}
            }
            catch { }
        }
        bool CheckSongDownload()
        {
            for (int i = 0; i < ListDownload.Count; i++)
                if (ListDownload[i].Stream == List_NowPlaying[Current_NowPlaying].Stream)
                    return false;
            return true;
        }
        void Init_Timer()
        {
            try
            {
                DTimer = new DispatcherTimer();
                DTimer.Interval = TimeSpan.Parse("00:00:01");
                DTimer.Tick += DTimer_Tick;

                DTimerDownload = new DispatcherTimer();
                DTimerDownload.Interval = TimeSpan.Parse("00:00:35");
                DTimerDownload.Tick += DTimerDownload_Tick;
                DTimerDownload.Start();

                DTimerWait = new DispatcherTimer();
                DTimerWait.Interval = TimeSpan.Parse("00:00:2");
                DTimerWait.Tick += DTimerWait_Tick;
            }
            catch { }
        }

        void DTimerWait_Tick(object sender, EventArgs e)
        {
            Reset_List_Select();
            DTimerWait.Stop();
        }

        void DTimerDownload_Tick(object sender, EventArgs e)
        {
            if (bool_statusDownload == true)
                Status_Download.Text = "Welcome to Music Room ! Hope you have relax time with music!";
            Storyboard1.Begin();
        }
        private void Init_Variable()
        {
            try
            {
                MOffline = new MusicOffline();
                Init_ListOffline();
                ListDownload = new ObservableCollection<MySong>();
                MOnline = new MusicOnline();
                MOnline.LoadingSongCompleted += MOnline_LoadingSongCompleted;
                MOnline.LoadingListCompleted += MOnline_LoadingListCompleted;
                MOnline.DownloadCompleted += MOnline_DownloadCompleted;
                BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;
                TimeCurrent = TimeEnd = "";
            }
            catch { }
        }

        void MOnline_DownloadCompleted(MySong Output)
        {
            Status_Download.Text = "Download '"+ Output.Name + "' complete !";
            ListDownload.Add(Output);
        }
        private void Init_ListOffline()
        {
            //Radio_Song_Offline.IsChecked = true;
        }
        void Init_Songs(AudioTrack Item)
        {
            try
            {
                Slider_Time.Maximum = Item.Duration.TotalSeconds;
                TimeEnd = Get_String_Time(Item.Duration.Minutes, Item.Duration.Seconds);
                NameNowSong = Item.Title;
                NameNowArtist = Item.Artist;
                ImageNowSong = Item.Tag;
            }
            catch { }
        }
        #endregion

        #region Offline Music Event
        private void DTimer_Tick(object sender, EventArgs e)
        {
            try
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
            catch { }
        }
        private void Radio_Song_Offline_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List_Song_Offline.Visibility = System.Windows.Visibility.Visible;
                List_Album_Offline.Visibility = System.Windows.Visibility.Collapsed;
                List_Artist_Offline.Visibility = System.Windows.Visibility.Collapsed;

                MOffline.Load_Songs();
            }
            catch { }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                List_Song_Offline.Visibility = System.Windows.Visibility.Collapsed;
                List_Artist_Offline.Visibility = System.Windows.Visibility.Visible;
                List_Album_Offline.Visibility = System.Windows.Visibility.Collapsed;
                MOffline.Update_LibraryArtists();
            }
            catch { }
        }
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                List_Song_Offline.Visibility = System.Windows.Visibility.Collapsed;
                List_Artist_Offline.Visibility = System.Windows.Visibility.Collapsed;
                List_Album_Offline.Visibility = System.Windows.Visibility.Visible;
                MOffline.Update_LibraryAlbums();
            }
            catch { }
        }
        private void List_Song_Offline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (List_Song_Offline.SelectedIndex == -1)
                    return;
                MediaLibrary ML = new MediaLibrary();
                if (List_Song_Offline.SelectedIndex == -1)
                    return;

                Btn_Play.Content = ";";
                Add_Song_NowPlaying(MOffline.List_Current_Song[List_Song_Offline.SelectedIndex]);
                Current_NowPlaying = List_NowPlaying.Count - 1;
                PlaySong();
            }
            catch { }
            finally
            {
            }
        }
        private void MenuItem_Play_ListOffline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = List_Song_Offline.Items.IndexOf((sender as MenuItem).DataContext);
                Add_Song_NowPlaying(MOffline.List_Current_Song[selectedIndex]);

                Btn_Play.Content = ";";
                Current_NowPlaying = List_NowPlaying.Count - 1;
                PlaySong();
            }
            catch { }
        }
        private void MenuItem_Add_ListOffline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = List_Song_Offline.Items.IndexOf((sender as MenuItem).DataContext);
                Add_Song_NowPlaying(MOffline.List_Current_Song[selectedIndex]);
            }
            catch { }
        }
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
        #endregion

        #region Background Music
        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (Checkbox_Round.IsChecked == true)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    Current_NowPlaying++;
                    PlaySong();
                }
            }
        }
        private void Instance_PlayStateChanged(object sender, EventArgs e)
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
        #endregion

        #region Application Bar Envent
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
                MessageBox.Show("Music Room Ver 2.0 \r\nDeveloper Nguyễn Duy Nguyên\r\nEmail: duynguyena3win@gmail.com", "Information", MessageBoxButton.OK);
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

        #endregion     

        #region Function Helper
        private void SetProgressIndicator(string status, bool value)
        {
            try
            {
                SystemTray.ProgressIndicator.Text = status;
                SystemTray.ProgressIndicator.IsIndeterminate = value;
                SystemTray.ProgressIndicator.IsVisible = value;
            }
            catch { }
        }        
        void Remove_From_List(ObservableCollection<MySong> List_Data, int indexRemove)
        {
            try
            {
                List_Data.RemoveAt(indexRemove);
            }
            catch { }
        }
        void Add_Song_NowPlaying(Song Item)
        {
            MySong temp = new MySong(Item);
            List_NowPlaying.Add(temp);
        }
        string Get_String_Time(int min, int sec)
        {
            string temp;
            if (min <= 9)
                temp = "0" + min.ToString();
            else
                temp = min.ToString();

            if (sec <= 9)
                temp += ":0" + sec.ToString();
            else
                temp += ":" + sec.ToString();
            return temp;
        }
        void Write_XML()
        {
            try
            {
                // Write to the Isolated Storage
                ListHistory = new List<HistorySong>();
                for (int i = 0; i < List_NowPlaying.Count; i++)
                {
                    if (List_NowPlaying[i].Stream != null)
                    {
                        HistorySong HS = new HistorySong();
                        HS.CopySong(List_NowPlaying[i]);
                        HS.Current_Index = Current_NowPlaying;
                        ListHistory.Add(HS);
                    }
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
            catch { MessageBox.Show("Have some error when save history file!"); }
        }

        void Write_Download_XML()
        {
            try
            {
                // Write to the Isolated Storage
                List<DownloadSong> ListSave = new List<DownloadSong>();
                for (int i = 0; i < ListDownload.Count; i++)
                {
                    if (ListDownload[i].Stream != null)
                    {
                        DownloadSong DS = new DownloadSong(ListDownload[i]);
                        ListSave.Add(DS);
                    }
                }
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("DownloadHistory.xml", FileMode.Create))
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<DownloadSong>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            serializer.Serialize(xmlWriter, ListSave);
                        }
                    }
                }
            }
            catch { MessageBox.Show("Have some error when save file"); }
        }
        void Read_XML()
        {
            try
            {
                List_NowPlaying = new ObservableCollection<MySong>();
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("DataHistory.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<HistorySong>));

                        ListHistory = (List<HistorySong>)serializer.Deserialize(stream);

                        for (int i = 0; i < ListHistory.Count; i++)
                            List_NowPlaying.Add(new MySong(ListHistory[i]));
                        if (ListHistory.Count > 0)
                            Current_NowPlaying = ListHistory[0].Current_Index;
                    }
                }
            }
            catch
            {
                //add some code here
            }
        }
        void Read_Download_XML()
        {
            try
            {
                List<DownloadSong> ListSave = new List<DownloadSong>();
                ListDownload = new ObservableCollection<MySong>();

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("DownloadHistory.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<DownloadSong>));

                        ListSave = (List<DownloadSong>)serializer.Deserialize(stream);

                        for (int i = 0; i < ListSave.Count; i++)
                            ListDownload.Add(new MySong(ListSave[i]));
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }
        void Block_Control_Online(bool state)
        {
            try
            {
                if (state == true)
                    SetProgressIndicator("", false);
                else
                    SetProgressIndicator("Loading", true);
                Text_Search.IsEnabled = Button_Search.IsEnabled = Radio_NEW.IsEnabled = Radio_KPOP.IsEnabled = Radio_USUK.IsEnabled = Radio_VPOP.IsEnabled = state;
            }
            catch { }
        }
        int Cheack_List_OnNowPlaying(MySong Item)
        {
            try
            {
                for (int i = 0; i < List_NowPlaying.Count; i++)
                {
                    if (List_NowPlaying[i].Stream == Item.Stream)
                        return i;
                }
                return -1;
            }
            catch { return -1; }
        }
        int Cheack_List_OffNowPlaying(MySong Item)
        {
            try
            {
                for (int i = 0; i < List_NowPlaying.Count; i++)
                {
                    if (List_NowPlaying[i].OfflineSong == Item.OfflineSong)
                        return i;
                }
                return -1;
            }
            catch { return -1; }
        }
        int Add_Song_NowPlaying(MySong Item)
        {
            try
            {
                int index;
                if (Item.Stream == null)
                {
                    index = Cheack_List_OffNowPlaying(Item);
                    if (index == -1)
                    {
                        MySong temp = new MySong(Item);
                        List_NowPlaying.Add(temp);
                        return List_NowPlaying.Count - 1;
                    }
                    else
                        return index;
                }
                else
                {
                    index = Cheack_List_OnNowPlaying(Item);
                    if (index == -1)
                    {
                        MySong temp = new MySong(Item);
                        List_NowPlaying.Add(temp);
                        return List_NowPlaying.Count - 1;
                    }
                    else
                    {
                        if (Item.Path != null)
                        {
                            List_NowPlaying[index].Path = Item.Path;
                            List_NowPlaying[index].Update_Offline_Track();
                        }
                        return index;
                    }
                }
            }
            catch { return -2; }
        }
        void Init_Songs(MySong Item)
        {
            try
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
            catch { }
        }
        void PlaySong()
        {
            try
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
            catch { }
            finally
            {
                DTimerWait.Start();
            }
        }
        #endregion

        #region Envent Override Apps
    
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Write_XML();
            Write_Download_XML();
        }      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Read_XML();
                Read_Download_XML();
                if (PlayState.Playing == BackgroundAudioPlayer.Instance.PlayerState || PlayState.Paused == BackgroundAudioPlayer.Instance.PlayerState)
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

                if (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused)
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
            }
            catch { }
        }
        #endregion

        #region Online Music
        private void MOnline_LoadingSongCompleted(MySong Output)
        {
            try
            {
                Block_Control_Online(true);
                int index = Add_Song_NowPlaying(Output);
                if (index == -2)
                    return;
                Current_NowPlaying = index;
                PlaySong();
            }
            catch { }
            finally
            {
            }
        }

        void Reset_List_Select()
        {
            List_Song_Online.SelectedIndex = -1;
            List_Song_Offline.SelectedIndex = -1;
            List_Song_NowPlaying.SelectedIndex = -1;
            List_Song_Download.SelectedIndex = -1;
        }
        private void MOnline_LoadingListCompleted()
        {
            Block_Control_Online(true);
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

        private void Text_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Text_Search.Text != String.Empty)
                    Button_Search.IsEnabled = true;
                else
                    Button_Search.IsEnabled = false;
            }
            catch { }
        }

        private void List_Song_Online_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (List_Song_Online.SelectedIndex < 0 || MOnline.State == false)
                    return;
                Block_Control_Online(false);

                MOnline.GetSongSelected(List_Song_Online.SelectedIndex);
            }
            catch { }
            finally
            {
                
            }
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Block_Control_Online(false);
                MOnline.Button_Search_Click(Text_Search.Text);
            }
            catch { }
        }

        #endregion

        #region Slider of Apps
        private void Slider_Time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (e.NewValue > Slider_Time.Maximum - 1 && Slider_Time.Maximum != 0)
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
            catch { }
        }

        private void Slider_Time_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DTimer.Stop();
            }
            catch { }
        }

        private void Slider_Time_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DTimer.Start();
                int pos = Convert.ToInt32(Slider_Time.Value);
                if (Bool_Play == false)
                {
                    TimeSpan temp = new TimeSpan(pos / 3600, pos / 60, pos % 60);
                    BackgroundAudioPlayer.Instance.Position = temp;
                }

            }
            catch { }
        }
        #endregion

        #region Tool Play Music
        private void Checkbox_Reve_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch { }
        }
        private void Checkbox_Round_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkDispatcher.Update();
                MediaPlayer.IsShuffled = true;
            }
            catch { }
        }
        private void Btn_Pre_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch { }
        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    switch (BackgroundAudioPlayer.Instance.PlayerState)
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
            catch { }
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch { }
        }
        #endregion

        #region List Now Playing
        private void MenuItem_Remove_ListNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = List_Song_NowPlaying.Items.IndexOf((sender as MenuItem).DataContext);
                Remove_From_List(List_NowPlaying, selectedIndex);
            }
            catch { }
        }

        private void MenuItem_Remove_ListDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = List_Song_Download.Items.IndexOf((sender as MenuItem).DataContext);
                Remove_From_List(ListDownload, selectedIndex);
            }
            catch { }
        }

        private void MenuItem_Play_ListNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Current_NowPlaying = List_Song_NowPlaying.Items.IndexOf((sender as MenuItem).DataContext); ;
                PlaySong();
            }
            catch { }
        }

        private void MenuItem_Play_ListDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Current_NowPlaying = List_Song_Download.Items.IndexOf((sender as MenuItem).DataContext); ;
                PlaySong();
            }
            catch { }
        }

        private void List_Song_NowPlaying_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(List_Song_NowPlaying.SelectedIndex == -1)
                    return;
                Current_NowPlaying = List_Song_NowPlaying.SelectedIndex;

                PlaySong();
            }
            catch { }
            finally
            {
            }
        }

        void Remove_Song_NowPlaying(int index)
        {
            try
            {
                List_NowPlaying.RemoveAt(index);
            }
            catch { }
        }
        #endregion

        private void Text_Search_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Text_Search.Text = "";
            
        }

        private void List_Song_Download_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (List_Song_Download.SelectedIndex == -1)
                    return;
                Btn_Play.Content = ";";
                Current_NowPlaying = Add_Song_NowPlaying(ListDownload[List_Song_Download.SelectedIndex]);
                PlaySong();
            }
            catch { }
            finally
            {
            }
        }
    }
}