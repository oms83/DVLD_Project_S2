using DVLD.Global_Classes;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _btnFilterEnabled = true;

        public bool btnFilterEnabled
        {
            get => _btnFilterEnabled;
            set
            {
                _btnFilterEnabled = value;
                btnFind.Enabled = _btnFilterEnabled;
            }
        }

        private bool _gbEnabled = true;

        public bool gbEnabled
        {
            get => _gbEnabled;
            set
            {
                _gbEnabled = value;
                gbFiltering.Enabled = _gbEnabled;
            }
        }

        public clsLicense SelectedLicense
        {
            get => ctrlDriverLicenseInfo1.SelectedLicense;
        }

        public int SelectedLicenseID
        {
            get => ctrlDriverLicenseInfo1.SelectedLicense.LicenseID;
        }

        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;

            if (handler != null)
            {
                handler(LicenseID);
            }
        }

        private int _LicenseID = -1;
        private clsLicense _License;
        private enum enMode { Filter, Display }
        private enMode _Mode;
        public void ResetData()
        {
            ctrlDriverLicenseInfo1.ResetData();
            txtLicenseID.Text = "";
        }
        
        public void LoadLicenseData(int LicenseID)
        {
            _LicenseID = LicenseID;

            ResetData();

            txtLicenseID.Text = _LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadLicenseInfo(_LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;

            if (OnLicenseSelected != null && gbEnabled)
            {
                OnLicenseSelected(_LicenseID);
            }

        }
        private void btnFind_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                return;

            }

            _LicenseID = int.Parse(txtLicenseID.Text);

            LoadLicenseData(_LicenseID);
        }

        public void FocusFilter()
        {
            txtLicenseID.Focus();
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            clsValidation.KeyPressHandle(sender, e);

            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "this field is required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }
    }
}
