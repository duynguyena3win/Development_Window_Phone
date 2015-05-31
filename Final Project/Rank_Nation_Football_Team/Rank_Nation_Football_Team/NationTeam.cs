using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Rank_Nation_Football_Team
{
    public class NationTeam : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string name_Nation;

        public string Name_Nation
        {
            get { return name_Nation; }
            set {
                switch (value)
                {
                    case "Việt Nam":
                        name_Nation = "Vietnam";
                        Head_Coach = "Hoàng Văn Phúc";
                        Flag_Team = @"Assets/Vietnam.jpg";
                        Stadium_Nation = "Mỹ Đình National Stadium";
                        Year_Join_Fifa = "1964";
                        Year_Create = "1960";
                        President_Nation_Team = "Lê Hùng Dũng";
                        break;
                    case "Thái Lan ":
                        name_Nation = "Thailand";
                        Head_Coach = "Kiatisuk Senamuang";
                        Flag_Team = @"Assets/Thailand.jpg";
                        Stadium_Nation = "Rajamangala Stadium";
                        Year_Join_Fifa = "1925";
                        Year_Create = "1916";
                        President_Nation_Team = "Vijit Getkaew";
                        break;
                    case "Lào ":
                        name_Nation = "Laos";
                        Head_Coach = "Kokichi Kimura";
                        Flag_Team = @"Assets/Laos.jpg";
                        Stadium_Nation = "New Laos National Stadium";
                        Year_Join_Fifa = "1952";
                        Year_Create = "1951";
                        President_Nation_Team = "Bountiem Phissamay";
                        break;
                    case "Campuchia ":
                        name_Nation = "Cambodia";
                        Head_Coach = "Lee Tae Hoon";
                        Flag_Team = @"Assets/Cambodia.jpg";
                        Stadium_Nation = "Olympic Stadium";
                        Year_Join_Fifa = "1953";
                        Year_Create = "1933";
                        President_Nation_Team = "Sao Sokha";
                        break;
                    case "Brunei Darussalam ":
                        name_Nation = "Brunei";
                        Head_Coach = "Vjeran Simunić";
                        Flag_Team = @"Assets/Brunei.jpg";
                        Stadium_Nation = "Sultan Hassanal Bolkiah Stadium";
                        Year_Join_Fifa = "1969";
                        Year_Create = "1959";
                        President_Nation_Team = "Pehin Dato' Haji Hussain Yussof";
                        break;
                    case "Đông Timor ":
                        name_Nation = "Timor-Leste";
                        Head_Coach = "Emerson Alcântara";
                        Flag_Team = @"Assets/Timor-Leste.jpg";
                        Stadium_Nation = "East Timor National Stadium";
                        Year_Join_Fifa = "2005";
                        Year_Create = "2000";
                        President_Nation_Team = "Francisco Kalbuadi Lay";
                        break;
                    case "Indonesia ":
                        name_Nation = "Indonesia";
                        Head_Coach = "Alfred Riedl";
                        Flag_Team = @"Assets/Indonesia.jpg";
                        Stadium_Nation = "Gelora Bung Karno Stadium";
                        Year_Join_Fifa = "1952";
                        Year_Create = "1930";
                        President_Nation_Team = "Djohar Arifin Husin";
                        break;
                    case "Philippines ":
                        name_Nation = "Philippines";
                        Head_Coach = "Thomas Dooley";
                        Flag_Team = @"Assets/Indonesia.jpg";
                        Stadium_Nation = "Rizal Memorial Stadium";
                        Year_Join_Fifa = "1928";
                        Year_Create = "1907";
                        President_Nation_Team = "Mariano Araneta";
                        break;
                    case "Malaysia ":
                        name_Nation = "Malaysia";
                        Head_Coach = "Ong Kim Swee";
                        Flag_Team = @"Assets/Malaysia.jpg";
                        Stadium_Nation = "National Stadium, Bukit Jalil";
                        Year_Join_Fifa = "1956";
                        Year_Create = "1933";
                        President_Nation_Team = "Vua Ahmad Shah";
                        break;
                    case "Singapore ":
                        name_Nation = "Singapore";
                        Head_Coach = "Bernd Stange";
                        Flag_Team = @"Assets/Singapore.jpg";
                        Stadium_Nation = "Jalan Besar Stadium";
                        Year_Join_Fifa = "1952";
                        Year_Create = "1892";
                        President_Nation_Team = "Ho Peng Kee";
                        break;
                    case "Myanmar ":
                        name_Nation = "Myanmar";
                        Head_Coach = "Radojko Avramovic";
                        Flag_Team = @"Assets/Myanmar.jpg";
                        Stadium_Nation = "Zayarthiri Stadium";
                        Year_Join_Fifa = "1957";
                        Year_Create = "1947";
                        President_Nation_Team = "U Zaw Zaw";
                        break;
                    default:
                        name_Nation = value;
                        break;
                }
                NotifyPropertyChanged("Name_Nation"); }
        }

        string head_Coach;

        public string Head_Coach
        {
            get { return head_Coach; }
            set { head_Coach ="Head Coach: " + value; NotifyPropertyChanged("Head_Coach"); }
        }
        string flag_Nation;

        public string Flag_Nation
        {
            get { return flag_Nation; }
            set { flag_Nation = value; NotifyPropertyChanged("Flag_Nation"); }
        }

        string flag_Team;

        public string Flag_Team
        {
            get { return flag_Team; }
            set { flag_Team = value; NotifyPropertyChanged("Flag_Team"); }
        }

        string year_Create;

        public string Year_Create
        {
            get { return year_Create; }
            set { year_Create = "Founded: " + value; NotifyPropertyChanged("Year_Create"); }
        }

        string year_Join_Fifa;

        public string Year_Join_Fifa
        {
            get { return year_Join_Fifa; }
            set { year_Join_Fifa ="FIFA join: " + value; NotifyPropertyChanged("Year_Join_Fifa"); }
        }

        string stadium_Nation;

        public string Stadium_Nation
        {
            get { return stadium_Nation; }
            set { stadium_Nation ="Stadium: " + value; NotifyPropertyChanged("Stadium_Nation"); }
        }

        string president_Nation_Team;

        public string President_Nation_Team
        {
            get { return president_Nation_Team; }
            set { president_Nation_Team = "President Team: " + value; NotifyPropertyChanged("President_Nation_Team"); }
        }

        int rank;

        public int Rank
        {
            get { return rank; }
            set { rank =  value; NotifyPropertyChanged("Rank"); }
        }

        string point_Rank;

        public string Point_Rank
        {
            get { return point_Rank; }
            set {
                if(value.IndexOf("Point") > 0)
                    point_Rank = value;
                else
                    point_Rank = "Point:  " + value;
                NotifyPropertyChanged("Point_Rank"); }
        }

        string url_PageNation;

        public string Url_PageNation
        {
            get { return url_PageNation; }
            set { url_PageNation = value; }
        }

        public NationTeam()
        {
        }
    }
}
