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

namespace DVLD.Tests.Test_Types
{
    public partial class frmEditTestType : Form
    {
        int _TestTypeID = -1;
        private clsTestType _TestType;
        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _ResetData();
            _LoadData();
        }

        private void _ResetData()
        {

            lblTestTypeID.Text = "[????]";
            txtDescription.Text = string.Empty;
            textBox1.Text = string.Empty;
            txtTitle.Text = string.Empty;
        }

        private void _LoadData()
        {
            lblTitle.Text = "Update Test Type";

            _TestType = clsTestType.GetTestTypeInfoTestTypeID(_TestTypeID);

            if (_TestType == null)
            {
                _ResetData();

                MessageBox.Show($"No Test Type with ID = {_TestTypeID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            txtTitle.Text = _TestType.TestTypeTitle.Trim();
            textBox1.Text = _TestType.TestTypeFees.ToString();
            txtDescription.Text = _TestType.TestTypeDescription.Trim();
            lblTestTypeID.Text = _TestType.TestTypeID.ToString();
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

            _TestType.TestTypeTitle = txtTitle.Text.Trim();
            _TestType.TestTypeDescription = txtDescription.Text.Trim();
            _TestType.TestTypeFees = Convert.ToSingle(textBox1.Text.Trim());

            if (_TestType.Save())
            {
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBoxValidation(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;

            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                Temp.Focus();
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsValidation.KeyPressHandleFloatNumber(sender, e);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                e.Cancel = true;
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "This field is required!");

                return;
            }

            if (!clsValidation.IsNumber(textBox1.Text))
            {
                e.Cancel = true;
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "This field should be a number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox1, null);
            }
        }
    }
}
