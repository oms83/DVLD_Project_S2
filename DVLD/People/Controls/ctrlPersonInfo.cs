using DVLD.Global;
using DVLD.Properties;
using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlPersonInfo : UserControl
    {

        private clsPerson _Person;
        private int _PersonID = -1;
        private string _NationalNo = "";

        public clsPerson Person
        {
            get => _Person;
        }
        public int PersonID
        {
            get => _PersonID;
        }
        public ctrlPersonInfo()
        {
            InitializeComponent();
        }

        public event Action<string> OnPersonEdited;
        public void ResetData()
        {
            _PersonID = -1;
            _Person = null;
            _NationalNo = string.Empty;

            lblFullName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblPersonID.Text = "[????]";
            lblEmail.Text = "[????]";
            lblCountry.Text = "[????]";
            lblDateOfBirth.Text = "[DD/MM/YYYY]";
            lblPhone.Text = "[????]";
            lblAddress.Text = "[????]";

            pbGender.Image = Resources.Man_32;
            lblGender.Text = "[????]";

            pbImage.Image = Resources.Male_512;
            pbImage.ImageLocation = "";

            llEditPerson.Enabled = false;
        }
        private void _LoadData()
        {
            lblFullName.Text = _Person.FullName.Trim();
            lblNationalNo.Text = _Person.NationalNo.Trim();
            lblPersonID.Text = _Person.PersonID.ToString().Trim();
            lblEmail.Text = _Person.Email.Trim();
            lblCountry.Text = _Person.CountryInfo.CountryName.Trim();
            lblDateOfBirth.Text = clsFormat.ShortDateFormat(_Person.DateOfBirth).Trim();
            lblPhone.Text = _Person.Phone.ToString().Trim();
            lblAddress.Text = _Person.Address.Trim();

            _LoadPersonImage();

            llEditPerson.Enabled = true;

        }

        private void _LoadPersonImage()
        {
            if (_Person.Gender == 0)
            {
                lblGender.Text = "Male";
                pbGender.Image = Resources.Man_32;

                pbImage.Image = Resources.Male_512;
            }
            else
            {
                lblGender.Text = "Female";
                pbGender.Image = Resources.Woman_32;

                pbImage.Image = Resources.Female_512;
            }

            if (_Person.ImagePath != "" && File.Exists(_Person.ImagePath))
            {
                pbImage.ImageLocation = _Person.ImagePath;
            }
            else
            {
                pbImage.ImageLocation = "";

                MessageBox.Show("Could not find this image: = " + _Person.ImagePath,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        public void LoadPersonInfo(int PersonID)
        {
            _PersonID = PersonID;

            _Person = clsPerson.GetPersonInfoByPersonID(_PersonID);

            if (_Person == null)
            {
                ResetData();

                MessageBox.Show($"No person with personID = {PersonID}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
                _LoadData();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _NationalNo = NationalNo;

            _Person = clsPerson.GetPersonByNationalNo(_NationalNo);


            if (_Person == null)
            {
                ResetData();

                MessageBox.Show($"No person with NationalNo = {NationalNo}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            _PersonID = Person.PersonID;

            _LoadData();
        }

        private void llEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNewEdit frm = new frmAddNewEdit(_PersonID);
            frm.ShowDialog();

            // Refresh data if person info has been edited
            LoadPersonInfo(_PersonID);

            if (OnPersonEdited != null)
            {
                OnPersonEdited(_Person.NationalNo);
            }
        }
    }
}
