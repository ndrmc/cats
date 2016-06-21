﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Common;
using Cats.Services.Dashboard;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Plan = Cats.Models.Partial.Plan;

namespace Cats.Areas.EarlyWarning.Controllers
{
    public class EWDashboardController : Controller
    {

        private readonly IEWDashboardService _eWDashboardService;
        private readonly IUserAccountService _userAccountService;
        private ICommonService _commonService;
        private IHRDDetailService _hrdDetailService;
        public EWDashboardController(IEWDashboardService ewDashboardService,
            IUserAccountService userAccountService,ICommonService commonService,
            IHRDDetailService hrdDetailService)
        {
            _eWDashboardService = ewDashboardService;
            _userAccountService = userAccountService;
            _commonService = commonService;
            _hrdDetailService = hrdDetailService;
        }

        public JsonResult GetRation()
        {
            var currentHrd = GetCurrentHrd();
                //_eWDashboardService.FindByHrd(
                //    m => m.Status == 3 || m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published")
                    //.FirstOrDefault();

            var rationDetail = _eWDashboardService.FindByRationDetail(m => m.RationID == currentHrd.RationID);
            var rationDetailInfo = GetRationDetailInfo(rationDetail);

            return Json(rationDetailInfo, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<RationDetailViewModel>  GetRationDetailInfo(IEnumerable<RationDetail> rationDetails)
        {
            return (from rationDetail in rationDetails
                    select new RationDetailViewModel
                        {
                            Commodity = rationDetail.Commodity.Name,
                            Amount = rationDetail.Amount
                        });
        }

        public JsonResult GetRegionalRequests()
        {

            var currentHrd = GetCurrentHrd();           
            var requests =
                _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID && m.ProgramId == 1)
                    .OrderByDescending(m => m.RegionalRequestID);       
            var requestDetail = GetRecentRegionalRequests(requests);

            return Json(requestDetail, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<RegionalRequestViewModel> GetRecentRegionalRequests(IEnumerable<RegionalRequest> regionalRequests)
        {
            return (from regionalRequest in regionalRequests
                where regionalRequest.ProgramId == 1
                // only for relief program
                // from requestDetail in regionalRequest.RegionalRequestDetails
                select new RegionalRequestViewModel()
                {
                    RegionalRequestID = regionalRequest.RegionalRequestID,
                    Region = regionalRequest.AdminUnit.Name,
                    Round = regionalRequest.Round,
                    MonthName = RequestHelper.MonthName(regionalRequest.Month),
                    StatusID = regionalRequest.Status,
                    Beneficiary = regionalRequest.RegionalRequestDetails.Sum(m => m.Beneficiaries),
                    NumberOfFDPS = regionalRequest.RegionalRequestDetails.Count(),
                    Status = _eWDashboardService.GetStatusName(WORKFLOW.REGIONAL_REQUEST, regionalRequest.Status)

                }); //.Take(10);
        }
        public JsonResult GetRequisition()
        {
            var currentHrd = GetCurrentHrd();         
            var requests =
                _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID)
                    .OrderByDescending(m => m.RegionalRequestID);
            var requisitions = GetRequisisition(requests);
            return Json(requisitions, JsonRequestBehavior.AllowGet);

        }
        private IEnumerable<ReliefRequisitionInfoViewModel> GetRequisisition(IEnumerable<RegionalRequest> requests)
        {
            var reliefRequisitions = _eWDashboardService.GetAllReliefRequisition();
            return (from reliefRequisition in reliefRequisitions
                    from request in requests
                    where reliefRequisition.RegionalRequestID == request.RegionalRequestID 
                    //&& reliefRequisition.Status==(int)ReliefRequisitionStatus.Draft 
                    select new ReliefRequisitionInfoViewModel
                        {
                            RequisitionID = reliefRequisition.RequisitionID ,
                            RequisitonNumber = reliefRequisition.RequisitionNo,
                            Round = reliefRequisition.Round ?? 0,
                            Region = reliefRequisition.AdminUnit.Name,
                            Zone = reliefRequisition.AdminUnit1.Name,
                            Commodity = reliefRequisition.Commodity.Name,
                            Beneficiary = reliefRequisition.ReliefRequisitionDetails.Sum(m=>m.BenficiaryNo),
                            Amount = reliefRequisition.ReliefRequisitionDetails.Sum(m=>m.Amount),
                            Status =_eWDashboardService.GetStatusName(WORKFLOW.RELIEF_REQUISITION,
                                                                  reliefRequisition.Status.Value)


                        }); //Take(8) removed
        }
        public JsonResult GetRequestedInfo()
        {
            var currentHrd = GetCurrentHrd();
            var request =
                _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID)
                    .OrderByDescending(m => m.RegionalRequestID);
            var requestDetail = GetRquestDetailViewModel(request);
            return Json(requestDetail, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<RegionalRequestInfoViewModel> GetRquestDetailViewModel(IEnumerable<RegionalRequest> regionalRequests)
        {
            var request = (from regionalRequest in regionalRequests
                           select new RegionalRequestInfoViewModel
                               {
                                   RegionID = regionalRequest.RegionID,
                                   RegionName = regionalRequest.AdminUnit.Name,
                                   NoOfRequests =
                                       _eWDashboardService.FindByRequest(m => m.RegionID == regionalRequest.RegionID
                                                                              && m.PlanID == regionalRequest.PlanID).
                               Count,
                                    Remaining = _eWDashboardService.GetRemainingRequest(regionalRequest.RegionID,regionalRequest.PlanID)


                               });
            var distinictRequest = request.GroupBy(m=>m.RegionID,(key, group) => group.First()).ToList();
            return distinictRequest;

        }
        public JsonResult GetStatusInPercentage()
        {
            var hrd = GetCurrentHrd();
            RequestPercentageViewModel percentage = null;
            var request = _eWDashboardService.FindByRequest(m => m.PlanID == hrd.PlanID);
            decimal draft = request.Count(m => m.Status == (int) RegionalRequestStatus.Draft);
            decimal approved = request.Count(m => m.Status == (int) RegionalRequestStatus.Approved);
            decimal closed = request.Count(m => m.Status == (int) RegionalRequestStatus.Closed);

            if (request.Count <1 )
            {
                percentage = new RequestPercentageViewModel();
                return Json(percentage, JsonRequestBehavior.AllowGet);   
            }
            percentage = new RequestPercentageViewModel
                {
                  
                    Pending = ((draft)/(request.Count))*100,
                    Approved = (approved/request.Count)*100,
                    RequisitionCreated = (closed/request.Count)*100
                };
            return Json(percentage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRequisitionStatusPercentage()
        {
            RequisitionStatusPercentage requisitionStatusPercentage = null;
            var currentHrd = GetCurrentHrd();
            var requests = _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID).OrderByDescending(m => m.RegionalRequestID);
            var allRequisitions = _eWDashboardService.GetAllReliefRequisition();

            var requisitons = (from requisiton in allRequisitions
                               from request in requests
                               where requisiton.RegionalRequestID == request.RegionalRequestID
                               select new
                                   {
                                       requisiton.Status
                                   }).ToList();

            decimal draft = requisitons.Count(m => m.Status == (int) ReliefRequisitionStatus.Draft);
            decimal approved = requisitons.Count(m => m.Status == (int)ReliefRequisitionStatus.Approved);
            decimal hubAssigned = requisitons.Count(m => m.Status == (int)ReliefRequisitionStatus.HubAssigned);
            decimal pcAssigned = requisitons.Count(m => m.Status == (int)ReliefRequisitionStatus.ProjectCodeAssigned);
            decimal transportRequisitionCreated = requisitons.Count(m => m.Status == (int)ReliefRequisitionStatus.TransportRequisitionCreated);
            decimal transportOrderCreated=requisitons.Count(m => m.Status == (int) ReliefRequisitionStatus.TransportOrderCreated);

            if (requisitons.Count < 1)
            {
                requisitionStatusPercentage = new RequisitionStatusPercentage();
                return Json(requisitionStatusPercentage, JsonRequestBehavior.AllowGet);
            }
             requisitionStatusPercentage = new RequisitionStatusPercentage
                {
                    Pending = (draft/requisitons.Count)*100,
                    Approved = (approved/requisitons.Count)*100,
                    HubAssigned = (hubAssigned/requisitons.Count)*100,
                    ProjectCodeAssigned = (pcAssigned/requisitons.Count)*100,
                    TransportRequistionCreated = (transportRequisitionCreated/requisitons.Count)*100,
                    TransportOrderCreated = (transportOrderCreated/requisitons.Count)*100,
                    NoOfDraft =(int) draft,
                    NoOfApproved = (int)approved,
                    NoHubAssigned = (int)hubAssigned,
                    NoOfPcAssigned = (int)pcAssigned,
                    NoOfTransportReqCreated = (int)transportRequisitionCreated,
                    NoOfTransportOrderCreated = (int)transportOrderCreated

                };
            return Json(requisitionStatusPercentage, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetHrdRegionPercentage()
        {
            var currentHrd = GetCurrentHrd();
            IEnumerable<RegionalTotalViewModel> regionalSummery = new List<RegionalTotalViewModel>(); 
            if (currentHrd != null)
            {

                var regionGroup = from detail in currentHrd.HRDDetails
                                     group detail by detail.AdminUnit.AdminUnit2.AdminUnit2
                                     into regionalDetail
                                     select new
                                         {
                                             Region = regionalDetail.Key,
                                             NumberOfBeneficiaries = regionalDetail.Sum(m => m.NumberOfBeneficiaries)     
                                         };
                regionalSummery= (from total in regionGroup
                        select new RegionalTotalViewModel
                        {
                            RegionName = total.Region.Name,
                            TotalBeneficary = total.NumberOfBeneficiaries,
                         
                        });
                decimal totalNationalBeneficiary = regionalSummery.Sum(m => m.TotalBeneficary);
                regionalSummery = (from regionalTotalViewModel in regionalSummery
                                   where regionalTotalViewModel.TotalBeneficary>0
                                   select new RegionalTotalViewModel
                                       {
                                           RegionName = regionalTotalViewModel.RegionName,
                                           TotalBeneficary = regionalTotalViewModel.TotalBeneficary,
                                           BeneficiaryPercentage =(regionalTotalViewModel.TotalBeneficary/totalNationalBeneficiary)*100

                                       }).OrderByDescending(m=>m.TotalBeneficary);


            }
            return Json(regionalSummery, JsonRequestBehavior.AllowGet);
        }
        private HRD GetCurrentHrd()
        {
            try
            {
                return
                    _eWDashboardService.FindByHrd(
                        m => m.Status == 3 || m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published")
                        .FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public JsonResult GetRecentGiftCertificates(DateTime startDate,DateTime endDate )
        {
            var draftGiftCertificate =
                from gs in
                    _eWDashboardService.GetAllGiftCertificate()
                        .Where(m => m.StatusID == 1)
                        .OrderByDescending(m => m.GiftCertificateID)
                where DateTime.Compare(gs.GiftDate, startDate) >= 0
                      && DateTime.Compare(gs.GiftDate, endDate) <= 0
                select gs;

            var giftCertificate = GetGiftCertificate(draftGiftCertificate);

            return Json(giftCertificate, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<GiftCertificateViewModel> GetGiftCertificate(IEnumerable<Cats.Models.GiftCertificate> giftCertificates)
        {
              var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            return (from giftCertificate in giftCertificates
                    select new GiftCertificateViewModel
                        {
                            GiftCertificateID=giftCertificate.GiftCertificateID,
                            DonorName = giftCertificate.Donor.Name,
                            SINumber = giftCertificate.ShippingInstruction.Value,
                            DclarationNumber = giftCertificate.DeclarationNumber,
                            Status = "Draft",
                            GiftDate = giftCertificate.GiftDate.ToCTSPreferedDateFormat(datePref),
                            Wieght = giftCertificate.GiftCertificateDetails.Sum(m=>m.WeightInMT),
                            EstimatedPrice = giftCertificate.GiftCertificateDetails.Sum(m=>m.EstimatedPrice),
                            TotalEstimatedTax = giftCertificate.GiftCertificateDetails.Sum(m=>m.EstimatedTax)

                            // Commodity = giftCertificate.GiftCertificateDetails.FirstOrDefault().Commodity.Name
                        }).OrderByDescending(o=>o.GiftDate); //take(5) Removed


        }
        public JsonResult GetEarlyWarningRequiredNumbers()
        {
            var currentHrd = GetCurrentHrd();
            var nationalBenficiaryNo = currentHrd.HRDDetails.Sum(m => m.NumberOfBeneficiaries);
             var requests = _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID);
            var requistions = _eWDashboardService.GetAllReliefRequisition();
            //var totalCommodity = currentHrd.Ration.RationDetails.Sum(m => m.Amount);
            var regions = (from item in currentHrd.HRDDetails
                           select new { item.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID }
                          ).Distinct().ToList();
            decimal total = 0;
            foreach (var region in regions)
            {
                foreach (var ration in GetCurrentHrd().Ration.RationDetails)
                {
                    var rationAmount = ration.Amount/1000; //todisplay in MT
                    var regionSum = currentHrd.HRDDetails.Where(t => t.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID == region.AdminUnitID).Sum(t => t.NumberOfBeneficiaries * t.DurationOfAssistance * rationAmount);

                    total += regionSum;

                }
            }
            var hrdAndRequestViewModel = new HrdAndRequestViewModel
                {
                    TotalHrdBeneficaryNumber = nationalBenficiaryNo,
                    HrdTotalCommodity = total,
                    TotalRequest = requests.Count,
                    TotalRequisitionNumber = (from requistion in requistions
                                                  from request in requests
                                                  where requistion.RegionalRequestID==request.RegionalRequestID
                                                  select new
                                                 {
                                                 requistion.RequisitionID
                                               }).Count()

                    //RequestedTotalBeneficaryNumber = requests.RegionalRequestDetails.Sum(m=>m.Beneficiaries)
                };
            return Json(hrdAndRequestViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentHrdStatistics()
        {
            var currentHrd = GetCurrentHrd();
            List<HrdTillDistributionViewModel> hrdTitllDistributionInfo =new List<HrdTillDistributionViewModel>();
            if (currentHrd!=null)
            {
                var requests = _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID);
                if (requests!=null)
                {
                    var requisitions = (from requisition in _eWDashboardService.GetAllReliefRequisition()
                                        from request in requests
                                        where requisition.RegionalRequestID == request.RegionalRequestID
                                        select new
                                            {
                                                requisitionID = requisition.RequisitionID,
                                                requisitionNo = requisition.RequisitionNo
                                            }).Select(m=>m.requisitionNo).ToList();
                    var dispatches = _eWDashboardService.GetDispatches(requisitions);



                    if (dispatches != null)
                    {
                        var groupedDispaches = (from dispatch in dispatches
                                                group dispatch by dispatch.FDP.AdminUnit.AdminUnit2.AdminUnit2
                                                into regionalDispatches
                                                select new
                                                    {
                                                        Region = regionalDispatches.Key,
                                                        dispatchAmount =
                                                    regionalDispatches.FirstOrDefault().DispatchDetails.Sum(
                                                        m => m.RequestedQuantityInMT)
                                                    });
                        var dispatchID = dispatches.Select(m => m.DispatchID).Distinct().ToList();
                        var deliveries = _eWDashboardService.GetDeliveries(dispatchID);

                        hrdTitllDistributionInfo = (from dispatch in groupedDispaches
                                                    select new HrdTillDistributionViewModel
                                                        {
                                                            Region = dispatch.Region.Name,
                                                            DispatchedAmount = dispatch.dispatchAmount,
                                                            DeliveredAmount = (from delivery in deliveries
                                                                               where delivery.FDP.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID==dispatch.Region.AdminUnitID
                                                                               group delivery by delivery.FDP.AdminUnit.AdminUnit2.AdminUnit2 into regionalDelievery
                                                                               select new
                                                                                    {
                                                                                        deliveredqty=dispatches.FirstOrDefault().DispatchDetails.Sum(m=>m.RequestedQuantityInMT)
                                                                                    }).Select(m=>m.deliveredqty).FirstOrDefault(),
                                                            DistributedAmount =
                                                                GetDistributionInfo(dispatch.Region.AdminUnitID)
                                                        }).ToList();


                    }

                }
            }
            return Json(hrdTitllDistributionInfo, JsonRequestBehavior.AllowGet);
        }
        private decimal GetDistributionInfo(int regionID)
        {
            var currentHrd = GetCurrentHrd();
            if (currentHrd !=null)
            {
                var distributions = _eWDashboardService.GetDistributions(currentHrd.PlanID);
                if (distributions!=null)
                {
                    var regionGrouped = (from distribution in distributions
                                         where distribution.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID == regionID
                                         group distribution by distribution.AdminUnit.AdminUnit2.AdminUnit2 into regionalDistribution
                                         select new
                                             {
                                                 Region = regionalDistribution.Key,
                                                 distributedAmount =
                                             regionalDistribution.FirstOrDefault().WoredaStockDistributionDetails.Sum(
                                                 m => m.DistributedAmount)
                                             }).Select(m => m.distributedAmount).FirstOrDefault();


                    return regionGrouped;

                }
                return 0;
            }
            return 0;
        }

      
        public JsonResult GetRegionalRequestDataEntryStatus()
        {
            var currentHrd = GetCurrentHrd();
            var rationDetail = _eWDashboardService.FindByRationDetail(m => m.RationID == currentHrd.RationID);
            var numberOfCommodities = rationDetail.Count();
            var regions = _commonService.GetAminUnits(t => t.AdminUnitTypeID == 2).OrderBy(t=>t.Name).ToList();
            var regionalRequestDataEntryStatus = new List<RegionalRequestDataEntryStatusViewModel>();
            var rounds = _eWDashboardService.GetDistinctRounds(currentHrd.PlanID);
            foreach (var region in regions)
            {
                foreach (int round in rounds)
                {
                    var numberOfZones = _commonService.GetAminUnits(p => p.ParentID == region.AdminUnitID).Count();
                    var completed =
                        _eWDashboardService.GetRegionalRequestSubmittedToLogistics(region.AdminUnitID, currentHrd.PlanID,
                            round);
                    var allocated = _eWDashboardService.GetRemainingRequest(region.AdminUnitID, currentHrd.PlanID,round);
                    int expected = numberOfCommodities*numberOfZones;
                    var ratio = expected != 0 ? completed/Convert.ToDouble(expected) : 0;
                    var progress = ratio*100.0;
                    var regionalDataEntryStatus = new RegionalRequestDataEntryStatusViewModel
                    {
                        Region = region.Name,
                        TotalRequested = expected,
                        Allocated = allocated,
                        AllocationProgress = Convert.ToDecimal(Math.Round(progress, 2)),
                        Completed = completed,
                        Round = (int) round,
                    };
                    regionalRequestDataEntryStatus.Add(regionalDataEntryStatus);
                }
            }
            return Json(regionalRequestDataEntryStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHRDDataEntryStatus()
        {
            var currentHrd = GetCurrentHrd();
            var regions = _commonService.GetAminUnits(t => t.AdminUnitTypeID == 2).OrderBy(t => t.Name).ToList();
            var hrdDataEntryStatus = new List<RegionalRequestDataEntryStatusViewModel>();

            foreach (var region in regions)
            {
                var zonesId =
                    _commonService.GetAminUnits(p => p.ParentID == region.AdminUnitID)
                        .Select(z => z.AdminUnitID)
                        .ToList();
                var woredaIds = _commonService.GetAminUnits(p => zonesId.Contains((int) p.ParentID))
                    .Select(z => z.AdminUnitID)
                    .ToList();
                var hrdDetails =
                    _hrdDetailService.FindBy(h => h.HRDID == currentHrd.HRDID && woredaIds.Contains(h.WoredaID));
                var woredaCounts = hrdDetails.Count;
                var woredasWithBeneficiary = hrdDetails.Count(w => w.NumberOfBeneficiaries > 0);
                var progress = woredaCounts != 0 ? (woredasWithBeneficiary/woredaCounts)*100.0 : 0;
                var dataEntryStatus = new RegionalRequestDataEntryStatusViewModel
                {
                    Region = region.Name,
                    TotalRequested = hrdDetails.Count,
                    AllocationProgress = Convert.ToDecimal(Math.Round(progress, 2)),
                    Completed = woredasWithBeneficiary,
                };
                hrdDataEntryStatus.Add(dataEntryStatus);
            }
            return Json(hrdDataEntryStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
