﻿#pragma checksum "C:\Users\Duy Nguyen\Desktop\appboy-windows-phone-ui-master\appboy-windows-phone-ui-master\Controls\Slideup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8AD5E72B7EB0BC3C5BDEA291AA675F40"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AppboyUI.Phone.Controls {
    
    
    public partial class Slideup : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.TextBlock Message;
        
        internal System.Windows.Controls.Image Chevron;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/AppboyUI.Phone;component/Controls/Slideup.xaml", System.UriKind.Relative));
            this.Message = ((System.Windows.Controls.TextBlock)(this.FindName("Message")));
            this.Chevron = ((System.Windows.Controls.Image)(this.FindName("Chevron")));
        }
    }
}

