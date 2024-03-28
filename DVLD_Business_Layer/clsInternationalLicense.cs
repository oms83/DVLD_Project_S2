using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsInternationalLicense 
    {
        public enum enInterantionalMode { AddNew = 1, Update = 2 }

        private enInterantionalMode _Mode = enInterantionalMode.AddNew;

        public int CreatedByUserID { get; set; }
        public int ApplicationID { get; set; }
        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public bool IsActive { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public clsDriver DriverInfo { get; set; }
        public clsApplication ApplicationInfo { get; set; }
        private clsInternationalLicense(
                                    int CreatedByUserID,
                                    int ApplicationID,
                                    int InternationalLicenseID,
                                    int DriverID,
                                    int IssuedUsingLocalLicenseID,
                                    bool IsActive,
                                    DateTime IssueDate,
                                    DateTime ExpirationDate
                                 )
        {
            this.CreatedByUserID = CreatedByUserID;
            this.ApplicationID = ApplicationID;
            this.InternationalLicenseID = InternationalLicenseID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IsActive = IsActive;
            this.IssueDate = IssueDate;
            this.ExpirationDate  = ExpirationDate;

            this.DriverInfo = clsDriver.GetDriverInfoDriverID(this.DriverID);
            this.ApplicationInfo = clsApplication.GetApplicationInfoByApplicationID(this.ApplicationID);

            _Mode = enInterantionalMode.Update;
        }


        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.CreatedByUserID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IsActive = false;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now.AddYears(2);

            _Mode = enInterantionalMode.AddNew;
        }

        public static clsInternationalLicense GetInternationalLicenseInfoByID(int InternationalLicenseID)
        {

            int CreatedByUserID = -1;
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;

            bool IsFound = clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                                                                                  ref IssuedUsingLocalLicenseID, ref CreatedByUserID,
                                                                                  ref IssueDate, ref ExpirationDate,
                                                                                  ref IsActive);
            if (IsFound)
            {
                return new clsInternationalLicense(CreatedByUserID,
                                                   ApplicationID,
                                                   InternationalLicenseID,
                                                   DriverID,
                                                   IssuedUsingLocalLicenseID,
                                                   IsActive,
                                                   IssueDate,
                                                   ExpirationDate
                                                   );
            }
            else
            {
                return null;
            }

        }

        public static clsInternationalLicense GetInternationalLicenseInfoByDriverID(int DriverID)
        {

            int CreatedByUserID = -1;
            int ApplicationID = -1;
            int InternationalLicenseID = -1;
            int IssuedUsingLocalLicenseID = -1;
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;

            bool IsFound = clsInternationalLicenseData.GetInternationalLicenseInfoByDriverID(ref InternationalLicenseID, ref ApplicationID, DriverID,
                                                                                  ref IssuedUsingLocalLicenseID, ref CreatedByUserID,
                                                                                  ref IssueDate, ref ExpirationDate,
                                                                                  ref IsActive);
            if (IsFound)
            {
                return new clsInternationalLicense(CreatedByUserID,
                                                   ApplicationID,
                                                   InternationalLicenseID,
                                                   DriverID,
                                                   IssuedUsingLocalLicenseID,
                                                   IsActive,
                                                   IssueDate,
                                                   ExpirationDate
                                                   );
            }
            else
            {
                return null;
            }

        }

        public static clsInternationalLicense GetInternationalLicenseInfoByApplicationID(int ApplicationID)
        {

            int CreatedByUserID = -1;
            int DriverID = -1;
            int InternationalLicenseID = -1;
            int IssuedUsingLocalLicenseID = -1;
            bool IsActive = false;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;

            bool IsFound = clsInternationalLicenseData.GetInternationalLicenseInfoByApplicationID(ref InternationalLicenseID, ApplicationID, ref DriverID,
                                                                                  ref IssuedUsingLocalLicenseID, ref CreatedByUserID,
                                                                                  ref IssueDate, ref ExpirationDate,
                                                                                  ref IsActive);
            if (IsFound)
            {
                return new clsInternationalLicense(CreatedByUserID,
                                                   ApplicationID,
                                                   InternationalLicenseID,
                                                   DriverID,
                                                   IssuedUsingLocalLicenseID,
                                                   IsActive,
                                                   IssueDate,
                                                   ExpirationDate
                                                   );
            }
            else
            {
                return null;
            }

        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, 
                                                                                                 this.DriverID,
                                                                                                 this.IssuedUsingLocalLicenseID, 
                                                                                                 this.CreatedByUserID,
                                                                                                 this.IssueDate, 
                                                                                                 this.ExpirationDate,
                                                                                                 this.IsActive);

            return this.InternationalLicenseID != -1;
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID,
                                                                          this.ApplicationID,
                                                                          this.DriverID,
                                                                          this.IssuedUsingLocalLicenseID,
                                                                          this.CreatedByUserID,
                                                                          this.IssueDate,
                                                                          this.ExpirationDate,
                                                                          this.IsActive);
        }

        public static bool Delete(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.DeleteInternationalLicense(InternationalLicenseID);
        }

        public bool Delete()
        {
            return clsInternationalLicenseData.DeleteInternationalLicense(this.InternationalLicenseID);
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }

        public DataTable GetDriverInternationalLicenses()
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(this.DriverID);
        }
        public static bool IsInternationalLicenseExistByID(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByID(InternationalLicenseID);
        }

        public bool IsInternationalLicenseExistByID()
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByID(this.InternationalLicenseID);
        }

        public static bool IsInternationalLicenseExistByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByDriverID(DriverID);
        }

        public bool IsInternationalLicenseExistByDriverID()
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByDriverID(this.DriverID);
        }

        public static bool IsInternationalLicenseExistByApplicationID(int ApplicationID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByApplicationID(ApplicationID);
        }

        public bool IsInternationalLicenseExistByApplicationID()
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByApplicationID(this.ApplicationID);
        }
        public bool Save()
        {
            
            switch (_Mode)
            {
                case enInterantionalMode.AddNew:

                    if (_AddNewInternationalLicense())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enInterantionalMode.Update:

                    return _UpdateInternationalLicense();
            }

            return false;
        }

        
    }
}
