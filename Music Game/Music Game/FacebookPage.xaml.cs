using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using Music_Game.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Music_Game
{
    public partial class FacebookPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public FacebookPage()
        {
            InitializeComponent();
            this.DataContext = this;
            list_Challenge = new ObservableCollection<ChallengeDAO>();
        }

        private void Btn_LogInFacebook_SessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
        {
            try
            {
                if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Opened)
                {
                    Storyboard_MoveOut.Begin();
                }
                else if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Closed)
                {
                    Storyboard_MoveIn.Begin();
                }
            }
            catch { }
        }
        
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LoginComplete()
        {
            ApplicationBar.IsVisible = true;
            Global.AccessToken = Btn_LogInFacebook.CurrentSession.AccessToken;
            image.Visibility = System.Windows.Visibility.Visible;
            SB_Rotation.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(Global.IsBGMusic == false)
                BG_Player.Volume = 0;
            else
                BG_Player.Volume = 1;
        }

        void CreateNewUser()
        {
            if (Btn_LogInFacebook.CurrentUser != null)
            {
                Global.CurrentUser = new UserMusicDAO();
                Global.CurrentUser.Name = Btn_LogInFacebook.CurrentUser.Name;
                Global.CurrentUser.IdFacebook = Btn_LogInFacebook.CurrentUser.Id;
                Global.CurrentUser.LinkFacebook = Btn_LogInFacebook.CurrentUser.Link;
                Global.CurrentUser.UserImage = Btn_LogInFacebook.CurrentUser.ProfilePictureUrl.OriginalString;
                Global.AccessToken = Btn_LogInFacebook.CurrentSession.AccessToken;
                ConnectServiceHelper.CreateUser(Btn_LogInFacebook.CurrentUser.Id, Btn_LogInFacebook.CurrentUser.Link, Btn_LogInFacebook.CurrentUser.Name, Global.AccessToken);
                LoadListChallenge();
            }
        }
        ObservableCollection<ChallengeDAO> list_Challenge;

        public ObservableCollection<ChallengeDAO> List_Challenge
        {
            get { return list_Challenge; }
            set
            {
                list_Challenge = value;
                image.Visibility = System.Windows.Visibility.Collapsed;
                SB_Rotation.Stop();
                if(list_Challenge.Count == 0)
                    Text_Info.Visibility = System.Windows.Visibility.Visible;
                else
                    Text_Info.Visibility = System.Windows.Visibility.Collapsed;
                
            }
        }
        private async void LoadListChallenge()
        {
            try
            {
                image.Visibility = System.Windows.Visibility.Visible;
                SB_Rotation.Begin();

                List_Challenge = await ConnectServiceHelper.GetChallenges(Global.AccessToken);
                ListChallenge.DataContext = List_Challenge;
            }
            catch { }
        }

        private void ListChallenge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListChallenge.SelectedIndex >= 0)
                {
                    Global.CurrentChallenge = List_Challenge[ListChallenge.SelectedIndex];
                    if (MessageBox.Show("You will play Challenge of " + Global.CurrentChallenge.NameSend + " !\r\nAre you ready?", "Challenge", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        return;
                    Global.TypeGenre = "challenge";
                    NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
                }
                ListChallenge.SelectedIndex = -1;
            }
            catch { }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("You will play this Challenge!\r\nAre you ready?", "Challenge", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
                int selectedIndex = ListChallenge.Items.IndexOf((sender as MenuItem).DataContext);
                if(selectedIndex >= 0)
                {
                    Global.CurrentChallenge = List_Challenge[selectedIndex];
                    if (MessageBox.Show("You will play Challenge of " + Global.CurrentChallenge.NameSend + " !\r\nAre you ready?", "Challenge", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        return;
                    Global.TypeGenre = "challenge";
                    NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
                }
            }
            catch
            {

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = ListChallenge.Items.IndexOf((sender as MenuItem).DataContext);
            }
            catch
            {

            }
        }

        private void SB_Rotation_Completed(object sender, EventArgs e)
        {
            image.Visibility = System.Windows.Visibility.Visible;
            SB_Rotation.Begin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Text_Info.Visibility = System.Windows.Visibility.Collapsed;
                LoadListChallenge();
            }
            catch { }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Log In by Facebook Account!\r\nIf Game cant load Your Challenge, let out User Profile and open it again!","User Profile", MessageBoxButton.OK);
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }

        private void Storyboard_MoveOut_Completed(object sender, EventArgs e)
        {
            LoginComplete();
        }

        private async void Storyboard_MoveIn_Completed(object sender, EventArgs e)
        {
            Global.AccessToken = null;
            await new WebBrowser().ClearCookiesAsync();
        }

        private void Btn_LogInFacebook_UserInfoChanged(object sender, Facebook.Client.Controls.UserInfoChangedEventArgs e)
        {
            CreateNewUser();
        }
    }
}