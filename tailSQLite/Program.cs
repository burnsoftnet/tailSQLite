/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BurnSoft;
using BurnSoft.Data;
using System.Data.SQLite;

namespace tailSQLite
{
    class Program
    {
        private static string _table;
        private static string _table_identity;
        private static int _interval;
        private static long _identitySeed;
        private static string _dbname;
        static void Main(string[] args)
        {
            bool didexist = false;

            _interval = General.GetCommand(args, "t", 5, ref didexist);
            _table = General.GetCommand(args, "table", "", ref didexist);
            _dbname = General.GetCommand(args, "db","", ref didexist);
            _table_identity = General.GetCommand(args, "idcol", "", ref didexist);
           
            _identitySeed = 0;
            Console.WriteLine("t switch value is : {0}",_interval);
            Console.WriteLine("Table switch is: {0}", _table);
            //Console.WriteLine("sql switch is: {0}", General.GetCommand(args, "sql", "", ref didexist));
            Timer t = new Timer(TimerCallback, null, 0, (_interval * 1000));
            Console.Read();
        }

        private static void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.
            Console.WriteLine("In TimerCallback: " + DateTime.Now);
            // Force a garbage collection to occur.
            GC.Collect();
        }
        static void startTail()
        {
            string errMsg = "";
            if (MySQLite.ConnectDB(_dbname,ref errMsg))
            {
                if (_identitySeed != 0)
                {
                    _identitySeed = MySQLite.getMaxID(_dbname, _table, _table_identity, ref errMsg);
                }
                string SQL = "";

            }
        }
    }
}
