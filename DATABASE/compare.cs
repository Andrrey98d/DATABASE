using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.Data.Sqlite;


//public const string || public readonly int _ a;
namespace DATABASE
{
    class compare: Program
    {

        public string ConString = _CONNECT;
        public string SQLCommand = SELECT_ALL;

        public void Initialize_DB(string Constring, string Exp)
        {
            this.ConString = Constring;
            SqliteConnection connection = new SqliteConnection(Constring);
            this.SQLCommand = Exp;
            SqliteCommand command = new SqliteCommand(Exp, connection);
            SqliteDataReader reader_ = command.ExecuteReader();
            using (connection)
            {
                connection.Open();
                if (reader_.HasRows)
                {
                    using (command)
                    { 
                        while (reader_.Read()) 
                        {
                            _ = reader_[0]; // guids
                                            
                            Guid guid;
                            for (int i = 0; i > 0; i++) // нужно выдрать по методу getguid все гуиды с первого (index: 0) столбца
                            {
                                guid = reader_.GetGuid(i);  // <---

                                List <string> GuidsList = new List<string>();
                                if (!GuidsList.Contains(Convert.ToString(guid)))
                                {
                                    GuidsList.Add(Convert.ToString(i));
                                }
                            }
                            //byte[] bytes = guid.ToByteArray();
                        }
                    }
                }
            }
        }


    }
}
