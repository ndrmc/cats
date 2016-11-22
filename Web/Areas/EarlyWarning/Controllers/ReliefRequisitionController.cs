using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.Services.Transaction;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.UI;
using WebGrease.Css.Ast.Selectors;
using NUnit.Framework;
using StateTemplate = Cats.Models.StateTemplate;
using Cats.Alert;

namespace Cats.Areas.EarlyWarning.Controllers
{
    public class ReliefRequisitionController : Controller
    {
        //
        // GET: /EarlyWarning/ReliefRequisition/

        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IWorkflowStatusService _workflowStatusService;
        private readonly IReliefRequisitionDetailService _reliefRequisitionDetailService;
        private readonly IRegionalRequestService _regionalRequestService;
        private readonly IUserAccountService _userAccountService;
        private readonly IRationService _rationService;
        private readonly IRationDetailService _rationDetailService;
        private readonly IDonorService _donorService;
        private readonly INotificationService _notificationService;
        private readonly IPlanService _planService;
        private readonly ICommonService _commonService;
        private readonly Cats.Services.Transaction.ITransactionService _transactionService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IStateTemplateService _stateTemplateService;
        private readonly IBusinessProcessService _businessProcessService;
        public ReliefRequisitionController(
            IReliefRequisitionService reliefRequisitionService,
            IWorkflowStatusService workflowStatusService,
            IReliefRequisitionDetailService reliefRequisitionDetailService,
            IUserAccountService userAccountService,
            IRegionalRequestService regionalRequestService,
            IRationService rationService,
            IDonorService donorService,
            INotificationService notificationService,
            IPlanService planService,
            ITransactionService transactionService,
            ICommonService commonService, IRationDetailService rationDetailService, IApplicationSettingService applicationSettingService, IStateTemplateService stateTemplateService, IBusinessProcessService businessProcessService)
        {
            this._reliefRequisitionService = reliefRequisitionService;
            this._workflowStatusService = workflowStatusService;
            this._reliefRequisitionDetailService = reliefRequisitionDetailService;
            _userAccountService = userAccountService;
            _rationService = rationService;
            _donorService = donorService;
            _notificationService = notificationService;
            _planService = planService;
            _transactionService = transactionService;
            _commonService = commonService;
            _rationDetailService = rationDetailService;
            _applicationSettingService = applicationSettingService;
            _stateTemplateService = stateTemplateService;
            _businessProcessService = businessProcessService;
            _regionalRequestService = regionalRequestService;
        }

        public ViewResult Index()
        {
            

            var filter = new SearchRequistionViewModel();
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
                filter.StatusID = stateTemplate.StateTemplateID;
            }
                
            var user = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name);
            var firstOrDefault = _commonService.GetAminUnits(t => t.AdminUnitTypeID == 2 && t.AdminUnitID == user.RegionID).FirstOrDefault();
            if (firstOrDefault != null)
                filter.RegionID = firstOrDefault.AdminUnitID;
            else
                filter.RegionID = 2;
            switch (user.CaseTeam)
            {
                case 1://earlywarning
                    var orDefault = _commonService.GetPrograms().FirstOrDefault(p => p.ProgramID == (int)Programs.Releif);
                    if (orDefault != null)
                        filter.ProgramID = orDefault.ProgramID;
                    break;
                case 2: //PSNP
                    var @default = _commonService.GetPrograms().FirstOrDefault(p => p.ProgramID == (int)Programs.PSNP);
                    if (@default != null)
                        filter.ProgramID = @default.ProgramID;
                    ViewBag.program = "PSNP";
                    break;
            }
            
            
            ViewBag.Filter = filter;
            Populatelookup();
            //ViewBag.Status = id;
            return View();
        }

        [HttpPost]
        public ActionResult Index(SearchRequistionViewModel filter)
        {
            ViewBag.Filter = filter;
            Populatelookup();
            return View(filter);
        }

        void Populatelookup()
        {
            var user = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name);
            ViewBag.RegionID = user.RegionalUser ? new SelectList(_commonService.GetAminUnits(t => t.AdminUnitTypeID == 2 && t.AdminUnitID == user.RegionID), "AdminUnitID", "Name") : new SelectList(_commonService.GetAminUnits(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");


            if (user.CaseTeam != null)
            {
                switch (user.CaseTeam)
                {
                    case 1://earlywarning
                        ViewBag.ProgramId = new SelectList(_commonService.GetPrograms().Where(p => p.ProgramID == (int)Programs.Releif || p.ProgramID == (int)Programs.IDPS).Take(2), "ProgramID", "Name");
                        break;
                    case 2: //PSNP
                        ViewBag.ProgramId = new SelectList(_commonService.GetPrograms().Where(p => p.ProgramID == (int)Programs.PSNP).Take(2), "ProgramID", "Name");
                        break;
                }
            }
            else if (user.RegionalUser)
            {
                ViewBag.ProgramId =
                    new SelectList(
                        _commonService.GetPrograms().Where(p => p.ProgramID == (int)Programs.Releif).Take(2),
                        "ProgramID", "Name");
            }
            else
            {
                ViewBag.ProgramId = new SelectList(_commonService.GetPrograms().Take(2), "ProgramID", "Name");
            }


           // ViewBag.ProgramId = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
            //ViewBag.Month = new SelectList(RequestHelper.GetMonthList(), "ID", "Name");
            //ViewBag.RationID = new SelectList(_commonService.GetRations(), "RationID", "RefrenceNumber");
            //ViewBag.DonorID = new SelectList(_commonService.GetDonors(), "DonorId", "Name");
            //ViewBag.Round = new SelectList(RequestHelper.GetMonthList(), "ID", "ID");
            //ViewBag.PlanID = new SelectList(_commonService.GetPlan(1), "PlanID", "PlanName");
            //ViewBag.PSNPPlanID = new SelectList(_commonService.GetPlan(2), "PlanID", "PlanName");
            //ViewBag.SeasonID = new SelectList(_commonService.GetSeasons(), "SeasonID", "Name");

            var statuslist = new List<RequestStatus>();

            statuslist.Add(new RequestStatus { StatusID = 1, StatusName = "Draft" });
            statuslist.Add(new RequestStatus { StatusID = 2, StatusName = "Approved" });
            statuslist.Add(new RequestStatus { StatusID = 3, StatusName = "Hub Assigned" });
            statuslist.Add(new RequestStatus { StatusID = 4, StatusName = "Project Code Assigned" });
            statuslist.Add(new RequestStatus { StatusID = 5, StatusName = "Transport Requisition Created" });
            statuslist.Add(new RequestStatus { StatusID = 6, StatusName = "Transport Order Created" });
            statuslist.Add(new RequestStatus { StatusID = 7, StatusName = "Rejected" });
            
            var firstOrDefault = _applicationSettingService.FindBy(t => t.SettingName == "ReliefRequisitionWorkflow").FirstOrDefault();
            var processTemplateId = 0;
            if (firstOrDefault != null)
            {
                processTemplateId = int.Parse(firstOrDefault.SettingValue);

                var processStates = _stateTemplateService.FindBy(t => t.ParentProcessTemplateID == processTemplateId);

                ViewBag.StatusID = new SelectList(processStates, "StateTemplateID", "Name");
            }
            
        }

        [HttpGet]
        public ActionResult CreateRequisitionForIDPS(int id, int programId = -1)
        {
            try
            {
                var planID = _regionalRequestService.FindById(id).PlanID;
                if (programId == (int)Programs.IDPS)
                {
                    var planToBeEdited = _planService.FindBy(p => p.PlanID == planID).Single();
                    if (planToBeEdited != null)
                    {
                        //var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
                        //var planViewModel = new PlanViewModel()
                        //                        {
                        //                            planID = planToBeEdited.PlanID,
                        //                            planName = planToBeEdited.PlanName,
                        //                            StartDate = planToBeEdited.StartDate.ToCTSPreferedDateFormat(datePref),
                        //                            EndDate = planToBeEdited.EndDate.ToCTSPreferedDateFormat(datePref),
                        //                            ProgramID = planToBeEdited.ProgramID,
                        //                            Program = planToBeEdited.Program.Name,
                        //                            StatusID = planToBeEdited.Status,
                        //                        };

                        return PartialView(planToBeEdited);
                    }
                }
               
                return null;
            }
            catch (Exception)
            {
                return null;

            }
            
        }

        [HttpPost]
        public ActionResult CreateRequisitionForIDPS(Plan plan, int id)
        {
            try
            {
                var planToBeEdited = _planService.FindBy(p => p.PlanID == plan.PlanID).Single();

                if (planToBeEdited != null)
                {
                    planToBeEdited.PlanName = plan.PlanName;
                    planToBeEdited.StartDate = plan.StartDate;
                    planToBeEdited.EndDate = plan.EndDate;

                    _planService.EditPlan(planToBeEdited);
                    return RedirectToAction("CreateRequisiton", new { id = id });
                }
                ModelState.AddModelError("Error", errorMessage: @"Can not edit Plan");
                return null;
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        [HttpGet]
        public ActionResult CreateRequisiton(int id)
        {
            var input = _reliefRequisitionService.CreateRequisition(id, User.Identity.Name);
            //if (input == null)
            //{
            //TempData["error"] = "You haven't selected any commodity. Please add at least one commodity and try again!";
            //return RedirectToAction("Details", "Request", new { id = id, Area = "EarlyWarning" });
            //}
            return RedirectToAction("NewRequisiton", "ReliefRequisition", new { id = id });
        }

        [HttpGet]
        public ViewResult NewRequisiton(int id)
        {
            var input = _reliefRequisitionService.GetRequisitionByRequestId(id);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            
            IList<string> allRequisitionNumbers = new List<string>();
            IList<string> filteredReqNumbers = new List<string>();

            ViewBag.RequestId = id;
            if (input == null) return View(new List<ReliefRequisitionNew>());
            foreach (var reliefRequisitionNew in input)
            {
                if (reliefRequisitionNew.RequestedDate.HasValue)
                {
                    reliefRequisitionNew.RequestDatePref = reliefRequisitionNew.RequestedDate.Value.ToCTSPreferedDateFormat(datePref);
                    reliefRequisitionNew.RegionalRequestId = id;

                    filteredReqNumbers.Add(reliefRequisitionNew.Input.RequisitionNo);
                }
                reliefRequisitionNew.MonthName = RequestHelper.MonthName(reliefRequisitionNew.Month);
            } 

            var requsitionNos = _reliefRequisitionService.GetAllReliefRequisition();

            foreach (var reliefRequisitionNew in requsitionNos)
            {
                allRequisitionNumbers.Add(reliefRequisitionNew.RequisitionNo);
            }

            ViewBag.AllRequistionNumbers = allRequisitionNumbers; // will be used to validate new input requistion numbers...
            ViewBag.ClientSideReqNumbers = filteredReqNumbers; // will be used to validate new input requistion numbers...

            return View(input);
        }

        public ActionResult ShowRepeatedRequisitions()
        {
            return View("_RepeatedRequisitions");
        }

        public ActionResult RepeatedRequisitions()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewRequisiton(List<DataFromGrid> input, int id = 0)
        {

            var requId = 0;
            var requisistionNos = input.Select(m => m.RequisitionNo).ToList();

            //var duplicateRequisitionNos = requisistionNos.GroupBy(g => g).Where(g => g.Count() > 1).ToList();
            //if (duplicateRequisitionNos.Any())
            //{
            //    var duplicatedValues = string.Empty;
            //    foreach (var requisionNo in duplicateRequisitionNos)
            //    {
            //        if (duplicatedValues == string.Empty) duplicatedValues = requisionNo.Key;
            //        else duplicatedValues = duplicatedValues + "," + requisionNo.Key;
            //    }
            //    ModelState.AddModelError("Errors",
            //           string.Format("{0} Requisition Nos are duplicated please Change Requisition Nos marked red.",
            //               duplicatedValues));

            //    return View();
            //}

            //
            var requsitionNo = _reliefRequisitionService.FindBy(m => requisistionNos.Contains(m.RequisitionNo) && m.RegionalRequestID != id);
            if (requsitionNo.Count > 0)
            {
                var requsisions = _reliefRequisitionService.GetRequisitionByRequestId(id).ToList();
                var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
                var existingRequisitionNo = requsitionNo.FirstOrDefault();
                foreach (var reliefRequisitionNew in requsisions)
                {
                    if (reliefRequisitionNew.RequestedDate.HasValue)
                    {
                        reliefRequisitionNew.RequestDatePref = reliefRequisitionNew.RequestedDate.Value.ToCTSPreferedDateFormat(datePref);
                        reliefRequisitionNew.RegionalRequestId = id;
                    }
                    reliefRequisitionNew.MonthName = RequestHelper.MonthName(reliefRequisitionNew.Month);
                }
                if (existingRequisitionNo != null)
                    ModelState.AddModelError("Errors",
                        string.Format("{0} Requisition No already existed please Change Requisition No",
                            existingRequisitionNo.RequisitionNo));

                return View(requsisions);
            }


            if (ModelState.IsValid)
            {
                var requisitionNumbers = input.ToDictionary(t => t.Number, t => t.RequisitionNo);
                _reliefRequisitionService.AssignRequisitonNo(requisitionNumbers);
            }
            return RedirectToAction("Index", "ReliefRequisition");
        }

        public ActionResult CancelChanges(int id)
        {

            var requisitions = _reliefRequisitionService.FindBy(t => t.RegionalRequestID == id);

            foreach (var reliefRequisition in requisitions)
            {
               var deatils = _reliefRequisitionDetailService.FindBy(t => t.RequisitionID == reliefRequisition.RequisitionID);
                foreach (var detail in deatils)
                {
                    _reliefRequisitionDetailService.DeleteReliefRequisitionDetail(detail);
                }
                _reliefRequisitionService.DeleteReliefRequisition(reliefRequisition);
            }

            var request = _regionalRequestService.FindById(id);
            request.Status = (int)RegionalRequestStatus.Approved;
            _regionalRequestService.EditRegionalRequest(request);

            return RedirectToAction("Details", "Request", new { id = id });
        }

        [HttpGet]
        public ActionResult Allocation(int? id)
        {
            if (id == null)
            {
                return Redirect(Url.Action("Index", "ReliefRequisition"));
            }
            ViewBag.TargetController = "ReliefRequisition";
            var requisition =
                _reliefRequisitionService.Get(t => t.RequisitionID == id, null, "ReliefRequisitionDetails").
                    FirstOrDefault();
            ViewData["donors"] = _donorService.GetAllDonor();
            //ViewBag.HRDID = new SelectList(_donorService.GetAllDonor(), "HRDID", "Year", donor.HRDID);

            if (requisition == null)
            {
                HttpNotFound();
            }
            if (requisition != null && requisition.ProgramID == (int)Programs.PSNP)
            {
                ViewBag.program = "PSNP";
            }
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisitionViewModel = RequisitionViewModelBinder.BindReliefRequisitionViewModel(requisition, datePref);
            if (requisition != null && (requisition.RationID != null && requisition.RationID > 0))
                requisitionViewModel.Ration = _rationService.FindById((int)requisition.RationID).RefrenceNumber;
            return View(requisitionViewModel);
        }

        public ActionResult Allocation_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            
            var requisitionDetails = _reliefRequisitionDetailService.Get(t => t.RequisitionID == id, null, "ReliefRequisition.AdminUnit,FDP.AdminUnit,FDP,Donor,Commodity").ToList();
            var commodityID = requisitionDetails.FirstOrDefault().CommodityID;
            var RationAmount = GetCommodityRation(id, commodityID);
            RationAmount = RationAmount.GetPreferedRation();
       
            var requisitionDetailViewModels = RequisitionViewModelBinder.BindReliefRequisitionDetailListViewModel(requisitionDetails, RationAmount);
            return Json(GetDonorCoveredWoredas(requisitionDetailViewModels, id).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<ReliefRequisitionDetailViewModel> GetDonorCoveredWoredas(IEnumerable<ReliefRequisitionDetailViewModel> reliefRequisitionDetailViewModels, int requisitionID)
        {
            var requisition = _reliefRequisitionService.FindById(requisitionID);
            
            if (requisition != null && requisition.ProgramID == (int)Programs.Releif)
            {
                var regionalRequest = _regionalRequestService.FindBy(m => m.RegionalRequestID == requisition.RegionalRequestID).FirstOrDefault();
                if (regionalRequest != null)
                {
                    var hrd = _planService.GetHrd(regionalRequest.PlanID);
                    if (hrd != null)
                    {
                        var donorCoveredWoredas = _planService.GetDonorCoverage(m => m.HRDID == hrd.HRDID, null,
                                                                                "HrdDonorCoverageDetails").ToList();
                        if (donorCoveredWoredas.Count != 0)
                        {
                            return (from reliefRequisitionDetailViewModel in reliefRequisitionDetailViewModels

                                    select new ReliefRequisitionDetailViewModel()
                                        {
                                        Zone = reliefRequisitionDetailViewModel.Zone,
                                        Woreda = reliefRequisitionDetailViewModel.Woreda,
                                        FDP = reliefRequisitionDetailViewModel.FDP,
                                        Donor = _planService.FindHrdDonorCoverage(donorCoveredWoredas, reliefRequisitionDetailViewModel.FDPID) ?? "DRMFSS",
                                        //_.DonorID.HasValue ? reliefRequisitionDetail.Donor.Name : "-",
                                        Commodity = reliefRequisitionDetailViewModel.Commodity,
                                        BenficiaryNo = reliefRequisitionDetailViewModel.BenficiaryNo,
                                        Amount = reliefRequisitionDetailViewModel.Amount,
                                        RequisitionID = reliefRequisitionDetailViewModel.RequisitionID,
                                        RequisitionDetailID = reliefRequisitionDetailViewModel.RequisitionDetailID,
                                        CommodityID = reliefRequisitionDetailViewModel.CommodityID,
                                        FDPID = reliefRequisitionDetailViewModel.FDPID,
                                        //DonorID = reliefRequisitionDetailViewModel.DonorID,
                                        //RationAmount =RationAmount,
                                        Contingency = reliefRequisitionDetailViewModel.Contingency
                                    }

                                   );
                        }
                    }
                }
            }

            return reliefRequisitionDetailViewModels;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Create([DataSourceRequest] DataSourceRequest request, ReliefRequisitionDetailViewModel reliefRequisitionDetailViewModel)
        {
            if (reliefRequisitionDetailViewModel != null && ModelState.IsValid)
            {
                _reliefRequisitionDetailService.AddReliefRequisitionDetail(RequisitionViewModelBinder.BindReliefRequisitionDetail(reliefRequisitionDetailViewModel));

                UpdateEditWorkflow(reliefRequisitionDetailViewModel.RequisitionID, AlertMessage.Workflow_ReliiefReqDetailAdded);
            }

            return Json(new[] { reliefRequisitionDetailViewModel }.ToDataSourceResult(request, ModelState));
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Update([DataSourceRequest] DataSourceRequest request, ReliefRequisitionDetailViewModel reliefRequisitionDetailViewModel)
        {
            if (reliefRequisitionDetailViewModel != null && ModelState.IsValid)
            {
                var target = _reliefRequisitionDetailService.FindById(reliefRequisitionDetailViewModel.RequisitionDetailID);
                if (target != null)
                {
                    //Only create log if there is any change
                    if(target.BenficiaryNo!= reliefRequisitionDetailViewModel.BenficiaryNo)
                    {
                        UpdateEditWorkflow(reliefRequisitionDetailViewModel.RequisitionID, 
                            AlertManager.GetWorkflowMessage_ReliefReqDetail( target.FDP.Name, target.BenficiaryNo.ToString(), reliefRequisitionDetailViewModel.BenficiaryNo.ToString()));

                   }

                    target.Amount = reliefRequisitionDetailViewModel.Amount.ToPreferedWeightUnitForInsert();
                    target.BenficiaryNo = reliefRequisitionDetailViewModel.BenficiaryNo;
                    target.Contingency = reliefRequisitionDetailViewModel.Contingency;
                    if (reliefRequisitionDetailViewModel.DonorID.HasValue)
                        target.DonorID = reliefRequisitionDetailViewModel.DonorID.Value;
                    _reliefRequisitionDetailService.EditReliefRequisitionDetail(target);
                }


            }

            return Json(new[] { reliefRequisitionDetailViewModel }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Allocation_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  ReliefRequisitionDetail reliefRequisitionDetail)
        {
            if (reliefRequisitionDetail != null)
            {
                _reliefRequisitionDetailService.DeleteById(reliefRequisitionDetail.RequisitionDetailID);

                UpdateEditWorkflow(reliefRequisitionDetail.RequisitionID, AlertMessage.Workflow_ReliiefReqDetailDeleted);

            }

            return Json(ModelState.ToDataSourceResult());
        }

        public Boolean UpdateEditWorkflow(int requisitionid, String changeNote = null)
        {

            var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == requisitionid, null,
                                  "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();

         
            WorkflowCommon.EnterEditWorkflow(requisition.BusinessProcess, changeNote);


            return true;
        }
        [HttpPost]
        public ActionResult RequistionDetailEdit(IEnumerable<ReleifRequisitionDetailEdit.ReleifRequisitionDetailEditInput> input)
        {
            // var requId = 0;
            if (ModelState.IsValid)
            {
                var allocaitons = input.ToDictionary(t => t.Number, t => t.Amount);

                _reliefRequisitionService.EditAllocatedAmount(allocaitons);

            }
            return RedirectToAction("Index", "ReliefRequisition");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
           var relifRequisition = _reliefRequisitionService.FindById(id);
            ViewBag.RequistionNumber = relifRequisition.RequisitionNo;

            if (relifRequisition != null)
            {
                //ViewBag.RationSelected = relifRequisition.RationID;
                //ViewBag.RationID = _rationService.GetAllRation();
                if (relifRequisition.ProgramID == (int)Programs.PSNP)
                {
                    ViewBag.program = "PSNP";
                }
                
                ViewBag.RationID = new SelectList(_rationService.Get(t => t.RationDetails.Select(m =>
                m.CommodityID).Contains((int)relifRequisition.CommodityID)), "RationID", "RefrenceNumber", relifRequisition.RationID);

                return View(relifRequisition);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(ReliefRequisition reliefrequisition, FormCollection collection)
        {
            if (reliefrequisition == null) return Json(new { status = "Bad", message = "Requistion is not found." });
            
            var requisitionById = _reliefRequisitionService.FindById(reliefrequisition.RequisitionID); // get object from db
            var requisitionByReqNumber = _reliefRequisitionService.FindBy(r => r.RequisitionNo == reliefrequisition.RequisitionNo).FirstOrDefault();

            //if (requisitionById == null) return Json(new { status = "Bad", message = "Requistion is not found." });
            //if (requisitionByReqNumber == null) return Json(new { status = "Bad", message = "Requistion is not found." });
            
            var requisitionNumber =
             _reliefRequisitionService.FindBy(r => r.RequisitionNo == reliefrequisition.RequisitionNo)
                 .FirstOrDefault();

            if (requisitionByReqNumber != null)
            {
                if (requisitionById.RequisitionID != requisitionByReqNumber.RequisitionID)
                {
                    if (requisitionNumber != null)
                    {
                        return
                            Json(
                                new
                                {
                                    status = "Bad",
                                    message = "Duplicate requisition number, please change it and try again."
                                });
                    }
                }
                
            }
            
            if (requisitionById.ReliefRequisitionDetails.Count > 0)
            {
                foreach (var oldRequisitionDetail in requisitionById.ReliefRequisitionDetails)
                {
                    var commodityAmount = (decimal)0.00;
                    if (reliefrequisition.RationID != null)
                    {
                        var detail = oldRequisitionDetail;
                        var ration = _rationDetailService.FindBy(t => t.RationID == (int)reliefrequisition.RationID && 
                        t.CommodityID == detail.CommodityID).FirstOrDefault();
                        if (ration != null) commodityAmount = ration.Amount / 1000;
                    }
                    var newRequisitionDetail = new ReliefRequisitionDetail()
                    {
                        RequisitionID = oldRequisitionDetail.RequisitionID,
                        RequisitionDetailID = oldRequisitionDetail.RequisitionDetailID,
                        CommodityID = oldRequisitionDetail.CommodityID,
                        BenficiaryNo = oldRequisitionDetail.BenficiaryNo,
                        Amount = oldRequisitionDetail.BenficiaryNo * commodityAmount,
                        FDPID = oldRequisitionDetail.FDPID,
                        DonorID = oldRequisitionDetail.DonorID,
                        Contingency = oldRequisitionDetail.Contingency
                    };
                    //oldRequisitionDetail.Amount = oldRequisitionDetail.BenficiaryNo*commodityAmount;
                    _reliefRequisitionDetailService.DeleteById(oldRequisitionDetail.RequisitionDetailID);
                    _reliefRequisitionDetailService.AddReliefRequisitionDetail(newRequisitionDetail);
                }
            }
            requisitionById.RationID = reliefrequisition.RationID;
            requisitionById.RequisitionNo = reliefrequisition.RequisitionNo;
            requisitionById.RequestedDate = reliefrequisition.RequestedDate;
            _reliefRequisitionService.EditReliefRequisition(requisitionById);

            UpdateEditWorkflow(reliefrequisition.RequisitionID,AlertMessage.Workflow_DefaultEdit);

            return Json(new { status = "Ok", message = string.Empty, Id = requisitionById.RequisitionID});
        }

        [HttpGet]
        public ActionResult SendToLogistics(int id)
        {
            //var requistion = _reliefRequisitionService.FindById(id);
            //if (requistion == null)
            //{
            //    HttpNotFound();
            //}
            var requisition =
                _reliefRequisitionService.Get(t => t.RequisitionID == id, null, "ReliefRequisitionDetails").
                    FirstOrDefault();
            if (requisition == null)
            {
                HttpNotFound();
            }
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisitionViewModel = RequisitionViewModelBinder.BindReliefRequisitionViewModel(requisition, datePref);

          

            return View(requisitionViewModel);
        }


        [HttpPost]
        public ActionResult ConfirmSendToLogistics(int requisitionid)
        {
            var requisition = _reliefRequisitionService.Get(t=>t.RequisitionID == requisitionid, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Approve");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "Requisition approved and sent to logistics",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = requisition.BusinessProcessID
                    };
                    //return 
                    _businessProcessService.PromotWorkflow(businessProcessState);
                    SendNotification(requisition);
                    _transactionService.PostRequestAllocation(requisitionid);
                }
            }
            
            //requisition.Status = (int)ReliefRequisitionStatus.Approved;
            //_reliefRequisitionService.EditReliefRequisition(requisition);
            //send notification
           return RedirectToAction("Index", "ReliefRequisition");
        }

        private void SendNotification(ReliefRequisition requisition)
        {
            try
            {
                string destinationURl;
                if (Request.Url.Host != null)
                {
                    if (Request.Url.Host == "localhost")
                    {
                        destinationURl = "http://" + Request.Url.Authority +
                                         "/Logistics/DispatchAllocation/IndexFromNotification?paramRegionId=" +
                                         requisition.RegionID +
                                         "&recordId=" + requisition.RequisitionID;
                        return;
                    }
                    destinationURl = 
                                     "/Logistics/DispatchAllocation/IndexFromNotification?paramRegionId=" +
                                     requisition.RegionID +
                                     "&recordId=" + requisition.RequisitionID;

                    _notificationService.AddNotificationForLogistcisFromEarlyWaring(destinationURl,
                                                                                    requisition.RequisitionID,
                                                                                    (int)requisition.RegionID,
                                                                                    requisition.RequisitionNo);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public ActionResult Requisition_Read([DataSourceRequest] DataSourceRequest request, int id = 0)
        {
            var requests = _reliefRequisitionService.Get(t => t.Status == id);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requestViewModels = RequisitionViewModelBinder.BindReliefRequisitionListViewModel(requests, datePref).OrderByDescending(m=>m.RequisitionID);
                        return Json(requestViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Requisition_Search([DataSourceRequest] DataSourceRequest request, int regionID, int programID, int id)// SearchRequsetViewModel filter)
        {
            var requests = _reliefRequisitionService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.StateTemplateID == id && t.RegionID==regionID && t.ProgramID==programID);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requestViewModels = RequisitionViewModelBinder.BindReliefRequisitionListViewModel(requests, datePref).OrderByDescending(m => m.RequisitionID);
            return Json(requestViewModels.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Requisition_Update([DataSourceRequest] DataSourceRequest request, ReliefRequisitionViewModel reliefRequisitionViewModel)
        {
            if (reliefRequisitionViewModel != null && ModelState.IsValid)
            {
                var target = _reliefRequisitionService.FindById(reliefRequisitionViewModel.RequisitionID);
                if (target != null)
                {

                    target.RequisitionNo = reliefRequisitionViewModel.RequisitionNo;

                    _reliefRequisitionService.EditReliefRequisition(target);
                }
            }

            return Json(new[] { reliefRequisitionViewModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Requisition_Destroy([DataSourceRequest] DataSourceRequest request,
                                                  ReliefRequisition reliefRequisition)
        {
            if (reliefRequisition != null)
            {
                _reliefRequisitionDetailService.DeleteById(reliefRequisition.RequisitionID);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult Details(int id)
        {
            var requisition = _reliefRequisitionService.FindById(id);
            if (requisition == null)
            {
                return HttpNotFound();
            }
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requisitionViewModel = RequisitionViewModelBinder.BindReliefRequisitionViewModel(requisition, datePref);
            return View(requisitionViewModel);
        }

        public decimal GetCommodityRation(int requisitionID, int commodityID)
        {
            var reliefRequisition = _reliefRequisitionService.FindById(requisitionID);
            if (reliefRequisition.RegionalRequestID == null)
            {
                var reliefRequisitionDetail = reliefRequisition.ReliefRequisitionDetails.FirstOrDefault();
                if (reliefRequisitionDetail != null)
                    return reliefRequisitionDetail.Amount;
            }
            var ration = _rationService.FindById(reliefRequisition.RegionalRequest.RationID);
            var rationModel = ration.RationDetails.FirstOrDefault(m => m.CommodityID == commodityID);

            return rationModel != null ? rationModel.Amount : 0;

        }

        private bool Promote(BusinessProcessStateViewModel st)
        {
            var fileName = "";
            if (st.AttachmentFile.HasFile())
            {
                //save the file
                fileName = st.AttachmentFile.FileName;
                var path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);
                if (System.IO.File.Exists(path))
                {
                    var indexOfDot = fileName.IndexOf(".", StringComparison.Ordinal);
                    fileName = fileName.Insert(indexOfDot - 1, GetRandomAlphaNumeric(6));
                    path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);
                }
                st.AttachmentFile.SaveAs(path);
            }
            var businessProcessState = new BusinessProcessState()
            {
                StateID = st.StateID,
                PerformedBy = HttpContext.User.Identity.Name,
                DatePerformed = DateTime.Now,
                Comment = st.Comment,
                AttachmentFile = fileName,
                ParentBusinessProcessID = st.ParentBusinessProcessID
            };
            return _businessProcessService.PromotWorkflow(businessProcessState);
        }


        [HttpPost]
        public ActionResult Promote(BusinessProcessStateViewModel st, int? statusId)
        {
            var fileName = "";
            if (st.AttachmentFile.HasFile())
            {
                //save the file
                fileName = st.AttachmentFile.FileName;
                var path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);

                if (System.IO.File.Exists(path))
                {
                    var indexOfDot = fileName.IndexOf(".", StringComparison.Ordinal);
                    fileName = fileName.Insert(indexOfDot - 1, GetRandomAlphaNumeric(6));
                    path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);
                }
                st.AttachmentFile.SaveAs(path);
            }
            var businessProcessState = new BusinessProcessState()
            {
                StateID = st.StateID,
                PerformedBy = HttpContext.User.Identity.Name,
                DatePerformed = DateTime.Now,
                Comment = st.Comment,
                AttachmentFile = fileName,
                ParentBusinessProcessID = st.ParentBusinessProcessID
            };
            _businessProcessService.PromotWorkflow(businessProcessState);
            if (statusId != null)
                return RedirectToAction("Allocation", "ReliefRequisition", new { Area = "EarlyWarning", statusId });
            return RedirectToAction("Index", "ReliefRequisition", new { Area = "EarlyWarning" });
        }

        public static string GetRandomAlphaNumeric(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }

        //  [HttpGet]
        public ActionResult PromotThese(int regionId, int programId, string promotTo)
        {
            var requests = _reliefRequisitionService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Draft" || 
            t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Rejected" && t.RegionID == regionId && t.ProgramID == programId);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var requestViewModels = RequisitionViewModelBinder.BindReliefRequisitionListViewModel(requests, datePref).OrderByDescending(m => m.RequisitionID);

            return View(requestViewModels.ToList());
        }

        [HttpPost]
        public ActionResult PromotThese(List<ReliefRequisitionViewModel> reliefRequisitionViewModels)
        {
            foreach (var rrvm in reliefRequisitionViewModels)
            {
                if(!rrvm.IsSelected) continue;
               
                var fileName = string.Empty;

                if (rrvm.AttachmentFile.HasFile())
                {
                    //save the file
                    fileName = rrvm.AttachmentFile.FileName;

                    var path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        var indexOfDot = fileName.IndexOf(".", StringComparison.Ordinal);
                        fileName = fileName.Insert(indexOfDot - 1, GetRandomAlphaNumeric(6));
                        path = Path.Combine(Server.MapPath("~/Content/Attachment/"), fileName);
                    }

                    rrvm.AttachmentFile.SaveAs(path);
                }

                var businessProcessState = new BusinessProcessState()
                {
                    StateID = rrvm.ApprovedId,
                    PerformedBy = HttpContext.User.Identity.Name,
                    DatePerformed = DateTime.Now,
                    Comment = rrvm.Comment,
                    AttachmentFile = fileName,
                    ParentBusinessProcessID = rrvm.BusinessProcessID
                };

                var requisition = _reliefRequisitionService.FindBy(b => b.BusinessProcessID == rrvm.BusinessProcessID).FirstOrDefault();
                string stateName = _stateTemplateService.FindById(rrvm.ApprovedId).Name;

                if (requisition != null)
                {
                    int requisitionId = requisition.RequisitionID;

                    if (stateName == "Approved" && OnApprove(requisitionId))
                    {
                        _businessProcessService.PromotWorkflow(businessProcessState);
                    }
                }
            }

            return RedirectToAction("Index", "ReliefRequisition", new { Area = "EarlyWarning" });
        }

        //public  JsonResult CancelChanges(List<DataFromGrid> input)
        //{
        //    List<int> ids = new List<int>();
        //    if (input!=null)
        //    {
        //        foreach (var id in ids)
        //        {

        //        }

        //    }
        //    return Json(ids, JsonRequestBehavior.AllowGet);
        //}

        #region Transaction process methods - these will be called upon workflow promotion

        public bool OnApprove(int requisitionId)
        {
            var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == requisitionId, null,
                           "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                SendNotification(requisition);
                return _transactionService.PostRequestAllocation(requisitionId);
            }
            
            return false;
        }

        #endregion
    }
}