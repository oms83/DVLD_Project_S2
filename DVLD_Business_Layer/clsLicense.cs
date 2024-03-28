using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Business_Layer.clsApplication;
using static DVLD_Business_Layer.clsLicense;

namespace DVLD_Business_Layer
{
    public class clsLicense
    {
        public enum enLicenseMode { AddNew = 1,  Update=2 }

        public enLicenseMode _Mode = enLicenseMode.AddNew;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public short IssueReason { get; set; }

        public clsDriver DriverInfo {  get; set; }
        public clsLicenseClass LicenseClassInfo {  get; set; }

        public clsDetainedLicense DetainedInfo { get; set; }
        public enum enIssueReason
        {
            FirstTime = 1,
            Renew = 2,
            ReplacementForDamaged = 3,
            ReplacementForLost = 4
        }

        public string IssueReasonText
        {
            get => _GetIssueReason();
        }

        private string _GetIssueReason()
        {
            switch (this.IssueReason)
            {
                case (short)enIssueReason.FirstTime:
                    return "First Time";

                case (short)enIssueReason.Renew:
                    return "Renew";

                case (short)enIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";

                case (short)enIssueReason.ReplacementForLost:
                    return "Repalcement For Lost";

                default:
                    return "Unkown";
            }
        }

        public bool IsDetained
        {
            get => IsDetain();
        }
        private clsLicense( int LicenseID, int ApplicationID,
                            int DriverID, int LicenseClassID,
                            DateTime IssueDate, DateTime ExpirationDate,
                            string Notes, float PaidFees,
                            bool IsActive, short IssueReason, int CreatedByUserID
                          )
        {
            this.LicenseClassID = LicenseClassID;
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.CreatedByUserID = CreatedByUserID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;

            this.DriverInfo = clsDriver.GetDriverInfoDriverID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.GetLicenseClassInfoByID(this.LicenseClassID);
            this.DetainedInfo = clsDetainedLicense.GetDetainedLicenseInfoByLicenseID(this.LicenseID);

            _Mode = enLicenseMode.Update;
        }

        public clsLicense()
        {
            this.LicenseClassID = -1;
            this.LicenseID = -1;
            this.CreatedByUserID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = (short)enIssueReason.FirstTime;

            _Mode = enLicenseMode.AddNew;
        }

        public bool IsDetain()
        {
            clsDetainedLicense _DetainedInfo = clsDetainedLicense.GetDetainedLicenseInfoByLicenseID(this.LicenseID);

            if (_DetainedInfo == null)
            {
                return false;
            }
            else
            {
                this.DetainedInfo = _DetainedInfo;

                return !_DetainedInfo.IsReleased;
            }
        }
        public static clsLicense GetLicenseInfoByID(int LicenseID)
        {
            int ApplicationID = -1;
            int CreatedByUserID = -1;
            int DriverID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            short IssueReason = (short)enIssueReason.FirstTime;



            bool IsFound = clsLicenseData.GetLicenseInfoByID(LicenseID, ref LicenseClassID, ref ApplicationID,
                                                             ref DriverID, ref CreatedByUserID,
                                                             ref IssueDate, ref ExpirationDate,
                                                             ref Notes, ref PaidFees, ref IsActive, ref IssueReason);

            if (IsFound)
            {
                return new clsLicense(LicenseID, ApplicationID,
                                      DriverID, LicenseClassID,
                                      IssueDate, ExpirationDate,
                                      Notes, PaidFees,
                                      IsActive, IssueReason, CreatedByUserID
                            );
            }
            else
            {
                return null;
            }
        }

        public static clsLicense GetOrdinaryDrivingLincese(int DriverID)
        {
            int ApplicationID = -1;
            int CreatedByUserID = -1;
            int LicenseID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            short IssueReason = (short)enIssueReason.FirstTime;



            bool IsFound = clsLicenseData.GetOrdinaryDrivingLincese(ref LicenseID, ref LicenseClassID, ref ApplicationID,
                                                             DriverID, ref CreatedByUserID,
                                                             ref IssueDate, ref ExpirationDate,
                                                             ref Notes, ref PaidFees, ref IsActive, ref IssueReason);

            if (IsFound)
            {
                return new clsLicense(LicenseID, ApplicationID,
                                      DriverID, LicenseClassID,
                                      IssueDate, ExpirationDate,
                                      Notes, PaidFees,
                                      IsActive, IssueReason, CreatedByUserID
                            );
            }
            else
            {
                return null;
            }
        }
        public static clsLicense GetLicenseInfoByDriverID(int DriverID, int LicenseClassID)
        {
            int ApplicationID = -1;
            int CreatedByUserID = -1;
            int LicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            short IssueReason = (short)enIssueReason.FirstTime;



            bool IsFound = clsLicenseData.GetLicenseInfoByDriverID(ref LicenseID, LicenseClassID, ref ApplicationID,
                                                             DriverID, ref CreatedByUserID,
                                                             ref IssueDate, ref ExpirationDate,
                                                             ref Notes, ref PaidFees, ref IsActive, ref IssueReason);

            if (IsFound)
            {
                return new clsLicense(LicenseID, ApplicationID,
                                      DriverID, LicenseClassID,
                                      IssueDate, ExpirationDate,
                                      Notes, PaidFees,
                                      IsActive, IssueReason, CreatedByUserID
                            );
            }
            else
            {
                return null;
            }
        }

        public static clsLicense GetLicenseInfoByApplicationID(int ApplicationID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            int LicenseID = -1;
            int LicenseClassID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            short IssueReason = (short)enIssueReason.FirstTime;



            bool IsFound = clsLicenseData.GetLicenseInfoByApplicationID(ref LicenseID, ref LicenseClassID, ApplicationID,
                                                             ref DriverID, ref CreatedByUserID,
                                                             ref IssueDate, ref ExpirationDate,
                                                             ref Notes, ref PaidFees, ref IsActive, ref IssueReason);

            if (IsFound)
            {
                return new clsLicense(LicenseID, ApplicationID,
                                      DriverID, LicenseClassID,
                                      IssueDate, ExpirationDate,
                                      Notes, PaidFees,
                                      IsActive, IssueReason, CreatedByUserID
                            );
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.LicenseClassID, this.ApplicationID,
                                        this.DriverID, this.CreatedByUserID, this.IssueDate, this.ExpirationDate,
                                        this.Notes, this.PaidFees, this.IsActive, this.IssueReason);
            return (this.LicenseID != -1);
        }

        private bool _UpdateClassName()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.LicenseClassID, this.ApplicationID,
                                        this.DriverID, this.CreatedByUserID, this.IssueDate, this.ExpirationDate,
                                        this.Notes, this.PaidFees, this.IsActive, this.IssueReason);
        }

        public static bool DeleteLicense(int LicenseID)
        {
            return clsLicenseData.DeleteLicense(LicenseID);
        }

        public static bool IsLicenseExistByLicenseID(int LicenseID)
        {
            return clsLicenseData.IsLicenseExistByLicenseID(LicenseID);
        }

        public static bool IsLicenseExistByLicenseID(int DriverID, int LicenseClassID)
        {
            return clsLicenseData.IsLicenseExistByDriverIDAndLicenseClassID(DriverID, LicenseClassID);
        }

        public static bool IsLicenseExistByApplicationID(int ApplicationID)
        {
            return clsLicenseData.IsLicenseExistByApplicationID(ApplicationID);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public DataTable GetDriverLicenses()
        {
            return clsLicenseData.GetDriverLicenses(this.DriverID);
        }

        public static bool Deactivate(int LicenseID)
        {
            return clsLicenseData.DeactiveteLicense(LicenseID);
        }

        public static bool Activete(int LicenseID)
        {
            return clsLicenseData.ActiveteLicense(LicenseID);
        }

        public bool Activete()
        {
            return clsLicenseData.ActiveteLicense(this.LicenseID);
        }

        public bool Deactivate()
        {
            return clsLicenseData.DeactiveteLicense(this.LicenseID);
        }
        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.IsLicenseExistByPersonID(PersonID, LicenseClassID);
        }

        public bool IsLicenseExistByPersonID(int PersonID)
        {
            return clsLicenseData.IsLicenseExistByPersonID(PersonID, this.LicenseClassID);
        }
        public static int GetAnActiveLicenseID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetAnActiveLicenseByPersonID(PersonID, LicenseClassID);
        }
        public bool IsExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enLicenseMode.AddNew:

                    if (_AddNewLicense())
                    {
                        _Mode = enLicenseMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enLicenseMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }

        public clsLicense Renew(int CreatedByUserID, string Notes)
        {

            clsApplication NewApplication = new clsApplication();

            //NewApplication.ApplicationID = ApplicationID;
            NewApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            NewApplication.ApplicationTypeID = clsApplication.enApplicationType.Renew;
            NewApplication.CreatedByUserID = CreatedByUserID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.ApplicationStatus = (byte)clsApplication.enApplicationStatus.Complated;
            NewApplication.PaidFees = PaidFees;

            if (!NewApplication.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)enApplicationType.Renew).ApplicationFees + this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = (short)clsLicense.enIssueReason.Renew;

            if (!NewLicense.Save())
            {
                return null;
            }
            
            this.Deactivate();

            return NewLicense;
        }

        public clsLicense ReplaceForDamegedOrLost(int CreatedByUserID, float PaidFees, enIssueReason ReplacementReason)
        {

            clsApplication NewApplication = new clsApplication();

            //NewApplication.ApplicationID = ApplicationID;
            NewApplication.ApplicantPersonID = this.DriverInfo.PersonID;

            NewApplication.ApplicationTypeID = ReplacementReason == enIssueReason.ReplacementForLost ? 
                                                                    enApplicationType.ReplacementForLost : 
                                                                    enApplicationType.NewInternationalLicense;
            NewApplication.CreatedByUserID = CreatedByUserID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.ApplicationStatus = (byte)clsApplication.enApplicationStatus.Complated;
            NewApplication.PaidFees = PaidFees;

            if (!NewApplication.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)enApplicationType.Renew).ApplicationFees + this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;

            NewLicense.IssueReason = ReplacementReason == enIssueReason.ReplacementForLost ? 
                                                          (short)enIssueReason.ReplacementForLost:
                                                          (short)enIssueReason.ReplacementForDamaged;

            if (!NewLicense.Save())
            {
                return null;
            }

            this.Deactivate();

            return NewLicense;
        }

        public clsDetainedLicense Detain(int CreatedByUserID, float FineFees)
        {
            clsDetainedLicense NewDetainedLicense = new clsDetainedLicense();

            NewDetainedLicense.LicenseID = this.LicenseID;
            NewDetainedLicense.CreatedByUserID = CreatedByUserID;
            NewDetainedLicense.IsReleased = false;
            NewDetainedLicense.FineFees = Convert.ToSingle(FineFees);
            NewDetainedLicense.DetainDate = DateTime.Now;

            if (!NewDetainedLicense.Save())
            {
                return null;
            }

            this.Deactivate();

            return NewDetainedLicense;
        }

        public bool Release(int CreatedByUserID, float PaidFees)
        {
            
            clsApplication NewApplication = new clsApplication();
            NewApplication.ApplicantPersonID = this.DriverInfo.PersonID;

            NewApplication.ApplicationTypeID = enApplicationType.ReleaseDetainedLicense;
            NewApplication.CreatedByUserID = CreatedByUserID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.ApplicationStatus = (byte)clsApplication.enApplicationStatus.Complated;
            NewApplication.PaidFees = PaidFees;

            if (!NewApplication.Save())
            {
                return false;
            }

            if (this.DetainedInfo.ReleaseDetainedLicense(CreatedByUserID, NewApplication.ApplicationID))
            {
                this.Activete();
                return true;
            }

            return false;
        }

        public clsInternationalLicense IssueInternationalLicense(int CreatedByUserID)
        {

            clsApplication NewApplication = new clsApplication();
            NewApplication.ApplicantPersonID = this.DriverInfo.PersonID;

            NewApplication.ApplicationTypeID = enApplicationType.NewInternationalLicense;
            NewApplication.CreatedByUserID = CreatedByUserID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.LastStatusDate = DateTime.Now;
            NewApplication.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            NewApplication.ApplicationStatus = (byte)clsApplication.enApplicationStatus.Complated;

            if (!NewApplication.Save())
            {
                return null;
            }

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.CreatedByUserID = CreatedByUserID;
            InternationalLicense.ApplicationID = NewApplication.ApplicationID;
            InternationalLicense.DriverID = this.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = this.LicenseID;
            InternationalLicense.IsActive = true;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(2);
            

            if (InternationalLicense.Save())
            {
                return InternationalLicense;
            }

            return null;
        }
    }

    
}
