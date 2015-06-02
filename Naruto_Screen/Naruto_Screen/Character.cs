using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto_Screen
{
    public class Character : INotifyPropertyChanged
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged("Name"); }
        }

        ObservableCollection<ItemView> list_Image = new ObservableCollection<ItemView>();

        public ObservableCollection<ItemView> List_Image
        {
            get { return list_Image; }
            set { list_Image = value; NotifyPropertyChanged("List_Image"); }
        }

        string image_Charater;

        public string Image_Charater
        {
            get { return image_Charater; }
            set { image_Charater = value; NotifyPropertyChanged("Image_Charater"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void Load_Itachi()
        {
            Name = "Itachi Uchiha";
            string Path = @"\Assets\Picture\ItachiUchiha\";
            Image_Charater = Path + "Itachi_Icon.jpg";
            for (int i = 1; i <= 15; i++)
                List_Image.Add(new ItemView() { Name = "Itachi Uchiha", ImageSource = Path + "Itachi" + i.ToString() + ".jpg" });
        }

        void Load_Sasuke()
        {
            Name = "Sasuke Uchiha";
            string Path = @"\Assets\Picture\SasukeUchiha\";
            Image_Charater = Path + "Sasuke_Icon.jpg";
            for (int i = 1; i <= 21; i++)
                List_Image.Add(new ItemView() { Name = "Sasuke Uchiha", ImageSource = Path + "Sasuke" + i.ToString() + ".jpg" });
        }

        void Load_Madara()
        {
            Name = "Madara Uchiha";
            string Path = @"\Assets\Picture\MadaraUchiha\";
            Image_Charater = Path + "Madara_Icon.jpg";
            for (int i = 1; i <= 16; i++)
                List_Image.Add(new ItemView() { Name = "Madara Uchiha", ImageSource = Path + "Madara" + i.ToString() + ".jpg" });
        }

        void Load_Shisui()
        {
            Name = "Shisui Uchiha";
            string Path = @"\Assets\Picture\ShisuiUchiha\";
            Image_Charater = Path + "Shisui_Icon.jpg";
            for (int i = 1; i <= 3; i++)
                List_Image.Add(new ItemView() { Name = "Shisui Uchiha", ImageSource = Path + "Shisui" + i.ToString() + ".jpg" });
        }

        void Load_Obito()
        {
            Name = "Obito Uchiha";
            string Path = @"\Assets\Picture\ObitoUchiha\";
            Image_Charater = Path + "Obito_Icon.jpg";
            for (int i = 1; i <= 5; i++)
                List_Image.Add(new ItemView() { Name = "Obito Uchiha", ImageSource = Path + "Obito" + i.ToString() + ".jpg" });
        }

        public Character(string n)
        {
            switch (n)
            {
                case "Itachi Uchiha":
                    Load_Itachi();
                    break;
                case "Sasuke Uchiha":
                    Load_Sasuke();
                    break;
                case "Shisui Uchiha":
                    Load_Shisui();
                    break;
                case "Obito Uchiha":
                    Load_Obito();
                    break;
                case "Madara Uchiha":
                    Load_Madara();
                    break;
            }
        }
    }
}
