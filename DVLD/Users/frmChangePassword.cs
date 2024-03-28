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
    public partial class frmChangePassword : Form
    {
        private enum enMode { AddNew, ChangePassword }
        private enMode _Mode;

        private int _UserID;
        private clsUser _User;
        public frmChangePassword()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _Mode = enMode.ChangePassword;
            _UserID = UserID;
        }

        private void _RestUserInfo()
        {
            _RestFields();

            ctrlUserCard1.ResetUserInfo();
        }
        private void _RestFields()
        {
            txtConfirmPassword.Text = string.Empty;
            txtCurrentPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
        }

        private void _LoadUserInfo()
        {
            _User = clsUser.GetUserInfoUserID(_UserID);

            if (_User == null)
            {
                MessageBox.Show("No user with UserID = " + _UserID,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID, "UserID");
            _RestFields();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.ChangePassword)
            {
                _LoadUserInfo();
            }
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            string Password = txtCurrentPassword.Text.Trim();
            if (string.IsNullOrEmpty(Password))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "This field is required!");

                return;
            }

            if (Password != clsGlobal.CurrentUser.Password)
            {

                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "The new password does not match the current password");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            string Password = txtNewPassword.Text.Trim();
            if (string.IsNullOrEmpty(Password))
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
            string NewPassword = txtNewPassword.Text.Trim();
            string ConfirmPassword = txtConfirmPassword.Text.Trim();
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This field is required!");

                return;
            }

            if (NewPassword != ConfirmPassword)
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

            if (_User.ChangePassword(txtNewPassword.Text.Trim()))
            {
                MessageBox.Show("Password changed successfully",
                                "Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Password is not changed",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
