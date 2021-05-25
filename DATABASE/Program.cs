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
        public const string ALL_TABLES = ". tables"; //list all tables in database 
        public static SqliteConnection sqn = new SqliteConnection(CNCT);
        public static SqliteConnection SQN = new SqliteConnection(CNCT);
        public static OpenFileDialog ofd = new OpenFileDialog();
        public static SaveFileDialog sfd = new SaveFileDialog();

        [STAThread] // in case if using GUI
        public static void Main(string [] args)
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
                        INSERT_FEW();
                        break;
                    case 6:
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
                                string text = "";
                                string[] words = text.Split(' ');
                                try
                                {
                                    string w1 = words[0];
                                    string w2 = words[1];
                                    string w3 = words[2];
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    Console.WriteLine("Index was out of range.");
                                }
                                List<string> ID = new List<string>();
                                var id = reader.GetValue(0);
                                object[] objs = new object[1]; //1,2,3,  и т.д
                                //string[] strSummCities = dtTemp1.AsEnumerable().Select(s => s.Field<string>("City")).ToArray<string>(); //где dtTemp1 - экзмепляр datatable
                                var column_val = reader.GetValues(objs); //берем все значения столбцов с второй строки 
                                var column_values = column_val.ToString();
                                string result_ = "";

                                foreach (var id_ in column_values)
                                {
                                    result_ += id_ + "\n"; //можно разобрать массив и через сплит, но не суть
                                }
                                foreach (var value in Convert.ToString(id))
                                {
                                    var COLUMN_ROW = reader.GetString(value);
                                    //_ = dt.AsEnumerable().Select(r => r.Field<int>("id")).ToList();
                                    ID.Add(COLUMN_ROW); // заносим в лист наши значения со столбца [0]
                                }
                                string[] ids = ID.ToArray();
                                for (int i = 0; i > ids.Length; i++)
                                {
                                    using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default)) //перезапись. True - дозапись
                                    {
                                        sw.WriteLine(ids);
                                    }
                                    using (ZipFile zip = new ZipFile())
                                    {
                                        using (Archive archive = new Archive()) //создаем архив + преобразовать эту функцию в преобразование по GUID
                                        {
                                            if (sfd.ShowDialog() == DialogResult.Cancel)
                                            {
                                                return;
                                            }
                                            string FileName = sfd.FileName;
                                            zip.AddFile(ids.ToString());
                                            DataTable_Name();
                                            string zip_path = @"D://" + /*datatable.name*/ (FileName) + ".7z"; // сюда вместо элемента листа [0] можно впихнуть чето типо "selected button" <= wf
                                            archive.Save(zip_path, new ArchiveSaveOptions { Encoding = Encoding.ASCII, ArchiveComment = result_ }); // or Encoding

                                            //"Добавлен новый файл в архив, guid"
                                            //один архив - один гуид(вся строка, привязанная к нему), в нем будут лежать еще файлы (dta или другие)
                                            //соответственно, в папке будут лежать n архивов = n строк(гуидов). 7 гуидов(или строк) => 7 архивов и т.д
                                            //и соответственно, количество таблиц = количество папок, в которых буду лежать архивы с гуидами. 7 таблиц(tables) = 7 папок
                                            //path для папок = название таблицы
                                            //path для архивов = файловая херня (там будет столбец с названиями, типо type_material и т.д), вот их и вбивать в путь, перед 
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
                SqliteCommand cmd = new SqliteCommand(Exp, SQN); //13-17 /13-18
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
        private static void DELETE() //delete manually row/column ## DELETE FROM таблица WHERE столбец = значение
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
            List <string> tables = new List <string>();
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