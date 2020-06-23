using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPCORE.ESSP
{
    public class VMESSPJobCardDetail : VEP_JobCardApplication
    {
        public List<VAT_JobCardFlow> DBVATjobcardflow { get; set; }
        public string LMName { get; set; }
        public string SubmittedBy { get; set; }
    }
}
