using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Total_Feed
{
    public partial class List_Item_Page : PhoneApplicationPage, INotifyPropertyChanged
    {
        public List_Item_Page()
        {
            InitializeComponent();
            this.DataContext = this;
            List_Binding = App.Items[App.Current_Character].List_Image;
        }

        ObservableCollection<ItemView> list_Binding = new ObservableCollection<ItemView>();

        public ObservableCollection<ItemView> List_Binding
        {
            get { return list_Binding; }
            set { list_Binding = value; NotifyPropertyChanged("List_Binding"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (List_Image.SelectedIndex < 0)
                return;
            App.Item_Current = App.Items[App.Current_Character].List_Image[List_Image.SelectedIndex];
            App.Bool_Back = true;
            NavigationService.GoBack(); //.Navigate(new Uri("/Background.xaml", UriKind.Relative));
            
        }
    }
}