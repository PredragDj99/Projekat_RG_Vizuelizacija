﻿#pragma checksum "..\..\Poligon.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7C44BDB40ED6F8F8B72968753E3C67C9E07A528F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using Vizuelizacija;


namespace Vizuelizacija {
    
    
    /// <summary>
    /// Poligon
    /// </summary>
    public partial class Poligon : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox debljinaKonturneLinije;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tekstUnutarPoligona;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox providnost;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblKonturnaLinijaGreska;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblcmdGreska;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblProvidnost;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbColor;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Poligon.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbColor2;
        
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
            System.Uri resourceLocater = new System.Uri("/Vizuelizacija;component/poligon.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Poligon.xaml"
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
            this.debljinaKonturneLinije = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.tekstUnutarPoligona = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.providnost = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.lblKonturnaLinijaGreska = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lblcmdGreska = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.lblProvidnost = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            
            #line 25 "..\..\Poligon.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NacrtajPoligon_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cmbColor = ((System.Windows.Controls.ComboBox)(target));
            
            #line 26 "..\..\Poligon.xaml"
            this.cmbColor.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbColor_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.cmbColor2 = ((System.Windows.Controls.ComboBox)(target));
            
            #line 36 "..\..\Poligon.xaml"
            this.cmbColor2.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbColor_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

