using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsCountry
    {
        public enum enCountryMode { AddNew = 0, Update = 1 };
        private enCountryMode _Mode;

        public int CountryID { set; get; }
        public string CountryName { set; get; }

        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
            
            _Mode = enCountryMode.Update;
        }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";

            _Mode = enCountryMode.AddNew;
        }

        public static clsCountry GetCountryInfoByCountryID(int CountryID)
        {
            
            string CountryName = "";

            bool IsFound = clsCountryData.GetCountryInfoByCountryID(CountryID, ref CountryName);

            if (IsFound)
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry GetCountryInfoByCountryName(string CountryName)
        {

            int CountryID = -1;



            bool IsFound = clsCountryData.GetCountryInfoByCountryName(ref CountryID, CountryName);

            if (IsFound)
            {
                return new clsCountry(CountryID, CountryName);
            }

            else
            {
                return null;
            }
        }

        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryData.AddNewCountry(this.CountryName);

            return (this.CountryID > -1);
        }

        private bool _UpdateCountry()
        {
            return clsCountryData.UpdateCountry(this.CountryID, this.CountryName);
        }

        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryData.DeleteCountry(CountryID);
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enCountryMode.AddNew:

                    if (_AddNewCountry())
                    {
                        _Mode = enCountryMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enCountryMode.Update:

                    return _UpdateCountry();
            }

            return false;
        }
    }
}
