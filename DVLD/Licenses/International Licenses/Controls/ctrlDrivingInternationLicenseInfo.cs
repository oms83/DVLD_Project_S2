using DVLD.Global;
using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.International_Licenses
{
    public partial class ctrlDrivingInternationLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicense = null;
        public ctrlDrivingInternationLicenseInfo()
        {
            InitializeComponent();
        }

        public void ResetInterLicenseInfo()
        {
            lblName.Text = "[????]";
            lblIntLicenseID.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalID.Text = "[????]";
            lblGender.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDriverID.Text = "[????]";

            lblIssueDate.Text = "[DD/MM/YYYY]";
            lblDateOfBirth.Text = "[DD/MM/YYYY]";
            lblExpirationDate.Text = clsFormat.ShortDateFormat(DateTime.Now.AddYears(2));

            pbGender.Image = Resources.Man_32;
            pbImage.ImageLocation = "";
            pbImage.Image = Resources.Male_512;
        }

        public void LoadInterLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;

            _InternationalLicense = clsInternationalLicense.GetInternationalLicenseInfoByID(InternationalLicenseID);

            if ( _InternationalLicense == null )
            {
                _InternationalLicenseID = -1;

                ResetInterLicenseInfo();

                clsMessages.ErrorMassege("No International License with ID = " +  InternationalLicenseID);

                return;
            }



            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblIntLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalID.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female";
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();

            lblIssueDate.Text = clsFormat.ShortDateFormat(_InternationalLicense.IssueDate);
            lblDateOfBirth.Text = clsFormat.ShortDateFormat(_InternationalLicense.DriverInfo.PersonInfo.DateOfBirth);
            lblExpirationDate.Text = clsFormat.ShortDateFormat(_InternationalLicense.ExpirationDate);

            pbGender.Image = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? Resources.Man_32 : Resources.Woman_32;

            if (File.Exists(_InternationalLicense.DriverInfo.PersonInfo.ImagePath))
            {
                pbImage.ImageLocation = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;
            }
            else
            {
                pbImage.ImageLocation = "";
                pbImage.Image = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? Resources.Male_512 : Resources.Female_512;
            }

        }
    }
}
