using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;

namespace Music_Game
{
    public partial class ResulChallengePage : PhoneApplicationPage
    {
        public ResulChallengePage()
        {
            InitializeComponent();
            ShowInformation();
            if (Text_Winner.Text == "! You Win !")
            {
                BG_PlayerInfor.Source = new Uri(@"Assets\Sound\Win.mp3", UriKind.Relative);
            }
            else if (Text_Winner.Text == "! You Lose !")
            {
                BG_PlayerInfor.Source = new Uri(@"Assets\Sound\Lose.mp3", UriKind.Relative);
            }
            BG_PlayerInfor.Play();
            BG_Player.Volume = 1;
            UpdateResult();
        }

        private void UpdateResult()
        {
            ConnectServiceHelper.UpdateScore(Global.GameScore, Global.GameTime, Global.CurrentUser.IdFacebook);
            ConnectServiceHelper.UpdateScore(Global.CurrentChallenge.Score, Global.CurrentChallenge.Time, Global.CurrentChallenge.IdFaceSend);
            if(Text_Winner.Text == "! You Win !")
            {
                ConnectServiceHelper.UpdateWinLose(true, Global.CurrentUser.IdFacebook);
                ConnectServiceHelper.UpdateWinLose(false, Global.CurrentChallenge.IdFaceSend);
            }
            else if(Text_Winner.Text == "! You Lose !")
            {
                ConnectServiceHelper.UpdateWinLose(false, Global.CurrentUser.IdFacebook);
                ConnectServiceHelper.UpdateWinLose(true, Global.CurrentChallenge.IdFaceSend);
            }
            ConnectServiceHelper.DeleteChallenge(Global.CurrentChallenge.IdChallenge);
        }

        private void ShowInformation()
        {

            Text_Score.Text = Global.GameScore.ToString();
            Text_Score_Challenger.Text = Global.CurrentChallenge.Score.ToString();
            Text_Time.Text = Global.GameTime.ToString();
            Text_Time_Challenger.Text = Global.CurrentChallenge.Time.ToString();
            if(Global.GameScore > Global.CurrentChallenge.Score)
            {
                Text_Winner.Text = "! You Win !";
            }
            else if (Global.GameScore == Global.CurrentChallenge.Score)
            {
                if(Global.GameTime < Global.CurrentChallenge.Time)
                    Text_Winner.Text = "! You Win !";
                else if(Global.GameTime == Global.CurrentChallenge.Time)
                    Text_Winner.Text = "! Game Draw !";
                else
                    Text_Winner.Text = "! You Lose !";
            }
            else
                Text_Winner.Text = "! You Lose !";
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Button_Click(null, null);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch { Debug.WriteLine("Have some error in OK Button !"); }
        }

        private void BG_PlayerInfor_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }

        private void BG_Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            BG_Player.Play();
        }
    }
}