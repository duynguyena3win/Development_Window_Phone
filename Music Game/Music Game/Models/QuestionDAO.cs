using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Game.Models
{
    public class QuestionDAO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        int idQuestion;

        public int IdQuestion
        {
            get { return idQuestion; }
            set { idQuestion = value; NotifyPropertyChanged("IdQuestion"); }
        }
        int type;

        public int Type
        {
            get { return type; }
            set { type = value; NotifyPropertyChanged("Type"); }
        }
        string source;

        public string Source
        {
            get { return source; }
            set { source = value; NotifyPropertyChanged("Source"); }
        }
        string textQuestion;

        public string TextQuestion
        {
            get { return textQuestion; }
            set { textQuestion = value; NotifyPropertyChanged("TextQuestion"); }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; NotifyPropertyChanged("Description"); }
        }
        string a;

        public string A
        {
            get { return a; }
            set { a = value; NotifyPropertyChanged("A"); }
        }
        string b;

        public string B
        {
            get { return b; }
            set { b = value; NotifyPropertyChanged("B"); }
        }
        string c;

        public string C
        {
            get { return c; }
            set { c = value; NotifyPropertyChanged("C"); }
        }
        string d;

        public string D
        {
            get { return d; }
            set { d = value; NotifyPropertyChanged("D"); }
        }
        string answer;

        public string Answer
        {
            get { return answer; }
            set { answer = value; NotifyPropertyChanged("Answer"); }
        }

        public QuestionDAO(JToken myJson)
        {
            IdQuestion = Convert.ToInt32(myJson["idQuestion"]);
            Type = Convert.ToInt32(myJson["type"]);
            Source = myJson["source"].ToString();
            TextQuestion = myJson["textQuestion"].ToString();
            Description = myJson["description"].ToString();
            A = myJson["a"].ToString();
            B = myJson["b"].ToString();
            C = myJson["c"].ToString();
            D = myJson["d"].ToString();
            Answer = myJson["answer"].ToString();
        }
    }
}
