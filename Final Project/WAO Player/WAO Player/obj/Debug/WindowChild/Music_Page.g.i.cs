﻿#pragma checksum "F:\Cong Nghe Thong Tin\Develop for Window Phone\Final Project\WAO Player\WAO Player\WindowChild\Music_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "739549A2BF6B7ECC6A22331CED9B882A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
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


namespace WAO_Player.WindowChild {
    
    
    public partial class Music_Page : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ListBox List_Song_Offline;
        
        internal System.Windows.Controls.ListBox List_New_Song;
        
        internal System.Windows.Controls.TextBox Text_Search;
        
        internal System.Windows.Controls.Button Button_Search;
        
        internal System.Windows.Controls.ListBox List_Search_Song;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WAO%20Player;component/WindowChild/Music_Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.List_Song_Offline = ((System.Windows.Controls.ListBox)(this.FindName("List_Song_Offline")));
            this.List_New_Song = ((System.Windows.Controls.ListBox)(this.FindName("List_New_Song")));
            this.Text_Search = ((System.Windows.Controls.TextBox)(this.FindName("Text_Search")));
            this.Button_Search = ((System.Windows.Controls.Button)(this.FindName("Button_Search")));
            this.List_Search_Song = ((System.Windows.Controls.ListBox)(this.FindName("List_Search_Song")));
        }
    }
}

