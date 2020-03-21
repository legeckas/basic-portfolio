using RecipeBookLibrary;
using RecipeBookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RecipeBookUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IRecipeRequester
    {
        /// <summary>
        /// Container for recipes, currently in the database.
        /// </summary>
        List<RecipeModel> existingRecipes;
        /// <summary>
        /// List of ingredient names, that are currently in the database.
        /// </summary>
        List<string> availableIngredients;
        /// <summary>
        /// List of ingredients added to the ListBox to search by ingredients.
        /// </summary>
        List<string> selectedIngredients = new List<string>();
        /// <summary>
        /// List of recipes corresponding to selected search parameters.
        /// </summary>
        List<RecipeModel> filteredRecipes;
        /// <summary>
        /// Recipe currenly displayed.
        /// </summary>
        RecipeModel recipeCurrentlyDisplayed;

        public MainWindow()
        {
            InitializeComponent();
            GlobalConfig.InitializeConnection();
            InitializeLists();
            UpdateWindow();
        }

        /// <summary>
        /// Opens new AddRecipeWindow to add a recipe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addNewRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeWIndow addNewRecipeWindow = new AddRecipeWIndow(this);
            addNewRecipeWindow.Show();
        }
        /// <summary>
        /// Adds ingredient to ListBox to use for search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedIngredients.Add(ingredientsAvailableListbox.SelectedItem.ToString());
                availableIngredients.Remove(ingredientsAvailableListbox.SelectedItem.ToString());
            }
            catch (NullReferenceException)
            {
                return;
            }

            UpdateWindow();
        }
        /// <summary>
        /// Removes ingredient from search criteria ListBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedIngredients.Remove(ingredientsSelectedListbox.SelectedItem.ToString());
                availableIngredients.Add(ingredientsSelectedListbox.SelectedItem.ToString());
            }
            catch (NullReferenceException)
            {
                return;
            }

            UpdateWindow();
        }
        /// <summary>
        /// Searches recipes corresponding to given criteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchByNameCheckBox.IsChecked == true && searchByNameComboBox.SelectedItem != null)
            {
                FillObjects((RecipeModel)searchByNameComboBox.SelectedItem);
            }
            else if (searchByTimeCheckBox.IsChecked == true)
            {
                filteredRecipes = GlobalConfig.Connection.Recipes_GetByPrepTime((int)preparationTimeSlider.Value);
                UpdateWindow();
            }
            else if (searchByIngredientsCheckBox.IsChecked == true && ingredientsSelectedListbox.HasItems == true)
            {
                SortingByIngredients();
                UpdateWindow();
            }
        }
        /// <summary>
        /// Looks trough recipes corresponding to given ingredients.
        /// </summary>
        private void SortingByIngredients()
        {
            List<RecipeModel> recipesToSort = GlobalConfig.Connection.Recipes_GetAll();

            foreach (string ingredient in selectedIngredients)
            {
                foreach (RecipeModel recipe in recipesToSort.ToList())
                {
                    bool delete = true;

                    foreach (IngredientModel internalIngredient in recipe.Ingredients)
                    {
                        if (internalIngredient.IngredientName == ingredient)
                        {
                            delete = false;
                        }
                    }

                    if (delete)
                    {
                        recipesToSort.Remove(recipe);
                    }
                }
            }

            filteredRecipes = recipesToSort;
        }
        /// <summary>
        /// Displays the recipe from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void availableRecipesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FillObjects((RecipeModel)availableRecipesListBox.SelectedItem);
        }
        /// <summary>
        /// Deletes displayed recipe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSelectedRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (recipeCurrentlyDisplayed != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete the displayed recipe?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        GlobalConfig.Connection.Recipes_Delete(recipeCurrentlyDisplayed);
                        recipeCurrentlyDisplayed = null;
                        FillObjects(recipeCurrentlyDisplayed);
                        InitializeLists();
                        UpdateWindow();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// Opens new EditRecipeWindow for displayed recipe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSelectedRecipe_Click(object sender, RoutedEventArgs e)
        {
            EditRecipeWindow editRecipeWindow = new EditRecipeWindow(this, recipeCurrentlyDisplayed);
            editRecipeWindow.Show();
        }
        /// <summary>
        /// Displays recipe returned from AddRecipeWindow.
        /// </summary>
        /// <param name="model"></param>
        public void Recipe_Complete(RecipeModel model)
        {
            FillObjects(model);

            InitializeLists();
            UpdateWindow();
        }
        /// <summary>
        /// Populates lists with database records.
        /// </summary>
        private void InitializeLists()
        {
            existingRecipes = GlobalConfig.Connection.Recipes_GetAll();
            availableIngredients = GlobalConfig.Connection.Ingredients_GetAllNames();

            preparationTimeSlider.Minimum = GlobalConfig.Connection.Recipes_GetMinPrepTime();
            preparationTimeSlider.Maximum = GlobalConfig.Connection.Recipes_GetMaxPrepTime();
        }
        /// <summary>
        /// Updates page objects with information from the lists.
        /// </summary>
        private void UpdateWindow()
        {
            searchByNameComboBox.ItemsSource = null;
            searchByNameComboBox.ItemsSource = existingRecipes;
            searchByNameComboBox.DisplayMemberPath = "RecipeName";

            ingredientsAvailableListbox.ItemsSource = null;
            ingredientsAvailableListbox.ItemsSource = availableIngredients;

            ingredientsSelectedListbox.ItemsSource = null;
            ingredientsSelectedListbox.ItemsSource = selectedIngredients;

            availableRecipesListBox.ItemsSource = null;
            availableRecipesListBox.ItemsSource = filteredRecipes;
            availableRecipesListBox.DisplayMemberPath = "RecipeName";
        }
        /// <summary>
        /// Displays full recipe.
        /// </summary>
        /// <param name="model">RecipeModel to display.</param>
        private void FillObjects(RecipeModel model)
        {
            selectedRecipeNameTextBlock.Text = model.RecipeName;
            selectedRecipeTimeToPrepare.Text = $"Time to prepare: { model.PreparationTime.ToString() } minutes.";

            if (model.ImageName != null && model.ImageName != "")
            {
                string targetPath = "pack://siteoforigin:,,,/images/" + model.ImageName;
                selectedRecipeDishImage.Source = new BitmapImage(new Uri(targetPath, UriKind.Absolute));
            }

            string selectedRecipeIngredients = String.Empty;

            foreach (IngredientModel ingredient in model.Ingredients)
            {
                selectedRecipeIngredients += $"{ ingredient.NameAmountCombined } \n";
            }

            selectedRecipeIngredientsTextBlock.Text = selectedRecipeIngredients;

            selectedRecipeInstructionsTextBlock.Text = model.PreparationInstructions;

            recipeCurrentlyDisplayed = model;
        }
        private void searchByNameCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            switch (searchByNameCheckBox.IsChecked)
            {
                case true:
                    searchByNameComboBox.IsEnabled = true;

                    searchByTimeCheckBox.IsEnabled = false;
                    searchByIngredientsCheckBox.IsEnabled = false;

                    searchRecipeButton.Content = "Show";
                    break;

                case false:
                    searchByNameComboBox.IsEnabled = false;

                    searchByTimeCheckBox.IsEnabled = true;
                    searchByIngredientsCheckBox.IsEnabled = true;

                    searchRecipeButton.Content = "Search";
                    break;
            }

        }
        private void searchByTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            switch (searchByTimeCheckBox.IsChecked)
            {
                case true:
                    preparationTimeSlider.IsEnabled = true;
                    preparationTimeTextBox.IsEnabled = true;

                    searchByNameCheckBox.IsEnabled = false;
                    searchByIngredientsCheckBox.IsEnabled = false;
                    break;
                case false:
                    preparationTimeSlider.IsEnabled = false;
                    preparationTimeTextBox.IsEnabled = false;

                    searchByNameCheckBox.IsEnabled = true;
                    searchByIngredientsCheckBox.IsEnabled = true;
                    break;
            }
        }
        private void searchByIngredientsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            switch (searchByIngredientsCheckBox.IsChecked)
            {
                case true:
                    ingredientsAvailableListbox.IsEnabled = true;
                    ingredientsSelectedListbox.IsEnabled = true;
                    addIngredientButton.IsEnabled = true;
                    removeIngredientButton.IsEnabled = true;

                    searchByNameCheckBox.IsEnabled = false;
                    searchByTimeCheckBox.IsEnabled = false;
                    break;
                case false:
                    ingredientsAvailableListbox.IsEnabled = false;
                    ingredientsSelectedListbox.IsEnabled = false;
                    addIngredientButton.IsEnabled = false;
                    removeIngredientButton.IsEnabled = false;

                    searchByNameCheckBox.IsEnabled = true;
                    searchByTimeCheckBox.IsEnabled = true;
                    break;
            }
        }
    }
}
