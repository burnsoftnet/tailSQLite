/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Data.SQLite;

namespace BurnSoft.Data
{
    public class MySQLite
    {
        static public SQLiteConnection conn;
        static public string myConnectionString(string dbName)
        {
            return "Data Source=" + dbName + ";Version-3";
        }

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
        static public void CloseDB()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
            conn = null;
        }
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
    }
}
