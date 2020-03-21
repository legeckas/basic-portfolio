using System.Collections.Generic;

namespace RecipeBookLibrary.Models
{
    /// <summary>
    /// Class containing information about a single recipe.
    /// </summary>
    public class RecipeModel
    {
        /// <summary>
        /// Unique identifier for a recipe.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the recipe.
        /// </summary>
        public string RecipeName { get; set; }
        /// <summary>
        /// Amount of minutes it takes to prepare the dish.
        /// </summary>
        public int PreparationTime { get; set; }
        /// <summary>
        /// A list of ingredients required to prepare the dish.
        /// </summary>
        public List<IngredientModel> Ingredients { get; set; }
        /// <summary>
        /// Instructions on how to prepare the dish.
        /// </summary>
        public string PreparationInstructions { get; set; }
        /// <summary>
        /// The path to the image of a cooked dish.
        /// </summary>
        public string ImageName { get; set; }

        public RecipeModel()
        {

        }

        public RecipeModel(string name, string preparationTime, List<IngredientModel> ingredients, string preparationInstructions, string imageName)
        {
            RecipeName = name;
            PreparationTime = int.Parse(preparationTime);
            Ingredients = ingredients;
            PreparationInstructions = preparationInstructions;
            ImageName = imageName;
        }
    }
}
