using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Music_Game
{
    public class Task : INotifyPropertyChanged
    {
        List<Question> list_Question;

        public List<Question> List_Question
        {
            get { return list_Question; }
            set { list_Question = value; OnPropertyChanged("List_Question"); }
        }
        string stt;

        public string STT
        {
            get { return stt; }
            set { stt = value; OnPropertyChanged("STT"); }
        }
        public int Current_Question;

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Task()
        {
            List_Question = new List<Question>();
        }

        public Question Load_NextQuestion()
        {
            return List_Question[++Current_Question];
        }
    }
}
