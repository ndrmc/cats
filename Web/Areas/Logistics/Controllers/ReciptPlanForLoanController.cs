using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Areas.Logistics.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Hub;
using Cats.Services.Logistics;
using Cats.Services.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using ICommonService = Cats.Services.Common.ICommonService;
using Cats.Models.ViewModels;
using System.IO;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cats.Areas.Logistics.Controllers
{
    [Authorize]
    public class ReciptPlanForLoanController : Controller
    {
        //
        // GET: /Logistics/ReciptPlanForLoanAndOthers/
        private readonly ILoanReciptPlanService _loanReciptPlanService;
        private readonly ICommonService _commonService;
        private readonly ILoanReciptPlanDetailService _loanReciptPlanDetailService;
        private readonly IUserAccountService _userAccountService;
        private readonly Services.Hub.ICommodityService _commodityService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IBusinessProcessService _businessProcessService;
        public ReciptPlanForLoanController(ILoanReciptPlanService loanReciptPlanService,ICommonService commonService,
                                           ILoanReciptPlanDetailService loanReciptPlanDetailService,IUserAccountService userAccountService,
                                           Services.Hub.ICommodityService commodityService
            , IApplicationSettingService applicationSettingService,
            IBusinessProcessService businessProcessService)
        {
            _loanReciptPlanService = loanReciptPlanService;
            _commonService = commonService;
            _loanReciptPlanDetailService = loanReciptPlanDetailService;
            _userAccountService = userAccountService;
            _commodityService = commodityService;
            _applicationSettingService = applicationSettingService;
            _businessProcessService = businessProcessService;

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name");
            ViewBag.CommodityID = new SelectList(_commonService.GetCommodities(), "CommodityID", "Name");
            ViewBag.SourceHubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name");
            ViewBag.CommodityTypeID = new SelectList(_commonService.GetCommodityTypes(), "CommodityTypeID", "Name");
            ViewBag.CommoditySourceID = new SelectList(_commonService.GetCommoditySource(), "CommoditySourceID", "Name",2);
            ViewBag.LoanSource = new SelectList(_commonService.GetDonors(), "DonorID", "Name");
            //ViewBag.HubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name");
            var loanReciptPlanViewModel = new LoanReciptPlanViewModel();
            loanReciptPlanViewModel.CommoditySourceName = _commonService.GetCommditySourceName(2);//commodity source for Loan
            return View(loanReciptPlanViewModel);

        }
        [HttpPost]
        public ActionResult Create(LoanReciptPlanViewModel loanReciptPlanViewModel)
        {
            if (ModelState.IsValid && loanReciptPlanViewModel!=null)
            {
                var loanReciptPlan = GetLoanReciptPlan(loanReciptPlanViewModel);
                int BP_PR = _applicationSettingService.getReciptPlanForLoanWorkflow();
                if (BP_PR != 0)
                {
                    BusinessProcessState createdstate = new BusinessProcessState
                    {
                        DatePerformed = DateTime.Now,
                        PerformedBy = User.Identity.Name,
                        Comment = "loan Recipt Plan  Added"

                    };
                    //_PaymentRequestservice.Create(request);

                    BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                                                                                    "ReciptPlanForLoan", createdstate);
                    if (bp != null)
                        loanReciptPlan.BusinessProcessID = bp.BusinessProcessID;


                }
                _loanReciptPlanService.AddLoanReciptPlan(loanReciptPlan);
                ModelState.AddModelError("Sucess",@"Sucessfully Saved");
                return RedirectToAction("Index");
            }
            return View(loanReciptPlanViewModel);
        }
        public ActionResult Edit(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindById(id);
            if (loanReciptPlan==null)
            {
                return HttpNotFound();
            }
           
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name",loanReciptPlan.ProgramID);
            ViewBag.CommodityID = new SelectList(_commodityService.FindBy(m=>m.ParentID==loanReciptPlan.Commodity.ParentID), "CommodityID", "Name",loanReciptPlan.CommodityID);
            //ViewBag.SourceHubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name",loanReciptPlan.SourceHubID);
            ViewBag.CommodityTypeID = new SelectList(_commonService.GetCommodityTypes(), "CommodityTypeID", "Name");
            ViewBag.CommoditySourceID = new SelectList(_commonService.GetCommoditySource(), "CommoditySourceID", "Name",loanReciptPlan.CommoditySourceID);
            //ViewBag.HubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name",loanReciptPlan.HubID);
            return View(loanReciptPlan);
        }
        [HttpPost]
        public ActionResult Edit(LoanReciptPlan loanReciptPlan)
        {
            if (ModelState.IsValid && loanReciptPlan!=null )
            {
                _loanReciptPlanService.EditLoanReciptPlan(loanReciptPlan);
                return RedirectToAction("Detail",new {id=loanReciptPlan.LoanReciptPlanID});
            }
            ModelState.AddModelError("Errors",@"Unable to update please check fields");
            ViewBag.ProgramID = new SelectList(_commonService.GetPrograms(), "ProgramID", "Name", loanReciptPlan.ProgramID);
            ViewBag.CommodityID = new SelectList(_commonService.GetCommodities(), "CommodityID", "Name", loanReciptPlan.CommodityID);
            ViewBag.CommodityTypeID = new SelectList(_commonService.GetCommodityTypes(), "CommodityTypeID", "Name");
            ViewBag.CommoditySourceID = new SelectList(_commonService.GetCommoditySource(), "CommoditySourceID", "Name", loanReciptPlan.CommoditySourceID);
            ViewBag.TargetController = "ReciptPlanForLoan";
            return View(loanReciptPlan);
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
                return RedirectToAction("Detail", "ReciptPlanForLoan", new { Area = "Logistics", statusId });
            return RedirectToAction("Index", "ReciptPlanForLoan", new { Area = "Logistics" });
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

        private LoanReciptPlan GetLoanReciptPlan(LoanReciptPlanViewModel loanReciptPlanViewModel)
        {
          
                var loanReciptPlan = new LoanReciptPlan()
                    {
                        ProgramID = loanReciptPlanViewModel.ProgramID,
                        CommodityID = loanReciptPlanViewModel.CommodityID,
                        CommoditySourceID = 2,//only for loan
                        ShippingInstructionID = _commonService.GetShippingInstruction(loanReciptPlanViewModel.SiNumber),
                        LoanSource = loanReciptPlanViewModel.LoanSource.ToString(),
                        //HubID = loanReciptPlanViewModel.HubID,
                        ProjectCode = loanReciptPlanViewModel.ProjectCode,
                        ReferenceNumber = loanReciptPlanViewModel.RefeenceNumber,
                        Quantity = loanReciptPlanViewModel.Quantity,
                        CreatedDate = DateTime.Today,
                        StatusID = (int)LocalPurchaseStatus.Draft,
                        IsFalseGRN = loanReciptPlanViewModel.IsFalseGRN
                    };
                return loanReciptPlan;
        }
        public ActionResult LoanReciptPlan_Read([DataSourceRequest] DataSourceRequest request)
        {
            var reciptPlan = _loanReciptPlanService.GetAllLoanReciptPlan().OrderByDescending(m => m.LoanReciptPlanID);
            var reciptPlanToDisplay = BindToViewModel(reciptPlan);

           return Json(reciptPlanToDisplay.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

      
        private IEnumerable<LoanReciptPlanViewModel> BindToViewModel(IEnumerable<LoanReciptPlan> loanReciptPlans)
        {
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            var list = new List<LoanReciptPlanViewModel>();
            var reciptPlans = loanReciptPlans as List<LoanReciptPlan> ?? loanReciptPlans.ToList();
            foreach (var loanReciptPlan in reciptPlans)
            {
                var loanSource = loanReciptPlan.LoanSource;
                var intLoanSource = Convert.ToInt32(loanSource);
                var firstOrDefault =
                    _commonService.GetDonors(d => d.DonorID == intLoanSource).FirstOrDefault();
                var loan = new LoanReciptPlanViewModel
                               {
                                   LoanReciptPlanID = loanReciptPlan.LoanReciptPlanID,
                                   ProgramName = loanReciptPlan.Program.Name,
                                   CommodityName = loanReciptPlan.Commodity.Name,
                                   CommoditySourceName = loanReciptPlan.CommoditySource.Name,
                                   BusinessProcessID = loanReciptPlan.BusinessProcessID,
                    
                LoanSource = intLoanSource
                               };
           
                loan.BusinessProcess = loanReciptPlan.BusinessProcess;
               
                //    new BusinessProcessClean
                //{
                //    BusinessProcessID = loanReciptPlan.BusinessProcessID,
                //    DocumentID = loanReciptPlan.BusinessProcess.DocumentID,
                //    DocumentType = loanReciptPlan.BusinessProcess.DocumentType,
                //    ProcessType = loanReciptPlan.BusinessProcess.ProcessType,
                //    ProcessTypeID = loanReciptPlan.BusinessProcess.ProcessTypeID,
                //    CurrentState =   loanReciptPlan.BusinessProcess.CurrentState,
                //    CurrentStateID = loanReciptPlan.BusinessProcess.CurrentStateID

                //};
                if (firstOrDefault != null) loan.Donor = firstOrDefault.Name;
                //SourceHubName = loanReciptPlan.Hub.Name,
                loan.RefeenceNumber = loanReciptPlan.ReferenceNumber;
                loan.SiNumber = loanReciptPlan.ShippingInstruction.Value;
                loan.ProjectCode = loanReciptPlan.ProjectCode;
                loan.Quantity = loanReciptPlan.Quantity;
                loan.StatusID = loanReciptPlan.StatusID;
                //loan.BusinessProcessID = loanReciptPlan.BusinessProcessID;
                //loan.BusinessProcess = loanReciptPlan.BusinessProcess;
                loan.CreatedDate = loanReciptPlan.CreatedDate.ToCTSPreferedDateFormat(datePref);
                loan.Status = _commonService.GetStatusName(WORKFLOW.LocalPUrchase, loanReciptPlan.StatusID);
                loan.IsFalseGRN = loanReciptPlan.IsFalseGRN;

                list.Add(loan);

            }

            return list;
            //return (from loanReciptPlan in reciptPlans
            //        let loanSource = loanReciptPlan.LoanSource
            //        where loanSource != null
            //        let intLoanSource = Convert.ToInt32(loanSource)
            //        let firstOrDefault = _commonService.GetDonors(d=>d.DonorID == Convert.ToInt32(loanSource)).FirstOrDefault()
            //        where firstOrDefault != null
            //        select new LoanReciptPlanViewModel
            //            {
            //                LoanReciptPlanID = loanReciptPlan.LoanReciptPlanID,
            //                ProgramName = loanReciptPlan.Program.Name,
            //                CommodityName = loanReciptPlan.Commodity.Name,
            //                CommoditySourceName = loanReciptPlan.CommoditySource.Name,
            //                LoanSource = intLoanSource,
            //                Donor = firstOrDefault.Name,
            //                //SourceHubName = loanReciptPlan.Hub.Name,
            //                RefeenceNumber = loanReciptPlan.ReferenceNumber,
            //                SiNumber = loanReciptPlan.ShippingInstruction.Value,
            //                ProjectCode = loanReciptPlan.ProjectCode,
            //                Quantity = loanReciptPlan.Quantity,
            //                StatusID = loanReciptPlan.StatusID,
            //                CreatedDate = loanReciptPlan.CreatedDate.ToCTSPreferedDateFormat(datePref),
            //                Status = _commonService.GetStatusName(WORKFLOW.LocalPUrchase, loanReciptPlan.StatusID),
            //                IsFalseGRN = loanReciptPlan.IsFalseGRN
            //            });

        }

        public ActionResult Approve(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindBy(m=>m.LoanReciptPlanID==id).FirstOrDefault();
            if (loanReciptPlan == null)
            {
                return HttpNotFound();

            }
            _loanReciptPlanService.ApproveRecieptPlan(loanReciptPlan);
            return RedirectToAction("Index");
        }
        public ActionResult Detail(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindById(id);
           


            if (loanReciptPlan==null)
            {
                return HttpNotFound();
            }

            var loan = new LoanReciptPlanViewModel
            {
                RefeenceNumber = loanReciptPlan.ReferenceNumber,
                SiNumber = loanReciptPlan.ShippingInstruction.Value,
                ProjectCode = loanReciptPlan.ProjectCode,
                Quantity = loanReciptPlan.Quantity,
                ProgramName = loanReciptPlan.Program.Name,
                CommodityName = loanReciptPlan.Commodity.Name,
                CommoditySourceName = loanReciptPlan.CommoditySource.Name,
                LoanReciptPlanID = loanReciptPlan.LoanReciptPlanID,
                BusinessProcessID = loanReciptPlan.BusinessProcessID,
                BusinessProcess = loanReciptPlan.BusinessProcess,
                //new BusinessProcessClean
                //{
                //    BusinessProcessID = loanReciptPlan.BusinessProcessID,
                //    DocumentID = loanReciptPlan.BusinessProcess.DocumentID,
                //    DocumentType = loanReciptPlan.BusinessProcess.DocumentType,
                //    ProcessType = loanReciptPlan.BusinessProcess.ProcessType,
                //    ProcessTypeID = loanReciptPlan.BusinessProcess.ProcessTypeID,
                //    CurrentState = loanReciptPlan.BusinessProcess.CurrentState,
                //    CurrentStateID = loanReciptPlan.BusinessProcess.CurrentStateID
                //}
        };

            var loanSource = loanReciptPlan.LoanSource;
            ViewBag.TargetController = "ReciptPlanForLoan";
            var intLoanSource = Convert.ToInt32(loanSource);
            var firstOrDefault =
                _commonService.GetDonors(d => d.DonorID == intLoanSource).FirstOrDefault();
            if (firstOrDefault != null) loan.Donor = firstOrDefault.Name;
            loan.StatusID = loanReciptPlan.StatusID;
            return View(loan);
        }
        public JsonResult GetMaxSINo()
        {
            var result =
                _loanReciptPlanService.GetAllLoanReciptPlan().Select(m => m.ShippingInstruction.Value);
            var siList = result.Select(si => Regex.Match(si, @"\d+").Value).Select(data => Convert.ToInt32(data)).ToList();
            int resultInt = siList.Max() + 1;
           return Json("LOAN-" + resultInt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoanReciptPlanDetail_Read([DataSourceRequest] DataSourceRequest request, int loanReciptPlanID)
        {
            var loanReciptPlanDetail = _loanReciptPlanDetailService.FindBy(m=>m.LoanReciptPlanID==loanReciptPlanID);
            var loanReciptPlanDetailToDisplay = BidToLoanReciptPlanDetail(loanReciptPlanDetail);
            return Json(loanReciptPlanDetailToDisplay.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<LoanReciptPlanWithDetailViewModel> BidToLoanReciptPlanDetail(IEnumerable<LoanReciptPlanDetail> loanReciptPlanDetails )
        {
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            return (from loanReciptPlanDetail in loanReciptPlanDetails
                    select new LoanReciptPlanWithDetailViewModel
                        {
                            LoanReciptPlanDetailID = loanReciptPlanDetail.LoanReciptPlanDetailID,
                            LoanReciptPlanID = loanReciptPlanDetail.LoanReciptPlanID,
                            HubID = loanReciptPlanDetail.HubID,
                            HubName = loanReciptPlanDetail.Hub.Name,
                           // MemoRefrenceNumber = loanReciptPlanDetail.MemoReferenceNumber,
                            Amount = loanReciptPlanDetail.RecievedQuantity,
                            CreatedDate = loanReciptPlanDetail.RecievedDate.ToCTSPreferedDateFormat(datePref),
                            Remaining = _loanReciptPlanDetailService.GetRemainingQuantity(loanReciptPlanDetail.LoanReciptPlanID)
                            
                        });
        }

        public ActionResult LoanReciptPlanDetail_Delete([DataSourceRequest] DataSourceRequest request, LoanReciptPlanWithDetailViewModel loanReciptPlanWithDetailViewModel)
        {
            if (loanReciptPlanWithDetailViewModel != null)
            {
                _loanReciptPlanDetailService.DeleteById(loanReciptPlanWithDetailViewModel.LoanReciptPlanDetailID);

            }
            return Json(ModelState.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult ReciptPlan(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindById(id);
            if (loanReciptPlan==null)
            {
                return HttpNotFound();
            }
            ViewBag.HubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name");
            var loanReciptPlanViewModel = new LoanReciptPlanWithDetailViewModel()
                {
                    LoanReciptPlanID = id,
                    TotalAmount = loanReciptPlan.Quantity,
                    Remaining = _loanReciptPlanDetailService.GetRemainingQuantity(id)
                };
            ViewBag.Errors = "Out of Quantity! The Remaining Quantity is:";
            return View(loanReciptPlanViewModel);
        }
        [HttpPost]
        public ActionResult ReciptPlan(LoanReciptPlanWithDetailViewModel loanReciptPlanDetail)
        {
            var userID = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).UserProfileID;
            if (ModelState.IsValid && loanReciptPlanDetail!=null)
            {
                var loanReciptPlanModel = new LoanReciptPlanDetail()
                    {
                        LoanReciptPlanID = loanReciptPlanDetail.LoanReciptPlanID,
                        HubID = loanReciptPlanDetail.HubID,
                        //MemoReferenceNumber = loanReciptPlanDetail.MemoRefrenceNumber,
                        RecievedQuantity = loanReciptPlanDetail.Amount,
                        RecievedDate = DateTime.Today,
                        ApprovedBy = userID
                    };
                _loanReciptPlanDetailService.AddRecievedLoanReciptPlanDetail(loanReciptPlanModel);
                return RedirectToAction("Detail", new {id = loanReciptPlanDetail.LoanReciptPlanID});
            }
            ViewBag.HubID = new SelectList(_commonService.GetAllHubs(), "HubID", "Name");
            return View(loanReciptPlanDetail);
        }
        public ActionResult Delete(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindById(id);
            if (loanReciptPlan!=null)
            {
                if (loanReciptPlan.StatusID==(int)LocalPurchaseStatus.Draft)
                {
                    _loanReciptPlanService.DeleteLoanWithDetail(loanReciptPlan);
                    return RedirectToAction("Index","ReciptPlanForLoan");
                }
                else
                {
                    if (_loanReciptPlanService.DeleteLoanReciptAllocation(loanReciptPlan))
                    {
                        _loanReciptPlanService.DeleteLoanWithDetail(loanReciptPlan);
                        return RedirectToAction("Index", "ReciptPlanForLoan");
                    }
                    else
                    {
                        TempData["Received"] = "Loan Recipt Plan can not be Deleted. It has already been Received!";
                        return RedirectToAction("Index");
                    }
                }
                
            }
            return RedirectToAction("Index", "ReciptPlanForLoan");
        }
        public ActionResult Revert(int id)
        {
            var loanReciptPlan = _loanReciptPlanService.FindById(id);

            if (loanReciptPlan != null)
            {
                if (!_loanReciptPlanService.DeleteLoanReciptAllocation(loanReciptPlan))
                {
                    TempData["Received"] = "Loan Recipt Plan can not be Reverted. It has already been Received!";
                    return RedirectToAction("Index");
                }
                loanReciptPlan.StatusID = (int)LocalPurchaseStatus.Draft;
                _loanReciptPlanService.EditLoanReciptPlan(loanReciptPlan);
                TempData["Reverted"] = "Loan Recipt Plan is Reverted to Draft";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Unable to revert Loan Recipt Plan!";
            return RedirectToAction("Index","ReciptPlanForLoan");
        }
       
    }
}
