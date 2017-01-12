using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class LossReason
    {
        //LossReasonID, LossReasonEg, LossReasonAM, LossReasonCodeEg, LossReasonCodeAm
        public int LossReasonId { get; set; }
        public string LossReasonEg { get; set; }
        public string LossReasonAm { get; set; }
        public string LossReasonCodeEg { get; set; }
        public string LossReasonCodeAm { get; set; }
    }
}