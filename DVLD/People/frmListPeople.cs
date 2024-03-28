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
    public partial class frmListPeople : Form
    {
        private static DataTable _dtAllPeople = clsPerson.GelAllPeople();

        /*
            The line of code you provided creates a new DataTable named _dtPerson from a default view of an existing DataTable called _dtAllPeople. 
            The ToTable method is used to make a new copy of the table with only the specified columns. 
            The false parameter indicates that the rows will not be distinct, meaning duplicate rows are possible. 
        */

        // Only select the columns that you want to show in grid view
        private static DataTable _dtPerson = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshData()
        {
            _dtAllPeople = clsPerson.GelAllPeople();
            _dtPerson = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GenderCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            dgvListPeople.DataSource = _dtPerson;
            lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
        }
        private void _SetDGVColumns()
        {
            cmbFilterBy.SelectedIndex = 0;

            if (dgvListPeople.Rows.Count > 0)
            {
                dgvListPeople.Columns[0].HeaderText = "Person ID";
                dgvListPeople.Columns[0].Width = 100;

                dgvListPeople.Columns[1].HeaderText = "National No.";
                dgvListPeople.Columns[1].Width = 110;

                dgvListPeople.Columns[2].HeaderText = "First Name";
                dgvListPeople.Columns[2].Width = 110;

                dgvListPeople.Columns[3].HeaderText = "Second Name";
                dgvListPeople.Columns[3].Width = 105;

                dgvListPeople.Columns[4].HeaderText = "Third Name";
                dgvListPeople.Columns[4].Width = 105;

                dgvListPeople.Columns[5].HeaderText = "Last Name";
                dgvListPeople.Columns[5].Width = 105;

                dgvListPeople.Columns[6].HeaderText = "Gender";
                dgvListPeople.Columns[6].Width = 100;

                dgvListPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvListPeople.Columns[7].Width = 150;

                dgvListPeople.Columns[8].HeaderText = "Country";
                dgvListPeople.Columns[8].Width = 100;

                dgvListPeople.Columns[9].HeaderText = "Phone";
                dgvListPeople.Columns[9].Width = 110;

                dgvListPeople.Columns[10].HeaderText = "Email";
                dgvListPeople.Columns[10].Width = 145;
            }
        }

        private void _FillDataInDGV()
        {
            _RefreshData();
            _SetDGVColumns();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _FillDataInDGV();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            /*
                None
                Person ID
                National No.
                First Name
                Second Name
                Third Name
                Last Name
                Nationality
                Gendor
                Phone
                Email
            */

            string FilterString = "";

            switch (cmbFilterBy.Text)
            {
                case "Person ID":
                    FilterString = "PersonID";
                    break;
                case "National No.":
                    FilterString = "NationalNo";
                    break;
                case "First Name":
                    FilterString = "FirstName";
                    break;
                case "Second Name":
                    FilterString = "SecondName";
                    break;
                case "Third Name":
                    FilterString = "ThirdName";
                    break;
                case "Last Name":
                    FilterString = "LastName";
                    break;
                case "Nationality":
                    FilterString = "CountryName";
                    break;
                case "Gendor":
                    FilterString = "Gender";
                    break;
                case "Phone":
                    FilterString = "Phone";
                    break;
                case "Email":
                    FilterString = "Email";
                    break;
                default: 
                    FilterString = "";
                    break;
            }

            if (txtFilterValue.Text.Trim() == "" || cmbFilterBy.Text.Trim() == "None")
            {
                _dtPerson.DefaultView.RowFilter = "";
                lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
                return;
            }
            if (cmbFilterBy.Text == "Person ID")
            {
                _dtPerson.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterString, txtFilterValue.Text.Trim());
            }
            else
            {
                _dtPerson.DefaultView.RowFilter = string.Format("[{0}] Like '{1}%'", FilterString, txtFilterValue.Text.Trim());
            }

            lblNumberOfRecord.Text = dgvListPeople.Rows.Count.ToString();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cmbFilterBy.Text != "None";
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // When filtering is by person ID, we prevent the user from entering any characters other than numbers
            if (cmbFilterBy.Text == "Person ID")
            {
                //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete this person with ID = {PersonID}",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No ) 
            {
                return;
            }

            if (clsPerson.DeletePerson(PersonID))
            {
                _RefreshData();

                MessageBox.Show($"Person with ID = {PersonID} deleted successfully",
                "Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                
            }
            else
            {
                MessageBox.Show($"Person with ID = {PersonID} is not deleted, Because has data linked to is.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
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

        private void tsmShowDetails_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[0].Value;
            
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void tsmAddNew_Click(object sender, EventArgs e)
        {

            frmAddNewEdit _frmAddNewEdit = new frmAddNewEdit();
            _frmAddNewEdit.ShowDialog();

            _RefreshData();

        }
        private void tsmEdit_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[0].Value;

            frmAddNewEdit frm = new frmAddNewEdit(PersonID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void dgvListPeople_DoubleClick(object sender, EventArgs e)
        {
            int PersonID = (int)dgvListPeople.CurrentRow.Cells[0].Value;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();

            _RefreshData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddNewEdit edit = new frmAddNewEdit();
            edit.ShowDialog();
            _RefreshData();
        }
    }
}
