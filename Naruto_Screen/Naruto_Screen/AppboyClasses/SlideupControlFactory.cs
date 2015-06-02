using AppboyPlatform.PCL.Models.Incoming;
using AppboyPlatform.Phone.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Total_Feed.AppboyClasses
{
    public class SlideupControlFactory : ISlideupControlFactory
    {
        public UserControl GetSlideupControl(Slideup slideup)
        {
            var control = new CustomSlideupControl();
            if (slideup.ClickAction == ClickAction.NONE)
            {
                control.Chevron.Visibility = Visibility.Collapsed;
            }
            else
            {
                control.Chevron.Visibility = Visibility.Visible;
            }

            StringBuilder message = new StringBuilder();
            message.Append(slideup.Message);
            if (slideup.Extras != null && slideup.Extras.Count > 0)
            {
                message.AppendLine(Environment.NewLine);
                foreach (var extra in slideup.Extras)
                {
                    message.AppendFormat("Extra key {0} has value {1}." + Environment.NewLine, extra.Key, extra.Value);
                }
            }
            control.Message.Text = message.ToString();
            return control;
        }
    }
}
