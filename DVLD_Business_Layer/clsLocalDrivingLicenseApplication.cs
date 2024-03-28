using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business_Layer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enLDLAppMode { AddNew = 0, Update = 1 };
        private enLDLAppMode _Mode = enLDLAppMode.AddNew;

        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }

        // Composition 
        public clsLicenseClass LicenseClassInfo { set; get; }

        public string PersonFullName
        {
            get => clsPerson.GetPersonInfoByPersonID(base.ApplicantPersonID).FullName;
        }

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID, 
                                                  int ApplicantPersonID, enApplicationType ApplicationTypeID, int CreatedByUserID, 
                                                  DateTime ApplicationDate, DateTime LastStatusDate, byte ApplicationStatus, float PaidFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationTypeID = ApplicationTypeID;
            base.CreatedByUserID = CreatedByUserID;
            base.ApplicationDate = ApplicationDate;
            base.LastStatusDate = LastStatusDate;
            base.ApplicationStatus = ApplicationStatus;
            base.PaidFees = PaidFees;

            LicenseClassInfo = clsLicenseClass.GetLicenseClassInfoByID(this.LicenseClassID);


            _Mode = enLDLAppMode.Update;
        }

        public clsLocalDrivingLicenseApplication()
        {
            this.ApplicationID = -1;
            this.LicenseClassID = -1;
            this.LocalDrivingLicenseApplicationID = -1;


            _Mode = enLDLAppMode.AddNew;
        }

        public static clsLocalDrivingLicenseApplication GetLDLAppInfoByLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            int LicenseClassID = -1; 
            int ApplicationID = -1;


            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLDLAppInfoByLDLAppID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                clsApplication Application = clsApplication.GetApplicationInfoByApplicationID(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID, 
                                                             Application.ApplicantPersonID, Application.ApplicationTypeID, 
                                                             Application.CreatedByUserID, Application.ApplicationDate, 
                                                             Application.LastStatusDate, Application.ApplicationStatus, 
                                                             Application.PaidFees);
            }
            else
            {
                return null;
            }
        }

        public static clsLocalDrivingLicenseApplication GetLDLAppInfoByApplicationID(int ApplicationID)
        {
            int LicenseClassID = -1;
            int LocalDrivingLicenseApplicationID= -1;


            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLDLAppInfoByApplicationID(ref LocalDrivingLicenseApplicationID, ApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                
                clsApplication Application = clsApplication.GetApplicationInfoByApplicationID(ApplicationID);

                //If LDLApp is found, then the Application must definitely be found, otherwise a Refrentail Integrety
                //error will be received in the database.

                if (Application != null)
                {

                    return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID,
                                                                 Application.ApplicantPersonID, Application.ApplicationTypeID,
                                                                 Application.CreatedByUserID, Application.ApplicationDate,
                                                                 Application.LastStatusDate, Application.ApplicationStatus,
                                                                 Application.PaidFees);
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        public static bool IsApplicationCancelled(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LocalDrivingLicenseApplicationID).ApplicationStatus == 3;
        }

        public bool IsApplicationCancelled()
        {
            return GetLDLAppInfoByLDLAppID(this.LocalDrivingLicenseApplicationID).ApplicationStatus == 3;
        }
        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, 
                                                                                                            this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID > -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID,
                                                                                              this.ApplicationID,
                                                                                              this.LicenseClassID);
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public static bool IsLocalDrivingLicenseApplicationExist(int LocalDrivingLicenseApplication)
        {
            return clsLocalDrivingLicenseApplicationData.IsLocalDrivingLicenseApplicationExistByID(LocalDrivingLicenseApplication);
        }

        public bool Save()
        {
            base._Mode = (enApplicationMode)this._Mode;

            if (!base.Save())
            {
                return false;
            }

            switch (_Mode)
            {
                case enLDLAppMode.AddNew:

                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        _Mode = enLDLAppMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enLDLAppMode.Update:

                    return _UpdateLocalDrivingLicenseApplication();
            }

            return false;
        }

        public static bool DoesPassedAllTest(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonPassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public bool DoesPassedAllTest()
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonPassedAllTests(this.LocalDrivingLicenseApplicationID);
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool DoesPassTestType(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        public bool DoesAttendTestType(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool IsThereAnActiveScheduleTest(int TestTypeID)
        {
            return clsTestAppointmentData.IsThereAnActiveScheduleTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool IsThereAnActiveScheduleTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.IsThereAnActiveScheduleTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static byte TotalTrialPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public byte TotalTrialPerTest(int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialPerTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool DoesPersonPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonPassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public bool DoesPersonPassedAllTests()
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonPassedAllTests(this.LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTestCount(LocalDrivingLicenseApplicationID); 
        }

        public byte GetPassedTestCount()
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public bool DoesPersonPassedAllTest()
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTestCount(this.LocalDrivingLicenseApplicationID) == 3;
        }

        public static bool DoesPersonHasComplatedApplication(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonHasComplatedApplication(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public static bool DoesPersonHasAnActiveApplication(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonHasAnActiveApplication(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public bool DoesPersonHasComplatedApplication()
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonHasComplatedApplication(base.ApplicantPersonID, 
                                                                                           (int)enApplicationType.NewLocalDrvingLicense, 
                                                                                           this.LicenseClassID);
        }

        public bool DoesPersonHasAnActiveApplication()
        {
            return clsLocalDrivingLicenseApplicationData.DoesPersonHasAnActiveApplication(base.ApplicantPersonID,
                                                                                          (int)enApplicationType.NewLocalDrvingLicense,
                                                                                          this.LicenseClassID);
        }

        public static bool IsThisTestRetakeTest(int LocalDrivingLicenseApplicationID, clsTestAppointment.enTestType TestTypeID)
        {
            return clsTestAppointment.IsThisTestRetakeTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool IsThisTestRetakeTest(clsTestAppointment.enTestType TestTypeID)
        {
            return clsTestAppointment.IsThisTestRetakeTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static bool DoesPassedPreviousTest(int LocalDrivingLicenseApplicationID, clsTestAppointment.enTestType TestTypeID)
        {
            switch (TestTypeID)
            {
                case clsTestAppointment.enTestType.VisionTest:
                    return true;

                case clsTestAppointment.enTestType.WrittenTest:
                    return DoesPassTestType(LocalDrivingLicenseApplicationID, (int)clsTestAppointment.enTestType.VisionTest);

                case clsTestAppointment.enTestType.StreetTest:
                    return DoesPassTestType(LocalDrivingLicenseApplicationID, (int)clsTestAppointment.enTestType.WrittenTest);
            }
            return false;
        }

        public bool DoesPassedPreviousTest(clsTestAppointment.enTestType TestTypeID)
        {
            switch (TestTypeID)
            {
                case clsTestAppointment.enTestType.VisionTest:
                    return true;

                case clsTestAppointment.enTestType.WrittenTest:
                    return DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)clsTestAppointment.enTestType.VisionTest);

                case clsTestAppointment.enTestType.StreetTest:
                    return DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)clsTestAppointment.enTestType.WrittenTest);
            }
            return false;
        }

        public int IssuseForFirstTime(string Notes, int CreatedByUserID)
        {
            clsDriver Driver = clsDriver.GetDriverInfoPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                Driver = new clsDriver();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                Driver.CreatedDate = DateTime.Now;

                if (!Driver.Save())
                {
                    return -1;
                }
            }
            
            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = this.ApplicationID;
            NewLicense.DriverID = Driver.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = (short)clsLicense.enIssueReason.FirstTime;

            if (!NewLicense.Save())
            {
                return -1;
            }

            this.SetComplate();

            return NewLicense.LicenseID;
        }

        public int GetAnActiveLicenseID()
        {
            return clsLicense.GetAnActiveLicenseID(this.ApplicantPersonID, this.LicenseClassID);
        }

        
    }
}
