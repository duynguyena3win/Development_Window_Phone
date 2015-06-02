using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Naruto_Quiz
{
    public abstract class VisibleQuestion : INotifyPropertyChanged // 0: Image ; 1: Sound 2: Text
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        string question;
        public string Question
        {
            get { return question; }
            set { question = value; OnPropertyChanged("Question"); }
        }
        int hard;

        public int Hard
        {
            get { return hard; }
            set { hard = value; }
        }
        string answer_A;

        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
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
        string answer;

        public string Answer
        {
            get { return answer; }
            set { answer = value; OnPropertyChanged("Answer"); }
        }

        string describle;

        public string Describle
        {
            get { return describle; }
            set { describle = value; OnPropertyChanged("Answer"); }
        }

        public int Type;
    }
}
