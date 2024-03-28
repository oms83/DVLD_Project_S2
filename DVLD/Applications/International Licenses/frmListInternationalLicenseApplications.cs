﻿using DVLD.Global_Classes;
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

namespace DVLD.Applications.International_Licenses
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        DataTable _dtInternationalLicenses;
        clsDriver _Driver;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }
        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _RefreshData()
        {
            _dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();
            dgvListInternationalLicenseApplications.DataSource = _dtInternationalLicenses;
            lblNumberOfRecord.Text = dgvListInternationalLicenseApplications.Rows.Count.ToString();
        }

        private void _SetDGVColumns()
        {
            cmbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;
            if (dgvListInternationalLicenseApplications.Rows.Count > 0)
            {
                dgvListInternationalLicenseApplications.Columns[0].Width = 220;
                dgvListInternationalLicenseApplications.Columns[0].HeaderText = "Interantion License ID";

                dgvListInternationalLicenseApplications.Columns[1].Width = 150;
                dgvListInternationalLicenseApplications.Columns[1].HeaderText = "Application ID";

                dgvListInternationalLicenseApplications.Columns[2].Width = 150;
                dgvListInternationalLicenseApplications.Columns[2].HeaderText = "Driver ID";

                dgvListInternationalLicenseApplications.Columns[3].Width = 245;
                dgvListInternationalLicenseApplications.Columns[3].HeaderText = "Local License ID";

                dgvListInternationalLicenseApplications.Columns[4].Width = 265;
                dgvListInternationalLicenseApplications.Columns[4].HeaderText = "Issue Date";

                dgvListInternationalLicenseApplications.Columns[5].Width = 150;
                dgvListInternationalLicenseApplications.Columns[5].HeaderText = "Expiration Date";
            }
        }

        private void cmbActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterString = "";
            string FilteringBy = "IsActive";
            //All
            //Yes
            //No
            switch (cmbActiveFilter.Text)
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

            if (cmbFilterBy.Text.Trim() == "All" || FilterString == "")
            {
                _dtInternationalLicenses.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvListInternationalLicenseApplications.Rows.Count.ToString();
                return;
            }
            else
            {
                _dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilteringBy, FilterString);
                lblNumberOfRecord.Text = dgvListInternationalLicenseApplications.Rows.Count.ToString();
            }

        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshData();
            if (cmbFilterBy.Text == "None")
            {
                txtFilterBy.Visible = false;
                txtFilterBy.Visible = false;
                cmbActiveFilter.Visible = false;
            }

            else if (cmbFilterBy.Text == "Is Active")
            {
                cmbActiveFilter.SelectedItem = "All";
                txtFilterBy.Visible = false;
                cmbActiveFilter.Visible = true;
            }
            else
            {
                cmbActiveFilter.Visible = false;
                txtFilterBy.Visible = true;
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            string FilterString = "";

            switch (cmbFilterBy.Text)
            {
                case "International License ID":
                    FilterString = "InternationalLicenseID";
                    break;
                case "Application ID":
                    FilterString = "ApplicationID";
                    break;
                case "Driver ID":
                    FilterString = "DriverID";
                    break;
                case "Local License ID":
                    FilterString = "IssuedUsingLocalLicenseID";
                    break;
                case "Is Active":
                    FilterString = "IsActive";
                    break;
                case "None":
                    FilterString = "";
                    break;
                default:
                    FilterString = "";
                    break;
            }

            if (cmbFilterBy.Text.Trim() == "None" || txtFilterBy.Text.Trim() == "")
            {
                _dtInternationalLicenses.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvListInternationalLicenseApplications.Rows.Count.ToString();
                _RefreshData();
                return;
            }

            if (cmbFilterBy.Text.Trim() == "International License ID" || cmbFilterBy.Text.Trim() == "Driver ID" ||
                cmbFilterBy.Text.Trim() == "Local License ID" || cmbFilterBy.Text.Trim() == "Application ID")
            {
                _dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterString, txtFilterBy.Text.Trim());
            }
            lblNumberOfRecord.Text = dgvListInternationalLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text.Trim() == "International License ID" || cmbFilterBy.Text.Trim() == "Driver ID" ||
                cmbFilterBy.Text.Trim() == "Local License ID" || cmbFilterBy.Text.Trim() == "Application ID")
            {
                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplications frm = new frmNewInternationalLicenseApplications();
            frm.ShowDialog();
            _RefreshData();
        }

        private void tsmShowPersonDetails_Click(object sender, EventArgs e)
        {
            int _DriverID = (int)dgvListInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            clsDriver _Driver = clsDriver.GetDriverInfoDriverID(_DriverID);
            if (_Driver == null)
            {
                clsMessages.ErrorMassege("No driver with DriverID = " +  _DriverID);
                return;
            }

            frmShowPersonInfo frm = new frmShowPersonInfo(_Driver.PersonID);

            frm.ShowDialog();
        }

        private void tsmShowLicenseDetails_Click(object sender, EventArgs e)
        { 
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvListInternationalLicenseApplications.CurrentRow.Cells[3].Value);

            frm.ShowDialog();
        }

        private void tsmShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            int _DriverID = (int)dgvListInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            clsDriver _Driver = clsDriver.GetDriverInfoDriverID(_DriverID);
            if (_Driver == null)
            {
                clsMessages.ErrorMassege("No driver with DriverID = " + _DriverID);
                return;
            }

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_Driver.PersonID);

            frm.ShowDialog();
        }
    }
}
