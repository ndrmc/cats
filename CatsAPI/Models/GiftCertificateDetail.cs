using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class GiftCertificateDetail
    {
        //GiftCertificateDetailId, TransactionGroupId, 
        //    GiftCertificateId, CommodityID, CommodityName, WeightMT, 
        //    BillOfLadding, AccountNumber, EstimatedPrice, EstimatedTax, YearPurchased, 
        //    DFundSourceId, FundSourceName, DCurrencyId, CurrencyName, DBudgetTypeId, BudgetTypeName, ExpiryDate
        public int GiftCertificateDetailID { get; set; }
        public int? PartitionId { get; set; }
        public int TransactionGroupID { get; set; }
        public int GiftCertificateID { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public decimal WeightInMT { get; set; }
        public string BillOfLoading { get; set; }
        public int AccountNumber { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal EstimatedTax { get; set; }
        public string YearPurchased { get; set; }
        public int DFundSourceID { get; set; }
        public string FunDourceName { get; set; }
        public int DCurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public int DBudgetTypeID { get; set; }
        public string BudgetTypeName { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}