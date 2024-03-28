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
    public partial class ctrlUserCard : UserControl
    {
        private clsUser _User;

        private int _PersonID = -1;
        private int _UserID = -1;

        public clsUser User
        {
            get
            {
                return _User;
            }
        }

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void ResetUserInfo()
        {
            ctrlPersonInfo1.ResetData();

            lblIsActive.Text = "---";
            lblUserID.Text = "---";
            lblUserName.Text = "---";
        }

        private void _LoadData()
        {
            ctrlPersonInfo1.LoadPersonInfo(_PersonID);

            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
            lblUserID.Text = _UserID.ToString();
            lblUserName.Text = _User.UserName.Trim();
        }
        public void LoadUserInfo(int ID, string ByWhat = "UserID")
        {
            switch (ByWhat)
            {
                case "UserID":

                    _User = clsUser.GetUserInfoUserID(ID);
                    if (_User ==  null)
                    {
                        ResetUserInfo();

                        MessageBox.Show($"No User with UserID = {ID}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                        return;
                    }

                    _UserID = _User.UserID;
                    _PersonID = _User.PersonID;

                    break;


                case "PersonID":

                    _User = clsUser.GetUserInfoByPersonID(ID);
                    if (_User == null)
                    {
                        ResetUserInfo();

                        MessageBox.Show($"No User with PersonID = {ID}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                        return;
                    }

                    _UserID = _User.UserID;
                    _PersonID = _User.PersonID;
                    break;
            }

            _LoadData();
        }
    }
}
