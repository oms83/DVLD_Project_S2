using DVLD.Global_Classes;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _TestAppointmentID = -1;
        private clsTestAppointment.enTestType _TestTypeID = clsTestAppointment.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsTestAppointment _TestAppointment;
        private clsTest _Test;

        private enum enMode { AddNew, Update }
        private enMode _Mode;
        public frmTakeTest(int LocalDrivingLicenseApplicationID, int TestAppointmentID, clsTestAppointment.enTestType TestTypeID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void _ResetData()
        {
            ctrlScheduledTest1.ResetData();
            
            txtNotes.Text = "";
            rbPass.Checked = true;
            lblUserMessage.Visible = false;
            btnSave.Enabled = false;
        }

        private void _LoadData()
        {
            _ResetData();

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                clsMessages.ErrorMassege("No Local Driving License Application with ID = " + _LocalDrivingLicenseApplication);

                return;
            }

            _TestAppointment = clsTestAppointment.GetTestAppointmentInfoTestAppointmentID(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                clsMessages.ErrorMassege("No Test Appointment with ID = " + _TestAppointmentID);

                return;
            }

            ctrlScheduledTest1.TestType = _TestTypeID;
            ctrlScheduledTest1.LoadTestInfo(_LocalDrivingLicenseApplication, _TestAppointment, _TestTypeID);

        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _LoadData();

            if (_TestAppointment.IsLocked)
            {
                _Mode = enMode.Update;

                _Test = clsTest.GetTestInfoByTestAppointmentID(_TestAppointmentID);

                if (_Test == null)
                {
                    clsMessages.ErrorMassege("No Test with TestAppointmentID = " + _TestAppointmentID);

                    return;
                }

                ctrlScheduledTest1.TestID = _Test.TestID;

                rbFail.Checked = !(rbPass.Checked = _Test.TestResult);

                txtNotes.Text = _Test.Notes;

                _SetInfoInUpdateModeAndAfterSaveInfo();
            }
            else
            {
                _Mode = enMode.AddNew;

                _Test = new clsTest();
                
                lblUserMessage.Visible = false;
                
                btnSave.Enabled = true;
            }
        }

        private void _SetInfoInUpdateModeAndAfterSaveInfo()
        {
            btnSave.Enabled = false;
            rbFail.Enabled = false;
            rbPass.Enabled = false;
            txtNotes.Enabled = false;

            lblUserMessage.Text = "You cannot change the results";
            lblUserMessage.Visible = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you can't change the test result", 
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.TestResult = rbPass.Checked;
            _Test.TestAppointmentID = _TestAppointmentID;

            if (_Test.Save())
            {
                //ctrlScheduledTest1.LoadTestInfo(_LocalDrivingLicenseApplication, _TestAppointment, _TestTypeID);
                ctrlScheduledTest1.TestID = _Test.TestID;
                ctrlScheduledTest1.TotalTrailPerTest = _LocalDrivingLicenseApplication.TotalTrialPerTest((int)_TestTypeID);

                _SetInfoInUpdateModeAndAfterSaveInfo();

                clsMessages.SuccessMassege();
            }
            else
            {
                clsMessages.ErrorMassege();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
