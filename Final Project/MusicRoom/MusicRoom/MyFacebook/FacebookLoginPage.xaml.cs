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
using System.ComponentModel;

namespace Music_App.MyFacebook
{
    public partial class FacebookLoginPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <LogOut>
        /// </summary>
        string name_facebook;

        public string Name_facebook
        {
            get { return name_facebook; }
            set { name_facebook = value; NotifyPropertyChanged("Name_facebook"); }
        }

        string image_facebook;

        public string Image_facebook
        {
            get { return image_facebook; }
            set { image_facebook = value; NotifyPropertyChanged("Image_facebook"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public FacebookLoginPage()
        {
            InitializeComponent();
            this.DataContext = this;
            Image_facebook = @"/Assets/Picture/UnknowUser.png";
            Name_facebook = "Connecting...";
            NetworkInformationUtility.GetNetworkTypeCompleted += GetNetworkTypeCompleted;
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
                    //Bool_Connect = false;
                }
            }
            catch { }
            // Always dispatch on the UI thread
            //Dispatcher.BeginInvoke(() => MessageBox.Show(message));
        }

        bool Bool_Load_Complete = false;
        private void loginButton_SessionStateChanged(object sender, Facebook.Client.Controls.SessionStateChangedEventArgs e)
        {
            try
            {
                if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Opened)
                {
                    if (Bool_Load_Complete == false)
                        LoadUserInfo();
                    Grid_Main.Visibility = this.shareButton.Visibility = Visibility.Visible;
                }
                else if (e.SessionState == Facebook.Client.Controls.FacebookSessionState.Closed)
                {
                    Grid_Main.Visibility = this.shareButton.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        private async void PublishStory()
        {
            try
            {
                SetProgressIndicator("Publish Status", true);
                try
                {
                    await this.loginButton.RequestNewPermissions("publish_stream");

                    var facebookClient = new Facebook.FacebookClient(this.loginButton.CurrentSession.AccessToken);

                    var postParams = new
                    {
                        message = Text_Status.Text,
                        name = (DTO_Class.Song_Post.Name == null && DTO_Class.Song_Post.Artist.Name == null) ? "Music Room for Window Phone" : DTO_Class.Song_Post.Name + " -- " + DTO_Class.Song_Post.Artist.Name,
                        caption = "Application for Window Phone", /* -- DuyNguyenIT Developer"*/
                        description = "Come here with music, let's draw your life! Share from Music Room Window Phone",
                        link = "http://www.windowsphone.com/vi-vn/store/app/musicroom/8d67625c-9c09-4a0e-b6d4-6fe29aa37f7a",
                        picture = (DTO_Class.Song_Post.Image == null || DTO_Class.Song_Post.URL == null) ? "http://cdn.marketplaceimages.windowsphone.com/v8/images/575e6ab1-00bb-456c-bc9d-82b794a11db9?imageType=ws_icon_large" : DTO_Class.Song_Post.Image
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
            catch { }
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
            try
            {
                SetProgressIndicator("Connecting to Facebook", true);
                var fb = new FacebookClient(this.loginButton.CurrentSession.AccessToken);
                MessageBox.Show("If connect too long, let logout and login again!");

                
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
                            //this.MyImage.ImageSource = new BitmapImage(new Uri(profilePictureUrl));
                            Image_facebook = profilePictureUrl;
                            Name_facebook = String.Format("{0} {1}", (string)result["first_name"], (string)result["last_name"]);
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