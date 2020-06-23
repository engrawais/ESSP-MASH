using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.Attendance
{

    public class VMJobCardCreate : VMEmpSelection
    {
        public DateTime JobDateFrom { get; set; }
        public DateTime JobDateTo { get; set; }
        public Int16 JobCardTypeID { get; set; }
        public string JobCardTypeName { get; set; }
        public short? TotalMinutes { get; set; }
        public string Remarks { get; set; }

    }
}
