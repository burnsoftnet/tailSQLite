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
    class General
    {

        static public string GetCommand(string[] args,string strLookFor, string sDefault, ref bool DidExist,string Switch = "/")
        {
            string sAns = "";
            DidExist = false;
            
            int cmdCount = args.Length;
            string sValue;
            for (int i=0; i <= cmdCount -1; i++)
            {
                sValue = args[i];
                sValue = sValue.Replace(Switch, "");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHigher = newValue.GetUpperBound(0);
                
                if (newValue[myLower].Equals(strLookFor,StringComparison.OrdinalIgnoreCase))
                {
                    if (myHigher != 0 )
                    {
                        sAns = newValue[myHigher];
                    } else
                    {
                        sAns = sDefault;
                    }
                    DidExist = true;
                    break;
                }

            }
            return sAns;
        }

        static public int GetCommand(string[] args, string strLookFor, int iDefault, ref bool DidExist,string Switch="/")
        {
            int iAns = 0;
            DidExist = false;
            int cmdCount = args.Length;
            string sValue;
            for (int i=0; i <= cmdCount -1; i++)
            {
                sValue = args[i];
                sValue = sValue.Replace(Switch,"");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHigher = newValue.GetUpperBound(0);
                if (newValue[myLower].Equals(strLookFor,StringComparison.OrdinalIgnoreCase))
                {
                    if (myHigher !=0)
                    {
                        iAns = Convert.ToInt32(newValue[myHigher]);
                    } else
                    {
                        iAns = iDefault;
                    }
                    DidExist = true;
                    break;
                }
            }
            return iAns;
        }

        static public bool GetCommand(string[] args, string strLookFor, bool bDefault, ref bool DidExist, string Switch = "/")
        {
            bool bAns = false;
            DidExist = false;
            int cmdCount = args.Length;
            string sValue;
            for (int i=0; i <= cmdCount -1; i++)
            {
                sValue = args[i];
                sValue = sValue.Replace(Switch, "");
                string[] newValue = sValue.Split('=');
                int myLower = newValue.GetLowerBound(0);
                int myHiger = newValue.GetUpperBound(0);
                if (newValue[myLower].Equals(strLookFor,StringComparison.OrdinalIgnoreCase))
                {
                    if (myHiger != 0)
                    {
                        
                    }
                }
            }
            return bAns;
        }
    }
}
