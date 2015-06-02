using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLib
{
    public abstract class AbstractQuestion: INotifyPropertyChanged
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

        string desQuestion;
        public string DesQuestion
        {
            get { return desQuestion; }
            set
            {
                desQuestion = value;
                NotifyPropertyChanged("DesQuestion");
            }
        }

        int idQuestion;
        public int IdQuestion
        {
            get { return idQuestion; }
            set
            {
                idQuestion = value;
                NotifyPropertyChanged("IdQuestion");
            }
        }

        int typeQuestion;
        public int TypeQuestion
        {
            get { return typeQuestion; }
            set
            {
                typeQuestion = value;
                NotifyPropertyChanged("TypeQuestion");
            }
        }

        string a;

        public string A
        {
            get { return a; }
            set
            {
                a = value;
                NotifyPropertyChanged("A");
            }
        }

        string b;
        public string B
        {
            get { return b; }
            set
            {
                b = value;
                NotifyPropertyChanged("B");
            }
        }

        string c;
        public string C
        {
            get { return c; }
            set
            {
                c = value;
                NotifyPropertyChanged("C");
            }
        }

        string d;
        public string D
        {
            get { return d; }
            set
            {
                d = value;
                NotifyPropertyChanged("D");
            }
        }

        string source;

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                NotifyPropertyChanged("Source");
            }
        }


        public abstract string GetType();
    }
}
