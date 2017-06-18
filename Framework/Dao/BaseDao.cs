using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Framework.Dao
{
    public class BaseDao
    {
        public string ConnectionString { get; set; }

        public string GetValueFromSqlQuery(string sqlQueryCmd)
        {
            var fieldIndexList = new List<object>() { 0 };
            var results = GetRowsFromSqlQueryGeneric(sqlQueryCmd,
                fieldIndexList, "");
            return results.Count > 0 ? results.First() : "";
        }

        public IList<string> GetRowsFromSqlQuery(string sqlQueryCmd)
        {
            var fieldIndexList = new List<object>() { 0 };
            return GetRowsFromSqlQueryGeneric(sqlQueryCmd, fieldIndexList, "");
        }

        public IDictionary<string, string> GetDictionaryFromSqlQuery(string
            sqlQueryCmd)
        {
            return GetRowsFromSqlQueryGeneric(sqlQueryCmd, new List<object>()
                {0, 1}, ",").Select(line => line.Split(',')).GroupBy(l => l.First
                ()).ToDictionary(d => d.Key, d => d.First()[1]);
        }

        public IList<string> GetRowsFromSqlQueryGeneric(string sqlQueryCmd,
            IList<object> fields, string separator)
        {
            var rows = new List<string>();
            using (var db = new SqlConnection())
            {
                db.ConnectionString = ConnectionString;
                db.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = db;
                    command.CommandText = sqlQueryCmd;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var values = new List<string>();
                        foreach (object field in fields)
                        {
                            if (field is string)
                                values.Add(reader[(string)field].ToString());
                            else
                                values.Add(reader[(int)field].ToString());
                        }
                        rows.Add(String.Join(separator, values));
                    }
                    reader.Close();
                }
                db.Close();
            }
            return rows;
        }
    }
}