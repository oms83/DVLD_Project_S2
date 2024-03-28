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
    public partial class frmListUsers : Form
    {
        private DataTable _dtUsers;
        public frmListUsers()
        {
            InitializeComponent();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _RefreshData()
        {
            _dtUsers = clsUser.GetAllUsers();

            dgvListPeople.DataSource = _dtUsers;

            lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void _SetDGVColumns()
        {
            cmbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;
            cmbIsActive.Visible = cmbFilterBy.Text == "Is Active";

            if (dgvListPeople.Rows.Count > 0)
            {
                dgvListPeople.Columns[0].HeaderText = "User ID";
                dgvListPeople.Columns[0].Width = 200;
                
                dgvListPeople.Columns[1].HeaderText = "Person ID";
                dgvListPeople.Columns[1].Width = 200;

                dgvListPeople.Columns[2].HeaderText = "Full Name";
                dgvListPeople.Columns[2].Width = 400;
                
                dgvListPeople.Columns[3].HeaderText = "UserName";
                dgvListPeople.Columns[3].Width = 200;
             
                dgvListPeople.Columns[4].HeaderText = "Is Active";
                dgvListPeople.Columns[4].Width = 140;
            }
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.Text == "None")
            {
                cmbIsActive.Visible = false;
                txtFilterBy.Visible = false;
            }
            else if (cmbFilterBy.Text == "Is Active")
            {
                cmbIsActive.Visible = true;
                cmbIsActive.SelectedIndex = 0;
                txtFilterBy.Visible = false;
            }
            else
            {
                cmbIsActive.Visible = false;
                txtFilterBy.Visible = true;
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }

            _RefreshData();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            /*
            
                None
                User ID
                UserName
                Person ID
                Full Name
                Is Active
            
            */
            string FilteringString = "";

            switch (cmbFilterBy.Text)
            {
                case "None":
                    FilteringString = "";
                    break;

                case "User ID":
                    FilteringString = "UserID";
                    break;

                case "Person ID":
                    FilteringString = "PersonID";
                    break;

                case "Full Name":
                    FilteringString = "FullName";
                    break;
                case "UserName":
                    FilteringString = "UserName";
                    break;
            }

            if (txtFilterBy.Text.Trim() == "" || FilteringString == "None")
            {
                _dtUsers.DefaultView.RowFilter = "";
                _RefreshData();
                return;
            }
            if (FilteringString == "PersonID" || FilteringString == "UserID")
            {
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilteringString, txtFilterBy.Text.Trim());
            }
            else if (FilteringString == "FullName" || FilteringString == "UserName")
            {
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilteringString, txtFilterBy.Text.Trim());
            }

            lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilteringString = "";
            string FilteringBy = "IsActive";
            switch (cmbIsActive.Text)
            {
                case "All":
                    FilteringString = "";
                    break;

                case "Yes":
                    FilteringString = "1";
                    break;

                case "No":
                    FilteringString = "0";
                    break;

                default:
                    FilteringString = "";
                    break;
            }

            if (FilteringBy == "All")
            {
                _dtUsers.DefaultView.RowFilter = "";
                _RefreshData();
                return;
            }
            if (cmbIsActive.Text == "No" || cmbIsActive.Text == "Yes")
            {
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilteringBy, FilteringString);
            }

            lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.Text == "User ID" || cmbFilterBy.Text == "Person ID")
            {
                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();

            _RefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmShowDetails_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvListPeople.CurrentRow.Cells[0].Value;
            
            frmShowUserInfo frm = new frmShowUserInfo(UserID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvListPeople.CurrentRow.Cells[0].Value;

            frmChangePassword frm = new frmChangePassword(UserID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvListPeople.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete this user with ID = {UserID}",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (clsUser.DeleteUser(UserID))
            {
                _RefreshData();

                MessageBox.Show($"User with ID = {UserID} deleted successfully",
                "Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show($"User with ID = {UserID} is not deleted, Because has data linked to is.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void tsmAddNew_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmEdit_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[1].Value;

            frmAddEditUser frm = new frmAddEditUser(PersonID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmSendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"This feature is not implemented yet !",
            "Not Ready",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation);
        }

        private void tsmPhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"This feature is not implemented yet !",
            "Not Ready",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation);
        }
    }
}
