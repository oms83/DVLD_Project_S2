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

namespace DVLD.Applications.Replace_Lost_Or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        
        private clsLicense.enIssueReason _IssueReason;

        private int _LicenseID = -1;
        private clsLicense _License = null;

        private float ReplacementForDamegedFees = 0;
        private float ReplacementForLostFees = 0;
        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetData();
        }
        private void _ResetData()
        {
            _IssueReason = clsLicense.enIssueReason.ReplacementForLost;

            ReplacementForLostFees = clsApplicationType.GetApplicationTypeInfoByID(
                                      (int)clsApplication.enApplicationType.ReplacementForLost).
                                      ApplicationFees;

            ReplacementForDamegedFees = clsApplicationType.GetApplicationTypeInfoByID(
                                      (int)clsApplication.enApplicationType.ReplacementForDamaged).
                                      ApplicationFees;

            rbLostLicense.Checked = true;
            lblApplicationID.Text = "[????]";
            lblApplicationDate.Text = clsFormat.ShortDateFormat(DateTime.Now);

            lblApplicationFees.Text = ReplacementForLostFees.ToString();

            lblRreplacedLicenseID.Text = "[????]";
            lblOldLicenseID.Text = "[????]";
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            btnIssueReplacement.Enabled = false;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            _IssueReason = rbDamagedLicense.Checked ? clsLicense.enIssueReason.ReplacementForDamaged :
                                                      clsLicense.enIssueReason.ReplacementForLost;

            lblApplicationFees.Text = ReplacementForDamegedFees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            _IssueReason = rbLostLicense.Checked ? clsLicense.enIssueReason.ReplacementForLost :
                                                      clsLicense.enIssueReason.ReplacementForDamaged;

            lblApplicationFees.Text = ReplacementForLostFees.ToString();
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

            lblOldLicenseID.Text = _LicenseID.ToString();

            bool IsActive = _License.IsActive;
            if (!IsActive)
            {
                clsMessages.ErrorMassege("Selected License is not Not Active, choose an active license.", "Not Allowed");

                btnIssueReplacement.Enabled = false;
                return;
            }


            bool IsExpired = _License.IsExpired();
            if (IsExpired)
            {
                clsMessages.ErrorMassege("Selected License is expiared, select another license " +
                                          clsFormat.ShortDateFormat(_License.ExpirationDate),
                                         "Not Allowed");

                btnIssueReplacement.Enabled = false;
                return;
            }

            bool IsDetained = _License.IsDetained;
            if (IsDetained)
            {
                clsMessages.ErrorMassege("Selected License is not Detained, choose an active license.", "Not Allowed");

                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
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

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Replace the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            float ApplicationFees = Convert.ToSingle(lblApplicationFees.Text);

            clsLicense ReplacedLicense = _License.ReplaceForDamegedOrLost(clsGlobal.CurrentUser.UserID, ApplicationFees, _IssueReason);

            if (ReplacedLicense == null)
            {
                clsMessages.ErrorMassege("Faild to Replace the License");

                return;
            }


            ctrlDriverLicenseInfoWithFilter1.LoadLicenseData(_License.LicenseID);

            _License = clsLicense.GetLicenseInfoByID(ReplacedLicense.LicenseID);

            btnIssueReplacement.Enabled = false;
            lblApplicationID.Text = _License.ApplicationID.ToString();
            lblRreplacedLicenseID.Text = _License.LicenseID.ToString();
            ctrlDriverLicenseInfoWithFilter1.gbEnabled = false;
            gbReplacement.Enabled = false;

            clsMessages.SuccessMassege("License Replaced Successfully with ID = " + ReplacedLicense.LicenseID.ToString(), "License Issued");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FocusFilter();
        }
    }
}
