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

namespace DVLD.Applications.Local_Driviing_Licenses.Controls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private int _LDLAppID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;
        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void ResetData()
        {
            lblDrivingLocalApplicationID.Text = "[????]";
            lblPassedTests.Text = "3/0";
            lblAppliedFor.Text = "[????]";

            llShowLicenseInfo.Enabled = false;
        }

        private void _LoadData()
        {
            lblDrivingLocalApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblPassedTests.Text = "3/"+ _LDLApp.GetPassedTestCount();
            lblAppliedFor.Text = _LDLApp.LicenseClassInfo.ClassName;

            ctrlApplicationBasicInfo2.LoadLDLAppInfo( _LDLAppID);

            llShowLicenseInfo.Enabled = clsLicense.IsLicenseExistByApplicationID(_LDLApp.ApplicationID);
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

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense Lincese = clsLicense.GetLicenseInfoByApplicationID(_LDLApp.ApplicationID);

            if (Lincese == null)
            {
                MessageBox.Show($"No License with Application with ID = {_LDLApp.ApplicationID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                return;
            }

            frmShowLicenseInfo frm = new frmShowLicenseInfo(Lincese.LicenseID);
            frm.ShowDialog();
        }
    }
}
