using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Hub;

namespace Cats.Rest.Controllers
{
    public class TransactionController : ApiController

    {
        //
        // GET: /Transaction/

        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        /// <summary>
        /// Gets all the transction Id and transaction group Id
        /// </summary>
        /// <returns></returns>
        public List<Models.Transaction> Get()
        {
            var transactions = _transactionService.GetAll();
            return transactions.Select(transaction => new Models.Transaction(transaction.TransactionID, transaction.TransactionGroupID)).ToList();
        }
        /// <summary>
        /// Gets a single object of transaction provided a transaction Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Transaction Get(Guid id)
        {
            var transaction = _transactionService.FindById(id);
            if (transaction!=null)
            {
                var newTrans = new Models.Transaction(transaction.TransactionID, transaction.TransactionGroupID);
                return newTrans;
            }
            return null;
        }
    }
}
 