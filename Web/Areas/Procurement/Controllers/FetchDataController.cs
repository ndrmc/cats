using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.Procurement.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Services.Dashboard;
using Cats.Services.Procurement;
using Cats.Services.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cats.Models.Constant;

namespace Cats.Areas.Procurement.Controllers
{
    public class FetchDataController : Controller
    {
        private readonly IPaymentRequestService _paymentRequestService;
        private readonly IBidService _bidService;
        private readonly IUserAccountService _userAccountService;
        private readonly IBidWinnerService _bidWinnerService;
        private readonly ITransportBidQuotationService _priceQuotataion;
        private readonly ITransportBidQuotationHeaderService _bidQuotationHeader;
        private readonly ITransportBidPlanService _transportBidPlanService;

        //
        // GET: /Procurement/FetchData/

        public FetchDataController(IPaymentRequestService paymentRequestService,
            ITransportBidQuotationHeaderService bidQuotationHeader,IBidService bidService, IUserAccountService userAccountService, IBidWinnerService bidWinnerService, ITransportBidPlanService transportBidPlanService)
        {
            _paymentRequestService = paymentRequestService;
            _bidService = bidService;
            _userAccountService = userAccountService;
            _bidWinnerService = bidWinnerService;
            _bidQuotationHeader = bidQuotationHeader;
            _transportBidPlanService = transportBidPlanService;
        }

        public JsonResult ReadSummarizedNumbers([DataSourceRequest]DataSourceRequest request)
        {
            var paymentRequests = _paymentRequestService.GetAll().Count();
            var paymentRequestsFromTransporters = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Payment Requested").Count();
            var paymentRequestsAtLogistics = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Submitted for Approval").Count();
            var approvedPaymentRequests = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved for Payment").Count();
            var rejectedPaymentRequests = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Rejected").Count();
            var checkIssuedPaymentRequests = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Check Issued").Count();
            var checkCashedPaymentRequests = _paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Check Cashed").Count();
            var summarizedNumbersViewModel = new SummarizedNumbersViewModel()
                                                 {
                                                     ApprovedPaymentRequests = approvedPaymentRequests,
                                                     CheckCashedPaymentRequests = checkCashedPaymentRequests,
                                                     CheckIssuedPaymentRequests = checkIssuedPaymentRequests,
                                                     PaymentRequests = paymentRequests,
                                                     PaymentRequestsAtLogistics = paymentRequestsAtLogistics,
                                                     PaymentRequestsFromTransporters = paymentRequestsFromTransporters,
                                                     RejectedPaymentRequests = rejectedPaymentRequests
                                                 };
            return Json(summarizedNumbersViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PaymentRequestsStatus([DataSourceRequest]DataSourceRequest request)
        {
            var paymentRequestStatus = _paymentRequestService.GetAll().Take(10);
            var paymentRequestStatusViewModels = BindPaymentRequestViewModel(paymentRequestStatus);
            return Json(paymentRequestStatusViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PaymentRequestsPercentage([DataSourceRequest]DataSourceRequest request)
        {
            var totalPaymentRequests = _paymentRequestService.GetAll().Count();
            var paymentRequestsFromTransporters =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Payment Requested").Count() / totalPaymentRequests) * 100;
            var paymentRequestsAtLogistics =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Submitted for Approval").Count() / totalPaymentRequests) * 100;
            var approvedPaymentRequests =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved for Payment").Count() / totalPaymentRequests) * 100;
            var rejectedPaymentRequests =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Rejected").Count() / totalPaymentRequests) * 100;
            var checkIssuedPaymentRequests =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Check Issued").Count() / totalPaymentRequests) * 100;
            var checkCashedPaymentRequests =
                (_paymentRequestService.Get(t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Check Cashed").Count() / totalPaymentRequests) * 100;

            var paymentRequestPercentage = new PaymentRequestPercentageViewModel()
                                               {
                                                   Requested = paymentRequestsFromTransporters,
                                                   Submitted = paymentRequestsAtLogistics,
                                                   Approved = approvedPaymentRequests,
                                                   Rejected = rejectedPaymentRequests,
                                                   CheckIssued = checkIssuedPaymentRequests,
                                                   CheckCashed = checkCashedPaymentRequests
                                               };
            return Json(paymentRequestPercentage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PaymentRequest([DataSourceRequest]DataSourceRequest request)
        {
            var paymentRequest =
                _paymentRequestService.Get(
                    t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Payment Requested").ToList();
            var paymentRequestViewModels = BindPaymentRequestViewModel(paymentRequest);
            return Json(paymentRequestViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PaymentRequestAtLogistics([DataSourceRequest]DataSourceRequest request)
        {
            var paymentRequestAtLogistics =
                _paymentRequestService.Get(
                    t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Submitted for Approval").ToList();
            var paymentRequestAtLogisticViewModels = BindPaymentRequestViewModel(paymentRequestAtLogistics);
            return Json(paymentRequestAtLogisticViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifiedPaymentRequest([DataSourceRequest]DataSourceRequest request)
        {
            var verifiedPaymentRequest =
                _paymentRequestService.Get(
                    t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved for Payment").ToList();
            var verifiedPaymentRequestViewModels = BindPaymentRequestViewModel(verifiedPaymentRequest);
            return Json(verifiedPaymentRequestViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCashedPaymentRequest([DataSourceRequest]DataSourceRequest request)
        {
            var checkCashedPaymentRequest =
                _paymentRequestService.Get(
                    t => t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Check Cashed").ToList();
            var checkCashedPaymentRequestViewModels = BindPaymentRequestViewModel(checkCashedPaymentRequest);
            return Json(checkCashedPaymentRequestViewModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecentBids([DataSourceRequest]DataSourceRequest request)
        {
            var recentBids =
                _bidService.FindBy(t => t.StatusID == 5).OrderByDescending(t => t.OpeningDate).Take(10).ToList();
            var recentBidViewModels = BindBidViewModels(recentBids);
            return Json(recentBidViewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RecentBidsByStatus  (int statusId)
        {
            var recentBids =
                _bidService.FindBy(t => t.StatusID == statusId).OrderByDescending(t => t.OpeningDate).Take(10).ToList();
            var recentBidViewModels = BindBidViewModels(recentBids);
            return Json(recentBidViewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DataBidStatus([DataSourceRequest]DataSourceRequest request)
        {
            var bidStatus = _bidService.Get(null,null,"Status");
            var recentBids = (from item in bidStatus select item.Status).Distinct();
            //FindBy(t => t.StatusID == 5).OrderByDescending(t => t.OpeningDate).Take(10).ToList();

            var statusViewModels = BindStatusViewModels(recentBids);
            return Json(statusViewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WinnerRanks([DataSourceRequest]DataSourceRequest request)
        {
            var winners = _bidWinnerService.GetAllBidWinner().Select(x => x.Position).Distinct();

            

            var BidWinnerStatusViewModels = BindBidWinnerStatusViewModels(winners);
            return Json(BidWinnerStatusViewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WinnerList([DataSourceRequest]DataSourceRequest request)
        {
            var winners = _bidWinnerService.GetAllBidWinner().Distinct().ToList();

            var result = winners.GroupBy(t => t.BidID)
                    .Select(grp => grp.First())
                    .ToList();

            var BidWinnerStatusViewModels = BindBidWinnerListViewModels(result);
            return Json(BidWinnerStatusViewModels, JsonRequestBehavior.AllowGet);
        }
        public List<BidsViewModel> BindBidWinnerListViewModels(IEnumerable<BidWinner> bids)
        {
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            return bids.Select(bid => new BidsViewModel()
            {
                BidID = bid.BidID,
                BidNumber = bid.Bid.BidNumber
            }).Distinct().ToList();
        }


        public JsonResult DataEntryStatus([DataSourceRequest]DataSourceRequest request)
        {
            var recentBids =
                _bidService.Get(t => t.StatusID == 5, null,  "TransportBidQuotationHeaders");
                //FindBy(t => t.StatusID == 5).OrderByDescending(t => t.OpeningDate).Take(10).ToList();
            
            var recentBidViewModels = BindBidViewModels(recentBids);
            return Json(recentBidViewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectDataEntryStatus(int bidId)
        {
            var recentBids =
                _bidService.Get(t => t.BidID == bidId, null, "TransportBidQuotationHeaders");
            //FindBy(t => t.StatusID == 5).OrderByDescending(t => t.OpeningDate).Take(10).ToList();
            var firstOrDefault = recentBids.FirstOrDefault();
            if (firstOrDefault == null) return null;
            var dataEntryViewModel = BinddataEntryViewModels(firstOrDefault.TransportBidQuotationHeaders);
  
            // var recentBidViewModels = BindBidViewModels(recentBids);
            return Json(dataEntryViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PriceQoutation(int bidID)
        {
            var proposals = _bidQuotationHeader.FindBy(e=>e.BidId==bidID).OrderByDescending(t => t.TransportBidQuotationHeaderID);
            var r = (from proposal in proposals
                     select new TransportBidQuotationHeaderViewModel()
                     {
                         TransportBidQuotationHeaderID = proposal.TransportBidQuotationHeaderID,
                         //BidNumber = proposal.Bid.BidNumber,
                         //BidBondAmount = proposal.BidBondAmount,
                         OffersCount = proposal.TransportBidQuotations.Count,
                         //Region = proposal.AdminUnit.Name,
                         Status = proposal.Status == 1 ? "Draft" : "Approved",
                         Transporter = proposal.Transporter.Name,
                         EnteredBy = proposal.EnteredBy,
                         //BidID = proposal.Bid.BidID,
                         //RegionId = proposal.AdminUnit.AdminUnitID,
                         TransporterId = proposal.Transporter.TransporterID
                     });

           return Json(r, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GroupedWinners(int bidid , int rank)
        {
            var winners = _bidWinnerService.FindBy(t => t.BidID == bidid && t.Position == rank);
            //var recentBidViewModels = BindBidViewModels(recentBids);
            var list = winners.Select(winner => new
                {
                    transporter = winner.Transporter.Name,
                    //offers = firstwinners.Count
                }).ToList();

            var grouped = (
                            from r in winners
                            group r by new
                            {
                                r.BidID,
                                r.TransporterID
                            }
                                into g
                                select g
                            );
            var groupedwinners = new List<Object>();

            foreach (var transporter in grouped)
            {
                var detail = new
                    {
                        Name = transporter.First().Transporter.Name,
                        Count = transporter.Count(),
                        minoffer = transporter.Min(t=>t.Tariff),
                        maxoffer = transporter.Max(t=>t.Tariff)
                    };

                groupedwinners.Add(detail);
            }

            return Json(groupedwinners, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Woredaswithoutoffer([DataSourceRequest]DataSourceRequest request)
        {
            var recentBids =
                _bidService.FindBy(t => t.StatusID == 5).OrderByDescending(t => t.OpeningDate).Take(10).ToList();
            var recentBidViewModels = BindBidViewModels(recentBids);
            return Json(recentBidViewModels, JsonRequestBehavior.AllowGet);
        }

        public List<PaymentRequestViewModel> BindPaymentRequestViewModel(IEnumerable<PaymentRequest> paymentRequests)
        {
            return paymentRequests.Select(paymentRequest => new PaymentRequestViewModel()
                                                                {
                                                                    BusinessProcessID = paymentRequest.BusinessProcessID,
                                                                    PaymentRequestID = paymentRequest.PaymentRequestID,
                                                                    ReferenceNo = paymentRequest.ReferenceNo,
                                                                    RequestedAmount = paymentRequest.RequestedAmount,
                                                                    TransportOrderID = paymentRequest.TransportOrderID,
                                                                    TransportOrderNo = paymentRequest.TransportOrder.TransportOrderNo,
                                                                    TransporterName = paymentRequest.TransportOrder.Transporter.Name
                                                                }).ToList();
        }
        
            public List<DataEntryViewModel> BinddataEntryViewModels(IEnumerable<TransportBidQuotationHeader> bids)
            {
                var transportBidQuotationHeaders = bids as TransportBidQuotationHeader[] ?? bids.ToArray();
           
            
            int  curBid = transportBidQuotationHeaders.FirstOrDefault().Bid.BidID;
            TransportBidPlan bidPlan = _transportBidPlanService.FindById(transportBidQuotationHeaders.FirstOrDefault().Bid.TransportBidPlanID);
             int numberOfWoredas=    bidPlan.TransportBidPlanDetails.Count();
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;

            return transportBidQuotationHeaders.Select(bid => new DataEntryViewModel()
            {
                BidID = bid.Bid.BidID,
                BidNumber = bid.Bid.BidNumber,
                EndDate = bid.Bid.EndDate,
                OpeningDate = bid.Bid.OpeningDate.ToCTSPreferedDateFormat(datePref),
                StartDate = bid.Bid.StartDate,
               // TransportBidQuotationHeaders = bid.TransportBidQuotationHeaders.FirstOrDefault().Transporter.Name,
                Time = bid.Bid.OpeningDate.ToShortTimeString(),
                NoOfExpectedWoreda = numberOfWoredas,
                TransporterName = bid.Transporter.Name,
                NoOfWoredaWithRFQ = bid.Transporter.TransportBidQuotations.Count(e => e.BidID == curBid),
                StatusID = bid.Bid.StatusID
            }).ToList();
        }
        public List<BidsViewModel> BindBidViewModels(IEnumerable<Bid> bids)
        {
            var datePref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).DatePreference;
            return bids.Select(bid => new BidsViewModel()
                                        {
                                            BidID = bid.BidID,
                                            BidNumber = bid.BidNumber,
                                            EndDate = bid.EndDate,
                                            OpeningDate = bid.OpeningDate.ToCTSPreferedDateFormat(datePref),
                                            StartDate = bid.StartDate,
                                            Time = bid.OpeningDate.ToShortTimeString(),
                                            StatusID = bid.StatusID
                                        }).ToList();
        }
        private List<BidWinnerRank>  BindBidWinnerStatusViewModels(IEnumerable<int?> winners)
        {
            return winners.Select(winner => winner != null ? new BidWinnerRank()
            {
                Id = winner.Value,
                Name = winner.Value == 1 ? "First" : "Second",

            } : null).ToList();
          
        }
        public List<StatusViewmodel> BindStatusViewModels(IEnumerable<Status> bids)
        {
          
            return bids.Select(bid => new StatusViewmodel()
            {
                StatusId = bid.StatusID,
                Status =    bid.Name

            }).ToList();
        }

    }
}
