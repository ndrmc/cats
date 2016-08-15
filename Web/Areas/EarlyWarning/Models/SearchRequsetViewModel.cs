using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.EarlyWarning.Models
{
    public class SearchRequsetViewModel
    {
        public SearchRequsetViewModel()
        {
            this.DateTo = String.Format("{0:MM/dd/yyyy}", DateTime.Now); 
            //this.DateToFormatted = String.Format("{0:MM/dd/yyyy}", DateTo);
            this.DateFrom = String.Format("{0:MM/dd/yyyy}", DateTime.Now.AddMonths(-2));
            //this.DateFromFormatted = String.Format("{0:MM/dd/yyyy}", DateFrom);
        }
        public string DateFrom { get; set; }
        //public string DateFromFormatted { get; set; }
        public string DateTo { get; set; }
        //public string DateToFormatted { get; set; }
        public int? RegionID{ get; set; }
        public int? ProgramID { get; set; }
        public int? StatusID { get; set; }
    }

    public class SearchRequistionViewModel
    {
        public int? RegionID { get; set; }
        public int? ProgramID { get; set; }
        public int? StatusID { get; set; }
    }

    public class RequestStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }
}