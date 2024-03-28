using DVLD.Global;
using DVLD.Global_Classes;
using DVLD.Licenses.International_Licenses;
using DVLD.Properties;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.International_Licenses
{
    public partial class frmNewInternationalLicenseApplications : Form
    {
        private int _InternationalLicenseID = -1;
        private int _LicenseID = -1;
        private clsInternationalLicense _InternationalLicense = null;
        private clsLicense _License = null;

        private enum enMode { Filter, Display }
        private enMode _Mode = enMode.Filter;
        public frmNewInternationalLicenseApplications()
        {
            InitializeComponent();
            _Mode = enMode.Filter;
        }

        public frmNewInternationalLicenseApplications(int LicenseID)
        {
            InitializeComponent();

            _LicenseID = LicenseID;
            _Mode = enMode.Display;
        }
        private void frmNewInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _ResetData();

            if (_Mode == enMode.Display)
            {
                _LoadData();
            }
        }
        private void _ResetData()
        {
            lblApplicationID.Text = "[????]";
            lblFees.Text = "[????]";
            lblInternationalLicenseID.Text = "[????]";
            lblLocalLicenseID.Text = "[????]";

            lblApplicationDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblIssueDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblExpirationDate.Text = clsFormat.ShortDateFormat(DateTime.Now.AddYears(2));
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFees.Text = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

            btnIssue.Enabled = false;
        }

        private void _LoadInternationalLicenseInfo()
        {
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.ShortDateFormat(_InternationalLicense.ApplicationInfo.ApplicationDate);
            lblIssueDate.Text = clsFormat.ShortDateFormat(_InternationalLicense.IssueDate);
            lblFees.Text = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();

            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblCreatedByUser.Text = _InternationalLicense.CreatedByUserID.ToString();
            lblExpirationDate.Text = clsFormat.ShortDateFormat(_InternationalLicense.ExpirationDate);
        }
        private void _LoadData()
        {
            _License = clsLicense.GetLicenseInfoByID(_LicenseID);

            if (_License == null)
            {

                _LicenseID = -1;

                _ResetData();

                clsMessages.ErrorMassege("No International License with ID = " + _LicenseID);

                return;
            }
            
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseData(_LicenseID);
            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(_LicenseID);

            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            btnIssue.Enabled = true;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _LicenseID = LicenseID;

            if (LicenseID == -1)
            {
                _ResetData();
                lblLocalLicenseID.Text = LicenseID.ToString();

                return;
            }

            _License = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            lblLocalLicenseID.Text = _LicenseID.ToString();

            bool IsDriverHasInterLicense = clsInternationalLicense.IsInternationalLicenseExistByDriverID(_License.DriverID);

            if (IsDriverHasInterLicense)
            {
                clsMessages.ErrorMassege("This diver already has an active international license", "Not Allowed");

                btnIssue.Enabled = false;
                return;
            }


            bool IsCreatedByOrdinaryLicenseClass = _License.LicenseClassInfo.LicenseClassID == (int)clsLicenseClass.enLicenseClasses.Class_3_OrdinaryDrivingLicense;

            if (!IsCreatedByOrdinaryLicenseClass)
            {
                clsMessages.ErrorMassege("Selected License is not created by Ordinary Driving License Class, choose another license.", "Not Allowed");

                btnIssue.Enabled = false;
                return;
            }

            bool IsActive = _License.IsActive;
            if (!IsActive)
            {
                clsMessages.ErrorMassege("Selected License is Not Active, choose an active license.", "Not Allowed");

                btnIssue.Enabled = false;
                return;
            }


            bool IsExpired = _License.IsExpired();
            if (IsExpired)
            {
                clsMessages.ErrorMassege("Selected License is expiared, select another license " +
                                          clsFormat.ShortDateFormat(_License.ExpirationDate),
                                         "Not Allowed");

                btnIssue.Enabled = false;
                return;
            }

            bool IsDetained = _License.IsDetained;
            if (IsDetained)
            {
                clsMessages.ErrorMassege("Selected License is Detained already, choose another license.", "Not Allowed");

                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue international license for this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            _InternationalLicense = _License.IssueInternationalLicense(clsGlobal.CurrentUser.UserID);

            if ( _InternationalLicense == null )
            {
                clsMessages.ErrorMassege("Internation License is not issued successfully");
            }

            btnIssue.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            clsMessages.SuccessMassege("International License  issued successfully with ID = " + _InternationalLicense.InternationalLicenseID.ToString(), "License Issued");

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (_InternationalLicense == null )
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(_License.LicenseID);

                frm.ShowDialog();
            }
            else
            {
                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicense.InternationalLicenseID);

                frm.ShowDialog();
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmShowPersonLicenseHistory(_License.DriverInfo.PersonID);

            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewInternationalLicenseApplications_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FocusFilter();
        }
    }
}
