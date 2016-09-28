﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Areas.EarlyWarning.Models;
using Cats.Helpers;
using Cats.Models;
using GiftCertificateViewModel = Cats.Areas.GiftCertificate.Models.GiftCertificateViewModel;

namespace Cats.ViewModelBinder
{
    public class GiftCertificateViewModelBinder
    {
        public static List<GiftCertificateViewModel> BindListGiftCertificateViewModel(List<GiftCertificate> giftCertificates, string userPrefrence, bool bindWithDetail = false)
        {
            var giftCertificatesViewModel = new List<GiftCertificateViewModel>();
            foreach (var giftCertificate in giftCertificates)
            {
                giftCertificatesViewModel.Add(BindGiftCertificateViewModel(giftCertificate, userPrefrence, bindWithDetail));
            }
            return giftCertificatesViewModel.ToList();
        }
        public static GiftCertificateViewModel BindGiftCertificateViewModel(GiftCertificate giftCertificateModel, string userPrefrence, bool bindWithDetail = false)
        {
            var giftCertificateViewModel = new GiftCertificateViewModel();

            giftCertificateViewModel.GiftCertificateID = giftCertificateModel.GiftCertificateID;
            giftCertificateViewModel.GiftDate = giftCertificateModel.GiftDate;
            giftCertificateViewModel.DonorID = giftCertificateModel.DonorID;
            giftCertificateViewModel.SINumber = giftCertificateModel.ShippingInstruction.Value;
            giftCertificateViewModel.SiId = giftCertificateModel.ShippingInstructionID;
            giftCertificateViewModel.ReferenceNo = giftCertificateModel.ReferenceNo;
            giftCertificateViewModel.Vessel = giftCertificateModel.Vessel;
            giftCertificateViewModel.ETA = giftCertificateModel.ETA;
            giftCertificateViewModel.ProgramID = giftCertificateModel.ProgramID;
            giftCertificateViewModel.PortName = giftCertificateModel.PortName;
            giftCertificateViewModel.DModeOfTransport = giftCertificateModel.DModeOfTransport;
            giftCertificateViewModel.Donor = giftCertificateModel.Donor.Name;
            giftCertificateViewModel.StatusID = giftCertificateModel.StatusID;
            giftCertificateViewModel.Status = giftCertificateModel.BusinessProcess.CurrentState.BaseStateTemplate.Name;
            giftCertificateViewModel.DeclarationNumber = giftCertificateModel.DeclarationNumber;
            giftCertificateViewModel.GiftDatePref=giftCertificateModel.GiftDate.ToCTSPreferedDateFormat(userPrefrence);
            giftCertificateViewModel.IsPrinted = giftCertificateModel.IsPrinted;
            giftCertificateViewModel.AllowReject = giftCertificateModel.Donor.DonationPlanHeaders.Count > 0; // default
            var giftCertificateDetail = giftCertificateModel.GiftCertificateDetails.FirstOrDefault();
            if (giftCertificateDetail != null)
                giftCertificateViewModel.CommodityTypeID = giftCertificateDetail.Commodity.CommodityTypeID;
            else
                giftCertificateViewModel.CommodityTypeID = 1;//by default 'food' 
            if (bindWithDetail)
            {
                giftCertificateViewModel.GiftCertificateDetails =
                     BindListOfGiftCertificateDetailsViewModel(
                         giftCertificateModel.GiftCertificateDetails.ToList(),userPrefrence);
            }


            return giftCertificateViewModel;
        }

        public static GiftCertificate BindGiftCertificate(GiftCertificateViewModel giftCertificateViewModel)
        {

            return BindGiftCertificate(new GiftCertificate(), giftCertificateViewModel);

        }
        public static GiftCertificate BindGiftCertificate(GiftCertificate giftCertificate, GiftCertificateViewModel giftCertificateViewModel)
        {

            giftCertificate.GiftCertificateID = giftCertificateViewModel.GiftCertificateID;
            giftCertificate.GiftDate = giftCertificateViewModel.GiftDate;
           // giftCertificate.SINumber = giftCertificateViewModel.SINumber;
            giftCertificate.DonorID = giftCertificateViewModel.DonorID;
            giftCertificate.ReferenceNo = giftCertificateViewModel.ReferenceNo;
            giftCertificate.Vessel = giftCertificateViewModel.Vessel;
            giftCertificate.ETA = giftCertificateViewModel.ETA;
            giftCertificate.IsPrinted = giftCertificateViewModel.IsPrinted;
            giftCertificate.DModeOfTransport = giftCertificateViewModel.DModeOfTransport;
            giftCertificate.ProgramID = giftCertificateViewModel.ProgramID;
            giftCertificate.PortName = giftCertificateViewModel.PortName;
            giftCertificate.DeclarationNumber = giftCertificateViewModel.DeclarationNumber;

            return giftCertificate;
        }

        public static List<GiftCertificateDetail> BindListGiftCertificateDetail(List<GiftCertificateDetailsViewModel> giftCertificateDetailsViewModel)
        {
            return giftCertificateDetailsViewModel.Select(BindGiftCertificateDetail).ToList();
        }

        public static GiftCertificateDetail BindGiftCertificateDetail(GiftCertificateDetailsViewModel giftCertificateDetailsViewModel)
        {

            return BindGiftCertificateDetail(new GiftCertificateDetail(), giftCertificateDetailsViewModel);
        }
        public static GiftCertificateDetail BindGiftCertificateDetail(GiftCertificateDetail giftCertificateDetail, GiftCertificateDetailsViewModel giftCertificateDetailsViewModel)
        {


            giftCertificateDetail.CommodityID = giftCertificateDetailsViewModel.CommodityID;
            giftCertificateDetail.BillOfLoading = giftCertificateDetailsViewModel.BillOfLoading;
            giftCertificateDetail.YearPurchased = giftCertificateDetailsViewModel.YearPurchased;
            giftCertificateDetail.AccountNumber = giftCertificateDetailsViewModel.AccountNumber;
            giftCertificateDetail.WeightInMT = giftCertificateDetailsViewModel.WeightInMT;
            giftCertificateDetail.EstimatedPrice = giftCertificateDetailsViewModel.EstimatedPrice;
            giftCertificateDetail.EstimatedTax = giftCertificateDetailsViewModel.EstimatedTax;
            giftCertificateDetail.DCurrencyID = giftCertificateDetailsViewModel.DCurrencyID;
            giftCertificateDetail.DFundSourceID = giftCertificateDetailsViewModel.DFundSourceID;
            giftCertificateDetail.DBudgetTypeID = giftCertificateDetailsViewModel.DBudgetTypeID;
            giftCertificateDetail.GiftCertificateDetailID = giftCertificateDetailsViewModel.GiftCertificateDetailID;
            giftCertificateDetail.GiftCertificateDetailID = giftCertificateDetailsViewModel.GiftCertificateDetailID;
            giftCertificateDetail.GiftCertificateID = giftCertificateDetailsViewModel.GiftCertificateID;
            giftCertificateDetail.TransactionGroupID = giftCertificateDetailsViewModel.TransactionGroupID;
            giftCertificateDetail.ExpiryDate = giftCertificateDetailsViewModel.ExpiryDate;
            return giftCertificateDetail;
        }
        
        public static GiftCertificateDetailsViewModel BindGiftCertificateDetailsViewModel(GiftCertificateDetail giftCertificateDetail, string pref)
        {
            var model = new GiftCertificateDetailsViewModel();

            model.GiftCertificateID = giftCertificateDetail.GiftCertificateID;
            model.GiftCertificateDetailID = giftCertificateDetail.GiftCertificateDetailID;
            model.CommodityID = giftCertificateDetail.CommodityID;
            model.BillOfLoading = giftCertificateDetail.BillOfLoading;
            model.YearPurchased = giftCertificateDetail.YearPurchased;//.ToCTSPreferedDateFormat(pref);
            model.AccountNumber = giftCertificateDetail.AccountNumber;
            model.WeightInMT = giftCertificateDetail.WeightInMT;
            model.EstimatedPrice = giftCertificateDetail.EstimatedPrice;
            model.EstimatedTax = giftCertificateDetail.EstimatedTax;
            model.DBudgetTypeID = giftCertificateDetail.DBudgetTypeID;
            model.DFundSourceID = giftCertificateDetail.DFundSourceID;
            model.DCurrencyID = giftCertificateDetail.DCurrencyID;
            model.ExpiryDate = giftCertificateDetail.ExpiryDate;
            model.FundSource = giftCertificateDetail.Detail.Name;
            model.CommodityName = giftCertificateDetail.Commodity.Name;
            model.ExpiryDate = giftCertificateDetail.ExpiryDate;
            model.YearPurchasedPrefered = giftCertificateDetail.YearPurchased;//.ToCTSPreferedDateFormat(pref);

            return model;
        }

        public static List<GiftCertificateDetailsViewModel> BindListOfGiftCertificateDetailsViewModel(List<GiftCertificateDetail> giftCertificateDetails,string pref)
        {
            var s = new List<GiftCertificateDetailsViewModel>();
            foreach (var giftCertificateDetail in giftCertificateDetails)
            {
                s.Add(BindGiftCertificateDetailsViewModel(giftCertificateDetail,pref));
            }
            //return giftCertificateDetails.Select(BindGiftCertificateDetailsViewModel).ToList();
            return s;
        }

        
    }
}