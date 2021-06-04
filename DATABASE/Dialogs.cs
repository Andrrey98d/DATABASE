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
            Program pr = new Program();
            Console.WriteLine("1 - create new table\n2 - insert values\n3 - delete row");
            int res = Convert.ToInt32(Console.ReadLine());
            switch (res)
            {
                case 1:
                    pr.Create_NewTable();
                    break;
                case 2:
                    pr.INSERT_VALUES();
                    break;
                case 3:
                    pr.DELETE();
                    break;
            }
        }
    }
}
// СПЕЦІАЛІСТ ЗА РАХУНОК ПОСАДИ ГОЛОВНОГО СПЕЦІАЛІСТА 55 ЦЕНТРУ 2 ДЕПАРТАМЕНТУ 2 УПРАВЛІННЯ 2 ВІДДІЛУ МОЛОДШИЙ ЛЕНТЕНАТ ДЕГТЯР АНДРІЙ