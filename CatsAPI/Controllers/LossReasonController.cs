using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using  Cats.Services.Administration;
namespace Cats.Rest.Controllers
{
    public class LossReasonController : ApiController
    {
        private readonly ILossReasonService _lossReasonService;

        public LossReasonController(ILossReasonService loasReasonService)
        {
            _lossReasonService = loasReasonService;
        }
        
        [HttpGet]
        public IEnumerable<LossReason> GetLossReasons()
        {
            return (from l in _lossReasonService.GetAllLossReason()
                select new LossReason()
                {
                    LossReasonAm = l.LossReasonAm,
                    LossReasonCodeAm = l.LossReasonCodeAm,
                    LossReasonCodeEg = l.LossReasonCodeEg,
                    LossReasonEg = l.LossReasonEg,
                    LossReasonId = l.LossReasonId
                }).ToList();
        }

        // GET api/<controller>/5
        public LossReason GetLossReason(int id)
        {
            var lossReason = _lossReasonService.FindById(id);
            if(lossReason == null) return null;
            return new LossReason
            {
                LossReasonAm = lossReason.LossReasonAm,
                LossReasonCodeAm = lossReason.LossReasonCodeAm,
                LossReasonCodeEg = lossReason.LossReasonCodeEg,
                LossReasonEg = lossReason.LossReasonEg,
                LossReasonId = lossReason.LossReasonId
            };
        }

      
    }
}