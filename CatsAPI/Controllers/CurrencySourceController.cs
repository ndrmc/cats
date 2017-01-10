using Cats.Services.EarlyWarning;
using System.Linq;
using System.Web.Http;


namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyController : ApiController
    {
        private readonly ICurrencyService _icurrencyService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="icurrencyService"></param>
        public CurrencyController(ICurrencyService icurrencyService)
        {
            _icurrencyService = icurrencyService;
        }

        /// <summary>
        /// Returns list of Currency objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetCurrencies()
        {
            var bids = _icurrencyService.GetAllCurrency().Select(item => new
            {
             item.CurrencyID,
             item.Code,
             item.Name
            }).ToList();

            return bids;
        }
        /// <summary>
        /// Given an id returns a Currency object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetCurrency(int id)
        {
            var bid = _icurrencyService.FindById(id);

            return bid;
        }
    }
}
