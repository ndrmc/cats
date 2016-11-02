using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Helpers;
using Cats.Models.Constant;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Services.Procurement;
using Cats.Services.Security;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace Cats.Areas.Regional.Controllers
{
    public class RegionRequisitionsController : Controller
    {
        //
        // GET: /Regional/RegionRequisitions/
        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IUserAccountService _userAccountService;
        private readonly IWorkflowStatusService _workflowStatusService;
        private readonly IDonorService _donorService;
        private readonly IRationService _rationService;
        private readonly ITransportOrderService _transportOrderService;

        private readonly IReliefRequisitionDetailService _reliefRequisitionDetailService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IStateTemplateService _stateTemplateService;
        private readonly IBusinessProcessService _businessProcessService;

        public RegionRequisitionsController(IUserAccountService userAccountService,
            IReliefRequisitionService reliefRequisitionService, IWorkflowStatusService workflowStatusService,
            IReliefRequisitionDetailService reliefRequisitionDetailService, IDonorService donorService,
            IRationService rationService, ITransportOrderService transportOrderService,
            IApplicationSettingService applicationSettingService, IStateTemplateService stateTemplateService,
            IBusinessProcessService businessProcessService)
        {
            _userAccountService = userAccountService;
            _reliefRequisitionService = reliefRequisitionService;
            _donorService = donorService;
            _rationService = rationService;
            _workflowStatusService = workflowStatusService;
            _reliefRequisitionDetailService = reliefRequisitionDetailService;
            _transportOrderService = transportOrderService;
            _applicationSettingService = applicationSettingService;
            _stateTemplateService = stateTemplateService;
            _businessProcessService = businessProcessService;
        }

        public ActionResult Index()
        {
            ViewBag.ProgramID = new SelectList(_transportOrderService.GetPrograms(), "ProgramID", "Name");

            var processTemplate = _applicationSettingService.FindBy(t => t.SettingName == "ReliefRequisitionWorkflow").FirstOrDefault();
            var processTemplateId = 0;
            var processStates = new List<Cats.Models.StateTemplate>();
            if (processTemplate != null)
            {
                processTemplateId = int.Parse(processTemplate.SettingValue);

                processStates = _stateTemplateService.FindBy(t => t.ParentProcessTemplateID == processTemplateId);

                ViewBag.StatusID = new SelectList(processStates, "StateTemplateID", "Name");
            }
            var stateTemplate = processStates.FirstOrDefault();
            if (stateTemplate != null)
            {
                ViewBag.Status = stateTemplate.StateTemplateID;
            }

            return View();
        }

        public ActionResult Requisition_Read([DataSourceRequest] DataSourceRequest request, string reqNoFilter, int? programFilter, int? roundFilter,int statusID)
        {
            var userRegionID= _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            var requests =
                _reliefRequisitionService.Get(
                    t =>
                        t.BusinessProcess.CurrentState.BaseStateTemplate.StateTemplateID == statusID && //  t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved"
                        t.RegionID == userRegionID,
                    null,
                    "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                    .ToList();
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requestViewModels =
                RequisitionViewModelBinder.BindReliefRequisitionListViewModel(requests, datePref)
                    .OrderByDescending(m => m.RequisitionID)
                    .Where(
                        p =>
                            (reqNoFilter.Length == 0 || p.RequisitionNo.Contains(reqNoFilter)) &&
                            (programFilter == null || p.ProgramID == programFilter) &&
                            (roundFilter == null || p.Round == roundFilter));
            return Json(requestViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public decimal GetCommodityRation(int requisitionID, int commodityID)
        {
            var reliefRequisition = _reliefRequisitionService.FindById(requisitionID);
            var ration = _rationService.FindById(reliefRequisition.RegionalRequest.RationID);
            var rationModel = ration.RationDetails.FirstOrDefault(m => m.CommodityID == commodityID);

            return rationModel != null ? rationModel.Amount : 0;

        }

        [HttpGet]
        public ActionResult Allocation(int id)
        {
            var requisition =
                _reliefRequisitionService.Get(t => t.RequisitionID == id, null, "ReliefRequisitionDetails").
                    FirstOrDefault();
            ViewData["donors"] = _donorService.GetAllDonor();
            //ViewBag.HRDID = new SelectList(_donorService.GetAllDonor(), "HRDID", "Year", donor.HRDID);

            if (requisition == null)
            {
                HttpNotFound();
            }
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisitionViewModel = RequisitionViewModelBinder.BindReliefRequisitionViewModel(requisition, datePref);

            return View(requisitionViewModel);
        }

        public ActionResult Allocation_Read([DataSourceRequest] DataSourceRequest request, int id)
        {

            var requisitionDetails = _reliefRequisitionDetailService.Get(t => t.RequisitionID == id, null, "ReliefRequisition.AdminUnit,FDP.AdminUnit,FDP,Donor,Commodity").ToList();
            var commodityID = requisitionDetails.FirstOrDefault().CommodityID;
            var RationAmount = GetCommodityRation(id, commodityID);
            RationAmount = RationAmount.GetPreferedRation();

            var requisitionDetailViewModels = RequisitionViewModelBinder.BindReliefRequisitionDetailListViewModel(requisitionDetails, RationAmount);
            return Json(requisitionDetailViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

    }



}
