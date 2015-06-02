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

namespace Naruto_Quiz
{
    public partial class Type_ChosenPicture : UserControl
    {
        // Data for this Question
        string name_Character;
        public string Name_Character
        {
            get { return name_Character; }
            set { name_Character = value; OnPropertyChanged("Name_Character"); }
        }

        string image_Character;
        public string Image_Character
        {
            get { return image_Character; }
            set { image_Character = value; OnPropertyChanged("Image_Character"); }
        }

        string discreble;
        public string Discreble
        {
            get { return discreble; }
            set { discreble = value; OnPropertyChanged("Discreble"); }
        }

        string question;
        public string Question
        {
            get { return question; }
            set { question = value; OnPropertyChanged("Question"); }
        }

        List<Images_TypeChosen> list_Answer;

        public List<Images_TypeChosen> List_Answer
        {
            get { return list_Answer; }
            set { list_Answer = value; OnPropertyChanged("List_Answer"); }
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Type_ChosenPicture()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }

    public class Images_TypeChosen : INotifyPropertyChanged
    {
        string id;
        public string ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged("ID"); }
        }

        string source;
        public string Source
        {
            get { return source; }
            set { source = value; OnPropertyChanged("Source"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Images_TypeChosen()
        {
        }

        public Images_TypeChosen(string id, string sour)
        {
            ID = id;
            Source = sour;
        }
    }
}
