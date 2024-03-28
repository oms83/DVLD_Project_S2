using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsDriver
    {
        public enum enDriverMode { AddNew = 0, Update = 1 };
        private enDriverMode _Mode;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsPerson PersonInfo { get; set; }
        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;

            this.PersonInfo = clsPerson.GetPersonInfoByPersonID(this.PersonID);
            _Mode = enDriverMode.Update;
        }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;


            _Mode = enDriverMode.AddNew;
        }

        public static clsDriver GetDriverInfoDriverID(int DriverID)
        {
            
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;



            bool IsFound = clsDriverData.GetDriverInfoDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate);

            if (IsFound)
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDriver GetDriverInfoPersonID(int PersonID)
        {

            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;



            bool IsFound = clsDriverData.GetDriverInfoPersonID(ref DriverID, PersonID, ref CreatedByUserID, ref CreatedDate);

            if (IsFound)
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }


        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        private bool _UpdateClassName()
        {
            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID, this.CreatedDate);
        }

        public static bool DeleteDriver(int DriverID)
        {
            return clsDriverData.DeleteDriver(DriverID);
        }
        public bool DeleteDriver()
        {
            return clsDriverData.DeleteDriver(this.DriverID);
        }
        public static bool IsDriverExistByDriverID(int DriverID)
        {
            return clsDriverData.IsDriverExistByDriverID(DriverID);
        }

        public bool IsDriverExistByDriverID()
        {
            return clsDriverData.IsDriverExistByDriverID(this.DriverID);
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enDriverMode.AddNew:

                    if (_AddNewDriver())
                    {
                        _Mode = enDriverMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enDriverMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }
    }
}
