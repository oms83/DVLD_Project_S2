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
using static DVLD_Business_Layer.clsLicense;

namespace DVLD.Licenses.Detain_Licenses
{
    public partial class frmDetainLicenseApplication : Form
    {
        private int _LicenseID = -1;
        private clsLicense _License = null;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetData();
        }

        private void _ResetData()
        {
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblDetainDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblLicenseID.Text = "[????]";
            lblDetainID.Text = "[????]";
            txtFineFees.Text = "";

            btnDetain.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;  
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

            bool IsActive = _License.IsActive;
            if (!IsActive)
            {
                clsMessages.ErrorMassege("Selected License is Not Active, choose an active license.", "Not Allowed");

                btnDetain.Enabled = false;
                return;
            }


            bool IsExpired = _License.IsExpired();
            if (IsExpired)
            {
                clsMessages.ErrorMassege("Selected License is expiared, select another license " +
                                          clsFormat.ShortDateFormat(_License.ExpirationDate),
                                         "Not Allowed");

                btnDetain.Enabled = false;
                return;
            }

            bool IsDetained = _License.IsDetained;
            if (IsDetained)
            {
                clsMessages.ErrorMassege("Selected License is Detained already, choose another license.", "Not Allowed");

                btnDetain.Enabled = false;
                return;
            }

            btnDetain.Enabled = true;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Detain the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            float DetainFees = Convert.ToSingle(txtFineFees.Text);

            clsDetainedLicense DetainedLicense = _License.Detain(clsGlobal.CurrentUser.UserID, DetainFees);

            if (DetainedLicense == null)
            {
                clsMessages.ErrorMassege("Faild to Detain the License");

                return;
            }

            btnDetain.Enabled = false;
            lblDetainID.Text = _License.ApplicationID.ToString();

            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            txtFineFees.Enabled = false;

            clsMessages.SuccessMassege("License Detained Successfully with DetainID = " + DetainedLicense.DetainID.ToString(), "License Issued");

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

        private void frmDetainLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FocusFilter();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "this field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);
            }
                
        }
    }
}
