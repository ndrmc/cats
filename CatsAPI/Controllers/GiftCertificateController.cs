using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.EarlyWarning;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class GiftCertificateController : ApiController
    {
        private readonly IGiftCertificateService _giftCerteficateService;
        public GiftCertificateController(IGiftCertificateService giftCerteficateService)
        {
            _giftCerteficateService = giftCerteficateService;
        }
        /// <summary>
        /// Get all gift certificates
        /// </summary>
        /// <remarks>
        ///  To get all giftcerteficates use this route /api/GiftCertificate
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<GiftCerteficate> GetGiftCerteficates()
        {
            var certeficates = _giftCerteficateService.GetAllGiftCertificate();
            var giftCerteficates = new List<GiftCerteficate>();
            GiftCerteficate giftCerteficate;
            foreach(var gc in certeficates)
            {
                giftCerteficate = new GiftCerteficate()
                {
                    DeclarationNumber = gc.DeclarationNumber,
                    DModeOfTransport = gc.DModeOfTransport,
                    DonorID = gc.DonorID,
                    ETA = gc.ETA,
                    GiftCertificateID = gc.GiftCertificateID,
                    GiftDate = gc.GiftDate,
                    IsPrinted = gc.IsPrinted,
                    PortName = gc.PortName,
                    PartitionId = gc.PartitionId,
                    ProgramID = gc.ProgramID,
                    ProgramName = gc.Program.Name,
                    ReferenceNo = gc.ReferenceNo,
                    ShippingInstructionID = gc.ShippingInstructionID,
                    StatusID = gc.StatusID,
                    TransactionGroupID = gc.TransactionGroupID,
                    Vessel = gc.Vessel,
                    GiftCerteficateDetails = (from gcd in gc.GiftCertificateDetails
                                              select new GiftCertificateDetail()
                                              {
                                                  AccountNumber = gcd.AccountNumber,
                                                  BillOfLoading = gcd.BillOfLoading,
                                                  //BudgetTypeName = ,
                                                  CommodityID = gcd.CommodityID,
                                                  CommodityName = gcd.Commodity.Name,
                                                  //CurrencyName = gcd
                                                  DBudgetTypeID = gcd.DBudgetTypeID,
                                                  DCurrencyID = gcd.DCurrencyID,
                                                  DFundSourceID = gcd.DFundSourceID,
                                                  EstimatedPrice = gcd.EstimatedPrice,
                                                  EstimatedTax = gcd.EstimatedTax,
                                                  //ExpiryDate = gcd.ExpiryDate,
                                                  GiftCertificateDetailID = gcd.GiftCertificateDetailID,
                                                  GiftCertificateID = gcd.GiftCertificateID,
                                                  PartitionId = gcd.PartitionId,
                                                  TransactionGroupID = gcd.TransactionGroupID,
                                                  WeightInMT = gcd.WeightInMT,
                                                  YearPurchased = gcd.YearPurchased
                                              }).ToList()
                };
                giftCerteficates.Add(giftCerteficate);
            }
            return giftCerteficates;
        }

        /// <summary>
        /// Get gift certeficate by Id
        /// </summary>
        /// <remarks>
        ///  get api/GetGiftCerteficate/{id}
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        public GiftCerteficate GetGiftCerteficate(int id)
        {
            var gc = _giftCerteficateService.FindById(id);
         
            GiftCerteficate giftCerteficate;

            giftCerteficate = new GiftCerteficate()
            {
                DeclarationNumber = gc.DeclarationNumber,
                DModeOfTransport = gc.DModeOfTransport,
                DonorID = gc.DonorID,
                ETA = gc.ETA,
                GiftCertificateID = gc.GiftCertificateID,
                GiftDate = gc.GiftDate,
                IsPrinted = gc.IsPrinted,
                PortName = gc.PortName,
                PartitionId = gc.PartitionId,
                ProgramID = gc.ProgramID,
                ProgramName = gc.Program.Name,
                ReferenceNo = gc.ReferenceNo,
                ShippingInstructionID = gc.ShippingInstructionID,
                StatusID = gc.StatusID,
                TransactionGroupID = gc.TransactionGroupID,
                Vessel = gc.Vessel,
                GiftCerteficateDetails = (from gcd in gc.GiftCertificateDetails
                                          select new GiftCertificateDetail()
                                          {
                                              AccountNumber = gcd.AccountNumber,
                                              BillOfLoading = gcd.BillOfLoading,
                                              //BudgetTypeName = ,
                                              CommodityID = gcd.CommodityID,
                                              CommodityName = gcd.Commodity.Name,
                                              //CurrencyName = gcd
                                              DBudgetTypeID = gcd.DBudgetTypeID,
                                              DCurrencyID = gcd.DCurrencyID,
                                              DFundSourceID = gcd.DFundSourceID,
                                              EstimatedPrice = gcd.EstimatedPrice,
                                              EstimatedTax = gcd.EstimatedTax,
                                              //ExpiryDate = gcd.ExpiryDate,
                                              GiftCertificateDetailID = gcd.GiftCertificateDetailID,
                                              GiftCertificateID = gcd.GiftCertificateID,
                                              PartitionId = gcd.PartitionId,
                                              TransactionGroupID = gcd.TransactionGroupID,
                                              WeightInMT = gcd.WeightInMT,
                                              YearPurchased = gcd.YearPurchased
                                          }).ToList()
            };
                
            return giftCerteficate;
        }

      
    }
}