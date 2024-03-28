using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Global
{
    public class clsFormat
    {
        public static string ShortDateFormat(DateTime dt)
        {
            return dt.ToString("dd/MMM/yyyy");
        }
    }
}
