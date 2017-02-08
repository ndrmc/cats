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
    public class BidWinnerController : ApiController
    {
        private readonly IBidWinnerService _bidWinnerService;

        public BidWinnerController(IBidWinnerService bidWinnerService)
        {
            _bidWinnerService = bidWinnerService;
        }
        // GET api/<controller>
        public IEnumerable<BidWinner> GetBidWinners()
        {
            return (from bw in _bidWinnerService.GetAllBidWinner()
                select new BidWinner()
                {
                    Amount = bw.Amount,
                    BidID = bw.BidID,
                    BidWinnerID = bw.BidWinnerID,
                    CommodityID = bw.CommodityID,
                    DestinationID = bw.DestinationID,
                    ExpiryDate = bw.ExpiryDate,
                    Position = bw.Position,
                    SourceID = bw.SourceID,
                    Status = bw.Status,
                    Tariff = bw.Tariff
                }).ToList();
        }

        // GET api/<controller>/5
        public BidWinner GetBidWinner(int id)
        {
            var bw = _bidWinnerService.FindById(id);
            return new BidWinner()
            {
                Amount = bw.Amount,
                BidID = bw.BidID,
                BidWinnerID = bw.BidWinnerID,
                CommodityID = bw.CommodityID,
                DestinationID = bw.DestinationID,
                ExpiryDate = bw.ExpiryDate,
                Position = bw.Position,
                SourceID = bw.SourceID,
                Status = bw.Status,
                Tariff = bw.Tariff
            };
        }

    }
}