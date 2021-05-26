using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.IO;
using Aspose.Zip;
using Aspose.Zip.Saving;
using Aspose.Zip.SevenZip;
using Ionic.Zip;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace DATABASE
{
    class Program : Dialogs
    {
        public const string _CONNECT = @"Data Source = desktop - 9mrv6e2;Initial Catalog = crypto; Integrated Security = True"; //unnecessary
        public static string SELECT_ALL = "SELECT * FROM dbo.Table_1";
        public static string SEL_ALL = "SELECT * FROM Users";
        public const string CNCT = "Data Source = usersdata.db";
        public const string ALL_TABLES = ".tables"; //list all tables in database 
        public static SqliteConnection sqn = new SqliteConnection(CNCT);
        public static SqliteConnection SQN = new SqliteConnection(CNCT);
        public static OpenFileDialog ofd = new OpenFileDialog();
        public static SaveFileDialog sfd = new SaveFileDialog();
        [STAThread] // in case if using GUI
        public static void Main(string[] args)
        {

            Console.WriteLine(CNCT + "\n");
            Task task_con = new Task(Initialize_Connection);
            task_con.Start();
            ShowMenu();
            int res = Convert.ToInt32(Console.ReadLine());
            switch (res)
            {
                /*Во всех случаях меняется только sql-выражение*/
                case 1:
                    for (int i = 0; i <= 2; i++)
                    {
                        Console.Write(".");
                    }
                    DataTable_Name();
                    break;
                case 2:
                    Create_NewTable();
                    break;
                case 3:
                    UPDATE();
                    break;
                case 4:
                    INSERT_VALUES();
                    break;
                case 5:
                    DELETE();
                    break;
            }
        }
        public static void Initialize_Connection()
        {
            using (sqn)
            {
                sqn.Open();
                SqliteCommand command = new SqliteCommand(SEL_ALL, sqn);
                SqliteCommand slc = new SqliteCommand(ALL_TABLES, sqn);
                SqliteDataReader reader = command.ExecuteReader();
                string path = @"D:\\file.dta";
                if (reader.HasRows)
                {
                    using (slc)
                    {
                        using (command)
                        {
                            while (reader.Read()) //counter++ => removed await method to avoid non-recognising point of entry (Main)
                            {
                                bool HR = reader.HasRows;
                                if (HR == true)
                                {
                                    string text = "";
                                    string[] words = text.Split(' ');
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.Write("\t" + reader[i]);
                                    }
                                    Console.WriteLine("");
                                  
                                        //string w1 = words[0]; [0],1,2..3..n
                                        
                                                                 
                                   
                                    List<string> ID = new List<string>();
                                    var id = reader.GetValue(0);

                                    object[] objs = new object[1]; //1,2,3,  и т.д
                                                                   //string[] column1_values = dtTemp1.AsEnumerable().Select(s => s.Field<string>("City")).ToArray<string>(); //где dtTemp1 - экземпляр datatable
                                   }
                                }
                            }
                        }
                    }
                    reader.Close();
                }
                Console.Read();
            }
        
        public static void Create_NewTable()
        {
            //users.db лежит  в src/*proj name*/bin/debug
            var sqn = new SqliteConnection(CNCT);
            using (sqn)
            {
                sqn.Open();
                SqliteCommand CMD = new SqliteCommand
                {
                    Connection = sqn,
                    CommandText = "CREATE TABLE Users_3(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Nameee TEXT NOT NULL, Age INTEGER NOT NULL, Value INTEGER NOT NULL)"
                };
                CMD.ExecuteNonQuery();
                if (CMD.CommandText.Substring(13, 18) == "Users") //or 13-17
                {
                    Console.WriteLine("Table already exists");
                }
                Console.WriteLine("Таблица Users создана");
            }
            Console.Read();
        }
        public static void INSERT_VALUES()
        {
            // добавить ридер по столбцам, прирaвняв его к переменной (var => reader[0])
            using (SQN)
            {
                SQN.Open();
                SqliteCommand CMD = new SqliteCommand
                {
                    Connection = SQN,
                    CommandText = "INSERT INTO Users (Name, Age) VALUES ('Cryptocommand', 128)"
                };
                int number = CMD.ExecuteNonQuery();
                Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
            }
            Console.Read();
        }
        private static void UPDATE()
        {
            int Age = Convert.ToInt32(Console.ReadLine());
            string Name = Console.ReadLine();
            string UPD = $"UPDATE Users SET Age={Age} WHERE Name={Name}";
            using (SQN)
            {
                sqn.Open();
                SqliteCommand cmd = new SqliteCommand(UPD, SQN);
                int number = cmd.ExecuteNonQuery();
                Console.WriteLine($"Обновлено объектов: {number}");
            }
            Console.Read();
        }
        private static void DELETE() //delete manually row/column ## DELETE FROM таблица WHERE столбец = значение
        {
            Console.WriteLine("Введите имя столбца/строки");
            string Name = Console.ReadLine();
            string DEL = $"DELETE FROM Users WHERE Name = {Name}";
            using (SQN)
            {
                SQN.Open();
                SqliteCommand cmd = new SqliteCommand(DEL, SQN);
                //int number = cmd.ExecuteNonQuery();
                //Console.WriteLine($"Удалено объектов: {number}");
            }
            Console.Read();
        }
        public static void DataTable_Name()
        {
            List<string> tables = new List<string>();
            using (SqliteConnection con = new SqliteConnection(CNCT))
            {
                con.Open();
                try
                {
                    using (SqliteCommand com = new SqliteCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES", con))
                    {
                        using (SqliteDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tables.Add((string)reader["TABLE_NAME"]);
                            }
                        }
                    }
                }
                catch (SqliteException)
                {
                    Console.WriteLine($"No such table exists");
                }
                return;
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}