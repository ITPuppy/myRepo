﻿#pragma checksum "..\..\..\UI\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F1ED4BB107CF9AA1E24B90138A53CE3D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Chatter.MetroClient.UI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Chatter.MetroClient.UI {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 49 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LeftGrid;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnFriendGrid;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnGroupGrid;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnRecentFriendGrid;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnSetting;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MiddleGrid;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtNickName;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RightGrid;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl mesgTabControl;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MetroClient;component/ui/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UI\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\UI\MainWindow.xaml"
            ((Chatter.MetroClient.UI.MainWindow)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.MainWindow_Drag);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LeftGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.btnFriendGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 64 "..\..\..\UI\MainWindow.xaml"
            this.btnFriendGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnGroupGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 69 "..\..\..\UI\MainWindow.xaml"
            this.btnGroupGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRecentFriendGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 74 "..\..\..\UI\MainWindow.xaml"
            this.btnRecentFriendGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnSetting = ((System.Windows.Controls.Grid)(target));
            
            #line 78 "..\..\..\UI\MainWindow.xaml"
            this.btnSetting.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.MiddleGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.txtNickName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.RightGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.mesgTabControl = ((System.Windows.Controls.TabControl)(target));
            return;
            case 11:
            
            #line 135 "..\..\..\UI\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.closeBtn_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 137 "..\..\..\UI\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.minBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

