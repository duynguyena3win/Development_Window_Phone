using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;
using MusicGameService.Models;

namespace MusicGameService.Controllers
{
    public class ScoreController : ApiController
    {
        MGDataContext db = new MGDataContext();

        // Post Score
        public void PostNewScore(JToken myJson)
        {
            try
            {
                MG_User TargetUser = db.MG_Users.Single(x => x.IdFacebook.Equals(myJson["idfacebook"].ToString()));
                
                MG_Score newScore = new MG_Score();
                newScore.IdUser = TargetUser.IdUser;
                newScore.Score = Convert.ToInt32(myJson["score"].ToString());
                newScore.Time = Convert.ToInt32(myJson["time"].ToString());

                db.MG_Scores.InsertOnSubmit(newScore);
                db.SubmitChanges();
                
                TargetUser = db.MG_Users.Single(x => x.IdFacebook.Equals(myJson["idfacebook"].ToString()));
                int TotalScore = 0;
                for (int i = 0; i < TargetUser.MG_Scores.Count; i++)
                    TotalScore += Convert.ToInt32(TargetUser.MG_Scores[i].Score);
                TargetUser.AverageScore = (float)TotalScore / TargetUser.MG_Scores.Count;
                db.SubmitChanges();
                NotificationHelper.CreateNotification("You have new score: " + newScore.Score + "! ");
            }
            catch
            {

            }
        }
    }
}
