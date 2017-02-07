using Cats.Rest.Models;
using Cats.Services.Procurement;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BidController : ApiController
    {
        private readonly IBidService _ibidService;
        private readonly IBidDetailService _ibidDetailService;
        private readonly IBidWinnerService _ibidWinnerService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ibidService"></param>
        /// <param name="ibidDetailService"></param>
        /// <param name="ibidWinnerService"></param>
        public BidController(IBidService ibidService,
                             IBidDetailService ibidDetailService,
                             IBidWinnerService ibidWinnerService)
        {

            _ibidService = ibidService;
            _ibidDetailService = ibidDetailService;
            _ibidWinnerService = ibidWinnerService;
        }
        /// <summary>
        /// Returns list of Bid objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBids()
        {
            var bids = _ibidService.GetAllBid().Select(item => new BidModelView
            {
                BidID = item.BidID,
                RegionID = item.RegionID,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                BidNumber = item.BidNumber,
                OpeningDate = item.OpeningDate,
                StatusID = item.StatusID,
                Status = item.Status == null ? string.Empty : item.Status.Name,
                TransportBidPlanID = item.TransportBidPlanID,
                BidBondAmount = item.BidBondAmount,
                startTime = item.startTime,
                endTime = item.endTime,
                BidOpeningTime = item.BidOpeningTime,
                BidDetails = item.BidDetails.Select(detail => new BidDetailModelView
                {
                    BidDetailId = detail.BidDetailID,
                    AmountForReliefProgram = detail.AmountForReliefProgram,
                    AmountForPSNPProgram = detail.AmountForPSNPProgram,
                    BidDocumentPrice = detail.BidDocumentPrice,
                    CPO = detail.CPO
                }).ToList(),
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a Bid object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBid(int id)
        {
            var obj = _ibidService.FindById(id);
            var element = new
            {
                BidID = obj.BidID,
                RegionID = obj.RegionID,
                StartDate = obj.StartDate,
                EndDate = obj.EndDate,
                BidNumber = obj.BidNumber,
                OpeningDate = obj.OpeningDate,
                StatusID = obj.StatusID,
                Status = obj.Status == null ? string.Empty : obj.Status.Name,
                TransportBidPlanID = obj.TransportBidPlanID,
                BidBondAmount = obj.BidBondAmount,
                startTime = obj.startTime,
                endTime = obj.endTime,
                BidOpeningTime = obj.BidOpeningTime,
                BidDetails = obj.BidDetails.Select(detail => new BidDetailModelView
                {
                    BidDetailId = detail.BidDetailID,
                    AmountForReliefProgram = detail.AmountForReliefProgram,
                    AmountForPSNPProgram = detail.AmountForPSNPProgram,
                    BidDocumentPrice = detail.BidDocumentPrice,
                    CPO = detail.CPO
                }).ToList(),
            };

            return element;
        }
    }
}
