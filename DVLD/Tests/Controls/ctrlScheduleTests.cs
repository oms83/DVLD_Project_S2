using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Business_Layer.clsTestAppointment;

namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduleTests : UserControl
    {
        private int _LDLAppID = -1;

        private clsTestAppointment.enTestType _TestTypeID = clsTestAppointment.enTestType.VisionTest;

        private bool _IsRetakeTest = false;
        public clsTestAppointment.enTestType TestType
        {
            get => _TestTypeID;

            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestAppointment.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;


                    case clsTestAppointment.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;


                    case clsTestAppointment.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                }
            }
        }
        
        private enum enMode { AddNew, Update }
        private enMode _Mode = enMode.AddNew;

        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;
        private clsLocalDrivingLicenseApplication  _LDLApp;
        public ctrlScheduleTests()
        {
            InitializeComponent();
        }

        private void ctrlScheduleTests_Load(object sender, EventArgs e)
        {

        }

        private void _ResetData()
        {

            
            lblRetakeTestAppID.Text = "[????]";
            
            dtpTestDate.Value = DateTime.Now;

            lblRetakeAppFees.Text = "0";
            lblFees.Text = clsTestType.GetTestTypeInfoTestTypeID((int)_TestTypeID).TestTypeFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblRetakeAppFees.Text) + Convert.ToSingle(lblFees.Text)).ToString();

            lblTitle.Text = "Schedule Test";


            lblUserMessage.Visible = false;
            gbRetakeTestInfo.Enabled = false;

            btnSave.Enabled = false;
        }

        public void LoadTestAppointmentInfo(int LDLAppID, clsTestAppointment.enTestType TestTypeID, int TestAppointment = -1)
        {

            _LDLAppID = LDLAppID;
            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointment;

            _ResetData();
            
            if (_TestAppointmentID == -1)
            {
                _Mode = enMode.AddNew;
                _TestAppointment = new clsTestAppointment();

            }
            else
            {
                _TestAppointment = clsTestAppointment.GetTestAppointmentInfoTestAppointmentID(_TestAppointmentID);

                if (_TestAppointment == null)
                {
                    clsMessages.ErrorMassege("No Test Appointment with ID = " + _TestAppointmentID);

                    return;
                }

                if (_TestAppointment.RetakeTestApplicationID != -1) 
                {
                    lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                }

                _Mode = enMode.Update;
            }

            _LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(LDLAppID);

            if (_LDLApp == null)
            {
                clsMessages.ErrorMassege("No L.D.L.Application with ID = " +  LDLAppID);

                return;
            }

            _LoadData();
        }

        private void _LoadData()
        {

            lblLocalDrivingLicenseAppID.Text = _LDLAppID.ToString();

            lblFullName.Text = _LDLApp.PersonFullName;

            lblDrivingClass.Text = _LDLApp.LicenseClassInfo.ClassName;

            lblTrial.Text = _LDLApp.TotalTrialPerTest((int)_TestTypeID).ToString();

            //dtpTestDate is was setted int _Reset() procedure
            //lblFees it was showed in _Reset() procedure

            _HandleRetakeTestConstraint();
            
            if (!_HandlePassingOfPreviousTest())
            {
                return;
            }
            
            if (_HandleDoesPassTestType())
            {
                return;
            }

            if (_Mode == enMode.AddNew)
            {
                dtpTestDate.MinDate = DateTime.Now;

                btnSave.Enabled = true;

                return;
            }

            if (_HandleLockedTestAppointment())
            {
                return;
            }

            _SetDateTimePickedForUpdateMode();

            btnSave.Enabled = true;
        }

        private bool _HandleDoesPassTestType()
        {
            bool DoesPassTestType = _LDLApp.DoesPassTestType((int)_TestTypeID);

            if (DoesPassTestType)
            {
                lblUserMessage.Text = "You Can't Update The Appointment Info Because It Is Already Locked!";
                dtpTestDate.Enabled = false;
                gbRetakeTestInfo.Enabled = false;
                return true;
            }
            return false;
        }
       
        private void _SetDateTimePickedForUpdateMode()
        {

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
            {
                dtpTestDate.MinDate = DateTime.Now;
            }
            else
            {
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;
            }
        }

        private bool _HandleLockedTestAppointment()
        {
            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                
                lblUserMessage.Text = $"You Can't Update The Appointment Info Because It Is Already Locked!";
                
                dtpTestDate.Enabled = false;

                gbRetakeTestInfo.Enabled = false;

                //gbRetakeTestInfo.Enabled we check to it in _HandleRetakeTestConstraint() procedure

                return true;
            }

            return false;
        }

        private void _HandleRetakeTestConstraint()
        {
            _IsRetakeTest = _LDLApp.DoesAttendTestType((int)_TestTypeID);
            
            if (_IsRetakeTest)
            {
                lblTitle.Text = "Retake Schedule Test";

                lblRetakeAppFees.Text = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();

                gbRetakeTestInfo.Enabled = true;
                
                //lblRetakeTestAppID this ID will be displayed in update mode or after added this appointment
            }

            if (_TestAppointment.RetakeTestApplicationID == -1 && _Mode == enMode.Update)
            {
                lblTitle.Text = "Schedule Test";

                gbRetakeTestInfo.Enabled = false;

                lblRetakeAppFees.Text = "0";
            }

            lblTotalFees.Text = (Convert.ToSingle(lblRetakeAppFees.Text) + Convert.ToSingle(lblFees.Text)).ToString();

        }
        private string _TestTypeAsText(clsTestAppointment.enTestType TestType)
        {
            switch (TestType)
            {
                case clsTestAppointment.enTestType.VisionTest:
                    return "";

                case clsTestAppointment.enTestType.WrittenTest:
                    return "Vision  Test";

                case clsTestAppointment.enTestType.StreetTest:
                    return "Written Test";
            }

            return "";
        }
        private bool _HandlePassingOfPreviousTest()
        {
            bool IsPreviousTestPassed = clsLocalDrivingLicenseApplication.DoesPassedPreviousTest(_LDLAppID, _TestTypeID);

            if (!IsPreviousTestPassed)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = $"Cannot Sechule, {_TestTypeAsText(_TestTypeID)} Should be Passed First.";

                gbRetakeTestInfo.Enabled = false;
                dtpTestDate.Enabled = false;

                return false;
            }

            return true;
        }

        private bool _HandleRetakTestApplication()
        {
            if (_Mode == enMode.AddNew && _IsRetakeTest) 
            {
                
                clsApplication RetakeApplication = new clsApplication();

                RetakeApplication.ApplicantPersonID = _LDLApp.ApplicantPersonID;
                RetakeApplication.ApplicationDate = DateTime.Now;
                RetakeApplication.ApplicationStatus = (byte)clsApplication.enApplicationStatus.Complated;
                RetakeApplication.LastStatusDate = DateTime.Now;
                RetakeApplication.ApplicationTypeID = clsApplication.enApplicationType.RetakeTest;
                RetakeApplication.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
                RetakeApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!RetakeApplication.Save())
                {
                    
                    clsMessages.ErrorMassege();
                    
                    _TestAppointment.RetakeTestApplicationID = -1;

                    return false;
                }
                
                _TestAppointment.RetakeTestApplicationID = RetakeApplication.ApplicationID;

            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandleRetakTestApplication())
            {
                return;
            }

            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LDLAppID;
            _TestAppointment.TestTypeID = (int)_TestTypeID;
            _TestAppointment.PaidFees = Convert.ToSingle(lblTotalFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            
            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                
                btnSave.Enabled = false;

                lblRetakeTestAppID.Text = _IsRetakeTest ? _TestAppointment.RetakeTestApplicationID.ToString() : "[????]";

                clsMessages.SuccessMassege();
            }
            else
            {
                clsMessages.ErrorMassege();
            }
        }
    }
}
