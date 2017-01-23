﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    public partial class DeliveryReconcile:IWorkflow
    {
        public int DeliveryReconcileID { get; set; }
        public string GRN { get; set; }
        public int FDPID { get; set; }
        public System.Guid DispatchID { get; set; }
        public string WayBillNo { get; set; }
        public string RequsitionNo { get; set; }
        public int HubID { get; set; }
        public string GIN { get; set; }
        public decimal ReceivedAmount { get; set; }
        public System.DateTime ReceivedDate { get; set; }
        public Nullable<decimal> LossAmount { get; set; }
        public int? LossReason { get; set; }
        public Nullable<System.Guid> TransactionGroupID { get; set; }
        public virtual Dispatch Dispatch { get; set; }
        public virtual FDP FDP { get; set; }
        public virtual Hub Hub { get; set; }
        public virtual TransactionGroup TransactionGroup { get; set; }
        //public int? BusinessProcessID { get; set; }
        public int BusinessProcessId { get; set; }
        public virtual BusinessProcess BusinessProcess { get; set; }
    }
}
