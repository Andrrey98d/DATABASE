using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Server;
//using System.Data.Sql;
//using System.Data.SqlClient;
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
        public const string _CONNECT = @"Data Source = desktop - 9mrv6e2;Initial Catalog = itproger; Integrated Security = True"; // <---
        public static string SELECT_ALL = "SELECT * FROM dbo.Table_1";
        public const string CNCT = "Data Source=usersdata.db";
        public static SqliteConnection sqn = new SqliteConnection(CNCT);
        public static SqliteConnection SQN = new SqliteConnection(CNCT);

        //[STAThread] - in case if using GUI

        public static void Main(string[] args)
        {
            ShowMenu();
            int res = Convert.ToInt32(Console.ReadLine());
            switch (res)
            {
                case 1:
                    Show_Connection_Line();
                    break;
                case 2:
                    for (int i = 0; i <= 2; i++)
                    {
                        Console.Write(".");
                    }
                    DataTable_Name();
                    break;
                case 3:
                    Create_NewTable();
                    break;
                case 4:
                    INSERT_VALUES();
                    break;
                case 5:
                    INSERT_FEW();
                    break;
            }
            //while (res != 1 || res != 2)          
            //{
            //    Console.WriteLine("Type 1 or 2...  ");
            //    Console.ReadLine();
            //}

            //SqlConnection connection = new SqlConnection(connectionString);
            using (sqn)
            {
                sqn.Open();
                
                SqliteCommand command = new SqliteCommand(SELECT_ALL, sqn);
                SqliteDataReader reader = command.ExecuteReader();
                string path = @"D:\\guids.txt";
                if (reader.HasRows)
                {
                    using (command)
                    {
                        while (reader.Read()) // counter++
                        {
                            //int ct = reader.FieldCount;
                            object id = reader[0]; // надо вытянуть с него массив значений guid, и взять файлстримом
                            /*
                            foreach(var i in id) { 
                            Console.WriteLine(i);
                            }
                            */
                            Console.WriteLine(id); // проходим консольным выводом 

                            //foreach(guid g in id)    

                            for (int i = 0; i > 0; i++)
                            {
                                Guid guid = reader.GetGuid(i);
                                byte[] bytes = guid.ToByteArray(); // преобразование напрямую из гуида, пройдясь циклом
                                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.ReadWrite);
                                fs.Write(bytes, 0, count: bytes.Length);

                                //Directory.CreateDirectory($"D:\\{}");
                                using (StreamWriter sw = new StreamWriter(path,
                                                                          true,
                                                                          Encoding.Default))
                                {
                                    sw.WriteLineAsync(Convert.ToChar(bytes.Length));
                                }
                                using (ZipFile zip = new ZipFile())
                                {
                                    using (Archive archive = new Archive()) // создаем архив + преобразовать эту функцию в преобразование по GUID*/
                                    {
                                        string zip_path = @"D://database_cr. /*7zip*/ ?";  // 7zip             zip.AddFile(path); //файл для сохранения (последний пункт задания)
                                        archive.Save(zip_path, new ArchiveSaveOptions  { Encoding = Encoding.ASCII, ArchiveComment = "Добавлен новый файл в архив, guid" }); //Default encoding
                                    }
                                }
                                Console.WriteLine("{0}", id);
                            }
                        }
                        Console.WriteLine("{0}\t{1}\t{2}", reader[0], reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4), reader.GetName(5));

                        while (reader.Read()) //++
                        {
                            object id = reader[0]; // этот столбец мы и выдираем
                            object firstValue = reader.GetValue(1); // это столбец со значениями (не гуиды)
                            object secondValue = reader.GetValue(2); // ---------||---------
                            object thirdValue = reader.GetValue(3); //  ---------||---------
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}", id, firstValue, secondValue, thirdValue); // выводим чисто в информативных целях 
                        }
                    } 
                    reader.Close();
                }
                Console.Read();
            }

        }
        public static void Show_Connection_Line()
        {
            Console.WriteLine(CNCT);
        }

        public static void Create_NewTable()
        {
            //users.db лежит  в src/*proj name*/bin/debug
            var sqn = new SqliteConnection(CNCT);
            using (sqn)
            {
                sqn.Open();
                SqliteCommand CMD = new SqliteCommand(); //13-17 ili 13-18
                CMD.Connection = sqn;
                CMD.CommandText = "CREATE TABLE Users(_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Age INTEGER NOT NULL, Value INTEGER NOT NULL)";
                CMD.ExecuteNonQuery();
                if(CMD.CommandText.Substring(13,18) == "Users")
                {
                    Console.WriteLine("Table already exists");
                }

                Console.WriteLine("Таблица Users создана");
            }
            Console.Read();
        }

        public static void INSERT_VALUES()
        {
            // добавить ридер по столбцам, прировняв его к переменной (var => reader[0])
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

        public static void INSERT_FEW()  //INSERT INTO Users (Name, Age) VALUES ('Alice', 32), ('Bob', 28)";
        {
            string Name = Console.ReadLine(); ;
            int Age = Convert.ToInt32(Console.ReadLine());
            string Exp = $"INSERT INTO Users (Name, Age) VALUES ({Name}, {Age}), ({Name}, {Age})";
            using (SQN)
            {
                SQN.Open();
                SqliteCommand cmd = new SqliteCommand(Exp, SQN);
                int number = cmd.ExecuteNonQuery();
                Console.WriteLine($"В таблицу Users добавлено объектов: {number}");
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
                    Console.WriteLine($"No such table {SELECT_ALL.Substring(13, SELECT_ALL.Length-1)} exists");
                }
            }
        }
        
    }
}

/* два варианта - либо через guid guid  пройтись в цикле по всем гуидам, создать массив с длиной, занести в цикле в каждый 
   элемент значение, с guid, и затем записать рез в файл, ЛИБО сразу преобразовать полученное с гуида в массив, вывести построчно, 
   поприсваивать значения, и тоже записать в файл преобразование массива в байтовый, в два шага - бесполезная трата времени 
*/

//for (int c = 0; c < guidArray.Length; c++)
//{
//    guidArray[i] = guid;
//}
//byte[] guidArray_new = guidArray; <= не рабочий вариант 

//  using (Archive archive    = new Archive()) // создаем архив + преобразовать эту функцию в преобразование по GUID*/
//{
//    zip.Password = MyPassword; //генерируем пароль по тыку кнопки создания архива
//    zip.AddFile(FileName); //файл для сохранения (последний пункт задания)
//    archive.Save(Sql_Path, new ArchiveSaveOptions { Encoding = Encoding.ASCII, ArchiveComment = "Добавлен новый файл в архив" });
//} 