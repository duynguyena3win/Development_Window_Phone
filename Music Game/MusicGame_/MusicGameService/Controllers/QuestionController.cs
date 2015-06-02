using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicGameService.Models;
using System.Web.Http;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MusicGameService.Controllers
{
    public class QuestionController : ApiController
    {
        MGDataContext db = new MGDataContext();

        [HttpGet]
        public JToken GetListQuestion(string listIdQuestion)
        {
            List<int> ListIdQuestion = new List<int>();
            string num = string.Empty;
            for (int i = 0; i < listIdQuestion.Length; i++)
            {
                if (listIdQuestion[i] == ',')
                {
                    ListIdQuestion.Add(Convert.ToInt32(num));
                    num = string.Empty;
                }
                else
                    num += listIdQuestion[i];
            }

            ObservableCollection<MG_Question> ListQuestion = new ObservableCollection<MG_Question>();
            for (int i = 0; i < ListIdQuestion.Count;i++ )
            {
                ListQuestion.Add(db.MG_Questions.Single(x => x.IdQuestion == ListIdQuestion[i]));
            }
            return JsonHelper.Instance.ParseListObject(ListQuestion);
        }

        [HttpGet]
        public JToken GetQuestions()
        {
            List<MG_Question> myList = new List<MG_Question>();

            List<MG_Question> TargetList = db.MG_Questions.ToList();
            Random rand = new Random();
            MG_Question targetQues;
            do {
                int val = rand.Next(0, TargetList.Count() - 1);
                targetQues = TargetList[val];
                if (CheckQuestions(myList, targetQues))
                    myList.Add(targetQues);
            } while (myList.Count < 10);

            return JsonHelper.Instance.ParseListObject(myList);
        }

        public JToken GetQuestionsByGenre(string genre)
        {
            List<MG_Question> myList = new List<MG_Question>();
            List<MG_Question> TargetList = db.MG_Questions.Where(x => x.Genre == genre).ToList();

            Random rand = new Random();
            MG_Question targetQues;
            do
            {
                int val = rand.Next(0, TargetList.Count()-1);
                targetQues = TargetList[val];
                if (CheckQuestions(myList, targetQues))
                    myList.Add(targetQues);
            } while (myList.Count < 10);

            return JsonHelper.Instance.ParseListObject(myList);
        }
        private bool CheckQuestions(List<MG_Question> list, MG_Question item)
        {
            for(int i=0; i<list.Count; i++)
            {
                if (list[i].IdQuestion == item.IdQuestion)
                    return false;
            }
            return true;
        }
    }

    
}