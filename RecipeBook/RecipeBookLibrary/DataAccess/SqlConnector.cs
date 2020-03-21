using RecipeBookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;

namespace RecipeBookLibrary.DataAccess
{
    /// <summary>
    /// Connects to the database to pull or create records.
    /// </summary>
    class SqlConnector : IDataConnection
    {
        private const string db = "RecipesDB";
        public RecipeModel Create_Recipe(RecipeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@RecipeName", model.RecipeName);
                p.Add("@PreparationTime", model.PreparationTime);
                p.Add("PreparationInstructions", model.PreparationInstructions);
                p.Add("@ImageName", model.ImageName);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spRecipes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                List<IngredientModel> newIngredients = Create_Ingredient(connection, model);

                model.Ingredients = newIngredients;
            }

            return model;
        }
        public List<IngredientModel> Create_Ingredient(IDbConnection connection, RecipeModel model)
        {
            List<IngredientModel> output = new List<IngredientModel>();

            // assigns parent recipe id to ingredient and passes it on into the database
            foreach (IngredientModel ingredient in model.Ingredients)
            {
                ingredient.ParentRecipeId = model.Id;

                var p = new DynamicParameters();

                p.Add("@IngredientName", ingredient.IngredientName);
                p.Add("@Amount", ingredient.Amount);
                p.Add("@ParentRecipeId", ingredient.ParentRecipeId);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spIngredients_Insert", p, commandType: CommandType.StoredProcedure);

                ingredient.Id = p.Get<int>("@Id");

                output.Add(ingredient);
            }

            return output;
        }
        public int Create_IngredientSingle(IngredientModel model)
        {
            int output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@IngredientName", model.IngredientName);
                p.Add("@Amount", model.Amount);
                p.Add("@ParentRecipeId", model.ParentRecipeId);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spIngredients_Insert", p, commandType: CommandType.StoredProcedure);

                output = p.Get<int>("@Id");
            }

            return output;
        }
        public List<RecipeModel> Recipes_GetAll()
        {
            List<RecipeModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<RecipeModel>("dbo.spRecipes_GetAll", commandType: CommandType.StoredProcedure).ToList();
            }

            foreach (RecipeModel recipe in output)
            {
                recipe.Ingredients = Ingredients_GetByRecipeId(recipe.Id);
            }

            return output;
        }
        public List<string> Ingredients_GetAllNames()
        {
            List<string> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<string>("dbo.spIngredients_GetAllNames", commandType: CommandType.StoredProcedure).ToList();
            }

            return output;
        }
        public List<IngredientModel> Ingredients_GetByRecipeId(int recipeId)
        {
            List<IngredientModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@ParentRecipeId", recipeId);

                output = connection.Query<IngredientModel>("dbo.spIngredients_GetAllByRecipeId", p, commandType: CommandType.StoredProcedure).ToList();
            }

            return output;
        }
        public int Recipes_GetMinPrepTime()
        {
            int output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                try
                {
                    List<int> semiOutput;
                    semiOutput = connection.Query<int>("dbo.spRecipes_GetMinPrepTime", commandType: CommandType.StoredProcedure).ToList();
                    output = semiOutput[0];
                }
                catch (NullReferenceException)
                {
                    return 0;
                }

            }

            return output;
        }
        public int Recipes_GetMaxPrepTime()
        {
            int output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                try
                {
                    List<int> semiOutput;
                    semiOutput = connection.Query<int>("dbo.spRecipes_GetMaxPrepTime", commandType: CommandType.StoredProcedure).ToList();
                    output = semiOutput[0];
                }
                catch (NullReferenceException)
                {

                    return 0;
                }

            }

            return output;
        }
        public List<RecipeModel> Recipes_GetByPrepTime(int selectedPrepTime)
        {
            List<RecipeModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PreparationTime", selectedPrepTime);

                output = connection.Query<RecipeModel>("dbo.spRecipes_GetByPrepTime", p, commandType: CommandType.StoredProcedure).ToList();
            }

            foreach (RecipeModel recipe in output)
            {
                recipe.Ingredients = Ingredients_GetByRecipeId(recipe.Id);
            }

            return output;
        }
        public void Recipes_Delete(RecipeModel model)
        {
            foreach (IngredientModel ingredient in model.Ingredients)
            {
                Ingredients_Delete(ingredient);
            }

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", model.Id);

                connection.Execute("dbo.spRecipes_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }
        public void Recipes_Edit(RecipeModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@RecipeName", model.RecipeName);
                p.Add("@PreparationTime", model.PreparationTime);
                p.Add("PreparationInstructions", model.PreparationInstructions);
                p.Add("@ImageName", model.ImageName);
                p.Add("@Id", model.Id);

                connection.Execute("dbo.spRecipes_Update", p, commandType: CommandType.StoredProcedure);

            }
        }
        public void Ingredients_Delete(IngredientModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", model.Id);

                connection.Execute("dbo.spIngredients_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
