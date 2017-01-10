using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class HRD
    {
        //HrdId, PlanId, Year, CreatedBy, RevisionNumber, DateCreated, 
        //    SeasonId, SeasonName, PublishedDate, RationId, 
        //    Ration, Status, TransactionGroupID, HRDDetails[]
        public int HRDID { get; set; }
        public int PlanID { get; set; }
        public int Year { get; set; }
        public Nullable<int> SeasonID { get; set; }
        public string SeasonName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public Nullable<int> CreatedBY { get; set; }
        public int RationID { get; set; }
        public int TransationGroupID { get; set; }
        public string  Status { get; set; }
       
        public int? PartitionId { get; set; }

        public Ration Ration { get; set; }
        public List<HRDDetail> HRDDetails { get; set; }
    }
    public class HRDDetail
    {
        public int HRDDetailID { get; set; }
        public int WoredaID { get; set; }
        public int NumberOfBeneficiaries { get; set; }
        public int DurationOfAssistance { get; set; }
        public int StartingMonth { get; set; }
        public string WoredaName { get; set; }
    }

    public class Ration
    {
        public int RationID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool? IsDefaultRation { get; set; }
        public string RefrenceNumber { get; set; }
    }
}