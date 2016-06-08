using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    

    public partial class Reason
    {
        public Reason()
        {
           
        }

        public int? PartitionId { get; set; }
        public int ReasonID { get; set; }
        public string Comment { get; set; }
        
        
        public int CommentedBy { get; set; }
        public DateTime CommentedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public string Attachment { get; set; }
        public string Status { get; set; }
        public int RegionalRequestID { get; set; }
       
    }
}
