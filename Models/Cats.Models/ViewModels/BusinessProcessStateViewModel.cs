using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cats.Models.ViewModels
{
    public class BusinessProcessStateViewModel
    {
        [Display(Name = "StateID")]
        public int StateID { get; set; }

        [Display(Name = "State")]
        public StateTemplate BaseStateTemplate { get; set; }

        [Display(Name = "Performed By")]
        public string PerformedBy { get; set; }

        [Display(Name = "Date Performed")]
        public DateTime DatePerformed { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Attachment File")]
        public HttpPostedFileBase AttachmentFile { get; set; }
        
        [Display(Name = "Process ID")]
        public int ParentBusinessProcessID { get; set; }
    }
}
