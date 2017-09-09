/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Collections;
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
        private static bool _showTables;
        private static bool _DoTail;
        /// <summary>
        /// Initialize private variables with the command arguments that were passed to the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void init(string[] args)
        {
            bool didexist = false;
            _DoTail = false;
            _dbname = General.GetCommand(args, "db", "", ref didexist);
            isRequired("/db", didexist);
            _interval = General.GetCommand(args, "t", 5, ref didexist);
            _table = General.GetCommand(args, "table", "", ref didexist);
            if (didexist)
            {
                _table_identity = General.GetCommand(args, "idcol", "", ref didexist);
                isRequired("/idcol", didexist);
                _DoTail = true;
            }
            _showTables = General.GetCommand(args, "showtables", false, ref didexist);

            _identitySeed = 0;
        }
        /// <summary>
        /// Main starting sub
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            init(args);

            if (!_showTables && _DoTail)
            {
                Timer t = new Timer(TimerCallback, null, 0, (_interval * 1000));
                Console.Read();
            } else if(_showTables && !_DoTail) {
                showtables();
            } else if (!_showTables && !_DoTail)
            {
                Console.WriteLine("You need to select another switch!");
            }
            
        }
        
        /// <summary>
        /// Used for the Command arguements to alert and exit if a
        /// required field/switch is missing
        /// </summary>
        /// <param name="ReqSwitch">Missing switch name</param>
        /// <param name="didexist">Boolean value passed from the general.getcommand didexist value</param>
        static void isRequired(string ReqSwitch,bool didexist)
        {
            if (!didexist)
            {
                Console.WriteLine("Missing required value: {0}", ReqSwitch);
                System.Environment.Exit(1);
            }
        }
        /// <summary>
        /// timer to start the tail on the sqlite database
        /// </summary>
        /// <param name="o"></param>
        private static void TimerCallback(Object o)
        {
           //add sqlite tail function here
            // Force a garbage collection to occur.
            GC.Collect();
        }
        /// <summary>
        /// Show the tables for the selected sqlite database
        /// requires the /db=dbname and path switch
        /// </summary>
        static private void showtables()
        {
            ArrayList tableList = new ArrayList();
            string errorMsg = "";
            tableList = MySQLite.listTables(_dbname, ref errorMsg);
            if (tableList != null)
            {
                Console.WriteLine("table name");
                Console.WriteLine("-----------");
                foreach (string value in tableList)
                {
                    Console.WriteLine(value);
;                }
                Console.WriteLine("");
            } else
            {
                if (errorMsg.Equals(""))
                {
                    Console.WriteLine("No Tables Listed!");
                } else
                {
                    Console.WriteLine("ERROR: {0}", errorMsg);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Press Any Key to Exit!");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
        /// <summary>
        /// start the database tail
        /// </summary>
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
