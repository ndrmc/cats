using System.Linq;
using System.Web.Http;
using Cats.Services.Procurement;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BidDetailController : ApiController
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
        public BidDetailController(IBidService ibidService,
                             IBidDetailService ibidDetailService,
                             IBidWinnerService ibidWinnerService)
        {
            _ibidService = ibidService;
            _ibidDetailService = ibidDetailService;
            _ibidWinnerService = ibidWinnerService;
        }

        /// <summary>
        /// Returns list of BidDetail objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBidDetails()
        {
            var bids = _ibidDetailService.GetAllBidDetail().Select(item => new
            {
                item.AmountForReliefProgram,
                item.AmountForPSNPProgram,
                item.BidDocumentPrice,
                item.CPO
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a BidDetail object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBidDetail(int id)
        {
            var bid = _ibidDetailService.FindById(id);

            return bid;
        }
    }
}
