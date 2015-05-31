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
using HtmlAgilityPack;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using DotNetApp.Utilities;
using System.Diagnostics;

namespace Music_Game
{
    public partial class GameStart : PhoneApplicationPage, INotifyPropertyChanged
    {
        string str_Question;
        //MediaElement MPlayer;
        WebClient WClient_MoreInfo;
        string StreamUri;
        void Init_WebClient()
        {
            WClient_MoreInfo = new WebClient();
            WClient_MoreInfo.DownloadStringCompleted += WClient_MoreInfo_DownloadStringCompleted;
        }

        void WClient_MoreInfo_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                StreamUri = GetStreamFile(e.Result);
                Play_Music();
            }
            catch { }
        }

        public string GetStreamFile(string Html)
        {
            try
            {
                string linkStream;
                HtmlDocument wap = new HtmlDocument();
                wap.LoadHtml(Html);
                //Lấy Stream
                try
                {
                    HtmlNode stream = wap.DocumentNode.SelectSingleNode("//div[@class='download']");
                    linkStream = stream.ChildNodes[1].ChildNodes[1].GetAttributeValue("href", "");
                }
                catch { linkStream = null; }
                return linkStream;
            }
            catch { return null; };
        }

        public string Str_Question
        {
            get { return str_Question; }
            set { str_Question = value; OnPropertyChanged("Str_Question"); }
        }

        string a;

        public void Play_Music()
        {
            Debug.WriteLine(StreamUri);
            MPlayer.Source = new Uri("http://download.a2.nixcdn.com/0f8c3baafd3f4415c1a3eb9d46ef8cc5/54759f49/NhacCuaTui177/AnhConNoEm-DamVinhHung_3b9jr.mp3", UriKind.Absolute);
            MPlayer.Position = new TimeSpan(0, 0, 0);
            DTimer_Position.Start();
            MPlayer.Volume = 1.0f;
        }
        public string A
        {
            get { return a; }
            set { a = value; OnPropertyChanged("A"); }
        }
        string b;

        public string B
        {
            get { return b; }
            set { b = value; OnPropertyChanged("B"); }
        }
        string c;

        public string C
        {
            get { return c; }
            set { c = value; OnPropertyChanged("C"); }
        }
        string d;

        public string D
        {
            get { return d; }
            set { d = value; OnPropertyChanged("D"); }
        }

        string numberQues;

        public string NumberQues
        {
            get { return numberQues; }
            set { numberQues = value; OnPropertyChanged("NumberQues"); }
        }

        string score;

        public string Score
        {
            get { return score; }
            set { score = value; OnPropertyChanged("Score"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        DispatcherTimer DTimer;
        DispatcherTimer DTimer_Position;

        string image_Check;

        public string Image_Check
        {
            get { return image_Check; }
            set { image_Check = value; OnPropertyChanged("Image_Check"); }
        }
        string status_time;

        public string Status_Time
        {
            get { return status_time; }
            set { status_time = value; OnPropertyChanged("Status_Time"); }
        }
        public GameStart()
        {
            InitializeComponent();
            this.DataContext = this;
            Init_WebClient();
            Init_Timer();
            Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            NetworkInformationUtility.GetNetworkTypeCompleted += GetNetworkTypeCompleted;
            
        }

        void Init_Timer()
        {
            DTimer = new DispatcherTimer();
            DTimer.Interval = new TimeSpan(0, 0, 1);
            DTimer.Tick += DTimer_Tick;

            DTimer_Position = new DispatcherTimer();
            DTimer_Position.Interval = new TimeSpan(0, 0, 3);
            DTimer_Position.Tick += DTimer_Position_Tick;
        }
        void DTimer_Position_Tick(object sender, EventArgs e)
        {
            try
            {
                MPlayer.Position = new TimeSpan(0, 0, 30);
                if (MPlayer.Position.Seconds > 29)
                {
                    MPlayer.Play();
                    MPlayer.Volume = 1;
                    DTimer.Start();
                    DTimer_Position.Stop();
                    Load_Answer(Global.ListQuestion[Global.Current_Question]);
                }
            }
            catch { }
        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (MessageBox.Show("Game is starting! Do you sure get out", "Game Start", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                e.Cancel = false;
            }
            else
                e.Cancel = true;
            
        }
        void DTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Global.Total_Time++;
                Status_Time = "Time: " + Global.Total_Time.ToString();
            }
            catch { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (++Global.Current_Question >= 10)
                {
                    Result();
                    return;
                }
                Global.Total_Score += Check_Answer(Global.ListQuestion[Global.Current_Question - 1], "E");
                Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            }
            catch { }
        }

        void Load_NextQuestion(Question ques)
        {
            try
            {
                Str_Question = "- - - Waiting the question - - -";
                if (Global.Current_Question > 10)
                    return;
                NumberQues = "Question: " + (Global.Current_Question + 1).ToString() + "/10";
                Score = Global.Total_Score.ToString();
                NetworkInformationUtility.GetNetworkTypeAsync(30);
                WClient_MoreInfo.DownloadStringAsync(new Uri(ques.URL));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void GetNetworkTypeCompleted(object sender, NetworkTypeEventArgs networkTypeEventArgs)
        {
            try
            {

                if (networkTypeEventArgs.HasTimeout)
                {
                }
                else if (networkTypeEventArgs.HasInternet)
                {
                    //Bool_Connect = true;
                }
                else
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("Cheack Internet! You disconnect !", "Internet Connection", MessageBoxButton.OK));
                    //NavigationContext
                }
            }
            catch { }
            // Always dispatch on the UI thread
            //
        }

        int Check_Answer(Question ques,string answer)
        {
            try
            {
                MPlayer.Stop();
                DTimer.Stop();
                A = B = C = D = Str_Question = "";
                if (answer == ques.Answer_true)
                {
                    Image_Check = @"/Assets/ImageGame/Correct.png";
                    myStoryboard.Begin();
                    MPlayer_Noti.Source = new Uri(@"/Assets/Sound/Correct.wav", UriKind.Relative);
                    return 10;
                }
                MPlayer_Noti.Source = new Uri(@"/Assets/Sound/Incorrecr.wav", UriKind.Relative);
                Image_Check = @"/Assets/ImageGame/Incorrect.png";

                return 0;
            }
            catch { return 0; }

        }
        bool Cheack_Internet()
        {
            bool isNetwork = NetworkInterface.GetIsNetworkAvailable();
            if (!isNetwork)
            {
                return true;
            }
            return false;
        }
        void Load_Answer(Question ques)
        {
            try
            {
                Image_Check = "";
                Str_Question = ques.MyQuestion;
                A = ques.Keys[0];
                B = ques.Keys[1];
                C = ques.Keys[2];
                D = ques.Keys[3];
                DTimer.Start();
            }
            catch { }
        }

        string score_Result;

        public string Score_Result
        {
            get { return score_Result; }
            set { score_Result = value; OnPropertyChanged("Score_Result"); }
        }

        string time_Result;

        public string Time_Result
        {
            get { return time_Result; }
            set { time_Result = value; OnPropertyChanged("Time_Result"); }
        }

        string status_Result;

        public string Status_Result
        {
            get { return status_Result; }
            set { status_Result = value; OnPropertyChanged("Status_Result"); }
        }
        private void Btn_A_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (++Global.Current_Question >= 10)
                {
                    Result();
                    return;
                }
                Global.Total_Score += Check_Answer(Global.ListQuestion[Global.Current_Question - 1], "A");
                Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            }
            catch { }
        }

        void Result()
        {
            try
            {
                DTimer.Stop();
                MPlayer.Stop();
                if (Global.Total_Score >= 90)
                {
                    if (Global.Total_Time < 20)
                    {
                        Status_Result = "You is hacker - too smast !!!";
                        Image_Cup = @"/Assets/ImageGame/Hack_medal.jpg";
                    }
                    else if (Global.Total_Time < 30)
                    {
                        Status_Result = "Your Knowledge of music is PERFECR !!!";
                        Image_Cup = @"/Assets/ImageGame/Diamon_Medal.jpg";
                    }
                    else if (Global.Total_Time < 60)
                    {
                        Status_Result = "Your Knowledge of music is GREAT !!!";
                        Image_Cup = @"/Assets/ImageGame/Gold_Medal.jpg";
                    }
                    else if (Global.Total_Time < 100)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/ImageGame/Sliver_Medal.jpg";
                    }
                    else if (Global.Total_Time < 200)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/ImageGame/Brozon_Medal.jpg";
                    }
                    else
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/ImageGame/Bad_Medal.png";
                    }
                }
                else if (Global.Total_Score >= 60 || Global.Total_Score < 89)
                {
                    if (Global.Total_Time < 30)
                    {
                        Status_Result = "Your Knowledge of music is GREAT !!!";
                        Image_Cup = @"/Assets/ImageGame/Gold_Medal.jpg";
                    }
                    else if (Global.Total_Time < 60)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/ImageGame/Sliver_Medal.jpg";
                    }
                    else if (Global.Total_Time < 100)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/ImageGame/Brozon_Medal.jpg";
                    }
                    else if (Global.Total_Time < 200)
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/ImageGame/Bad_Medal.png";
                    }
                    else
                    {
                        Status_Result = "You need listen music more and more !!!";
                        Image_Cup = @"/Assets/ImageGame/Listen_Icon.jpg";
                    }
                }
                else if (Global.Total_Score > 30 || Global.Total_Score < 59)
                {
                    if (Global.Total_Time < 30)
                    {
                        Status_Result = "Your Knowledge of music is GOOD !!!";
                        Image_Cup = @"/Assets/ImageGame/Sliver_Medal.jpg";
                    }
                    else if (Global.Total_Time < 60)
                    {
                        Status_Result = "Your Knowledge of music is NORMAL !!!";
                        Image_Cup = @"/Assets/ImageGame/Brozon_Medal.jpg";
                    }
                    else if (Global.Total_Time < 100)
                    {
                        Status_Result = "Your Knowledge of music is BAD !!!";
                        Image_Cup = @"/Assets/ImageGame/Bad_Medal.png";
                    }
                    else
                    {
                        Status_Result = "You need listen music more and more !!!";
                        Image_Cup = @"/Assets/ImageGame/Listen_Icon.jpg";
                    }
                }
                else
                {
                    Status_Result = "Let try again ! I thinks you can have good score!";
                    Image_Cup = @"/Assets/ImageGame/Try_Medal.png";
                }

                myStoryboard.Begin();
                Grid_Result.Visibility = System.Windows.Visibility.Visible;
                myStoryboard1.Begin();
                Score_Result = "Total Score: " + Global.Total_Score;
                Time_Result = "Total Time: " + Global.Total_Time;
            }
            catch { }
        }

        string image_Cup;

        public string Image_Cup
        {
            get { return image_Cup; }
            set { image_Cup = value; OnPropertyChanged("Image_Cup"); }
        }
        private void Btn_B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (++Global.Current_Question >= 10)
                {
                    Result();
                    return;
                }
                Global.Total_Score += Check_Answer(Global.ListQuestion[Global.Current_Question - 1], "B");
                Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            }
            catch { }
        }

        private void Btn_C_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (++Global.Current_Question >= 10)
                {
                    Result();
                    return;
                }
                Global.Total_Score += Check_Answer(Global.ListQuestion[Global.Current_Question - 1], "C");
                Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            }
            catch { }
        }

        private void Btn_D_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (++Global.Current_Question >= 10)
                {
                    Result();
                    return;
                }
                Global.Total_Score += Check_Answer(Global.ListQuestion[Global.Current_Question - 1], "D");
                Load_NextQuestion(Global.ListQuestion[Global.Current_Question]);
            }
            catch { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                MPlayer.Position = new TimeSpan(0, 0, 30);
                MPlayer.Play();
            }
            catch { }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                Grid_Result.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.GoBack();
            }
            catch { }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                Grid_Result.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.GoBack();
            }
            catch { }
        }
    }
}