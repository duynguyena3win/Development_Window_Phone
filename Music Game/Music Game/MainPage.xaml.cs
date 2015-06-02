using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Music_Game.Resources;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Threading;
using DropBoxImages;
using Music_Game.Models;
using System.IO.IsolatedStorage;
using Microsoft.Live;
using System.Net;
using System.Net.Http;

namespace Music_Game
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        string sourceImage;
        
        public string SourceImage
        {
            get { return sourceImage; }
            set
            {
                sourceImage = value;
                NotifyPropertyChanged("SourceImage");
            }
        }

        DispatcherTimer DTimerShow;
        List<string> ListImages = new List<string>();

        int currentImages = 0;

        public int CurrentImages
        {
            get { return currentImages; }
            set
            {
                if (value >= 3)
                    currentImages = 0;
                else
                    currentImages = value;
            }
        }

        // Constructor
        public MainPage()
        {
            
            InitializeComponent();
            this.DataContext = this;
            
            for (int i = 0; i < 3; i++)
            {
                ListImages.Add("\\Assets\\BG_Images\\BG_Menu" + i + ".jpg");
            }
            SourceImage = ListImages[CurrentImages];
            DTimerShow = new DispatcherTimer();
            DTimerShow.Interval = new TimeSpan(0, 0, 5);
            DTimerShow.Tick += DTimerShow_Tick;
            DTimerShow.Start();
            MessageBox.Show("Excuse me! You must connect Internet to Play Game!");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Global.IsBGMusic == false)
                BG_Player.Volume = 0;
            else
                BG_Player.Volume = 1;
        }
        void DTimerShow_Tick(object sender, EventArgs e)
        {
            DTimerShow.Stop();
            SB_Hide.Begin();
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

        private void SB_Show_Completed(object sender, EventArgs e)
        {
            DTimerShow.Start();
        }

        private void SB_Hide_Completed(object sender, EventArgs e)
        {
            ++CurrentImages;
            SourceImage = ListImages[CurrentImages];
            Debug.WriteLine(SourceImage);
            SB_Show.Begin();
        }

        private void Btn_HScore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/HScorePage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Btn_HScore Click!");
            }
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Btn_Setting Click!");
            }
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            SP_MenuPage.Visibility = System.Windows.Visibility.Collapsed;
            SP_GamePage.Visibility = System.Windows.Visibility.Visible;
            ApplicationBar.IsVisible = true;
        }

        private bool IsSpaceIsAvailable(long spaceReq)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {

                long spaceAvail = store.AvailableFreeSpace;
                if (spaceReq > spaceAvail)
                {
                    return false;
                }
                return true;
            }
        }

        void WClient_Download_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e1)
        {
            string Path;
            if (e1.Error == null)
            {
                try
                {
                    string filename = "MyFile545.mp3";
                    bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);

                    if (isSpaceAvailable)
                    {
                        // Save mp3 to Isolated Storage
                        try
                        {
                            using (var isfs = new IsolatedStorageFileStream(filename,
                                                FileMode.CreateNew,
                                                IsolatedStorageFile.GetUserStoreForApplication()))
                            {

                                long fileLen = 100 * 1024;
                                byte[] b = new byte[fileLen];
                                e1.Result.Position = 300 * 1024;
                                e1.Result.Read(b, 0, b.Length);
                                isfs.Write(b, 0, b.Length);
                                isfs.Flush();
                                Path = isfs.Name;

                                //var client = new DropNetClient("8o22eryy1qg3m1z", "2timqmgbvjg1qjj");
                                //var uploaded =  client.UploadFileTask("/", "music.mp3", b);
                                
                            }
                            

                        }
                        catch (Exception ex) { }
                        
                    }
                    else
                    {
                        MessageBox.Show("Not enough to save space available to download mp3.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(e1.Error.Message);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if(MessageBox.Show("Do you want leave Music Game?", "Exit Game", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Terminate();
            }
        }
        private void Btn_SinglePlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Global.TypeGenre = "default";
                NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Btn_HScore Click!");
            }
        }

        private void Btn_ChallengeFriend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/ChallengePage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Challenge Page Click!");
            }
        }

        private void Btn_CreateQuesition_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("We are developing!");
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            SP_MenuPage.Visibility = System.Windows.Visibility.Visible;
            SP_GamePage.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = false;
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }

        private void Btn_Profile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/FacebookPage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Btn_Proflie Click!");
            }
        }
    }
}