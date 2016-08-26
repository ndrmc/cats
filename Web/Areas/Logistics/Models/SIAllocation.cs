﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Services.Common;
namespace Cats.Areas.Logistics.Models
{
    public class RequestAllocationViewModel
    {
        public int RequisitionId{ get; set; }
        public int CommodityId { get; set; }
        public string Commodity { get; set; }
        public decimal Amount { get; set; }
        public decimal AllocatedAmount { get; set; }
        public string RegionName { get; set; }
        public string ZoneName { get; set; }
        public List<SIAllocation> SIAllocations {get;set;}
        public FreeSIPC FreeSIPCCodes { get; set; }
        public int HubAllocationID { get; set; }
        public string HubName { get; set; }
        public List<FDPRequestViewModel> FDPRequests { get; set; }
        //public int Code { get; set; }
    }
    public class FDPRequestViewModel
    {
        public int RequisitionId { get; set; }
        public int RequestDetailId { get; set; }
        public int FDPId { get; set; }
        public string FDPName { get; set; }
        public string Name { get; set; }
        public decimal RequestedAmount { get; set; }
        public int WoredaId { get; set; }
        public string WoredaName { get; set; }
        public string Commodity { get; set; }
        public List<SIPCAllocationViewModel> Allocations { get; set; }
    }
    public class SIPCAllocationViewModel
    {
        public int SIPCAllocationID { get; set; }
        public int FDPID { get; set; }
        public int HubID { get; set; }
        public int RequisitionDetailID { get; set; }
        public int Code { get; set; }
        public decimal AllocatedAmount { get; set; }
        public string AllocationType { get; set; }
    }
    public class SIAllocation
    {
        public int ShippingInstructionId { get; set; }
        public string ShippingInstructionCode { get; set; }
        public double AllocatedAmount { get; set; }
        public int AllocationId { get; set; }
        public string AllocationType { get; set; }
        public int FDPID { get; set; }
        public int HubID { get; set; }
    }
    public class FreeSIPC
    {
        public List<LedgerService.AvailableShippingCodes> FreeSICodes { get; set; }
        public List<LedgerService.AvailableProjectCodes> FreePCCodes { get; set; }
    }
    public class AllocationAction
    {
        public string Action {get;set;}
        public int? AllocationId {get;set;}
        public int RequisitionId {get;set;}
        public int ShippingInstructionId {get;set;}
        public double AllocatedAmount { get; set; }
        public int HubAllocationID { get; set; }
        public string AllocationType { get; set; }
    }
}