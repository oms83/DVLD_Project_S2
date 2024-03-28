using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;

        private static string _FileName = "UserLoginInfo.txt";
        public static void WriteDataToFile(string UserName, string Password, bool SaveData)
        {
            if (SaveData)
            {
                string UserLoginInfo = UserName + "#//#" + Password;

                File.WriteAllText(_FileName, UserLoginInfo);
            }
            else
            {
                File.WriteAllText(_FileName, string.Empty);
            }
        }

        public static void ReadDataFromFile(ref string UserName, ref string Password)
        {
            if (!File.Exists(_FileName))
            {
                UserName = "";
                Password = "";
                return;
            }

            string UserLoginInfo = File.ReadAllText(_FileName);

            if (UserLoginInfo == "" || UserLoginInfo == null)
            {
                UserName = "";
                Password = "";
            }
            else
            {
                string[] arrUserLoginInfo = UserLoginInfo.Split(new string[] { "#//#" }, StringSplitOptions.None);

                UserName = arrUserLoginInfo[0];
                Password = arrUserLoginInfo[1];
            }

        }
    }
}
