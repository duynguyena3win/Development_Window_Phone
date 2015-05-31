using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RateMyApp.Helpers;
using Microsoft.Phone.Tasks;
using RateMyApp.Controls;

namespace Rank_Nation_Football_Team
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FeedbackHelper.Default.Reviewed();

            var marketplace = new MarketplaceReviewTask();
            marketplace.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.To = "duynguyen.it.khtn@gmail.com";
            emailComposeTask.Subject = "Ranking South Asian Football - Feedback";
            emailComposeTask.Show(); 
        }
    }
}