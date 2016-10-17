using Cats.Models;
using System;
using Cats.Models.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cats.Areas.EarlyWarning.Models
{
    [Serializable]
    public class RegionalRequestViewModel
    {
        [ScaffoldColumn(false)]
        public int RegionalRequestID { get; set; }
        // [UIHint("AdminUnitEditor")]
        public int RegionID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Region { get; set; }
        // [UIHint("ProgramEditor")]
        public int ProgramId { get; set; }

        public int RationID { get; set; }
        public string Ration { get; set; }
        public string Program { get; set; }
        public int Month { get; set; }
        public DateTime RequistionDate { get; set; }
        // [ScaffoldColumn(false)]
        public string RequestDate { get; set; }
        public int Year { get; set; }
        public int? PlanId { get; set; }

        // [ScaffoldColumn(false)]
        public int StatusID { get; set; }
        // public BusinessProcess BusinessProcess { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string MonthName { get; set; }
        public int? Round { get; set; }
        public int BusinessProcessID { get; set; }

        public int Beneficiary { get; set; }
        public int NumberOfFDPS { get; set; }

        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }
        public List<FlowTemplateViewModel> CurrentStateNo { get; set; }
        public int StateId { get; set; }
        public bool IsApprovable { get; set; }
    }
}