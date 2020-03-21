using RecipeBookLibrary.Models;
using System.Collections.Generic;
using System.Data;

namespace RecipeBookLibrary.DataAccess
{
    public interface IDataConnection
    {
        /// <summary>
        /// Creates a recipe record in the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A recipe model with an assigned id to be displayed in MainWindow.</returns>
        RecipeModel Create_Recipe(RecipeModel model);
        /// <summary>
        /// Gets all recipes from database.
        /// </summary>
        /// <returns>List of RecipeModels.</returns>
        List<RecipeModel> Recipes_GetAll();
        /// <summary>
        /// Pulls shortest preparation time from the database.
        /// </summary>
        /// <returns>Integer of shortest preparation time.</returns>
        int Recipes_GetMinPrepTime();
        /// <summary>
        /// Pulls longest preparation time from the database.
        /// </summary>
        /// <returns>Integer of longes preparation time.</returns>
        int Recipes_GetMaxPrepTime();
        /// <summary>
        /// Pulls recipes with preparation time less or equal to parameter.
        /// </summary>
        /// <param name="selectedPrepTime">MainWindow slider value.</param>
        /// <returns>List of RecipeModels.</returns>
        List<RecipeModel> Recipes_GetByPrepTime(int selectedPrepTime);
        /// <summary>
        /// Deletes the RecipeModel passed-in from the database.
        /// </summary>
        /// <param name="model"></param>
        void Recipes_Delete(RecipeModel model);
        /// <summary>
        /// Updates the RecipeModel passed-in in the database.
        /// </summary>
        /// <param name="model">Updated RecipeModel.</param>
        void Recipes_Edit(RecipeModel model);
        /// <summary>
        /// Creates ingredient record in the database.
        /// </summary>
        /// <param name="connection">Connection to the database.</param>
        /// <param name="model">RecipeModel that contains ingredient list as a property.</param>
        /// <returns>List of IngredientModels with assigned Ids.</returns>
        List<IngredientModel> Create_Ingredient(IDbConnection connection, RecipeModel model);
        /// <summary>
        /// Creates a record for a single IngredientModel in the database.
        /// </summary>
        /// <param name="model">IngredientModel to be added to the database.</param>
        /// <returns>Id of the IngredientModel record in the database.</returns>
        int Create_IngredientSingle(IngredientModel model);
        /// <summary>
        /// Pulls all names of IngredientModels in the database.
        /// </summary>
        /// <returns>List of ingredient names as strings.</returns>
        List<string> Ingredients_GetAllNames();
        /// <summary>
        /// Pulls all IngredientModel records from the database with ParentRecipIds matching parameter.
        /// </summary>
        /// <param name="recipeId">Id of a recipe to compare to IngredientModels ParentRecipeId.</param>
        /// <returns>List of IngredientModels.</returns>
        List<IngredientModel> Ingredients_GetByRecipeId(int recipeId);
        /// <summary>
        /// Deletes IngredientModel record from the database.
        /// </summary>
        /// <param name="model">IngredientModel to delete.</param>
        void Ingredients_Delete(IngredientModel model);
    }
}