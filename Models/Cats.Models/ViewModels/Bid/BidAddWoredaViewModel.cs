using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.ViewModels.Bid
{
    public class BidAddWoredaViewModel
    {
        public int TransportBidQuotationHeaderID { get; set; }
        public int TransportBidQuotationID { get; set; }
        public int BidID { get; set; }
        public int TransporterID{ get; set; }
        public int WoredaID { get; set; }
        public int hubID { get; set; }
        public int Position { get; set; }
        public bool IsWinner { get; set; }
        public decimal Tariff { get; set; }
        public string Remark { get; set; }
        [Required]
        public int RegionID { get; set; }
        [Required]
        public int ZoneID { get; set; }
    }
}
