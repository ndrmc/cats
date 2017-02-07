using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;

namespace Cats.Rest.Controllers
{
    public class ReliefRequistionDetailController : ApiController
    {
        private readonly IReliefRequisitionDetailService _reliefRequisitionDetailService;

        public ReliefRequistionDetailController(IReliefRequisitionDetailService reliefRequisitionDetailService)
        {
            _reliefRequisitionDetailService = reliefRequisitionDetailService;
        }
        // GET api/<controller>
        public IEnumerable<ReliefRequisitionDetail> GetReliefRequisitionDetails()
        {
            return (from rrd in _reliefRequisitionDetailService.GetAllReliefRequisitionDetail()
                select new ReliefRequisitionDetail()
                {
                    CommodityName = rrd.Commodity != null ? rrd.Commodity.Name : String.Empty,
                    Amount = rrd.Amount,
                    BenficiaryNo = rrd.BenficiaryNo,
                    CommodityID = rrd.CommodityID,
                    DonorID = rrd.DonorID,
                    Contingency = rrd.Contingency,
                    DonorName = rrd.Donor != null ? rrd.Donor.Name : string.Empty,
                    RequisitionID = rrd.RequisitionID,
                    FDPID = rrd.FDPID,
                    FDPName = rrd.FDP.Name,
                    RequisitionDetailID = rrd.RequisitionDetailID
                }).ToList();
        }

        // GET api/<controller>/5
        public ReliefRequisitionDetail GetReliefRequisitionDetail(int id)
        {
            var rrd = _reliefRequisitionDetailService.FindById(id);
            return new ReliefRequisitionDetail()
            {
                CommodityName = rrd.Commodity != null ? rrd.Commodity.Name : String.Empty,
                Amount = rrd.Amount,
                BenficiaryNo = rrd.BenficiaryNo,
                CommodityID = rrd.CommodityID,
                DonorID = rrd.DonorID,
                Contingency = rrd.Contingency,
                DonorName = rrd.Donor != null ? rrd.Donor.Name : string.Empty,
                RequisitionID = rrd.RequisitionID,
                FDPID = rrd.FDPID,
                FDPName = rrd.FDP.Name,
                RequisitionDetailID = rrd.RequisitionDetailID
            };
        }

        
    }
}