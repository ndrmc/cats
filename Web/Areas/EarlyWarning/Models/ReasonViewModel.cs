using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.EarlyWarning.Models
{
    public class ReasonViewModel
    {
        public int RegionalRequestID { get; set; }
        public string Comment { get; set; }
        
        public string Attachment { get; set; }
    }
}