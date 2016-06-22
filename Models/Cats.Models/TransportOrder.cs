﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models
{
    [MetadataType(typeof(TransportOrderMeta))]
   public class TransportOrder
    {
       public TransportOrder()
       {
           this.TransportOrderDetails=new List<TransportOrderDetail>();
           // this.BidWinners=new List<BidWinner>();
       }
       public int TransportOrderID { get; set; }
       public string  TransportOrderNo { get; set; }
       public string ContractNumber { get; set; }
       public DateTime OrderDate { get; set; }
       public DateTime RequestedDispatchDate { get; set; }
       public DateTime OrderExpiryDate { get; set; }
       public string BidDocumentNo { get; set; }
       public string PerformanceBondReceiptNo { get; set; }
       public decimal? PerformanceBondAmount { get; set; }
       public int TransporterID { get; set; }
       public string ConsignerName { get; set; }
       public string TransporterSignedName { get; set; }
       public DateTime ConsignerDate { get; set; }
       public DateTime TransporterSignedDate { get; set; }
       public int? StatusID { get; set; }
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }
       public int? PartitionId { get; set; }
       public int? TransportRequiqsitionId { get; set; }
        // public int? BidID { get; set; }
        public int BusinessProcessID { get; set; }
        public virtual Transporter Transporter { get; set; }
       public virtual ICollection<TransportOrderDetail> TransportOrderDetails { get; set; }
       public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }
       public virtual ICollection<TransporterPaymentRequest> TransporterPaymentRequests { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
        //public virtual ICollection<BidWinner> BidWinners  { get; set; }
    }
}