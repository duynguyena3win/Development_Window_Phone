using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Naruto_Quiz
{
    public class Image_Question : VisibleQuestion, INotifyPropertyChanged
    {
        string image;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Question_Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Question_Image"); }
        }

        public Image_Question()
        {
            Type = 0;
        }
    }
}
