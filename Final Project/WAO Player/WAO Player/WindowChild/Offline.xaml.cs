using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.ComponentModel;
using WAO_Player.Class;

namespace WAO_Player.Window_Child
{
    public partial class Offline : PhoneApplicationPage, INotifyPropertyChanged
    {
        
        public Offline()
        {
            this.DataContext = this;
            InitializeComponent();
            ListBox_Song LBS = new ListBox_Song();
            LBS.LoadData();
            List_Song_Offline.ItemsSource = LBS.Items;
            //Get_List_Song();
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

    }
}