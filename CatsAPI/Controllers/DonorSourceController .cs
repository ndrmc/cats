using Cats.Services.EarlyWarning;
using System.Linq;
using System.Web.Http;


namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DonorController : ApiController
    {
        private readonly IDonorService _idonorService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idonorService"></param>
        public DonorController(IDonorService idonorService)
        {
            _idonorService = idonorService;
        }

        /// <summary>
        /// Returns list of Donors objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetDonors()
        {
            var bids = _idonorService.GetAllDonor().Select(item => new
            {
                item.DonorID,
                item.Name,
                item.DonorCode,
                item.IsResponsibleDonor,
                item.IsSourceDonor,
                item.LongName
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a Donor object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetDonor(int id)
        {
            var obj = _idonorService.FindById(id);
            var element = new
            {
                obj.DonorID,
                obj.Name,
                obj.DonorCode,
                obj.IsResponsibleDonor,
                obj.IsSourceDonor,
                obj.LongName
            };

            return element;
        }
    }
}
