using DVLD.Global_Classes;
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

namespace DVLD
{
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _PersonID = -1;

        public frmShowPersonLicenseHistory(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            
            ctrlUserCardWithFilter1.LoadPersonInfo(_PersonID);
            

            ctrlDriverLicenses1.LoadDriverLicensesData(_PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlUserCardWithFilter1_OnPersonSelected(int PersonID)
        {
            if (PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();

                return;
            }

            ctrlDriverLicenses1.LoadDriverLicensesData(PersonID);

        }
    }
}
