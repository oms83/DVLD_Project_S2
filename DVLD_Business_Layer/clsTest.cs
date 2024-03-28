using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsTest
    {
        public enum enTestMode { AddNew = 0, Update = 1 };
        private enTestMode _Mode;

        public int TestAppointmentID { get; set; }
        public int TestID { get; set; }
        public int CreatedByUserID { get; set; }
        public string Notes { get; set; }
        public bool TestResult { get; set; }

        public clsTestType TestTypeInfo { get; set; }

        private clsTest(int TestAppointmentID, int TestID,
                        int CreatedByUserID, string Notes,
                        bool TestResult)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestID = TestID;
            this.CreatedByUserID = CreatedByUserID;
            this.Notes = Notes;
            this.TestResult = TestResult;
            

            _Mode = enTestMode.Update;
        }

        public clsTest()
        {
            this.TestAppointmentID = -1;
            this.TestID = -1;
            this.CreatedByUserID = -1;
            this.Notes = "";
            this.TestResult = false;

            _Mode = enTestMode.AddNew;
        }

        public static clsTest GetTestInfoByTestAppointmentID(int TestAppointmentID)
        {

            int TestID = -1;
            int CreatedByUserID = -1;
            string Notes = "";
            bool TestResult = false;

            bool IsFound = clsTestData.GetTestInfoByTestAppointmentID(ref TestID, TestAppointmentID,
                                                                      ref TestResult, ref Notes, 
                                                                      ref CreatedByUserID);

            if (IsFound)
            {
                return new clsTest(TestAppointmentID, TestID,
                                   CreatedByUserID, Notes,
                                   TestResult);
            }
            else
            {
                return null;
            }
        }

        public static clsTest GetTestInfoByTestID(int TestID)
        {

            int TestAppointmentID = -1;
            int CreatedByUserID = -1;
            string Notes = "";
            bool TestResult = false;

            bool IsFound = clsTestData.GetTestInfoByTestID(TestID, ref TestAppointmentID,
                                                           ref TestResult, ref Notes,
                                                           ref CreatedByUserID);

            if (IsFound)
            {
                return new clsTest(TestAppointmentID, TestID,
                                   CreatedByUserID, Notes,
                                   TestResult);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static bool DeleteTest(int TestID)
        {
            return clsTestData.DeleteTest(TestID);
        }
        public bool DeleteTest()
        {
            return clsTestData.DeleteTest(this.TestID);
        }
        public static bool IsTestExistByTestID(int TestID)
        {
            return clsTestData.IsTestExistByTestID(TestID);
        }

        public bool IsTestExistByTestID()
        {
            return clsTestData.IsTestExistByTestID(this.TestID);
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enTestMode.AddNew:

                    if (_AddNewTest())
                    {
                        _Mode = enTestMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enTestMode.Update:

                    return _UpdateTest();
            }

            return false;
        }


    }
}
