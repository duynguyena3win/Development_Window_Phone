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
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Music_Game
{
    public partial class ResultPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        public ResultPage()
        {
            InitializeComponent();
            this.DataContext = this;
            DTimer = new DispatcherTimer();
            DTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            DTimer.Tick += DTimer_Tick;
            BG_Player.Pause();
            BG_PlayerScore.Play();
            DTimer.Start();
            frameScore = frameTime = 0;
            Result();
            TypeGame();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Global.IsBGMusic == false)
                BG_Player.Volume = 0;
            else
                BG_Player.Volume = 1;
        }

        private void TypeGame()
        {
            switch(Global.TypeGenre)
            {
                case "default":
                    break;
                case "challenge":
                    
                    break;
                default:
                    ChallengePlayer();
                    break;
            }
        }

        private void ResultChallenge()
        {
            try
            {
                NavigationService.Navigate(new Uri("/ResulChallengePage.xaml", UriKind.Relative));
            }
            catch { Debug.WriteLine("Have some error in Type Challenge !"); }
        }

        


        string status_time;

        public string Status_Time
        {
            get { return status_time; }
            set { status_time = value; OnPropertyChanged("Status_Time"); }
        }

        bool bTime = false;
        bool bScore = false;
        private void DTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (frameScore >= Global.GameScore)
                {
                    Score_Result = "Score: " + Global.GameScore;
                    frameScore = Global.GameScore;
                    bScore = true;
                }
                else
                {
                    frameScore += 1;
                    Score_Result = "Score: " + frameScore;
                }

                if (frameTime >= Global.GameTime)
                {
                    Time_Result = "Time: " + Global.GameTime;
                    frameTime = Global.GameTime;
                    bTime = true;
                }
                else
                {
                    frameTime += 1;
                    Time_Result = "Time: " + frameTime;
                }

                if (bScore && bTime)
                {
                    BG_PlayerScore.Stop();
                    BG_Player.Play();
                    DTimer.Stop();
                }
            }
            catch { }
        }

        int frameScore;
        int frameTime;
        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if(Global.TypeGenre == "challenge")
                ResultChallenge();
            try
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch
            {
                Debug.WriteLine("Error in Main Page Click!");
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            Btn_OK_Click(null, null);
        }
        private void Btn_Share_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("We are developing!");
        }

        string status_Result;
        
        public string Status_Result
        {
            get { return status_Result; }
            set { status_Result = value; OnPropertyChanged("Status_Result"); }
        }

        string score_Result;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Score_Result
        {
            get { return score_Result; }
            set { score_Result = value; OnPropertyChanged("Score_Result"); }
        }

        DispatcherTimer DTimer;
        void Result()
        {
            try
            {
                if (Global.GameScore >= 90)
                {
                    if (Global.GameTime < 20)
                    {
                        Status_Result = "You is hacker - too smast !!!";
                        Image_Cup = @"/Assets/MedalGame/Hack_medal.jpg";
                    }
                    else if (Global.GameTime < 30)
                    {
                        Status_Result = "Your Knowledge of music is PERFECR !!!";
                        Image_Cup = @"/Assets/MedalGame/Diamon_Medal.jpg";
                    }
                    else if (Global.GameTime < 60)
                    {
                        Status_Result = "Your Knowledge of music is GREAT !!!";
                        Image_Cup = @"/Assets/MedalGame/Gold_Medal.jpg";
                    }
                    else if (Global.GameTime < 100)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/MedalGame/Sliver_Medal.jpg";
                    }
                    else if (Global.GameTime < 200)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/MedalGame/Brozon_Medal.jpg";
                    }
                    else
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/MedalGame/Bad_Medal.png";
                    }
                }
                else if (Global.GameScore >= 60 || Global.GameScore < 89)
                {
                    if (Global.GameTime < 30)
                    {
                        Status_Result = "Your Knowledge of music is GREAT !!!";
                        Image_Cup = @"/Assets/MedalGame/Gold_Medal.jpg";
                    }
                    else if (Global.GameTime < 60)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/MedalGame/Sliver_Medal.jpg";
                    }
                    else if (Global.GameTime < 100)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/MedalGame/Brozon_Medal.jpg";
                    }
                    else if (Global.GameTime < 200)
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/MedalGame/Bad_Medal.jpg";
                    }
                    else
                    {
                        Status_Result = "You need listen music more and more !!!";
                        Image_Cup = @"/Assets/MedalGame/Listen_Icon.jpg";
                    }
                }
                else if (Global.GameScore > 30 || Global.GameScore < 59)
                {
                    if (Global.GameTime < 30)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/MedalGame/Sliver_Medal.jpg";
                    }
                    else if (Global.GameTime < 60)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/MedalGame/Brozon_Medal.jpg";
                    }
                    else if (Global.GameTime < 100)
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/MedalGame/Bad_Medal.png";
                    }
                    else
                    {
                        Status_Result = "You need listen music more and more !!!";
                        Image_Cup = @"/Assets/MedalGame/Listen_Icon.jpg";
                    }
                }
                else
                {
                    Status_Result = "Let try again ! I thinks you can have good score!";
                    Image_Cup = @"/Assets/MedalGame/Try_Medal.jpg";
                }
            }
            catch { }
        }

        string time_Result;

        public string Time_Result
        {
            get { return time_Result; }
            set { time_Result = value; OnPropertyChanged("Time_Result"); }
        }

        string image_Cup;

        public string Image_Cup
        {
            get { return image_Cup; }
            set { image_Cup = value; OnPropertyChanged("Image_Cup"); }
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }
        private async void ChallengePlayer()
        {
            Dictionary<string, string> mySend = new Dictionary<string, string>();
            mySend.Add("idFaceSend", Global.CurrentUser.IdFacebook);
            mySend.Add("nameSend", Global.CurrentUser.Name);
            mySend.Add("linkFaceSend", Global.CurrentUser.LinkFacebook);
            mySend.Add("idFaceReceive", Global.IdFaceChallenger);
            mySend.Add("score", Global.GameScore.ToString());
            mySend.Add("time", Global.GameTime.ToString());
            string listQuestion = string.Empty;
            for (int i = 0; i < Global.ListQuestion.Count; i++)
            {
                listQuestion += Global.ListQuestion[i].IdQuestion + ",";
            }
            mySend.Add("listIdQuestion", listQuestion);
            if (await ConnectServiceHelper.ChallengePlayer(mySend))
                MessageBox.Show("Challenge Success!\r\nWait Result form Player who you challenge!", "Challenge Information", MessageBoxButton.OK);
            else
                MessageBox.Show("Challenge Fail!\r\nHave some error! Do it again, sorry!", "Challenge Information", MessageBoxButton.OK);
        }
    }
}