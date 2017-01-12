using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Procurement;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class TransportBidQuotationController : ApiController
    {
        private readonly ITransportBidQuotationService _transportBidQuotationService;

        public TransportBidQuotationController(ITransportBidQuotationService transportBidQuotationService)
        {
            _transportBidQuotationService = transportBidQuotationService;
        }
        public IEnumerable<TransportBidQuotation> GetTransportBidQuotations()
        {
            return (from bq in _transportBidQuotationService.GetAllTransportBidQuotation()
                select new TransportBidQuotation
                {
                    SourceID = bq.SourceID,
                    DestinationID = bq.DestinationID,
                    Remark = bq.Remark,
                    BidID = bq.BidID,
                    IsWinner = bq.IsWinner,
                    Position = bq.Position,
                    Tariff = bq.Tariff,
                    TransportBidQuotationHeaderID = bq.TransportBidQuotationHeaderID,
                    TransportBidQuotationID = bq.TransportBidQuotationID,
                    TransporterID = bq.TransporterID,
                }).ToList();
        }

        // GET api/<controller>/5
        public TransportBidQuotation GetBidQuotation(int id)
        {
            var transportBidQuotation = _transportBidQuotationService.FindById(id);
            if (transportBidQuotation == null) return null;
            return new TransportBidQuotation
            {
                SourceID = transportBidQuotation.SourceID,
                DestinationID = transportBidQuotation.DestinationID,
                Remark = transportBidQuotation.Remark,
                BidID = transportBidQuotation.BidID,
                IsWinner = transportBidQuotation.IsWinner,
                Position = transportBidQuotation.Position,
                Tariff = transportBidQuotation.Tariff,
                TransportBidQuotationHeaderID = transportBidQuotation.TransportBidQuotationHeaderID,
                TransportBidQuotationID = transportBidQuotation.TransportBidQuotationID,
                TransporterID = transportBidQuotation.TransporterID,
            };
        }

    }
}