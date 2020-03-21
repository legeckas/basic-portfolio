using Microsoft.Win32;
using RecipeBookLibrary;
using RecipeBookLibrary.Models;
using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace RecipeBookUI
{
    /// <summary>
    /// Interaction logic for AddRecipeWIndow.xaml
    /// </summary>
    public partial class AddRecipeWIndow : Window, IIngredientRequester
    {
        /// <summary>
        /// Identifier for AddRecipeWindow caller.
        /// </summary>
        private IRecipeRequester callingWindow;
        /// <summary>
        /// Ingredients are added to this list from a separate window, then assigned to RecipeModel instance.
        /// </summary>
        private List<IngredientModel> ingredientsList = new List<IngredientModel>();
        /// <summary>
        /// Full path to the picture to be added to RecipeModel.
        /// </summary>
        string fullPictureFileName;

        public AddRecipeWIndow(IRecipeRequester caller)
        {
            InitializeComponent();

            callingWindow = caller;
        }

        /// <summary>
        /// Opens a new AddIngredientWindow to add ingredients to the recipe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRecipeAddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientWindow newIngredientWindow = new AddIngredientWindow(this);
            newIngredientWindow.Show();
        }
        /// <summary>
        /// Removes ingredient from the inredient list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRecipeRemoveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            ingredientsList.Remove((IngredientModel)addRecipeAddedIngredientsListbox.SelectedItem);
            UpdateLists();
        }
        /// <summary>
        /// Creates RecipeModel instance, adds it to the database and passes back to MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRecipeAddNewRecipeButton_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateWindow())
            {
                RecipeModel model = new RecipeModel(
                    addRecipeNameTextBox.Text, 
                    addRecipePrepTimeTextBox.Text,
                    ingredientsList,
                    addRecipeInstructionsTextBox.Text,
                    addRecipeAddImagePathTextBlock.Text);
                
                if (fullPictureFileName != null && fullPictureFileName != "")
                {
                    SaveDishPicture(model);
                }
                
                callingWindow.Recipe_Complete(GlobalConfig.Connection.Create_Recipe(model));

                this.Close();
            }
        }
        /// <summary>
        /// Handles ingredients returned from a separate window.
        /// </summary>
        /// <param name="model">And instance of IngredientModel.</param>
        public void Ingredient_Complete(IngredientModel model)
        {
            ingredientsList.Add(model);

            UpdateLists();
        }
        /// <summary>
        /// Checks whether the information in the window has been filled out properly and can proceed.
        /// </summary>
        /// <returns>Returns true, if all correct.</returns>
        private bool ValidateWindow()
        {
            bool output = true;

            int prepTime = 0;
            bool prepTimeValid = int.TryParse(addRecipePrepTimeTextBox.Text, out prepTime);

            if (addRecipeNameTextBox.Text == "")
            {
                MessageBox.Show("Please enter a valid recipe name.");
                output = false;
            }

            if (addRecipePrepTimeTextBox.Text == "" || !prepTimeValid || prepTime <= 0)
            {
                MessageBox.Show("Please enter a valid number for recipe preparation time.");
                output = false;
            }

            if (!addRecipeAddedIngredientsListbox.HasItems)
            {
                MessageBox.Show("Please add at least one ingredient.");
                output = false;
            }

            if (addRecipeInstructionsTextBox.Text == "")
            {
                MessageBox.Show("Please enter instructions.");
                output = false;
            }

            return output;
        }
        /// <summary>
        /// Populates lists and updates them, if information is updated.
        /// </summary>
        private void UpdateLists()
        {
            addRecipeAddedIngredientsListbox.ItemsSource = null;
            addRecipeAddedIngredientsListbox.ItemsSource = ingredientsList;
            addRecipeAddedIngredientsListbox.DisplayMemberPath = "NameAmountCombined";
        }
        /// <summary>
        /// Handles adding image to the recipe.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addRecipeAddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image files (.jpg, .png)|*.png;*.jpg";
            fd.ShowDialog();

            fullPictureFileName = fd.FileName;

            addRecipeAddImagePathTextBlock.Text = fullPictureFileName;
        }
        /// <summary>
        /// Copies selected picture to the images folder in app root directory and assigns its name to recipe.
        /// </summary>
        /// <param name="model"></param>
        private void SaveDishPicture(RecipeModel model)
        {
            // Creates an images directory, if it doesn't exist
            string targetPath = Directory.GetCurrentDirectory() + "\\images";
            Directory.CreateDirectory(targetPath);

            string destFileName = model.RecipeName + fullPictureFileName.Substring(fullPictureFileName.Length-4, 4).Replace(" ", "_");
            model.ImageName = destFileName;
            string destFilePath = System.IO.Path.Combine(targetPath, destFileName);

            try
            {
                File.Copy(fullPictureFileName, destFilePath, false);
            }
            catch (IOException)
            {
                MessageBox.Show("Such filename already exists. Please change the name of the recipe.");
            }
        }
    }
}
