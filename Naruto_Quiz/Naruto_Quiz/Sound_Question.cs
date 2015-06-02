using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Naruto_Quiz
{
    public class Sound_Question : VisibleQuestion,INotifyPropertyChanged
    {
        string sound;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Question_Sound
        {
            get { return sound; }
            set { sound = value; OnPropertyChanged("Question_Sound"); }
        }

        public Sound_Question()
        {
            Type = 1;
        }

    }
}
