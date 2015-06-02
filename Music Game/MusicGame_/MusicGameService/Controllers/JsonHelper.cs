using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicGameService.Models;

namespace MusicGameService.Controllers
{
    class JsonHelper
    {
        private static JsonHelper _Instance = new JsonHelper();
        private JsonHelper()
        {

        }

        public static JsonHelper Instance
        {
            get
            {
                return _Instance;
            }
        }

        public JToken ParseListObject(IEnumerable<object> projects)
        {
            try
            {
                List<object> arr = projects.ToList<object>();
                StringBuilder sb = new StringBuilder();
                ParseStringListObject(arr, sb);
                return JArray.Parse(sb.ToString());
            }
            catch { return JArray.Parse("{}"); }
        }

        public JToken ParseObject(object obj)
        {
            try
            { 
                return JObject.Parse(ParseStringObject(obj));
            }
            catch { return JArray.Parse("{}"); }
        }

        private void ParseStringListObject(List<object> arr, StringBuilder sb)
        {
            sb.Append("[");
            for (int i = 0; i < arr.Count(); i++)
            {
                sb.Append(ParseStringObject(arr[i]));
                if (i < arr.Count() - 1)
                    sb.Append(",\n");
            }
            sb.Append("]");
        }

        private string ParseStringObject(object obj)
        {
            if (obj.GetType() == typeof(MG_Question))
                return ParseStringProject((MG_Question)obj);

            if(obj.GetType() == typeof(MG_User))
                return ParseStringUser((MG_User)obj);

            if (obj.GetType() == typeof(MG_Challenge))
                return ParseStringChallenge((MG_Challenge)obj);
            return "{}";
        }

        private string ParseStringUser(MG_User p)
        {
            if (p == null)
            {
                return "{}";
            }

            string body = "{\nidUser:" + p.IdUser.ToString()
                + ", \nname:'" + p.Name.Trim()
                + "', \nlinkfacebook:'" + p.LinkFacebook.ToString()
                + "', \nidfacebook:'" + p.IdFacebook.ToString()
                + "', \nuserimage:'" + p.UserImage.ToString()
                + "', \naveragescore:" + p.AverageScore.ToString()
                + "\n}";

            return body;
        }

        private string ParseStringChallenge(MG_Challenge p)
        {
            if (p == null)
            {
                return "{}";
            }

            string body = "{\nidChallenge:" + p.IdChallenge.ToString()
                + ", \nscore:" + p.ScoreChallenge.ToString()
                + ", \ntime:" + p.TimeChallenge.ToString()
                + ", \nidFaceSend:'" + p.IdFaceSend.Trim()
                + "', \nnameSend:'" + p.NameSend.Trim()
                + "', \nlinkFaceSend:'" + p.LinkFaceSend.Trim()
                + "', \nidFaceReceive:'" + p.IdFaceReceive.Trim()
                + "', \nidQuestion:'" + p.IdQuestion.Trim()
                + "', \nidUserReceive:" + p.IdUserReceive.ToString()
                + "\n}";

            return body;
        }

        private string ParseStringProject(MG_Question p)
        {
            if (p == null)
            {
                return "{}";
            }

            string body = "{\nidQuestion:" + p.IdQuestion.ToString()
                + ", \ntype:" + p.Type.ToString()
                + ", \nsource:'" + p.Source.Trim()
                + "', \ntextQuestion:'" + p.TextQuestion.Trim()
                + "', \ndescription:'" + p.Description.Trim()
                + "', \na:'" + p.A.Trim()
                + "', \nb:'" + p.B.Trim()
                + "', \nc:'" + p.C.Trim()
                + "', \nd:'" + p.D.Trim()
                + "', \nanswer:'" + p.Answer.Trim()
                + "', \ngenre:'" + p.Genre.Trim()
                + "'\n}";

            return body;
        }
    }
}
