using DVLD_DataAccess_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsPerson
    {
        public enum enPersonMode { AddNew = 0, Update = 1 };
        private enPersonMode _Mode;

        public int PersonID { set; get; }
        public byte Gender { set; get; }
        public int CountryID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string NationalNo { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Address { set; get; }
        private string _ImagePath;
        public string ImagePath
        {
            set { _ImagePath = value; }
            get { return _ImagePath; }
        }
        public DateTime DateOfBirth { set; get; }

        public string FullName
        {
            get => FirstName + " " + SecondName + ( ThirdName == "" ? "" : $" {ThirdName}" ) + " "+ LastName;
        }
        // Composition 
        public clsCountry CountryInfo { set; get; }

        private clsPerson(int PersonID, byte Gender, string NationalNo, int CountryID, string FirstName,
                          string SecondName, string ThirdName, string LastName, string Email,
                          string Phone, string Address, string ImagePath, DateTime DateOfBirth)
        {
            this.PersonID = PersonID;
            this.Gender = Gender;
            this.CountryID = CountryID;

            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.ImagePath = ImagePath;

            this.DateOfBirth = DateOfBirth;

            CountryInfo = clsCountry.GetCountryInfoByCountryID(CountryID);
            
            _Mode = enPersonMode.Update;
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.CountryID = -1;
            this.Gender = 0;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.NationalNo = "";
            this.Email = "";
            this.Phone = "";
            this.Address = "";
            this.ImagePath = "";
            this.DateOfBirth = DateTime.Now;

            _Mode = enPersonMode.AddNew;
        }

        public static clsPerson GetPersonInfoByPersonID(int PersonID)
        {
            int CountryID = -1;
            byte Gender = 0;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string NationalNo = "";
            string Email = "";
            string Phone = "";
            string Address = "";
            string ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;


            bool IsFound = clsPersonData.GetPersonInfoByPersonID(PersonID, ref Gender, ref NationalNo, ref CountryID, ref FirstName,
                                                                 ref SecondName, ref ThirdName, ref LastName, ref Email, ref Phone,
                                                                 ref Address, ref ImagePath, ref DateOfBirth);

            if (IsFound)
            {
                return new clsPerson(PersonID, Gender, NationalNo, CountryID, FirstName,
                                     SecondName, ThirdName, LastName, Email, Phone,
                                     Address, ImagePath, DateOfBirth);
            }

            else
            {
                return null;
            }
        }

        public static clsPerson GetPersonByNationalNo(string NationalNo)
        {
            int CountryID = -1;
            int PersonID = -1;
            byte Gender = 0;
            string FirstName = "";
            string SecondName = "";
            string ThirdName = "";
            string LastName = "";
            string Email = "";
            string Phone = "";
            string Address = "";
            string ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;


            bool IsFound = clsPersonData.GetPersonByNationalNo(ref PersonID, NationalNo, ref Gender, ref CountryID, ref FirstName,
                                                                 ref SecondName, ref ThirdName, ref LastName, ref Email, ref Phone,
                                                                 ref Address, ref ImagePath, ref DateOfBirth);

            if (IsFound)
            {
                return new clsPerson(PersonID, Gender, NationalNo, CountryID, FirstName,
                                     SecondName, ThirdName, LastName, Email, Phone,
                                     Address, ImagePath, DateOfBirth);
            }

            else
            {
                return null;
            }
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.Gender, this.NationalNo, this.CountryID, this.FirstName,
                                                       this.SecondName, this.ThirdName, this.LastName, this.Email,
                                                       this.Phone, this.Address, this.ImagePath, this.DateOfBirth);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.Gender, this.NationalNo, this.CountryID, this.FirstName,
                                              this.SecondName, this.ThirdName, this.LastName, this.Email,
                                              this.Phone, this.Address, this.ImagePath, this.DateOfBirth);
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);   
        }
        public static bool IsPersonExistByPersonID(int PersonID)
        {
            return clsPersonData.IsPersonExistByPersonID(PersonID);
        }
        public static bool IsPersonExistByNationalNo(string NationalNo)
        {
            return clsPersonData.IsPersonExistByNationalNo(NationalNo);
        }

        public bool DeletePerson()
        {
            return clsPersonData.DeletePerson(this.PersonID);
        }
        public bool IsPersonExistByPersonID()
        {
            return clsPersonData.IsPersonExistByPersonID(this.PersonID);
        }
        public bool IsPersonExistByNationalNo()
        {
            return clsPersonData.IsPersonExistByNationalNo(this.NationalNo);
        }
        public static DataTable GelAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enPersonMode.AddNew:

                    if (_AddNewPerson())
                    {
                        _Mode = enPersonMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                case enPersonMode.Update:

                    return _UpdatePerson();
            }

            return false;
        }
    }

}
