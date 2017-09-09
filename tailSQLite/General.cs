/* ------------------------------------
 * Created By BurnSoft
 * www.burnsoft.net
 *------------------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSoft
{
    public class General
    {
        /// <summary>
        /// Auto detect which switch was used to seperate the commands. Since this is all up to the programmer
        /// on how or what switches the user is able to use, this method combined with the GetCommends functions listed below
        /// the user or programmer can easily saw use --command, /command, -command or even ?command
        /// as long as the it is something that is not an alphabetical character that is first, it can be used as a switch.
        /// </summary>
        /// <param name="sValue">One of the parameters that was passed</param>
        /// <returns></returns>
        static private string detectSwitch(string sValue)
        {
            string sAns = "";
            foreach (char c in sValue)
            {
                if (!char.IsLetter(c))
                {
                    if (sAns.Equals(""))
                    {
                        sAns = c.ToString();
                    } else
                    {
                        sAns += c.ToString();
                    }
                } else
                {
                    break;
                }
            }
            return sAns;
        }
        #region "Get Command arguments"
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
