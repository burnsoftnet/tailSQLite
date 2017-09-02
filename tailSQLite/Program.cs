/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BurnSoft;
namespace tailSQLite
{
    class Program
    {
        static void Main(string[] args)
        {
            bool didexist = false;
            Console.WriteLine(General.GetCommand(args, "t", "", ref didexist));
        }
    }
}
