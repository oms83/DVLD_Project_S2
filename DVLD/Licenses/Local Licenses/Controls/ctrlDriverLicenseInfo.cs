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

namespace DVLD
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private clsLicense _License;
        private int _LicenseID = -1;
        public clsLicense SelectedLicense
        {
            get => _License;
        }

        public int LicenseID
        {
            get => _LicenseID;
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void ResetData()
        {
            lblIsDetained.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblNotes.Text = "Not Notes";
            lblIssueReason.Text = "[????]";
            lblGender.Text = "[????]";
            lblNationalID.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblName.Text = "[????]";
            lblClass.Text = "[????]";
            lblExpirationDate.Text = "[DD/MM/YYYY]";
            lblIssueDate.Text = "[DD/MM/YYYY]";

            pbGender.Image = Resources.Man_32;
            pbImage.ImageLocation = "";
            pbImage.Image = Resources.Male_512;
            
            llEditPerson.Enabled = false;
        }

        private void _LoadDriverPicture()
        {
            pbGender.Image = _License.DriverInfo.PersonInfo.Gender == 0 ? Resources.Man_32 : Resources.Woman_32;

            if (File.Exists(_License.DriverInfo.PersonInfo.ImagePath))
            {
                pbImage.Load(_License.DriverInfo.PersonInfo.ImagePath);
            }
            else
            {
                pbImage.ImageLocation = "";
            }
        }
        private void _LoadData()
        {
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            lblDriverID.Text = _License.DriverID.ToString();
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblNotes.Text = _License.Notes == "" ? "Not Notes" : _License.Notes;
            lblIssueReason.Text = _License.IssueReasonText;
            lblGender.Text = _License.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female";
            lblNationalID.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblClass.Text = _License.LicenseClassInfo.ClassName;
            lblDateOfBirth.Text = clsFormat.ShortDateFormat(_License.DriverInfo.PersonInfo.DateOfBirth);
            lblExpirationDate.Text = clsFormat.ShortDateFormat(_License.ExpirationDate);
            lblIssueDate.Text = clsFormat.ShortDateFormat(_License.IssueDate);

            _LoadDriverPicture();
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID;

            _License = clsLicense.GetLicenseInfoByID(LicenseID);
            
            llEditPerson.Enabled = _License != null;

            if (_License == null )
            {
                ResetData();

                _LicenseID = -1;
                
                clsMessages.ErrorMassege("No license with licenseID = " +  LicenseID);

                return;
            }

            _LicenseID = LicenseID;

            _LoadData();
        }

        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNewEdit frm = new frmAddNewEdit(_License.DriverInfo.PersonID);

            frm.ShowDialog();

            _LoadData();
        }

        private void ctrlDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            ResetData();
        }
    }
}
