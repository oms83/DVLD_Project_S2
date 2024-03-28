using DVLD.Applications.International_Licenses;
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
    public partial class frmListDrivers : Form
    {
        private DataTable _dtDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _SetDGVColumns()
        {
            txtFilterBy.Visible = false;    
            cmbFilterBy.SelectedIndex = 0;

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 120;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 120;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 140;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 320;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 200;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }
        }

        private void _RefreshData()
        {
            _dtDrivers = clsDriver.GetAllDrivers();

            dgvDrivers.DataSource = _dtDrivers;

            lblNumberOfRecord.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cmbFilterBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterBy.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName" && FilterColumn != "NationalNo")
                //in this case we deal with numbers not string.
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterBy.Text.Trim());
            else
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterBy.Text.Trim());

            lblNumberOfRecord.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Visible = cmbFilterBy.Text != "None";
            
            txtFilterBy.Text = string.Empty;
            txtFilterBy.Focus();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text == "Driver ID" || cmbFilterBy.Text == "Person ID")
            {
                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvDrivers.CurrentRow.Cells[1].Value);

            frm.ShowDialog();

            _RefreshData();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);

            frm.ShowDialog();

            _RefreshData();
        }

        private void issueInternationalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvDrivers.CurrentRow.Cells[0].Value;

            clsLicense _License = clsLicense.GetLicenseInfoByDriverID(DriverID, (int)clsLicenseClass.enLicenseClasses.Class_3_OrdinaryDrivingLicense);

            frmNewInternationalLicenseApplications frm = new frmNewInternationalLicenseApplications(
                                                             _License.LicenseID);
            frm.ShowDialog();

            
        }

        private void cmsDrivers_Opening(object sender, CancelEventArgs e)
        {
            int DriverID = (int)dgvDrivers.CurrentRow.Cells[0].Value;
            
            clsLicense _License = clsLicense.GetOrdinaryDrivingLincese(DriverID);
            clsInternationalLicense _InternationalLicense = clsInternationalLicense.GetInternationalLicenseInfoByDriverID(DriverID);

            issueInternationalToolStripMenuItem.Enabled = _License != null && _InternationalLicense == null;
        }
    }
}
