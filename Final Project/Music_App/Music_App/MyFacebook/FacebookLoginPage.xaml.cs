using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook.Client;
using System.Threading.Tasks;
using Facebook;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Net.NetworkInformation;
using System.Threading;
using DotNetApp.Utilities;

namespace Music_App.MyFacebook
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        /// <LogOut>
        /// </summary>
        bool Bool_Connect = false;

        public FacebookLoginPage()
        {
            InitializeComponent();
            this.MyImage.ImageSource = new BitmapImage(new Uri("/Assets/Picture/UnknowUser.png",UriKind.Relative));
            this.MyName.Text = "Connecting to Facebook...";
            NetworkInformationUtility.GetNetworkTypeCompleted += GetNetworkTypeCompleted;
        }

        private void GetNetworkTypeCompleted(object sender, NetworkTypeEventArgs networkTypeEventArgs)
        {
            

            if (networkTypeEventArgs.HasTimeout)
            {
            }
            else if (networkTypeEventArgs.HasInternet)
            {
                Bool_Connect = true;
            }
            else
            {
                Bool_Connect = false;
            }

            // Always dispatch on the UI thread
            //Dispatcher.BeginInvoke(() => MessageBox.Show(message));
        }

        bool Bool_Load_Complete = false;
        private void loginButton_SessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
        {
            if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Opened)
            {
                if(Bool_Load_Complete == false)
                    LoadUserInfo();
                Grid_Main.Visibility = this.shareButton.Visibility = Visibility.Visible;
            }
            else if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Closed)
            {
                Grid_Main.Visibility = this.shareButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void PublishStory()
        {
            SetProgressIndicator("Publish Status", true);
            try
            {
                await this.loginButton.RequestNewPermissions("publish_stream");

                var facebookClient = new Facebook.FacebookClient(this.loginButton.CurrentSession.AccessToken);

                var postParams = new
                {
                    message = Text_Status.Text,
                    name = (DTO_Class.Song_Post.Name == null && DTO_Class.Song_Post.Artist.Name == null)? "Ứng dụng Music Room for Window Phone": DTO_Class.Song_Post.Name + " -- " + DTO_Class.Song_Post.Artist.Name,
                    caption = "Application for Window Phone", /* -- Nguyễn Duy Nguyên Developer"*/
                    description = "Âm nhạc thăng hoa, dâng tràn cảm xúc! Được chia sẽ từ Ứng dụng Music Room Window Phone",
                    link = (DTO_Class.Song_Post.URL == null) ? "http://facebooksdk.net/": DTO_Class.Song_Post.URL,
                    picture = (DTO_Class.Song_Post.Image == null || DTO_Class.Song_Post.URL == null) ? "http://facebooksdk.net/assets/img/logo75x75.png" : DTO_Class.Song_Post.Image
                };

                try
                {
                    dynamic fbPostTaskResult = await facebookClient.PostTaskAsync("/me/feed", postParams);
                    var result = (IDictionary<string, object>)fbPostTaskResult;

                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Successful! Your status is posted");
                    });
                }
                catch (Exception ex)
                {
                    SetProgressIndicator("", false);
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Fail post! Have exception is :' " + ex.Message + "' !", "Error", MessageBoxButton.OK);
                    });
                }
                SetProgressIndicator("", false);
            }
            catch { SetProgressIndicator("", false); }
        }

        void Block_Control(bool state)
        {
            loginButton.IsEnabled = shareButton.IsEnabled = state;
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            Block_Control(false);
            this.PublishStory();
            Block_Control(true);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        void SetProgressIndicator(string status, bool value)
        {
            SystemTray.ProgressIndicator.Text = status;
            SystemTray.ProgressIndicator.IsIndeterminate = value;
            SystemTray.ProgressIndicator.IsVisible = value;
        }

        private void LoadUserInfo()
        {
            SetProgressIndicator("Connecting to Facebook", true);
            try
            {
                var fb = new FacebookClient(this.loginButton.CurrentSession.AccessToken);
                MessageBox.Show("If connect too long, let logout and login again!");

                NetworkInformationUtility.GetNetworkTypeAsync(3000); // Timeout of 3 seconds

                if (Bool_Connect != true)
                {
                    //Dispatcher.BeginInvoke(() => MessageBox.Show(message));
                    return;
                }
                fb.GetCompleted += (o, e) =>
                {
                    if (e.Error != null)
                    {
                        //MessageBox.Show(e.Error.Message);
                        return;
                    }

                    var result = (IDictionary<string, object>)e.GetResultData();

                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            var profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", loginButton.CurrentUser.Id, "square", this.loginButton.CurrentSession.AccessToken);
                            this.MyImage.ImageSource = new BitmapImage(new Uri(profilePictureUrl));
                            this.MyName.Text = String.Format("{0} {1}", (string)result["first_name"], (string)result["last_name"]);
                            Bool_Load_Complete = true;
                            SetProgressIndicator("", false);
                        }
                        catch
                        {
                            SetProgressIndicator("", false);
                        }
                    });
                };
                
                fb.GetTaskAsync("me");
            }
            catch (Exception ex)
            {
                SetProgressIndicator("", true);
                MessageBox.Show(ex.ToString());
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Text_Status.Text = "";
        }
    }

}