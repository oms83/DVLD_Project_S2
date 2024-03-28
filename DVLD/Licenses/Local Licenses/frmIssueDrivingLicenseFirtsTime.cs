using DVLD.Global_Classes;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses
{
    public partial class frmIssueDrivingLicenseFirtsTime : Form
    {
        private int _LDLAppID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;
        public frmIssueDrivingLicenseFirtsTime(int LDLAppID)
        {
            InitializeComponent();

            _LDLAppID = LDLAppID;
        }

        private void _ResetData()
        {
            ctrlDrivingLicenseApplicationInfo1.ResetData();
            txtNotes.Text = string.Empty;
        }
        private void _LoadData()
        {
            _LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(_LDLAppID);

            if (_LDLApp == null )
            {
                _ResetData();

                clsMessages.ErrorMassege("No Local Driving License Application With ID = " + _LDLAppID);

                return;
            }

            ctrlDrivingLicenseApplicationInfo1.LoadLDLAppInfo(_LDLAppID);
        }

        private void frmIssueDrivingLicenseFirtsTime_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to issue the license for this application with ID = " + _LDLAppID,
                "Confirm Issue",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            int LicenseID = _LDLApp.IssuseForFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                clsMessages.SuccessMassege("License Issued Successfully with License ID = " + LicenseID);
                return;
            }
            else
            {
                clsMessages.ErrorMassege("License Was not Issued ! ", "Faild");
            }
        }
    }
}
