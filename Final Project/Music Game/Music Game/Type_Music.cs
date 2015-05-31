using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Music_Game
{
    public enum Type
    {
        VPOP, USUK, KPOP
    }
    public class Type_Music : INotifyPropertyChanged
    {
        string name_Type;

        public string NameType
        {
            get { return name_Type; }
            set { name_Type = value; OnPropertyChanged("NameType"); }
        }

        string image;

        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }
        public List<Task> List_Task;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public Type_Music()
        {
            List_Task = new List<Task>();
        }
        public Type_Music(string name, string image)
        {
            NameType = name;
            Image = image;
            List_Task = new List<Task>();
        }
    }
}
