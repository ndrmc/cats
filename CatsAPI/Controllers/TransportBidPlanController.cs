using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Procurement;
using Cats.Rest.Models;;
namespace Cats.Rest.Controllers
{
    public class TransportBidPlanController : ApiController
    {
        private readonly ITransportBidPlanService _transportBidPlanService;

        public TransportBidPlanController(ITransportBidPlanService transportBidPlanService)
        {
            _transportBidPlanService = transportBidPlanService;
        }
        public IEnumerable<TransportBidPlan> GettTransportBidPlans()
        {
            return (from tbp in _transportBidPlanService.GetAllTransportBidPlan()
                select new TransportBidPlan
                {
                    Year = tbp.Year,
                    ProgramName = tbp.Program.Name,
                    ProgramID = tbp.ProgramID,
                    TransportBidPlanID = tbp.TransportBidPlanID,
                    YearHalf = tbp.YearHalf,
                    TransportBidPlanDetails = (from d in tbp.TransportBidPlanDetails
                        select new TransportBidPlanDetail
                        {
                            ProgramID = d.ProgramID,
                            BidPlanID = d.BidPlanID,
                            DestinationID = d.DestinationID,
                            Quantity = d.Quantity,
                            SourceID = d.SourceID,
                            TransportBidPlanDetailID = d.TransportBidPlanDetailID
                        }).ToList()
                }).ToList();
        }

        // GET api/<controller>/5
        public TransportBidPlan GetTransportBidPlan(int id)
        {
            var tranportBidPlan = _transportBidPlanService.FindById(id);
            if (tranportBidPlan == null) return null;
            return new TransportBidPlan
            {
                Year = tranportBidPlan.Year,
                ProgramName = tranportBidPlan.Program.Name,
                ProgramID = tranportBidPlan.ProgramID,
                TransportBidPlanID = tranportBidPlan.TransportBidPlanID,
                YearHalf = tranportBidPlan.YearHalf,
                TransportBidPlanDetails = (from d in tranportBidPlan.TransportBidPlanDetails
                    select new TransportBidPlanDetail
                    {
                        ProgramID = d.ProgramID,
                        BidPlanID = d.BidPlanID,
                        DestinationID = d.DestinationID,
                        Quantity = d.Quantity,
                        SourceID = d.SourceID,
                        TransportBidPlanDetailID = d.TransportBidPlanDetailID
                    }).ToList()
            };
        }

      
    }
}