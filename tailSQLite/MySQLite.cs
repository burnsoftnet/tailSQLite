﻿/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Data.SQLite;

namespace BurnSoft.Data
{
    /// <summary>
    /// General functions and subs used to connect and interact with the database
    /// </summary>
    public class MySQLite
    {
        /// <summary>
        /// Public facing connection Object
        /// </summary>
        static public SQLiteConnection conn;

        /// <summary>
        /// Basic Connection string format that is used in this class or you can use outside outside of this class
        /// </summary>
        /// <param name="dbName">name and path of the database</param>
        /// <returns></returns>
        static public string myConnectionString(string dbName)
        {
            return "Data Source=" + dbName + ";Version=3;PRAGMA journal_mode=WAL;";
        }
        /// <summary>
        /// Create a connection, if it is unable to connect it will return false
        /// if it does connect it will set the public facinf "conn" as the object that
        /// you can use for your program
        /// </summary>
        /// <param name="dbname">name and path of the database</param>
        /// <param name="sErrMsg">any error messages that occured</param>
        /// <returns></returns>
        static public bool ConnectDB(string dbname, ref string sErrMsg)
        {
            bool bAns = false;
            try
            {
                conn = new SQLiteConnection(myConnectionString(dbname));
                conn.Open();
                bAns = true;
            } catch (Exception ex)
            {
                sErrMsg = ex.Message.ToString();
            }
            return bAns;
        }
        /// <summary>
        /// Close the connection that is used by the conn object if it is open, then set to null
        /// </summary>
        static public void CloseDB()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
            conn = null;
        }
        /// <summary>
        /// execute a t-sql statement, if it was successfull it will return true, if it is false, 
        /// then the error sErrMsg will contain any error messages
        /// </summary>
        /// <param name="dbname">database name and path</param>
        /// <param name="mySQL">t-sql statement that you want to execute</param>
        /// <param name="sErrMsg">any error messages that was caught</param>
        /// <returns></returns>
        static public bool runQuery (string dbname, string mySQL, ref string sErrMsg)
        {
            bool bAns = false;
            string ErrMsg = "";
            try
            {
                if (ConnectDB(dbname,ref ErrMsg))
                {
                    SQLiteCommand CMD = new SQLiteCommand();
                    CMD.CommandText = mySQL;
                    CMD.Connection = conn;
                    CMD.ExecuteNonQuery();
                    CMD.Connection.Close();
                    CloseDB();
                    bAns = true;
                } else
                {
                    sErrMsg = ErrMsg;
                }
            } catch (Exception ex)
            {
                sErrMsg = ex.Message.ToString();
            }
            return bAns;
        }

        static public long getMaxID (string dbName, string table, string identityColumn, ref string errMsg)
        {
            long lAns = 0;
            string errorMsg = "";
            if (MySQLite.ConnectDB(dbName,ref errorMsg))
            {
                string SQL = "SELECT max(" + identityColumn + ") as maxid from " + table;
                SQLiteCommand CMD = new SQLiteCommand(SQL, MySQLite.conn);
                using (SQLiteDataReader RS = CMD.ExecuteReader())
                {
                    while (RS.Read())
                    {
                        lAns = RS.GetInt32(0);
                    }
                    RS.Close();
                }
                CMD = null;
                MySQLite.CloseDB();

                 
            } else
            {
                errMsg = errorMsg;
            }

            return lAns;
        }
    }
}
