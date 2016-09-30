using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Cats.Areas.EarlyWarning.Models;
using Cats.Services.EarlyWarning;
using System.Web.Mvc;
using Cats.Services.Security;
using Cats.Services.Transaction;
using Cats.ViewModelBinder;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Cats.Services.Common;
using Master = Cats.Models.Constant.Master;
using Cats.Helpers;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.ViewModels;
using Cats.Security;
using Cats.Services.Logistics;
using log4net;
using NetSqlAzMan.Providers;
using GiftCertificateViewModel = Cats.Areas.GiftCertificate.Models.GiftCertificateViewModel;

namespace Cats.Areas.EarlyWarning.Controllers
{
    public class GiftCertificateController : Controller
    {
        private readonly IGiftCertificateService _giftCertificateService;
        private readonly IGiftCertificateDetailService _giftCertificateDetailService;
        private readonly ICommonService _commonService;
        private readonly ITransactionService _transactionService;
        private readonly ILetterTemplateService _letterTemplateService;
        private readonly IUnitOfWork _unitofwork;
        private readonly IUserAccountService _userAccountService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private readonly ILog _log;
        private readonly IDonationPlanHeaderService _donationPlanHeaderService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IBusinessProcessService _businessProcessService;
        private readonly IPlanService _planService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly ISeasonService _seasonService;
        private readonly ITypeOfNeedAssessmentService _typeOfNeedAssessmentService;
        public GiftCertificateController(IGiftCertificateService giftCertificateService, IGiftCertificateDetailService giftCertificateDetailService,
                                         ICommonService commonService, ITransactionService transactionService, ILetterTemplateService letterTemplateService,
                                         IUnitOfWork unitofwork, IUserAccountService userAccountService, IShippingInstructionService shippingInstructionService, ILog log, IApplicationSettingService applicationSettingService,
                                         IBusinessProcessService businessProcessService, IPlanService planService,
                                          IAdminUnitService adminUnitService, ISeasonService seasonService,
                                          ITypeOfNeedAssessmentService typeOfNeedAssessmentService, IDonationPlanHeaderService donationPlanHeaderService)
        {
            _giftCertificateService = giftCertificateService;
            _giftCertificateDetailService = giftCertificateDetailService;
            _commonService = commonService;
            _transactionService = transactionService;
            _letterTemplateService = letterTemplateService;
            _unitofwork = unitofwork;
            _userAccountService = userAccountService;
            _shippingInstructionService = shippingInstructionService;
            _log = log;
            _donationPlanHeaderService = donationPlanHeaderService;
            _applicationSettingService = applicationSettingService;
            _businessProcessService = businessProcessService;
            _planService = planService;
            _adminUnitService = adminUnitService;
            _seasonService = seasonService;
            _typeOfNeedAssessmentService = typeOfNeedAssessmentService;
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.View_Gift_Certificate_list)]
        public ActionResult Index(int id = 1)
        {
            ViewBag.Title = id == 1 ? "Draft Gift Certificates" : "Approved Gift Certificates";
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var gifts = _giftCertificateService.Get(b => b.BusinessProcessID != 1, null, "GiftCertificateDetails,Donor,GiftCertificateDetails.Detail,GiftCertificateDetails.Commodity, BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate");
            var giftsViewModel = GiftCertificateViewModelBinder.BindListGiftCertificateViewModel(gifts.ToList(), datePref, true);
            var user = UserAccountHelper.GetUser(HttpContext.User.Identity.Name);
            var roles = _userAccountService.GetUserPermissions(user.UserName);

            ViewBag.TargetController = "GiftCertificate";

            return View(giftsViewModel);
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.View_Gift_Certificate_list)]
        public JsonResult GetListOfCertificate([DataSourceRequest] DataSourceRequest request)
        {
            var giftCertList = _giftCertificateDetailService.GetAllGiftCertificateDetail();

            var result = giftCertList.ToList().Select(item => new GiftCertificateViewModel
            {
                CommodityName = item.Commodity.Name,
                GiftDate = item.GiftCertificate.GiftDate,
                Donor = item.GiftCertificate.Donor.Name,
                Program = item.GiftCertificate.Program.Name,
                ReferenceNo = item.GiftCertificate.ReferenceNo,
                GiftCertificateID = item.GiftCertificateID,
                SINumber = item.GiftCertificate.ShippingInstruction.Value,
                PortName = item.GiftCertificate.PortName,
                IsPrinted = item.GiftCertificate.IsPrinted
            }).ToList();

            return Json(result.ToDataSourceResult(request, ModelState));
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Generate_Gift_Certificate_Template)]
        public ActionResult GenerateTemplate(int id)
        {
            return RedirectToAction("LetterTemplate", new { giftceritificateId = id });
        }

        public virtual ActionResult NotUnique(string siNumber, int giftCertificateId = -1)
        {
            if (_giftCertificateService.IsSINumberNewOrEdit(siNumber, giftCertificateId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(string.Format("{0} is invalid, there is an existing record with the same SI Number ", siNumber),
                        JsonRequestBehavior.AllowGet);
            }
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.New_Gift_Certificate)]
        public ActionResult Create()
        {

            PopulateLookup();
            var gift = new GiftCertificateViewModel();
            gift.GiftDate = DateTime.Today;
            gift.ETA = DateTime.Today;
            gift.CommodityTypeID = 1;
            return View();
        }

        [HttpPost]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.New_Gift_Certificate)]
        public ActionResult Create(GiftCertificateViewModel giftcertificateViewModel, FormCollection collection)
        {
            //int season = int.Parse(collection["SeasonID"].ToString(CultureInfo.InvariantCulture));
            //int typeOfNeedID = int.Parse(collection["TypeOfNeedID"].ToString(CultureInfo.InvariantCulture));
            //string planName = collection["Plan.PlanName"].ToString(CultureInfo.InvariantCulture);
            //DateTime startDate = DateTime.Parse(collection["Plan.StartDate"].ToString(CultureInfo.InvariantCulture));
            //var firstDayOfTheMonth = startDate.AddDays(1 - startDate.Day);
            //var duration = int.Parse(collection["Plan.Duration"].ToString(CultureInfo.InvariantCulture));
            //DateTime endDate = DateTime.Parse(collection["Plan.EndDate"].ToString(CultureInfo.InvariantCulture));
            //var endDate = firstDayOfTheMonth.AddMonths(duration);

            if (ModelState.IsValid && giftcertificateViewModel != null)
            {
                try
                {
                    int BP_PR = _applicationSettingService.getGiftCertificateWorkflow();

                    if (BP_PR != 0)
                    {
                        BusinessProcessState createdstate = new BusinessProcessState
                        {
                            DatePerformed = DateTime.Now,
                            PerformedBy = User.Identity.Name,
                            Comment = "Gift Certificate Workflow"
                        };
                        //_PaymentRequestservice.Create(request);

                        BusinessProcess bp = _businessProcessService.CreateBusinessProcess(
                            BP_PR, 0, "GiftCertificateWorkflow", createdstate);

                        if (bp != null)
                        {
                            var giftCertificate =
                                GiftCertificateViewModelBinder.BindGiftCertificate(giftcertificateViewModel);
                            giftCertificate.StatusID = 1;
                            giftCertificate.BusinessProcessID = bp.BusinessProcessID;

                            var shippingInstructionID =
                                _shippingInstructionService.GetSiNumber(giftcertificateViewModel.SINumber)
                                    .ShippingInstructionID;
                            giftCertificate.ShippingInstructionID = shippingInstructionID;
                            _giftCertificateService.AddGiftCertificate(giftCertificate);

                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception exception)
                {
                    var log = new Logger();
                    log.LogAllErrorsMesseges(exception, _log);
                    ViewBag.Regions = new SelectList(_adminUnitService.FindBy(t => t.AdminUnitTypeID == 2),
                                                     "AdminUnitID", "Name");
                    ViewBag.Season = new SelectList(_seasonService.GetAllSeason(), "SeasonID", "Name");
                    //ViewBag.TypeOfNeed = new SelectList(_typeOfNeedAssessmentService.GetAllTypeOfNeedAssessment(),
                    //                                    "TypeOfNeedAssessmentID", "TypeOfNeedAssessment1");
                    ViewBag.Error = "Gift Certificate Already Exists Please Change Name";
                    ModelState.AddModelError("Errors", ViewBag.Error);

                    return View();
                }
            }

            PopulateLookup();

            return Create(); //GiftCertificateViewModel.GiftCertificateModel(giftcertificate));
        }

        [HttpPost]
        public ActionResult Promote(BusinessProcessStateViewModel st, int? statusId)
        {
            var fileName = string.Empty;
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
                return RedirectToAction("Detail", "GiftCertificate", new { Area = "EarlyWarning", statusId });

            return RedirectToAction("Index", "GiftCertificate", new { Area = "EarlyWarning" });
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

        //[AcceptVerbs(HttpVerbs.Post)]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.New_Gift_Certificate)]
        public ActionResult GiftCertificateDetail_Create([DataSourceRequest] DataSourceRequest request, GiftCertificateDetailsViewModel giftCertificateDetailsViewModel, int? id)
        {
            if (giftCertificateDetailsViewModel != null && id.HasValue)
            {
                giftCertificateDetailsViewModel.GiftCertificateID = id.Value;
                var giftcertifiateDtail = GiftCertificateViewModelBinder.BindGiftCertificateDetail(giftCertificateDetailsViewModel);
                _giftCertificateDetailService.AddGiftCertificateDetail(giftcertifiateDtail);
                return RedirectToAction("Index");
            }
            //return RedirectToAction("Index");
            return Json(new[] { giftCertificateDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.View_Gift_Certificate_list)]
        public ActionResult GiftCertificateDetail_Read([DataSourceRequest] DataSourceRequest request, int? id)
        {
            if (!id.HasValue)
            {
                return Json((new List<GiftCertificateDetailsViewModel>()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var gc = _giftCertificateService.FindById(id.Value);
                if (gc != null)
                {
                    var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
                    var gC =
                        GiftCertificateViewModelBinder.BindListOfGiftCertificateDetailsViewModel(gc.GiftCertificateDetails.ToList(), datePref);

                    return Json(gC.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((new List<GiftCertificateDetailsViewModel>()).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Edit_Gift_Certificate)]
        public ActionResult GiftCertificateDetail_Update([DataSourceRequest]DataSourceRequest request, GiftCertificateDetailsViewModel giftCertificateDetailsViewModel)
        {
            if (giftCertificateDetailsViewModel != null)
            {
                var target = _giftCertificateDetailService.FindById(giftCertificateDetailsViewModel.GiftCertificateDetailID);
                if (target != null)
                {
                    target = GiftCertificateViewModelBinder.BindGiftCertificateDetail(target, giftCertificateDetailsViewModel);

                    _giftCertificateDetailService.EditGiftCertificateDetail(target);
                }
                return RedirectToAction("Index");
            }

            return Json(new[] { giftCertificateDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }

        // [AcceptVerbs(HttpVerbs.Get)]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Delete_needs_assessment)]
        public ActionResult GiftCertificateDetail_Destroy([DataSourceRequest] DataSourceRequest request,
                                                   GiftCertificateDetailsViewModel giftCertificateDetailsViewModel)
        {
            if (giftCertificateDetailsViewModel != null)
            {
                _giftCertificateDetailService.DeleteById(giftCertificateDetailsViewModel.GiftCertificateDetailID);
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Edit_Gift_Certificate)]
        public ActionResult Edit(int id)
        {
            var giftcertificate = _giftCertificateService.Get(t => t.GiftCertificateID == id, null, "GiftCertificateDetails,GiftCertificateDetails.Commodity").FirstOrDefault();
            PopulateLookup(false, giftcertificate);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var giftCertificateViewModel = GiftCertificateViewModelBinder.BindGiftCertificateViewModel(giftcertificate, datePref);
            return View(giftCertificateViewModel);
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Edit_Gift_Certificate)]
        public ActionResult Detail(int id)
        {
            var giftcertificate = _giftCertificateService.Get(t => t.GiftCertificateID == id, null, "GiftCertificateDetails,GiftCertificateDetails.Commodity").FirstOrDefault();
            PopulateLookup(false, giftcertificate);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var giftCertificateViewModel = GiftCertificateViewModelBinder.BindGiftCertificateViewModel(giftcertificate, datePref);
            return View(giftCertificateViewModel);
        }

        [HttpPost]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Edit_Gift_Certificate)]
        public ActionResult Edit(GiftCertificateViewModel giftcertificate)
        {
            //just incase the user meses with the the hidden GiftCertificateID field
            var giftcert = _giftCertificateService.FindById(giftcertificate.GiftCertificateID);

            if (ModelState.IsValid && giftcert != null)
            {

                giftcert = GiftCertificateViewModelBinder.BindGiftCertificate(giftcert, giftcertificate);

                // add the si number 
                var shippingInstructionID = _shippingInstructionService.GetSiNumber(giftcertificate.SINumber).ShippingInstructionID;
                giftcert.ShippingInstructionID = shippingInstructionID;


                _giftCertificateService.EditGiftCertificate(giftcert);

                return RedirectToAction("Index");
            }
            PopulateLookup(false, giftcert);


            return View(giftcertificate);
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Delete_Gift_Certificate)]
        public ActionResult Delete(int id)
        {
            var giftcertificate = _giftCertificateService.FindById(id);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            return View(GiftCertificateViewModelBinder.BindGiftCertificateViewModel(giftcertificate, datePref));
        }


        [HttpPost, ActionName("Delete")]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Delete_Gift_Certificate)]
        public ActionResult DeleteConfirmed(int id)
        {
            _giftCertificateService.DeleteById(id);
            return RedirectToAction("Index");
        }

        public ActionResult IsBillOfLoadingDuplicate(string BillOfLoading)
        {
            return Json(_giftCertificateService.IsBillOfLoadingDuplicate(BillOfLoading), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Approve_Gift_Certeficate)]
        public ActionResult Rejected(int id)
        {
            int donationHeaderCount = _giftCertificateService.FindById(id).Donor.DonationPlanHeaders.Count;
            var giftCertificate = _giftCertificateService.FindById(id);
            if (giftCertificate.IsPrinted || donationHeaderCount > 0)
                return PartialView("_NotReject", null);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var giftCertificateViewModel = GiftCertificateViewModelBinder.BindGiftCertificateViewModel(giftCertificate, datePref);
            return PartialView("_Reject", giftCertificateViewModel);
        }

        [HttpPost]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Approve_Gift_Certeficate)]
        public ActionResult Reject(int giftCertificateId)
        {
            int donationHeaderCount = _giftCertificateService.FindById(giftCertificateId).Donor.DonationPlanHeaders.Count;

            var giftCertificate = _giftCertificateService.Get(t => t.GiftCertificateID == giftCertificateId, null,
                   "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                   .FirstOrDefault();

            if (giftCertificate == null)
            {
                return RedirectToAction("Index", 2);
            }

            // if any approved or commited receipt plan is found under this giftcertificate, then don't revert/reject
            if (donationHeaderCount > 0) // if count is greater than 0, then don't reject
            {
                return RedirectToAction("Index", 2);
            }

            var donations =
             _donationPlanHeaderService.GetAllDonationPlanHeader()
                 .Where(d => d.ShippingInstructionId == _giftCertificateService.FindById(giftCertificateId).ShippingInstructionID);

            // find all receipt plans that are not commited or approved under this giftcertificate
            // and revert them all
            if (donationHeaderCount > 0)
            {
                foreach (var donation in donations.ToList())
                {
                    if (donation.IsCommited == true)
                    {
                        if (_donationPlanHeaderService.DeleteReceiptAllocation(donation))
                        {
                            donation.IsCommited = false;
                            _donationPlanHeaderService.EditDonationPlanHeader(donation);
                        }
                    }
                }
            }

            // now revert the giftcertificate itself
            if (_transactionService.RevertGiftCertificate(giftCertificateId)) // check if transaction returns true
            {
                // workflow implementation
                var approveFlowTemplate = giftCertificate.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates
                        .FirstOrDefault(t => t.Name == "Reject");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "GiftCertificate has been rejected!",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = giftCertificate.BusinessProcessID
                    };

                    _businessProcessService.PromotWorkflow(businessProcessState);
                }
                // workflow implementation ends here
            }

            return RedirectToAction("Index", 2);
        }
        [HttpGet]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Approve_Gift_Certeficate)]
        public ActionResult Approved(int id)
        {
            var giftCertificate = _giftCertificateService.FindById(id);
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var giftCertificateViewModel = GiftCertificateViewModelBinder.BindGiftCertificateViewModel(giftCertificate, datePref);
            return PartialView("_Approve", giftCertificateViewModel);
        }

        [HttpPost]
        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Approve_Gift_Certeficate)]
        public ActionResult Approve(int giftCertificateId)
        {
            bool result = _transactionService.PostGiftCertificate(giftCertificateId);
            var giftCertificate = _giftCertificateService.Get(t => t.GiftCertificateID == giftCertificateId, null,
                  "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate")
                  .FirstOrDefault();

            if (giftCertificate != null && result)
            {
                var approveFlowTemplate = giftCertificate.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Approve");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = HttpContext.User.Identity.Name,
                        DatePerformed = DateTime.Now,
                        Comment = "GiftCertificate has been approved",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = giftCertificate.BusinessProcessID
                    };

                    //_giftCertificateService.EditGiftCertificate(giftCertificate);
                    _businessProcessService.PromotWorkflow(businessProcessState);
                }
            }

            return RedirectToAction("Index");
        }

        private void PopulateLookup(bool isNew = true, Cats.Models.GiftCertificate giftCertificate = null)
        {
            ViewData["Commodities"] = _commonService.GetCommodities(null, t => t.OrderBy(o => o.Name));

            ViewBag.DCurrencies = _commonService.GetDetails(d => d.MasterID == Master.Constants.CURRENCY, t => t.OrderBy(o => o.SortOrder));
            ViewBag.DFundSources = _commonService.GetDetails(d => d.MasterID == Master.Constants.FUND_SOURCE, t => t.OrderBy(o => o.SortOrder));
            ViewBag.DBudgetTypes = _commonService.GetDetails(d => d.MasterID == Master.Constants.BUDGET_TYPE, t => t.OrderBy(o => o.SortOrder));

            var giftCertificateDetails = new List<GiftCertificateDetailsViewModel>();
            ViewBag.GiftCertificateDetails = giftCertificateDetails;
            if (isNew && giftCertificate == null)
            {
                ViewBag.DonorID = new SelectList(_commonService.GetDonors(null, t => t.OrderBy(o => o.Name)), "DonorID",
                                                 "Name");
                ViewBag.CommodityTypeID =
                    new SelectList(_commonService.GetCommodityTypes(null, t => t.OrderBy(o => o.Name)),
                                   "CommodityTypeID", "Name", 1);
                ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
                ViewBag.DModeOfTransport = new SelectList(_commonService.GetDetails(d => d.MasterID == Master.Constants.TRANSPORT_MODE, t => t.OrderBy(o => o.SortOrder)), "DetailID", "Name");

            }
            else
            {
                var giftDetails = giftCertificate.GiftCertificateDetails.FirstOrDefault();
                ViewBag.DonorID = new SelectList(_commonService.GetDonors(null, t => t.OrderBy(o => o.Name)), "DonorID",
                                                "Name", giftCertificate.DonorID);
                ViewBag.CommodityTypeID =
                    new SelectList(_commonService.GetCommodityTypes(null, t => t.OrderBy(o => o.Name)),
                                   "CommodityTypeID", "Name", giftDetails == null ? "1" : giftDetails.Commodity.CommodityTypeID.ToString());
                ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name", giftCertificate.ProgramID);
                ViewBag.DModeOfTransport = new SelectList(_commonService.GetDetails(d => d.MasterID == Master.Constants.TRANSPORT_MODE, t => t.OrderBy(o => o.SortOrder)), "DetailID", "Name", giftCertificate.DModeOfTransport);

            }
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Generate_Gift_Certificate_Template)]
        public ActionResult LetterTemplate(int giftceritificateId)
        {
            ViewData["giftcertficateId"] = giftceritificateId;
            return View();
        }

        [EarlyWarningAuthorize(operation = EarlyWarningConstants.Operation.Generate_Gift_Certificate_Template)]
        public ActionResult ShowLetterTemplates([DataSourceRequest] DataSourceRequest request)
        {
            return Json(_letterTemplateService.GetAllLetterTemplates().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
        public void ShowTemplate(string fileName, int giftCertificateId)
        {
            // TODO: Make sure to use DI to get the template generator instance
            try
            {
                var template = new TemplateHelper(_unitofwork, _log);
                string filePath = template.GenerateTemplate(giftCertificateId, 1, fileName); //here you have to send the name of the tempalte and the id of the giftcertificate


                Response.Clear();
                Response.ContentType = "application/text";
                Response.AddHeader("Content-Disposition", @"filename= " + fileName + ".docx");
                Response.TransmitFile(filePath);
                Response.End();

                bool result = _transactionService.PrintedGiftCertificate(giftCertificateId);

                var giftCertificate = _giftCertificateService.Get(t => t.GiftCertificateID == giftCertificateId, null,
                    "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();

                if (giftCertificate != null && result)
                {
                    var approveFlowTemplate = giftCertificate.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Print");
                    if (approveFlowTemplate != null)
                    {
                        var businessProcessState = new BusinessProcessState()
                        {
                            StateID = approveFlowTemplate.FinalStateID,
                            PerformedBy = HttpContext.User.Identity.Name,
                            DatePerformed = DateTime.Now,
                            Comment = "GiftCertificate has been printed",
                            //AttachmentFile = fileName,
                            ParentBusinessProcessID = giftCertificate.BusinessProcessID
                        };

                        //_giftCertificateService.EditGiftCertificate(giftCertificate);
                        _businessProcessService.PromotWorkflow(businessProcessState);
                    }
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message.ToString(CultureInfo.InvariantCulture), ex.GetBaseException());
                //System.IO.File.AppendAllText(@"c:\temp\errors.txt", " ShowTemplate : " + ex.Message.ToString(CultureInfo.InvariantCulture));

            }


        }
        protected override void Dispose(bool disposing)
        {
            _giftCertificateService.Dispose();
        }


        [HttpGet]
        public JsonResult AutoCompleteSiNumber(string term)
        {
            var result = (from siNumber in _shippingInstructionService.GetAllShippingInstruction()
                          where siNumber.Value.ToLower().StartsWith(term.ToLower())
                          select siNumber.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
