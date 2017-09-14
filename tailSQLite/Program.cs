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
        private static string _tablename;
        private static bool _showcolumns;
        /// <summary>
        /// Initialize private variables with the command arguments that were passed to the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void init(string[] args)
        {
            try
            {
                //-showcolumns -tablename=app_project_name --db=C:\BSAP\bsap_client.db
                //-showtables --db=C:\BSAP\bsap_client.db
                bool didexist = false;
                _DoTail = false;
                _dbname = General.GetCommand(args, "db", "", ref didexist);
                isRequired("/db", didexist);
                didexist = false;
                _interval = General.GetCommand(args, "t", 5, ref didexist);
                didexist = false;
                _table = General.GetCommand(args, "table", "", ref didexist);
                if (didexist)
                {
                    _table_identity = General.GetCommand(args, "idcol", "", ref didexist);
                    isRequired("/idcol", didexist);
                    _DoTail = true;
                }
                _showTables = General.GetCommand(args, "showtables", false, ref didexist);

                _showcolumns = General.GetCommand(args, "showcolumns", false, ref didexist);
                if (_showcolumns)
                {
                    didexist = false;
                    _tablename = General.GetCommand(args, "tablename", "", ref didexist);
                    if (didexist)
                    {
                        showColumns(_tablename);
                    } else
                    {
                        throw new Exception("You are missing the table switch that you want to view the columns of.");
                    }
                } 

                _identitySeed = 0;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                System.Environment.Exit(1);
            }
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
            try
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
                        ;
                    }
                    Console.WriteLine("");
                }
                else
                {
                    if (errorMsg.Equals(""))
                    {
                        Console.WriteLine("No Tables Listed!");
                    }
                    else
                    {
                        throw new Exception(errorMsg);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            Console.WriteLine("");
            Console.WriteLine("Press Any Key to Exit!");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
        /// <summary>
        /// Display the columns for the selected table
        /// </summary>
        /// <param name="stable">The Table Name</param>
        static private void showColumns(string stable)
        {

            try
            {
                string errorMsg = "";
                ArrayList columns = MySQLite.listColumns(_dbname, stable, ref errorMsg);
                if (columns.Count > 0)
                {
                    Console.WriteLine("column name");
                    Console.WriteLine("-----------");
                    foreach (string value in columns)
                    {
                        Console.WriteLine(value);
                        ;
                    }
                    Console.WriteLine("");
                }
                else
                {
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        Console.WriteLine("No columns Listed!");
                    }
                    else
                    {
                        throw new Exception(errorMsg);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
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
                //string SQL = "";

            }
        }
    }
}
