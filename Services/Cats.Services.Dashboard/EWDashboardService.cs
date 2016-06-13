using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.Dashboard
{
  public  class EWDashboardService:IEWDashboardService
  {
      private IUnitOfWork _unitOfWork;
        public EWDashboardService(IUnitOfWork unitOfWork )
      {
          _unitOfWork = unitOfWork;

      }

        public List<Models.HRD> FindByHrd(System.Linq.Expressions.Expression<Func<Models.HRD, bool>> predicate)
        {
            return _unitOfWork.HRDRepository.FindBy(predicate);

        }

        public List<Models.RationDetail> FindByRationDetail(System.Linq.Expressions.Expression<Func<Models.RationDetail, bool>> predicate)
        {
            return _unitOfWork.RationDetailRepository.FindBy(predicate);
        }

        public List<Models.RegionalRequest> FindByRequest(System.Linq.Expressions.Expression<Func<Models.RegionalRequest, bool>> predicate)
        {
            return _unitOfWork.RegionalRequestRepository.FindBy(predicate);
        }
        public string GetStatusName(Models.Constant.WORKFLOW workflow, int statusId)
        {
            var workflowStatus =
                _unitOfWork.WorkflowStatusRepository.Get(t => t.WorkflowID == (int)workflow && t.StatusID == statusId).
                    FirstOrDefault();
            return workflowStatus != null ? workflowStatus.Description :
             string.Empty;
        }
        public List<Models.ReliefRequisition> FindByRequisition(System.Linq.Expressions.Expression<Func<Models.ReliefRequisition, bool>> predicate)
        {
            return _unitOfWork.ReliefRequisitionRepository.FindBy(predicate);
        }
        public List<ReliefRequisition> GetAllReliefRequisition()
        {
            return _unitOfWork.ReliefRequisitionRepository.GetAll();
        }

      public int GetRemainingRequest(int regionID, int planID)
      {
            var hrd = _unitOfWork.HRDRepository.FindBy(m => m.Status == 2).FirstOrDefault();
            var totalRequest = (from hrdDetail in hrd.HRDDetails
                                where hrdDetail.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID == regionID
                                select new
                                {

                                    hrdDetail.DurationOfAssistance
                                });

          var requested =
              _unitOfWork.RegionalRequestRepository.FindBy(
                  m => m.RegionID == regionID && m.PlanID == planID).Count;
          return (totalRequest.Max(m => m.DurationOfAssistance) - requested);
      }

      public int GetRemainingRequest(int regionID, int planID,int roundId)
        {
            var hrd = _unitOfWork.HRDRepository.FindBy(m => m.Status==2).FirstOrDefault();
             var totalRequest = (from hrdDetail in hrd.HRDDetails
                                  where hrdDetail.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID==regionID
                                 select new
                                 {
                                      
                                   hrdDetail.DurationOfAssistance
                                 });

            var requested =
                _unitOfWork.RegionalRequestRepository.FindBy(
                    m => m.RegionID == regionID && m.PlanID == planID && m.Round == roundId).Count;
            return (totalRequest.Max(m=>m.DurationOfAssistance) - requested);
        }
      public  List<Models.GiftCertificate> GetAllGiftCertificate()
      {
          return _unitOfWork.GiftCertificateRepository.GetAll();
      }
      public List<Dispatch> GetDispatches(List<string> requisitionNos)
      {
          return _unitOfWork.DispatchRepository.Get(m => requisitionNos.Contains(m.RequisitionNo)).ToList();
      }
     public List<WoredaStockDistribution> GetDistributions(int planID)
     {
         return _unitOfWork.WoredaStockDistributionRepository.Get(m => m.PlanID == planID).ToList();
     }
     public List<Delivery> GetDeliveries(List<Guid> dispatchIds)
     {
         return _unitOfWork.DeliveryRepository.Get(m => dispatchIds.Contains(m.DispatchID.Value)).ToList();
     }

      public int GetTotalRegionalRequest(int regionId, int planId)
      {
            var requested = _unitOfWork.RegionalRequestRepository.FindBy(m => m.RegionID == regionId && m.PlanID == planId).Count;
          return requested;
      }

      public int GetRegionalRequestSubmittedToLogistics(int regionId, int planId)
      {
          var requested =
              _unitOfWork.RegionalRequestRepository.FindBy(
                  m => m.RegionID == regionId && m.PlanID == planId && m.Status == 2).Count; //status = 2 submitted to finance
            return requested;
        }

      public int GetRegionalRequestSubmittedToLogistics(int regionId, int planId, int round)
      {
          var requested =
              _unitOfWork.RegionalRequestRepository.FindBy(
                  m => m.RegionID == regionId && m.PlanID == planId && m.Round == round && m.Status == 2).Count; //status = 2 submitted to finance
            return requested;
        }

      public void Dispose()
        {
            _unitOfWork.Dispose();
        }
        public List<int?> GetDistinctRounds(int planId)
        {
            return
              _unitOfWork.RegionalRequestRepository.Get(r => r.PlanID == planId)
                  .Select(p => p.Round)
                  .Distinct()
                  .ToList();
        }
    }
}
