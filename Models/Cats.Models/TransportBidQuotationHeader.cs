﻿using System;
using System.Collections.Generic;

namespace Cats.Models
{
    public partial class TransportBidQuotationHeader:IWorkflow
    {
        public TransportBidQuotationHeader()
        {
            this.TransportBidQuotations = new List<TransportBidQuotation>();
        }

        public int TransportBidQuotationHeaderID { get; set; }
        public Nullable<System.DateTime> BidQuotationDate { get; set; }
        //public float BidBondAmount { get; set; }
        public Nullable<int> TransporterId { get; set; }
        public Nullable<int> BidId { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string EnteredBy { get; set; }
        public int Status { get; set; }
        public virtual AdminUnit AdminUnit { get; set; }
        public virtual Bid Bid { get; set; }
        public int? PartitionId { get; set; }
        public virtual ICollection<TransportBidQuotation> TransportBidQuotations { get; set; }
        public virtual Transporter Transporter { get; set; }

        public int? BusinessProcessId { get; set; }

        public BusinessProcess BusinessProcess { get; set; }
    }
}