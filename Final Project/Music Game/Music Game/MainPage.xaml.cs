using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Music_Game.Resources;
using Nokia.Music;
using Windows.System;
using Nokia.Music.Tasks;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
namespace Music_Game
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private const string PROD_FILE = "XML/Data.xml";
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        

        void Get_Information()
        {

        }
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MPlayer_Button.Play();
            Setting.Visibility = System.Windows.Visibility.Collapsed;
            MenuGame.Visibility = System.Windows.Visibility.Visible;
        }

        private void Btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            MPlayer_Button.Play();
            MenuGame.Visibility = System.Windows.Visibility.Collapsed;
            Setting.Visibility = System.Windows.Visibility.Visible;
        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page_ChosenDataCountry.xaml", UriKind.Relative));
        }

        private void Btn_Help_Click(object sender, RoutedEventArgs e)
        {
            MPlayer_Button.Play();
            Help.Visibility = System.Windows.Visibility.Visible;
            MenuGame.Visibility = System.Windows.Visibility.Collapsed;
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want exit?","Exit Game",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MessageBox.Show("Thanks you play my game!\n\rLet rate my game, i will update more task in new verison!", "Exit Game", MessageBoxButton.OK);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MPlayer_Button.Play();
            MenuGame.Visibility = System.Windows.Visibility.Visible;
            Help.Visibility = System.Windows.Visibility.Collapsed;
        }
        bool bool_Playing = true;
        private void Image_Volume_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BitmapImage bm;
            if (bool_Playing == true)
            {
                MPlayer_Back.Volume = 0;
                bool_Playing = false;
                bm = new BitmapImage(new Uri("/Assets/ImageGame/Volume_Mute.png", UriKind.Relative));
                Image_Volume.Source = bm;
            }
            else
            {
                MPlayer_Back.Volume = 1;
                bool_Playing = true;
                bm = new BitmapImage(new Uri("/Assets/ImageGame/Volume_On.png", UriKind.Relative));
                Image_Volume.Source = bm;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Source = new Uri("/Assets/Sound/Background_Music.mp3", UriKind.Relative);
            MPlayer_Back.Play();
            myStoryboard.Begin();
        }

        private void MPlayer_Back_MediaEnded(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Play();
        }

        private void MPlayer_Button_MediaOpened(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Pause();
            MPlayer_Button.Play();
        }

        private void MPlayer_Button_MediaEnded(object sender, RoutedEventArgs e)
        {
            MPlayer_Back.Play();
            MPlayer_Button.Stop();
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