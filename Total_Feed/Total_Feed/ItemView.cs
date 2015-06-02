using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Total_Feed
{
    public class ItemView : INotifyPropertyChanged
    {
        string imagesource;

        public string ImageSource
        {
            get { return imagesource; }
            set { imagesource = value; NotifyPropertyChanged("ImageSource"); }
        }

        string name;

        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        public ItemView()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
