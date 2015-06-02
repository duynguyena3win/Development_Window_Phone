using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicLib
{
    public class ImageQuestion : AbstractQuestion
    {
        public ImageQuestion()
        {
            string abc = "{'IsMember' : true, 'Name' : 'John', 'Age' : 24}";
            JToken m_Json = JArray.Parse(abc);
            string name = m_Json["Name"].ToString();
            int age = Convert.ToInt32(m_Json["Age"]);
            bool ismem = Convert.ToBoolean(m_Json["IsMember"]);
        }

        public override string GetType()
        {
            return "Image";
        }
    }
}
