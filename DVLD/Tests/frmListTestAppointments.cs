using DVLD.Global_Classes;
using DVLD.Properties;
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

namespace DVLD.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private DataTable _dtTestAppointments;

        private int _LDLAppID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;
        private clsTestAppointment.enTestType _TestTypeID = clsTestAppointment.enTestType.VisionTest;
        public frmListTestAppointments(int LDLAppID, clsTestAppointment.enTestType TestTypeID)
        {
            InitializeComponent();

            _LDLAppID = LDLAppID;
            _TestTypeID = TestTypeID;
        }

        private void _RestData()
        {
            ctrlDrivingLicenseApplicationInfo1.ResetData();

            lblTestTypeTitle.Text = "Vision Test";
            pbTestType.Image = Resources.Vision_512;
        }

        private void _SetData()
        {
            ctrlDrivingLicenseApplicationInfo1.LoadLDLAppInfo(_LDLAppID);

            switch( _TestTypeID )
            {
                case clsTestAppointment.enTestType.VisionTest:
                    lblTestTypeTitle.Text = "Vision Test";
                    pbTestType.Image = Resources.Vision_512;
                    break;


                case clsTestAppointment.enTestType.WrittenTest:
                    lblTestTypeTitle.Text = "Written Test";
                    pbTestType.Image = Resources.Written_Test_512;
                    break; 


                case clsTestAppointment.enTestType.StreetTest:
                    lblTestTypeTitle.Text = "Street Test";
                    pbTestType.Image = Resources.driving_test_512;
                    break;
            }

        }

        private void _FillDataInDGV() 
        {
            _dtTestAppointments = clsTestAppointment.GetTestAppointmentByTestTypeIDAndLDLApp(_LDLAppID, _TestTypeID);

            dgvListLocalDrivingAppointments.DataSource = _dtTestAppointments;

            lblNumberOfRecord.Text = dgvListLocalDrivingAppointments.Rows.Count.ToString();
        }

        private void _SetDGVColumns()
        {
            if (dgvListLocalDrivingAppointments.Rows.Count > 0)
            {
                dgvListLocalDrivingAppointments.Columns[0].HeaderText = "Test Appointment ID";
                dgvListLocalDrivingAppointments.Columns[0].Width = 250;

                dgvListLocalDrivingAppointments.Columns[1].HeaderText = "Test Appointment Data";
                dgvListLocalDrivingAppointments.Columns[1].Width = 250;

                dgvListLocalDrivingAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvListLocalDrivingAppointments.Columns[2].Width = 150;

                dgvListLocalDrivingAppointments.Columns[3].HeaderText = "Is Locked";
                dgvListLocalDrivingAppointments.Columns[3].Width = 140;
            }
        }
        
        private void _RefreshData()
        {
            _FillDataInDGV();
            _SetDGVColumns();
        }


        private void _LoadData()
        {
            _RefreshData();
            _SetData();
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            bool IsThereAnActiveSchedule = clsLocalDrivingLicenseApplication.IsThereAnActiveScheduleTest(_LDLAppID, (int)_TestTypeID);

            if (IsThereAnActiveSchedule)
            {
                clsMessages.ErrorMassege("You can not add a new test appointement because there is an active test!");
                return;
            }

            bool IsPassedTest = clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, (int)_TestTypeID);

            if (IsPassedTest)
            {
                clsMessages.ErrorMassege("You can not add a new test appointement because this test has already been passed!");
                return;
            }

            
            frmScheduleTest frm = new frmScheduleTest(_LDLAppID, _TestTypeID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvListLocalDrivingAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTest frm = new frmScheduleTest(_LDLAppID, _TestTypeID, TestAppointmentID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvListLocalDrivingAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(_LDLAppID, TestAppointmentID, _TestTypeID);
            frm.ShowDialog();

            _RefreshData();
        }
    }
}
