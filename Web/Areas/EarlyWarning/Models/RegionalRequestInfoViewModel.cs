﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.EarlyWarning.Models
{
    public class RegionalRequestInfoViewModel
    {
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public int NoOfRequests { get; set; }
        public int Remaining { get; set; }

    }
    public class ReliefRequisitionInfoViewModel
    {

        public int RequisitionID { get; set; }
        public string RequisitonNumber { get; set; }
        public string Commodity { get; set; }
        public string Zone { get; set; }
        public int Beneficiary { get; set; }
        public decimal Amount { get; set; }
        public string Status     { get; set; }
        public string Region { get; set; }
        public int Round { get; set; }
    }
    public class RequestPercentageViewModel
    {
        public decimal Approved { get; set; }
        public decimal Pending { get; set; }
        public decimal RequisitionCreated { get; set; }
    }
    public class RequisitionStatusPercentage
    {
        public decimal Pending { get; set; }
        public decimal Approved { get; set; }
        public decimal HubAssigned { get; set; }
        public decimal ProjectCodeAssigned { get; set; }
        public decimal TransportRequistionCreated { get; set; }
        public decimal TransportOrderCreated { get; set; }
        public int NoOfDraft { get; set; }
        public int NoOfApproved { get; set; }
        public int NoHubAssigned { get; set; }
        public int NoOfPcAssigned { get; set; }
        public int NoOfTransportReqCreated { get; set; }
        public int NoOfTransportOrderCreated { get; set; }
    }
    public class RegionalTotalViewModel
    {
        public string RegionName { get; set; }
        public int TotalBeneficary { get; set; }
        public decimal BeneficiaryPercentage { get; set; }
    }
    public class GiftCertificateViewModel
    {
        public int GiftCertificateID { get; set; }
        public string SINumber { get; set; }
        public string DonorName { get; set; }
        public string DclarationNumber { get; set; }
        public string Commodity { get; set; }
        public string CommodityType { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal TotalEstimatedTax { get; set; }
        public decimal Wieght { get; set; }
        public string GiftDate { get; set; }
        public string Status { get; set; }
    }
    public class HrdAndRequestViewModel
    {
        public int TotalHrdBeneficaryNumber { get; set; }
        public int TotalRequest { get; set; }
        public int TotalRequisitionNumber    { get; set; }
        public decimal HrdTotalCommodity { get; set; }
    }
    public class HrdTillDistributionViewModel
    {
        public string Region { get; set; }
        public decimal DispatchedAmount { get; set; }
        public decimal DeliveredAmount{ get; set; }
        public decimal DistributedAmount { get; set; }
    }

}