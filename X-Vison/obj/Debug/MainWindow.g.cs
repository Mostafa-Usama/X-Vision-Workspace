﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8D3F2A7B17215BF64E7FB9352EFEAC709AC04EBF9C8BC1FA534DDDBB7D7F1887"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Center_Maneger;
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


namespace Center_Maneger {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 41 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl settings_tab;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn0;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn1;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn2;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn3;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn4;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn5;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn6;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn7;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn8;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl settings_controls;
        
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
            System.Uri resourceLocater = new System.Uri("/Center_Maneger;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            case 2:
            this.settings_tab = ((System.Windows.Controls.TabControl)(target));
            return;
            case 3:
            
            #line 56 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TabItem)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.settings_clicked);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 58 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.num_chairs_btn);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn0 = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\MainWindow.xaml"
            this.btn0.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btn1 = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\MainWindow.xaml"
            this.btn1.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btn2 = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\MainWindow.xaml"
            this.btn2.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btn3 = ((System.Windows.Controls.Button)(target));
            
            #line 82 "..\..\MainWindow.xaml"
            this.btn3.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btn4 = ((System.Windows.Controls.Button)(target));
            
            #line 88 "..\..\MainWindow.xaml"
            this.btn4.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn5 = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\MainWindow.xaml"
            this.btn5.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btn6 = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\MainWindow.xaml"
            this.btn6.Click += new System.Windows.RoutedEventHandler(this.openUserControl);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btn7 = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.btn8 = ((System.Windows.Controls.Button)(target));
            return;
            case 14:
            this.settings_controls = ((System.Windows.Controls.TabControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.close_tab);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

