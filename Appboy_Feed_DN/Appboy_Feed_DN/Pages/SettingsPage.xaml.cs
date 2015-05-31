using AppboyPlatform.Phone;
using Microsoft.Phone.Controls;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows;
using Appboy_Feed_DN.AppboyClasses;
using AppboyUI.Phone;

namespace Appboy_Feed_DN.Pages
{
  public partial class SettingsPage : PhoneApplicationPage {
    public SettingsPage() {
      InitializeComponent();
    }

    protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
      base.OnNavigatedTo(e);
      ToastEnabledCheckBox.IsChecked = Appboy.SharedInstance.PushManager.ToastOptInStatus ?? false;
    }

    private void Back_Click(object sender, System.EventArgs e) {
      NavigationService.GoBack();
    }

    private void ToastEnabledCheckBox_Click(object sender, RoutedEventArgs e) {
      if (ToastEnabledCheckBox.IsChecked == true) {
        Appboy.SharedInstance.PushManager.ToastOptInStatus = true;
      } else {
        Appboy.SharedInstance.PushManager.ToastOptInStatus = false;
      }
    }
  }
}