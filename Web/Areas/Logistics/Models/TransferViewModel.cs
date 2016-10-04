﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Areas.Logistics.Models
{
    public class TransferViewModel
    {
        public int TransferID { get; set; }
        public string SiNumber { get; set; }
        public int SourceHubID { get; set; }
        public string SourceHubName { get; set; }
        public int ProgramID { get; set; }
        public string Program { get; set; }
        public int CommoditySourceID { get; set; }
        public string CommoditySource { get; set; }
        public int CommodityTypeID { get; set; }
        public string CommodityType { get; set; }
        public int ParentCommodityID { get; set; }
        public string ParentCommodity { get; set; }
        public int CommodityID { get; set; }
        public string Commodity { get; set; }
        public int DestinationHubID { get; set; }
        public string DestinationHubName { get; set; }
        public string ProjectCode { get; set; }
        public decimal Quantity { get; set; }
        public string CreatedDate { get; set; }
        public string ReferenceNumber { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string Remark { get; set; }

        public int SourceSwap { get; set; }
        public string SourceSwapName { get; set; }
        public int DestinationSwap { get; set; }
        public string DestinationSwapName { get; set; }
        public string Status { get; set; }
        public int ApprovedId { get; set; }
        public int RejectedId { get; set; }
        public int BusinessProcessID { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
    }
}