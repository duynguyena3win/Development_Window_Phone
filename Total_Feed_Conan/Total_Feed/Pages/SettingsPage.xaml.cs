using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppboyPlatform.Phone;

namespace Total_Feed.Pages
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ToastEnabledCheckBox.IsChecked = Appboy.SharedInstance.PushManager.ToastOptInStatus ?? false;
        }

        private void Back_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ToastEnabledCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ToastEnabledCheckBox.IsChecked == true)
            {
                Appboy.SharedInstance.PushManager.ToastOptInStatus = true;
            }
            else
            {
                Appboy.SharedInstance.PushManager.ToastOptInStatus = false;
            }
        }
    }
}