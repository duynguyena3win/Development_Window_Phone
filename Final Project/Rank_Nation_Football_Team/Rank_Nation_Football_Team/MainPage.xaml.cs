using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Rank_Nation_Football_Team.Resources;
using HtmlAgilityPack;
using System.ComponentModel;
using System.Collections.ObjectModel;
using RateMyApp.Controls;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace Rank_Nation_Football_Team
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        WebClient WClient_GetList;
        ObservableCollection<NationTeam> Nations;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<NationTeam> Nations_Team
        {
            get { return Nations; }
            set { Nations = value; NotifyPropertyChanged("Nations_Team"); }
        }

        public MainPage()
        {
            this.DataContext = this;
            InitializeComponent();
            Init_WebClient();
            Load_List();
            Init_AppBar();
            //MessageBox.Show("Hello");
        }

        void Load_List()
        {
            try
            {
                Read_XML();
                WClient_GetList.DownloadStringAsync(new Uri("http://bongdaso.com/Ranking.aspx?Type=M", UriKind.Absolute));
            }
            catch {  }
        }
        void Init_WebClient()
        {
            WClient_GetList = new WebClient();
            WClient_GetList.DownloadStringCompleted += WClient_GetList_DownloadStringCompleted;
        }

        void WClient_GetList_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                GetListNation(e.Result);
            }
            catch
            {
            }
        }

        void Write_XML()
        {
            try
            {
                // Write to the Isolated Storage
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("TeamHistory.xml", FileMode.Create))
                    {
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ObservableCollection<NationTeam>));
                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                        {
                            serializer.Serialize(xmlWriter, Nations_Team);
                        }
                    }
                }
            }
            catch {  }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                Write_XML();
            }
            catch { }
        }
        void Read_XML()
        {
            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("TeamHistory.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<NationTeam>));

                        Nations_Team = (ObservableCollection<NationTeam>)serializer.Deserialize(stream);
                    }
                }
            }
            catch
            {
                //add some code here
            }
        }

        public void GetListNation(string Html)
        {
            try
            {
                Nations_Team = new ObservableCollection<NationTeam>();
                HtmlDocument wap = new HtmlDocument();
                wap.LoadHtml(Html);
                //Lấy Team

                HtmlDocument docPage = new HtmlDocument();
                docPage.LoadHtml(Html);
                HtmlNode nodeListSong = docPage.DocumentNode.SelectSingleNode("//div[@class='ranking']").ChildNodes[1];
                if (nodeListSong == null)
                    return;
                HtmlNodeCollection nodeSongs = nodeListSong.SelectNodes("tr");
                if (nodeSongs == null)
                    return;
                int i = 0;
                foreach (HtmlNode nodeSong in nodeSongs)
                {
                    try
                    {
                        int x;
                        NationTeam NT = new NationTeam();
                        NT.Name_Nation = nodeSong.ChildNodes[5].ChildNodes[0].InnerText;
                        NT.Rank = Convert.ToInt32(nodeSong.ChildNodes[1].InnerText);
                        if (NT.Rank == 123)
                            NT.Rank = 123;
                        if (AsianTeam(NT.Name_Nation))
                        {
                            NT.Rank = Convert.ToInt32(nodeSong.ChildNodes[1].InnerText);
                            NT.Flag_Nation = "http://bongdaso.com/" + nodeSong.ChildNodes[3].ChildNodes[0].GetAttributeValue("src", "");
                            NT.Url_PageNation = "http://bongdaso.com/" + nodeSong.ChildNodes[5].ChildNodes[0].GetAttributeValue("href", "");
                            NT.Point_Rank = nodeSong.ChildNodes[7].ChildNodes[0].InnerText;
                            Nations_Team.Add(NT);
                        }
                        i++;
                    }
                    catch { }
                }
            }
            catch { }
        }

        bool AsianTeam(string name)
        {
            if (name == "Vietnam" ||
                name == "Philippines" ||
                name == "Thailand" ||
                name == "Malaysia" ||
                name == "Singapore" ||
                name == "Indonesia" ||
                name == "Myanmar" ||
                name == "Laos" ||
                name == "Cambodia" ||
                name == "Brunei" ||
                name == "Timor-Leste")
                return true;
            return false;
        }
        private void List_Nation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Global.Current_Team = Nations_Team[List_Nation.SelectedIndex];
                NavigationService.Navigate(new Uri("/Nation_Info.xaml", UriKind.Relative));
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        void Init_AppBar()
        {
            try
            {
                ApplicationBar = new ApplicationBar();

                ApplicationBar.Mode = ApplicationBarMode.Default;
                ApplicationBar.Opacity = 0.6f;
                ApplicationBar.IsVisible = true;
                ApplicationBar.IsMenuEnabled = true;

                ApplicationBarIconButton Button_Info = new ApplicationBarIconButton();
                Button_Info.IconUri = new Uri("/Assets/about.png", UriKind.Relative);
                Button_Info.Text = "Information";
                Button_Info.Click += Button_Info_Click;
                ApplicationBar.Buttons.Add(Button_Info);
                
            }
            catch { }
        }

        private void Button_Info_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
            }
            catch { }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            FeedbackOverlay.VisibilityChanged += FeedbackOverlay_VisibilityChanged;
            DataContext = RateMyApp.Helpers.FeedbackHelper.Default;
            this.DataContext = this;
        }

        void FeedbackOverlay_VisibilityChanged(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = (FeedbackOverlay.Visibility != Visibility.Visible);
        }
    }
}