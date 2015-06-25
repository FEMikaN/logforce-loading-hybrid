using System;
using System.Linq;
using FifthElement.LogforceLoadingHybrid.Core.Model;
using System.Collections.Generic;

namespace FifthElement.LogforceLoadingHybrid.Database.Repository
{
    public class KeyValueRepository : RepositoryBase
    {
        public KeyValueStorage GetValue(string key)
        {
            return DatabaseConnection.Table<KeyValueStorage>().FirstOrDefault(a => a.Key == key);
        }

        public IEnumerable<KeyValueStorage> GetList(KeyValueStorage filter)
        {
            WhereObject wo = BuildWhereClause(filter);
            if (wo != null && !string.IsNullOrEmpty(wo.WhereClause))
            {
                string sql = string.Format("SELECT * FROM KEYVALUESTORAGE WHERE {0}", wo.WhereClause);
                return DatabaseConnection.Query<KeyValueStorage>(sql, wo.Parameters.ToArray());
            }
            else
            {
                return new List<KeyValueStorage>(DatabaseConnection.Table<KeyValueStorage>());
            }
        }

        public int UpdateValue(KeyValueStorage keyValue)
        {
            keyValue.UpdatedOn = DateTime.UtcNow;
            return DatabaseConnection.Update(keyValue);
        }

        public int InsertValue(KeyValueStorage keyValue)
        {
            return DatabaseConnection.Insert(keyValue);
        }
        public int DeleteValue(KeyValueStorage keyValue)
        {
            return DatabaseConnection.Delete(keyValue);
        }

    }
}
