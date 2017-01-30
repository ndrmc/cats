using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class ReliefRequisition
    {
        public int RequisitionID { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public string CommodityName { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public string ZoneName { get; set; }
        public Nullable<int> Round { get; set; }
        public int Month { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string StatusName { get; set; } 
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public int? RationID { get; set; }
        public Nullable<int> RegionalRequestID { get; set; }
        public ICollection<Cats.Rest.Models.RationDetail> RationDetail { get; set; }
        public ICollection<Cats.Rest.Models.ReliefRequisitionDetail> ReliefRequisitionDetail { get; set; }

        public ReliefRequisition(int requisitionId, int? commodityId, string commodityName, int? regionId,
                                 string regionName, int? zoneId, string zoneName, int? round, int month,
                                 string requisitionNo, int? requestedBy,
                                 string requestedByName, DateTime? requestedDate, int? approvedBy, string approvedByName,
                                 DateTime? approvedDate,
                                 int? status, string statusName, int programId, string programName, int? rationId,
                                 int? regionalRequestId,ICollection<RationDetail> rationDetail,ICollection<Cats.Rest.Models.ReliefRequisitionDetail> reliefRequisitionDetail)
        {
            RequisitionID = requisitionId;
            CommodityID = commodityId;
            CommodityName = commodityName;
            RegionID = regionId;
            RegionName = regionName;
            ZoneID = zoneId;
            ZoneName = zoneName;
            Round = round;
            Month = month;
            RequisitionNo = requisitionNo;
            RequestedBy = requestedBy;
            RequestedByName = requestedByName;
            RequestedDate = requestedDate;
            ApprovedBy = approvedBy;
            ApprovedByName = approvedByName;
            ApprovedDate = approvedDate;
            Status = status;
            StatusName = statusName;
            ProgramID = programId;
            ProgramName = programName;
            RationID = rationId;
            RegionalRequestID = regionalRequestId;
            RationDetail = rationDetail;
            ReliefRequisitionDetail = reliefRequisitionDetail;
        }
    }

    public class RequisitionNo
    {
        public string Requisition_No { get; set; }
        public int  RequisitionId   { get; set; }

        public RequisitionNo(string requisitionNo, int requisitionId)
        {
            Requisition_No = requisitionNo;
            RequisitionId = requisitionId;
        }
    }
    ///
    public class Allocation
    {
     

        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public string Status { get; set; }
        public string Percent { get; set; }
        
        public List<CommodityAllocation> Commodities { get; set; }

        public Allocation(int? regionId, string regionName, List<CommodityAllocation> commodities)
        {
            RegionId = regionId;
            RegionName = regionName;
           Commodities = commodities;
           
        }

       
    }
    public class CommodityAllocation
    {
     

        public CommodityAllocation(int regionId, decimal allocatedAmount, string name, int commodityId)
        {
            RegionId = regionId;
            AllocatedAmount = allocatedAmount;
            Name = name;
            CommodityId = commodityId;
        }

        public int CommodityId { get; set; }
        public string Name { get; set; }
        public decimal AllocatedAmount { get; set; }
        public int RegionId { get; set; }
    }
} 