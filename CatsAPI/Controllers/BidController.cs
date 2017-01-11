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
            var bids = _ibidService.GetAllBid().Select(item => new
            {
                item.BidID,
                item.RegionID,
                item.StartDate,
                item.EndDate,
                item.BidNumber,
                item.OpeningDate,
                item.StatusID,
                item.Status,
                item.TransportBidPlanID,
                item.BidBondAmount,
                item.startTime,
                item.endTime,
                item.BidOpeningTime,
                item.BidDetails
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
            var bid = _ibidService.FindById(id);

            return bid;
        }
    }
}
