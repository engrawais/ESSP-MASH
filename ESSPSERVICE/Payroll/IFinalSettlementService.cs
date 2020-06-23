using ESSPCORE.Common;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Payroll
{
    public interface IFinalSettlementService
    {
        void SaveFinalSettlementFlow(int? SubmittedTo, int? SubmittedBy, int Status, int FID);
    }
}
