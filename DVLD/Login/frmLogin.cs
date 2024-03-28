using DVLD.Global_Classes;
using DVLD.Properties;
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
    public partial class frmLogin : Form
    {
        private string _Password;
        private string _UserName;
        private clsUser _User;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            _Password = txtPassword.Text.Trim();
            _UserName = txtUserName.Text.Trim();

            if (!clsUser.IsUserExistByUserNameAndPassword(_UserName, _Password))
            {
                MessageBox.Show("Invalid UserName/Password",
                                "Wrong Credintials",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _User = clsUser.GetUserInfoByUserName(_UserName);
            
            if (!_User.IsActive)
            {
                MessageBox.Show("Your account is not active, Contact Admin",
                                "Inactive Account",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            bool _SaveData = chkbRememberMe.Checked;

            clsGlobal.WriteDataToFile(_UserName, _Password, _SaveData);


            frmMain frm = new frmMain(this);

            clsGlobal.CurrentUser = _User;
            this.Hide();
            
            frm.ShowDialog();

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string _UserName = "";
            string _Password = "";

            clsGlobal.ReadDataFromFile(ref _UserName, ref _Password);

            txtUserName.Text = _UserName;
            txtPassword.Text = _Password;

            chkbRememberMe.Checked = txtPassword.Text != "" && txtUserName.Text != "";
        }

        bool IsShowing = false;
        private void btnShowIgnore_Click(object sender, EventArgs e)
        {
            IsShowing = !IsShowing;

            if (IsShowing)
            {
                txtPassword.UseSystemPasswordChar = false;
                btnShowIgnore.Image = Resources.red_eye;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
                btnShowIgnore.Image = Resources.eye_closed;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
