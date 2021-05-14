using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
namespace DATABASE
{
    public class FetchDb
    {
        //get all table names from a database 

        /*public IList<string> ListTables()
        {
            List<string> tables = new List<string>();
            DataTable dt = _connection.GetSchema("Tables");
            foreach (DataRow row in dt.Rows)
            {
                string tablename = (string)row[2];
                tables.Add(tablename);
            }
            return tables;
        }
        */

        public Dictionary<string, string> GetAllTables(SqlConnection _connection)
        {
            //this.connection = _connection;
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            DataTable dt = _connection.GetSchema("Tables");
            Dictionary<string, string> tables = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                if (row[3].ToString().Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)) //ignore views
                {
                    string tableName = row[2].ToString();
                    //string schema = row[1].ToString();
                    string guid = row[0].ToString();
                    tables.Add(tableName, guid);
                }
            }
            _connection.Close();
            return tables;
        }
    }
}