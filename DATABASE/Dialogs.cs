using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE
{
    public class Dialogs
    {
        public string _err;
        public string err
        {
            get
            {
                return _err;
            }
            set
            {
                _err = "Error, no such column exists.";
            }
        }
        //public static void Main()
        //{
        //    ShowMenu();
        //}
        public static void ShowMenu()
        {
            Console.WriteLine("1 - create new table\n2 - update table\n3 - insert values\n4 - delete row");
        }
    }
}
