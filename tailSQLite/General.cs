/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.IO;
using System.Text;

namespace BurnSoft
{
    /// <summary>
    /// The General class contains misc. subs and functions that can be used for this project or any other project in general
    /// </summary>
    public class General
    {
        /// <summary>
        /// Current Class Name for error logging
        /// </summary>
        private static string CURRENT_CLASS
        {
            get
            {
                return "BurnSoft.General";
            }
        }
        /// <summary>
        /// Error Message Formating to include the class location and the sub or function that it came from
        /// also the exception that occurred.
        /// </summary>
        /// <param name="fromSubFunction"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string ErrorMessage(string fromSubFunction, Exception ex)
        {
            string sAns = CURRENT_CLASS + "." + fromSubFunction + " - " + ex.Message.ToString();
            return sAns;
        }
        #region "File Operations"
        /// <summary>
        /// format the path for c# standards considering the singular backslash is an escape character
        /// </summary>
        /// <param name="sPath">file and and path</param>
        /// <returns>string with forward slash</returns>
        public static string FormatFilepath(string sPath)
        {
            string sAns = "";
            sAns = sPath.Replace("\\", "//");
            return sAns;
        }
        /// <summary>
        /// Public Sub to write a value to the log file, also appending the current date and time to the
        /// value that was passed
        /// </summary>
        /// <param name="logfile">Name and Path of logfile</param>
        /// <param name="sValue">The new line that you want to add to the logfile</param>
        public static void WriteToLog(string logfile, string sValue)
        {
            string sMsg = DateTime.Now + sValue;
            AppendToFile(logfile, sValue);
        }
        /// <summary>
        /// Append contents to an Exisintg file.  Or non-existing since it will create the file if it does
        /// not exist.  Can be used on it's own, currently the WriteToLog sub usies this sub as well
        /// </summary>
        /// <param name="sPath">Name and Path of Logfile</param>
        /// <param name="sValue">The new line that you want o add to the file</param>
        public static void AppendToFile(string sPath, string sValue)
        {
            if (!File.Exists(sPath)) { CreateFile(sPath); }
            StreamWriter sw = new StreamWriter(sPath, true, ASCIIEncoding.ASCII);
            sw.WriteLine(sValue);
            sw.Close();
        }
        /// <summary>
        /// Private sub to create a file if it does not exist
        /// </summary>
        /// <param name="sPath">path and name of file</param>
        private static void CreateFile(string sPath)
        {
            if (!File.Exists(sPath))
            {
                FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                fs.Close();
            }
        }
        #endregion
        #region "Get Command arguments"
        /// <summary>
        /// Auto detect which switch was used to seperate the commands. Since this is all up to the programmer
        /// on how or what switches the user is able to use, this method combined with the GetCommends functions listed below
        /// the user or programmer can easily saw use --command, /command, -command or even ?command
        /// as long as the it is something that is not an alphabetical character that is first, it can be used as a switch.
        /// </summary>
        /// <param name="sValue">One of the parameters that was passed</param>
        /// <returns>string value of the assumed switch</returns>
        private static string detectSwitch(string sValue)
        {
            string sAns = "";
            foreach (char c in sValue)
            {
                if (!char.IsLetter(c))
                {
                    if (sAns.Equals(""))
                    {
                        sAns = c.ToString();
                    }
                    else
                    {
                        sAns += c.ToString();
                    }
                }
                else
                {
                    break;
                }
            }
            return sAns;
        }
        /// <summary>
        /// The GetCommand will parse through the command arguments that was passed to the application
        /// and look for the switch that you want it to look for and return that value
        /// </summary>
        /// <param name="args">Command Arguments passed from Main</param>
        /// <param name="strLookFor">The String that you want to look for </param>
        /// <param name="sDefault">the default string value you want to use if the switch doesn't exist</param>
        /// <param name="DidExist">a boolean value to us if there are additional action you want to take after the GetCommand completes</param>
        /// <param name="Switch">the charcter that was used for the command switch i.e ( /, --, -, etc)</param>
        /// <returns>string value</returns>
        static public string GetCommand(string[] args, string strLookFor, string sDefault, ref bool DidExist)
        {
            string sAns = "";
            DidExist = false;

            int cmdCount = args.Length;
            string sValue;
            for (int i = 0; i <= cmdCount - 1; i++)
            {
                sValue = args[i];
                string Switch = detectSwitch(sValue);
                sValue = sValue.Replace(Switch, "");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHigher = newValue.GetUpperBound(0);

                if (newValue[myLower].Equals(strLookFor, StringComparison.OrdinalIgnoreCase))
                {
                    if (myHigher != 0)
                    {
                        sAns = newValue[myHigher];
                    }
                    else
                    {
                        sAns = sDefault;
                    }
                    DidExist = true;
                    break;
                }

            }
            return sAns;
        }

        /// <summary>
        /// The GetCommand will parse through the command arguments that was passed to the application
        /// and look for the switch that you want it to look for and return that value
        /// </summary>
        /// <param name="args">Command Arguments passed from Main</param>
        /// <param name="strLookFor">The String that you want to look for </param>
        /// <param name="iDefault">the default integer value you want to use if the switch doesn't exist</param>
        /// <param name="DidExist">a boolean value to us if there are additional action you want to take after the GetCommand completes</param>
        /// <param name="Switch">the charcter that was used for the command switch i.e ( /, --, -, etc)</param>
        /// <returns>Integer Value</returns>
        static public int GetCommand(string[] args, string strLookFor, int iDefault, ref bool DidExist)
        {
            int iAns = 0;
            DidExist = false;
            int cmdCount = args.Length;
            string sValue;
            for (int i = 0; i <= cmdCount - 1; i++)
            {
                sValue = args[i];
                string Switch = detectSwitch(sValue);
                sValue = sValue.Replace(Switch, "");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHigher = newValue.GetUpperBound(0);
                if (newValue[myLower].Equals(strLookFor, StringComparison.OrdinalIgnoreCase))
                {
                    if (myHigher != 0)
                    {
                        iAns = Convert.ToInt32(newValue[myHigher]);
                    }
                    else
                    {
                        iAns = iDefault;
                    }
                    DidExist = true;
                    break;
                }
            }
            return iAns;
        }
        /// <summary>
        /// The GetCommand will parse through the command arguments that was passed to the application
        /// and look for the switch that you want it to look for and return that value
        /// </summary>
        /// <param name="args">Command Arguments passed from Main</param>
        /// <param name="strLookFor">The String that you want to look for</param>
        /// <param name="bDefault">the default boolean value you want to use if the switch doesn't exist</param>
        /// <param name="DidExist">a boolean value to us if there are additional action you want to take after the GetCommand completes</param>
        /// <param name="Switch">the charcter that was used for the command switch i.e ( /, --, -, etc)</param>
        /// <returns>boolean</returns>
        static public bool GetCommand(string[] args, string strLookFor, bool bDefault, ref bool DidExist)
        {
            bool bAns = false;
            DidExist = false;
            int cmdCount = args.Length;
            string sValue;
            for (int i = 0; i <= cmdCount - 1; i++)
            {
                sValue = args[i];
                string Switch = detectSwitch(sValue);
                sValue = sValue.Replace(Switch, "");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHiger = newValue.GetUpperBound(0);
                if (newValue[myLower].Equals(strLookFor, StringComparison.OrdinalIgnoreCase))
                {
                    if (myHiger != 0)
                    {
                        bAns = Convert.ToBoolean(newValue[myHiger]);
                    }
                    else
                    {
                        bAns = true;
                    }
                    DidExist = true;
                    break;
                }
            }
            return bAns;
        }

        #endregion
    }
}
