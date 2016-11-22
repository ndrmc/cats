using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models.Hubs
{
    public partial class GiftCertificate
    {
        public GiftCertificate()
        {
            this.GiftCertificateDetails = new List<GiftCertificateDetail>();
        }
        [Key]
        public int GiftCertificateID { get; set; }
        public System.DateTime GiftDate { get; set; }
        public int DonorID { get; set; }
        public int ShippingInstructionID { get; set; }
        public string ReferenceNo { get; set; }
        public string Vessel { get; set; }
        public System.DateTime ETA { get; set; }
        public bool IsPrinted { get; set; }
        public int ProgramID { get; set; }
        public int DModeOfTransport { get; set; }
        public string PortName { get; set; }
        public virtual Detail Detail { get; set; }
        public virtual Donor Donor { get; set; }
        public virtual Program Program { get; set; }
        public virtual ShippingInstruction ShippingInstruction { get; set; }
        public virtual ICollection<GiftCertificateDetail> GiftCertificateDetails { get; set; }
    }
}
