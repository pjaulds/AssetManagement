using System.Data.Common;
using System.Configuration;
namespace Qtech.AssetManagement.Dal
{
    /// <summary>
    /// The AppConfiguaration class contains read-only properties that are essentially short cuts to settings in the app.config file.
    /// </summary>
    public static class AppConfiguration
    {
        #region Public Properties

        /// <summary>Returns the connectionstring for the application.</summary>
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            }
        }

        /// <summary>
        /// Returns the connection string for logs
        /// </summary>
        public static string ConnectionLogString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionLogStringName].ConnectionString;
            }
        }

        public static string ConnectionLogisticsString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionLogisticsStringName].ConnectionString;
            }
        }

        public static string ConnectionSignatureString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionSignatureStringName].ConnectionString;
            }
        }

        public static string ConnectionCommonString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[ConnectionCommonStringName].ConnectionString;
            }
        }

        /// <summary>Returns the name of the current connectionstring for the application.</summary>
        public static string ConnectionStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.Connection"];
            }
        }

        public static string ConnectionLogisticsStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.ConnectionLogistics"];
            }
        }

        /// <summary>
        /// returns the name of the current connection string for logs
        /// </summary>
        public static string ConnectionLogStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.ConnectionLog"];
            }
        }

        public static string ConnectionSignatureStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.SignaturesConnection"];
            }
        }

        /// <summary>
        /// returns the name of the common database connection string
        /// </summary>
        public static string ConnectionCommonStringName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.CommonConnection"];
            }
        }

        /// <summary>
        /// returns the kind of database provider the application is using
        /// </summary>
        public static string DBProviderName
        {
            get
            {
                return ConfigurationManager.AppSettings["Qtech.AssetManagement.DBProvider"];
            }
        }
        #endregion

        // creates and prepares a new DbCommand object on a new connection
        /// <summary>
        /// create a command for the application
        /// </summary>
        /// <returns></returns>
        public static DbCommand CreateCommand()
        {
            // Obtain the database provider name
            string dataProviderName = AppConfiguration.DBProviderName;

            // Obtain the database connection string
            string connectionString = AppConfiguration.ConnectionString;

            // Create a new data provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

            // Obtain a database-specific connection object
            DbConnection conn = factory.CreateConnection();

            // Set the connection string
            conn.ConnectionString = connectionString;

            // Create a database-specific command object
            DbCommand comm = conn.CreateCommand();


            // Return the initialized command object
            return comm;
        }

        /// <summary>
        /// create a command for logs
        /// </summary>
        /// <returns></returns>
        public static DbCommand CreateCommandLog()
        {
            // Obtain the database provider name
            string dataProviderName = AppConfiguration.DBProviderName;

            // Obtain the database connection string
            string connectionString = AppConfiguration.ConnectionLogStringName;

            // Create a new data provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

            // Obtain a database-specific connection object
            DbConnection conn = factory.CreateConnection();

            // Set the connection string
            conn.ConnectionString = ConnectionLogString;

            // Create a database-specific command object
            DbCommand comm = conn.CreateCommand();


            // Return the initialized command object
            return comm;
        }

        public static DbCommand CreateCommandSignature()
        {
            // Obtain the database provider name
            string dataProviderName = AppConfiguration.DBProviderName;

            // Obtain the database connection string
            string connectionString = AppConfiguration.ConnectionSignatureString;

            // Create a new data provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

            // Obtain a database-specific connection object
            DbConnection conn = factory.CreateConnection();

            // Set the connection string
            conn.ConnectionString = connectionString;

            // Create a database-specific command object
            DbCommand comm = conn.CreateCommand();


            // Return the initialized command object
            return comm;
        }

        //public static DbCommand CreateCommand()
        //{
        //    // Obtain the database provider name
        //    string dataProviderName = AppConfiguration.DBProviderName;

        //    // Obtain the database connection string
        //    string connectionString = AppConfiguration.ConnectionLogisticsString;

        //    // Create a new data provider factory
        //    DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

        //    // Obtain a database-specific connection object
        //    DbConnection conn = factory.CreateConnection();

        //    // Set the connection string
        //    conn.ConnectionString = connectionString;

        //    // Create a database-specific command object
        //    DbCommand comm = conn.CreateCommand();


        //    // Return the initialized command object
        //    return comm;
        //}
    }
}
