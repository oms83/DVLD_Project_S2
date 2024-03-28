using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsLicenseClass
    {
        public enum enLicenseClassMode { AddNew = 0, Update = 1 };
        private enLicenseClassMode _Mode;

        public enum enLicenseClasses
        {
            Class_1_SmallMotorcycle = 1,
            Class_2_HeavyMotorcycleLicense,
            Class_3_OrdinaryDrivingLicense,
            Class_4_Commercial,
            Class_5_Agricultural,
            Class_6_SmallAndMediumBus,
            Class_7_TruckAndHeavyVehicle
        }
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }
        private clsLicenseClass(int LicenseClassID, string ClassName,
                                string ClassDescription, byte MinimumAllowedAge,
                                byte DefaultValidityLength, float ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.DefaultValidityLength = DefaultValidityLength;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.ClassFees = ClassFees;

            _Mode = enLicenseClassMode.Update;
        }

        public clsLicenseClass()
        {
            this.LicenseClassID = 3;
            this.ClassName = "";
            this.ClassDescription = string.Empty;
            this.DefaultValidityLength = 10;
            this.MinimumAllowedAge = 18;
            this.ClassFees = 0;

            _Mode = enLicenseClassMode.AddNew;
        }

        public static clsLicenseClass GetLicenseClassInfoByID(int LicenseClassID)
        {
            string ClassName = "";
            string ClassDescription = string.Empty;
            byte DefaultValidityLength = 10;
            byte MinimumAllowedAge = 18;
            float ClassFees = 0;



            bool IsFound = clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName,
                                                                       ref ClassDescription, ref MinimumAllowedAge,
                                                                       ref DefaultValidityLength, ref ClassFees);

            if (IsFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName,
                                           ClassDescription, MinimumAllowedAge,
                                           DefaultValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }

        public static clsLicenseClass GetLicenseClassInfoByClassName(string ClassName)
        {
            int LicenseClassID = 3;
            string ClassDescription = string.Empty;
            byte DefaultValidityLength = 10;
            byte MinimumAllowedAge = 18;
            float ClassFees = 0;



            bool IsFound = clsLicenseClassData.GetLicenseClassInfoByClassName(ref LicenseClassID, ClassName,
                                                                       ref ClassDescription, ref MinimumAllowedAge,
                                                                       ref DefaultValidityLength, ref ClassFees);

            if (IsFound)
            {
                return new clsLicenseClass(LicenseClassID, ClassName,
                                           ClassDescription, MinimumAllowedAge,
                                           DefaultValidityLength, ClassFees);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName,
                                                                         this.ClassDescription, this.MinimumAllowedAge,
                                                                         this.DefaultValidityLength, this.ClassFees);
            return (this.LicenseClassID > -1);
        }

        private bool _UpdateClassName()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName,
                                                          this.ClassDescription, this.MinimumAllowedAge,
                                                          this.DefaultValidityLength, this.ClassFees);
        }

        public static bool DeleteLicenseClass(int LicenseClassID)
        {
            return clsLicenseClassData.DeleteLicenseClass(LicenseClassID);
        }

        public static bool IsLicenseClassExistByLicenseClassID(int LicenseClassID)
        {
            return clsLicenseClassData.IsLicenseClassExistByLicenseClassID(LicenseClassID);
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enLicenseClassMode.AddNew:

                    if (_AddNewLicenseClass())
                    {
                        _Mode = enLicenseClassMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enLicenseClassMode.Update:

                    return _UpdateClassName();
            }

            return false;
        }
    }
}
