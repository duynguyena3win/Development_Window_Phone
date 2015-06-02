using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Game.Models
{
    public class UserMusicDAO : INotifyPropertyChanged
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

        double avescore;

        public double AverageScore
        {
            get { return avescore; }
            set
            {
                avescore = value;
                NotifyPropertyChanged("AverageScore");
            }
        }

        string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        string idFacebook;

        public string IdFacebook
        {
            get { return idFacebook; }
            set
            {
                idFacebook = value;
                NotifyPropertyChanged("IdFacebook");
            }
        }

        string linkFacebook;

        public string LinkFacebook
        {
            get { return linkFacebook; }
            set
            {
                linkFacebook = value;
                NotifyPropertyChanged("LinkFacebook");
            }
        }

        string userImage;

        public string UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                NotifyPropertyChanged("UserImage");
            }
        }
        public UserMusicDAO(JToken myJson)
        {
            IdFacebook = myJson["idfacebook"].ToString();
            LinkFacebook = myJson["linkfacebook"].ToString();
            Name = myJson["name"].ToString();
            AverageScore = Convert.ToDouble(myJson["averagescore"].ToString());
            AverageScore = Math.Round(AverageScore, 2);
            UserImage = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", IdFacebook, "square", Global.AccessToken); 
        }
        public UserMusicDAO()
        {

        }
    }
}
