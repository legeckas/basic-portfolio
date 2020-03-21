using RecipeBookLibrary;
using RecipeBookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RecipeBookUI
{
    /// <summary>
    /// Interaction logic for AddIngredientWindow.xaml
    /// </summary>
    public partial class AddIngredientWindow : Window
    {
        /// <summary>
        /// List pulled at initialization of already existing ingredient names.
        /// </summary>
        List<string> existingIngredients = GlobalConfig.Connection.Ingredients_GetAllNames();
        /// <summary>
        /// Window with interface that is calling AddIngredientWinwdow.
        /// </summary>
        IIngredientRequester callingWindow;

        public AddIngredientWindow(IIngredientRequester caller)
        {
            InitializeComponent();
            UpdateLists();

            addIngredientsNewNameTextBox.IsEnabled = false;

            callingWindow = caller;
        }

        /// <summary>
        /// Creates an IngredientModel instance and passes it in a database method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addIngredientsAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateWindow())
            {
                string ingredientName;

                if (addIngredientsNewCheckBox.IsChecked == true)
                {
                    ingredientName = addIngredientsNewNameTextBox.Text;
                }
                else
                {
                    ingredientName = addIngredientsAvailableComboBox.SelectedItem.ToString();
                }

                IngredientModel model = new IngredientModel(ingredientName, addIngredientsAmountTextBox.Text);

                callingWindow.Ingredient_Complete(model);
                this.Close();
            }
            
        }
        /// <summary>
        /// Ensures that the information has been entered correctly.
        /// </summary>
        /// <returns>Returns 'true' if all is entered correctly.</returns>
        private bool ValidateWindow()
        {
            bool output = true;

            if (addIngredientsAvailableComboBox.SelectedItem == null && (addIngredientsNewCheckBox.IsChecked == false && addIngredientsNewNameTextBox.Text == String.Empty))
            {
                MessageBox.Show("Please select ingredient name.");
                output = false;
            }

            if (addIngredientsAvailableComboBox.SelectedItem == null && (addIngredientsNewCheckBox.IsChecked == true && addIngredientsNewNameTextBox.Text == String.Empty))
            {
                MessageBox.Show("Please enter ingredient name.");
                output = false;
            }

            if (addIngredientsAmountTextBox.Text == String.Empty)
            {
                MessageBox.Show("Please enter ingredient amount.");
                output = false;
            }

            return output;
        }
        /// <summary>
        /// Updates lists, whenever info sources have changed.
        /// </summary>
        private void UpdateLists()
        {
            addIngredientsAvailableComboBox.ItemsSource = existingIngredients;
        }
        private void addIngredientsNewCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            switch (addIngredientsNewCheckBox.IsChecked)
            {
                case true:
                    addIngredientsAvailableComboBox.IsEnabled = false;
                    addIngredientsNewNameTextBox.IsEnabled = true;
                    addIngredientsAvailableComboBox.SelectedItem = null;
                    break;
                case false:
                    addIngredientsAvailableComboBox.IsEnabled = true;
                    addIngredientsNewNameTextBox.IsEnabled = false;
                    addIngredientsNewNameTextBox.Text = String.Empty;
                    break;
            }
        }
    }
}
