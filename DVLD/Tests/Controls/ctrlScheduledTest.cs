using DVLD.Global;
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

namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _TestAppointmentID = -1;
        private clsTestAppointment.enTestType _TestTypeID = clsTestAppointment.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsTestAppointment _TestAppointment;

        private int _TestID = -1;
        public int TestID
        {
            get => _TestID;
            set
            {
                _TestID = value;
                lblTestID.Text = _TestID.ToString();
            }
        }

        private int _TotalTrailPerTest = 0;
        public int TotalTrailPerTest
        {
            get => _TotalTrailPerTest;
            set
            {
                _TotalTrailPerTest = value;
                lblTrial.Text = _TotalTrailPerTest.ToString();
            }
        }
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
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void ResetData()
        {
            lblDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblDrivingClass.Text = "[????]";
            lblFees.Text = "[$$$$]";
            lblFullName.Text = "[????]";
            lblLocalDrivingLicenseAppID.Text = "[????]";
            lblTestID.Text = "[????]";
            lblTitle.Text = "Schedule Test";
            lblTrial.Text = "[????]";
            pbTestTypeImage.Image = Resources.Vision_512;
            gbTestType.Text = "Vision Test";
        }

        private void _LoadData()
        {
            lblDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblLocalDrivingLicenseAppID.Text = "[????]";
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialPerTest((int)_TestTypeID).ToString();
        }

        public void LoadTestInfo(clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication, clsTestAppointment TestAppointment, clsTestAppointment.enTestType TestTypeID)
        {
            ResetData();
          
            _LocalDrivingLicenseApplication = LocalDrivingLicenseApplication;
            _TestAppointment = TestAppointment;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointment.TestAppointmentID;
            _TestTypeID = TestTypeID;

            if (_LocalDrivingLicenseApplication == null)
            {
                clsMessages.ErrorMassege("No Local Driving License Application with ID = " + _LocalDrivingLicenseApplication);

                return;
            }

            if (_TestAppointment == null)
            {
                clsMessages.ErrorMassege("No Test Appointment with ID = " + _TestAppointmentID);

                return;
            }

            _LoadData();
        }
    }
}
