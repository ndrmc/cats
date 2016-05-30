using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Areas.EarlyWarning.Models
{
    public class NeedAssessmentPlanViewModel
    {
        public int NeedAssessmentID { get; set; }
        public int PlanID { get; set; }
        public int BusinessProcessID { get; set; }
        public string AssessmentName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Year { get; set; }
        public int StatuID { get; set; }
        public string Status { get; set; }

        public BusinessProcess BusinessProcess { get; set; }
    }
}