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
    public partial class frmFindPerson : Form
    {
        public frmFindPerson()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmFindPerson_Load(object sender, EventArgs e)
        {
            //ctrlUserCardWithFilter1.LoadPersonInfo(51);
        }

        private void ctrlUserCardWithFilter1_OnPersonSelected(int obj)
        {
            //MessageBox.Show($"{obj}");
        }
    }
}
