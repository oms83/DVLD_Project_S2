using DVLD.Global_Classes;
using DVLD.Licenses.International_Licenses;
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
    public partial class ctrlDriverLicenses : UserControl
    {
        private DataTable _dtInternationlLicenses;
        private DataTable _dtLocalLicenses;

        private clsDriver _Driver;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }
        
        public void LoadDriverLicensesData(int PersonID)
        {

            _Driver = clsDriver.GetDriverInfoPersonID(PersonID);

            if (_Driver == null)
            {
                clsMessages.ErrorMassege("No Driver with PersonID = " + PersonID);

                return;
            }
            
            tabControl1.SelectedTab = tabControl1.TabPages["tbLocalLicenses"];
            _RefreshLocalLicenses();
        }
        private void _SetInternationalLicensesDGVColumns()
        {
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 170;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 170;

                dgvInternationalLicenses.Columns[2].HeaderText = "License ID";
                dgvInternationalLicenses.Columns[2].Width = 120;

                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[3].Width = 200;

                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[4].Width = 200;

                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[5].Width = 100;
            }
        }

        private void _SetLocalLicensesDGVColumns()
        {
            if (dgvLocalLicenses.Rows.Count > 0)
            {
                dgvLocalLicenses.Columns[0].HeaderText = "Loc. ID";
                dgvLocalLicenses.Columns[0].Width = 95;

                dgvLocalLicenses.Columns[1].HeaderText = "App. ID";
                dgvLocalLicenses.Columns[1].Width = 95;

                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[2].Width = 310;

                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[3].Width = 200;

                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[4].Width = 200;

                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[5].Width = 95;
            }
        }
        private void _FillDGVWithInternationalLicensesData()
        {
            _dtInternationlLicenses = clsInternationalLicense.GetDriverInternationalLicenses(_Driver.DriverID);

            dgvInternationalLicenses.DataSource = _dtInternationlLicenses;

            lblNumberOfRecord.Text = dgvInternationalLicenses.Rows.Count.ToString();    
        }
        private void _FillDGVWithLocalLicensesData()
        {
            _dtLocalLicenses = clsLicense.GetDriverLicenses(_Driver.DriverID);

            dgvLocalLicenses.DataSource = _dtLocalLicenses;

            lblNumberOfRecord.Text = dgvLocalLicenses.Rows.Count.ToString();
        }

        private void _RefreshInternationalLicenses()
        {
            _FillDGVWithInternationalLicensesData();
            _SetInternationalLicensesDGVColumns();
        }

        private void _RefreshLocalLicenses()
        {
            _FillDGVWithLocalLicensesData();
            _SetLocalLicensesDGVColumns();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tbLocalLicenses"])
            {
                _RefreshLocalLicenses();
            }
            else
            {
                _RefreshInternationalLicenses();
            }
        }

        public void Clear()
        {
            //_dtLocalLicenses.Clear();
        }

        private void ShowLocalLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);

            frm.ShowDialog();
        }

        private void tsmShowInternationalLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);

            frm.ShowDialog();
        }
    }
}
