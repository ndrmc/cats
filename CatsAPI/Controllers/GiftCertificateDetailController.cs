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
    public class GiftCertificateDetailController : ApiController
    {
        private readonly IGiftCertificateDetailService _giftCerteficateDetailService;
        public GiftCertificateDetailController(IGiftCertificateDetailService giftCerteficateDetailService)
        {
            _giftCerteficateDetailService = giftCerteficateDetailService;
        }
        /// <summary>
        /// Get all gift certeficate details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<GiftCertificateDetail> GetGiftCerteficateDetails()
        {
            return (from gcd in _giftCerteficateDetailService.GetAllGiftCertificateDetail()
                    select new Models.GiftCertificateDetail()
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
                    }).ToList();
        }

      /// <summary>
      ///  Get gift certeficate by Id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        public GiftCertificateDetail GetGiftCerteficateDetail(int id)
        {
            var gcd = _giftCerteficateDetailService.FindById(id);
            if (gcd == null) return null;
            return new Models.GiftCertificateDetail()
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
                    };
        }

    }
}