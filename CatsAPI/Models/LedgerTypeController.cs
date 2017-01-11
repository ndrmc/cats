using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Hub;
using Cats.Rest.Models;
namespace Cats.Rest.Models
{
    public class LedgerTypeController : ApiController
    {
        private readonly ILedgerTypeService _ledgerTypeService;

        public LedgerTypeController(ILedgerTypeService ledgerTypeService)
        {
            _ledgerTypeService = ledgerTypeService;
        }
        [HttpGet]
        public IEnumerable<LedgerType> GetlLedgerTypes()
        {
            return (from l in _ledgerTypeService.GetAllLedgerType()
                select new LedgerType
                {
                    LedgerTypeID = l.LedgerTypeID,
                    Direction = l.Direction,
                    Name = l.Name
                }).ToList();
        }

        [HttpGet]
        public LedgerType GetLedgerType(int id)
        {
            var ledgerType = _ledgerTypeService.FindById(id);
            if (ledgerType == null) return null;
               
            return new LedgerType
            {
                LedgerTypeID = ledgerType.LedgerTypeID,
                Direction = ledgerType.Direction,
                Name = ledgerType.Name
            };
        }

        
    }
}