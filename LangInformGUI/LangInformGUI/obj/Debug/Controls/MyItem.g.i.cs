﻿#pragma checksum "..\..\..\Controls\MyItem.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5D09EB5612D323E16D9FC05DF960CBAD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LangInformGUI.Controls;
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


namespace LangInformGUI.Controls {
    
    
    /// <summary>
    /// MyItemControl
    /// </summary>
    public partial class MyItemControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMain;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid dimmer;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdControls;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border btnExclude;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Controls\MyItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LangInformGUI.Controls.CustomSlider track;
        
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
            System.Uri resourceLocater = new System.Uri("/LangInformGUI;component/controls/myitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\MyItem.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 7 "..\..\..\Controls\MyItem.xaml"
            ((LangInformGUI.Controls.MyItemControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded_1);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\Controls\MyItem.xaml"
            ((LangInformGUI.Controls.MyItemControl)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter_1);
            
            #line default
            #line hidden
            
            #line 7 "..\..\..\Controls\MyItem.xaml"
            ((LangInformGUI.Controls.MyItemControl)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseLeave_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grdMain = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.image = ((System.Windows.Controls.Image)(target));
            
            #line 10 "..\..\..\Controls\MyItem.xaml"
            this.image.Loaded += new System.Windows.RoutedEventHandler(this.image_Loaded);
            
            #line default
            #line hidden
            return;
            case 4:
            this.dimmer = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.grdControls = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.btnExclude = ((System.Windows.Controls.Border)(target));
            
            #line 20 "..\..\..\Controls\MyItem.xaml"
            this.btnExclude.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.track = ((LangInformGUI.Controls.CustomSlider)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

