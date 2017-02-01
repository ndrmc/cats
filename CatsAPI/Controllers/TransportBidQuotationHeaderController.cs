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
    public class TransportBidQuotationHeaderController : ApiController
    {
        protected readonly ITransportBidQuotationHeaderService _transportBidQuotationHeaderService;

        public TransportBidQuotationHeaderController(
            ITransportBidQuotationHeaderService transportBidQuotationHeaderService)
        {
            _transportBidQuotationHeaderService = transportBidQuotationHeaderService;
        }
        // GET api/<controller>
        public IEnumerable<Models.TransportBidQuotationHeader> GetTransportBidQuotationHeaders()
        {
            return (from t in _transportBidQuotationHeaderService.GetAllTransportBidQuotationHeader()
                select new TransportBidQuotationHeader()
                {                
                    TransportBidQuotationHeaderID = t.TransportBidQuotationHeaderID,
                    BidBondAmount = Convert.ToDecimal(t.Bid.BidBondAmount),
                    RegionID = t.RegionID,
                    BidId = t.BidId,
                    BidQuotationDate = t.BidQuotationDate,
                    EnteredBy = t.EnteredBy,
                    RegionName = t.AdminUnit.Name,
                    Status = t.Status,
                    statusName = t.BusinessProcess != null ? t.BusinessProcess.CurrentState.BaseStateTemplate.Name:string.Empty,
                    TransportBidQuotations = (from bq in t.TransportBidQuotations
                                              select new TransportBidQuotation()
                                              {
                                                  BidID = bq.BidID,
                                                  DestinationID = bq.DestinationID,
                                                  Position = bq.Position,
                                                  IsWinner = bq.IsWinner,
                                                  Remark = bq.Remark,
                                                  SourceID = bq.SourceID,
                                                  Tariff = bq.Tariff,
                                                  TransportBidQuotationHeaderID = bq.TransportBidQuotationHeaderID,
                                                  TransportBidQuotationID = bq.TransportBidQuotationID,
                                                  TransporterID = bq.TransporterID
                                              }).ToList(),
                    TransporterId = t.TransporterId,
                    TransporterName = t.Transporter.Name,           

                }).ToList();
        }

        // GET api/<controller>/5
        public TransportBidQuotationHeader GetTransportBidQuotationHeader(int id)
        {
            var t = _transportBidQuotationHeaderService.FindById(id);
            return
                new TransportBidQuotationHeader()
                {
                    TransportBidQuotationHeaderID = t.TransportBidQuotationHeaderID,
                    BidBondAmount = Convert.ToDecimal(t.Bid.BidBondAmount),
                    RegionID = t.RegionID,
                    BidId = t.BidId,
                    BidQuotationDate = t.BidQuotationDate,
                    EnteredBy = t.EnteredBy,
                    RegionName = t.AdminUnit.Name,
                    Status = t.Status,
                    statusName = t.BusinessProcess != null? t.BusinessProcess.CurrentState.BaseStateTemplate.Name : string.Empty,
                    TransportBidQuotations = (from bq in t.TransportBidQuotations
                        select new TransportBidQuotation()
                        {
                            BidID = bq.BidID,
                            DestinationID = bq.DestinationID,
                            Position = bq.Position,
                            IsWinner = bq.IsWinner,
                            Remark = bq.Remark,
                            SourceID = bq.SourceID,
                            Tariff = bq.Tariff,
                            TransportBidQuotationHeaderID = bq.TransportBidQuotationHeaderID,
                            TransportBidQuotationID = bq.TransportBidQuotationID,
                            TransporterID = bq.TransporterID
                        }).ToList(),
                    TransporterId = t.TransporterId,
                    TransporterName = t.Transporter.Name,

                };
        }

    }
}