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
    public partial class frmShowUserInfo : Form
    {
        private int _UserID = -1;
        public frmShowUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID; 
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void frmShowUserInfo_Load_1(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID, "UserID");
        }
    }
}
