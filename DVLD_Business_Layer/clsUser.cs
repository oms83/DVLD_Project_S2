using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsUser
    {
        public enum enUserMode { AddNew = 0, Update = 1 };
        private enUserMode _Mode;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsPerson PersonInfo { get; set; }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            this.PersonInfo = clsPerson.GetPersonInfoByPersonID(this.PersonID);

            _Mode = enUserMode.Update;
        }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;


            _Mode = enUserMode.AddNew;
        }

        public static clsUser GetUserInfoUserID(int UserID)
        {

            int PersonID = -1;
            string UserName = "";
            string Password = "";
            bool IsActive = true;

            bool IsFound = clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static clsUser GetUserInfoByPersonID(int PersonID)
        {

            int UserID = -1;
            string UserName = "";
            string Password = "";
            bool IsActive = true;

            bool IsFound = clsUserData.GetUserInfoByPersonID(ref UserID, PersonID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static clsUser GetUserInfoByUserName(string UserName)
        {

            int UserID = -1;
            int PersonID = -1;
            string Password = "";
            bool IsActive = true;

            bool IsFound = clsUserData.GetUserInfoByUserName(ref UserID, ref PersonID, UserName, ref Password, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);
            return (this.UserID > -1);
        }

        private bool _UpdateClassName()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static bool IsUserExistByUserID(int UserID)
        {
            return clsUserData.IsUserExistByUserID(UserID);
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            return clsUserData.IsUserExistByPersonID(PersonID);
        }

        public static bool IsUserExistByUserName(string UserName)
        {
            return clsUserData.IsUserExistByUserName(UserName);
        }
        public static bool IsUserExistByUserNameAndPassword(string UserName, string Password)
        {
            return clsUserData.IsUserExistByUserNameAndPassword(UserName, Password);
        }

        public bool DeleteUser()
        {
            return clsUserData.DeleteUser(this.UserID);
        }

        public bool IsUserExistByUserID()
        {
            return clsUserData.IsUserExistByUserID(this.UserID);
        }

        public bool IsUserExistByPersonID()
        {
            return clsUserData.IsUserExistByPersonID(this.PersonID);
        }

        public bool IsUserExistByUserName()
        {
            return clsUserData.IsUserExistByUserName(this.UserName);
        }
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static bool ChangePassword(int UserID, string Password)
        {
            return clsUserData.ChangePassword(UserID, Password);
        }

        public bool ChangePassword(string Password)
        {
            return clsUserData.ChangePassword(this.UserID, Password);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enUserMode.AddNew:

                    if (_AddNewUser())
                    {
                        _Mode = enUserMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enUserMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }
    }
}
