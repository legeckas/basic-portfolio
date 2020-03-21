using RecipeBookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBookUI
{
    public interface IIngredientRequester
    {
        /// <summary>
        /// Ensures that the requested ingredient model is returned and prevents direct communication.
        /// </summary>
        /// <param name="model">Created ingredient model.</param>
        void Ingredient_Complete(IngredientModel model);
    }
}
