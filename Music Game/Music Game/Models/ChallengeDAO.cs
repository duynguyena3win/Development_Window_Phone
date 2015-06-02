using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Game.Models
{
    public class ChallengeDAO : INotifyPropertyChanged
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

        int idChallenge;

        public int IdChallenge
        {
            get { return idChallenge; }
            set
            {
                idChallenge = value;
                NotifyPropertyChanged("IdChallenge");
            }
        }
        
        int idUserReceive;
        public int IdUserReceive
        {
            get { return idUserReceive; }
            set { idUserReceive = value; NotifyPropertyChanged("IdUserReceive"); }
        }
        
        string idFaceReceive;
        public string IdFaceReceive
        {
            get { return idFaceReceive; }
            set { idFaceReceive = value; NotifyPropertyChanged("IdFaceReceive"); }
        }
        
        string nameSend;
        public string NameSend
        {
            get { return nameSend; }
            set { nameSend = value; NotifyPropertyChanged("NameSend"); }
        }
        
        string linkFaceSend;
        public string LinkFaceSend
        {
            get { return linkFaceSend; }
            set { linkFaceSend = value; NotifyPropertyChanged("LinkFaceSend"); }
        }
        
        int score;
        public int Score
        {
            get { return score; }
            set { score = value; NotifyPropertyChanged("Score"); }
        }
        
        int time;
        public int Time
        {
            get { return time; }
            set { time = value; NotifyPropertyChanged("Time"); }
        }

        string idFaceSend;
        public string IdFaceSend
        {
            get { return idFaceSend; }
            set { idFaceSend = value; NotifyPropertyChanged("IdFaceSend"); }
        }

        string listIdQuestion;

        public string ListIdQuestion
        {
            get { return listIdQuestion; }
            set
            {
                listIdQuestion = value;
                NotifyPropertyChanged("ListIdQuestion");
            }
        }
        public ChallengeDAO()
        {

        }

        public ChallengeDAO(JToken myJon)
        {
            IdChallenge = Convert.ToInt32(myJon["idChallenge"].ToString());
            IdFaceReceive = myJon["idFaceReceive"].ToString();
            IdFaceSend = myJon["idFaceSend"].ToString();
            IdUserReceive = Convert.ToInt32(myJon["idUserReceive"].ToString());
            Score = Convert.ToInt32(myJon["score"].ToString());
            Time = Convert.ToInt32(myJon["time"].ToString());
            LinkFaceSend = myJon["linkFaceSend"].ToString();
            NameSend = myJon["nameSend"].ToString();
            string temp;
            temp = string.Empty;
            ListIdQuestion = myJon["idQuestion"].ToString();
        }
    }
}
