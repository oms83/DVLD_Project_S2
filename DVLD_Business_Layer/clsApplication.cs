using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsApplication
    {
        public enum enApplicationMode { AddNew = 0, Update = 1 };
        public enApplicationMode _Mode;

        public enum enApplicationStatus { New = 1, Cancelled = 2, Complated = 3 }

        public byte ApplicationStatus = (byte)enApplicationStatus.New;
        public enum enApplicationType
        {
            NewLocalDrvingLicense = 1, 
            Renew = 2,
            ReplacementForLost = 3,
            ReplacementForDamaged = 4,
            ReleaseDetainedLicense = 5,
            NewInternationalLicense = 6,
            RetakeTest = 7
        }
        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }

        public enApplicationType ApplicationTypeID = enApplicationType.NewLocalDrvingLicense;
        public int CreatedByUserID { set; get; }
        public DateTime LastStatusDate { set; get; }
        public DateTime ApplicationDate { set; get; }
        public float PaidFees { set; get; }

        public string ApplicationTypeAsText
        {
            get => _GetApplicationTypeAsText();
        }

        private string _GetApplicationTypeAsText()
        {
            switch (this.ApplicationTypeID)
            {
                case enApplicationType.NewLocalDrvingLicense:
                    return "Issue For First Time";

                case enApplicationType.Renew:
                    return "Renew";

                case enApplicationType.ReplacementForLost:
                    return "Replacement For Lost";

                case enApplicationType.ReplacementForDamaged:
                    return "Replacement For Damaged";

                case enApplicationType.ReleaseDetainedLicense:
                    return "Release Detained License";

                case enApplicationType.NewInternationalLicense:
                    return "Issue New International License";

                case enApplicationType.RetakeTest:
                    return "Retake Test";

                default:
                    return "Unknown";
            }

            //return "Unknown";
        }
        public string StatusAsText
        {
            get => _GetStatusAsText();
        }
        private string _GetStatusAsText()
        {
            switch(this.ApplicationStatus)
            {
                case (byte)enApplicationStatus.New:
                    return "New";
                case (byte)enApplicationStatus.Cancelled:
                    return "Cancelled";
                case (byte)enApplicationStatus.Complated:
                    return "Complated";
                default:
                    return "Unknown";
            }
        }
        // Composition 
        public clsPerson PersonInfo { set; get; }

        private clsApplication(int ApplicationID, int ApplicantPersonID, enApplicationType ApplicationTypeID,
                               int CreatedByUserID, DateTime ApplicationDate, DateTime LastStatusDate,
                               byte ApplicationStatus, float PaidFees)

        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationTypeID = ApplicationTypeID;

            this.CreatedByUserID = CreatedByUserID;
            this.ApplicationDate = ApplicationDate;
            this.LastStatusDate = LastStatusDate;
            this.ApplicationStatus = ApplicationStatus;
            this.PaidFees = PaidFees;

            PersonInfo = clsPerson.GetPersonInfoByPersonID(this.ApplicantPersonID);

            _Mode = enApplicationMode.Update;
        }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;

            this.ApplicationTypeID = enApplicationType.NewLocalDrvingLicense;
            this.ApplicationStatus = (byte)enApplicationStatus.New;

            this.CreatedByUserID = -1;
            this.ApplicationDate = DateTime.Now;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;

            _Mode = enApplicationMode.AddNew;
        }

        public static clsApplication GetApplicationInfoByApplicationID(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            int ApplicationTypeID = -1;
            int CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusDate = DateTime.Now;
            byte ApplicationStatus = (byte)enApplicationStatus.New;
            float PaidFees = 0;


            bool IsFound = clsApplicationData.GetApplicationInfoByApplicationID(ApplicationID, ref ApplicantPersonID, ref ApplicationTypeID,
                                                                                ref CreatedByUserID, ref ApplicationDate, ref LastStatusDate,
                                                                                ref ApplicationStatus, ref PaidFees);

            if (IsFound)
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, (enApplicationType)ApplicationTypeID,
                                          CreatedByUserID, ApplicationDate, LastStatusDate,
                                          ApplicationStatus, PaidFees);
            }
            else
            {
                return null;
            }
        }

        public static clsApplication GetApplicationInfoByPersonID(int ApplicantPersonID)
        {
            int ApplicationID = -1;
            int ApplicationTypeID = -1;
            int CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now;
            DateTime LastStatusDate = DateTime.Now;
            byte ApplicationStatus = (byte)enApplicationStatus.New;
            float PaidFees = 0;


            bool IsFound = clsApplicationData.GetApplicationInfoByApplicantPersonID(ref ApplicationID, ApplicantPersonID, ref ApplicationTypeID,
                                                                                ref CreatedByUserID, ref ApplicationDate, ref LastStatusDate,
                                                                                ref ApplicationStatus, ref PaidFees);

            if (IsFound)
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, (enApplicationType)ApplicationTypeID,
                                          CreatedByUserID, ApplicationDate, LastStatusDate,
                                          ApplicationStatus, PaidFees);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(ApplicantPersonID, (int)ApplicationTypeID,
                                                                      CreatedByUserID, ApplicationDate, LastStatusDate,
                                                                      (byte)ApplicationStatus, PaidFees);

            return (this.ApplicationID > -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(ApplicationID, ApplicantPersonID, (int)ApplicationTypeID,
                                                        CreatedByUserID, ApplicationDate, LastStatusDate,
                                                        (byte)ApplicationStatus, PaidFees);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }

        public bool DeleteApplication()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }
        public static DataTable GelAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }

        public static bool Cancel(int ApplicationID)
        {
            return clsApplicationData.ChangeStatus(ApplicationID, (byte)clsApplication.enApplicationStatus.Cancelled);
        }

        public bool Cancel()
        {
            return clsApplicationData.ChangeStatus(this.ApplicationID, (byte)clsApplication.enApplicationStatus.Cancelled);
        }

        public static bool SetComplated(int ApplicationID)
        {
            return clsApplicationData.ChangeStatus(ApplicationID, (byte)clsApplication.enApplicationStatus.Complated);
        }

        public  bool SetComplate()
        {
            return clsApplicationData.ChangeStatus(this.ApplicationID, (byte)clsApplication.enApplicationStatus.Complated);
        }

        public static bool IsApplicationExistByApplicantPersonID(int ApplicantPersonID)
        {
            return clsApplicationData.IsApplicationExistByApplicantPersonID(ApplicantPersonID);
        }

        public static bool IsApplicationExistByApplicationID(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExistByApplicationID(ApplicationID);
        }

        public bool IsApplicationExistByApplicantPersonID()
        {
            return clsApplicationData.IsApplicationExistByApplicantPersonID(this.ApplicantPersonID);
        }

        public bool IsApplicationExistByApplicationID()
        {
            return clsApplicationData.IsApplicationExistByApplicationID(this.ApplicantPersonID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enApplicationMode.AddNew:

                    if (_AddNewApplication())
                    {
                        _Mode = enApplicationMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enApplicationMode.Update:

                    return _UpdateApplication();
            }

            return false;
        }
    }
}
