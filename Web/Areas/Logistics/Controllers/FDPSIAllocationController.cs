using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.EarlyWarning;
using Cats.Services.Transaction;
using Cats.ViewModelBinder;
using log4net;
using Cats.Services.Common;
using Cats.Areas.Logistics.Models;
using Cats.Services.Logistics;
using Cats.Helpers;
using FluentMigrator.Expressions;
using Cats.Services.Hubs;

namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class FDPSIAllocationController : Controller
    {
        private readonly IReliefRequisitionService _requisitionService;
        private readonly ILedgerService _ledgerService;
        private readonly IHubAllocationService _hubAllocationService;
        private readonly IHubService _hubService;
        private readonly ISIPCAllocationService _allocationService;
        private readonly ITransactionService _transactionService;
        private readonly ICommonService _commonService;
        private readonly IReliefRequisitionDetailService _requisitionDetailService;
        private readonly IBusinessProcessService _businessProcessService;

        public FDPSIAllocationController
            (
             IReliefRequisitionService requisitionService
            , ILedgerService ledgerService
            , IHubAllocationService hubAllocationService, IHubService hubService
            , ISIPCAllocationService allocationService
            , ITransactionService transactionService, ICommonService commonService, IReliefRequisitionDetailService requisitionDetailService, IBusinessProcessService businessProcessService)
        {
                this._requisitionService = requisitionService;
                this._ledgerService = ledgerService;
                this._hubAllocationService = hubAllocationService;
            this._hubService = hubService;
            this._allocationService = allocationService;
            this._transactionService = transactionService;
            _commonService = commonService;
            _requisitionDetailService = requisitionDetailService;
			_businessProcessService = businessProcessService;
            }

        public List<RequestAllocationViewModel> getIndexList(int regionId = 0, int RequisitionID = 0)
        {
            List<ReliefRequisition> req;// = _requisitionService.FindBy(r => r.RegionID == regionId && r.Status == (int)ReliefRequisitionStatus.HubAssigned);
           if (RequisitionID > 0)
            {
                req = _requisitionService.Get(r => r.RequisitionID==RequisitionID && r.RegionID == regionId && r.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Hub Assigned", null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").ToList();
            }
            else
            {
               req = _requisitionService.Get(r => r.RegionID == regionId && r.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Hub Assigned", null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").ToList();

            }
            var result = req.ToList().Select(item => new RequestAllocationViewModel
            {
                Commodity = item.Commodity.Name,
                RegionName = item.AdminUnit.Name,
                ZoneName = item.AdminUnit1.Name,
                RequisitionId = item.RequisitionID,
                CommodityId = (int)item.CommodityID,
                Amount = item.ReliefRequisitionDetails.Sum(a => a.Amount).ToPreferedWeightUnit(),
                HubAllocationID = item.HubAllocations.ToList()[0].HubAllocationID,
                HubName = item.HubAllocations.ToList()[0].Hub.Name,
           		//     SIAllocations=item.ReliefRequisitionDetails.RegionalRequestDetails.First().Fdpid
                //                AllocatedAmount = geteAllocatedAmount(item),
                //               SIAllocations = getSIAllocations(item.HubAllocations.ToList()[0].ProjectCodeAllocations.ToList()),
                FDPRequests = getRequestDetail(item),
                FreeSIPCCodes = getSIPCLists(item.RequisitionID, (int)item.CommodityID),

            }).ToList();

            return result;
        }
        public List<FDPRequestViewModel> getRequestDetail(ReliefRequisition Request)
        {

            var result = Request.ReliefRequisitionDetails.ToList().Select(item => new FDPRequestViewModel
            {
                RequisitionId = Request.RequisitionID,
                RequestDetailId = item.RequisitionDetailID,
                FDPId = item.FDPID,
                FDPName = item.FDP.Name,
                Name = item.FDP.Name,
                RequestedAmount = item.Amount.ToPreferedWeightUnit(),
                WoredaId = item.FDP.AdminUnitID,
                WoredaName = item.FDP.AdminUnit.Name,
                Commodity = item.Commodity.Name,
                Allocations = ToAllocationViewModel(item.SIPCAllocations.ToList())
            }).ToList();
            return result;
        }
        public List<SIPCAllocationViewModel> ToAllocationViewModel(List<SIPCAllocation> allocation)
        {
            var result = allocation.Select(item => new SIPCAllocationViewModel
            {
                SIPCAllocationID = item.SIPCAllocationID,
                RequisitionDetailID = item.RequisitionDetailID,
                Code = item.Code,
                AllocatedAmount = item.AllocatedAmount.ToPreferedWeightUnit(),
                AllocationType = item.AllocationType,
                FDPID = item.FDPID

            }).ToList();
            return result;
        }
        public FreeSIPC getSIPCLists(int reqId, int CommodityID)
        {
            var hubId = _hubAllocationService.GetAllocatedHubId(reqId);
            List<LedgerService.AvailableShippingCodes> freeSICodes = _ledgerService.GetFreeSICodesByCommodity(hubId, CommodityID);
            List<LedgerService.AvailableProjectCodes> freePCCodes = _ledgerService.GetFreePCCodesByCommodity(hubId, CommodityID);
            foreach (var fsi in freeSICodes)
            {
                fsi.amount = fsi.amount.ToPreferedWeightUnit();
            }
            foreach (var fpc in freePCCodes)
            {
                fpc.amount = fpc.amount.ToPreferedWeightUnit();
            }
            FreeSIPC free = new FreeSIPC { FreePCCodes = freePCCodes, FreeSICodes = freeSICodes };
            return free;
        }
        public ActionResult Index(int regionId = 0, int RequisitionID = 0)
        {
            ViewBag.regionId = regionId;
            ViewBag.RequisitionID = RequisitionID;
            ViewBag.Hubs = _hubService.GetAllHub();
           // ViewBag.Hubs = _commonService.GetHubsAndStores();
            ViewBag.AllocatedHub = _hubAllocationService.GetAllocatedHubId(RequisitionID);
            //ViewBag.Allocations = _hubAllocationService.GetAllHubAllocation().Select(m => m.HubID);
            return View();
        }

        public ActionResult getRequisitionList(int regionId = 0, int RequisitionID = 0)
        {
            List<RequestAllocationViewModel> list = getIndexList(regionId, RequisitionID);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
                public ActionResult UpdateAllocation(List<SIPCAllocationViewModel> allocations)
        {
            //SIPCAllocation allocation2 = _allocationService.FindById(6);
            // int RequisitionID = 1;
            // change the requistion detail based on allocation 



            List<FDPRequestViewModel> RequisitionDetailList = null;
            if (allocations.Count == 1 && allocations[0].AllocationType == "no" && allocations[0].AllocatedAmount == 0)
            {
                var requisitionId = allocations[0].RequisitionDetailID; // RequisitionDetailID is used to send requisitionId 
                var tobeDeleted =
                    _allocationService.FindBy(t => t.ReliefRequisitionDetail.RequisitionID == requisitionId);
                foreach (var deletedSI in tobeDeleted)
                {
                    _allocationService.Delete(deletedSI);
                }
            }
            else
            {
                var sipcAllocationViewModel = allocations.FirstOrDefault();
                var temp = sipcAllocationViewModel.RequisitionDetailID;
                var tempRequisitionID = _requisitionDetailService.FindById(temp).RequisitionID;
                var tobeDeleted =
                    _allocationService.FindBy(t => t.ReliefRequisitionDetail.RequisitionID == tempRequisitionID);
                foreach (var deletedSI in tobeDeleted)
                {
                    _allocationService.Delete(deletedSI);
                }




                foreach (SIPCAllocationViewModel item in allocations)
                {
                    //var tempAllocation =
                    //    _allocationService
                    //        .GetAll(
                    //            ).FirstOrDefault(t => t.FDPID == item.FDPID && t.Code == item.Code &&
                    //                t.RequisitionDetailID == item.RequisitionDetailID);
                    //if (item.SIPCAllocationID > 0)
                    //{
                    //    SIPCAllocation allocation = _allocationService.FindById(item.SIPCAllocationID);
                    //    if (item.AllocatedAmount == 0)
                    //    {
                    //        _allocationService.Delete(allocation);
                    //    }
                    //    else
                    //    {
                    //        allocation.Code = item.Code;
                    //        allocation.AllocatedAmount = Math.Round(item.AllocatedAmount, 2);
                    //        allocation.AllocationType = item.AllocationType;
                    //        allocation.FDPID = item.FDPID;
                    //        _allocationService.Update(allocation);
                    //    }
                    //}
                    //else if (tempAllocation != null)
                    //{
                    //    SIPCAllocation allocation = tempAllocation;
                    //    if (item.AllocatedAmount == 0)
                    //    {
                    //        _allocationService.Delete(allocation);
                    //    }
                    //    else
                    //    {
                    //        allocation.Code = item.Code;
                    //        allocation.AllocatedAmount = Math.Round(item.AllocatedAmount, 2);
                    //        allocation.AllocationType = item.AllocationType;
                    //        allocation.FDPID = item.FDPID;
                    //        _allocationService.Update(allocation);
                    //    }
                    //}
                    //else
                    //{
                    if (item.AllocatedAmount > 0)
                    {
                        SIPCAllocation allocation = new SIPCAllocation
                        {
                            Code = item.Code,
                            AllocatedAmount = item.AllocatedAmount.ToPreferedWeightUnitForInsert(),
                            AllocationType = item.AllocationType,
                            RequisitionDetailID = item.RequisitionDetailID,
                            HubID = item.HubID,
                            FDPID = item.FDPID
                        };
                        _allocationService.Create(allocation);
                    }
                    //}
                }
            }
            //List<RequestAllocationViewModel> list = getIndexList(regionId, RequisitionID);
            return Json(allocations, JsonRequestBehavior.AllowGet);
        }

        private int checkChangeAndUpdate(int RequisitionDetailID, List<FDPRequestViewModel> RequestList, decimal AllocatedAmount)
        {

            int flag = 0;
            for (int i = 0; i < RequestList.Count; i++)
            {
                if (RequestList[i].RequestDetailId == RequisitionDetailID &&
                    RequestList[i].RequestedAmount != AllocatedAmount)
                {

                }

            }
            return 0;
        }
        public JsonResult updateRequisitionStatus(int RequisitionId)
{

	ReliefRequisition req = _requisitionService.FindById(RequisitionId);
	var tnx = _transactionService.PostSIAllocation(RequisitionId);
	if (tnx)
	{
		var requisition = _requisitionService.Get(t => t.RequisitionID == RequisitionId, null,
				"BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
		if (requisition != null)
		{
			var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Assign Project Code");
			if (approveFlowTemplate != null)
			{
				var businessProcessState = new BusinessProcessState()
				{
					StateID = approveFlowTemplate.FinalStateID,
					PerformedBy = HttpContext.User.Identity.Name,
					DatePerformed = DateTime.Now,
					Comment = "Requisition has been assigned SI/PC",
					//AttachmentFile = fileName,
					ParentBusinessProcessID = requisition.BusinessProcessId
				};
				//return 
				_businessProcessService.PromotWorkflow(businessProcessState);
			}
		}
	}
	List<RequestAllocationViewModel> list = getIndexList((int)req.RegionID, RequisitionId);
	/*
	List<RequestAllocationViewModel> list = new List<RequestAllocationViewModel> ();*/
	return Json(list, JsonRequestBehavior.AllowGet);

}


    }
}
