﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Helpers;
using Cats.Models.Constant;
using Cats.Services.Common;
using Cats.Services.Logistics;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.ViewModelBinder;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Cats.Areas.Logistics.Controllers
{
    public class UtilizationController : Controller
    {

        private readonly IUtilizationHeaderSerivce _utilizationService;
        private readonly IUtilizationDetailSerivce _utilizationDetailSerivce;
        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly UserAccountService _userAccountService;
        private readonly IWorkflowStatusService _workflowStatusService;
        private readonly ICommonService _commonService;
        private readonly IRegionalRequestService _regionalRequestService;
        public UtilizationController(IUtilizationHeaderSerivce utilizationService, IUtilizationDetailSerivce utilizationDetailSerivce, UserAccountService userAccountService, IWorkflowStatusService workflowStatusService, ICommonService commonService, IRegionalRequestService regionalRequestService)
        {
            _utilizationService = utilizationService;
            _utilizationDetailSerivce = utilizationDetailSerivce;
            _userAccountService = userAccountService;
            _workflowStatusService = workflowStatusService;
            _commonService = commonService;
            _regionalRequestService = regionalRequestService;
        }

        //
        // GET: /Logistics/Utilization/

        public ActionResult Index()
        {
           
            ViewBag.RegionCollection = _commonService.GetAminUnits(t => t.AdminUnitTypeID == 2);
           
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms().Take(2),"ProgramId","Name");
            return View();
        }


        public ActionResult ReadRequestionNumbers([DataSourceRequest] DataSourceRequest request, 
                                                  int zoneId=-1, 
                                                  int programId = -1,
                                                  int planId = -1,
                                                  int round =-1,
                                                   int month=-1)
        {
            if (zoneId == -1 || programId ==-1 || planId ==-1)
                return null;
            if (programId == 1 && (month == -1 && round == -1))
                return null;
            if (programId == 2 && round == -1)
                return null;

           

            
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisition = _utilizationService.GetRequisitions(zoneId,programId,planId,6,month,round);
            var requisitionViewModel =UtilizationViewModelBinder.GetUtilizationViewModel(requisition);
            return Json(requisitionViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        private  bool IsSaved(int planId, int month, int round)
        {
            try
            {
                var utilization =
                    _utilizationService.FindBy(u => u.PlanId == planId && u.Month == month && u.Round == round).ToList();
                if (utilization.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public ActionResult ReadRequisitionDetail([DataSourceRequest] DataSourceRequest request,   int requisitionId = -1 )
        {
            if (requisitionId == -1)
                return null;
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisition = _utilizationService.GetReliefRequisitions(requisitionId);
            var requisitionViewModel = UtilizationViewModelBinder.GetUtilizationDetailViewModel(requisition);
            return Json(requisitionViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, IEnumerable<Models.UtilizationDetailViewModel> utilizationDetailViewModels)
        {
            var results = new List<Models.UtilizationDetailViewModel>();
            return Json(results.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<Models.UtilizationDetailViewModel> utilizationDetailViewModels )
        {


            

            var userProfileId = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).UserProfileID;
            var results = new List<Models.UtilizationDetailViewModel>();


             var utilizationToBeSaved =
                    _utilizationService.FindBy(u => u.PlanId == planId && u.Month == month && u.Round == round).ToList();
             if (utilizationToBeSaved.Count > 0)

                {
                    
                }
            var utilization = new Cats.Models.UtilizationHeader();
                                 


            foreach (var utilizationDetailViewModel in utilizationDetailViewModels)
            {

                var utilizationDetail = new Cats.Models.UtilizationDetail
                                            {
                                                DistributedQuantity = utilizationDetailViewModel.DistributedQuantity,
                                                FdpId = utilizationDetailViewModel.FdpId,
                                                UtilizationHeader = utilization
                                            };
                utilization.RequisitionId = utilizationDetailViewModel.RequisitionId;
                utilization.PlanId = utilizationDetailViewModel.PlanId;
                utilization.DistributionDate = DateTime.Now;
                utilization.DistributedBy = userProfileId;
                _utilizationDetailSerivce.AddDetailDistribution(utilizationDetail);
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }



        public JsonResult GetPlans(string id)
        {
            var programId = int.Parse(id);
            var plans = _commonService.GetPlan(programId);
            return Json(new SelectList(plans.ToList(), "PlanID", "PlanName"), JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetMonth(string id)
        {
            try
            {
                var planid = int.Parse(id);

                var months = _regionalRequestService.FindBy(r => r.PlanID == planid).ToList();
                var month = from m in months
                             select new {month = m.Month};
                var distinctMonth = month.Distinct();
                return Json(new SelectList(distinctMonth, "month", "month"), JsonRequestBehavior.AllowGet);
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
                            select new {round = r.Round};
                var distinctRound = round.Distinct();
                return Json(new SelectList(distinctRound, "round", "round"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return null;
            }
           
        }
    }
}
