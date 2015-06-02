using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace Naruto_Screen
{
    public partial class List_Character : PhoneApplicationPage
    {
        public List_Character()
        {
            InitializeComponent();
            List_Characters.ItemsSource = App.Items;
        }
        private void List_Characters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Current_Character = List_Characters.SelectedIndex;
            try
            {
                //Total_Feed.MainPage Page = new Total_Feed.MainPage();
                NavigationService.Navigate(new Uri(@"\List_Item_Page.xaml", UriKind.Relative));
            }
            catch { }
        }
    }
}