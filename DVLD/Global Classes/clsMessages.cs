using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public class clsMessages
    {
        public static void ErrorMassege(string Body = "Data is not saved successfully", string Title = "Error")
        {
            MessageBox.Show(Body, Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void SuccessMassege(string Body = "Data Saved Successfully", string Title = "Successful")
        {
            MessageBox.Show(Body, Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void AlertMassege(string Body, string Title)
        {
            MessageBox.Show(Body, Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

    }
}
