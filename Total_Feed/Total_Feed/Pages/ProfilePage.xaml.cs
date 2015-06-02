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
using AppboyPlatform.PCL.Models;

namespace Total_Feed.Pages
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Appboy.SharedInstance.AppboyUser.FirstName = FirstNameTextBox.Text;
            Appboy.SharedInstance.AppboyUser.LastName = LastNameTextBox.Text;
            Appboy.SharedInstance.AppboyUser.Email = EmailTextBox.Text;
            Appboy.SharedInstance.AppboyUser.Bio = BioTextBox.Text;
            Appboy.SharedInstance.AppboyUser.PhoneNumber = PhoneNumberTextBox.Text;
            if (GenderMale.IsChecked ?? false)
            {
                Appboy.SharedInstance.AppboyUser.Gender = Gender.Male;
            }
            if (GenderFemale.IsChecked ?? false)
            {
                Appboy.SharedInstance.AppboyUser.Gender = Gender.Female;
            }
            Appboy.SharedInstance.AppboyUser.SetCustomAttribute("FavoriteColor", FavoriteColorTextBox.Text);
            NavigationService.GoBack();
        }
    }
}