using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using FifthElement.LogforceLoadingHybrid.Core;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;

namespace FifthElement.LogforceLoadingHybrid.Database.Repository
{
    /// <summary>
    /// Base class for repositories
    /// This should have a collection of "helper" etc functions that is used with repositories
    /// </summary>
    public class RepositoryBase
    {
        private static readonly Random RandomNumber = new Random(); //TODO: eventually this can be removed when we have the sequence as a DB table /TNU
        protected SQLiteConnection _databaseConnection;
        protected SQLiteConnection DatabaseConnection
        {
            get { return _databaseConnection; }
        }

        protected RepositoryBase()
        {
            _databaseConnection = DbConnectionManager.Instance.Connection;
        }

        public static int GetNextId()
        {
            return -RandomNumber.Next(10000, int.MaxValue);
        }

        public static void StoreList<T>(string jsonArray)
        {
            var objectArray = JsonConvert.DeserializeObject<JArray>(jsonArray);
            var connection = DbConnectionManager.Instance.Connection;
            connection.RunInTransaction(() =>
                                            {
                                                foreach (var objectItem in objectArray)
                                                {
                                                    string objectItemJson = objectItem.ToString();
                                                    var obj = (JsonConvert.DeserializeObject<T>(objectItemJson));
                                                    var prop = obj.GetType().GetProperties().FirstOrDefault(p => p.Name == "Language");
                                                    if (prop != null)
                                                        if (prop.GetValue(obj, null).ToString() != Constants.Language)
                                                            continue;
                                                        
                                                    var dbObject = obj as IBaseObject;
                                                    dbObject.Json = objectItemJson;
                                                    if (connection.Update(dbObject) == 0)
                                                        connection.Insert(dbObject);
                                                }
                                            });
        }

        internal static WhereObject BuildWhereClause(object filter)
        {
            if (filter == null) return null;
            var whereClause = new StringBuilder();
            var retval = new WhereObject();
            PropertyInfo[] props = filter.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {

                var attribute = (ColumnAttribute)propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();

                

                //Will ignore arrays and collections
                if (propertyInfo.PropertyType.IsArray 
                    || ((propertyInfo.PropertyType.GetInterface(typeof (IEnumerable<>).FullName) != null) && (propertyInfo.PropertyType.FullName != typeof(string).FullName) )
                    || attribute == null 
                    || propertyInfo.GetValue(filter, null) == null
                    || string.IsNullOrEmpty(propertyInfo.GetValue(filter, null).ToString())) continue;

                whereClause.AppendFormat(whereClause.Length > 0 ? " AND {0} = ?" : " {0} = ?", attribute.Name);
                retval.Parameters.Add(propertyInfo.GetValue(filter, null));
            }
            retval.WhereClause = whereClause.ToString();
            return retval;
        }

        internal class WhereObject
        {
            public WhereObject()
            {
                Parameters = new List<object>();
            }
            public void AddWhereExpression(string expression, object parameter)
            {
                AddWhereExpression(expression);
                Parameters.Add(parameter);
            }
            public void AddWhereExpression(string expression)
            {
                WhereClause += String.IsNullOrEmpty(WhereClause) ? expression : " AND " + expression;
            }

            public string WhereClause { get; set; }
            public IList<object> Parameters { get; set; }
        }
    }
}
