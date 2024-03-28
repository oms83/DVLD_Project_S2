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
    public partial class frmLocalDrivingLicenseApplication : Form
    {
        private int _LDLAppID = -1;
        public frmLocalDrivingLicenseApplication(int LDLAppID)
        {
            InitializeComponent();

            _LDLAppID = LDLAppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadLDLAppInfo(_LDLAppID);
        }
    }
}
