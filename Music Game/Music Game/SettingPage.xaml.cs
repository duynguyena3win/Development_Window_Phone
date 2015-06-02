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
    public partial class SettingPage : PhoneApplicationPage
    {
        public SettingPage()
        {
            InitializeComponent();
            Slider_Sound.Value = Global.GameVolume * 10;
            Slider_Level.Value = Global.GameLevel * 10;
            if (Global.IsBGMusic == false)
                BG_Player.Volume = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Btn_Checked.IsChecked = Global.IsBGMusic;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Slider_Sound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global.GameVolume = Slider_Sound.Value / Slider_Sound.Maximum;
        }

        private void Slider_Level_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Global.GameLevel = Slider_Level.Value / Slider_Level.Maximum;
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Global.IsBGMusic = true;
            if(BG_Player != null)
                BG_Player.Volume = 1;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Global.IsBGMusic = false;
            if (BG_Player != null)
                BG_Player.Volume = 0;
        }
    }
}