﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Areas.Finance.Models
{
    public class TransporterChequeViewModel
    {
        public int TransporterChequeId { get; set; }
        public string PaymentRequestsList { get; set; }
        [Required]
        public string CheckNo { get; set; }
        [Required]
        public string PaymentVoucherNo { get; set; }
        [Required]
        public string BankName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public int TransporterId { get; set; }
        public string Transporter { get; set; }
        public int PreparedByID { get; set; }
        public string PreparedBy { get; set; }
        public int? AppovedByID { get; set; }
        public string AppovedBy { get; set; }
        public int Status { get; set; }
        public DateTime? AppovedDate { get; set; }
        public string AppovedDateString { get; set; }
        public List<TransporterChequeDetail> TransporterChequeDetails { get; set; }
    }



}