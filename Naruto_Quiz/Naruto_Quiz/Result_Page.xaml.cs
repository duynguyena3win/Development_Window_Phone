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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace Naruto_Quiz
{
    public partial class Result_Page : PhoneApplicationPage, INotifyPropertyChanged
    {
        DispatcherTimer Timer_PlaySound;
        DispatcherTimer Timer_Score;
        bool Bool_Animation = true;
        
        public Result_Page()
        {
            InitializeComponent();
            this.DataContext = this;
            Init_Timer();
            Text_Score.Text = "0";
            Text_Times.Text = "0";
            Medal_Player();
            Storyboard_Visible.Begin();
            Storyboard_Image.Begin();
        }

        private void Init_Timer()
        {
            Timer_PlaySound = new DispatcherTimer();
            Timer_PlaySound.Interval = new TimeSpan(0, 0, 1);

            Timer_Score = new DispatcherTimer();
            Timer_Score.Interval = new TimeSpan(0, 0, 0, 0, 150);
            Timer_Score.Tick += Timer_Score_Tick;
        }

        void Timer_Score_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Text_Score.Text) != Global.F_Score.Total_Score)
                Text_Score.Text = (Convert.ToInt32(Text_Score.Text) + 1).ToString();

            if (Convert.ToInt32(Text_Times.Text) != Global.F_Score.Total_Time)
                Text_Times.Text = (Convert.ToInt32(Text_Times.Text) + 1).ToString();

            if (Convert.ToInt32(Text_Score.Text) == Global.F_Score.Total_Score && Convert.ToInt32(Text_Times.Text) == Global.F_Score.Total_Time)
            {
                Player_Sound.Stop();
                Timer_Score.Stop();
                Bool_Animation = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Bool_Back = true;
        }
        void Medal_Player()
        {
            try
            {
                //DTimer.Stop();
                //MPlayer.Stop();
                if (Global.F_Score.Total_Score >= 90)
                {
                    if (Global.F_Score.Total_Time < 20 && Global.Level >= 4)
                    {
                        Text_Medal.Text = "Very Perfect - You are Super Shinobi";
                        Image_Source = @"/Assets/Medal_Naruto/Super_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 30 && Global.Level >= 4)
                    {
                        Text_Medal.Text = "Perfect - You can become Hokage";
                        Image_Source = @"/Assets/Medal_Naruto/Kage_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 60 && Global.Level >= 3)
                    {
                        Text_Medal.Text = "Great - You can become Jonin !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Jonin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 100 && Global.Level >= 2)
                    {
                        Text_Medal.Text = "Good - Chunin of Shinobi !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Chunin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 200 && Global.Level <= 1)
                    {
                        Text_Medal.Text = "You are Genin ! !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Genin_Logo.jpg";
                    }
                    else
                    {
                        Text_Medal.Text = "Let try again! I belive you ! !!!";
                        Image_Source = @"/Assets/ImageGame/Bad_Medal.png";
                    }
                }
                else if (Global.F_Score.Total_Score >= 60 && Global.F_Score.Total_Score < 89)
                {
                    if (Global.F_Score.Total_Time < 30)
                    {
                        Text_Medal.Text = "Great - You can become Jonin !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Jonin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 60)
                    {
                        Text_Medal.Text = "Good - Chunin of Shinobi !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Chunin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 100)
                    {
                        Text_Medal.Text = "You are Genin ! !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Genin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 200)
                    {
                        Text_Medal.Text = "Let try again! I belive you ! !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Bad_Logo.jpg";
                    }
                    else
                    {
                        Text_Medal.Text = "You need listen music more and more !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Academy Student_Logo.jpg";
                    }
                }
                else if (Global.F_Score.Total_Score > 30 && Global.F_Score.Total_Score < 59)
                {
                    if (Global.F_Score.Total_Time < 30)
                    {
                        Text_Medal.Text = "Good - Chunin of Shinobi !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Chunin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 60)
                    {
                        Text_Medal.Text = "You are Genin ! !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Genin_Logo.jpg";
                    }
                    else if (Global.F_Score.Total_Time < 100)
                    {
                        Text_Medal.Text = "Let try again! I belive you ! !!!";
                        Image_Source = @"/Assets/Medal_Naruto/Bad_Logo.jpg";
                    }
                    else
                    {
                        Text_Medal.Text = "You need learn more !!!, Academy Student";
                        Image_Source = @"/Assets/Medal_Naruto/Academy Student_Logo.jpg";
                    }
                }
                else
                {
                    Text_Medal.Text = "Let try again ! I thinks you can have good score!";
                    Image_Source = @"/Assets/Medal_Naruto/Try_Logo.jpg";
                }

            }
            catch { }
        }
        string image_Source;
        public string Image_Source
        {
            get { return image_Source; }
            set { image_Source = value; OnPropertyChanged("Image_Source"); }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Global.F_Score.Reset_Score();
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Update Late!");
        }
        private void Player_Sound_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (Bool_Animation == true)
            {
                Player_Sound.Play();
            }
        }

        private void Storyboard_Image_Completed(object sender, EventArgs e)
        {
            Storyboard_Image.Begin();
        }

        private void Storyboard_Visible_Completed(object sender, EventArgs e)
        {
            Timer_Score.Start();
            Border_Image.Opacity = 1;
            Storyboard_Rotation.Begin();
            Player_Sound.Play();
            Timer_PlaySound.Stop();
        }
    }
}