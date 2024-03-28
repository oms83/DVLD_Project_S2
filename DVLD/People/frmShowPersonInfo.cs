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
    public partial class frmShowPersonInfo : Form
    {
        private int _PersonID = -1;
        private string _NationalNo = "";

        private enum enShowPersonInfo { ByID, ByNationalNo }
        private enShowPersonInfo _ShowPersonInfo;
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            _ShowPersonInfo = enShowPersonInfo.ByID;
        }

        public frmShowPersonInfo(string NationalNo)
        {
            InitializeComponent();

            _NationalNo = NationalNo;

            _ShowPersonInfo = enShowPersonInfo.ByNationalNo;
        }
        private void frmShowPersonInfo_Load(object sender, EventArgs e)
        {
            switch (_ShowPersonInfo)
            {
                case enShowPersonInfo.ByID:
                    ucPersonInfo1.LoadPersonInfo(_PersonID); 
                    break;

                case enShowPersonInfo.ByNationalNo:
                    ucPersonInfo1.LoadPersonInfo(_NationalNo);
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
