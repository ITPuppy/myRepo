﻿#pragma checksum "..\..\..\UI\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "20DED70F65ADD0CAADF1A8BBECF26AF9"
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
using System.Windows.Shell;


namespace Chatter.MetroClient.UI {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LeftGrid;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnFriendGrid;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnGroupGrid;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnRecentFriendGrid;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid btnSetting;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MiddleGrid;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtNickName;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\UI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RightGrid;
        
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
            
            #line 11 "..\..\..\UI\MainWindow.xaml"
            ((Chatter.MetroClient.UI.MainWindow)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.MainWindow_Drag);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\UI\MainWindow.xaml"
            ((Chatter.MetroClient.UI.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MainWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LeftGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.btnFriendGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 44 "..\..\..\UI\MainWindow.xaml"
            this.btnFriendGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnGroupGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 49 "..\..\..\UI\MainWindow.xaml"
            this.btnGroupGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRecentFriendGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 54 "..\..\..\UI\MainWindow.xaml"
            this.btnRecentFriendGrid.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.SelectMode_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnSetting = ((System.Windows.Controls.Grid)(target));
            
            #line 58 "..\..\..\UI\MainWindow.xaml"
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
            }
            this._contentLoaded = true;
        }
    }
}

