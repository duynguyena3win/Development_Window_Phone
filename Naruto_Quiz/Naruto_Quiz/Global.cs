using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Naruto_Quiz
{
    public static class Global
    {
        public static string Path_File = @"Assets\XML_Data\DataQuestion_Naruto.xml";
        public static string Name_Exam = null;
        public static List<VisibleQuestion> Questions;
        public static List<VisibleQuestion> Current_Questions;
        public static Random rand = new Random();
        public static int Current_Index = -1;
        public static int Level = -1;
        public static int STT = 0;
        public static Final_Score F_Score = new Final_Score();
        public static void Load_Question()
        {
            Questions = new List<VisibleQuestion>();
            XDocument Document = XDocument.Load(Path_File);

            //Load Image Question
            var List_Element_Image = from element in Document.Descendants("QUESTION_IMAGE")
                                     select element;
            foreach (var node in List_Element_Image)
            {
                VisibleQuestion temp = new Image_Question();
                temp.Id = STT++;
                temp.Question = node.Attribute("Question").Value;
                temp.Answer = node.Attribute("Answer").Value;
                temp.Answer_A = node.Attribute("A").Value;
                temp.Answer_B = node.Attribute("B").Value;
                temp.Answer_C = node.Attribute("C").Value;
                temp.Answer_D = node.Attribute("D").Value;
                temp.Hard = Convert.ToInt32(node.Attribute("Hard").Value);
                ((Image_Question)temp).Question_Image = node.Attribute("Image").Value;
                Global.Questions.Add(temp);
            }

            // Load Sound Question
            var List_Element_Sound = from element in Document.Descendants("QUESTION_SOUND")
                                     select element;
            foreach (var node in List_Element_Sound)
            {
                VisibleQuestion temp = new Sound_Question();
                temp.Id = STT++;
                temp.Question = node.Attribute("Question").Value;
                temp.Answer = node.Attribute("Answer").Value;
                temp.Answer_A = node.Attribute("A").Value;
                temp.Answer_B = node.Attribute("B").Value;
                temp.Answer_C = node.Attribute("C").Value;
                temp.Answer_D = node.Attribute("D").Value;
                temp.Hard = Convert.ToInt32(node.Attribute("Hard").Value);
                ((Sound_Question)temp).Question_Sound = node.Attribute("Sound").Value;
                Global.Questions.Add(temp);
            }
        }

        public static void Load_10_Question(int level)
        {
            Global.Level = level;
            Global.Current_Questions = new List<VisibleQuestion>();
            if (Global.Current_Questions.Count != 0)
            {
                Global.Current_Questions.Clear();
                Global.Current_Index = -1;
            }

            //while (Global.Current_Questions.Count < 10)
            //{
            //    int i;
            //    do
            //    {
            //        i = rand.Next() % Questions.Count;
            //    } while (Global.Check_Question(Questions[i],level) == false);

            //    Current_Questions.Add(Questions[i]);
            //    if (Questions.Count < 10)
            //        break;
            //}
            Current_Questions.Add(Questions[Questions.Count - 1]);
        }

        public static bool Check_Question(VisibleQuestion VQ, int level)
        {
            if (VQ.Hard > level || VQ.Hard < level-1)
                return false;
            for (int i = 0; i < Current_Questions.Count; i++)
                if (Current_Questions[i].Id == VQ.Id)
                    return false;
            return true;
        }
    }
}
