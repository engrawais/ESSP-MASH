using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.ESSP
{
    public class VMESSPLeaveDetails : VAT_LeaveApplication
    {
        public List<VAT_LeaveApplicationFlow> DBVATLeaveApplicationFlow { get; set; }
        public string LMName { get; set; }





    }
}
