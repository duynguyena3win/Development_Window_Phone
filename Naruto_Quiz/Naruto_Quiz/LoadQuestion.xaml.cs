using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Naruto_Quiz
{
    public partial class LoadQuestion : UserControl
    {
        public LoadQuestion()
        {
            InitializeComponent();
            Storyboard_Ready.Begin();
        }
        public delegate void Complete();
        public event Complete Complete_Animation;

        private void Storyboard_Ready_Completed(object sender, EventArgs e)
        {
            Player.Play();
            Storyboard_1.Begin();
        }

        private void Storyboard_1_Completed(object sender, EventArgs e)
        {
            Storyboard_2.Begin();
        }

        private void Storyboard_2_Completed(object sender, EventArgs e)
        {
            Storyboard_3.Begin();
        }

        private void Storyboard_3_Completed(object sender, EventArgs e)
        {
            Storyboard_GO.Begin();
        }

        private void Storyboard_GO_Completed(object sender, EventArgs e)
        {
            Complete_Animation();
        }
    }
}
