using Microsoft.Phone.Net.NetworkInformation;
using Music_Game.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Music_Game
{
    public static class Global
    {
        public static double GameVolume = 1.0f;
        public static double GameLevel = 0.5f;
        public static ObservableCollection<QuestionDAO> ListQuestion = null;
        public static string TypeGenre = null;
        public static int GameScore;
        public static int GameTime;
        public static UserMusicDAO CurrentUser = null;
        public static ChallengeDAO CurrentChallenge = null;
        public static string IdFaceChallenger = null;
        public static bool IsBGMusic = true;
        public static string AccessToken = null;
        
    }
}
