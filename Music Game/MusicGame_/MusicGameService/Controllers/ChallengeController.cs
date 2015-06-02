using MusicGameService.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MusicGameService.Controllers
{
    public class ChallengeController : ApiController
    {
        MGDataContext db = new MGDataContext();

        [HttpPost]
        public bool PostChallenge(string idFaceSend, string nameSend, string linkFaceSend, string idFaceReceive, int score, int time, string listIdQuestion)
        {
            try
            {
                MG_Challenge myChallenge = new MG_Challenge();
                myChallenge.IdFaceSend = idFaceSend;
                myChallenge.IdFaceReceive = idFaceReceive;
                myChallenge.IdUserReceive = db.MG_Users.Single(x => x.IdFacebook == myChallenge.IdFaceReceive).IdUser;
                myChallenge.NameSend = nameSend;
                myChallenge.LinkFaceSend = linkFaceSend;
                myChallenge.ScoreChallenge = score;
                myChallenge.TimeChallenge = time;
                myChallenge.IdQuestion = listIdQuestion;
                db.MG_Challenges.InsertOnSubmit(myChallenge);
                db.SubmitChanges();
                NotificationHelper.CreateNotification("Your challenge have been sent to "+ db.MG_Users.Single(x => x.IdFacebook == idFaceReceive).Name +"! ");
                return true;
            }
            catch { return false; }
        }

        [HttpGet]
        public JToken GetListChallenge(string token)
        {
            return JsonHelper.Instance.ParseListObject(db.MG_Challenges.Where(x => x.IdFaceReceive == db.MG_Users.Single(m => m.AccessToken == token).IdFacebook));
        }

        [HttpDelete]
        public void DeleteChallenge(int idChallenge)
        {
            try
            {
                MG_Challenge TargetChallenge = db.MG_Challenges.Single(x => x.IdChallenge == idChallenge);
                db.MG_Challenges.DeleteOnSubmit(TargetChallenge);
                db.SubmitChanges();
            }
            catch
            {
                DeleteChallenge(idChallenge);
            }
        }

    }
}
