using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MusicLib
{
    public class MG_User : INotifyPropertyChanged
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

        int idUser;
        public int IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                NotifyPropertyChanged("IdUser");
            }
        }

        string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                NotifyPropertyChanged("UserName");
            }
        }

        List<User> listFriends;
        public List<User> ListFriends
        {
            get { return listFriends; }
            set
            {
                listFriends = value;
                NotifyPropertyChanged("ListFriends");
            }
        }

        float score;
        public float Score
        {
            get { return score; }
            set
            {
                score = value;
                NotifyPropertyChanged("Score");
            }
        }

        string about;
        public string About
        {
            get { return about; }
            set
            {
                about = value;
                NotifyPropertyChanged("About");
            }
        }
    }
}
