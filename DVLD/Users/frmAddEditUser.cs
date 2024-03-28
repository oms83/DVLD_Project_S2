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
    public partial class frmAddEditUser : Form
    {
        private enum enMode { AddNew, Update }
        private enMode _Mode;

        private clsUser _User;

        private int _UserID = -1;
        private int _PersonID = -1;

        public frmAddEditUser()
        {
            InitializeComponent();
            
            _Mode = enMode.AddNew;
        }
        public frmAddEditUser(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            _Mode = enMode.Update;
        }

        private void _RestUserInfo()
        {

            if (_Mode == enMode.AddNew)
            {
                _User = new clsUser();

                lblMode.Text = "Add New User";
            }
            else
            {
                lblMode.Text = "Update User Info";
            }

            ctrlUserCardWithFilter1.ResetData();

            txtConfirmPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtUserName.Text = string.Empty;
            lblUserID.Text = "[????]";
            cbIsActive.Checked = false;
            tpLoginInfo.Enabled = false;

        }

        private void _LoadUserInfo()
        {

            _User = clsUser.GetUserInfoByPersonID(_PersonID);

            if (_User == null)
            {
                _RestUserInfo();

                MessageBox.Show("No user with PersonID = " + _PersonID,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            tpLoginInfo.Enabled = true;
            ctrlUserCardWithFilter1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
            txtUserName.Text = _User.UserName;
            cbIsActive.Checked = _User.IsActive;
            
        }
        private void frmAddEditUser_Load(object sender, EventArgs e)
        {
            _RestUserInfo();

            if (_Mode == enMode.Update)
            {
                _LoadUserInfo();
            }
        }

        private void ctrlUserCardWithFilter1_OnPersonSelected(int PersonID)
        {
            _PersonID = PersonID;

            if (PersonID == -1)
            {
                _RestUserInfo();
              
                return;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tcAddEditUser.SelectedTab = tcAddEditUser.TabPages["tpLoginInfo"];
            }
            if (_Mode == enMode.AddNew)
            {
                if (ctrlUserCardWithFilter1.PersonID != -1)
                {
                    if (!clsUser.IsUserExistByPersonID(ctrlUserCardWithFilter1.PersonID))
                    {
                        tpLoginInfo.Enabled = true;
                        tcAddEditUser.SelectedTab = tcAddEditUser.TabPages["tpLoginInfo"];
                    }
                    else
                    {
                        tpLoginInfo.Enabled = false;
                        MessageBox.Show($"This person with ID = {ctrlUserCardWithFilter1.PersonID} already is a user in the system, you should select/add another person",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
                else
                {
                    tpLoginInfo.Enabled = false;
                    MessageBox.Show("You should select or add a person",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are invalid, Please put the mouse over the red icon(s) to see the error",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _User.Password = txtConfirmPassword.Text.Trim();
            _User.UserName = txtUserName.Text.Trim();
            _User.PersonID = ctrlUserCardWithFilter1.PersonID;
            _User.IsActive = cbIsActive.Checked;

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();

                lblMode.Text = "Update User Info";

                _Mode = enMode.Update;

                MessageBox.Show("Data saved successfully",
                                "Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data is not saved successfully",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This field is required!");

                return;
            }

            if (txtUserName.Text.Trim() != _User.UserName && clsUser.IsUserExistByUserName(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This Username used for another user!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text) || string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "This field is required!");

                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This field is required!");

                return;
            }

            if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {

                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "New passwords do not match");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditUser_Activated(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
                ctrlUserCardWithFilter1.FilterFocus();
        }
    }
}
