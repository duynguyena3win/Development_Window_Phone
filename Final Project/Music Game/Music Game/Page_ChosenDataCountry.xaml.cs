using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Music_Game
{
    public partial class Page_ChosenDataCountry : PhoneApplicationPage
    {
        public Page_ChosenDataCountry()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        
        private void List_Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List_Country.SelectedIndex == -1)
                return;
            try
            {
                Global.Current_Type = List_Country.SelectedIndex;
                Chosen_Country.Visibility = System.Windows.Visibility.Collapsed;
                Chosen_Task.Visibility = System.Windows.Visibility.Visible;
                Text_Type.Text = Global.List_Data[List_Country.SelectedIndex].NameType;
                List_Task.ItemsSource = Global.List_Data[List_Country.SelectedIndex].List_Task;
            }
            catch { }
            finally
            {
                List_Country.SelectedIndex = -1;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void List_Task_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List_Task.SelectedIndex == -1)
                return;
            try
            {
                Global.Current_Task = List_Task.SelectedIndex;
                if (MessageBox.Show("Are you ready for game start?", "Game Ready", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Global.LoadGame(Global.List_Data[Global.Current_Type].List_Task[Global.Current_Task].List_Question);
                    NavigationService.Navigate(new Uri("/GameStart.xaml", UriKind.Relative));
                }
                else
                {

                }
            }
            catch { }
            finally
            {
                List_Task.SelectedIndex = -1;
            }
        }

        private void List_Country_Loaded(object sender, RoutedEventArgs e)
        {
            List_Country.ItemsSource = Global.Load_Data_Game();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Chosen_Task.Visibility = System.Windows.Visibility.Collapsed;
                Chosen_Country.Visibility = System.Windows.Visibility.Visible;
            }
            catch { }
        }

        private void MPlayer_Back_MediaEnded(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Play();
        }

        private void MPlayer_Button_MediaEnded(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Play();
            MPlayer_Button.Stop();
        }

        private void MPlayer_Button_MediaOpened(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Pause();
            MPlayer_Button.Play();
        }
    }
}