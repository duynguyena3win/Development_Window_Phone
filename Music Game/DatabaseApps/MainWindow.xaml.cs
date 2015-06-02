using HtmlAgilityPack;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WAO_Player.Music_Online_NCT;

namespace DatabaseApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Song mySong;
        public MainWindow()
        {
            InitializeComponent();
            Init_Slider_Volume();
            WClient_Download = new WebClient();
            WClient_Download.DownloadFileCompleted += WClient_Download_DownloadFileCompleted;
            WClient_Download.OpenReadCompleted += WClient_Download_OpenReadCompleted;
            Player.MediaOpened += Player_MediaOpened;
        }

        void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            Slider_Time.Value = 0;
            Slider_Time.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
            Player.Play();
        }

        void WClient_Download_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Succes!");
        }

        const string Location = "E:\\MyData\\";
        void WClient_Download_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e1)
        {
            if (e1.Error == null)
            {
                try{
                    var myFile = File.Create(Location + mySong.Name_Song +".mp3");
                    long fileLen = e1.Result.Length;
                    byte[] b = new byte[fileLen];
                    //e1.Result.Position = 300 * 1024;
                    e1.Result.Read(b, 0, b.Length);
                    myFile.Write(b, 0, b.Length);
                    myFile.Flush();
                    MessageBox.Show("Success!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(e1.Error.Message);
            }
        }

        TimeSpan TimeBegin;
        TimeSpan TimeEnd;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySong = new Song();
            mySong.Image_Song = "";
            mySong.URL = Edit_URL.Text;
            mySong = ReadHTML.GetMoreSongInfo(mySong);
            Player.Source = new Uri(mySong.URL, UriKind.Absolute);
            Player.Play();
            Text_NameSong.Text = "Name Song: " + mySong.Name_Song;
            Text_NameArtist.Text = "Name Artist: " + mySong.Name_Artist;
        }
        void Init_Slider_Volume()
        {
        }

        private void Slider_Time_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int pos = Convert.ToInt32(Slider_Time.Value);
            Player.Position = new TimeSpan(0, 0, pos);
        }
        private void Slider_Time_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimeBegin = new TimeSpan(0, 0, (int)Slider_Time.Value);
            TimeEnd = TimeBegin + new TimeSpan(0, 0, 10);
            Btn_Import.IsEnabled = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        void Remove()
        {

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteInFolder();
            }
            catch { }

            Btn_Import.IsEnabled = false;
            string Path = Location + mySong.Name_Song + ".mp3";
            string Path1 = Location + mySong.Name_Song + "SoundCloud.mp3";
            var myFile = File.Create(Path);
            myFile.Close();
            Player.Stop();
            
            
            

            WClient_Download.DownloadFile(new Uri(mySong.URL), Path);
            TrimMp3(Path, Path1, TimeBegin, TimeEnd);
            File.Delete(Path);
            if(Check_Song.IsChecked == true)
                CreateQuestionSong();

            if (Check_Artist.IsChecked == true)
                CreateQuestionArtist();
            
            try
            {
                db.MG_Songs.Single(x => x.NameSong == mySong.Name_Song);
            }
            catch
            {
                CreateSong(mySong.Name_Song, mySong.Name_Artist);
            }

        }

        void DeleteInFolder()
        {
            string[] list = Directory.GetFiles(Location);
            foreach(string val in list)
            {
                File.Delete(val);
            }
        }
        void TrimMp3(string inputPath, string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");

            using (var reader = new Mp3FileReader(inputPath))
            using (var writer = File.Create(outputPath))
            {
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                    if (reader.CurrentTime >= begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        else break;
                    }
            }
        }

        MusicGameDataContext db = new MusicGameDataContext();
        WebClient WClient_Download;

        void CreateQuestionArtist()
        {
            MG_Question myQuestionSong = new MG_Question();
            myQuestionSong.TextQuestion = "Name Of Artist?";
            myQuestionSong.Source = "";
            myQuestionSong.Description = "Chosen Correct Answer";
            myQuestionSong.Type = 1;
            myQuestionSong.Genre = ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString();
            myQuestionSong.Answer = mySong.Name_Artist;
            RandomQuestionArtist(ref myQuestionSong);
            db.MG_Questions.InsertOnSubmit(myQuestionSong);
            db.SubmitChanges();
        }

        void RandomQuestionArtist(ref MG_Question myQues)
        {
            int val;
            Random rand = new Random();

            List<MG_Artist> ListArtist = db.MG_Artists.Where(x => x.Nation == ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString()).ToList();
            
            List<int> myVal = new List<int>();
            do
            {
                val = rand.Next(0, ListArtist.Count() - 1);
            }
            while (IsHave(myVal, val) && ListArtist[val].NameArtist != myQues.Answer);

            myQues.A = ListArtist[val].NameArtist;
            myVal.Add(val);

            do
            {
                val = rand.Next(0, ListArtist.Count()-1);
            }
            while (IsHave(myVal, val) && ListArtist[val].NameArtist != myQues.Answer);

            myQues.B = ListArtist[val].NameArtist;
            myVal.Add(val);

            do
            {
                val = rand.Next(0, ListArtist.Count()-1);
            }
            while (IsHave(myVal, val) && ListArtist[val].NameArtist != myQues.Answer);
            myQues.C = ListArtist[val].NameArtist;
            myVal.Add(val);

            do
            {
                val = rand.Next(0, ListArtist.Count()-1);
            }
            while (IsHave(myVal, val) && ListArtist[val].NameArtist != myQues.Answer);
            myQues.D = ListArtist[val].NameArtist;
            myVal.Add(val);

            val = rand.Next(1, 4);
            switch (val)
            {
                case 1:
                    myQues.A = myQues.Answer;
                    break;
                case 2:
                    myQues.B = myQues.Answer;
                    break;
                case 3:
                    myQues.C = myQues.Answer;
                    break;
                case 4:
                    myQues.D = myQues.Answer;
                    break;
            }
        }
        void CreateQuestionSong()
        {
            MG_Question myQuestionSong = new MG_Question();
            myQuestionSong.TextQuestion = "Name Of Song?";
            myQuestionSong.Source = "";
            myQuestionSong.Description = "Chosen Correct Answer";
            myQuestionSong.Type = 0;
            myQuestionSong.Genre = ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString();
            myQuestionSong.Answer = mySong.Name_Song;
            RandomQuestion(ref myQuestionSong);
            db.MG_Questions.InsertOnSubmit(myQuestionSong);
            db.SubmitChanges();
        }
        void CreateSong(string NameSong, string NameArtist)
        {
            MG_Song myNewSong = new MG_Song();
            myNewSong.NameSong = NameSong;
            myNewSong.Genre = ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString();
            try
            {
                db.MG_Artists.Single(x => x.NameArtist == NameArtist);
            }
            catch
            {
                CreateArtist(NameArtist);
            }
            finally
            {
                myNewSong.IdArtist = db.MG_Artists.Single(x => x.NameArtist == NameArtist).IdArtist;
                db.MG_Songs.InsertOnSubmit(myNewSong);
                db.SubmitChanges();
            }

        }

        void CreateArtist(string Artist)
        {
            db.MG_Artists.InsertOnSubmit(new MG_Artist() { NameArtist = Artist, Nation= ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString() });
            db.SubmitChanges();
        }

        bool IsHave(List<int> myList, int val)
        {
            for(int i=0;i<myList.Count;i++)
            {
                if (myList[i] == val)
                    return true;
            }
            return false;
        }
        void RandomQuestion(ref MG_Question myQues)
        {
            int val;
            Random rand = new Random();

            List<MG_Song> ListQuestions = db.MG_Songs.Where(x => x.Genre == ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString()).ToList();
            
            List<int> myVal = new List<int>();

            do
            {
                val = rand.Next(0, ListQuestions.Count() - 1);
            }
            while (IsHave(myVal, val) && ListQuestions[val].NameSong != myQues.Answer);
            myQues.A = ListQuestions[val].NameSong;
            myVal.Add(val);

            do {
                val = rand.Next(0, ListQuestions.Count()-1);
            }
            while (IsHave(myVal, val) && ListQuestions[val].NameSong != myQues.Answer);
            myQues.B = ListQuestions[val].NameSong;
            myVal.Add(val);

            do
            {
                val = rand.Next(0, ListQuestions.Count()-1);
            }
            while (IsHave(myVal, val) && ListQuestions[val].NameSong != myQues.Answer);
            myQues.C = ListQuestions[val].NameSong;
            myVal.Add(val);

            do
            {
                val = rand.Next(0, ListQuestions.Count()-1);
            }
            while (IsHave(myVal, val) && ListQuestions[val].NameSong != myQues.Answer);

            myQues.D = ListQuestions[val].NameSong;
            myVal.Add(val);

            val = rand.Next(1, 4);
            switch(val)
            {
                case 1:
                    myQues.A = myQues.Answer;
                    break;
                case 2:
                    myQues.B = myQues.Answer;
                    break;
                case 3:
                    myQues.C = myQues.Answer;
                    break;
                case 4:
                    myQues.D = myQues.Answer;
                    break;
            }
        }

        void UpdateNation()
        {
            List<MG_Artist> ListArtist = db.MG_Artists.ToList();
            for(int i=0; i<ListArtist.Count;i++)
            {
                ListArtist[i].Nation = "Vietnam";
            }
            db.SubmitChanges();
        }
        void AddArtist()
        {
            HtmlDocument wap = new HtmlDocument();
            string htmlCode = File.ReadAllText("E:\\Artist.txt");
            wap.LoadHtml(htmlCode);

            HtmlNodeCollection Artists = wap.DocumentNode.SelectNodes("//div[@class='singer-block-item']");

            foreach (HtmlNode node in Artists)
            {
                try
                {
                    MG_Artist myArtist = new MG_Artist();
                    myArtist.Nation = ((ComboBoxItem)ComboBox.SelectedValue).Content.ToString();
                    myArtist.NameArtist = node.ChildNodes[3].InnerText;
                    db.MG_Artists.InsertOnSubmit(myArtist);
                    db.SubmitChanges();
                }
                catch { MessageBox.Show("Remove!"); }
            }
            
        }

        private void Player_Loaded(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                AddArtist();
                MessageBox.Show("Success!");
            }
            catch(Exception ex) { MessageBox.Show("Fail!" + ex.Message); }
        }
    }
}
