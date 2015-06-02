using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Shapes;
using System.Windows.Media;
using System.ComponentModel;
using System.Net.Http;
using Music_Game.Models;
using System.Diagnostics;
using System.Windows.Threading;

namespace Music_Game
{
    public partial class GamePage : PhoneApplicationPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        QuestionDAO currentQuestion;
        int currentIndex = -1;

        string soundResult;
        public string SoundResult
        {
            get { return soundResult; }
            set
            {
                soundResult = value;
                NotifyPropertyChanged("SoundResult");
            }
        }

       
        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                currentIndex = value;
                if (currentIndex == 10)
                {
                    try
                    {
                        NavigationService.Navigate(new Uri("/ResultPage.xaml", UriKind.Relative));
                    }
                    catch
                    {
                        Debug.WriteLine("Error in Result Page Click!");
                    }
                }
            }
        }
        public QuestionDAO CurrentQuestion
        {
            get { return currentQuestion; }
            set
            {
                currentQuestion = value;
                NotifyPropertyChanged("CurrentQuestion");
            }
        }
        
        List<string> ListImages = new List<string>();
        string imageBackground;

        public string ImageBackground
        {
            get { return imageBackground; }
            set
            {
                imageBackground = value;
                NotifyPropertyChanged("ImageBackground");
            }
        }

        int currentImages = 0;

        public int CurrentImages
        {
            get { return currentImages; }
            set
            {
                if (value >= 5)
                    currentImages = 0;
                else
                    currentImages = value;
            }
        }

        DispatcherTimer DTimerShow;
        DispatcherTimer DTimerGame;

        public GamePage()
        {
            InitializeComponent();

            this.DataContext = this;
            for (int i = 0; i < 5; i++)
            {
                ListImages.Add("\\Assets\\BG_Images\\BG_Game" + i + ".jpg");
            }
            myPlayer.Volume = Global.GameVolume;
            Global.GameScore = Global.GameTime = 0;
            ImageBackground = ListImages[CurrentImages];
            DTimerShow = new DispatcherTimer();
            DTimerShow.Interval = new TimeSpan(0, 0, 5);
            DTimerShow.Tick += DTimerShow_Tick;
            DTimerShow.Start();

            DTimerGame = new DispatcherTimer();
            DTimerGame.Interval = new TimeSpan(0, 0, 1);
            DTimerGame.Tick += DTimerGame_Tick;
            SB_Loading.Begin();
            LoadGame();
        }

        private async void LoadGame()
        {
            

            switch (Global.TypeGenre)
            {
                case "default":
                    Global.ListQuestion = await ConnectServiceHelper.LoadListQuestion();
                    if (Global.ListQuestion != null)
                    {
                        NextQuestion();
                    }
                    break;
                case "challenge":
                    Global.ListQuestion = await ConnectServiceHelper.GetListQuestionChallenge(Global.CurrentChallenge.ListIdQuestion);
                    if (Global.ListQuestion != null)
                    {
                        NextQuestion();
                    }
                    break;
                default:
                    Global.ListQuestion = await ConnectServiceHelper.LoadQuestionByGenre(Global.TypeGenre);
                    if (Global.ListQuestion != null)
                    {
                        NextQuestion();
                    }
                    break;
            }
        }

        void DTimerGame_Tick(object sender, EventArgs e)
        {
            Global.GameTime += 1;
            Time.Text = Global.GameTime.ToString();
        }

        void DTimerShow_Tick(object sender, EventArgs e)
        {
            ++CurrentImages;
            ImageBackground = ListImages[CurrentImages];
        }

        private async void NextQuestion()
        {
            Grid_Loading.Visibility = System.Windows.Visibility.Visible;
            SB_Loading.Begin();
            CurrentIndex++;
            if (CurrentIndex < 10)
            {
                CurrentQuestion = Global.ListQuestion[CurrentIndex];
                string streamUrl = await SoundCloudMusicService.LaunchTrack(CurrentQuestion.Source);
                if (streamUrl != null)
                {
                    myPlayer.Source = new Uri(streamUrl, UriKind.Absolute);
                }
            }
            return;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            myPlayer.Pause();
            DTimerGame.Stop();
            if (MessageBox.Show("Do you want leave Game? \r\nThist Game will 'NO SCORE' !", "Stop Game", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                myPlayer.Play();
                DTimerGame.Start();
                e.Cancel = true;
            }
            else
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }
        private void myPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            Grid_Loading.Visibility = System.Windows.Visibility.Collapsed;
            SB_Loading.Stop();
            ImageButton = @"Assets\ButtonImages\pause.png";
            DTimerGame.Start();
        }

        private void myPlayer_MediaEnded(object sender, Microsoft.PlayerFramework.MediaPlayerActionEventArgs e)
        {
            ImageButton = @"Assets\ButtonImages\play.png";
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteResult(CurrentIndex, false);
                NextQuestion();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("!!! Have bug in Next Button: " + ex.Message);
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            DTimerGame.Stop();
            myPlayer.Stop();
            Button target = (Button)sender;
            if (target.Content.ToString().Equals(CurrentQuestion.Answer))
            {
                Global.GameScore += 10;
                Score.Text = Global.GameScore.ToString();
                WriteResult(CurrentIndex, true);
            }
            else
                WriteResult(CurrentIndex, false);
            NextQuestion();
        }

        private void WriteResult(int index, bool bResult)
        {
            try
            {
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                int dem = 0;
                foreach (var item in WP_Questions.Children)
                {
                    if (dem == index)
                    {
                        Rectangle myitem = (Rectangle)item;
                        if (bResult)
                        {
                            myitem.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                            SoundResult = "Assets\\Sound\\Correct.wav";
                        }
                        else
                        {
                            myitem.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                            SoundResult = "Assets\\Sound\\Incorrect.wav";
                        }
                        myPlayer_Info.Play();
                        break;
                    }
                    dem++;
                }
            }
            catch
            {
                Debug.WriteLine("! ! ! Error in WriteResult Function ! ! !");
            }
        }
        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            ImageButton = @"Assets\ButtonImages\pause.png";
            myPlayer.Play();
        }

        string imageButton;

        public string ImageButton
        {
            get { return imageButton; }
            set
            {
                imageButton = value;
                NotifyPropertyChanged("ImageButton");
            }

        }

        private void SB_Loading_Completed(object sender, EventArgs e)
        {
            SB_Loading.Begin();
        }
    }
}