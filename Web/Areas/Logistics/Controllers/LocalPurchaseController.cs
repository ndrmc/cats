﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.Logistics.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Cats.Services.Logistics;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StateTemplate = Cats.Models.StateTemplate;

namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class LocalPurchaseController : Controller
    {
        //
        // GET: /Logistics/LocalPurchase/
        private readonly ILocalPurchaseService _localPurchaseService;
        private readonly ICommonService _commonService;
        private readonly ILocalPurchaseDetailService _localPurchaseDetailService;
        private readonly IGiftCertificateService _giftCertificateService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private readonly ICommodityService _commodityService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IBusinessProcessService _businessProcessService;
        private readonly IStateTemplateService _stateTemplateService;

        public LocalPurchaseController(ILocalPurchaseService localPurchaseService, ICommonService commonService,
            ILocalPurchaseDetailService localPurchaseDetailService,
            IGiftCertificateService giftCertificateService, IShippingInstructionService shippingInstructionService,
            ICommodityService commodityService, IApplicationSettingService applicationSettingService, IBusinessProcessService businessProcessService,
            IStateTemplateService stateTemplateService)
        {
            _localPurchaseService = localPurchaseService;
            _commonService = commonService;
            _localPurchaseDetailService = localPurchaseDetailService;
            _giftCertificateService = giftCertificateService;
            _shippingInstructionService = shippingInstructionService;
            _commodityService = commodityService;
            _applicationSettingService = applicationSettingService;
            _businessProcessService = businessProcessService;
            _stateTemplateService = stateTemplateService;
        }

        public ActionResult Index()
        {
            if (TempData["success"] != null)
                ModelState.AddModelError("Success", TempData["success"].ToString());
            if (TempData["Reverted"] != null)
                ModelState.AddModelError("Success", TempData["Reverted"].ToString());
            else if (TempData["Received"] != null)
                ModelState.AddModelError("Errors", TempData["Received"].ToString());
            else if (TempData["Error"] != null)
                ModelState.AddModelError("Errors", TempData["Error"].ToString());

            ViewBag.TargetController = "LocalPurchase";
            return View();
        }
        public ActionResult Create()
        {
            PopulateLookUps();
            var localpurchase = new LocalPurchaseWithDetailViewModel
            {
                CommoditySource = _commonService.GetCommditySourceName(3),//commodity source for local purchase
                LocalPurchaseDetailViewModels = GetNewLocalPurchaseDetail()
            };
            return View(localpurchase);

        }
        public ActionResult Details(int id)
        {
            var localPurchase = _localPurchaseService.FindById(id);
            var parentCommodityID = _commodityService.Get(m => m.CommodityID == localPurchase.CommodityID).FirstOrDefault().ParentID;
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name", localPurchase.ProgramID);
            ViewBag.CommodityID = new SelectList(_commodityService.FindBy(m => m.ParentID == parentCommodityID), "CommodityID", "Name", localPurchase.CommodityID);
            ViewBag.CommodityTypeID = new SelectList(_commonService.GetCommodityTypes(), "CommodityTypeID", "Name");
            ViewBag.DonorID = new SelectList(_commonService.GetDonors(), "DonorID", "Name", localPurchase.DonorID);

            if (localPurchase != null)
            {
                var localPurchaseWithDetailViewModel = new LocalPurchaseWithDetailViewModel()
                {
                    LocalPurchaseID = localPurchase.LocalPurchaseID,
                    ProgramID = localPurchase.ProgramID,
                    DonorID = localPurchase.DonorID,
                    CommodityID = localPurchase.DonorID,
                    ProjectCode = localPurchase.ProjectCode,
                    SINumber = localPurchase.ShippingInstruction.Value,
                    ReferenceNumber = localPurchase.ReferenceNumber,
                    SupplierName = localPurchase.SupplierName,
                    PurchaseOrder = localPurchase.PurchaseOrder,
                    Quantity = localPurchase.Quantity,
                    StatusID = localPurchase.StatusID,
                    CommoditySource = _commonService.GetCommditySourceName(3), //commodity source for local purchase
                    LocalPurchaseDetailViewModels = GetLocalPurchaseDetail(localPurchase.LocalPurchaseDetails),
                    BusinessProcessID = localPurchase.BusinessProcessID,
                    BusinessProcess = localPurchase.BusinessProcess
                };
                if (TempData["CustomError"] != null)
                {
                    ModelState.AddModelError("Errors", TempData["CustomError"].ToString());
                }
                if (TempData["success"] != null)
                    ModelState.AddModelError("Success", TempData["success"].ToString());

                ViewBag.TargetController = "LocalPurchase";
                return View(localPurchaseWithDetailViewModel);

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Create(LocalPurchaseWithDetailViewModel localPurchaseWithDetailViewModel)
        {
            if (localPurchaseWithDetailViewModel != null &&
                localPurchaseWithDetailViewModel.Quantity >=
                localPurchaseWithDetailViewModel.LocalPurchaseDetailViewModels.Sum(m => m.AllocatedAmonut))
            {
                var shippingInstractionId = CheckAvilabilityOfSiNumber(localPurchaseWithDetailViewModel.SINumber);

                // Workflow Implementation
                int BP_PR = _applicationSettingService.GetLocalPurchaseReceiptPlanWorkflow();
                if (BP_PR != 0)
                {
                    var createdstate = new BusinessProcessState
                    {
                        DatePerformed = DateTime.Now,
                        PerformedBy = User.Identity.Name,
                        Comment = "Local Purchase Created"
                    };
                    var bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                        "Local Purchase Receipt plan", createdstate);


                    if (shippingInstractionId != 0)
                    {

                        if (!CheckAvailabilityOfSiInLocalPurchase(localPurchaseWithDetailViewModel.SINumber) && bp != null)
                        {

                            SaveNewLocalPurchase(localPurchaseWithDetailViewModel, shippingInstractionId, bp.BusinessProcessID);
                        }

                    }
                    else
                    {
                        var si = AddSiNumber(localPurchaseWithDetailViewModel.SINumber);
                        if (si != -1 && bp != null)
                            SaveNewLocalPurchase(localPurchaseWithDetailViewModel, si, bp.BusinessProcessID); // second in doation table
                    }
                    TempData["success"] = "Local Purchase Sucessfully Saved";
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("Errors", @"Total Allocated Amount Can't Exceed Planned Quantity");
            PopulateLookUps();
            return View(localPurchaseWithDetailViewModel);
        }
        public JsonResult GetMaxSINo()
        {
            var result =
                _localPurchaseService.GetAllLocalPurchase().Select(m => m.ShippingInstruction.Value);
            var siList = result.Select(si => Regex.Match(si, @"\d+").Value).Select(data => Convert.ToInt32(data)).ToList();
            int resultInt = siList.Max() + 1;
            return Json("LP-" + resultInt, JsonRequestBehavior.AllowGet);
        }
        private bool SaveNewLocalPurchase(LocalPurchaseWithDetailViewModel localPurchaseWithDetailViewModel, int sippingInstractionID, int businessProcessId)
        {
            try
            {
                var localPurchase = new LocalPurchase()
                {
                    DateCreated = DateTime.Now,
                    CommodityID = localPurchaseWithDetailViewModel.CommodityID,
                    DonorID = localPurchaseWithDetailViewModel.DonorID,
                    ProgramID = localPurchaseWithDetailViewModel.ProgramID,
                    ShippingInstructionID = sippingInstractionID,
                    PurchaseOrder = localPurchaseWithDetailViewModel.PurchaseOrder,
                    SupplierName = localPurchaseWithDetailViewModel.SupplierName,
                    Quantity = localPurchaseWithDetailViewModel.Quantity,
                    ReferenceNumber = localPurchaseWithDetailViewModel.ReferenceNumber,
                    ProjectCode = localPurchaseWithDetailViewModel.ProjectCode,
                    StatusID = (int)LocalPurchaseStatus.Draft,
                    BusinessProcessID = businessProcessId,

                };

                foreach (var localPurchaseDetail in localPurchaseWithDetailViewModel.LocalPurchaseDetailViewModels
                    .Select(localPurchaseDetail => new LocalPurchaseDetail()
                    {
                        HubID = localPurchaseDetail.HubID,
                        AllocatedAmount = localPurchaseDetail.AllocatedAmonut,
                        RecievedAmount = localPurchaseDetail.RecievedAmonut,
                        LocalPurchase = localPurchase
                    }))
                {
                    _localPurchaseDetailService.AddLocalPurchaseDetail(localPurchaseDetail);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ActionResult UpdateLocalPurchase(LocalPurchaseWithDetailViewModel localPurchaseDetailViewModel)
        {
            var localPurchase = _localPurchaseService.FindById(localPurchaseDetailViewModel.LocalPurchaseID);
            if (localPurchase != null && localPurchaseDetailViewModel.LocalPurchaseDetailViewModels.Sum(m => m.AllocatedAmonut) <= localPurchase.Quantity)
            {
                localPurchase.CommodityID = localPurchaseDetailViewModel.CommodityID;
                localPurchase.DonorID = localPurchaseDetailViewModel.DonorID;
                localPurchase.ProgramID = localPurchaseDetailViewModel.ProgramID;
                localPurchase.PurchaseOrder = localPurchaseDetailViewModel.PurchaseOrder;
                localPurchase.SupplierName = localPurchaseDetailViewModel.SupplierName;
                localPurchase.Quantity = localPurchaseDetailViewModel.Quantity;
                localPurchase.ReferenceNumber = localPurchaseDetailViewModel.ReferenceNumber;
                localPurchase.ProjectCode = localPurchaseDetailViewModel.ProjectCode;

                BusinessProcess bp = _businessProcessService.FindById(localPurchase.BusinessProcessID);
                BusinessProcessState bps = bp.CurrentState;
                StateTemplate stateTemplate = _stateTemplateService.FindBy(p => p.Name == ConventionalAction.Edited &&
                p.ParentProcessTemplateID == bps.BaseStateTemplate.ParentProcessTemplateID).FirstOrDefault();

                // Partial workflow implementation
                if (_localPurchaseService.EditLocalPurchase(localPurchase))
                {
                    if (stateTemplate != null)
                    {
                        var businessProcessState = new BusinessProcessState()
                        {
                            StateID = stateTemplate.StateTemplateID, // mark as edited
                            PerformedBy = HttpContext.User.Identity.Name,
                            DatePerformed = DateTime.Now,
                            Comment = "Local Purchase is edited, a system internally captured data.",
                            ParentBusinessProcessID = bps.ParentBusinessProcessID
                        };

                        // Promot
                        _businessProcessService.PromotWorkflow(businessProcessState);
                    }
                }

                string localPurchaseDetails = string.Empty;

                foreach (var localPurchaseDetail in localPurchaseDetailViewModel.LocalPurchaseDetailViewModels)
                {
                    var detail = _localPurchaseDetailService.FindById(localPurchaseDetail.LocalPurchaseDetailID);

                    if (detail != null)
                    {
                        detail.AllocatedAmount = localPurchaseDetail.AllocatedAmonut;

                        // Concatenate all saved details onto a string
                        if (_localPurchaseDetailService.EditLocalPurchaseDetail(detail))
                        {
                            localPurchaseDetails += "[" + detail.LocalPurchaseDetailID + "|" + detail.AllocatedAmount + "] ";
                        }
                    }
                }

                if (localPurchaseDetails != string.Empty)
                {
                    // Partial workflow implementation
                    if (stateTemplate != null)
                    {
                        var businessProcessState = new BusinessProcessState()
                        {
                            StateID = stateTemplate.StateTemplateID, // mark as edited
                            PerformedBy = HttpContext.User.Identity.Name,
                            DatePerformed = DateTime.Now,
                            Comment =
                                "Local Purchase detail is edited, " + localPurchaseDetails +
                                "\n a system internally captured data.",
                            ParentBusinessProcessID = bps.ParentBusinessProcessID
                        };

                        // Promot
                        _businessProcessService.PromotWorkflow(businessProcessState);
                    }
                }

                TempData["success"] = "Local Purchase Sucessfully Updated";
                //ModelState.AddModelError("Success", @"Local Purchase Sucessfully Updated");
                return RedirectToAction("Details", new { id = localPurchase.LocalPurchaseID });
            }
            TempData["CustomError"] = "Total Allocated Amount Can't Exceed Planned Quantity";
            return RedirectToAction("Details", new { id = localPurchaseDetailViewModel.LocalPurchaseID });
        }
        public ActionResult LocalPurchase_Read([DataSourceRequest] DataSourceRequest request)
        {
            var localPurchase = _localPurchaseService.GetAllLocalPurchase().OrderByDescending(m => m.LocalPurchaseID).ToList();
            var localPurchseToDisplay = GetLocalPurchase(localPurchase);
            return Json(localPurchseToDisplay.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult HubsLocalPurchaseDetail_Read([DataSourceRequest] DataSourceRequest request, int localPurchaseID)
        {
            var localPurchaseDetail = _localPurchaseDetailService.FindBy(m => m.LocalPurchaseID == localPurchaseID);
            if (localPurchaseDetail.Count != 0)
            {
                var localPurchaseToDisplay = GetLocalPurchaseDetail(localPurchaseDetail);
                return Json(localPurchaseToDisplay.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            var newLocalPurchaseDetail = GetNewLocalPurchaseDetail();
            return Json(newLocalPurchaseDetail.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<LocalPurchaseDetailViewModel> GetLocalPurchaseDetail(IEnumerable<LocalPurchaseDetail> localPurchaseDetails)
        {
            return (from localPurchaseDetail in localPurchaseDetails
                    select new LocalPurchaseDetailViewModel
                    {
                        LocalPurchaseDetailID = localPurchaseDetail.LocalPurchaseDetailID,
                        LocalPurchaseID = localPurchaseDetail.LocalPurchaseID,
                        HubID = localPurchaseDetail.HubID,
                        HubName = localPurchaseDetail.Hub.Name,
                        AllocatedAmonut = localPurchaseDetail.AllocatedAmount,
                        RecievedAmonut = localPurchaseDetail.RecievedAmount,
                        RemainingAmonut = _localPurchaseService.GetRemainingAmount(localPurchaseDetail.LocalPurchaseID)

                    });

        }
        private IEnumerable<LocalPurchaseViewModel> GetLocalPurchase(IEnumerable<LocalPurchase> localPurchases)
        {
            return (from localPurchase in localPurchases
                let status =
                    localPurchase.BusinessProcess.CurrentState != null
                        ? localPurchase.BusinessProcess.CurrentState.BaseStateTemplate.Name
                        : string.Empty
                //_commonService.GetStatusName(WORKFLOW.LocalPUrchase, localPurchase.StatusID)
                select new LocalPurchaseViewModel
                {
                    LocalPurchaseID = localPurchase.LocalPurchaseID,
                    CommodityID = localPurchase.CommodityID,
                    Commodity = localPurchase.Commodity!=null?localPurchase.Commodity.Name:string.Empty,
                    ProgramID = localPurchase.ProgramID,
                    Program = localPurchase.Program.Name,
                    DonorID = localPurchase.DonorID,
                    DonorName = localPurchase.Donor.Name,
                    SupplierName = localPurchase.SupplierName,
                    ReferenceNumber = localPurchase.ReferenceNumber,
                    SiNumber = localPurchase.ShippingInstruction.Value,
                    Quantity = localPurchase.Quantity,
                    ProjectCode = localPurchase.ProjectCode,
                    Status = status,
                    BusinessProcessID = localPurchase.BusinessProcessID,
                    //BusinessProcess = localPurchase.BusinessProcess
                    //CreatedDate = localPurchase.DateCreated,                     
                });
        }

        private IEnumerable<LocalPurchaseDetailViewModel> GetNewLocalPurchaseDetail()
        {
            var hubs = _localPurchaseService.GetAllHub().Where(m => m.HubOwnerID == 1);
            return (from hub in hubs
                    select new LocalPurchaseDetailViewModel
                    {
                        HubID = hub.HubID,
                        HubName = hub.Name,
                        AllocatedAmonut = 0,
                        RecievedAmonut = 0
                    });
        }
        public JsonResult GetGiftCertificateInfo(string siNumber)
        {
            var giftCertificate = _giftCertificateService.FindBy(m => m.ShippingInstruction.Value == siNumber).FirstOrDefault();
            if (giftCertificate != null)
            {
                var giftDetail = (from detail in giftCertificate.GiftCertificateDetails
                                  select new LocalPurchaseFromGiftCertificateInfo
                                  {
                                      GiftCertificateID = detail.GiftCertificateID,
                                      CommodityID = detail.CommodityID,
                                      CommodityName = detail.Commodity.Name,
                                      ProgramName = giftCertificate.Program.Name,
                                      DonorID = giftCertificate.DonorID,
                                      DonorName = giftCertificate.Donor.Name,
                                      QuantityInMT = detail.WeightInMT,
                                      CommoditySource = "Local Purchase"
                                  });
                return Json(giftDetail, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public JsonResult AutoCompleteSiNumber(string term)
        {
            var result = (from siNumber in _shippingInstructionService.GetAllShippingInstruction()
                          where siNumber.Value.ToLower().StartsWith(term.ToLower())
                          select siNumber.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private int CheckAvilabilityOfSiNumber(string siNumber)
        {
            try
            {
                var siId = _shippingInstructionService.GetShipingInstructionId(siNumber);
                return siId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private Boolean CheckAvailabilityOfSiInLocalPurchase(string siNumber)
        {
            var shippingInstructionID =
                _shippingInstructionService.FindBy(m => m.Value == siNumber).FirstOrDefault().ShippingInstructionID;
            try
            {
                var siId =
                    _localPurchaseService.FindBy(d => d.ShippingInstructionID == shippingInstructionID)
                        .SingleOrDefault();
                if (siId == null)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int AddSiNumber(string siNumber)
        {
            try
            {
                var shippingInstruction = new Cats.Models.ShippingInstruction();
                shippingInstruction.Value = siNumber;
                _shippingInstructionService.AddShippingInstruction(shippingInstruction);
                return shippingInstruction.ShippingInstructionID;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public ActionResult Approve(int id)
        {
            var localPurchase = _localPurchaseService.FindById(id);
            if (localPurchase != null)
            {
                //localPurchase.StatusID = (int) LocalPurchaseStatus.Approved;
                _localPurchaseService.Approve(localPurchase);
                return RedirectToAction("Details", new { id = localPurchase.LocalPurchaseID });
            }
            return RedirectToAction("Index");
        }
        public void PopulateLookUps()
        {
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
            ViewBag.CommodityID = new SelectList(_commonService.GetCommodities(), "CommodityID", "Name");
            ViewBag.CommodityTypeID = new SelectList(_commonService.GetCommodityTypes(), "CommodityTypeID", "Name");
            ViewBag.DonorID = new SelectList(_commonService.GetDonors(), "DonorID", "Name");
        }

        public ActionResult Delete(int id)
        {
            var localPurchase = _localPurchaseService.FindById(id);

            if (localPurchase != null)
            {
                switch (localPurchase.StatusID)
                {
                    case (int)LocalPurchaseStatus.Approved:
                        if (!_localPurchaseService.DelteLocalPurchaseAllocation(localPurchase))
                        {
                            TempData["Received"] = "local Purchase can not be Deleted. It has already been Received!";
                            return RedirectToAction("Index");
                        }

                        _localPurchaseService.DeleteLocalPurchae(localPurchase);
                        break;
                    case (int)LocalPurchaseStatus.Draft:
                        {
                            _localPurchaseService.DeleteLocalPurchae(localPurchase);
                            return RedirectToAction("Index");
                        }

                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Revert(int id)
        {
            var localPurchase = _localPurchaseService.FindById(id);

            if (localPurchase != null)
            {
                if (!_localPurchaseService.DelteLocalPurchaseAllocation(localPurchase))
                {
                    TempData["Received"] = "local Purchase can not be Reverted. It has already been Received!";
                    return RedirectToAction("Index");
                }
                localPurchase.StatusID = (int)LocalPurchaseStatus.Draft;
                _localPurchaseService.EditLocalPurchase(localPurchase);
                TempData["Reverted"] = "Local purchase is Reverted to Draft";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Unable to revert local purchase.";
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
            var localPurchase = _localPurchaseService.FindBy(b => b.BusinessProcessID == st.ParentBusinessProcessID).FirstOrDefault();

            //var transfer = _transferService.FindById(id);
            string stateName = _stateTemplateService.FindById(st.StateID).Name;
            if (stateName == "Approved" && localPurchase != null)
            {
                //localPurchase.StatusID = (int) LocalPurchaseStatus.Approved;
                if (_localPurchaseService.Approve(localPurchase))
                    _businessProcessService.PromotWorkflow(businessProcessState);
            }
            if (stateName == "Reverted" && localPurchase != null)
            {
                //localPurchase.StatusID = (int) LocalPurchaseStatus.Approved;
                if (_localPurchaseService.Revert(localPurchase))
                    _businessProcessService.PromotWorkflow(businessProcessState);
            }
            if (statusId != null)
                return RedirectToAction("Index", "LocalPurchase", new { Area = "Logistics", statusId });
            return RedirectToAction("Index", "LocalPurchase", new { Area = "Logistics" });
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
    }
}
