
namespace RecipeBookLibrary.Models
{
    /// <summary>
    /// Class holding all of the information regarding single ingredient.
    /// </summary>
    public class IngredientModel
    {
        /// <summary>
        /// Unique identifier for an ingredient.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The name of the ingredient.
        /// </summary>
        public string IngredientName { get; set; }
        /// <summary>
        /// Specified amount of the ingredient to be used in the recipe.
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// Unique identifier of the recipe that the ingredient belongs to.
        /// </summary>
        public int ParentRecipeId { get; set; }
        /// <summary>
        /// Combined name + amount property for AddRecipeWindow ComboBox.
        /// </summary>
        public string NameAmountCombined => $"{ IngredientName }    { Amount }";
        
        public IngredientModel()
        {
        }

        public IngredientModel(string name, string amount)
        {
            IngredientName = name;
            Amount = amount;
        }
    }
}
