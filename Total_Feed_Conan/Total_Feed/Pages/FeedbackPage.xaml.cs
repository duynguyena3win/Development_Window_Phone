﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Total_Feed.Pages
{
    public partial class FeedbackPage : PhoneApplicationPage
    {
        public FeedbackPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void Feedback_OnCancel(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Feedback_AfterSubmit(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}