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
    public partial class ctrlUserCardWithFilter : UserControl
    {
        private int _PersonID = -1;

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int PersonID)
        {

            // Create a protected method to raise the event with a parameter
            Action<int> handler = OnPersonSelected;

            if (handler != null)
            {
                handler(PersonID);  // Raise the event with the parameter
            }
        }

        public clsPerson SelectedPersonInfo
        {
            get => ucPersonInfo1.Person;
        }

        public int PersonID
        {
            get => ucPersonInfo1.PersonID;
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get => _FilterEnabled;
            set
            {
                _FilterEnabled = value;
                txtFilterBy.Enabled = _FilterEnabled;
            }
        }

        private bool _ShowAddPerson = true;
        
        public bool ShowAddPerson
        {
            get => _ShowAddPerson;
            set
            {
                _ShowAddPerson = value;
                btnAddNew.Visible = _ShowAddPerson;
            }
        }

        public void FilterFocus()
        {
            txtFilterBy.Focus();
        }

        public ctrlUserCardWithFilter()
        {
            InitializeComponent();
        }

        public void ResetData()
        {
            ucPersonInfo1.ResetData();

            txtFilterBy.Text = string.Empty;
            cmbFilterBy.SelectedIndex = 0;
        }
        private void _FindNow()
        {
            switch (cmbFilterBy.Text.Trim())
            {
                case "Person ID":
                    
                    ucPersonInfo1.LoadPersonInfo(int.Parse(txtFilterBy.Text));
                    
                    break;


                case "National No.":
                    
                    ucPersonInfo1.LoadPersonInfo(txtFilterBy.Text.Trim());
                    
                    break;

                default:
                    break;
            }
            
            if (OnPersonSelected != null && _FilterEnabled)
            {
                OnPersonSelected(ucPersonInfo1.PersonID);
            }
        }

        public void LoadPersonInfo(int PersonID)
        {
            cmbFilterBy.SelectedIndex = 1;
            txtFilterBy.Text = PersonID.ToString();

            _FindNow();
            
            gbFilterBy.Enabled = false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are invalid, Please put the mouse over the red icon(s) to see the error",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }


            _FindNow();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddNewEdit frm = new frmAddNewEdit();

            frm.DataBack += DataBackEvent;
            
            frm.ShowDialog();
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            
            ucPersonInfo1.LoadPersonInfo(PersonID);

            _PersonID = PersonID;

            cmbFilterBy.SelectedIndex = 1;
            
            txtFilterBy.Text = _PersonID.ToString();
            
            gbFilterBy.Enabled = false;

        }
        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }

            // This will allow digits only when person id is selected
            if (cmbFilterBy.Text == "Person ID")
            {
                //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

                clsValidation.KeyPressHandle(sender, e);
            }
        }

        private void ctrlUserCardWithFilter_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
            txtFilterBy.Focus();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Text = "";
            txtFilterBy.Focus();
        }

        private void txtFilterBy_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterBy.Text.Trim()))
            {
                e.Cancel = true;
                txtFilterBy.Focus();
                errorProvider1.SetError(txtFilterBy, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilterBy, null);
            }
        }

        private void ucPersonInfo1_OnPersonEdited(string obj)
        {
            txtFilterBy.Text = obj;
        }
    }
}
