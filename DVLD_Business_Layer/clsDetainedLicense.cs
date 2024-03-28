using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsDetainedLicense 
    {
        public enum enDetainedMode {AddNew = 1, Update = 2}

        private enDetainedMode _Mode = enDetainedMode.AddNew;

        public int CreatedByUserID { get; set; }
        public int LicenseID { get; set; }
        public int DetainID { get;  set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }
        public float FineFees { get; set; }
        public bool IsReleased { get; set; }
        public DateTime DetainDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        private clsDetainedLicense(
                                    int DetainID,
                                    int LicenseID,
                                    int CreatedByUserID,
                                    int ReleasedByUserID,
                                    int ReleaseApplicationID,
                                    float FineFees,
                                    bool IsReleased,
                                    DateTime DetainDate,
                                    DateTime ReleaseDate
                                 )
        {
            this.DetainID  = DetainID ; 
            this.LicenseID = LicenseID; 
            this.CreatedByUserID = CreatedByUserID; 
            this.ReleasedByUserID = ReleasedByUserID; 
            this.ReleaseApplicationID = ReleaseApplicationID; 
            this.FineFees = FineFees; 
            this.IsReleased = IsReleased; 
            this.DetainDate = DetainDate;
            this.ReleaseDate = ReleaseDate;

            _Mode = enDetainedMode.Update;
        }


        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.CreatedByUserID = -1;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
            this.FineFees = 0;
            this.IsReleased = false;
            this.DetainDate = DateTime.Now;
            this.ReleaseDate = DateTime.MaxValue;

            _Mode = enDetainedMode.AddNew;
        }

        public static clsDetainedLicense GetDetainedLicenseInfoByID(int DetainID)
        {
            
            int ReleasedByUserID = -1;
            int LicenseID = -1;
            int CreatedByUserID = -1;
            int ReleaseApplicationID = -1;
            float FineFees = -1;
            bool IsReleased = false;
            DateTime DetainDate = DateTime.Now;
            DateTime ReleaseDate = DateTime.Now;

            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID, ref LicenseID, ref ReleasedByUserID,
                                              ref ReleaseApplicationID, ref CreatedByUserID,
                                              ref ReleaseDate, ref DetainDate,
                                              ref FineFees, ref IsReleased);
            if (IsFound)
            {
                return new clsDetainedLicense(DetainID, LicenseID, CreatedByUserID,
                                              ReleasedByUserID, ReleaseApplicationID, FineFees,
                                              IsReleased, DetainDate, ReleaseDate);
            }
            else
            {
                return null; 
            }

        }

        public static clsDetainedLicense GetDetainedLicenseInfoByLicenseID(int LicenseID)
        {

            int ReleasedByUserID = -1;
            int DetainID = -1;
            int CreatedByUserID = -1;
            int ReleaseApplicationID = -1;
            float FineFees = -1;
            bool IsReleased = false;
            DateTime DetainDate = DateTime.Now;
            DateTime ReleaseDate = DateTime.Now;

            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(ref DetainID, LicenseID, ref ReleasedByUserID,
                                              ref ReleaseApplicationID, ref CreatedByUserID,
                                              ref ReleaseDate, ref DetainDate,
                                              ref FineFees, ref IsReleased);
            if (IsFound)
            {
                return new clsDetainedLicense(DetainID, LicenseID, CreatedByUserID,
                                              ReleasedByUserID, ReleaseApplicationID, FineFees,
                                              IsReleased, DetainDate, ReleaseDate);
            }
            else
            {
                return null;
            }

        }

        private bool _AddNewDetainLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate, 
                                                                         this.FineFees, this.CreatedByUserID);

            return this.DetainID != -1;
        }

        private bool _UpdateDetainLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.ReleasedByUserID,
                                                                this.ReleaseApplicationID, this.CreatedByUserID,
                                                                this.ReleaseDate, this.DetainDate,
                                                                this.FineFees, this.IsReleased);
        }

        public static bool Delete(int DetainID)
        {
            return clsDetainedLicenseData.DeleteDetainedLicense(DetainID);
        }

        public bool Delete()
        {
            return clsDetainedLicenseData.DeleteDetainedLicense(this.DetainID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public static bool IsDetainLicenseExistByID(int DetainID)
        {
            return clsDetainedLicenseData.IsDetainedLicenseExistByDetainID(DetainID);
        }

        public bool IsDetainLicenseExistByID()
        {
            return clsDetainedLicenseData.IsDetainedLicenseExistByDetainID(this.DetainID);
        }

        public static bool IsDetainedLicenseExistByLicenseID(int LicenseID)
        {
            return clsDetainedLicenseData.IsDetainedLicenseExistByLicenseID(LicenseID);
        }

        public bool IsDetainedLicenseExistByLicenseID()
        {
            return clsDetainedLicenseData.IsDetainedLicenseExistByLicenseID(this.LicenseID);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enDetainedMode.AddNew:
                    
                    if (_AddNewDetainLicense())
                    {
                        return true;
                    }    
                    else
                    {
                        return false;
                    }

                case enDetainedMode.Update:

                    return _UpdateDetainLicense();
            }

            return false;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        }

    }                             
}                                 
                                  