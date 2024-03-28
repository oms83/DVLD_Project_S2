using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Business_Layer.clsApplicationType;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business_Layer
{
    public class clsApplicationType
    {
        public enum enApplicationTypeMode { AddNew = 0, Update = 1 };
        private enApplicationTypeMode _Mode;

        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }
        private clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;

            this.ApplicationFees = ApplicationFees;

            _Mode = enApplicationTypeMode.Update;
        }

        public clsApplicationType()
        {
            this.ApplicationTypeID = (int)clsApplication.enApplicationType.NewLocalDrvingLicense;
            this.ApplicationTypeTitle = "";

            this.ApplicationFees = 0;

            _Mode = enApplicationTypeMode.AddNew;
        }

        public static clsApplicationType GetApplicationTypeInfoByID(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";

            float ApplicationFees = 0;

            bool IsFound = clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees);

            if (IsFound)
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID = clsApplicationTypeData.AddNewApplicationType(this.ApplicationTypeTitle, this.ApplicationFees);
            return (this.ApplicationTypeID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }

        public static bool DeleteApplication(int ApplicationTypeID)
        {
            return clsApplicationTypeData.DeleteApplicationType(ApplicationTypeID);
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enApplicationTypeMode.AddNew:

                    if (_AddNewApplicationType())
                    {
                        _Mode = enApplicationTypeMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enApplicationTypeMode.Update:

                    return _UpdateApplicationType();
            }

            return false;
        }
    }
}
