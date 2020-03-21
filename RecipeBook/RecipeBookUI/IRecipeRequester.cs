using RecipeBookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBookUI
{
    public interface IRecipeRequester
    {
        /// <summary>
        /// Ensures that the requested recipe model is returned and prevents direct communication.
        /// </summary>
        /// <param name="model">Created recipe model.</param>
        void Recipe_Complete(RecipeModel model);
    }
}
