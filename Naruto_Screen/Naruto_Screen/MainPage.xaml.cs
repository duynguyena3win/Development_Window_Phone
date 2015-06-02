using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Naruto_Screen.Resources;
using System.ComponentModel;
using Windows.Phone.System.UserProfile;
using vservWindowsPhone;

namespace Naruto_Screen
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        string image_source;
        
        VservAdControl VSB = VservAdControl.Instance;
        string zoneId = "__8deaea04"; 

        public string Image_Source
        {
            get { return image_source; }
            set { image_source = value; NotifyPropertyChanged("Image_Source"); }
        }

        string name;

        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        bool bool_QC = false;
        bool bool_close = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = this;
            QuangCao();
        }

        void QuangCao()
        {
            try
            {
                App.Bool_Load = true;
                if (App.Bool_Load == false)
                {
                    App.Bool_Load = true;
                    VSB.SetRequestTimeOut(30);
                    VSB.DisplayAd(zoneId, LayoutRoot);
                    // Xử lí khi hiển thị tắt quảng cáo 
                    VSB.VservAdClosed += new EventHandler(VACCallback_OnVservAdClosing);
                    // Xử lí khi không có kết nối mạng 
                    VSB.VservAdError += new EventHandler(VACCallback_OnVservAdNetworkError);
                    // HET QUANG CAO --- 
                }
            }
            catch
            {
            }
            finally
            {
                Init_AppBar();
            }

        }

        private void VACCallback_OnVservAdNetworkError(object sender, EventArgs e)
        {
            MessageBox.Show("Data connection not available", "No Data",
                    MessageBoxButton.OKCancel);
        }

        private void VACCallback_OnVservAdClosing(object sender, EventArgs e)
        {
            if (bool_close == true)
                Application.Current.Terminate();
            Init_AppBar();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (bool_QC == false)
                return;
            if (add.Visibility == Visibility.Collapsed)
            {
                // tích hợp nếu dùng thoát app có quảng cáo 
                e.Cancel = true;
                bool_close = true;
                VSB.DisplayAd(zoneId, LayoutRoot);
                add.Visibility = Visibility.Visible;
                
            }
            else
            {
                bool_close = true;
                e.Cancel = false;
            }
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            bool_QC = false;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool_QC = true;
            if (App.Item_Current.ImageSource == string.Empty || App.Item_Current.ImageSource == null)
            {
                try
                {
                    Image_Source = LockScreen.GetImageUri().LocalPath;
                    Name = App.Item_Current.Name;
                }
                catch { }
            }
            else
            {
                Image_Source = App.Item_Current.ImageSource;
            }
        }

        void Init_AppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            ApplicationBarIconButton Button_List = new ApplicationBarIconButton();
            Button_List.IconUri = new Uri("/Assets/Picture/add.png", UriKind.Relative);
            Button_List.Text = "Image Library";
            Button_List.Click += Button_List_Click;
            ApplicationBar.Buttons.Add(Button_List);

            ApplicationBarIconButton Button_OK = new ApplicationBarIconButton();
            Button_OK.IconUri = new Uri("/Assets/Picture/check.png", UriKind.Relative);
            Button_OK.Text = "OK";
            Button_OK.Click += Button_OK_Click;
            ApplicationBar.Buttons.Add(Button_OK);

            ApplicationBarIconButton Button_Edit = new ApplicationBarIconButton();
            Button_Edit.IconUri = new Uri("/Assets/Picture/edit.png", UriKind.Relative);
            Button_Edit.Text = "About";
            Button_Edit.Click += Button_Edit_Click;
            ApplicationBar.Buttons.Add(Button_Edit);

            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = 0.5; 
        }

        void Button_Edit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lock Screen Image Uchiha Ver 1.0 \r\nDeveloper Nguyễn Duy Nguyên\r\nEmail: duynguyena3win@gmail.com\r\nApp use picture on Internet! Thanks all artists design image !", "Information", MessageBoxButton.OK);
        }

        string info;
        void Button_OK_Click(object sender, EventArgs e)
        {
            LockHelper(App.Item_Current.ImageSource, true);
            try
            {
                MessageBox.Show(info);
            }
            catch { }
        }

        void Button_List_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/List_Character.xaml", UriKind.Relative));
                
            }
            catch { }
        }

        private async void LockHelper(string filePathOfTheImage, bool isAppResource)
        {
            try
            {
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;
                if (!isProvider)
                {
                    // If you're not the provider, this call will prompt the user for permission.
                    // Calling RequestAccessAsync from a background agent is not allowed.
                    var op = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();

                    // Only do further work if the access was granted.
                    isProvider = op == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }

                if (isProvider)
                {
                    // At this stage, the app is the active lock screen background provider.

                    // The following code example shows the new URI schema.
                    // ms-appdata points to the root of the local app data folder.
                    // ms-appx points to the Local app install folder, to reference resources bundled in the XAP package.
                    var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                    var uri = new Uri(schema + filePathOfTheImage, UriKind.Absolute);

                    // Set the lock screen background image.
                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);

                    // Get the URI of the lock screen background image.
                    var currentImage = Windows.Phone.System.UserProfile.LockScreen.GetImageUri();
                    info = string.Format("The new lock screen background image is set to {0}", currentImage.ToString());
                }
                else
                {
                    info = "You said no, so I can't update your background.";
                }
            }
            catch (System.Exception ex)
            {
                info = "You can't set background! \r\nHave exception: "+ex.Message.ToString();
            }
        }

    }
}