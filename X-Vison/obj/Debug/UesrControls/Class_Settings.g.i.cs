﻿#pragma checksum "..\..\..\UesrControls\Class_Settings.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1F8C3632C9A63A1FC4215194247B60BA37EAA373611E2A9D903CA507C92A3555"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Center_Maneger.UesrControls;
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


namespace Center_Maneger.UesrControls {
    
    
    /// <summary>
    /// Class_Settings
    /// </summary>
    public partial class Class_Settings : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 25 "..\..\..\UesrControls\Class_Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name_of_class_input;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\UesrControls\Class_Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox cost_of_class_input;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\UesrControls\Class_Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid data_grid;
        
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
            System.Uri resourceLocater = new System.Uri("/Center_Maneger;component/uesrcontrols/class_settings.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UesrControls\Class_Settings.xaml"
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
            this.name_of_class_input = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.cost_of_class_input = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            
            #line 38 "..\..\..\UesrControls\Class_Settings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.remove_class_record);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 44 "..\..\..\UesrControls\Class_Settings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.save_class_record);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 50 "..\..\..\UesrControls\Class_Settings.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.new_class_record);
            
            #line default
            #line hidden
            return;
            case 6:
            this.data_grid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 59 "..\..\..\UesrControls\Class_Settings.xaml"
            this.data_grid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.change_selected_record);
            
            #line default
            #line hidden
            
            #line 59 "..\..\..\UesrControls\Class_Settings.xaml"
            this.data_grid.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.change_header_name);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

