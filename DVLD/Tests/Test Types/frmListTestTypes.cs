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

namespace DVLD.Tests.Test_Types
{
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshData();
            _SetDGVColumns();
        }

        private void _SetDGVColumns()
        {
            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 100;
                
                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 250;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 400;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 120;

            }
        }

        private void _RefreshData()
        {
            _dtTestTypes = clsTestType.GetAllTestTypes();

            dgvTestTypes.DataSource = _dtTestTypes;

            lblNumberOfRecord.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType frm = new frmEditTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _RefreshData();
        }
    }
}
