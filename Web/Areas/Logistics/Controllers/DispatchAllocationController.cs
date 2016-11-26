using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.Services.Transaction;
using Cats.ViewModelBinder;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cats.Helpers;
using Cats.Models.ViewModels;
using IAdminUnitService = Cats.Services.EarlyWarning.IAdminUnitService;


namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class DispatchAllocationController : Controller
    {
        //
        // GET: /Logistics/DispatchAllocation/

        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IHubService _hubService;
        private readonly IHubAllocationService _hubAllocationService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly INeedAssessmentService _needAssessmentService;
        private readonly IAllocationByRegionService _allocationByRegionService;
        private readonly IUserAccountService _userAccountService;
        private readonly ITransactionService _transactionService;
        private readonly IBusinessProcessService _businessProcessService;
        //private readonly IStoreService _storeService;

        public DispatchAllocationController(IReliefRequisitionService reliefRequisitionService,
            IHubService hubService, IAdminUnitService adminUnitService,
            INeedAssessmentService needAssessmentService,
            IHubAllocationService hubAllocationService,
            IUserAccountService userAccountService,
            IAllocationByRegionService allocationByRegionService, ITransactionService transactionService, IBusinessProcessService businessProcessService)
        {
            _reliefRequisitionService = reliefRequisitionService;
            _hubService = hubService;
            _adminUnitService = adminUnitService;
            _needAssessmentService = needAssessmentService;
            _hubAllocationService = hubAllocationService;
            _userAccountService = userAccountService;
            _allocationByRegionService = allocationByRegionService;
            _transactionService = transactionService;
            _businessProcessService = businessProcessService;
        }




        public ActionResult Index(int regionId = -1)
        {

            ViewBag.TargetController = "DispatchAllocation";
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //hubContext.Clients.All.receiveNotification("this is a sample data");

            ViewBag.regionId = regionId;
            ViewBag.Region = new SelectList(_adminUnitService.GetRegions(), "AdminUnitID", "Name");
            return View();
        }
        public ActionResult AllocationAdjustment(int requisitionId)
        {
            var requisition = _reliefRequisitionService.FindById(requisitionId);
            var data = new List<int> { requisitionId, requisition.RegionID.Value };
            return View(data);
        }

        //#region "test"

        //public ActionResult Main()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public JsonResult HubAllocationByRegion(int regionId = -1)
        //{
        //    List<AllocationByRegion> requisititions = null;
        //    requisititions = regionId != -1 ? _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned && r.RegionID == regionId) : _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned);

        //    var requisitionViewModel = BindAllocation(requisititions);// HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);

        //    return Json(requisitionViewModel,JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult AllocatedProjectCode(int regionId = -1,int status=-1)
        //{
        //    if (regionId < 0 || status < 0) return Json(new List<RequisitionViewModel>(), JsonRequestBehavior.AllowGet);
        //    var requisititions = new List<ReliefRequisition>();
        //    requisititions = _reliefRequisitionService.FindBy(r => r.Status == status && r.RegionID == regionId);

        //    var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
        //    return Json(requisitionViewModel,JsonRequestBehavior.AllowGet);
        //}


        //#endregion

        public ActionResult GetRegions()
        {
            IOrderedEnumerable<RegionsViewModel> regions = _needAssessmentService.GetRegions();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HubAllocation([DataSourceRequest]DataSourceRequest request, int regionId)
        {
            List<AllocationByRegion> requisititions = null;
            requisititions = regionId != -1
                                 ? _allocationByRegionService.FindBy(
                                     r =>
                                     r.Status == (int)ReliefRequisitionStatus.HubAssigned && r.RegionID == regionId)
                                 : null;// _AllocationByRegionService.FindBy(r => r.Status == (int)ReliefRequisitionStatus.HubAssigned);

            var requisitionViewModel = BindAllocation(requisititions);// HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);

            return Json(requisitionViewModel.ToDataSourceResult(request));
        }

        public ActionResult AllocateProjectCode([DataSourceRequest]DataSourceRequest request, int regionId, string status)
        {
            ViewBag.requestStatus = status;
            List<ReliefRequisition> requisititions = null;
            if (regionId == -1 || status == "") return Json((new List<RequisitionViewModel>()).ToDataSourceResult(request));
            if (status == "Project Code Assigned")
            {
                requisititions = _reliefRequisitionService.Get(r => r.BusinessProcess.CurrentState.BaseStateTemplate.Name == status
                        || r.BusinessProcess.CurrentState.BaseStateTemplate.Name == "SiPc Allocation Approved" && r.RegionID == regionId,
                        null, "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                        .OrderByDescending(t => t.RequisitionID).ToList();
            }
            else
            {
                requisititions = _reliefRequisitionService.Get(
                r => r.BusinessProcess.CurrentState.BaseStateTemplate.Name == status && r.RegionID == regionId,
                        null, "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                .OrderByDescending(t => t.RequisitionID).ToList();
            }

            var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
            return Json(requisitionViewModel.ToDataSourceResult(request));
        }


        public ActionResult IndexFromNotification(int paramRegionId, int recordId)
        {
            ViewBag.regionId = paramRegionId;
            NotificationHelper.MakeNotificationRead(recordId);
            return RedirectToAction("Hub", new { regionId = paramRegionId });

        }

        public ActionResult AssignHubFromLogisticsDashboard(int paramRegionId)
        {
            ViewBag.regionId = paramRegionId;
            return RedirectToAction("Hub", new { regionId = paramRegionId });

        }
        public ActionResult Hub(int regionId)
        {
            if (regionId != -1)
            {
                ViewBag.regionId = regionId;
                ViewBag.RegionName = _adminUnitService.GetRegions().Where(r => r.AdminUnitID == regionId).Select(r => r.Name).Single();
                ViewData["Hubs"] = _hubService.FindBy(h => h.HubOwnerID == 1);
                // ViewData["Stores"] = _storeService.FindBy(s => s.Hub.HubOwnerID == 1); //get DRMFSS stores
                return View();
            }
            return View();
        }

        [HttpGet]
        public JsonResult ReadSWarehouse(int hubId)
        {
            var SWarehouse = _hubService.GetAllHub().Where(r => r.HubParentID == hubId);
            return this.Json(
           (from obj in SWarehouse select new { Id = obj.HubID, Name = obj.Name })
           , JsonRequestBehavior.AllowGet
           );
            // var requisitionViewModel = HubViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
            //return Json(SWarehouse, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ReadRequisitions(int regionId)
        {
            var requisititions = _reliefRequisitionService.Get(r => r.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved" && r.RegionID == regionId,
                                 null, "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").ToList();
            var requisitionViewModel = HubAllocationViewModelBinder.ReturnRequisitionGroupByReuisitionNo(requisititions);
            return Json(requisitionViewModel, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Http.HttpPost]
        public JsonResult Save(List<Allocation> allocation)
        {
            var userName = HttpContext.User.Identity.Name;
            var user = _userAccountService.GetUserDetail(userName);

            try
            {
                foreach (var all in allocation)
                {

                    var hubAllocated = _hubAllocationService.FindBy(h => h.RequisitionID == all.ReqId).FirstOrDefault();



                    if (hubAllocated != null)
                    {


                        hubAllocated.AllocatedBy = user.UserProfileID;
                        hubAllocated.AllocationDate = DateTime.Now.Date;
                        hubAllocated.HubID = all.HubId;

                        // hubAllocated.StoreId = all.StoreId;

                        hubAllocated.SatelliteWarehouseID = all.SatelliteWarehouseID;

                        _hubAllocationService.EditHubAllocation(hubAllocated);
                        var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == all.ReqId, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
                        if (requisition != null)
                        {
                            var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Assign Hub");
                            if (approveFlowTemplate != null)
                            {
                                var businessProcessState = new BusinessProcessState()
                                {
                                    StateID = approveFlowTemplate.FinalStateID,
                                    PerformedBy = HttpContext.User.Identity.Name,
                                    DatePerformed = DateTime.Now,
                                    Comment = "Requisition has been re-assigned a hub",
                                    //AttachmentFile = fileName,
                                    ParentBusinessProcessID = requisition.BusinessProcessID
                                };
                                //return 
                                _businessProcessService.PromotWorkflow(businessProcessState);
                            }
                        }
                    }
                    else
                    {
                        _hubAllocationService.AddHubAllocations(allocation, user.UserProfileID);
                        var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == all.ReqId, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
                        if (requisition != null)
                        {
                            var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Assign Hub");
                            if (approveFlowTemplate != null)
                            {
                                var businessProcessState = new BusinessProcessState()
                                {
                                    StateID = approveFlowTemplate.FinalStateID,
                                    PerformedBy = HttpContext.User.Identity.Name,
                                    DatePerformed = DateTime.Now,
                                    Comment = "Requisition has been assigned a hub",
                                    //AttachmentFile = fileName,
                                    ParentBusinessProcessID = requisition.BusinessProcessID
                                };
                                //return 
                                _businessProcessService.PromotWorkflow(businessProcessState);
                            }
                        }
                    }


                }

                ModelState.AddModelError("Success", @"Allocation is Saved.");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }

        }


        public ActionResult RegionId(int id)
        {
            return RedirectToAction("Index", new { regionId = id });
        }



        public List<HubAllocationByRegionViewModel> BindAllocation(List<AllocationByRegion> reliefRequisitions)
        {

            try
            {
                if (reliefRequisitions == null)
                    return new List<HubAllocationByRegionViewModel>();

                //var result = (reliefRequisitions.Select(req => new HubAllocationByRegionViewModel()
                //{
                //    Region = req.Name,
                //    RegionId = (int)req.RegionID,
                //    AdminUnitID = (int)req.RegionID,
                //    Hub = req.Hub,
                //    AllocatedAmount = ((decimal)req.Amount).ToPreferedWeightUnit()
                //}));

                var r = new List<HubAllocationByRegionViewModel>();

                foreach (var allocationByRegion in reliefRequisitions)
                {
                    var allc = new HubAllocationByRegionViewModel();

                    allc.Region = allocationByRegion.Name;
                    allc.RegionId = (int)allocationByRegion.RegionID;
                    allc.AdminUnitID = (int)allocationByRegion.RegionID;
                    allc.Hub = allocationByRegion.Hub;
                    allc.AllocatedAmount = ((decimal)allocationByRegion.Amount).ToPreferedWeightUnit();

                    r.Add(allc);
                }

                return Enumerable.Cast<HubAllocationByRegionViewModel>(r).ToList();
            }
            catch
            {

                return new List<HubAllocationByRegionViewModel>();
            }


        }

        public ActionResult RejectRequsition(int id)
        {
            //var requistion = _reliefRequisitionService.FindById(id);
            var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == id, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Reject");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "Requisition has been rejected",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = requisition.BusinessProcessID
                    };
                    //return 
                    _businessProcessService.PromotWorkflow(businessProcessState);

                    return RedirectToAction("Index", new { regionId = requisition.RegionID });
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult UncommitRequsition(int id)
        {
            var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == id, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Uncommit");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "Requisition has been uncommitted",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = requisition.BusinessProcessID
                    };
                    //return 
                    _transactionService.PostSIAllocationUncommit(id);
                    _businessProcessService.PromotWorkflow(businessProcessState);

                    return RedirectToAction("Index", new { regionId = requisition.RegionID });
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult ApproveSiPcAllocation(int id)
        {
            var requisition = _reliefRequisitionService.Get(t => t.RequisitionID == id, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Approve SI/PC Allocation");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "SI/PC Allocation of requisition has been approved",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = requisition.BusinessProcessID
                    };
                    //return 
                    _reliefRequisitionService.EditReliefRequisition(requisition);
                    _businessProcessService.PromotWorkflow(businessProcessState);

                    return RedirectToAction("Index", new { regionId = requisition.RegionID });
                }
            }
            return RedirectToAction("Index");
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
                return RedirectToAction("Index", "DispatchAllocation", new { Area = "Logistics", statusId });
            return RedirectToAction("Index", "DispatchAllocation", new { Area = "Logistics" });
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

        public ActionResult UpdateRequisition(int requisitionId)
        {
            var request = _reliefRequisitionService.FindBy(t => t.RequisitionID == requisitionId).FirstOrDefault();

            return View(request);
        }
        [HttpPost]
        public JsonResult UpdateRequisition(ReliefRequisition requisition)
        {
            if (requisition == null) return Json(new { status = "Bad", message = "Requistion is not found. Unable to update." });

            if (!requisition.IsTransfer)
            {
                return Json(new { status = "Bad", message = "Requisition is not of type Transfer. Unable to update.", regionId = requisition.RegionID });
            }

            var requisitionById = _reliefRequisitionService.FindById(requisition.RequisitionID); // get object from db
            var requisitionByReqNumber = _reliefRequisitionService.FindBy(r => r.RequisitionNo == requisition.RequisitionNo).FirstOrDefault();
            var requisitionNumber = _reliefRequisitionService.FindBy(r => r.RequisitionNo == requisition.RequisitionNo);

            if (requisitionByReqNumber != null)
            {
                if (requisitionByReqNumber.RequisitionID != requisitionById.RequisitionID &&
                    requisitionByReqNumber.RequisitionNo == requisition.RequisitionNo && requisitionNumber.Count > 0)
                {
                    return
                        Json(
                            new
                            {
                                status = "Bad",
                                message =
                                    "Duplicate requisition number, please change it and try again. Unable to update.",
                                regionId = requisition.RegionID
                            });
                }
            }

            requisitionById.RequisitionNo = requisition.RequisitionNo;

            _reliefRequisitionService.EditReliefRequisition(requisitionById);

            return Json(new { status = "Ok", message = "Requisition number has been successfully updated.", regionId = requisition.RegionID, Id = requisitionById.RequisitionID });
        }
    }
}

