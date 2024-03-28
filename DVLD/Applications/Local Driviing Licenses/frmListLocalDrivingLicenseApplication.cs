using DVLD.Global_Classes;
using DVLD.Licenses.Local_Licenses;
using DVLD.Tests;
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

namespace DVLD.Applications.Local_Driviing_Licenses
{
    public partial class frmListLocalDrivingLicenseApplication : Form
    {
        private DataTable _dtLDLApp;
        public frmListLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _RefreshData()
        {
            _dtLDLApp = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvListLocalDrivingApplications.DataSource = _dtLDLApp;

            lblNumberOfRecord.Text = dgvListLocalDrivingApplications.Rows.Count.ToString();
        }

        private void _SetDGVColumns()
        {
            cmbFilterBy.SelectedIndex = 0;

            if (dgvListLocalDrivingApplications.Rows.Count > 0)
            {
                dgvListLocalDrivingApplications.Columns[0].HeaderText = "LDLAppID";
                dgvListLocalDrivingApplications.Columns[0].Width = 140;

                dgvListLocalDrivingApplications.Columns[1].HeaderText = "Driving Class";
                dgvListLocalDrivingApplications.Columns[1].Width = 300;

                dgvListLocalDrivingApplications.Columns[2].HeaderText = "National No.";
                dgvListLocalDrivingApplications.Columns[2].Width = 120;

                dgvListLocalDrivingApplications.Columns[3].HeaderText = "Full Name";
                dgvListLocalDrivingApplications.Columns[3].Width = 280;

                dgvListLocalDrivingApplications.Columns[4].HeaderText = "Application Date";
                dgvListLocalDrivingApplications.Columns[4].Width = 200;

                dgvListLocalDrivingApplications.Columns[5].HeaderText = "Passed Tests";
                dgvListLocalDrivingApplications.Columns[5].Width = 140;

                dgvListLocalDrivingApplications.Columns[6].HeaderText = "Status";
                dgvListLocalDrivingApplications.Columns[6].Width = 140;
            }
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.SelectedIndex == 0)
            {
                txtFilterBy.Visible = false; 
            }
            else
            {
                txtFilterBy.Visible = true;
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilteringString = "";

            /*
                None
                L.D.L.AppID
                National No.
                Full Name
                Status 
            */

            switch (cmbFilterBy.Text)
            {

                case "L.D.L.AppID":
                    FilteringString = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilteringString = "NationalNo";
                    break;


                case "Full Name":
                    FilteringString = "FullName";
                    break;

                case "Status":
                    FilteringString = "Status";
                    break;


                default:
                    FilteringString = "None";
                    break;
            }

            if (txtFilterBy.Text.Trim() == "" || FilteringString == "None")
            {
                _dtLDLApp.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvListLocalDrivingApplications.Rows.Count.ToString();
                return;
            }
            if (FilteringString == "LocalDrivingLicenseApplicationID")
            {
                _dtLDLApp.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilteringString, txtFilterBy.Text.Trim());
            }
            else
            {
                _dtLDLApp.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilteringString, txtFilterBy.Text.Trim());
            }

            lblNumberOfRecord.Text = dgvListLocalDrivingApplications.Rows.Count.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text == "L.D.L.AppID")
            {
                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmApplicationEdit_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication(LDLAppID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsnCancelApplication_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);

            if (MessageBox.Show($"Are you sure you want to cancel this LDLApp with ID = {LDLAppID}",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (clsLocalDrivingLicenseApplication.Cancel(LDLApp.ApplicationID))
            {
                _RefreshData();

                MessageBox.Show($"Local Driving License Application with ID = {LDLAppID} cancelled successfully",
                "Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show($"Local Driving License Application with ID = {LDLAppID} is not cancelled successfully",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void tsmApplicationDelete_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;


            if (MessageBox.Show($"Are you sure you want to delete this LDLApp with ID = {LDLAppID}",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (clsLocalDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication(LDLAppID))
            {
                _RefreshData();

                MessageBox.Show($"Local Driving License Application with ID = {LDLAppID} deleted successfully",
                "Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show($"Local Driving License Application with ID = {LDLAppID} is not deleted, Because has data linked to is.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void tsmShowApplicationDetails_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmLocalDrivingLicenseApplication frm = new frmLocalDrivingLicenseApplication(LDLAppID);

            frm.ShowDialog();

        }

        private void tsmVision_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmListTestAppointments frmListTestAppointments = new frmListTestAppointments(LDLAppID, clsTestAppointment.enTestType.VisionTest);
            
            frmListTestAppointments.ShowDialog();

            _RefreshData();
        }

        private void tsmWritten_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmListTestAppointments frmListTestAppointments = new frmListTestAppointments(LDLAppID, clsTestAppointment.enTestType.WrittenTest);

            frmListTestAppointments.ShowDialog();

            _RefreshData();
        }

        private void tsmStreet_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmListTestAppointments frmListTestAppointments = new frmListTestAppointments(LDLAppID, clsTestAppointment.enTestType.StreetTest);

            frmListTestAppointments.ShowDialog();

            _RefreshData();
        }

        private void _InitailizContextMenuStrip()
        {
            tsmShowApplicationDetails.Enabled = false;
            
            tsmApplicationDelete.Enabled = false;
            tsmApplicationEdit.Enabled = false;
            tsnCancelApplication.Enabled = false;

            tsmSechduleTest.Enabled = false;
            tsmIssueDrivingLicenseFirstTime.Enabled = false;
            
            tsmShowLicense.Enabled = false;
            tsmShowPersonLicenseHistory.Enabled = false;
        }
        private void _CanceledStatus()
        {
            tsmShowApplicationDetails.Enabled = true;
            tsmShowPersonLicenseHistory.Enabled = true;
        }

        private void _NewStatus()
        {
            _CanceledStatus();

            tsmApplicationDelete.Enabled = true;
            tsmApplicationEdit.Enabled = true;
            tsnCancelApplication.Enabled = true;

            tsmSechduleTest.Enabled = true;
        }

        private void _CheckToPassedTest(clsLocalDrivingLicenseApplication LDLApp)
        {
            _CanceledStatus();

            bool PassedVisionTest = LDLApp.DoesPassTestType((int)clsTestAppointment.enTestType.VisionTest);
            bool PassedWrittenTest = LDLApp.DoesPassTestType((int)clsTestAppointment.enTestType.WrittenTest);
            bool PassedStreetTest = LDLApp.DoesPassTestType((int)clsTestAppointment.enTestType.StreetTest);

            tsmVision.Enabled = !PassedVisionTest && !PassedWrittenTest && !PassedStreetTest;
            tsmWritten.Enabled = PassedVisionTest && !PassedWrittenTest && !PassedStreetTest;
            tsmStreet.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
        }

        private void _ComplatedStatus()
        {
            _InitailizContextMenuStrip();
            tsmShowApplicationDetails.Enabled = true;
            tsmShowPersonLicenseHistory.Enabled = true;
            tsmShowLicense.Enabled = true;

        }
        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);
            
            if (LDLApp == null)
            {
                clsMessages.ErrorMassege("No Local Driving License Application With ID = " + LDLAppID);

                return;
            }

            _InitailizContextMenuStrip();

            bool IsCanceled = LDLApp.ApplicationStatus == (byte)clsApplication.enApplicationStatus.Cancelled;

            if (IsCanceled)
            {
                _CanceledStatus();

                return;
            }


            bool IsNew = LDLApp.ApplicationStatus == (byte)clsApplication.enApplicationStatus.New; ;

            if (IsNew)
            {
                _NewStatus();

                bool PassedAllTest = LDLApp.DoesPersonPassedAllTest();

                if (PassedAllTest)
                {
                    tsmSechduleTest.Enabled = false;
                    tsmIssueDrivingLicenseFirstTime.Enabled = true;
                }
                else
                {
                    _CheckToPassedTest(LDLApp);
                }

                return;
            }


            bool IsComplated = LDLApp.ApplicationStatus == (byte)clsApplication.enApplicationStatus.Complated;

            if (IsComplated)
            {
                _ComplatedStatus();

                return;
            }
        }

        private void tsmIssueDrivingLicenseFirstTime_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            frmIssueDrivingLicenseFirtsTime frm = new frmIssueDrivingLicenseFirtsTime(LDLAppID);

            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmShowLicense_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);

            if (LDLApp == null)
            {
                clsMessages.ErrorMassege("No Local Driving License Application With ID = " + LDLAppID);

                return;
            }

            int LicenseID = LDLApp.GetAnActiveLicenseID();

            frmShowLicenseInfo frm  = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void tsmShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvListLocalDrivingApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication _LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);

            if (_LDLApp == null)
            {
                clsMessages.ErrorMassege("No Local Driving License Application with ID = " + LDLAppID);

                return;
            }

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_LDLApp.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
