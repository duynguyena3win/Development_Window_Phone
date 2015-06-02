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
using Google.Apis.Translate;
namespace Naruto_Quiz
{
    public partial class Question_Page : PhoneApplicationPage
    {
        DispatcherTimer Timer_TotalTimes;

        public Question_Page()
        {
            InitializeComponent();
            Name_Exam.Text = Global.Name_Exam;
            Control_QuesImage.CORRECT_IMAGE += Control_QuesImage_CORRECT_IMAGE;
            Init_Timer();
            Load.Complete_Animation += Load_Complete_Animation;
        }


        void Load_Complete_Animation()
        {
            Load.Visibility = System.Windows.Visibility.Collapsed;
            Player_Back.Play();
            Timer_TotalTimes.Start();
            LoadQuestion();
        }

        private void Init_Timer()
        {
            Timer_TotalTimes = new DispatcherTimer();
            Timer_TotalTimes.Interval = new TimeSpan(0, 0, 1);
            Timer_TotalTimes.Tick += Timer_TotalTimes_Tick;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Timer_TotalTimes.Stop();
        }

        void Timer_TotalTimes_Tick(object sender, EventArgs e)
        {
            Global.F_Score.Total_Time += 1;
        }
        
        void Control_QuesImage_CORRECT_IMAGE(bool Bool_Correct)
        {
            if (Bool_Correct == true)
            {
                Global.F_Score.Total_Score += 10;
            }
            LoadQuestion();
        }

        void End_Game()
        {
            Global.Current_Index = -1;
            NavigationService.Navigate(new Uri("/Result_Page.xaml", UriKind.Relative));
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.Bool_Back == true)
            {
                App.Bool_Back = false;
                NavigationService.GoBack();
            }
        }
        bool Play_Sound = false;
        void LoadQuestion()
        {
            Global.Current_Index++;
            if (Global.Current_Index >= Global.Current_Questions.Count)
            {
                End_Game();
                return;
            }
            int Type = Global.Current_Questions[Global.Current_Index].Type;
            Hide_Control_Question();
            switch (Type)
            {
                case 0:
                    if (Play_Sound == true)
                    {
                        Play_Sound = false;
                        Player_Back.Play();
                    }
                    Control_QuesImage.Visibility = System.Windows.Visibility.Visible;
                    Control_QuesImage.Load_Question(Global.Current_Questions[Global.Current_Index]);
                    break;
                case 1:
                    Player_Back.Pause();
                    Control_QuesSound.Visibility = System.Windows.Visibility.Visible;
                    Control_QuesSound.Load_Question(Global.Current_Questions[Global.Current_Index]);
                    break;
                case 2:
                    break;
                default:
                    break;
            }

        }

        void Hide_Control_Question()
        {
            Control_QuesImage.Visibility = System.Windows.Visibility.Collapsed;
            Control_QuesSound.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Type_Question_CORRECT()
        {
            Global.F_Score.Total_Score += 10;
            
        }
        
    }
}