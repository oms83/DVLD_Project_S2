using DVLD.Global;
using DVLD.Global_Classes;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Renew_Local_Licenses
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private int _LicenseID = -1;
        private clsLicense _License = null;

        private enum enMode { Filter, Display }
        private enMode _Mode;
        public frmRenewLocalDrivingLicenseApplication(int LicenseID = -1)
        {
            InitializeComponent();

            _LicenseID = LicenseID;
            
            if (_LicenseID == -1)
            {
                _Mode = enMode.Filter;
            }
            else
            {
                _Mode = enMode.Display;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            
            _ResetData();

            if (_Mode == enMode.Display)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseData(_LicenseID);
                ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            }
            else
            {
                _ResetData();
            }
        }

        private void _ResetData()
        {
            lblApplicationID.Text = "[????]";
            lblApplicationDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblIssueDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.Renew).ApplicationFees.ToString();
            lblLicenseFees.Text = "[$$$$]";

            txtNotes.Text = "";

            lblRenewedLicenseID.Text = "[????]";
            lblOldLicenseID.Text = "[????]";
            lblExpirationDate.Text = "[DD/MM/YYYY]";
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblTotalFees.Text = "[$$$$]";

            btnRenew.Enabled = false;
            txtNotes.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
        }
        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense RenewedLicense = _License.Renew(clsGlobal.CurrentUser.UserID, txtNotes.Text.Trim());

            if (RenewedLicense == null)
            {
                clsMessages.ErrorMassege("Faild to Renew the License");

                return;
            }
            
            lblApplicationID.Text = RenewedLicense.ApplicationID.ToString();
            lblRenewedLicenseID.Text = RenewedLicense.LicenseID.ToString();

            btnRenew.Enabled = false;
            txtNotes.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;

            _License = clsLicense.GetLicenseInfoByID(RenewedLicense.LicenseID);

            clsMessages.SuccessMassege("License Renewed Successfully with ID = " + RenewedLicense.LicenseID.ToString(), "License Issued");
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int LicenseID)
        {
            _LicenseID = LicenseID;

            // if License is not found
            if (LicenseID == -1)
            {
                _ResetData();
                lblOldLicenseID.Text = LicenseID.ToString();

                return;
            }

            _License = ctrlDriverLicenseInfoWithFilter1.SelectedLicense;

            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            lblExpirationDate.Text = clsFormat.ShortDateFormat(_License.ExpirationDate);
            lblOldLicenseID.Text = _LicenseID.ToString();
            lblLicenseFees.Text = _License.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblLicenseFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();

            bool IsActive = _License.IsActive;
            if (!IsActive)
            {
                clsMessages.ErrorMassege("Selected License is Not Active, choose an active license.", "Not Allowed");

                btnRenew.Enabled = false;
                txtNotes.Enabled = false;
                return;
            }
            

            bool IsExpired = _License.IsExpired();
            if (!IsExpired)
            {
                clsMessages.ErrorMassege("Selected License is not yet expiared, it will expire on: " + 
                                          clsFormat.ShortDateFormat(_License.ExpirationDate), 
                                         "Not Allowed");

                btnRenew.Enabled = false;
                txtNotes.Enabled = false;
                return;
            }

            bool IsDetained = _License.IsDetained;
            if (IsDetained)
            {
                clsMessages.ErrorMassege("Selected License is not Detained, choose an active license.", "Not Allowed");

                btnRenew.Enabled = false;
                txtNotes.Enabled = false;
                return;
            }

            btnRenew.Enabled = true;
            txtNotes.Enabled = true;
        }

        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FocusFilter();
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
    }
}
