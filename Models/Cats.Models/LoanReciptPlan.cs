﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cats.Models
{
   public class LoanReciptPlan
    {
       public LoanReciptPlan()
       {
           this.LoanReciptPlanDetails=new List<LoanReciptPlanDetail>();
       }
       
       public int LoanReciptPlanID { get; set; }
       public int ShippingInstructionID { get; set; }
      // public int HubID { get; set; }
       public string LoanSource { get; set; }
       public int ProgramID { get; set; }
       public string ProjectCode { get; set; }
       public DateTime CreatedDate { get; set; }
       public string ReferenceNumber { get; set; }
       public int CommoditySourceID { get; set; }
       public int CommodityID { get; set; }
       public decimal Quantity { get; set; }
       public int StatusID { get; set; }
       public bool IsFalseGRN { get; set; }
       public int BusinessProcessID { get; set; }
        // public virtual Hub Hub  { get; set; }
        //public virtual Hub Hub { get; set; }
        public virtual Program Program { get; set; }
       public virtual CommoditySource CommoditySource { get; set; }
       public virtual Commodity Commodity { get; set; }
       public virtual ShippingInstruction ShippingInstruction { get; set; }
        [JsonIgnore]
        public virtual BusinessProcess BusinessProcess { get; set; }
        public ICollection<LoanReciptPlanDetail> LoanReciptPlanDetails { get; set; }
    }
}
