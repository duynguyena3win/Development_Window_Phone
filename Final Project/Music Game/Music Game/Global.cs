using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Music_Game
{
    public static class Global
    {
        public static string PROD_FILE = "XML/Data.xml";
        public static List<Type_Music> List_Data = new List<Type_Music>();

        public static int Total_Score = 0;
        public static int Total_Time = 0;
        
        public static List<Question> ListQuestion;
        public static int Current_Question = 0;

        public static int Current_Type = 0;
        public static int Current_Task = 0;

        public static IEnumerable Load_Data_Game()
        {
            var Document = XDocument.Load(Global.PROD_FILE);
            XDocument xml = (XDocument)Document;
            //var ele_node = from element in doc.Elements("WAO_PLAYER").Elements("List_Playlist").Elements("Playlist")
            //               select element;
            var Types = from elem in Document.Descendants("TYPE")
                        //orderby elem.Attribute("name").Value
                        select new Type_Music
                        {
                            NameType = elem.Attribute("name").Value,
                            Image = elem.Attribute("Image").Value,
                        };

            foreach (Type_Music type in Types)
            {
                Type_Music temp_Type = new Type_Music(type.NameType, type.Image);

                var Ele_Tasks = from element in Document.Descendants("TASK")
                          where element.Parent.Attribute("name").Value == temp_Type.NameType
                          select element;

                foreach (var task in Ele_Tasks)
                {
                    Task temp_Task = new Task();
                    temp_Task.STT = task.Attribute("Ma").Value;
                    var Ele_Questions = from element in Document.Descendants("QUESTION")
                                        where element.Parent.Attribute("Ma").Value == temp_Task.STT &&
                                        element.Parent.Parent.Attribute("name").Value == temp_Type.NameType
                                        select element;
                    foreach (var ques in Ele_Questions)
                    {
                        string[] strs = {ques.Attribute("A").Value,ques.Attribute("B").Value,ques.Attribute("C").Value,ques.Attribute("D").Value};
                        Question temp_ques = new Question(ques.Attribute("Name").Value, ques.Attribute("Answer").Value, strs, ques.Attribute("URL").Value);
                        temp_Task.List_Question.Add(temp_ques);
                    }
                    temp_Type.List_Task.Add(temp_Task);
                }

                List_Data.Add(temp_Type);
            }
            return Types;
        }
        public static void LoadGame(List<Question> list)
        {
            Global.Total_Score = Global.Current_Question = 0;
            Global.ListQuestion = new List<Question>();
            Global.ListQuestion = list;
        }

    }
}
