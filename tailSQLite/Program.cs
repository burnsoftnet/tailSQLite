﻿/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Collections;
using System.Threading;
using BurnSoft;
using BurnSoft.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
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
        private static bool _DEBUG;
        private static Timer t;
        private static string _LOGTOFILE;
        private static bool _DOLOG;
        /// <summary>
        /// Showhelps this instance.
        /// </summary>
        static void showhelp()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.WriteLine("{0}", fvi.Comments);
            Console.WriteLine("");
            Console.WriteLine("--h or --help       This Help File");
            Console.WriteLine("--tail             Tell the app to be in tail mode, Requires the db, table and idcol parameters, the t parameter is optional");
            Console.WriteLine("--db=DatabaseName  The path and name of the database you want to follow");
            Console.WriteLine("--t=seconds        the number of seconds you want to refresh the trace, by default this will be 5 seconds");
            Console.WriteLine("--table            The table name that you want to trace");
            Console.WriteLine("--idol             the Identity column to the table that you want to tail");
            Console.WriteLine("--showtables       Show all the tables of the database, requires the db parameter");
            Console.WriteLine("--showcolumns      Show all the columns for the table requires the table & db parameters");
            Console.WriteLine("--debug            Display Debug messages, currently this is just displaying on the query when running the tail.");
            Console.WriteLine("--log=LOGNAME      Write output to a log file as well as display on screen.");
            Console.WriteLine("");
            Console.WriteLine("Examples:");
            Console.WriteLine("");
            Console.WriteLine("start tailing:");
            Console.WriteLine("{0} --table=process_stats_main --idcol=id --db=C:\\BSAP\\bsap_client.db --t=5 -tail", fvi.InternalName);
            Console.WriteLine("");
            Console.WriteLine("Show Tables:");
            Console.WriteLine("{0} --db=C:\\BSAP\\bsap_client.db --showtables", fvi.InternalName);
            Console.WriteLine("");
            Console.WriteLine("Show Columns:");
            Console.WriteLine("{0} --db=C:\\BSAP\\bsap_client.db --table=process_stats_main --showcolumns", fvi.InternalName);
            Console.WriteLine("");
            Console.WriteLine("Press Any Key to Exit");
            Console.Read();
            //Console.WriteLine("");
            System.Environment.Exit(0);
        }
        /// <summary>
        /// Headers this instance.
        /// </summary>
        static void header()
        {
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            Console.WriteLine("{0} v.{1}", fvi.InternalName, fvi.ProductVersion);
            Console.WriteLine("By {0} - {1}", fvi.CompanyName,fvi.LegalCopyright);
            Console.WriteLine("");
            
        }
        /// <summary>
        /// Initialize private variables with the command arguments that were passed to the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void init(string[] args)
        {
            try
            {
                bool didexist = false;
                if (General.GetCommand(args, "help", false, ref didexist) || General.GetCommand(args, "h", false, ref didexist))
                {
                    header();
                    showhelp();
                }
                _DEBUG = General.GetCommand(args, "debug", false, ref didexist);
                _LOGTOFILE = General.GetCommand(args, "log", "", ref _DOLOG);
                _DoTail = General.GetCommand(args,"tail",false, ref didexist);
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
                    if (_table.Length > 0)
                    {
                        _tablename = _table;
                        showColumns(_tablename);
                    } else
                    {
                        throw new Exception("You are missing the table switch that you want to view the columns of.");
                    }
                } 

                _identitySeed = 0;
                if (_DOLOG)
                {
                    General.AppendToFile(_LOGTOFILE, $"Database Name: {_dbname}\n");
                    General.AppendToFile(_LOGTOFILE, $"Refresh Every { _interval} seconds. \n");
                    General.AppendToFile(_LOGTOFILE, $"Table Name: {_table}\n");
                    General.AppendToFile(_LOGTOFILE, $"Identity Seed: {_table_identity}\n");
                    General.AppendToFile(_LOGTOFILE, $"\n");
                }
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
            header();
            if (!_showTables && _DoTail)
            {
                TimeSpan TimeToRun = new TimeSpan(0, 0, _interval);
                t = new Timer(TimerCallback, null, TimeToRun, TimeToRun);
                Console.WriteLine("Waiting for Results");
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
            startTail();
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
                MySQLite obj = new MySQLite();
                tableList = obj.listTables(_dbname, ref errorMsg);
                if (tableList != null)
                {
                    Console.WriteLine("table name");
                    Console.WriteLine("-----------");
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "table name"); }
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "-----------"); }
                    foreach (string value in tableList)
                    {
                        Console.WriteLine(value);
                        if (_DOLOG) { General.AppendToFile(_LOGTOFILE,value); }
                    }
                    Console.WriteLine("");
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, ""); }
                }
                else
                {
                    if (errorMsg.Equals(""))
                    {
                        Console.WriteLine("No Tables Listed!");
                        if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "No Tables Listed!"); }
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
                MySQLite obj = new MySQLite();

                ArrayList columns = obj.listColumns(_dbname, stable, ref errorMsg);
                if (columns.Count > 0)
                {
                    Console.WriteLine("column name");
                    Console.WriteLine("-----------");
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "column name"); }
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "-----------"); }
                    foreach (string value in columns)
                    {
                        Console.WriteLine(value);
                        if (_DOLOG) { General.AppendToFile(_LOGTOFILE, value); }
                    }
                    Console.WriteLine("");
                    if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "\n"); }
                }
                else
                {
                    if (String.IsNullOrEmpty(errorMsg))
                    {
                        Console.WriteLine("No columns Listed!");
                        if (_DOLOG) { General.AppendToFile(_LOGTOFILE, "No columns Listed!\n"); }
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
            try
            {
                string errMsg = "";
                string logToFile = "";

                MySQLite obj = new MySQLite();
                ArrayList columns = obj.listColumns(_dbname, _table, ref errMsg);
                int columncount = columns.Count;

                if (_identitySeed == 0)
                {
                    _identitySeed = obj.getMaxID(_dbname, _table, _table_identity, ref errMsg);
                }

                if (obj.ConnectDB(_dbname, ref errMsg))
                {
                    string SQL = "select * from " + _table + " where id > " + _identitySeed;
                    if (_DEBUG) { Console.WriteLine(SQL); }
                    SQLiteCommand CMD = new SQLiteCommand(SQL, obj.conn);
                    using (SQLiteDataReader rs = CMD.ExecuteReader())
                    {

                        while (rs.Read())
                        {
                            foreach (string value in columns)
                            {
                                var svalue = rs.GetValue(rs.GetOrdinal(value));
                                Console.WriteLine("{0}: {1}", value, svalue.ToString());
                                if (_DOLOG) { logToFile += "\n" + value + ": " + svalue.ToString(); }
                                if (value.Equals(_table_identity))
                                {
                                    _identitySeed = Convert.ToInt32(svalue);
                                }
                            }
                            Console.WriteLine("");
                        }
                        rs.Close();
                    }
                    CMD = null;
                    obj.CloseDB();
                    obj = null;
                    if (_DOLOG && logToFile.Length > 0) { General.AppendToFile(_LOGTOFILE, logToFile); }

                } else
                {
                    throw new Exception(errMsg);
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
