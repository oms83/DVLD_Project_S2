using DVLD.Applications.Local_Driviing_Licenses;
using DVLD.Global;
using DVLD.Global_Classes;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Release_Detain_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private enum enMode { Filter, Display }
        private enMode _Mode;

        private int _LicenseID = -1;
        private clsLicense _License = null;
        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();

            _Mode = enMode.Filter;
        }

        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();

            _LicenseID = LicenseID;
            _Mode = enMode.Display;
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetData();

            if ( _Mode == enMode.Display )
            {
                _LoadData();
            }
        }

        private void _ResetDetainInfo()
        {
            lblDetainID.Text = "[????]";
            lblDetainDate.Text = "[????]";
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfoByID(
                                      (int)clsApplication.enApplicationType.ReleaseDetainedLicense).ApplicationFees.ToString();

            lblTotalFees.Text = lblApplicationFees.Text;
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFineFees.Text = "[$$$$]";
            lblApplicationID.Text = "[????]";
        }
        private void _ResetData()
        {
            _ResetDetainInfo();

            llShowLicenseHistory.Enabled = false;   
            llShowLicenseInfo.Enabled = false;
            btnRelease.Enabled = false; 
        }

        private void _LoadBiscData()
        {
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblDetainDate.Text = clsFormat.ShortDateFormat(_License.DetainedInfo.DetainDate);
            lblFineFees.Text = _License.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();
            lblDetainID.Text = _License.DetainedInfo.DetainID.ToString();
            lblApplicationID.Text = _License.ApplicationID.ToString();

        }
        private void _LoadData()
        {

            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseData(_LicenseID);

            _License = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;
            
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            btnRelease.Enabled = true;
            _LoadBiscData();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Release the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            bool IsReleased = _License.Release(clsGlobal.CurrentUser.UserID, Convert.ToSingle(lblTotalFees.Text));

            if (!IsReleased )
            {
                clsMessages.ErrorMassege("Faild to Detain the License");

                return;
            }


            btnRelease.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseData(_LicenseID);

            _License = clsLicense.GetLicenseInfoByID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseID);

            clsMessages.SuccessMassege("License Released Successfully with LicenseID = " + _License.LicenseID.ToString(), "License Issued");
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _LicenseID = LicenseID;

            if (LicenseID == -1)
            {
                _ResetData();

                lblLicenseID.Text = LicenseID.ToString();

                return;
            }

            _License = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;
            lblLicenseID.Text = _LicenseID.ToString();

            bool IsExpired = _License.IsExpired();
            if (IsExpired)
            {
                clsMessages.ErrorMassege("Selected License is expiared, select another license " +
                                          clsFormat.ShortDateFormat(_License.ExpirationDate),
                                         "Not Allowed");
                _ResetDetainInfo();
                btnRelease.Enabled = false;
                return;
            }

            bool IsDetained = _License.IsDetained;
            if (!IsDetained)
            {
                clsMessages.ErrorMassege("Selected License is not Detained , choose another license.", "Not Allowed");

                _ResetDetainInfo();
                btnRelease.Enabled = false;
                return;
            }

            _LoadBiscData();

            btnRelease.Enabled = true;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_License.LicenseID);

            frm.ShowDialog();
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

        private void frmReleaseDetainedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FocusFilter();
        }
    }
}
