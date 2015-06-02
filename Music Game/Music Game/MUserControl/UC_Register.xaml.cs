using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Music_Game.MUserControl
{
    public partial class UC_Register : UserControl
    {
        public UC_Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Edit_Name.Text != string.Empty && Edit_UName.Text.Length >= 6 &&
                Edit_UName.Text != string.Empty && CheckPassword())
            {
                Btn_Register.IsEnabled = true;
            }
            else
                Btn_Register.IsEnabled = false;
        }

        bool CheckPassword()
        {
            if (Edit_Password.Text.Length >= 6 && Edit_CPassword.Text.Length >= 6 &&
                Edit_Password.Text == Edit_CPassword.Text && checkSpace(Edit_Password.Text))
                return true;
            return false;
        }

        private void Edit_UName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text_Info.Text = "User Name must have least 6 letters and no space!";
            Edit_UName.Background = GetBackground(Edit_UName.Text.Length >= 6 && checkSpace(Edit_UName.Text));
        }

        bool checkSpace(string myValue)
        {
            for (int i = 0; i < myValue.Length; i++)
                if (myValue[i] == ' ')
                    return false;
            return true;
        }

        private void Edit_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Edit_Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text_Info.Text = "Password must have least 6 letters and no space!";
            Edit_Password.Background = GetBackground(CheckPassword());
        }

        private void Edit_CPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text_Info.Text = "Confirm Password must have least 6 letters and no space!";
            Edit_CPassword.Background = GetBackground(CheckPassword());
        }

        public static ImageBrush GetBackground(bool isTrue)
        {
            var imageBrush = new ImageBrush();

            if (isTrue)
            {
                imageBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("Assets/ButtonImages/accept.png", UriKind.Relative))
                };
            }
            else
            {
                imageBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri("Assets/ButtonImages/deni.png", UriKind.Relative))
                };
            }

            return imageBrush;
        }
    }
}
