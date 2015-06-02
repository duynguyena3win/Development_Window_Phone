using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;

namespace Naruto_Quiz
{
    public partial class Type_Question_Sound : UserControl, INotifyPropertyChanged
    {
        public delegate void TRUE_SOUNDANSWER();
        public event TRUE_SOUNDANSWER CORRECT_SOUND;

        MediaElement MEle = new MediaElement();

        string question_String;

        public string Question_String
        {
            get { return question_String; }
            set { question_String = value; OnPropertyChanged("Question_String"); }
        }

        string answer_A;
        public string Answer_A
        {
            get { return answer_A; }
            set { answer_A = value; OnPropertyChanged("Answer_A"); }
        }

        string answer_B;
        public string Answer_B
        {
            get { return answer_B; }
            set { answer_B = value; OnPropertyChanged("Answer_B"); }
        }

        string answer_C;
        public string Answer_C
        {
            get { return answer_C; }
            set { answer_C = value; OnPropertyChanged("Answer_C"); }
        }

        string answer_D;
        public string Answer_D
        {
            get { return answer_D; }
            set { answer_D = value; OnPropertyChanged("Answer_D"); }
        }

        string sound_Question;

        public string Question_Sound
        {
            get { return sound_Question; }
            set { sound_Question = value; OnPropertyChanged("Question_Sound"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Type_Question_Sound()
        {
            InitializeComponent();
            this.DataContext = this;
            MEle.AutoPlay = true;
            MEle.Volume = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameworkDispatcher.Update();
            Song temp = Song.FromUri("Question", new Uri(Question_Sound, UriKind.Relative));
            MediaPlayer.Play(temp);
        }

        public void Load_Question(VisibleQuestion Ques)
        {
            Answer_A = Ques.Answer_A;
            Answer_B = Ques.Answer_B;
            Answer_C = Ques.Answer_C;
            Answer_D = Ques.Answer_D;
            Question_String = Ques.Question;
            Question_Sound = ((Sound_Question)Ques).Question_Sound;
            FrameworkDispatcher.Update();
            Song temp = Song.FromUri("Question", new Uri(Question_Sound, UriKind.Relative));
            MediaPlayer.Play(temp);
        }
        void Answer_Click(string ans)
        {
            if (string.Compare(ans, Global.Current_Questions[Global.Current_Index].Answer) == 0)
            {
                CORRECT_SOUND();
            }
        }

        private void A_Click(object sender, RoutedEventArgs e)
        {
            Answer_Click("A");
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Answer_Click("B");
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            Answer_Click("C");
        }

        private void D_Click(object sender, RoutedEventArgs e)
        {
            Answer_Click("D");
        }
    }
}
