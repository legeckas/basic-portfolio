using RecipeBookLibrary.DataAccess;
using System.Configuration;

namespace RecipeBookLibrary
{
    public static class GlobalConfig
    {
        /// <summary>
        /// Defines connection to the database.
        /// </summary>
        public static IDataConnection Connection { get; set; }
        /// <summary>
        /// Initializes a connector instance and assigns it to the variable 'Connection'.
        /// </summary>
        public static void InitializeConnection()
        {
            SqlConnector sql = new SqlConnector();
            Connection = sql;
        }
        /// <summary>
        /// Gets the connection string for a selected database from App.config.
        /// </summary>
        /// <param name="name">Name of the database.</param>
        /// <returns>Returns a connection string.</returns>
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
