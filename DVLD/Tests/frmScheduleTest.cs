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
    public partial class frmScheduleTest : Form
    {
        private int _LDLAppID = -1;
        private clsTestAppointment.enTestType _TestTypeID = clsTestAppointment.enTestType.VisionTest;
        private int _TestAppointmentID = -1;
        public frmScheduleTest(int LDLAppID, clsTestAppointment.enTestType TestTypeID, int TestAppointmentID=-1)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
            _TestAppointmentID = TestAppointmentID;
            _LDLAppID = LDLAppID;
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTests1.TestType = _TestTypeID;
            ctrlScheduleTests1.LoadTestAppointmentInfo(_LDLAppID, _TestTypeID, _TestAppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
