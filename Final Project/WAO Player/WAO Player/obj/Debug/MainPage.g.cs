﻿#pragma checksum "F:\Cong Nghe Thong Tin\Develop for Window Phone\Final Project\WAO Player\WAO Player\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0ADF51A0F3C6E99C7110A410A48EBB26"
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


namespace WAO_Player {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard Animation_Opicaty;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textBlock;
        
        internal System.Windows.Controls.Slider Slider_Time;
        
        internal System.Windows.Controls.Button Btn_Pre;
        
        internal System.Windows.Controls.Button Btn_Play;
        
        internal System.Windows.Controls.Button Btn_Next;
        
        internal System.Windows.Controls.TextBlock Text_Name;
        
        internal System.Windows.Controls.TextBlock TextBlock_Lyric;
        
        internal System.Windows.Controls.MediaElement Player;
        
        internal System.Windows.Controls.ListBox List_Song_Offline;
        
        internal Microsoft.Phone.Controls.ContextMenu MyContextMenu;
        
        internal System.Windows.Controls.TextBox Text_Search;
        
        internal System.Windows.Controls.Button Button_Search;
        
        internal System.Windows.Controls.ListBox List_Song_Online;
        
        internal System.Windows.Controls.ListBox List_Song_NowPlaying;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WAO%20Player;component/MainPage.xaml", System.UriKind.Relative));
            this.Animation_Opicaty = ((System.Windows.Media.Animation.Storyboard)(this.FindName("Animation_Opicaty")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.textBlock = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock")));
            this.Slider_Time = ((System.Windows.Controls.Slider)(this.FindName("Slider_Time")));
            this.Btn_Pre = ((System.Windows.Controls.Button)(this.FindName("Btn_Pre")));
            this.Btn_Play = ((System.Windows.Controls.Button)(this.FindName("Btn_Play")));
            this.Btn_Next = ((System.Windows.Controls.Button)(this.FindName("Btn_Next")));
            this.Text_Name = ((System.Windows.Controls.TextBlock)(this.FindName("Text_Name")));
            this.TextBlock_Lyric = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_Lyric")));
            this.Player = ((System.Windows.Controls.MediaElement)(this.FindName("Player")));
            this.List_Song_Offline = ((System.Windows.Controls.ListBox)(this.FindName("List_Song_Offline")));
            this.MyContextMenu = ((Microsoft.Phone.Controls.ContextMenu)(this.FindName("MyContextMenu")));
            this.Text_Search = ((System.Windows.Controls.TextBox)(this.FindName("Text_Search")));
            this.Button_Search = ((System.Windows.Controls.Button)(this.FindName("Button_Search")));
            this.List_Song_Online = ((System.Windows.Controls.ListBox)(this.FindName("List_Song_Online")));
            this.List_Song_NowPlaying = ((System.Windows.Controls.ListBox)(this.FindName("List_Song_NowPlaying")));
        }
    }
}

