using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Music_Game.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Music_Game
{
    public partial class ChallengePage : PhoneApplicationPage, INotifyPropertyChanged
    {
        public ChallengePage()
        {
            InitializeComponent();
            ListGenre.Add(new MusicGenre("V - POP", @"\Assets\HubTile\Vpop.jpg"));
            ListGenre.Add(new MusicGenre("K - POP", @"\Assets\HubTile\KPop.jpg"));
            ListGenre.Add(new MusicGenre("US - UK", @"\Assets\HubTile\UKUS.jpg"));
            List_TypeSong.DataContext = ListGenre;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowGame();
        }
        void ShowGame()
        {
            if (Global.CurrentUser != null)
            {
                Information = "Chosen Player whose you want challenger!";
                Grid_Player.Visibility = System.Windows.Visibility.Visible;
                Btn_Next_Click(null, null);
                image.Visibility = System.Windows.Visibility.Visible;
                SB_Rotation.Begin();
				Text_Info.Text = "Chosen Player ! ! !";
                Btn_Login.Visibility = System.Windows.Visibility.Collapsed;
            }
            Information = "Login to Game by Facebook Account!";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/FacebookPage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in UseProfile Page Click!");
            }
        }

        ObservableCollection<UserMusicDAO> listPlayer;

        public ObservableCollection<UserMusicDAO> ListPlayer
        {
            get { return listPlayer; }
            set {
                listPlayer = value;
                image.Visibility = System.Windows.Visibility.Collapsed;
                SB_Rotation.Stop();
            }
        }
        int currentPage = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        private async void Btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                CurrentPage--;
                if (CurrentPage < 0)
                {
                    Btn_Prev.IsEnabled = false;
                    CurrentPage = 0;
                    return;
                }
                else
                {
                    image.Visibility = System.Windows.Visibility.Visible;
                    SB_Rotation.Begin();
                    Btn_Next.IsEnabled = true;
                    var list = await ConnectServiceHelper.LoadPlayer(CurrentPage);
                    if (list != null)
                    {
                        ListPlayer = list;
                        ListBox_Player.DataContext = ListPlayer;
                    }
                    else
                    {
                        Btn_Prev.IsEnabled = false;
                        image.Visibility = System.Windows.Visibility.Collapsed;
                        SB_Rotation.Stop();
                    }
                }
            }
            catch { }
        }

        private async void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                image.Visibility = System.Windows.Visibility.Visible;
                SB_Rotation.Begin();
                CurrentPage++;
                var list = await ConnectServiceHelper.LoadPlayer(CurrentPage);
                if (list == null)
                {
                    CurrentPage--;
                    Btn_Next.IsEnabled = false;
                    image.Visibility = System.Windows.Visibility.Collapsed;
                    SB_Rotation.Stop();
                }
                else
                {
                    ListPlayer = list;
                    ListBox_Player.DataContext = ListPlayer;
                    Btn_Prev.IsEnabled = true;
                }
            }
            catch { }
        }

        private void ListBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ListBox_Player.SelectedIndex >= 0)
                {
                    Global.IdFaceChallenger = ListPlayer[ListBox_Player.SelectedIndex].IdFacebook;
                    if (Global.CurrentUser.IdFacebook == Global.IdFaceChallenger)
                    {
                        Global.IdFaceChallenger = null;
                        MessageBox.Show("You can't challenge yoursefl!", "Information", MessageBoxButton.OK);
                    }
                    else
                    {
                        Information = "Chosen Genre Music!";
                        Grid_Player.Visibility = System.Windows.Visibility.Collapsed;
						Text_Info.Text = "Chosen Genre ! ! !";
                        List_TypeSong.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            catch
            {
                Debug.WriteLine("Error in Selected Change in ListBox Player!");
            }
            finally
            {
                ListBox_Player.SelectedIndex = -1;
            }
        }

        List<MusicGenre> ListGenre = new List<MusicGenre>();
        private void List_Song_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (List_TypeSong.SelectedIndex >= 0)
                {
                    switch (List_TypeSong.SelectedIndex)
                    {
                        case 0:
                            Global.TypeGenre = "Vietnam";
                            break;
                        case 1:
                            Global.TypeGenre = "Korea";
                            break;
                        case 2:
                            Global.TypeGenre = "USUK";
                            break;
                        default:
                            Global.TypeGenre = "default";
                            break;
                    }

                    try
                    {
                        NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
                    }
                    catch
                    {
                        Debug.WriteLine("Error in Game Page Click!");
                    }
                }
                List_TypeSong.SelectedIndex = -1;
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Random rand = new Random();
                if (ListPlayer.Count >= 0)
                {
                    Global.IdFaceChallenger = ListPlayer[rand.Next(0, ListPlayer.Count - 1)].IdFacebook;
                }
            }
            catch { }
        }

        private void SB_Rotation_Completed(object sender, EventArgs e)
        {
            image.Visibility = System.Windows.Visibility.Visible;
            SB_Rotation.Begin();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Information, "Challenge", MessageBoxButton.OK);
        }

        public string Information { get; set; }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in UseProfile Page Click!");
            }
        }
    }
    
}