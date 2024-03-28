using DVLD.Global;
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
using static DVLD_Business_Layer.clsApplication;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.Local_Driviing_Licenses
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        private enum enMode { AddNew, Update }
        private enMode _Mode;
        private int _PersonID = -1;
        private clsLocalDrivingLicenseApplication _LDLApp;

        int _LDLAppID = -1;
        public frmAddUpdateLocalDrivingLicenseApplication(int LDLAppID)
        {
            InitializeComponent();

            _LDLAppID = LDLAppID;

            _Mode = enMode.Update;
        }

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
         
            _Mode = enMode.AddNew;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetData();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dataTable = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                cmbLicenseClass.Items.Add(dataRow["ClassName"]);
            }
        }

        private void _ResetData()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                _LDLApp = new clsLocalDrivingLicenseApplication();
                lblMode.Text = "Add New L.D.L.App";
            }
            else
            {
                lblMode.Text = "Update L.D.L.App";
            }

            ctrlUserCardWithFilter1.ResetData();

            lblApplicationDate.Text = clsFormat.ShortDateFormat(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFees.Text = clsLicenseClass.GetLicenseClassInfoByID(3).ClassFees.ToString();
            lblLocalDrivingLicebseApplicationID.Text = "[????]";
            cmbLicenseClass.SelectedIndex = 2;

            tpApplicationInfo.Enabled = false;
        }

        private void _LoadData()
        {
            _LDLApp = clsLocalDrivingLicenseApplication.GetLDLAppInfoByLDLAppID(_LDLAppID);

            if (_LDLApp == null)
            {
                MessageBox.Show("No Application with LDLApp = " + _LDLAppID,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _LDLAppID = _LDLApp.LocalDrivingLicenseApplicationID;
            
            ctrlUserCardWithFilter1.LoadPersonInfo(_LDLApp.ApplicantPersonID);

            lblApplicationDate.Text = clsFormat.ShortDateFormat(_LDLApp.ApplicationDate);
            lblCreatedByUser.Text = clsUser.GetUserInfoUserID(_LDLApp.CreatedByUserID).UserName;
            lblFees.Text = _LDLApp.LicenseClassInfo.ClassFees.ToString();
            lblLocalDrivingLicebseApplicationID.Text = _LDLAppID.ToString();
            cmbLicenseClass.SelectedIndex = cmbLicenseClass.FindString(_LDLApp.LicenseClassInfo.ClassName);
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

            int LicenseClassID = clsLicenseClass.GetLicenseClassInfoByClassName(cmbLicenseClass.Text.Trim()).LicenseClassID;


            if (clsLocalDrivingLicenseApplication.DoesPersonHasAnActiveApplication(ctrlUserCardWithFilter1.PersonID, 
                                                                                   clsApplication.enApplicationType.NewLocalDrvingLicense,
                                                                                  LicenseClassID))
            {
                MessageBox.Show("You can not add a new application for {" + cmbLicenseClass.Text + "} license class, because person with ID = " + 
                                 ctrlUserCardWithFilter1.PersonID + " has an acitve application",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            if (clsLocalDrivingLicenseApplication.DoesPersonHasComplatedApplication(ctrlUserCardWithFilter1.PersonID,
                                                                                   clsApplication.enApplicationType.NewLocalDrvingLicense,
                                                                                   LicenseClassID))
            {
                MessageBox.Show("@You can not add a new application for {" + cmbLicenseClass.Text + "} license class, because person with ID = " + 
                                 ctrlUserCardWithFilter1.PersonID + " has a complated application",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(ctrlUserCardWithFilter1.PersonID,
                                                    LicenseClassID))
            {
                MessageBox.Show(@"You can not add a new application for {" + cmbLicenseClass.Text + "} license class, because person with ID = " + 
                                  ctrlUserCardWithFilter1.PersonID + " has a license for this license class",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            _LDLApp.ApplicationStatus = (byte)clsApplication.enApplicationStatus.New;
            _LDLApp.ApplicantPersonID = _PersonID;
            _LDLApp.ApplicationTypeID = clsApplication.enApplicationType.NewLocalDrvingLicense;
            _LDLApp.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LDLApp.ApplicationDate = DateTime.Now;
            _LDLApp.LastStatusDate = DateTime.Now;
            _LDLApp.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)_LDLApp.ApplicationTypeID).ApplicationFees;
            _LDLApp.LicenseClassID = LicenseClassID;

            if (_LDLApp.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
                
                MessageBox.Show("Data saved successfully",
                                "Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                _Mode = enMode.Update;

                lblMode.Text = "Update L.D.L.App";
            }
            else
            {
                MessageBox.Show("Data is not saved successfully",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void ctrlUserCardWithFilter1_OnPersonSelected(int PersonID)
        {
            _PersonID = PersonID;

            if (_PersonID == -1)
            {
                return;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                tpApplicationInfo.Enabled = true;

                tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
            }

            if (_Mode == enMode.AddNew)
            {
                if (_PersonID == -1)
                {
                    tpApplicationInfo.Enabled = false;

                    MessageBox.Show("You should add or select a person",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    tpApplicationInfo.Enabled = true;
                    tabControl1.SelectedTab = tabControl1.TabPages["tpApplicationInfo"];
                }
            }
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlUserCardWithFilter1.FilterFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblFees.Text = clsLicenseClass.GetLicenseClassInfoByID(cmbLicenseClass.SelectedIndex + 1).ClassFees.ToString();
            //lblFees.Text = clsLicenseClass.GetLicenseClassInfoByClassName(cmbLicenseClass.Text).ClassFees.ToString();
        }
    }
}
