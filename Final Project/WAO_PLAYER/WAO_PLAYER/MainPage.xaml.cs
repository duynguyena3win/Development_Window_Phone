using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WAO_PLAYER
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.GlobalAudioElement.Source = new Uri("http://stream1.nixcdn.com/7cd8121ae662acded52a63c51efc561d/52a92e01/NhacCuaTui846/TuNayVeSau-TrinhThangBinh-2874939_hq.mp3");
            App.GlobalAudioElement.Play();
        }
    }
}