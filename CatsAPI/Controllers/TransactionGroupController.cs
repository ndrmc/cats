using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Hub;

namespace Cats.Rest.Controllers
{
    public class TransactionGroupController : ApiController

    {
        //
        // GET: /TransactionGroup/

        private readonly ITransactionGroupService _transactionGroupService;

        public TransactionGroupController(ITransactionGroupService transactionGroupService)
        {
            _transactionGroupService = transactionGroupService;
        }

        public List<Models.TransactionGroup> Get()
        {
            var transactions = _transactionGroupService.GetAllTransactionGroup();
            return transactions.Select(transactionGroup => new Models.TransactionGroup(transactionGroup.TransactionGroupID)).ToList();
        }
        public Models.TransactionGroup Get(Guid id)
        {
            var transaction = _transactionGroupService.FindById(id);
            if (transaction!=null)
            {
                var newtransactionGroup = new Models.TransactionGroup(transaction.TransactionGroupID);
                return newtransactionGroup;
            }
            return null;
        }
    }
}
