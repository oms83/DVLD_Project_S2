using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsTestType
    {
        public enum enTestTypeMode { AddNew = 0, Update = 1 };
        private enTestTypeMode _Mode;

        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }

        private clsTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;

            _Mode = enTestTypeMode.Update;
        }

        public clsTestType()
        {
            this.TestTypeID = -1;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.TestTypeFees = 0;


            _Mode = enTestTypeMode.AddNew;
        }

        public static clsTestType GetTestTypeInfoTestTypeID(int TestTypeID)
        {

            string TestTypeTitle = "";
            string TestTypeDescription = "";
            float TestTypeFees = 0;



            bool IsFound = clsTestTypeData.GetTestTypeInfoByTestTypeID(TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees);

            if (IsFound)
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewTestType()
        {
            this.TestTypeID = clsTestTypeData.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
            return (this.TestTypeID > -1);
        }

        private bool _UpdateClassName()
        {
            return clsTestTypeData.UpdateTestType(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

        public static bool DeleteTestType(int TestTypeID)
        {
            return clsTestTypeData.DeleteTestType(TestTypeID);
        }

        public static bool IsTestTypeExistByTestTypeID(int TestTypeID)
        {
            return clsTestTypeData.IsTestTypeExistByTestTypeID(TestTypeID);
        }

        public  bool DeleteTestType()
        {
            return clsTestTypeData.DeleteTestType(this.TestTypeID);
        }

        public bool IsTestTypeExistByTestTypeID()
        {
            return clsTestTypeData.IsTestTypeExistByTestTypeID(this.TestTypeID);
        }
        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enTestTypeMode.AddNew:

                    if (_AddNewTestType())
                    {
                        _Mode = enTestTypeMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enTestTypeMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }
    }
}
