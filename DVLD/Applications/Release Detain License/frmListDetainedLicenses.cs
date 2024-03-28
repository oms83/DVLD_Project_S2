using DVLD.Global_Classes;
using DVLD.Licenses.Detain_Licenses;
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

namespace DVLD.Applications.Release_Detain_License
{
    public partial class frmListDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _SetDGVColumns()
        {
            cmbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;

            if (dgvListDetainedLicenses.Rows.Count > 0)
            {
                dgvListDetainedLicenses.Columns[0].Width = 100;
                dgvListDetainedLicenses.Columns[0].HeaderText = "Detained ID";

                dgvListDetainedLicenses.Columns[1].Width = 100;
                dgvListDetainedLicenses.Columns[1].HeaderText = "L. ID";

                dgvListDetainedLicenses.Columns[2].HeaderText = "Detained Date";
                dgvListDetainedLicenses.Columns[2].Width = 190;

                dgvListDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvListDetainedLicenses.Columns[3].Width = 100;

                dgvListDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvListDetainedLicenses.Columns[4].Width = 150;

                dgvListDetainedLicenses.Columns[5].HeaderText = "Released Date";
                dgvListDetainedLicenses.Columns[5].Width = 175;

                dgvListDetainedLicenses.Columns[6].HeaderText = "N. No.";
                dgvListDetainedLicenses.Columns[6].Width = 100;

                dgvListDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvListDetainedLicenses.Columns[7].Width = 220;

                dgvListDetainedLicenses.Columns[8].HeaderText = "Release App. ID";
                dgvListDetainedLicenses.Columns[8].Width = 130;
            }

            
        }
        private void _RefreshData()
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();
            dgvListDetainedLicenses.DataSource = _dtDetainedLicenses;
            lblNumberOfRecord.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.Text == "None")
            {
                txtFilterBy.Visible = false;
                cmbReleased.Visible = false;
                _RefreshData();
            }
            else if (cmbFilterBy.Text == "Is Released")
            {
                txtFilterBy.Visible = false;
                cmbReleased.Visible = true;
                cmbReleased.SelectedIndex = 0;
            }
            else
            {
                txtFilterBy.Visible = true;
                cmbReleased.Visible = false;
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterString = "";


            switch (cmbFilterBy.Text)
            {
                case "None":
                    FilterString = "";
                    break;
                case "Detain ID":
                    FilterString = "DetainID";
                    break;
                case "National No.":
                    FilterString = "NationalNo";
                    break;
                case "Full Name":
                    FilterString = "FullName";
                    break;
                case "Release Application ID":
                    FilterString = "ReleaseApplicationID";
                    break;
                default:
                    FilterString = "";
                    break;
            }

            if (cmbFilterBy.Text.Trim() == "None" || txtFilterBy.Text.Trim() == "")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvListDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterString == "DetainID" || FilterString == "ReleaseApplicationID")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterString, txtFilterBy.Text.Trim());
            }
            else if (FilterString == "NationalNo" || FilterString == "FullName")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterString, txtFilterBy.Text.Trim());
            }

            lblNumberOfRecord.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void cmbReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterString = "";
            string FilterBy = "IsReleased";

            switch (cmbReleased.Text)
            {
                case "All":
                    FilterString = "";
                    break;
                case "Yes":
                    FilterString = "1";
                    break;
                case "No":
                    FilterString = "0";
                    break;
                default:
                    FilterString = "";
                    break;
            }

            if (cmbReleased.Text == "All")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                _RefreshData();
                return;
            }
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterBy, FilterString);
            }
            lblNumberOfRecord.Text = dgvListDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text.Trim() == "Detain ID" || cmbFilterBy.Text.Trim() == "Release Application ID")
            {
                clsValidation.KeyPressHandle(sender, e);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();

            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();

            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void tsmShowPersonDetails_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value;
            
            int PersonID = clsLicense.GetLicenseInfoByID(LicenseID).DriverInfo.PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void tsmShowLicenseDetails_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value;

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void tsmShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.GetLicenseInfoByID(LicenseID).DriverInfo.PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void tsmReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvListDetainedLicenses.CurrentRow.Cells[1].Value;

            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication(LicenseID);
            frm.ShowDialog();
            //refresh
            frmListDetainedLicenses_Load(null, null);
        }

        private void cmsIApplications_Opening(object sender, CancelEventArgs e)
        {
            tsmReleaseDetainedLicense.Enabled = !(bool)dgvListDetainedLicenses.CurrentRow.Cells[3].Value;
        }
    }
}
