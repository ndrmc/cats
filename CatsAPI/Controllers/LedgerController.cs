using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Hub;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class LedgerController : ApiController
    {
        private readonly ILedgerService _ledgerService;

        public LedgerController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }
        [HttpGet]
        public IEnumerable<Ledger> GetLedgers()
        {
            return (from l in _ledgerService.GetAllLedger()
                select new Ledger()
                {
                    LedgerID = l.LedgerID,
                    Name = l.Name,
                    LedgerTypeID = l.LedgerTypeID,
                    LedgerTypeName = l.LedgerType.Name,
                }).ToList();
        }

       [HttpGet]
        public Ledger GetLedger(int id)
       {
           var l = _ledgerService.FindById(id);
            if(l== null) return null;
           return new Ledger()
           {
               LedgerID = l.LedgerID,
               Name = l.Name,
               LedgerTypeID = l.LedgerTypeID,
               LedgerTypeName = l.LedgerType.Name,
           };
       }

    }
}