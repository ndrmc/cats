using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Services.Procurement.Annotations;
using Cats.Services.Security;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using log4net;
using Cats.Helpers;
using Cats.Models.ViewModels;
using Cats.ViewModelBinder;
using Cats.Security;
using Cats.Services.Workflows;

namespace Cats.Areas.EarlyWarning.Controllers
{
    public class NeedAssessmentController : Controller
    {
        private readonly IBusinessProcessService _businessProcessService;
        private readonly INeedAssessmentService _needAssessmentService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly INeedAssessmentHeaderService _needAssessmentHeaderService;
        private readonly INeedAssessmentDetailService _needAssessmentDetailService;
        private readonly ISeasonService _seasonService;
        private readonly ITypeOfNeedAssessmentService _typeOfNeedAssessmentService;
        private readonly ILog _log;
        private readonly IPlanService _planService;
        private readonly ICommonService _commonService;
        private readonly IUserAccountService _userAccountService;
        private readonly INotificationService _notificationService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IWorkflowActivityService _workflowActivityService;
        public NeedAssessmentController(INeedAssessmentService needAssessmentService,
                                        IAdminUnitService adminUnitService,
                                        INeedAssessmentHeaderService needAssessmentHeaderService,
                                        INeedAssessmentDetailService needAssessmentDetailService,
                                        ISeasonService seasonService, ITypeOfNeedAssessmentService typeOfNeedAssessmentService,
                                        ILog log, 
                                        IPlanService planService,
                                        ICommonService commonService,IUserAccountService userAccountService,
                                        INotificationService notificationService,
                                        IBusinessProcessService businessProcessService, 
                                        IApplicationSettingService applicationSettingService,
                                        IWorkflowActivityService workflowActivityService)
        {
            _needAssessmentService = needAssessmentService;
            _adminUnitService = adminUnitService;
            _needAssessmentHeaderService = needAssessmentHeaderService;
            _needAssessmentDetailService = needAssessmentDetailService;
            _seasonService = seasonService;
            _typeOfNeedAssessmentService = typeOfNeedAssessmentService;
            _log = log;
            _planService = planService;
            _commonService = commonService;
            _userAccountService = userAccountService;
            _notificationService = notificationService;
            _businessProcessService = businessProcessService;
            _applicationSettingService = applicationSettingService;
            _workflowActivityService = workflowActivityService;
        }

      
        // GET: /EarlyWarning/NeedAssessment/
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.View_Draft_Needs_Assessment)]
        public ActionResult Index(string id="")
        {

            ViewBag.AssessmentStatus = id;
            ViewData["zones"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 3);
            ViewData["woredas"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 4);
            ViewBag.userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            ViewBag.TargetController = "NeedAssessment";

            var needAssessment = _needAssessmentService.GetAllNeedAssessment().Select(m => m.PlanID).Distinct();
            List<Plan> plans;
            if (id == "")
                plans =
                    _planService.Get(
                        m =>
                            m.Program.Name == "Relief" &&
                            m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Draft"
                            || m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "AssessmentCreated",
                        null,
                        "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                        .OrderByDescending(m => m.PlanID)
                        .ToList();
            else
                plans =
                    _planService.Get(
                        m => needAssessment.Contains(m.PlanID) && m.Program.Name == "Relief" &&
                             m.BusinessProcess.CurrentState.BaseStateTemplate.Name == id, null,
                        "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                        .OrderByDescending(m => m.PlanID)
                        .ToList();

            foreach (var plan in plans)
            {
                var assessment = needAssessment as IList<int> ?? needAssessment.ToList();
                if (assessment.Contains(plan.PlanID)) plan.NeedAssessmentCreated = true;
                else plan.NeedAssessmentCreated = false;
            }
            //Commented out as there is no need of model binding for the needs assessment plan
            //var statuses = _commonService.GetStatus(WORKFLOW.Plan);
            //var needAssesmentsViewModel = NeedAssessmentViewModelBinder.GetNeedAssessmentPlanInfo(plans, statuses);
            //ModelState.AddModelError("Success", "Sample Error Message. Use in Your Controller: ModelState.AddModelError('Errors', 'Your Error Message.')");
            //return View(needAssesmentsViewModel);

            return View(plans);
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
                    fileName = fileName.Insert(indexOfDot-1, GetRandomAlphaNumeric(6));
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
            if(statusId!=null)
                return RedirectToAction("Detail", "NeedAssessment", new { Area = "EarlyWarning", statusId });
            return RedirectToAction("Index", "NeedAssessment", new { Area = "EarlyWarning" });
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

        public ActionResult Edit(int id, int typeOfNeed)
        {
            ViewData["TypeOfNeedAssessment"] =
                _typeOfNeedAssessmentService.FindBy(t => t.TypeOfNeedAssessmentID == typeOfNeed).Select(
                    a => a.TypeOfNeedAssessment1).SingleOrDefault();
           
            var region = _needAssessmentService.FindBy(t => t.NeedAID == id).SingleOrDefault();
            if (region != null) ViewData["region"] = region.Region;
            ViewData["Id"] = id;
            if (region != null) ViewData["RegionName"] = region.AdminUnit.Name;

            if (region != null) ViewBag.Zones = _adminUnitService.GetZones(region.Region).ToList();

            var needAssessment = _needAssessmentService.FindById(id);
            ViewData["Status"] = needAssessment.Plan.Status;
            return View(needAssessment);
        }
        public ActionResult Approved()
        {
            return View();
        }
        public ActionResult GetRegions()
        {
            IOrderedEnumerable<RegionsViewModel> regions = _needAssessmentService.GetRegions();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetZones(int region)
        {

            var zones = _needAssessmentService.GetZoness(region);
            return Json(zones, JsonRequestBehavior.AllowGet);

        }
       
        public ActionResult CreateNeedAssessment()
        {
            var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            if (userRegionID != null)
            {
                ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitID == userRegionID), "AdminUnitID", "Name");
            }
            else
            {
                ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");
            }
            ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
            ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(), "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
            ViewBag.PlanID = new SelectList(_planService.GetAllPlan(), "PlanID", "PlanName");
            var needAssessement = new NeedAssessment();
            return View(needAssessement);
        }

        [HttpPost]
        public ActionResult CreateNeedAssessment(NeedAssessment needAssessment, FormCollection collection)
        {
           
           
             ViewBag.Error = "";
             //var region = collection["RegionID"].ToString(CultureInfo.InvariantCulture);
             //var regionID = int.Parse(region);
             int season = int.Parse(collection["SeasonID"].ToString(CultureInfo.InvariantCulture));
             int typeOfNeedID = int.Parse(collection["TypeOfNeedID"].ToString(CultureInfo.InvariantCulture));
             string planName = collection["Plan.PlanName"].ToString(CultureInfo.InvariantCulture);
             DateTime startDate = DateTime.Parse(collection["Plan.StartDate"].ToString(CultureInfo.InvariantCulture));
             var firstDayOfTheMonth = startDate.AddDays(1 - startDate.Day);
             var duration = int.Parse(collection["Plan.Duration"].ToString(CultureInfo.InvariantCulture));
             //DateTime endDate = DateTime.Parse(collection["Plan.EndDate"].ToString(CultureInfo.InvariantCulture));
             var endDate = firstDayOfTheMonth.AddMonths(duration);
             if (ModelState.IsValid)
             {
                 var existingPlan = _planService.FindBy(m => m.PlanName == planName && m.ProgramID==1).FirstOrDefault();
                 if (existingPlan != null)
                 {
                     ModelState.AddModelError("Errors", @"Needs Assessment Name already Exists Please Change the Name");
                 }
                 else
                 {

                    try
                    {
                        int BP_PR = _applicationSettingService.getNeedAssessmentPlanWorkflow();
                        if (BP_PR != 0)
                        {
                            BusinessProcessState createdstate = new BusinessProcessState
                            {
                                DatePerformed = DateTime.Now,
                                PerformedBy = User.Identity.Name,
                                Comment = "Needs Assessment Plan Created"

                            };
                            //_PaymentRequestservice.Create(request);

                            BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                                                                                            "NeedAssessmentPlan", createdstate);
                            if (bp != null)
                                _planService.AddNeedAssessmentPlan(planName, firstDayOfTheMonth, endDate, bp.BusinessProcessID);
                            //var plan = _planService.Get(p => p.PlanName == planName).Single();
                            //var userID = _needAssessmentHeaderService.GetUserProfileId(HttpContext.User.Identity.Name);
                            //_needAssessmentService.AddNeedAssessment(plan.PlanID, regionID, season, userID, typeOfNeedID);
                            return RedirectToAction("Index");
                        }
                    }

                     catch (Exception exception)
                     {
                         var log = new Logger();
                         log.LogAllErrorsMesseges(exception, _log);
                         ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2),
                                                          "AdminUnitID", "Name");
                         ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
                         ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(),
                                                             "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
                         ViewBag.Error = "Need Assessment Already Exists Please Change Plan Name or Region Name";
                         ModelState.AddModelError("Errors", ViewBag.Error);
                         return View();
                     }
                 }

                 //return RedirectToAction("Edit", new { id = regionID, typeOfNeed = typeOfNeedID });
             }
            ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");
           ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
           ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(), "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
            return View(needAssessment);

        }
        public ActionResult NeedAssessmentPlan()
        {
            return View();
        }
        public ActionResult NeedAssessment_Plan([DataSourceRequest] DataSourceRequest request,int id=0)
        {
            var needAssessment = _needAssessmentService.GetAllNeedAssessment().Select(m => m.PlanID).Distinct();
            List<Plan> plans;
            if (id == 0)
                plans =
                    _planService.FindBy(
                        m => m.Program.Name == "Relief" && ( m.Status == (int) PlanStatus.Draft || m.Status == (int) PlanStatus.AssessmentCreated))
                        .OrderByDescending(m => m.PlanID)
                        .ToList();
            else
                plans =
                    _planService.FindBy(
                        m => needAssessment.Contains(m.PlanID) && m.Program.Name == "Relief" && m.Status == id)
                        .OrderByDescending(m => m.PlanID)
                        .ToList();
            var statuses = _commonService.GetStatus(WORKFLOW.Plan);
            var needAssesmentsViewModel = NeedAssessmentViewModelBinder.GetNeedAssessmentPlanInfo(plans,statuses);
            return Json(needAssesmentsViewModel.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult NeedAssessmentRead([DataSourceRequest] DataSourceRequest request)
        {
            var needAssessment = _needAssessmentService.FindBy(g => g.NeedAApproved == false).OrderByDescending(m=>m.NeedAID).ToList(); //featch unapproved need assessments
            var needAssesmentsViewModel = NeedAssessmentViewModelBinder.ReturnViewModel(needAssessment);
            return Json(needAssesmentsViewModel.ToDataSourceResult(request));

        }
        public ActionResult NeedAssessmentHeaderRead([DataSourceRequest] DataSourceRequest request)
        {

            return Json(_needAssessmentService.GetListOfZones().ToDataSourceResult(request));

        }
        public ActionResult NeedAssessmentDetailRead([DataSourceRequest] DataSourceRequest request, int region)//, string season)
        {
            var woredas = _needAssessmentDetailService.FindBy(z => z.NeedAssessmentHeader.NeedAssessment.NeedAID == region);// .NeedAssessmentHeader.AdminUnit.ParentID == region);
            var needAssesmentsViewModel = NeedAssessmentViewModelBinder.ReturnNeedAssessmentDetailViewModel(woredas);
            return Json(needAssesmentsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);


        }

        public ActionResult NeedAssessmentReadApproved([DataSourceRequest] DataSourceRequest request)
        {

            var needAssessment =
                _needAssessmentService.FindBy(
                    g => g.NeedAApproved == true); //featch unapproved need assessments
            var needAssesmentsViewModel = NeedAssessmentViewModelBinder.ReturnViewModelApproved(needAssessment);
            return Json(needAssesmentsViewModel.ToDataSourceResult(request));

        }

        public ActionResult DisapproveNeedAssessment(int id)
        {
            var needAssessment = _needAssessmentService.FindById(id);
            needAssessment.NeedAApproved = false;
            _needAssessmentService.EditNeedAssessment(needAssessment);
            return RedirectToAction("Index");
        }

        public ActionResult ApproveNeedAssessment(int id)
        {
            var plan = _planService.FindById(id);
            plan.Status = (int) PlanStatus.Approved;
            var status = _planService.EditPlan(plan);
            //if(status)SendNotification(plan);
            return RedirectToAction("Index");
        }

        public ActionResult ApprovedNeedAssessment()
        {
            ViewBag.AssessmentStatus = (int) PlanStatus.Approved;
            ViewData["zones"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 3);
            ViewData["woredas"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 4);
            return View();
        }

        public ActionResult HRDCreateAssessment()
        {
            ViewBag.AssessmentStatus = (int)PlanStatus.HRDCreated;
            ViewData["zones"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 3);
            ViewData["woredas"] = _adminUnitService.FindBy(t => t.AdminUnitTypeID == 4);
            return View();
        }

        public ActionResult EditNeedAssessment(int id)
        {
            try
            {
                var needAssessment = _needAssessmentService.FindBy(r => r.NeedAID == id).Single();
                int typeOfNeedAsseessment = (int)needAssessment.TypeOfNeedAssessment;
                return RedirectToAction("Edit", new { id = id, typeOfNeed = typeOfNeedAsseessment });
            }
            catch (Exception exception)
            {
                var log = new Logger();
                log.LogAllErrorsMesseges(exception, _log);
                return RedirectToAction("Index");
            }


        }

        public ActionResult DeleteNeedAssessment(int id)
        {
            try
            {

                var needAssessment = _needAssessmentService.FindBy(r => r.NeedAID == id).Single();
                if (!_needAssessmentService.IsNeedAssessmentUsedInHrd(needAssessment.PlanID))
                {
                    _needAssessmentService.DeleteById(id);
                    //ModelState.AddModelError("Success", "Need Requirment is deleted.");
                    //TempData["ModelState"] = ModelState;
                    return RedirectToAction("Index");
                }
                else
                {
                    //ModelState.AddModelError("Errors","Need Requirment can not be deleted. Need Requirment is already used in HRD.");
                    //TempData["ModelState"] = ModelState;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception exception)
            {
                var log = new Logger();
                log.LogAllErrorsMesseges(exception, _log);
                ModelState.AddModelError("Errors", @"Unable to delete this need Assessment");
                return RedirectToAction("Index");
            }



        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NeedAssessmentUpdate([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]List<NeedAssessmentDetail> needAssessmentlDetails)
        {
            List<NeedAssessmentDetail> result = new List<NeedAssessmentDetail>();
            if (needAssessmentlDetails != null && ModelState.IsValid)
            {
                bool deatilUpdated = false;
                var needAssessmentId = 0;
                 
                foreach (NeedAssessmentDetail details in needAssessmentlDetails)
                {
                   // details.
                   //deatilUpdated = _needAssessmentDetailService.EditNeedAssessmentDetail(details);
                    //details.
                    var record = _needAssessmentDetailService.FindById(details.NAId);
                    record.ProjectedFemale = details.ProjectedFemale;
                    record.ProjectedMale = details.ProjectedMale;
                    deatilUpdated = deatilUpdated || _needAssessmentDetailService.EditNeedAssessmentDetail(record);
                    //if (record != null)
                    //{
                    //    result.Add(record);
                    //}
                    needAssessmentId = Convert.ToInt32(record.NeedAId);
                    var id = record.NAId;
                }
                if (deatilUpdated)
                {
                    var needAssessmentDetail = needAssessmentlDetails.FirstOrDefault();
                    if (needAssessmentDetail != null)
                    {
                        var needAssessment = _needAssessmentHeaderService.FindById(needAssessmentId);
                        if (needAssessment != null)
                        {
                           //_workflowActivityService.EnterEditWorkflow(needAssessment.NeedAssessment);
                            var businessProcessId = needAssessment.NeedAssessment.BusinessProcessID;
                            var globalEditeStateId = _businessProcessService.GetGlobalEditStateTempId();
                            var businesProcessState = new BusinessProcessState()
                            {
                                Comment = "Detail Edited",
                                DatePerformed = DateTime.Now,
                                ParentBusinessProcessID = businessProcessId,
                                StateID = globalEditeStateId,
                                PerformedBy = User.Identity.Name
                            };
                            _businessProcessService.InsertBusinessProcessState(businesProcessState);
                        }

                    }
                }
            }
            //var needAssesmentsViewModel = NeedAssessmentViewModelBinder.ReturnNeedAssessmentDetailViewModel(result);
            return Json(new[] { needAssessmentlDetails }.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return Json(needAssessmentlDetails.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
           // return Json(ModelState.ToDataSourceResult());
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetail([DataSourceRequest] DataSourceRequest request, NeedAssessmentViewModel needAssessmentViewModel)
        {
            try
            {
                // TODO: Add delete logic here
                var needAssessment = _needAssessmentService.FindBy(e => e.NeedAID == needAssessmentViewModel.NAId).Single();
                _needAssessmentService.DeleteNeedAssessment(needAssessment);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                var log = new Logger();
                log.LogAllErrorsMesseges(exception, _log);
                return RedirectToAction("Index");
            }
        }
        public ActionResult Detail(int id=0)
        {
            var plan = _planService.FindBy(m => m.PlanID == id).OrderByDescending(m=>m.PlanID).FirstOrDefault();
            ViewBag.plan = plan;
            ViewBag.userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
            if (plan != null)
            {
                ViewBag.Status = plan.BusinessProcess.CurrentState.BaseStateTemplate.Name;
                ViewBag.TargetController = "NeedAssessment";
                var regionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
                var needAssessment = regionID == null ? 
                    _needAssessmentService.Get(m => m.PlanID == id && 
                                                    (m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Submitted to EW" ||
                                                     m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved"), null,
                        "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").OrderByDescending(m => m.NeedAID).ToList() :
                    _needAssessmentService.Get(m => m.PlanID == id && m.Region == regionID &&
                                                    (m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Draft" ||
                                                     m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Reversed"), null,
                        "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").OrderByDescending(m => m.NeedAID).ToList();
                
                return View(needAssessment);
            }
            return null;
        }
      public ActionResult PlannedNeedAssessmentInfo_Read([DataSourceRequest] DataSourceRequest request,int id=0)
      {
          var regionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID; 
          var needAssessment =regionID==null? _needAssessmentService.FindBy(m=>m.PlanID==id).OrderByDescending(m => m.NeedAID).ToList():
                                              _needAssessmentService.FindBy(m => m.PlanID == id && m.Region==regionID).OrderByDescending(m => m.NeedAID).ToList(); 
          var needAssesmentsViewModel = NeedAssessmentViewModelBinder.ReturnViewModel(needAssessment);
          return Json(needAssesmentsViewModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
      }
     public ActionResult AddNeedAssessment(int id)
     {
         var needAssessment = _needAssessmentService.FindBy(m => m.PlanID == id).FirstOrDefault();
         ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(), "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
         var userRegionID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).RegionID;
         if (userRegionID != null)
         {
             ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitID == userRegionID), "AdminUnitID", "Name");
         }
         else
         {
             ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");
         }
         
         ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
         if (needAssessment!=null)
         {
             return View(needAssessment);
         }
         var newAssessment = new NeedAssessment {PlanID = id};
         return View(newAssessment);
     }
    [HttpPost]
    public ActionResult AddNeedAssessment(NeedAssessment needAssessment,FormCollection collection)
    {
        var region = collection["RegionID"].ToString(CultureInfo.InvariantCulture);
        var regionID = int.Parse(region);
        int season = int.Parse(collection["SeasonID"].ToString(CultureInfo.InvariantCulture));
        int planID = int.Parse(collection["PlanID"].ToString(CultureInfo.InvariantCulture));
        int typeOfNeedID = int.Parse(collection["TypeOfNeedID"].ToString(CultureInfo.InvariantCulture));
        var userID = _needAssessmentHeaderService.GetUserProfileId(HttpContext.User.Identity.Name);
        try
        {
            int BP_PR = _applicationSettingService.getNeedAssessmentWorkflow();
            if (BP_PR != 0)
            {
                BusinessProcessState createdstate = new BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = User.Identity.Name,
                    Comment = "Needs Assessment Created"

                };
                //_PaymentRequestservice.Create(request);

                BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                                                                                "NeedAssessment", createdstate);
                if (bp != null)
                    _needAssessmentService.AddNeedAssessment(needAssessment.PlanID, regionID, season, userID, typeOfNeedID, bp.BusinessProcessID);
                    //var plan = _planService.Get(p => p.PlanName == planName).Single();
                    //var userID = _needAssessmentHeaderService.GetUserProfileId(HttpContext.User.Identity.Name);
                    //_needAssessmentService.AddNeedAssessment(plan.PlanID, regionID, season, userID, typeOfNeedID);
                return RedirectToAction("Detail", "NeedAssessment", new { id = needAssessment.PlanID });
            }
            return RedirectToAction("Index");
        }
        catch (Exception exception)
        {

            var log = new Logger();
            log.LogAllErrorsMesseges(exception, _log);
            //ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2), "AdminUnitID", "Name");
            //ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
            //ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(), "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
            ViewBag.Error = "Need Assessment is already Created for this region";
            //ModelState.AddModelError("Errors", ViewBag.Error);
            return RedirectToAction("Detail", "NeedAssessment", new { id = needAssessment.PlanID });
        }
    }
        /// <summary>
        /// Send notification to regional users
        /// </summary>
        /// <param name="Plan"></param>
        private void SendNotification(Plan Plan)
        {
            if (Request.Url != null && Request.Url.Host == "localhost")
                return;
                    
            var destinationURl =
                                    "/EarlyWarning/NeedAssessment/Detail/" +
                                    Plan.PlanID;

            _notificationService.AddNotificationForRegionOnNeedAssessmentApproval(destinationURl, Plan);
        }
    }
}
