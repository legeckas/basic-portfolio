using System;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Basic_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double numOne = 0;
        double numTwo = 0;
        string operationType = String.Empty;
        bool toClearEntryBox = false;
        bool equaled = false;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(200, 400));
        }

        private void CharacterEntry(object sender, RoutedEventArgs e)
        {
            if (toClearEntryBox && (sender as Button).Name == "commaButton")
            {
                digitBox.Text = "0.";
                ClearMiscThings();
                
            }
            else if (toClearEntryBox)
            {
                digitBox.Text = String.Empty;
                ClearMiscThings();
            }

            if (digitBox.Text.Length == 16 && !digitBox.Text.Contains("."))
            {
                return;
            }
            else if (digitBox.Text.Contains(".") && digitBox.Text.Length == 17)
            {
                return;
            }
            else
            {
                if (digitBox.Text == "0")
                {
                    if ((sender as Button).Name == "commaButton")
                    {
                        digitBox.Text = "0.";
                    }
                    else
                    {
                        digitBox.Text = (sender as Button).Content.ToString();
                    }
                }
                else
                {
                    if ((sender as Button).Name == "commaButton" && digitBox.Text.Contains("."))
                    {
                        return;
                    }
                    else
                    {
                        digitBox.Text += (sender as Button).Content.ToString();
                    }
                }
            }
        }

        private void DeleteLast(object sender, RoutedEventArgs e)
        {
            if (digitBox.Text == "Cannot divide by zero")
            {
                digitBox.Text = "0";
            }

            digitBox.Text = digitBox.Text.Remove(digitBox.Text.Length - 1, 1);

            if (digitBox.Text == "-" || digitBox.Text == String.Empty)
            {
                digitBox.Text = "0";
            }

            if (equaled)
            {
                ClearMiscThings();
                operationType = String.Empty;
            }
        }

        private void ClearNumberBox(object sender, RoutedEventArgs e)
        {
            digitBox.Text = "0";
            numOne = 0;
            numTwo = 0;
            operationType = String.Empty;
            historyBox.Text = String.Empty;
            ClearMiscThings();
        }

        private void OperationSetter(object sender, RoutedEventArgs e)
        {
            if (digitBox.Text[digitBox.Text.Length - 1].ToString() == ".")
            {
                digitBox.Text = digitBox.Text.Remove(digitBox.Text.Length - 1, 1);
            }

            try
            { 
                numOne = double.Parse(digitBox.Text); 
            }
            catch (FormatException)
            {
                ClearMiscThings();
                digitBox.Text = "0";
                numOne = double.Parse(digitBox.Text);
            }

            toClearEntryBox = true;

            historyBox.Text = digitBox.Text + " " + (sender as Button).Content;

            switch ((sender as Button).Name)
            {
                case "divisionButton":
                    operationType = "division";
                    break;
                case "multiplicationButton":
                    operationType = "multiplication";
                    break;
                case "deductionButton":
                    operationType = "deduction";
                    break;
                case "additionButton":
                    operationType = "addition";
                    break;
                default:
                    break;
            }
        }

        private void EqualsOperation(object sender, RoutedEventArgs e)
        {
            if (digitBox.Text == "Cannot divide by zero")
            {
                return;
            }
            else if (equaled)
            {
                numOne = double.Parse(digitBox.Text);
                HistoryBoxSetter();
            }
            else if (operationType == String.Empty)
            {
                return;
            }
            else
            {
                numTwo = double.Parse(digitBox.Text);
                toClearEntryBox = true;
                HistoryBoxSetter();
            }

            switch (operationType)
            {
                case "division":
                    if (numTwo == 0)
                    {
                        digitBox.FontSize = 19;
                        digitBox.Text = "Cannot divide by zero";
                        equaled = true;
                        toClearEntryBox = true;
                    }
                    else
                    {
                        digitBox.Text = (numOne / numTwo).ToString();
                        equaled = true;
                    }
                    break;
                case "multiplication":
                    digitBox.Text = (numOne * numTwo).ToString();
                    equaled = true;
                    break;
                case "deduction":
                    digitBox.Text = (numOne - numTwo).ToString();
                    equaled = true;
                    break;
                case "addition":
                    digitBox.Text = (numOne + numTwo).ToString();
                    equaled = true;
                    break;
                default:
                    break;
            }
        }

        private void PositiveNegativeSetter(object sender, RoutedEventArgs e)
        {
            switch (digitBox.Text[0].ToString())
            {
                case "-":
                    digitBox.Text = digitBox.Text.Remove(0, 1);
                    break;
                case "0":
                    break;
                default:
                    digitBox.Text = digitBox.Text.Insert(0, "-");
                    break;
            }
        }

        private void ClearMiscThings()
        {
            if (equaled)
            {
                historyBox.Text = String.Empty;
            }

            toClearEntryBox = false;
            equaled = false;
            digitBox.FontSize = 22;
        }

        private void HistoryBoxSetter()
        {
            string operationSymbol = String.Empty;

            switch (operationType)
            {
                case "division":
                    operationSymbol = " ÷ ";
                    break;
                case "multiplication":
                    operationSymbol = " x ";
                    break;
                case "deduction":
                    operationSymbol = " - ";
                    break;
                case "addition":
                    operationSymbol = " + ";
                    break;
                default:
                    break;
            }

            historyBox.Text = numOne.ToString() + operationSymbol + numTwo.ToString() + " = ";
        }
    }
}
