using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsTestAppointment
    {
        public enum enTestAppointmentMode { AddNew = 0, Update = 1 };
        private enTestAppointmentMode _Mode;

        public enum enTestType {VisionTest = 1, WrittenTest = 2, StreetTest = 3};
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int CreatedByUserID { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsLocked { get; set; }
        public float PaidFees { get; set; }

        public int TestID
        {
            get => GetTestID();
        }

        public clsTestType TestTypeInfo { get; set; }

        private clsTestAppointment(int TestAppointmentID, int TestTypeID,
                                   int LocalDrivingLicenseApplicationID, int CreatedByUserID,
                                   DateTime AppointmentDate, float PaidFees,
                                   bool IsLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.CreatedByUserID = CreatedByUserID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;

            TestTypeInfo = clsTestType.GetTestTypeInfoTestTypeID(this.TestTypeID);
            
            _Mode = enTestAppointmentMode.Update;
        }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = (int)enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.CreatedByUserID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;


            _Mode = enTestAppointmentMode.AddNew;
        }

        public int GetTestID()
        {
            return clsTestAppointmentData.GetTestID(this.TestAppointmentID);
        }

        public int GetTestID(int TestAppointmentID)
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }
        public static clsTestAppointment GetTestAppointmentInfoTestAppointmentID(int TestAppointmentID)
        {

            int TestTypeID = (int)enTestType.VisionTest;
            int LocalDrivingLicenseApplicationID = -1;
            int CreatedByUserID = -1;
            int RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;



            bool IsFound = clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID,
                                                                             ref LocalDrivingLicenseApplicationID, ref CreatedByUserID,
                                                                             ref AppointmentDate, ref PaidFees,
                                                                             ref IsLocked, ref RetakeTestApplicationID);

            if (IsFound)
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID,
                                              LocalDrivingLicenseApplicationID, CreatedByUserID,
                                              AppointmentDate, PaidFees,
                                              IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static clsTestAppointment GetTestAppointmentInfoByLDLAppIDAndTestType(int LocalDrivingLicenseApplicationID, clsTestAppointment.enTestType TestTypeID)
        {

            int TestAppointmentID = -1;
            int CreatedByUserID = -1;
            int RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;



            bool IsFound = clsTestAppointmentData.GetTestAppointmentInfoByLDLAppIDAndTestType(ref TestAppointmentID, (int)TestTypeID,
                                                                             LocalDrivingLicenseApplicationID, ref CreatedByUserID,
                                                                             ref AppointmentDate, ref PaidFees,
                                                                             ref IsLocked, ref RetakeTestApplicationID);

            if (IsFound)
            {
                return new clsTestAppointment(TestAppointmentID, (int)TestTypeID,
                                              LocalDrivingLicenseApplicationID, CreatedByUserID,
                                              AppointmentDate, PaidFees,
                                              IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static bool IsThisTestRetakeTest(int LocalDrivingLicenseApplicationID, clsTestAppointment.enTestType TestTypeID)
        {
            return clsTestAppointment.GetTestAppointmentInfoByLDLAppIDAndTestType(LocalDrivingLicenseApplicationID, TestTypeID).RetakeTestApplicationID != -1;
        }

        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment(this.TestTypeID,
                                                                                  this.LocalDrivingLicenseApplicationID, this.CreatedByUserID,
                                                                                  this.AppointmentDate, this.PaidFees,
                                                                                  this.IsLocked, this.RetakeTestApplicationID);
            return (this.TestAppointmentID > -1);
        }

        private bool _UpdateClassName()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID,
                                                                this.LocalDrivingLicenseApplicationID, this.CreatedByUserID,
                                                                this.AppointmentDate, this.PaidFees,
                                                                this.IsLocked, this.RetakeTestApplicationID);
        }

        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            return clsTestAppointmentData.DeleteTestAppointment(TestAppointmentID);
        }

        public bool DeleteTestAppointment()
        {
            return clsTestAppointmentData.DeleteTestAppointment(this.TestAppointmentID);
        }
        public static bool IsTestAppointmentExistByTestAppointmentID(int TestAppointmentID)
        {
            return clsTestAppointmentData.IsTestAppointmentExistByAppointmentID(TestAppointmentID);
        }

        public static bool IsThereAnActiveScheduleTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.IsThereAnActiveScheduleTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool IsThereAnActiveScheduleTest(int TestTypeID)
        {
            return clsTestAppointmentData.IsThereAnActiveScheduleTest(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public static DataTable GetTestAppointmentByTestTypeIDAndLDLApp(int LocalDrivingLicenseApplicationID, clsTestAppointment.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetTestAppointmentByTestTypeIDAndLDLApp(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enTestAppointmentMode.AddNew:

                    if (_AddNewTestAppointment())
                    {
                        _Mode = enTestAppointmentMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enTestAppointmentMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }
    }
}
