﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.Logistics.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.Hub;
using Cats.Services.Logistics;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.ViewModelBinder;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using IAdminUnitService = Cats.Services.Administration.IAdminUnitService;
using ICommonService = Cats.Services.Common.ICommonService;
using IProgramService = Cats.Services.EarlyWarning.IProgramService;
using ITransactionService = Cats.Services.Transaction.ITransactionService;

namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class WoredaStockDistributionController : Controller
    {

        private readonly IUtilizationHeaderSerivce _utilizationService;
        private readonly IUtilizationDetailSerivce _utilizationDetailSerivce;
        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly UserAccountService _userAccountService;
        private readonly ICommonService _commonService;
        private readonly IRegionalRequestService _regionalRequestService;
        private readonly IReliefRequisitionDetailService _reliefRequisitionDetailService;
        private readonly ITransactionService _transactionService;
        private readonly IDispatchService _dispatchService;
        private readonly IDeliveryService _deliveryService;
        private readonly IProgramService _programService;
        private readonly ILossReasonService _lossReasonService;
        private readonly IPlanService _planService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly IDeliveryReconcileService _deliveryReconcileService;

        public WoredaStockDistributionController(
            IUtilizationHeaderSerivce utilizationService,
            IProgramService programService,
            IUtilizationDetailSerivce utilizationDetailSerivce,
            UserAccountService userAccountService,
            ICommonService commonService,
            IRegionalRequestService regionalRequestService,
            IReliefRequisitionDetailService reliefRequisitionDetailService,
            IReliefRequisitionService reliefRequisitionService,

            ITransactionService transactionService, IDispatchService dispatchService, IDeliveryService deliveryService,
            ILossReasonService lossReasonService, IPlanService planService, IAdminUnitService adminUnitService,
            IDeliveryReconcileService deliveryReconcileService)
        {
            _utilizationService = utilizationService;
            _programService = programService;
            _utilizationDetailSerivce = utilizationDetailSerivce;
            _userAccountService = userAccountService;
            _commonService = commonService;
            _regionalRequestService = regionalRequestService;
            _reliefRequisitionDetailService = reliefRequisitionDetailService;
            _reliefRequisitionService = reliefRequisitionService;
            _transactionService = transactionService;
            _dispatchService = dispatchService;
            _deliveryService = deliveryService;
            _lossReasonService = lossReasonService;
            _planService = planService;
            _adminUnitService = adminUnitService;
            _deliveryReconcileService = deliveryReconcileService;
        }

        //
        // GET: /Logistics/Utilization/

        public ActionResult Index()
        {

            ViewBag.RegionCollection = _commonService.GetAminUnits(t => t.AdminUnitTypeID == 2);

            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms().Take(2), "ProgramId", "Name");
            LookUps();
            if (ViewBag.Errors == "errors")
                ModelState.AddModelError("Errors", @"Please Select values from the Left Side");
            return View();
        }


        private void selectedLookUps(int programId, int planId, int woredaId, int month)
        {
            var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;

            if (userRegionID != null)
            {
                var adminUnit = _commonService.FindBy(m => m.AdminUnitID == userRegionID).FirstOrDefault();
                if (adminUnit != null)
                    ViewBag.SelectedRegion = adminUnit.Name;
            }




            var unit = _commonService.FindBy(a => a.AdminUnitID == woredaId).FirstOrDefault();
            if (unit != null)
            {
                ViewBag.SelectedWoreda = unit.Name;
                var adminUnit = _adminUnitService.FindBy(f => f.AdminUnitID == unit.ParentID).FirstOrDefault();
                if (adminUnit != null)
                {
                    var orDefault = adminUnit.Name;
                    ViewBag.SelectedZone = orDefault;
                }
            }
            var firstOrDefault = _programService.FindBy(p => p.ProgramID == programId).FirstOrDefault();
            if (firstOrDefault != null)
                ViewBag.SelectedProgram = firstOrDefault.Name;

            var @default = _planService.FindBy(p => p.PlanID == planId).FirstOrDefault();
            if (@default != null)
                ViewBag.SelectedPlan = @default.PlanName;
            ViewBag.selectedmonth = month;
        }

        public ActionResult Create(int Woreda = -1, int planID = -1, int programID = -1, int month = -1)
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError("Success", TempData["CustomError"].ToString());
            }

            else if (TempData["CustomError2"] != null)
            {
                ModelState.AddModelError("Errors", TempData["CustomError2"].ToString());
            }

            if (Woreda == -1 || planID == -1 || programID == -1 || month == -1)
            {

                LookUps();
                ViewBag.Errors = "errors";

                return RedirectToAction("Index");

            }



            var woredaStockDistributionViewModel = new WoredaStockDistributionWithDetailViewModel();
            //woredaStockDistributionViewModel.WoredaDistributionDetailViewModels
            var woredaStockDistribution = CheckWoredaDistribution(Woreda, planID, month, programID);
            if (woredaStockDistribution != null)
            {
                selectedLookUps(programID, planID, Woreda, month);
                LookUps(woredaStockDistribution);
                woredaStockDistributionViewModel = woredaStockDistribution;
                woredaStockDistributionViewModel.PlanID = planID;
                woredaStockDistributionViewModel.WoredaID = Woreda;
                woredaStockDistributionViewModel.Month = month;
                woredaStockDistributionViewModel.ProgramID = programID;
                woredaStockDistributionViewModel.ActualBeneficairies = getWoredaBeneficiaryNoFromHRD(planID, Woreda, month, programID);

                return View(woredaStockDistributionViewModel);
            }
            //ModelState.AddModelError("Errora",@"Request is Not Created for this plan");
            LookUps();

            //ViewBag.WoredaName =_commonService.GetAminUnits(m => m.AdminUnitID == woredaStockDistribution.WoredaID).FirstOrDefault().Name;
            var distributionDetail = _commonService.GetFDPs(Woreda);
            //var listOfFdps = GetWoredaStockDistribution(distributionDetail);
            //woredaStockDistributionViewModel.WoredaDistributionDetailViewModels = listOfFdps;
            woredaStockDistributionViewModel.PlanID = planID;
            woredaStockDistributionViewModel.WoredaID = Woreda;
            woredaStockDistributionViewModel.Month = month;
            woredaStockDistributionViewModel.ProgramID = programID;
            woredaStockDistributionViewModel.ActualBeneficairies = getWoredaBeneficiaryNoFromHRD(planID, Woreda, month, programID);

            return View(woredaStockDistributionViewModel);
        }
        private int getWoredaBeneficiaryNoFromHRD(int planId, int woredaId, int roundOrMonth, int program)
        {
            return _commonService.GetWoredaBeneficiaryNo(planId, woredaId, roundOrMonth, program);
        }
        public WoredaStockDistributionWithDetailViewModel CheckWoredaDistribution(int woredaID = -1, int planID = -1, int month = -1, int programId = -1)
        {
            if (woredaID == -1 || planID == -1 || month == -1 || programId == -1)
                return null;

            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;

            //var woredaStockDistribution = _utilizationService.FindBy(m => m.WoredaID == woredaID && m.Month == month && m.PlanID == planID).FirstOrDefault();
            var zoneID = _commonService.GetZoneID(woredaID);
            var regionID = _commonService.GetRegion(zoneID);
            RegionalRequest regionalRequest = null;
            if (programId == (int)Programs.Releif)
            {
                regionalRequest =
                    _regionalRequestService.FindBy(m => m.PlanID == planID && m.Round == month && m.RegionID == regionID)
                        .FirstOrDefault();
            }
            else if (programId == (int)Programs.PSNP)
            {
                regionalRequest =
                    _regionalRequestService.FindBy(m => m.PlanID == planID && m.Month == month && m.RegionID == regionID)
                        .FirstOrDefault();
            }
            if (regionalRequest == null) return null;
            var requisition =
                _reliefRequisitionService.FindBy(
                    m => m.RegionalRequestID == regionalRequest.RegionalRequestID && m.ZoneID == zoneID);

            if (requisition == null) return null;
            var woredaStockDistribution =
                _utilizationService.FindBy(m => m.WoredaID == woredaID && m.Month == month && m.PlanID == planID)
                    .FirstOrDefault();

            var fdpStockDistribution = _commonService.GetFDPs(woredaID);
            if (woredaStockDistribution == null)
            {
                var woredaDistributionDetailViewModels = new List<WoredaDistributionDetailViewModel>();
                foreach (var reliefRequisition in requisition)
                {
                    var detail = GetWoredaStockDistribution(fdpStockDistribution, reliefRequisition);
                    if (detail != null)
                    {
                        woredaDistributionDetailViewModels.AddRange(detail);
                    }
                }
                var listOfFdps = new WoredaStockDistributionWithDetailViewModel
                {
                    WoredaDistributionDetailViewModels = woredaDistributionDetailViewModels,

                };
                return listOfFdps;
            }

            var woredaStockDistributionWithDetailViewModel = new WoredaStockDistributionWithDetailViewModel()
            {
                WoredaStockDistributionID = woredaStockDistribution.WoredaStockDistributionID,
                WoredaID = woredaStockDistribution.WoredaID,
                ProgramID = woredaStockDistribution.ProgramID,
                PlanID = woredaStockDistribution.PlanID,
                Month = woredaStockDistribution.WoredaStockDistributionID,
                DirectSupport = woredaStockDistribution.DirectSupport,
                PublicSupport = woredaStockDistribution.PublicSupport,
                ActualBeneficairies = woredaStockDistribution.ActualBeneficairies,
                MaleBetween5And18Years = woredaStockDistribution.MaleBetween5And18Years,
                FemaleLessThan5Years = woredaStockDistribution.FemaleLessThan5Years,
                MaleAbove18Years = woredaStockDistribution.MaleAbove18Years,
                MaleLessThan5Years = woredaStockDistribution.MaleLessThan5Years,
                FemaleAbove18Years = woredaStockDistribution.FemaleAbove18Years,
                FemaleBetween5And18Years = woredaStockDistribution.FemaleBetween5And18Years,
                WoredaDistributionDetailViewModels =
                    (from woredaDistributionDetail in woredaStockDistribution.WoredaStockDistributionDetails
                     from reliefRequisition in requisition
                     where woredaDistributionDetail.CommodityID == reliefRequisition.CommodityID
                     let lossReason = woredaDistributionDetail.LossReason
                     //let reliefRequisition = _reliefRequisitionService.Get(r=>r.RequisitionID == woredaDistributionDetail.RequisitionId).FirstOrDefault()
                     let totalIn =
                         _deliveryReconcileService.Get(
                             d =>
                                 d.RequsitionNo == reliefRequisition.RequisitionNo &&
                                 d.FDPID == woredaDistributionDetail.FdpId).Sum(s => s.ReceivedAmount)
                     where lossReason != null
                     select new WoredaDistributionDetailViewModel()
                     {
                         WoredaStockDistributionDetailID = woredaDistributionDetail.WoredaStockDistributionDetailID,
                         FdpId = woredaDistributionDetail.FdpId,
                         FDP = woredaDistributionDetail.FDP.Name,
                         CommodityID =
                             GetRequisionInfo(reliefRequisition.RequisitionID, woredaDistributionDetail.FdpId)
                                 .CommodityID,
                         CommodityName =
                             GetRequisionInfo(reliefRequisition.RequisitionID, woredaDistributionDetail.FdpId)
                                 .CommodityName,
                         AllocatedAmount =
                             GetRequisionInfo(reliefRequisition.RequisitionID, woredaDistributionDetail.FdpId)
                                 .AllocatedAmount,
                         NumberOfBeneficiaries =
                             GetRequisionInfo(reliefRequisition.RequisitionID, woredaDistributionDetail.FdpId)
                                 .BeneficaryNumber,
                         //RequisitionDetailViewModel = GetRequisionInfo(reliefRequisition.RequisitionID, woredaDistributionDetail.FdpId),
                         dispatched =
                             GetDispatchAllocation(reliefRequisition.RequisitionNo,
                                 woredaDistributionDetail.FDP.FDPID),
                         delivered =
                             GetDelivered(reliefRequisition.RequisitionNo, woredaDistributionDetail.FDP.FDPID),
                         RequistionNo = reliefRequisition.RequisitionNo,
                         Round = reliefRequisition.Round,
                         Month = RequestHelper.MonthName(reliefRequisition.Month),
                         BeginingBalance = woredaDistributionDetail.StartingBalance,
                         EndingBalance = woredaDistributionDetail.EndingBalance,
                         DistributedAmount = woredaDistributionDetail.DistributedAmount,
                         TotalIn = totalIn, //woredaDistributionDetail.TotalIn,
                         TotalOut = woredaDistributionDetail.TotoalOut,
                         LossAmount = woredaDistributionDetail.LossAmount,
                         LossReasonId = (int)lossReason,
                         RequisitionId = woredaDistributionDetail.RequisitionId,
                         DistributionStartDate = woredaDistributionDetail.DistributionStartDate,
                         DistributionStartDatePref = woredaDistributionDetail.DistributionStartDate.GetValueOrDefault().ToCtsPreferedDateFormatShort(datePref),
                         DistributionEndDate = woredaDistributionDetail.DistributionEndDate,
                         DistributionEndDatePref = woredaDistributionDetail.DistributionEndDate.GetValueOrDefault().ToCtsPreferedDateFormatShort(datePref)

                     }
                        ).ToList()
            };

            return woredaStockDistributionWithDetailViewModel;
        }
        private RequisitionDetailViewModel GetRequisionInfo(int requisitionID, int fdpID)
        {
            var requisitionDetail = _reliefRequisitionDetailService.FindBy(m => m.RequisitionID == requisitionID && m.FDPID == fdpID).FirstOrDefault();
            if (requisitionDetail != null)
            {
                var requisitonDetailInfo = new RequisitionDetailViewModel()
                {
                    CommodityID = requisitionDetail.CommodityID,
                    CommodityName = requisitionDetail.Commodity.Name,
                    BeneficaryNumber = requisitionDetail.BenficiaryNo,
                    AllocatedAmount = requisitionDetail.Amount
                };
                return requisitonDetailInfo;
            }
            return null;
        }

        private Decimal GetDispatchAllocation(string reqNo, int fdpId)
        {
            var dispatches = _dispatchService.Get(t => t.RequisitionNo == reqNo && t.FDPID == fdpId).ToList();
            var totaldispatched = dispatches.Sum(dispatch => dispatch.DispatchDetails.Sum(m => m.DispatchedQuantityInMT));

            return totaldispatched;
        }
        public JsonResult getReasonName(int id)
        {
            var loss = _lossReasonService.FindBy(l => l.LossReasonId == id).FirstOrDefault();
            if (loss == null)
                return Json("");
            var name = loss.LossReasonCodeEg;

            return Json(name, JsonRequestBehavior.AllowGet);
        }
        private decimal GetDelivered(string reqNo, int fdpId)
        {
            var dispatchIds = _dispatchService.Get(t => t.DispatchAllocation.RequisitionNo == reqNo && t.FDPID == fdpId).Select(t => t.DispatchID).ToList();
            var deliveries = _deliveryService.Get(t => dispatchIds.Contains(t.DispatchID.Value), null, "DeliveryDetails");
            return deliveries.Sum(delivery => delivery.DeliveryDetails.Sum(m => m.ReceivedQuantity));
        }

        private WoredaStockDistribution GetWoredaDetailMOdel(WoredaStockDistributionWithDetailViewModel distributionViewModel)
        {
            if (distributionViewModel != null)
            {
                var distributionModel = new WoredaStockDistribution()
                {
                    WoredaStockDistributionID = distributionViewModel.WoredaStockDistributionID,
                    ProgramID = distributionViewModel.ProgramID,
                    PlanID = distributionViewModel.PlanID,
                    WoredaID = distributionViewModel.WoredaID,
                    Month = distributionViewModel.Month,
                    ActualBeneficairies = distributionViewModel.ActualBeneficairies,
                    MaleBetween5And18Years = distributionViewModel.MaleBetween5And18Years,
                    MaleLessThan5Years = distributionViewModel.MaleLessThan5Years,
                    MaleAbove18Years = distributionViewModel.MaleAbove18Years,
                    FemaleAbove18Years = distributionViewModel.FemaleAbove18Years,
                    FemaleBetween5And18Years = distributionViewModel.FemaleBetween5And18Years,
                    FemaleLessThan5Years = distributionViewModel.FemaleLessThan5Years,
                    DirectSupport = distributionViewModel.DirectSupport,
                    PublicSupport = distributionViewModel.PublicSupport


                };
                return distributionModel;
            }
            return null;
        }
        String datePref = String.Empty;
        [HttpPost]
        public ActionResult Create(WoredaStockDistributionWithDetailViewModel woredaStockDistribution)
        {


            if (woredaStockDistribution != null)
            {
                var utilization = _utilizationService.FindBy(m => m.WoredaID == woredaStockDistribution.WoredaID &&
                                                             m.PlanID == woredaStockDistribution.PlanID &&
                                                             m.Month == woredaStockDistribution.Month).FirstOrDefault();

                if (utilization == null)
                {
                    var bindToModel = GetWoredaDetailMOdel(woredaStockDistribution);
                    var userProfileId = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).UserProfileID;
                    bindToModel.DistributedBy = userProfileId;
                    bindToModel.DistributionDate = DateTime.Now;
                    var saved = _utilizationService.AddHeaderDistribution(bindToModel);
                    if (saved)
                    {


                        var distributionHeader = _utilizationService.FindBy(m => m.WoredaID == woredaStockDistribution.WoredaID &&
                                                             m.PlanID == woredaStockDistribution.PlanID &&
                                                             m.Month == woredaStockDistribution.Month).FirstOrDefault();

                        foreach (var woredaDistributionDetailViewModel in woredaStockDistribution.WoredaDistributionDetailViewModels)
                        {
                            var distributionDetailModel = new WoredaStockDistributionDetail()
                            {
                                WoredaStockDistributionID = distributionHeader.WoredaStockDistributionID,
                                FdpId = woredaDistributionDetailViewModel.FdpId,
                                CommodityID = woredaDistributionDetailViewModel.CommodityID,
                                StartingBalance = woredaDistributionDetailViewModel.BeginingBalance,
                                EndingBalance = woredaDistributionDetailViewModel.EndingBalance,
                                TotalIn = woredaDistributionDetailViewModel.TotalIn,
                                TotoalOut = woredaDistributionDetailViewModel.TotalOut,
                                LossAmount = woredaDistributionDetailViewModel.LossAmount,
                                LossReason = woredaDistributionDetailViewModel.LossReasonId,
                                DistributedAmount = woredaDistributionDetailViewModel.DistributedAmount,
                                RequisitionId = woredaDistributionDetailViewModel.RequisitionId,

                            };

                            SetServiceModelDate(distributionDetailModel, woredaDistributionDetailViewModel);

                            _utilizationDetailSerivce.AddDetailDistribution(distributionDetailModel);
                        }

                        // ModelState.AddModelError("Success", @"Distribution Information Successfully Saved");
                        LookUps(woredaStockDistribution);


                        if (distributionHeader != null)
                        {
                            _transactionService.PostDistribution(distributionHeader.WoredaStockDistributionID);
                        }
                        WoredaStockDistributionWithDetailViewModel woredaStockDistributionViewModel = GetWoredaStockDistributionFormDB(distributionHeader);

                        //ModelState.AddModelError("Success", @"Distribution Information Successfully Saved!");
                        TempData["CustomError"] = "Distribution Information Successfully Saved!";
                        return RedirectToAction("Create",
                                                new
                                                {
                                                    Woreda = woredaStockDistributionViewModel.WoredaID,
                                                    planID = woredaStockDistributionViewModel.PlanID,
                                                    programID = woredaStockDistributionViewModel.ProgramID,
                                                    month = woredaStockDistributionViewModel.Month
                                                });
                        // return View(woredaStockDistributionViewModel);
                    }
                }
                else
                {
                    utilization.ActualBeneficairies = woredaStockDistribution.ActualBeneficairies;
                    utilization.FemaleLessThan5Years = woredaStockDistribution.FemaleLessThan5Years;
                    utilization.FemaleBetween5And18Years = woredaStockDistribution.FemaleBetween5And18Years;
                    utilization.FemaleAbove18Years = woredaStockDistribution.FemaleAbove18Years;
                    utilization.MaleLessThan5Years = woredaStockDistribution.MaleLessThan5Years;
                    utilization.MaleBetween5And18Years = woredaStockDistribution.MaleBetween5And18Years;
                    utilization.MaleAbove18Years = woredaStockDistribution.MaleAbove18Years;
                    utilization.DirectSupport = woredaStockDistribution.DirectSupport;
                    utilization.PublicSupport = woredaStockDistribution.PublicSupport;

                    _utilizationService.EditHeaderDistribution(utilization);

                    var woredaDistributionDetails = _utilizationDetailSerivce.FindBy(m => m.WoredaStockDistributionID == utilization.WoredaStockDistributionID);
                    if (woredaDistributionDetails != null)
                    {
                        foreach (var woredaDistributionDetailViewModel in woredaStockDistribution.WoredaDistributionDetailViewModels)
                        {
                            var woredaDistributionDetail = _utilizationDetailSerivce.FindById(woredaDistributionDetailViewModel.WoredaStockDistributionDetailID);
                            if (woredaDistributionDetail != null)
                            {
                                woredaDistributionDetail.DistributionStartDate =
                                    woredaDistributionDetailViewModel.DistributionStartDate != null
                                        ? (DateTime?)
                                          Convert.ToDateTime(
                                              (woredaDistributionDetailViewModel.DistributionStartDate.ToString()))
                                        : null;

                                woredaDistributionDetail.DistributionEndDate =
                                    woredaDistributionDetailViewModel.DistributionEndDate != null
                                        ? (DateTime?)
                                          Convert.ToDateTime(
                                              woredaDistributionDetailViewModel.DistributionEndDate.ToString())
                                        : null;

                                woredaDistributionDetail.CommodityID = woredaDistributionDetailViewModel.CommodityID;
                                woredaDistributionDetail.StartingBalance = woredaDistributionDetailViewModel.BeginingBalance;
                                woredaDistributionDetail.EndingBalance = woredaDistributionDetailViewModel.EndingBalance;
                                woredaDistributionDetail.TotalIn = woredaDistributionDetailViewModel.TotalIn;
                                woredaDistributionDetail.TotoalOut = woredaDistributionDetailViewModel.TotalOut;
                                woredaDistributionDetail.LossAmount = woredaDistributionDetailViewModel.LossAmount;
                                woredaDistributionDetail.LossReason = woredaDistributionDetailViewModel.LossReasonId;
                                woredaDistributionDetail.DistributedAmount = woredaDistributionDetailViewModel.DistributedAmount;
                                woredaDistributionDetail.RequisitionId = woredaDistributionDetailViewModel.RequisitionId;
                                _utilizationDetailSerivce.EditDetailDistribution(woredaDistributionDetail);

                            }
                        }


                    }
                    LookUps();
                    //ModelState.AddModelError("Success", @"Distribution Information Successfully Saved!");
                    TempData["CustomError"] = "Distribution Information Successfully Saved!";
                    return RedirectToAction("Create",
                                                    new
                                                    {
                                                        Woreda = utilization.WoredaID,
                                                        planID = utilization.PlanID,
                                                        programID = utilization.ProgramID,
                                                        month = utilization.Month
                                                    });
                }

                //WoredaStockDistributionWithDetailViewModel woredaStockDistributionViewModel2 = GetWoredaStockDistributionFormDB(woredaDistributionHeader);

            }
            //ModelState.AddModelError("Errors",@"Unable to Save Distribution Information");
            TempData["CustomError2"] = "Unable to Save Distribution Information";

            LookUps();
            ViewBag.Errors = "errors";
            if (woredaStockDistribution != null)
                return RedirectToAction("Create",
                                        new
                                        {
                                            Woreda = woredaStockDistribution.WoredaID,
                                            planID = woredaStockDistribution.PlanID,
                                            programID = woredaStockDistribution.ProgramID,
                                            month = woredaStockDistribution.Month
                                        });
            return RedirectToAction("Create");
        }

        private void SetServiceModelDate(WoredaStockDistributionDetail distributionDetailModel, WoredaDistributionDetailViewModel woredaDistributionDetailViewModel)
        {
            datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;

            //if(datePref.Trim().ToLower().Equals("gc"))
            //{
            distributionDetailModel.DistributionStartDate = woredaDistributionDetailViewModel.DistributionStartDate;
            distributionDetailModel.DistributionEndDate = woredaDistributionDetailViewModel.DistributionEndDate;


            //}
            //else
            //{
            //    //distributionDetailModel.DistributionStartDate =  DateTime.Parse(woredaDistributionDetailViewModel.DistributionStartDatePref).ToCTSPreferedDateFormat()
            //    //    . ToCTSPreferedDateFormat(datePref);
            //    //distributionDetailModel.DistributionEndDate = woredaDistributionDetailViewModel.DistributionEndDate;

            //}

        }


        public void LookUps()
        {
            var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            var regions = _commonService.GetRegions();
            if (userRegionID != null)
            {
                regions = _commonService.FindBy(m => m.AdminUnitID == userRegionID);

            }

            var lossReasons = _lossReasonService.GetAllLossReason().Select(t => new
            {
                name =
            t.LossReasonCodeEg + "-" +
            t.LossReasonEg,
                Id = t.LossReasonId
            });

            ViewData["LossReasons"] = lossReasons;

            ViewBag.Region = new SelectList(regions, "AdminUnitID", "Name", "--Select Region--");
            ViewBag.Zone = new SelectList(_commonService.FindBy(m => m.AdminUnitTypeID == 3 && m.ParentID == 3), "AdminUnitID", "Name");
            ViewBag.Woreda = new SelectList(_commonService.FindBy(m => m.AdminUnitTypeID == 4 && m.ParentID == 19), "AdminUnitID", "Name");
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
            //ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.SupportTypeID = new SelectList(_commonService.GetAllSupportType(), "SupportTypeID", "Description");
        }

        public void LookUps(WoredaStockDistributionWithDetailViewModel distributionInfo)
        {


            var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            var regions = _commonService.GetRegions();
            if (userRegionID != null)
            {
                regions = _commonService.FindBy(m => m.AdminUnitID == userRegionID);

            }
            var lossReasons = _lossReasonService.GetAllLossReason().Select(t => new
            {
                name =
            t.LossReasonCodeEg + "-" +
            t.LossReasonEg,
                Id = t.LossReasonId
            });

            ViewData["LossReasons"] = lossReasons;
            ViewBag.Region = new SelectList(regions, "AdminUnitID", "Name", "--Select Region--");
            ViewBag.Zone = new SelectList(_commonService.FindBy(m => m.AdminUnitTypeID == 3 && m.ParentID == 3), "AdminUnitID", "Name");
            ViewBag.Woreda = new SelectList(_commonService.FindBy(m => m.AdminUnitTypeID == 4 && m.ParentID == 19), "AdminUnitID", "Name", distributionInfo.WoredaID);
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name", distributionInfo.ProgramID);
            //ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "Id", "Name");
            ViewBag.SupportTypeID = new SelectList(_commonService.GetAllSupportType(), "SupportTypeID", "Description");
        }
        public ActionResult ReadRequestionNumbers([DataSourceRequest] DataSourceRequest request,
                                                  int zoneId = -1,
                                                  int programId = -1,
                                                  int planId = -1,
                                                  int round = -1,
                                                   int month = -1)
        {
            if (zoneId == -1 || programId == -1 || planId == -1)
                return null;
            if (programId == 1 && (month == -1 && round == -1))
                return null;
            if (programId == 2 && round == -1)
                return null;




            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisition = _utilizationService.GetRequisitions(zoneId, programId, planId, 6, month, round);
            var requisitionViewModel = UtilizationViewModelBinder.GetUtilizationViewModel(requisition);
            return Json(requisitionViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult WoredaStockDetail_Read([DataSourceRequest] DataSourceRequest request, int woredaStockDistributionID = 0, int woredaID = 0, int planID = 0, int month = 0)
        {
            if (woredaID == 0 || planID == 0 || month == 0) return null;
            var zone = _commonService.GetZoneID(woredaID);
            var region = _commonService.GetRegion(zone);
            var regionalRequest = _regionalRequestService.FindBy(m => m.PlanID == planID && m.Month == month && m.RegionID == region).FirstOrDefault();

            if (regionalRequest != null)
            {
                var requisitions = _reliefRequisitionService.FindBy(m => m.RegionalRequestID == regionalRequest.RegionalRequestID
                                                                && m.ZoneID == zone);

                if (requisitions != null)
                {
                    if (woredaStockDistributionID != 0)
                    {
                        var woredaStockDistribution =
                            _utilizationDetailSerivce.FindBy(
                                m => m.FDP.AdminUnitID == woredaID && m.WoredaStockDistributionID == woredaStockDistributionID);
                        var woredaDistributionDetail = new List<WoredaDistributionDetailViewModel>();
                        foreach (var reliefRequisition in requisitions)
                        {
                            woredaDistributionDetail = GetWoredaStockDistributionDetail(woredaStockDistribution, reliefRequisition);


                        }

                        return Json(woredaDistributionDetail.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }


                    var fdpStockDistribution = _commonService.GetFDPs(woredaID);
                    var woredaStockDistributionDetail = new List<WoredaDistributionDetailViewModel>();
                    foreach (var requisition in requisitions)
                    {
                        var detail = GetWoredaStockDistribution(fdpStockDistribution, requisition);
                        woredaStockDistributionDetail.AddRange(detail);
                    }

                    return Json(woredaStockDistributionDetail.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }


            //var fdps = _commonService.GetFDPs(woredaID);
            //var detail = GetWoredaStockDistribution(fdps, requisitions);
            return Json(new WoredaDistributionDetailViewModel(), JsonRequestBehavior.AllowGet);
        }


        private List<WoredaDistributionDetailViewModel> GetWoredaStockDistributionDetail(IEnumerable<WoredaStockDistributionDetail> woredaStockDistributionDetails, ReliefRequisition requisition)
        {

            return (from woredaStockDistributionDetail in woredaStockDistributionDetails
                    select new WoredaDistributionDetailViewModel()
                    {
                        FdpId = woredaStockDistributionDetail.FdpId,
                        FDP = woredaStockDistributionDetail.FDP.Name,
                        WoredaStockDistributionID = woredaStockDistributionDetail.WoredaStockDistributionID,
                        WoredaStockDistributionDetailID = woredaStockDistributionDetail.WoredaStockDistributionDetailID,
                        DistributedAmount = woredaStockDistributionDetail.DistributedAmount,
                        BeginingBalance = woredaStockDistributionDetail.StartingBalance,
                        EndingBalance = woredaStockDistributionDetail.EndingBalance,
                        TotalIn = woredaStockDistributionDetail.TotalIn,
                        TotalOut = woredaStockDistributionDetail.TotoalOut,
                        LossAmount = woredaStockDistributionDetail.LossAmount,
                        RequistionNo = requisition.RequisitionNo,
                        Round = requisition.Round,
                        Month = RequestHelper.MonthName(requisition.RegionalRequest.Month),
                        CommodityName = requisition.Commodity.Name,
                        RequisitionDetailViewModel = GetRequisionInfo(requisition.RequisitionID, woredaStockDistributionDetail.FdpId)
                    }).ToList();

        }

        private List<WoredaDistributionDetailViewModel> GetWoredaStockDistribution(IEnumerable<FDP> fdps, ReliefRequisition reliefRequisition)
        {


            return (from fdp in fdps
                    from detail in reliefRequisition.ReliefRequisitionDetails
                    where fdp.FDPID == detail.FDPID
                    select new WoredaDistributionDetailViewModel()
                    {
                        FdpId = fdp.FDPID,
                        FDP = fdp.Name,
                        DistributedAmount = 0,
                        RequistionNo = reliefRequisition.RequisitionNo,
                        Round = reliefRequisition.Round,
                        Month = RequestHelper.MonthName(reliefRequisition.RegionalRequest.Month),
                        CommodityID = detail.CommodityID,
                        CommodityName = detail.Commodity.Name,
                        AllocatedAmount = detail.Amount,
                        NumberOfBeneficiaries = detail.BenficiaryNo,
                        dispatched = GetDispatchAllocation(reliefRequisition.RequisitionNo, fdp.FDPID),
                        delivered = GetDelivered(reliefRequisition.RequisitionNo, fdp.FDPID),
                        RequisitionId = reliefRequisition.RequisitionID,
                        TotalIn = _utilizationService.GetTotalIn(fdp.FDPID, (int)reliefRequisition.RequisitionID) != 0 ? _utilizationService.GetTotalIn(fdp.FDPID, (int)reliefRequisition.RequisitionID) : 0,

                        //RequisitionDetailViewModel = new RequisitionDetailViewModel()
                        //    {
                        //        CommodityID = detail.CommodityID,
                        //        CommodityName = detail.Commodity.Name,
                        //        AllocatedAmount = detail.Amount,
                        //        BeneficaryNumber = detail.BenficiaryNo

                        //    },
                        //GetRequisionInfo(reliefRequisition.RequisitionID, fdp.FDPID)

                    }).ToList();

            //return (from fdp in fdps
            //        select new WoredaDistributionDetailViewModel()
            //        {
            //            FdpId = fdp.FDPID,
            //            FDP = fdp.Name,
            //            DistributedAmount = 0,


            //        }).ToList();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateWoredaDistribution([DataSourceRequest] DataSourceRequest request, WoredaDistributionDetailViewModel woredaDistributionDetail)
        {
            if (woredaDistributionDetail != null && ModelState.IsValid)
            {
                var detail = _utilizationDetailSerivce.FindById(woredaDistributionDetail.WoredaStockDistributionDetailID);
                if (detail != null)
                {
                    detail.WoredaStockDistributionID = woredaDistributionDetail.WoredaStockDistributionID;
                    detail.DistributedAmount = woredaDistributionDetail.DistributedAmount;
                    detail.TotalIn = woredaDistributionDetail.TotalIn;
                    detail.TotoalOut = woredaDistributionDetail.TotalOut;
                    detail.StartingBalance = woredaDistributionDetail.BeginingBalance;
                    detail.EndingBalance = woredaDistributionDetail.EndingBalance;
                    detail.LossAmount = woredaDistributionDetail.LossAmount;


                    _utilizationDetailSerivce.EditDetailDistribution(detail);
                }

            }
            return Json(new[] { woredaDistributionDetail }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetPlans(string id, int zoneID)
        {
            var programId = int.Parse(id);
            var plans = _commonService.GetRequisitionGeneratedPlan(programId, zoneID);
            return Json(new SelectList(plans.ToList(), "PlanID", "PlanName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonth(string id, int zoneID, int programId)
        {
            try
            {
                var planid = int.Parse(id);
                var requisition = _reliefRequisitionService.FindBy(m => m.ZoneID == zoneID).Select(m => m.RegionalRequestID).Distinct();

                var request = _regionalRequestService.FindBy(m => requisition.Contains(m.RegionalRequestID) && m.PlanID == planid).ToList();
                //var months = _regionalRequestService.FindBy(r => r.PlanID == planid).ToList();

                if (programId == (int)Cats.Models.Constant.Programs.Releif)
                {
                    var round = from m in request
                                select new { round = m.Round };
                    var distinctRound = round.Distinct();
                    return Json(new SelectList(distinctRound, "round", "round"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var month = from m in request
                                select new { month = m.Month };
                    var distinctMonth = month.Distinct();
                    return Json(new SelectList(distinctMonth, "month", "month"), JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {

                return null;
            }
        }

        public JsonResult GetRound(string id)
        {
            try
            {
                var planid = int.Parse(id);
                var rounds = _regionalRequestService.FindBy(r => r.PlanID == planid).ToList();
                var round = from r in rounds
                            where r.Round != null
                            select new { round = r.Round };
                var distinctRound = round.Distinct();
                return Json(new SelectList(distinctRound, "round", "round"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return null;
            }

        }
        public JsonResult GetAdminUnits()
        {
            var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            var regions = _commonService.GetRegions();
            if (userRegionID != null)
            {
                regions = _commonService.FindBy(m => m.AdminUnitID == userRegionID);

            }
            var r = (from region in regions
                     select new
                     {

                         RegionID = region.AdminUnitID,
                         RegionName = region.Name,
                         Zones = from zone in _commonService.GetZones(region.AdminUnitID)
                                 select new
                                 {
                                     ZoneID = zone.AdminUnitID,
                                     ZoneName = zone.Name,
                                     Woredas = from woreda in _commonService.GetWoreda(zone.AdminUnitID)
                                               select new
                                               {
                                                   WoredaID = woreda.AdminUnitID,
                                                   WoredaName = woreda.Name
                                               }
                                 }
                     }
                    );
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrograms()
        {
            try
            {
                var programs = _programService.GetAllProgram();
                var prog = (from program in programs
                            select new
                            {
                                ProgramID = program.ProgramID,
                                ProgramName = program.Name
                            }).ToList();
                return Json(prog, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private WoredaStockDistributionWithDetailViewModel GetWoredaStockDistributionFormDB(WoredaStockDistribution woredaStockDistribution)
        {
            var woredaStockDistributionWithDetailViewModel = new WoredaStockDistributionWithDetailViewModel()
            {
                WoredaStockDistributionID = woredaStockDistribution.WoredaStockDistributionID,
                WoredaID = woredaStockDistribution.WoredaID,
                ProgramID = woredaStockDistribution.ProgramID,
                PlanID = woredaStockDistribution.PlanID,
                Month = woredaStockDistribution.Month,
                DirectSupport = woredaStockDistribution.DirectSupport,
                PublicSupport = woredaStockDistribution.PublicSupport,
                ActualBeneficairies = woredaStockDistribution.ActualBeneficairies,
                MaleBetween5And18Years = woredaStockDistribution.MaleBetween5And18Years,
                MaleAbove18Years = woredaStockDistribution.MaleAbove18Years,
                MaleLessThan5Years = woredaStockDistribution.MaleLessThan5Years,
                FemaleAbove18Years = woredaStockDistribution.FemaleAbove18Years,
                FemaleBetween5And18Years = woredaStockDistribution.FemaleBetween5And18Years,

            };

            return woredaStockDistributionWithDetailViewModel;
        }
    }
}
