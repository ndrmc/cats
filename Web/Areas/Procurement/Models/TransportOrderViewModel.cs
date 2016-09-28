﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cats.Models.ViewModels;

namespace Cats.Areas.Procurement.Models
{
    public class TransportOrderViewModel
    {
        public int TransportOrderID { get; set; }
        public string TransportOrderNo { get; set; }
        public string ContractNumber { get; set; }
        public DateTime OrderDate { get; set; }
        [Display(Name="Order Date")]
        public string OrderDateET { get; set; }

        public DateTime RequestedDispatchDate { get; set; }
        [Display(Name = "Requested Dispatch Date")]
        public string RequestedDispatchDateET { get; set; }
        public DateTime OrderExpiryDate { get; set; }
         [Display(Name = "Order Expiry Date")]
        public string OrderExpiryDateET { get; set; }
        public string BidDocumentNo { get; set; }
        public string PerformanceBondReceiptNo { get; set; }
        public int TransporterID { get; set; }
        public string Transporter { get; set; }
        public string ConsignerName { get; set; }
        public string TransporterSignedName { get; set; }
        public DateTime ConsignerDate { get; set; }
         [Display(Name = "Consigner Signed Date")]
        public string ConsignerDateET { get; set; }
        public DateTime TransporterSignedDate { get; set; }
        [Display(Name = "Transporter Signed Date")]
        public string TransporterSignedDateET { get; set; }
        [Display(Name ="Status")]
        public int? StatusID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string Status { get; set; }
        public List<FlowTemplateViewModel> InitialStateFlowTemplates { get; set; }
        public int BusinessProcessID { get; set; }
        public int RejectStateID { get; set; }
        public int SignStateID { get; set; }
        public int ApproveStateID { get; set; }
    }
    public class TransportOrderApprovalViewModel
    {
        public int TransportOrderID { get; set; }
        public string TransportOrderNo { get; set; }
        public string ContractNumber { get; set; }
        public string Transporter { get; set; }
        public bool Checked { get; set; }
    }
    public class RevertRequsitionsViewModel
    {
        public int RequsitionID { get; set; }
        public string RequsitionNumber { get; set; }
        public string Zone { get; set; }
        public bool Checked { get; set; }
    }
}