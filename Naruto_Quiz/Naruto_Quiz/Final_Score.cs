using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Naruto_Quiz
{
    public class Final_Score : INotifyPropertyChanged
    {
        int total_Score;

        public int Total_Score
        {
            get { return total_Score; }
            set { total_Score = value; OnPropertyChanged("Total_Score"); }
        }

        int total_Time;

        public int Total_Time
        {
            get { return total_Time; }
            set { total_Time = value; OnPropertyChanged("Total_Time"); }
        }

        int correct_Answer;

        public int Correct_Answer
        {
            get { return correct_Answer; }
            set { correct_Answer = value; OnPropertyChanged("Correct_Answer"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reset_Score()
        {
            Total_Score = Total_Time = Correct_Answer = 0;
        }
        public Final_Score()
        {
            Reset_Score();
        }
    }
}
