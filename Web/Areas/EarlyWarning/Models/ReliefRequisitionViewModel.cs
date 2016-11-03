﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cats.Models;
using Cats.Models.ViewModels;

namespace Cats.Areas.EarlyWarning.Models
{
    public class ReliefRequisitionViewModel
    {
        public int RequisitionID { get; set; }
        public int BusinessProcessID { get; set; }
        public string RequestRefNo { get; set; }
        public Nullable<int> CommodityID { get; set; }
        public string Commodity { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string Region { get; set; }
        public Nullable<int> ZoneID { get; set; }
        public string Zone { get; set; }
        public Nullable<int> Round { get; set; }
        public string RequisitionNo { get; set; }
        public Nullable<int> RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        [Display(Name = "Request Date")]
        public string RequestedDateEt { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        [Display(Name = "Approved Date ")]
        public string ApprovedDateEt { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Status { get; set; }
        public Nullable<int> ProgramID { get; set; }
        public string Program { get; set; }
        public Nullable<int> RegionalRequestID { get; set; }
        public string Month { get; set; }

        public string Ration { get; set; }
        public int RationID { get; set; }
        public int RationName { get; set; }
        public string StateName { get; set; }
        public List<FlowTemplateViewModel> InitialStateFlowTemplates { get; set; }
        public bool IsDraft { get; set; }
        public bool IsSelected { get; set; }
        public string Comment { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public int ApprovedId { get; set; }
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }


        public string MonthRound
        {
            get
            {
                string mr="";
                if (ProgramID == 1)
                { mr = Round.ToString(); }
                else if (ProgramID == 2)
                {
                    mr = Month;
                }
                return mr;
            }
        }
    }
}