﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Hubs
{
    public class HubView
    {
        public int HubId { get; set; }
        public string Name { get; set; }
        public int HubOwnerID { get; set; }
    }

    public class ProgramView
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
    }

    public class CommodityView
    {
        public int CommodityId { get; set; }
        public string Name { get; set; }
    }
    public class HubFreeStockView
    {
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public decimal FreeStock { get; set; }
        public decimal PhysicalStock { get; set; }

        public decimal? Commited { get; set; }
   }

    public class HubFreeStockSummaryView {
        public int HubID { get; set; }
        public string HubName { get; set; }
        public decimal TotalFreestock { get; set; }
        public decimal TotalPhysicalStock { get; set; }
    }
    public class TrueAndFlaseGRNStatus
    {
        public int HubID { get; set; }
        public string HubName { get; set; }
        public int CommodityID { get; set; }
        public string CommodityName { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string SINo { get; set; }
        public decimal FalseAmount { get; set; }
        public decimal TrueAmount { get; set; }
        public decimal Balance { get; set; }
       
    }
    public class HubRecentDispachesViewModel
    {
        public string BidNumber { get; set; }
        public string GIN { get; set; }
        public int FDPID { get; set; }
        public string FDPName { get; set; }
        public string RequisitionNo { get; set; }
        public string Commodity { get; set; }
        public string Transporter { get; set; }
        public decimal DispatchedAmount { get; set; }
        public string Program { get; set; }
        public Nullable<int> BeneficiaryNumber { get; set; }
    }
    public class HubDispatchAllocationViewModel

{
    public int HubID { get; set; }
        public string HubName { get; set; }
        public decimal TotalFreestock { get; set; }
        public decimal TotalPhysicalStock { get; set; }
        public decimal DispatchedAmount { get; set; }
        public decimal Remaining { get; set; }
        //public decimal Remaining { get { return TotalPhysicalStock - DispatchedAmount; } }
}
}
