using System.Linq;
using System.Web.Http;
using Cats.Services.Procurement;
using Cats.Models.ViewModels.Bid;
using Cats.Rest.Models;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BidDetailController : ApiController
    {
        //private readonly IBidService _ibidService;
        private readonly IBidDetailService _ibidDetailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ibidDetailService"></param>
        public BidDetailController(IBidDetailService ibidDetailService)
        {
            //_ibidService = ibidService;
            _ibidDetailService = ibidDetailService;
        }

        /// <summary>
        /// Returns list of BidDetail objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBidDetails()
        {
            var bidDetails = _ibidDetailService.GetAllBidDetail().Select(item => new BidDetailModelView
            {
                BidDetailId = item.BidDetailID,
                AmountForReliefProgram = item.AmountForReliefProgram,
                AmountForPSNPProgram = item.AmountForPSNPProgram,
                BidDocumentPrice = item.BidDocumentPrice,
                CPO = item.CPO
            }).ToList();

            return bidDetails;
        }
        /// <summary>
        /// Given an id returns a BidDetail object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetBidDetail(int id)
        {
            var obj = _ibidDetailService.FindById(id);
            var element = new BidDetailModelView
            {
                BidDetailId = obj.BidDetailID,
                AmountForReliefProgram = obj.AmountForReliefProgram,
                AmountForPSNPProgram = obj.AmountForPSNPProgram,
                BidDocumentPrice = obj.BidDocumentPrice,
                CPO = obj.CPO
            };

            return element;
        }
    }
}
