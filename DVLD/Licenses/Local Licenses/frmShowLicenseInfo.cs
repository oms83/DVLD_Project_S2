using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmShowLicenseInfo : Form
    {
        private int _LicneseID = -1;
        public frmShowLicenseInfo(int LicenseID)
        {
            InitializeComponent();

            _LicneseID = LicenseID;
        }

        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadLicenseInfo(_LicneseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
