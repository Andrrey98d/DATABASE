using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace DATABASE
{
    class tap_on_button : Program
    {
        OpenFileDialog on_open = new OpenFileDialog();
        SaveFileDialog on_save = new SaveFileDialog();

        [STAThread]
        static void Main(string[] args)
        {

        }

        public void writename_path(string path, string name)
        {
            name = on_open.FileName;
            path = @"D:\" + on_open.InitialDirectory + "\\" + name;
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
                            reader.
                            }
                        }
                    }
                }
            }
        }
    }
}
