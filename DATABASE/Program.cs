﻿using System;
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

namespace DATABASE
{
    class Program : Dialogs
    {
        public const string _CONNECT = @"Data Source = desktop - 9mrv6e2;Initial Catalog = crypto; Integrated Security = True"; // <---
        public static string SELECT_ALL = "SELECT * FROM dbo.Table_1";
        public static string SEL_ALL = "SELECT * FROM Users";
        public const string CNCT = "Data Source = usersdata.db";
        public const string ALL_TABLES = ".tables"; // list all tables in database 
        public static SqliteConnection sqn = new SqliteConnection(CNCT);
        public static SqliteConnection SQN = new SqliteConnection(CNCT);

        //[STAThread] - in case if using GUI
        public static async void Main(string[] args)
        {
            Console.WriteLine(CNCT + "\n");
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
                    INSERT_FEW();
                    break;
                case 6:
                    DELETE();
                    break;
            }

            using (sqn)
            {
                sqn.Open();
                SqliteCommand command = new SqliteCommand(SEL_ALL, sqn);
                SqliteCommand slc = new SqliteCommand(ALL_TABLES, sqn);
                SqliteDataReader reader = command.ExecuteReader();
                string path = @"D:\\guids.txt";
                if (reader.HasRows)
                {
                    using (slc)
                    {
                        using (command)
                        {
                            while (await reader.ReadAsync()) // counter++
                            {
                                string text = "";
                                string[] words = text.Split(' ');
                                string w1 = words[0];
                                string w2 = words[1];
                                string w3 = words[2];

                                List<string> ID = new List<string>();
                                var id = reader.GetValue(0);
                                object[] objs = new object[1]; // 1,2,3,  и т.д
                                var column_val = reader.GetValues(objs); // берем все значения столбцов с второй строки 
                                var column_values = column_val.ToString();
                                string result_ = "";

                                foreach (var id_ in column_values)
                                {
                                    result_ += id_ + "\n"; // можно разобрать массив и через сплит, но не суть
                                }

                                foreach (var value in Convert.ToString(id))
                                {
                                    var COLUMN_ROW = reader.GetString(value);
                                    //_ = dt.AsEnumerable().Select(r => r.Field<int>("id")).ToList();
                                    ID.Add(COLUMN_ROW);
                                }
                                string[] ids = ID.ToArray();
                                for (int i = 0; i > ids.Length; i++)
                                {
                                    //ids[0]
                                    //FileStream fs = new FileStream(path, FileMode.Append, FileAccess.ReadWrite);
                                    //fs.Write(ids, 0, count: ids.Length);

                                        //Directory.CreateDirectory($"D:\\{}");
                                    using (StreamWriter sw = new StreamWriter(path,
                                                                              false, // перезапись. True - дозапись
                                                                              Encoding.Default))
                                    {
                                        sw.WriteLine(ids);
                                    }
                                    using (ZipFile zip = new ZipFile())
                                    {
                                        using (Archive archive = new Archive()) // создаем архив + преобразовать эту функцию в преобразование по GUID*/
                                        {
                                            zip.AddFile(ids.ToString());
                                            string zip_path = @"D://d.7z";
                                            archive.Save(zip_path, new ArchiveSaveOptions { Encoding = Encoding.ASCII, ArchiveComment = result_ }); // or Encoding
                                            //"Добавлен новый файл в архив, guid"
                                        }
                                    }
                                    Console.WriteLine("{0}", id);
                                }
                                
                              
                            }
                        }
                    }
                    reader.Close();
                }
                Console.Read();
            }
        }

       


    public static void Create_NewTable()
        {
            //users.db лежит  в src/*proj name*/bin/debug
            var sqn = new SqliteConnection(CNCT);
            using (sqn)
            {
                sqn.Open();
                SqliteCommand CMD = new SqliteCommand();
                CMD.Connection = sqn;
                CMD.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL, Value INTEGER NOT NULL)";
                CMD.ExecuteNonQuery();
                if (CMD.CommandText.Substring(13, 18) == "Users") //index => 13-17 or 13-18 
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
                    CommandText = "INSERT INTO Users (Name, Age) VALUES ('Cryptocom', 128)"
                };
                int number = CMD.ExecuteNonQuery();
                Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
            }
            Console.Read();

        }
        public static void INSERT_FEW()  //primer: INSERT INTO Users (Name, Age) VALUES ('Alice', 32), ('Bob', 28)";
        {
            Console.WriteLine("INSERT NAME: ");
            string Name = Console.ReadLine(); ;
            Console.WriteLine("INSERT AGE: ");
            int Age = Convert.ToInt32(Console.ReadLine());
            string Exp = $"INSERT INTO Users (Name, Age) VALUES ({Name}, {Age}), ({Name}, {Age})";
            using (SQN)
            {
                SQN.Open();
                SqliteCommand cmd = new SqliteCommand(Exp, SQN);
                try
                {
                    int number = cmd.ExecuteNonQuery();
                    Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
                }
                catch (SqliteException)
                {
                    Dialogs d = new Dialogs();
                    Console.WriteLine(d.err);
                }
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
        private static void DELETE() // delete manually row/column ## DELETE FROM таблица WHERE столбец = значение
        {
            string Name = Console.ReadLine();
            string DEL = $"DELETE FROM Users WHERE Name = {Name}";
            using (SQN)
            {
                sqn.Open();
                SqliteCommand cmd = new SqliteCommand(DEL, SQN);
                int number = cmd.ExecuteNonQuery();
                Console.WriteLine($"Удалено объектов: {number}");
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
                catch (SqliteException) //13 -24
                {
                    Console.WriteLine($"No such table exists");
                }
            }
        }
    }
}

/* два варианта - либо через guid guid  пройтись в цикле по всем гуидам, создать массив с длиной, занести в цикле в каждый 
   элемент значение, с guid, и затем записать рез в файл, ЛИБО сразу преобразовать полученное с гуида в массив, вывести построчно, 
   поприсваивать значения, и тоже записать в файл преобразование массива в байтовый, в два шага - бесполезная трата времени 
*/
//{ SELECT_ALL.Substring(13, SELECT_ALL.Length - 2)}
//  using (Archive archive = new Archive()) // создаем архив + преобразовать эту функцию в преобразование по GUID*/
//{
//    zip.Password = MyPassword; //генерируем пароль по тыку кнопки создания архива
//    zip.AddFile(FileName); //файл для сохранения (последний пункт задания)
//    archive.Save(Sql_Path, new ArchiveSaveOptions { Encoding = Encoding.ASCII, ArchiveComment = "Добавлен новый файл в архив" });
//} 