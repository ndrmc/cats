﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Cats.Models
{
    public partial class Hub
    {
        public Hub()
        {
            //this.DispatchAllocations = new List<DispatchAllocation>();
            //this.HubAllocations = new List<HubAllocation>();
            //this.ReceiptAllocations = new List<ReceiptAllocation>();
            //this.ReceiptAllocations1 = new List<ReceiptAllocation>();
            //this.Transactions = new List<Transaction>();
            //this.TransportOrderDetails = new List<TransportOrderDetail>();
           // this.HubOwner = new HubOwner();
            this.TransportBidQuotations = new List<TransportBidQuotation>();
            this.ReceiptPlanDetails = new List<ReceiptPlanDetail>();

            this.DonationPlanDetails = new List<DonationPlanDetail>();

            this.LocalPurchaseDetails=new List<LocalPurchaseDetail>();
            this.LoanReciptPlanDetails=new List<LoanReciptPlanDetail>();
            this.Transfers = new List<Transfer>();
            this.Transfers1 = new List<Transfer>();
           // this.LoanReciptPlans=new List<LoanReciptPlan>();

           
        }

        public int HubID { get; set; }
        public string Name { get; set; }
        public int HubOwnerID { get; set; }
        public int HubParentID { get; set; }
        public virtual HubOwner HubOwner { get; set; }
        public virtual ICollection<DispatchAllocation> DispatchAllocations { get; set; }
        public virtual ICollection<HubAllocation> HubAllocations { get; set; }
        public virtual ICollection<ReceiptAllocation> ReceiptAllocations { get; set; }
        public virtual ICollection<ReceiptAllocation> ReceiptAllocations1 { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<TransportOrderDetail> TransportOrderDetails { get; set; }
        public virtual ICollection<BidWinner> BidWinners { get; set; }
        public virtual ICollection<Dispatch> Dispatches { get; set; }
        public virtual ICollection<TransportBidPlanDetail> TransportBidPlanSources { get; set; }
        public virtual ICollection<DonationPlanDetail> DonationPlanDetails { get; set; }
        public virtual ICollection<TransportBidQuotation> TransportBidQuotations { get; set; }

        public virtual ICollection<PromisedContribution> PromisedContributions { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<OtherDispatchAllocation> OtherDispatchAllocations { get; set; }
        public virtual ICollection<WoredaHubLink> WoredaHubLinks { get; set; }
        public virtual ICollection<ReceiptPlanDetail> ReceiptPlanDetails { get; set; }
        public virtual ICollection<DeliveryReconcile> DeliveryReconciles { get; set; }
        public virtual ICollection<LocalPurchaseDetail> LocalPurchaseDetails  { get; set; }
        //public virtual ICollection<LoanReciptPlan> LoanReciptPlans { get; set; }
        //public virtual ICollection<LoanReciptPlan> LoanReciptPlans { get; set; }
        public virtual ICollection<LoanReciptPlanDetail> LoanReciptPlanDetails { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<Transfer> Transfers1 { get; set; }
        public virtual ICollection<Transfer> Transfers2 { get; set; }
        public virtual ICollection<Transfer> Transfers3 { get; set; }

       
    }
}