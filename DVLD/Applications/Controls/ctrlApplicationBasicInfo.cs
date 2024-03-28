using DVLD.Global;
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

namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private int _LDLAppID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        private void ctrlApplicationBasicInfo_Load(object sender, EventArgs e)
        {
            llViewPersonInfo.Enabled = _LDLApp != null;
        }

        public void ResetData()
        {
            lblApplicant.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblApplicationType.Text = "[????]";
            lblCreatedBy.Text = "[????]";
            lblDate.Text = "[DD/MM/YYYY]";
            lblFees.Text = "[$$$$]";
            lblStatus.Text = "[????]";
            lblStatusDate.Text = "[DD/MM/YYYY]";

            llViewPersonInfo.Enabled = _LDLApp != null;
        }

        private void _LoadData()
        {
            lblApplicant.Text = _LDLApp.ApplicantPersonID.ToString();
            lblApplicationID.Text = _LDLApp.ApplicationID.ToString();
            lblApplicationType.Text = _LDLApp.ApplicationTypeAsText;
            lblCreatedBy.Text = clsUser.GetUserInfoUserID(_LDLApp.CreatedByUserID).UserName;
            lblDate.Text = clsFormat.ShortDateFormat(_LDLApp.ApplicationDate);
            lblFees.Text = _LDLApp.PaidFees.ToString();
            lblStatus.Text = _LDLApp.StatusAsText;
            lblStatusDate.Text = clsFormat.ShortDateFormat(_LDLApp.LastStatusDate);

            llViewPersonInfo.Enabled = _LDLApp != null;
        }

        public void LoadLDLAppInfo(int LDLAppID)
        {
            _LDLAppID = LDLAppID;
            
            _LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);

            ResetData();
            
            if (_LDLApp == null)
            {
                MessageBox.Show($"No Local Driving License Application with ID = {LDLAppID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                return;
            }

            _LoadData();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_LDLApp.ApplicantPersonID);

            frm.ShowDialog();
        }
    }
}
