using System;
using System.Data;
using System.Linq;
using SQLite;
//using NATIVE = System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace FifthElement.LogforceLoadingHybrid.Database
{
    /// <summary>
    /// DbConnectionStateManager is a singleton class that holds the state of database connections and 
    /// cached table wrappers. 
    /// </summary>
    public sealed class DbConnectionManager : IDisposable
    {
        private string _dbFilePath;
        private SQLiteConnection _connection;
        private const string _connectionString = "Data Source=\"{0}\";Version=3;PRAGMA synchronous = NORMAL;PRAGMA journal_mode = WAL";
        private const string DatabaseCreationScriptFilePath = "FifthElement.LogforceLoadingHybrid.Database.Script.database.sql";

        public void SetDefaultDatabase(string dbFilePath,string clientVersion)
        {
            _dbFilePath = dbFilePath;

            if (!File.Exists(dbFilePath))
            {
                _connection = CreateDatabase(dbFilePath);
            }
                
            if (_connection == null)
                _connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite);
        }

        private SQLiteConnection CreateDatabase(string dbFilePath)
        {
            var connection = new SQLiteConnection(dbFilePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseCreationScriptFilePath))
            using (var reader = new StreamReader(stream, System.Text.Encoding.Default))
            {
                string[] sqls = reader.ReadToEnd().Split(';');

                connection.RunInTransaction(() =>
                {
                    foreach (var command in from sql in sqls where !string.IsNullOrEmpty(sql.Replace("\r\n", "")) select connection.CreateCommand(sql))
                    {
                        command.ExecuteNonQuery();
                    }
                });
            }
            return connection; 
        }

        public SQLiteConnection Connection
        {
            get
            {
                {
                    if (string.IsNullOrEmpty(_dbFilePath)) throw new Exception("Default database not set");
                    return _connection;
                }
            }
        }

        private void Shutdown()
        {
            //if (_transaction != null)
            //    RollbackTransaction();

            //if (_connection.State != ConnectionState.Closed)
            //    _connection.Close();
            if (_connection == null) return;

            _connection.Dispose();
            _connection = null;
        }

        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static readonly DbConnectionManager Instance = new DbConnectionManager();
       
        /// <summary>
        /// Parameterless private instance constructor
        /// </summary>
        DbConnectionManager() { }

        /// <summary>
        ///  Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        /// </summary>
        static DbConnectionManager() { }



        #endregion Singleton

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Shutdown();
        }

        #endregion
    }
}
