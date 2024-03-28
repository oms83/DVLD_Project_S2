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

namespace DVLD.Applications.Application_Types
{
    public partial class frmListApplicationTypes : Form
    {
        private DataTable _dtApplicationsType;

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

        private void _SetDGVColumns()
        {
            if (dgvApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 100;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 400;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 115;

            }
        }

        private void _RefreshData()
        {
            _dtApplicationsType = clsApplicationType.GetAllApplicationTypes();

            dgvApplicationTypes.DataSource = _dtApplicationsType;

            lblNumberOfRecord.Text = dgvApplicationTypes.Rows.Count.ToString();
        }
        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);  
            frm.ShowDialog();

            _RefreshData();
        }
    }
}
