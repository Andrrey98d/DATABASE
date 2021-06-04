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
        public static string SEL_ALL = "SELECT * FROM Users";
        public const string CNCT = "Data Source = usersdata.db";
        public const string ALL_TABLES = ".tables"; //list all tables in database 
        public static SqliteConnection sqn = new SqliteConnection(CNCT);
        public static SqliteConnection SQN = new SqliteConnection(CNCT);
        public static OpenFileDialog ofd = new OpenFileDialog();
        public static SaveFileDialog sfd = new SaveFileDialog();    

        [STAThread] // in case if using GUI
        public static void Main( )
        {
            Console.WriteLine(CNCT + "\n");
            ShowMenu();
            Initialize_Connection();
        }
        public static void Initialize_Connection()
        {
            using (sqn)
            {
                sqn.Open();
                SqliteCommand command = new SqliteCommand(SEL_ALL, sqn);
                SqliteCommand slc = new SqliteCommand(ALL_TABLES, sqn);
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    using (slc)
                    {
                        using (command)
                        {
  
                            while (reader.Read()) //counter++ => removed await method to avoid non-recognising point of entry (Main)
                            {
                                bool hasRows = reader.HasRows;
                                if (hasRows == true)
                                {
                                    List<string> ofValues = new List<string>();
                                    Console.WriteLine("");
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        Console.Write("\t" + reader[i]);
                                        ofValues.Add(reader[i].ToString());
                                    }
                                    Console.WriteLine("");
                                    string val = "";
                                    for (int i = 0; i < ofValues.Count; i++)
                                    {
                                        val += "\n" + ofValues[i];
                                    }
                                      string path = @"D:\\" + ofValues.Count
                                    + ".txt";
                                    Console.WriteLine("");
                                    Console.WriteLine("write? y/n");
                                    string res = Console.ReadLine();
                                    if (res == "y")
                                    {

                                        using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                                        {
                                            foreach (var values in ofValues)
                                            {
                                                sw.WriteLine(val);
                                            }
                                            sw.Close();
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    //string w1 = words[0]; [0],1,2..3..n
                                    //List<string> ID = new List<string>();
                                    //var id = reader.GetValue(0);
                                    //object[] objs = new object[1];  
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

        public void Create_NewTable()
        {
            //users.db лежит  в src/*proj name*/bin/debug
            var sqn = new SqliteConnection(CNCT);
            using (sqn)
            {
                sqn.Open();
                SqliteCommand CMD = new SqliteCommand
                {
                    Connection = sqn,
                    CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Nameee TEXT NOT NULL, Age INTEGER NOT NULL, Value INTEGER NOT NULL)"
                };
                if (CMD.CommandText.Substring(13, 18) == "Users") //or 13-17
                {
                    try
                    {
                        CMD.ExecuteNonQuery();
                    }
                    catch (SqliteException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                Console.WriteLine("Таблица Users создана");
            }
            Console.Read();
        }
        public void INSERT_VALUES()
        {
            // добавить ридер по столбцам, прирaвняв его к переменной (var => reader[0])
            using (SQN)
            {
                SQN.Open();
                SqliteCommand CMD = new SqliteCommand
                {
                    Connection = SQN,
                    CommandText = "INSERT INTO Users (Nameee, Age) VALUES ('Cryptocommand', 128)"
                };
                int number = CMD.ExecuteNonQuery();
                Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
            }
            Console.Read();
        }
        public void DELETE() //delete manually row/column ## DELETE FROM таблица WHERE столбец = значение
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
    }
}