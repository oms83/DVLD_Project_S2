using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddNewEdit : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack;

        private int _PersonID = -1;
        private clsPerson _Person;

        private enum enMode { AddNew, Update }
        private enMode _Mode = enMode.AddNew;

        public frmAddNewEdit(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            _Mode = enMode.Update;
        }

        public frmAddNewEdit()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        private void frmAddNewEdit_Load(object sender, EventArgs e)
        {
            _ResetData();

            if (_Mode == enMode.Update)
            {
                _FillPersonInfo();
            }
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow Country in dtCountries.Rows)
            {
                cbCountry.Items.Add(Country["CountryName"]);
            }
        }

        private void _ResetData()
        {
            _FillCountriesInComboBox();


            if (_Mode == enMode.AddNew)
            {
                // Add New Mode
                _Person = new clsPerson();

                lblMode.Text = "Add New Person";
            }
            else
            {
                lblMode.Text = "Update Person Info";
            }

            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtNationalNo.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtSecondName.Text = string.Empty;
            txtThirdName.Text = string.Empty;

            pbImage.Image = Resources.Male_512;
            pbGender.Image = Resources.Man_32;
            rbMale.Checked = true;

            llRemoveIamge.Visible = (pbImage.ImageLocation != null);


            //cbCountry.SelectedIndex = 179;
            cbCountry.SelectedIndex = cbCountry.FindString("Syria");

            lblPersonID.Text = "N/A";

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

        }

        private void _FillPersonInfo()
        {
            _Person = clsPerson.GetPersonInfoByPersonID(_PersonID);

            if (_Person == null)
            {
                _ResetData();

                MessageBox.Show($"No person with personID = {_PersonID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            _PersonID = _Person.PersonID;

            txtAddress.Text = _Person.Address.Trim();
            txtEmail.Text = _Person.Email.Trim();
            txtFirstName.Text = _Person.FirstName.Trim();
            txtLastName.Text = _Person.LastName.Trim();
            txtNationalNo.Text = _Person.NationalNo.Trim();
            txtPhone.Text = _Person.Phone.Trim();
            txtSecondName.Text = _Person.SecondName.Trim();
            txtThirdName.Text = _Person.ThirdName.Trim();
            dtpDateOfBirth.Value = _Person.DateOfBirth;


            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
            lblPersonID.Text = _PersonID.ToString().Trim();

            if (_Person.Gender == 0)
            {
                rbMale.Checked = true;
            }
            else
            {
                rbFemale.Checked = true;
            }

            if (_Person.ImagePath != "" && File.Exists(_Person.ImagePath))
            {

                pbImage.ImageLocation = _Person.ImagePath;
            }

            llRemoveIamge.Visible = (_Person.ImagePath != "");


        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
            {
                pbImage.Image = Resources.Male_512;
            }


        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
            {
                pbImage.Image = Resources.Female_512;
            }

        }

        private bool _HandlePersonImage()
        {

            if (_Person.ImagePath != pbImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException iox)
                    {

                    }
                }

                if (pbImage.ImageLocation != null)
                {
                    string SourceFile = pbImage.ImageLocation.ToString();

                    if (clsUtility.CopySourceFileToImagesFolder(ref SourceFile))
                    {
                        pbImage.ImageLocation = SourceFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }

                }
            }

            return true;
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

            if (!_HandlePersonImage())
            {
                return;
            }

            _Person.Address = txtAddress.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            if (pbImage.ImageLocation != null)
            {
                _Person.ImagePath = pbImage.ImageLocation.ToString();
            }
            else
            {
                _Person.ImagePath = "";
            }

            _Person.Gender = (byte)(rbMale.Checked ? 0 : 1);

            _Person.CountryID = clsCountry.GetCountryInfoByCountryName(cbCountry.Text).CountryID;

            if (_Person.Save())
            {
                _Mode = enMode.Update;
                lblMode.Text = "Update Person Info";

                _PersonID = _Person.PersonID;

                DataBack?.Invoke(this, _PersonID);

                lblPersonID.Text = _Person.PersonID.ToString();

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

        private void llRemoveIamge_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbImage.ImageLocation = null;

            if (rbMale.Checked)
                pbImage.Image = Resources.Male_512;
            else
                pbImage.Image = Resources.Female_512;

            llRemoveIamge.Visible = false;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process of selected file
                string ImagePath = openFileDialog1.FileName;

                pbImage.ImageLocation = ImagePath;

                llRemoveIamge.Visible = true;
                // ... 
            }
        }

        private void ValidationOfEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;

            if (string.IsNullOrEmpty(Temp.Text))
            {
                e.Cancel = true;
                Temp.Focus();
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExistByNationalNo(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                return;
            }

            if (!clsValidation.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email address!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
