using Music_Game.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music_Game
{
    public static class ConnectServiceHelper
    {
        public static async Task<ObservableCollection<QuestionDAO>> LoadListQuestion()
        {
            ObservableCollection<QuestionDAO> myList = new ObservableCollection<Models.QuestionDAO>();
            try
            {
                var List = await App.MobileService.InvokeApiAsync("Question", HttpMethod.Get, null);
                if (List != null)
                {
                    myList.Clear();
                    foreach (var item in List)
                    {
                        myList.Add(new Models.QuestionDAO(item));
                    }
                    return myList;
                    // NextQuestion();
                }
                myList.Clear();
                return myList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("! ! ! Loading Question in Service ! ! ! " + ex.Message);
                return ConnectServiceHelper.LoadListQuestion().Result;
            }
        }

        public async static Task<ObservableCollection<UserMusicDAO>> LoadPlayer(int CurrentPage)
        {
            ObservableCollection<UserMusicDAO>  ListPlayer = new ObservableCollection<UserMusicDAO>();
            try
            {
                Dictionary<string, string> myDic = new Dictionary<string, string>();
                myDic.Add("firstOffset", CurrentPage.ToString());
                var myuser = await App.MobileService.InvokeApiAsync("User", HttpMethod.Get, myDic);

                if (myuser != null)
                {
                    try
                    {
                        if (Convert.ToBoolean(myuser["isbool"].ToString()) == false)
                        {
                            return null;
                        }
                    }
                    catch { }

                    if (myuser != null)
                    {
                        foreach (JToken item in myuser)
                        {
                            ListPlayer.Add(new UserMusicDAO(item));
                        }
                    }
                    //ListBox_Player.DataContext = ListPlayer;
                    return ListPlayer;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async static void CreateUser(string idface, string linkface, string name, string token)
        {
            try
            {
                string body = "{\nname:'" + name.Trim()
                        + "', \nlinkfacebook:'" + linkface.Trim()
                        + "', \nidfacebook:'" + idface.Trim()
                        + "', \ntoken:'" + token.Trim()
                        + "'\n}";

                await App.MobileService.InvokeApiAsync("User", JObject.Parse(body));
            }
            catch { }
        }
        
        public async static Task<ObservableCollection<UserMusicDAO>> LoadTopScore()
        {
            ObservableCollection<UserMusicDAO> ListUser = new ObservableCollection<UserMusicDAO>();
            try
            {
                ListUser.Clear();

                var myuser = await App.MobileService.InvokeApiAsync("User", HttpMethod.Get, null);
                if (myuser != null)
                {
                    foreach (JToken item in myuser)
                    {
                        ListUser.Add(new UserMusicDAO(item));
                    }

                    for (int i = 0; i < ListUser.Count(); i++)
                    {
                        for (int j = i + 1; j < ListUser.Count(); j++)
                        {
                            if (ListUser[i].AverageScore < ListUser[j].AverageScore)
                            {
                                UserMusicDAO temp = ListUser[i];
                                ListUser[i] = ListUser[j];
                                ListUser[j] = temp;
                            }
                        }
                    }
                    return ListUser;
                }
                ListUser.Clear();
                return ListUser;
            }
            catch
            {
                ListUser.Clear();
                return ListUser;
            }
        }

        public async static Task<ObservableCollection<QuestionDAO>> LoadQuestionByGenre(string genre)
        {
            ObservableCollection<QuestionDAO> ListQuestion = new ObservableCollection<QuestionDAO>();
            try
            {

                Dictionary<string, string> myDic = new Dictionary<string, string>();
                myDic.Add("genre", genre);

                var myQues = await App.MobileService.InvokeApiAsync("Question", HttpMethod.Get, myDic);

                if (myQues != null)
                {
                    if (ListQuestion.Count != 0)
                        ListQuestion.Clear();
                    ListQuestion.Clear();
                    foreach (var item in myQues)
                    {
                        ListQuestion.Add(new Models.QuestionDAO(item));
                    }
                    return ListQuestion;
                }
                ListQuestion.Clear();
                return ListQuestion;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("! ! ! Loading Question in Service ! ! ! " + ex.Message);
                return ConnectServiceHelper.LoadQuestionByGenre(genre).Result;
            }
        }

        public async static Task<bool> ChallengePlayer(Dictionary<string, string> infoSend)
        {
            bool bSuccess;
            bSuccess = await App.MobileService.InvokeApiAsync<bool>("Challenge", HttpMethod.Post, infoSend);
            return bSuccess;
        }

        public async static Task<ObservableCollection<ChallengeDAO>> GetChallenges(string token)
        {
            ObservableCollection<ChallengeDAO> ListChallenge = new ObservableCollection<ChallengeDAO>();
            try
            {
                var myList = await App.MobileService.InvokeApiAsync<JToken>("Challenge",
                    HttpMethod.Get,
                    new Dictionary<string, string>() { { "token", token } });
                if (myList != null)
                {
                    foreach(var item in myList)
                    {
                        ListChallenge.Add(new ChallengeDAO(item));
                    }
                    return ListChallenge;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("!!! Error when load Challenge Quest !!! " + ex.Message);
                return GetChallenges(token).Result;
            }
        }

        public async static Task<ObservableCollection<QuestionDAO>> GetListQuestionChallenge(string listIdQuestion)
        {
            ObservableCollection<QuestionDAO> myList = new ObservableCollection<Models.QuestionDAO>();
            try
            {
                Dictionary<string, string> myInfo = new Dictionary<string, string>();
                myInfo.Add("listIdQuestion", listIdQuestion);
                var List = await App.MobileService.InvokeApiAsync("Question", HttpMethod.Get, myInfo);
                if (List != null)
                {
                    myList.Clear();
                    foreach (var item in List)
                    {
                        myList.Add(new Models.QuestionDAO(item));
                    }
                    return myList;
                }
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("! ! ! Loading Question in Service ! ! ! " + ex.Message);
                return ConnectServiceHelper.GetListQuestionChallenge(listIdQuestion).Result;
            }
        }

        public async static void UpdateScore(int Score, int Time, string idfacebook)
        {
            try
            {
                string body = "{\nscore:" + Score.ToString()
                + ", \ntime:" + Time.ToString()
                + ", \nidfacebook:'" + idfacebook.Trim()
                + "'\n}";
                await App.MobileService.InvokeApiAsync("Score", JObject.Parse(body));
            }
            catch
            {
                Debug.WriteLine("!! Have Error When Update Score !");
            }
        }

        public async static void UpdateWinLose(bool IsWin, string idfacebook)
        {
            try
            {
                Dictionary<string, string> myInfo = new Dictionary<string,string>();
                myInfo.Add("isWin", IsWin.ToString());
                myInfo.Add("idfacebook", idfacebook);
                await App.MobileService.InvokeApiAsync("User", HttpMethod.Post, myInfo);
            }
            catch
            {

            }
        }

        public async static void DeleteChallenge(int idChallenge)
        {
            try
            {
                Dictionary<string, string> myInfo = new Dictionary<string,string>();
                myInfo.Add("idChallenge", idChallenge.ToString());
                await App.MobileService.InvokeApiAsync("Challenge", HttpMethod.Delete, myInfo);
            }
            catch { }
        }
        
    }
}
