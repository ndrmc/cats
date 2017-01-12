using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.Procurement;
namespace Cats.Rest.Controllers
{
    public class TransportBidPlanDetailController : ApiController
    {

        private readonly ITransportBidPlanDetailService _transportBidPlanDetailService;

        public TransportBidPlanDetailController(ITransportBidPlanDetailService transportBidPlanDetailService)
        {
            _transportBidPlanDetailService = transportBidPlanDetailService;
        }
        public IEnumerable<TransportBidPlanDetail> GetTransportBidPlanDetails()
        {
            return (from d in _transportBidPlanDetailService.GetAllTransportBidPlanDetail()
                select new TransportBidPlanDetail()
                {
                    ProgramID = d.ProgramID,
                    BidPlanID = d.BidPlanID,
                    DestinationID = d.DestinationID,
                    Quantity = d.Quantity,
                    SourceID = d.SourceID,
                    TransportBidPlanDetailID = d.TransportBidPlanDetailID
                }).ToList();
        }

        // GET api/<controller>/5
        public TransportBidPlanDetail GetTransportBidPlanDetail(int id)
        {
            var transportBidPlanDetail = _transportBidPlanDetailService.FindById(id);
            if (transportBidPlanDetail == null) return null;
            return new TransportBidPlanDetail()
            {
                ProgramID = transportBidPlanDetail.ProgramID,
                BidPlanID = transportBidPlanDetail.BidPlanID,
                DestinationID = transportBidPlanDetail.DestinationID,
                Quantity = transportBidPlanDetail.Quantity,
                SourceID = transportBidPlanDetail.SourceID,
                TransportBidPlanDetailID = transportBidPlanDetail.TransportBidPlanDetailID
            };
            
        }

       
    }
}