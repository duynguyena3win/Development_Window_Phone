using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Naruto_Quiz.Resources;

namespace Naruto_Quiz
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Storyboard_Visible.Begin();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void StackPanel_Genin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Global.Name_Exam = "Genin";
            Global.Load_10_Question(2);
            NavigationService.Navigate(new Uri("/Question_Page.xaml", UriKind.Relative));
        }

        private void StackPanel_Chunin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Global.Name_Exam = "Chunin";
            Global.Load_10_Question(3);
            NavigationService.Navigate(new Uri("/Question_Page.xaml", UriKind.Relative));
        }

        private void StackPanel_Jonin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
             Global.Name_Exam = "Jonin";
             Global.Load_10_Question(4);
             NavigationService.Navigate(new Uri("/Question_Page.xaml", UriKind.Relative));
        }

        private void StackPanel_Hokage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
             Global.Name_Exam = "Hokage";
             Global.Load_10_Question(5);
             NavigationService.Navigate(new Uri("/Question_Page.xaml", UriKind.Relative));
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        void Collapsed_Control()
        {
            Menu.Visibility = System.Windows.Visibility.Collapsed;
            Help.Visibility = System.Windows.Visibility.Collapsed;
            PlayGame.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Collapsed_Control();
            PlayGame.Visibility = System.Windows.Visibility.Visible;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Collapsed_Control();
            Menu.Visibility = System.Windows.Visibility.Visible;
            if(Global.Current_Questions != null)
                Global.Current_Questions.Clear();
            Global.F_Score.Reset_Score();
            Global.Current_Index = -1;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Collapsed_Control();
            Help.Visibility = System.Windows.Visibility.Visible;
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