using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;
using MusicGameService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;

namespace MusicGameService.Controllers
{
    public class UserController : ApiController
    {
        public ApiServices Services { get; set; }

        MGDataContext db = new MGDataContext();

        [HttpPost]
        public void PostNewUser(JToken myJson)
        {
            MG_User newUser = new MG_User();
            newUser.Name = myJson["name"].ToString();
            newUser.IdFacebook = myJson["idfacebook"].ToString();
            newUser.AccessToken = myJson["token"].ToString();
            bool isHave = false;
            try
            {
                MG_User temp = db.MG_Users.Single(x => x.IdFacebook == newUser.IdFacebook);
                temp.AccessToken = myJson["token"].ToString();
                db.SubmitChanges();
                isHave = true;
            }
            catch
            {
            }


            string str_message = string.Empty;
            if (isHave)
            {
                NotificationHelper.CreateNotification("Welcome " + newUser.Name + ", come back Music Game! ");
            }
            else
            {
                NotificationHelper.CreateNotification("Welcome " + newUser.Name + ", come to with Music Game! ");
                
                newUser.LinkFacebook = myJson["linkfacebook"].ToString();
                newUser.UserImage = "";
                newUser.AverageScore = 0.0f;
                db.MG_Users.InsertOnSubmit(newUser);
                db.SubmitChanges();
            }
            
        }

        [HttpGet]
        public JToken GetTopUser()
        {
            return JsonHelper.Instance.ParseListObject(db.MG_Users.OrderByDescending(x => x.AverageScore).Take(5));
        }

        [HttpGet]
        public JToken Get10User(int firstOffset)
        {
            List<MG_User> myList = new List<MG_User>();

            int firstPos = (firstOffset - 1) * 10;
            int secPos = (firstPos + 10 > db.MG_Users.Count()) ? db.MG_Users.Count() : firstPos + 10;
            if(secPos < firstPos)
            {
                string body = "{\nisbool: 'false'\n}";
                return JObject.Parse(body);
            }

            for (int i = firstPos; i < secPos; i++)
            {
                if (db.MG_Users.ToList()[i].IdFacebook != null)
                {
                    myList.Add(db.MG_Users.ToList()[i]);
                }
                else
                    break;
            }
            return JsonHelper.Instance.ParseListObject(myList);
        }

        [HttpGet]
        public JToken GetListChallenge(string token)
        {
            string idFacebook = db.MG_Users.Single(x => x.AccessToken == token).IdFacebook;
            List<MG_Challenge> mylist = db.MG_Challenges.Where(x => x.IdFaceReceive == idFacebook).ToList();
            return JsonHelper.Instance.ParseListObject(mylist);
        }

        [HttpPost]
        public void PostWinLose(bool isWin, string idfacebook)
        {
            MG_User TargetUser = db.MG_Users.Single(x => x.IdFacebook == idfacebook);
            if (isWin)
                TargetUser.Win++;
            else
                TargetUser.Lose++;
            db.SubmitChanges();

            
        }
    }
}

