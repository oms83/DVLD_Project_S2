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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;
        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
        }
        private void _ResetData()
        {

            lblApplicationTypeID.Text = "[????]";
            txtFees.Text = string.Empty;
            txtTitle.Text = string.Empty;
        }

        private void _LoadData()
        {
            lblTitle.Text = "Update Test Type";

            _ApplicationType = clsApplicationType.GetApplicationTypeInfoByID(_ApplicationTypeID);

            if (_ApplicationType == null)
            {
                _ResetData();

                MessageBox.Show($"No Application Type with ID = {_ApplicationTypeID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            txtTitle.Text = _ApplicationType.ApplicationTypeTitle.Trim();
            txtFees.Text = _ApplicationType.ApplicationFees.ToString();
            lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
        }
        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            _ResetData();
            _LoadData();
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

            _ApplicationType.ApplicationTypeTitle = txtTitle.Text.Trim();
            _ApplicationType.ApplicationFees = Convert.ToSingle(txtFees.Text.Trim());

            if (_ApplicationType.Save())
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

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                txtTitle.Focus();
                errorProvider1.SetError(txtTitle, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                txtFees.Focus();
                errorProvider1.SetError(txtFees, "This field is required!");
                
                return;
            }

            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                txtFees.Focus();
                errorProvider1.SetError(txtFees, "This field should be a number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsValidation.KeyPressHandleFloatNumber(sender, e); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
