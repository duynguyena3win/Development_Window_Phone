using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Music_Game
{
    public class Question : INotifyPropertyChanged
    {
        string myQuestion;

        public string MyQuestion
        {
            get { return myQuestion; }
            set { myQuestion = value; OnPropertyChanged("MyQuestion"); }
        }
        string[] keys = new string[4];

        public string[] Keys
        {
            get { return keys; }
            set { keys = value; OnPropertyChanged("Keys"); }
        }
        string answer_true;

        public string Answer_true
        {
            get { return answer_true; }
            set { answer_true = value; OnPropertyChanged("Answer_true"); }
        }
        string answer_user;

        public string Answer_user
        {
            get { return answer_user; }
            set { answer_user = value; OnPropertyChanged("Answer_user"); }
        }
        string url;

        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public Question(string _ques, string _ans, string[] _keys, string url)
        {
            MyQuestion = _ques;
            Answer_true = _ans;
            Keys = _keys;
            URL = url;
        }
        public bool Check_Corret()
        {
            if (string.Compare(Answer_true, Answer_user) == 0)
                return true;
            return false;
        }
        public void Set_Answer_User(string ans)
        {
            Answer_user = ans;
        }
    }
}
