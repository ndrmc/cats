using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models
{
    public class BusinessProcess
    {
        
        public int BusinessProcessID { get; set; }
        //Fields

        [Display(Name = "Document")]
        public int DocumentID { get; set; }

        [Display(Name = "DocumentType")]
        public string DocumentType { get; set; }


        //Relationships

        [Display(Name = "Process Template")]
        public virtual ProcessTemplate ProcessType { get; set; }

        [Display(Name = "Process Template ID")]
        public int ProcessTypeID { get; set; }

        [Display(Name = "CurrentState")]
        public virtual BusinessProcessState CurrentState { get; set; }

        [Display(Name = "CurrentState ID")]
        public int CurrentStateID { get; set; }

        public virtual ICollection<RegionalPSNPPlan> RegionalPSNPPlans { get; set; }

        public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }

        public virtual ICollection<BidWinner> BidWinners { get; set; }
        public virtual ICollection<TransporterCheque> TransporterCheques { get; set; }

        public virtual ICollection<BusinessProcessState> BusinessProcessStates { get; set; }
        public virtual ICollection<TransporterPaymentRequest> TransporterPaymentRequests { get; set; }
        public virtual ICollection<NeedAssessment> NeedAssessments { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<RegionalRequest> RegionalRequests { get; set; }
        public virtual ICollection<ReliefRequisition> ReliefRequisitions { get; set; }
        public virtual ICollection<TransportRequisition> TransportRequisitions { get; set; }
        public virtual ICollection<HRD> Hrds { get; set; } 

 
        
        public virtual ICollection<DonationPlanHeader> DonationPlanHeaders { get; set; }
       
       
       
        public virtual ICollection<LocalPurchase> LocalPurchases { get; set; } 
        public ICollection<TransportOrder> TransportOrders { get; set; }
        public virtual ICollection<LoanReciptPlan> LoanReciptPlans { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<GiftCertificate> GiftCertificates { get; set; }
        public virtual ICollection<DeliveryReconcile> DeliveryReconciles { get; set; }
        public virtual ICollection<Dispatch> Dispatches { get; set; }
        public virtual ICollection<DispatchAllocation> DispatchAllocations { get; set; }
        public virtual ICollection<TransportBidQuotationHeader> TransportBidQuotationHeaders { get; set; }
        //public virtual ICollection<TransportBidQuotation> TransportBidQuotations { get; set; }
        public virtual ICollection<Transporter> Transporters { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<NeedAssessmentDetail> NeedAssessmentDetails { get; set; } 

    }

    public class BusinessProcessClean
    {

        public int BusinessProcessID { get; set; }
        //Fields

        [Display(Name = "Document")]
        public int DocumentID { get; set; }

        [Display(Name = "DocumentType")]
        public string DocumentType { get; set; }


        //Relationships

        [Display(Name = "Process Template")]
        public virtual ProcessTemplate ProcessType { get; set; }

        [Display(Name = "Process Template ID")]
        public int ProcessTypeID { get; set; }

        [Display(Name = "CurrentState")]
        public virtual BusinessProcessState CurrentState { get; set; }

        [Display(Name = "CurrentState ID")]
        public int CurrentStateID { get; set; }

       
    }

    public class BusinessProcessPOCO
    {
        public int BusinessProcessID { get; set; }
        //Fields

        [Display(Name = "Document")]
        public int DocumentID { get; set; }

        [Display(Name = "DocumentType")]
        public string DocumentType { get; set; }


        //Relationships

        [Display(Name = "Process Template ID")]
        public int ProcessTypeID { get; set; }

        [Display(Name = "CurrentState ID")]
        public int CurrentStateID { get; set; }

        [Display(Name = "CurrentStat")]
        public string CurrentStateName { get; set; }
    }
}