﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Cats.Models.Hubs;

namespace Cats.Models.Hubs
{
    public class GiftCertificateDetailsViewModel
    {
        //[Required(ErrorMessage = "Gift Certificate Detail is required")]
        public Int32 GiftCertificateDetailID { get; set; }

        //[Required(ErrorMessage = "Transaction Group is required")]
        public Int32 TransactionGroupID { get; set; }

        [Required(ErrorMessage = "Gift Certificate is required")]
        public Int32 GiftCertificateID { get; set; }

        [Required(ErrorMessage = "Commodity is required")]
        public Int32 CommodityID { get; set; }

        //public Decimal GrossWeightInMT { get; set; }

        [Required(ErrorMessage = "Weight In MT is required")]
        [Range(0.5, 999999.99)]
        public Decimal WeightInMT { get; set; }

        private string _billOfLoading;

        [StringLength(50)]
        [UIHint("WayBillWarning")]
        public String BillOfLoading
        {
            get { return _billOfLoading; }
            set { _billOfLoading = value; }
        }

        [Required(ErrorMessage = "Account Number is required")]
        public Int32 AccountNumber { get; set; }

        [Required(ErrorMessage = "Estimated Price is required")]
        [Range(0, 9999999999999.99)]
        public Decimal EstimatedPrice { get; set; }

        [Required(ErrorMessage = "Estimated Tax is required")]
        [Range(0, 9999999999999.99)]
        public Decimal EstimatedTax { get; set; }

        [Required(ErrorMessage = "Year Purchased is required")]
        public DateTime YearPurchased { get; set; }

        [Required(ErrorMessage = "Fund Source is required")]
        public Int32 DFundSourceID { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public Int32 DCurrencyID { get; set; }

        [Required(ErrorMessage = "Budget Type is required")]
        public Int32 DBudgetTypeID { get; set; }

    
        [Display(Name="Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        public GiftCertificateDetailsViewModel()
        {
            this.YearPurchased = DateTime.Now;
            this.DBudgetTypeID = 9;
            this.DFundSourceID = 5;
            this.DCurrencyID = 1;
            this.BillOfLoading = "";
        }
        //Modified:Banty 24/5/2013 from EntityCollection<> to ICollection<>
        public static List<GiftCertificateDetailsViewModel> GenerateListOfGiftCertificateDetailsViewModel(ICollection<GiftCertificateDetail> entityCollection)
        {
            var details = new List<GiftCertificateDetailsViewModel>();
            foreach (var giftDetail in entityCollection)
            {
                details.Add(GenerateGiftCertificateDetailsViewModel(giftDetail));
            }
            return details;
        }

        public static GiftCertificateDetailsViewModel GenerateGiftCertificateDetailsViewModel(GiftCertificateDetail giftCertificateDetail)
        {
            GiftCertificateDetailsViewModel model = new GiftCertificateDetailsViewModel();

            model.GiftCertificateID = giftCertificateDetail.GiftCertificateID;
            model.CommodityID = giftCertificateDetail.CommodityID;
            model.BillOfLoading = giftCertificateDetail.BillOfLoading;
            model.YearPurchased = giftCertificateDetail.YearPurchased;
            model.AccountNumber = giftCertificateDetail.AccountNumber;
            model.WeightInMT = giftCertificateDetail.WeightInMT;
            model.EstimatedPrice = giftCertificateDetail.EstimatedPrice;
            model.EstimatedTax = giftCertificateDetail.EstimatedTax;
            model.DBudgetTypeID = giftCertificateDetail.DBudgetTypeID;
            model.DFundSourceID = giftCertificateDetail.DFundSourceID;
            model.DCurrencyID = giftCertificateDetail.DCurrencyID;
            model.ExpiryDate = giftCertificateDetail.ExpiryDate;
            
            return model;
        }

    }
}
