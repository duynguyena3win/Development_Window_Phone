using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppboyPlatform.Phone;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using AppboyPlatform.PCL.Results;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Total_Feed.Pages
{
    public partial class MiscPage : PhoneApplicationPage
    {
        private string _userId1;
        private string _userId2;
        private bool? _isUserId1;

        public MiscPage()
        {
            InitializeComponent();
            _userId1 = "test23User1";
            _userId2 = "test32User2";
        }

        private void LogCustomEvent_Click(object sender, RoutedEventArgs e)
        {
            Appboy.SharedInstance.EventLogger.LogCustomEvent("Clicked Button");
        }

        private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            Appboy.SharedInstance.SubmitFeedback("duynguyen@appboy.com", "FeedBack", false);
        }

        private void RequestFeed_Click(object sender, RoutedEventArgs e)
        {
            Action<Task<IResult>> logCards = (continuation) =>
            {
                Debug.WriteLine("Received the following news feed cards.");
                foreach (BaseCard card in continuation.Result.Cards ?? Enumerable.Empty<BaseCard>())
                {
                    Debug.WriteLine("News feed card {0}.", card);
                }
            };
            Appboy.SharedInstance.RequestFeed().ContinueWith(logCards);
        }

        private void RequestDataFlush_Click(object sender, RoutedEventArgs e)
        {
            Appboy.SharedInstance.RequestDataFlush();
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (!(_isUserId1 ?? false))
            {
                Appboy.SharedInstance.ChangeUser(_userId1);
                _isUserId1 = true;
            }
            else
            {
                Appboy.SharedInstance.ChangeUser(_userId2);
                _isUserId1 = false;
            }
        }

        private void LogPurchase_Click(object sender, RoutedEventArgs e)
        {
            Appboy.SharedInstance.EventLogger.LogPurchase("Test_Purchase", "USD", 0.99m);
        }
    }
}