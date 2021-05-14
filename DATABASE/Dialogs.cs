using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE
{
    public class Dialogs
    {
        public static void ShowMenu()
        {
            Console.WriteLine("1 - show connection string\n2 - get DB table(-s) name\n3 - create new table\n4 - insert values");
        }
    }
}
