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
using System.ComponentModel;

namespace Naruto_Screen
{
    public partial class Feed_Naruto : PhoneApplicationPage
    {
        public Feed_Naruto()
        {
            InitializeComponent();
            this.DataContext = this;
            Init_Timer();
            Source_Image = Name + Current_Image.ToString() + ".jpg";
            Current_Image++;
            Storyboard_Visible.Begin();
        }

        DispatcherTimer Timer;
        int dem = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        // Constructor

        string Name = @"Assets\Images\Dota2";
        string source_Image;
        

        public int Current_Image
        {
            get { return App.current_Image; }
            set
            {
                if (value > 3)
                    App.current_Image = 0;
                else
                    App.current_Image = value;
            }
        }
        public string Source_Image
        {
            get { return source_Image; }
            set { source_Image = value; OnPropertyChanged("Source_Image"); }
        }
        
        void Init_Timer()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            dem++;
            if (dem % 10 == 0 && dem != 0)
            {
                Source_Image = Name + Current_Image.ToString() + ".jpg";
                Current_Image++;
                Storyboard_Visible.Begin();
                dem = 0;
            }
            if (dem % 6 == 0 && dem != 0)
            {
                Storyboard_Hide.Begin();
            }
        }
        
        private void Feed_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FeedPage.xaml", UriKind.Relative));
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FeedbackPage.xaml", UriKind.Relative));
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dota 2 Information \r\nDeveloper Nguyễn Duy Nguyên\r\nEmail: duynguyena.it.khtn@gmail.com", "Information", MessageBoxButton.OK);
        }

        private void Misc_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MiscPage.xaml", UriKind.Relative));
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingsPage.xaml", UriKind.Relative));
        }

        private void Slideups_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SlideupPage.xaml", UriKind.Relative));
        }
    }
}