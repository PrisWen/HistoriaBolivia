﻿

#pragma checksum "C:\Users\Priscila Wendy\Documents\GitHub\HistoriaBolivia\OurHistory\UserControls\PreguntaControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D95B88E8D284BD2A72E2A3999246002A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OurHistory.UserControls
{
    partial class PreguntaControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 41 "..\..\UserControls\PreguntaControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Tapped += this.btnSiguiente_Tapped;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 30 "..\..\UserControls\PreguntaControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.lstOpciones_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


