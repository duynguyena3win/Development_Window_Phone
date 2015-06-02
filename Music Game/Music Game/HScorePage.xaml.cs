using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Music_Game.Models;
using System.Collections.ObjectModel;

namespace Music_Game
{
    public partial class HScorePage : PhoneApplicationPage, INotifyPropertyChanged
    {
        ObservableCollection<UserMusicDAO> listUser;

        public ObservableCollection<UserMusicDAO> ListUser
        {
            get { return listUser; }
            set
            {
                listUser = value;
                NotifyPropertyChanged("ListUser");
            }
        }

        public HScorePage()
        {
            InitializeComponent();
            this.DataContext = this;
            
            ListUser = new ObservableCollection<UserMusicDAO>();
            LoadTopPlayer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Global.IsBGMusic == false)
                BG_Player.Volume = 0;
            else
                BG_Player.Volume = 1;
        }

        private async void LoadTopPlayer()
        {
            ListUser = await ConnectServiceHelper.LoadTopScore();
            if (ListUser != null)
            {
                ListBox_Score.DataContext = ListUser;
            }
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

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            ApplicationBarIconButton_Click(null, null);
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }
    }
}