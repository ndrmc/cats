using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class RegionalRequest
    {
        //RegionalRequestId, RequestDate, ProgramId, ProgramName, 
        //    RationId, RationDetail, Month, RegionID, RegionName, RequestNumber, 
        //    Year, Remark, StatusId, StatusName, DonorID, DonorName,
        //    Round, SeasonId, SeasonName, PlanID, 
        //    RequestedBy, ApprovedBy, Contigency, RegionalRequestDetails[]
        public int RegionalRequestID { get; set; }

        public int RegionID { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int Month { get; set; } 
        public int Year { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public int RegionId
        {
            get; set;
            
        }
        public string RegionName { get; set; }
        public String ReferenceNumber { get; set; }
        public string Remark { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int RationID { get; set; }
        public Ration RationDetail { get; set; }
        public int? DonorID { get; set; }
        public string DonorName { get; set; }
        public int PlanID { get; set; }
        public int? IDPSReasonType { get; set; }
        public int? PartitionId { get; set; }
        public int? RequestedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public bool Contingency { get; set; }

        public List<RegionalRequestDetail> RegionalRequestDetails { get; set; }
    }
}