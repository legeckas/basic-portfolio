﻿#pragma checksum "C:\Users\MIND\Documents\C# Projects\Simple Calculator\Simple Calculator\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6A13E7BF038EE081175258CD8D49CD8E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Simple_Calculator
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 14
                {
                    this.sevenButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.sevenButton).Click += this.CharacterEntry;
                }
                break;
            case 3: // MainPage.xaml line 15
                {
                    this.eightButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.eightButton).Click += this.CharacterEntry;
                }
                break;
            case 4: // MainPage.xaml line 16
                {
                    this.nineButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.nineButton).Click += this.CharacterEntry;
                }
                break;
            case 5: // MainPage.xaml line 17
                {
                    this.deleteButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.deleteButton).Click += this.DeleteLast;
                }
                break;
            case 6: // MainPage.xaml line 18
                {
                    this.fourButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.fourButton).Click += this.CharacterEntry;
                }
                break;
            case 7: // MainPage.xaml line 19
                {
                    this.fiveButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.fiveButton).Click += this.CharacterEntry;
                }
                break;
            case 8: // MainPage.xaml line 20
                {
                    this.sixButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.sixButton).Click += this.CharacterEntry;
                }
                break;
            case 9: // MainPage.xaml line 21
                {
                    this.divisionButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.divisionButton).Click += this.OperationSetter;
                }
                break;
            case 10: // MainPage.xaml line 22
                {
                    this.oneButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.oneButton).Click += this.CharacterEntry;
                }
                break;
            case 11: // MainPage.xaml line 23
                {
                    this.twoButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.twoButton).Click += this.CharacterEntry;
                }
                break;
            case 12: // MainPage.xaml line 24
                {
                    this.threeButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.threeButton).Click += this.CharacterEntry;
                }
                break;
            case 13: // MainPage.xaml line 25
                {
                    this.multiplicationButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.multiplicationButton).Click += this.OperationSetter;
                }
                break;
            case 14: // MainPage.xaml line 26
                {
                    this.clearButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.clearButton).Click += this.ClearNumberBox;
                }
                break;
            case 15: // MainPage.xaml line 27
                {
                    this.zeroButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.zeroButton).Click += this.CharacterEntry;
                }
                break;
            case 16: // MainPage.xaml line 28
                {
                    this.positiveNegativeButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.positiveNegativeButton).Click += this.PositiveNegativeSetter;
                }
                break;
            case 17: // MainPage.xaml line 29
                {
                    this.deductionButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.deductionButton).Click += this.OperationSetter;
                }
                break;
            case 18: // MainPage.xaml line 30
                {
                    this.additionButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.additionButton).Click += this.OperationSetter;
                }
                break;
            case 19: // MainPage.xaml line 31
                {
                    this.equalsButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.equalsButton).Click += this.EqualsOperation;
                }
                break;
            case 20: // MainPage.xaml line 32
                {
                    this.commaButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.commaButton).Click += this.CharacterEntry;
                }
                break;
            case 21: // MainPage.xaml line 33
                {
                    this.historyBox = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 22: // MainPage.xaml line 34
                {
                    this.digitBox = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

