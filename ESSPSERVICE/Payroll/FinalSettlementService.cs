using ESSPCORE.Common;
using ESSPCORE.EF;
using ESSPREPO.Generic;
using ESSPSERVICE.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Payroll
{
    public class FinalSettlementService : IFinalSettlementService
    {
        IDDService DDService;
        IRepository<FinalSettlement> FinalSettlementRepository;
        IRepository<FinalSettelmentFlow> FinalSettelmentFlowRepository;
        IUnitOfWork UnitOfWork;
        public FinalSettlementService(IUnitOfWork unitOfWork, IDDService dDService, IRepository<FinalSettlement> finalSettlementRepository
            , IRepository<FinalSettelmentFlow> finalSettelmentFlowRepository)
        {
            UnitOfWork = unitOfWork;
            DDService = dDService;
            FinalSettlementRepository = finalSettlementRepository;
            FinalSettelmentFlowRepository = finalSettelmentFlowRepository;
        }

        public void SaveFinalSettlementFlow(int? SubmittedTo, int? SubmittedBy, int Status, int FID)
        {
            FinalSettelmentFlow dbFinalSettelmentFlow = new FinalSettelmentFlow();
            dbFinalSettelmentFlow.SubmittedDateTime = DateTime.Now;
            dbFinalSettelmentFlow.EmpID = FID;
            dbFinalSettelmentFlow.FSStatusID = Status;
            dbFinalSettelmentFlow.SubmittedBy = SubmittedBy;
            dbFinalSettelmentFlow.SubmittedTo = SubmittedTo;
            FinalSettelmentFlowRepository.Add(dbFinalSettelmentFlow);
            FinalSettelmentFlowRepository.Save();
        }
    }
}
