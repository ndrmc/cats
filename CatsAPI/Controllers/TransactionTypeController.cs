using Cats.Services.Transaction;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class TransactionTypeController : ApiController
    {
        private readonly ITranscationTypeService _iTransactionTypeService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="iTransactionTypeService"></param>
        public TransactionTypeController(ITranscationTypeService iTransactionTypeService)
        {
            _iTransactionTypeService = iTransactionTypeService;
        }
        /// <summary>
        /// Returns list of TransactionType objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetTransactionTypes()
        {
            var results = _iTransactionTypeService.GetAllTranscationType().Select(item => new
						   {
							item.TransactionTypeID,
							item.Name,
							item.Description
						  }).ToList();

            return results;
        }
        /// <summary>
        /// Given an id returns a TransactionType object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetTransactionType(int id)
        {
            var obj = _iTransactionTypeService.FindById(id);
            var element = new
						 {
			 			obj.TransactionTypeID,
						obj.Name,
						obj.Description
						 };

            return element;
        }
    }
}


