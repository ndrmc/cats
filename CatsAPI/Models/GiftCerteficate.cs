using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class GiftCerteficate
    {
        //GiftCertificateId, GiftDate, DonorID, ReferenceNo, Vessel, 
        //    ETA, IsPrinted, ProgramId, ProgramName, DModeOfTransport, PortName, StatusID, 
        //    DeclarationNumber, ShippingInstructionId, TransactionGroupId, GiftCertificateDetails[]
        public int GiftCertificateID { get; set; }
        public System.DateTime GiftDate { get; set; }
        public int DonorID { get; set; }
        //public string SINumber { get; set; }
        public int ShippingInstructionID { get; set; }
        public string ReferenceNo { get; set; }
        public string Vessel { get; set; }
        public System.DateTime ETA { get; set; }
        public bool IsPrinted { get; set; }
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public int DModeOfTransport { get; set; }
        public string PortName { get; set; }
        public int StatusID { get; set; }
        public int? PartitionId { get; set; }

        public string DeclarationNumber { get; set; }
        public Nullable<Guid> TransactionGroupID { get; set; }
        public List<GiftCertificateDetail> GiftCerteficateDetails { get; set; }
      

    }
}