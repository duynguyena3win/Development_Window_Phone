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

namespace Rank_Nation_Football_Team
{

    public partial class Nation_Info : PhoneApplicationPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        NationTeam team;

        string rank;

        public string Rank
        {
            get { return rank; }
            set { rank = value; NotifyPropertyChanged("Rank"); }
        }
        public NationTeam Team_Binding
        {
            get { return team; }
            set { team = value; NotifyPropertyChanged("Team_Binding"); }
        }
        public Nation_Info()
        {
            InitializeComponent();
            this.DataContext = this;
            Team_Binding = Global.Current_Team;
            Rank = "Ranking: " + Team_Binding.Rank;
            Init_AppBar();
        }
        void Init_AppBar()
        {
            try
            {
                ApplicationBar = new ApplicationBar();

                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Opacity = 0.6f;
                ApplicationBar.IsVisible = true;
                ApplicationBar.IsMenuEnabled = true;

                ApplicationBarIconButton Button_Info = new ApplicationBarIconButton();
                Button_Info.IconUri = new Uri("/Assets/about.png", UriKind.Relative);
                Button_Info.Text = "Information";
                Button_Info.Click += Button_Info_Click;
                ApplicationBar.Buttons.Add(Button_Info);

            }
            catch { }
        }

        private void Button_Info_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
            }
            catch { }
        }
    }
}