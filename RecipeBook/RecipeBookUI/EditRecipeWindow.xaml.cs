using Microsoft.Win32;
using RecipeBookLibrary;
using RecipeBookLibrary.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace RecipeBookUI
{
    /// <summary>
    /// Interaction logic for EditRecipeWindow.xaml
    /// </summary>
    public partial class EditRecipeWindow : Window, IIngredientRequester
    {
        /// <summary>
        /// Identifier for EditRecipeWindow caller.
        /// </summary>
        private IRecipeRequester callingWindow;
        /// <summary>
        /// Container for RecipeModel that is being edited.
        /// </summary>
        private RecipeModel recipeToEdit;
        /// <summary>
        /// Container for ingredients removed from the recipe.
        /// </summary>
        private List<IngredientModel> ingredientsToDelete = new List<IngredientModel>();
        /// <summary>
        /// Full path to new picture file.
        /// </summary>
        string fullPictureFileName;

        public EditRecipeWindow(IRecipeRequester caller, RecipeModel model)
        {
            InitializeComponent();

            callingWindow = caller;
            recipeToEdit = model;

            UpdateWindow();
        }

        private void editRecipeAddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientWindow newIngredientWindow = new AddIngredientWindow(this);
            newIngredientWindow.Show();
        }
        private void editRecipeRemoveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            ingredientsToDelete.Add((IngredientModel)editRecipeAddedIngredientsListbox.SelectedItem);
            recipeToEdit.Ingredients.Remove((IngredientModel)editRecipeAddedIngredientsListbox.SelectedItem);
            UpdateWindow();
        }
        private void editRecipeAddImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();

            fullPictureFileName = fd.FileName;
        }
        private void SaveUpdatedDishPicture(RecipeModel model)
        {
            // Creates an images directory, if it doesn't exist
            string targetPath = Directory.GetCurrentDirectory() + "\\images";
            Directory.CreateDirectory(targetPath);

            string destFileName = (model.RecipeName + fullPictureFileName.Substring(fullPictureFileName.Length - 4, 4)).Replace(" ", "_");
            model.ImageName = destFileName;
            string destFilePath = System.IO.Path.Combine(targetPath, destFileName);

            File.Copy(fullPictureFileName, destFilePath, true);
        }
        private void editRecipeUpdateRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateWindow())
            {
                AddNewIngredientsToDb();

                recipeToEdit.RecipeName = editRecipeNameTextBox.Text;
                recipeToEdit.PreparationTime = int.Parse(editRecipePrepTimeTextBox.Text);
                recipeToEdit.PreparationInstructions = editRecipeInstructionsTextBox.Text;

                GlobalConfig.Connection.Recipes_Edit(recipeToEdit);

                if (fullPictureFileName != null && fullPictureFileName != "")
                {
                    SaveUpdatedDishPicture(recipeToEdit);
                }
                callingWindow.Recipe_Complete(recipeToEdit);
                IngredientsRemoveCleanup();

                this.Close();
            }
        }
        /// <summary>
        /// Checks whether there are ingredients in the list missing in the dataabase and adds them.
        /// </summary>
        private void AddNewIngredientsToDb()
        {
            foreach (IngredientModel ingredient in recipeToEdit.Ingredients)
            {
                if (ingredient.Id < 1)
                {
                    ingredient.Id = GlobalConfig.Connection.Create_IngredientSingle(ingredient);
                }
            }
        }
        /// <summary>
        /// Checks, whether any of the removed ingredients are in the database, and removes them, if so.
        /// </summary>
        private void IngredientsRemoveCleanup()
        {
            foreach (IngredientModel ingredient in ingredientsToDelete)
            {
                if (ingredient.ParentRecipeId > 0)
                {
                    GlobalConfig.Connection.Ingredients_Delete(ingredient);
                }
            }
        }
        private bool ValidateWindow()
        {
            bool output = true;

            int prepTime = 0;
            bool prepTimeValid = int.TryParse(editRecipePrepTimeTextBox.Text, out prepTime);

            if (editRecipeNameTextBox.Text == "")
            {
                MessageBox.Show("Please enter a valid recipe name.");
                output = false;
            }

            if (editRecipePrepTimeTextBox.Text == "" || !prepTimeValid || prepTime <= 0)
            {
                MessageBox.Show("Please enter a valid number for recipe preparation time.");
                output = false;
            }

            if (!editRecipeAddedIngredientsListbox.HasItems)
            {
                MessageBox.Show("Please add at least one ingredient.");
                output = false;
            }

            if (editRecipeInstructionsTextBox.Text == "")
            {
                MessageBox.Show("Please enter instructions.");
                output = false;
            }

            return output;
        }
        private void UpdateWindow()
        {
            editRecipeNameTextBox.Text = recipeToEdit.RecipeName;
            editRecipePrepTimeTextBox.Text = recipeToEdit.PreparationTime.ToString();
            editRecipeAddedIngredientsListbox.ItemsSource = null;
            editRecipeAddedIngredientsListbox.ItemsSource = recipeToEdit.Ingredients;
            editRecipeAddedIngredientsListbox.DisplayMemberPath = "NameAmountCombined";
            editRecipeInstructionsTextBox.Text = recipeToEdit.PreparationInstructions;
            editRecipeAddImagePathTextBlock.Text = Directory.GetCurrentDirectory() + "\\images\\" + recipeToEdit.ImageName;
        }
        public void Ingredient_Complete(IngredientModel model)
        {
            model.ParentRecipeId = recipeToEdit.Id;
            recipeToEdit.Ingredients.Add(model);
            UpdateWindow();
        }
    }
}
