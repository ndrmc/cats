using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
  public  class WorkflowActivity
    {

        public int BusinessProcessStateID { get; set; }
        //Fields
        /*
        [Display(Name = "StateID")]
        public int StateID { get; set; }
        */
    
        public int StateID { get; set; }

   
        public virtual StateTemplate BaseStateTemplate { get; set; }

     
        public string PerformedBy { get; set; }

   
        public DateTime DatePerformed { get; set; }

     
        public string Comment { get; set; }
 
        public string AttachmentFile { get; set; }

        //Relationships

     
        public virtual BusinessProcess ParentBusinessProcess { get; set; }

      
        public int ParentBusinessProcessID { get; set; }

        public Object TargetObject { get; set; }

        public String TargetObjectReferenceId { get; set; }

        public Type TargetObjectType { get; set; }

        public String TargetObjectJsonData { get; set; }

        public String TargetObjectLink { get; set; }

        public Dictionary<String,String> PersonToWorkflow { get; set; }


    }
}
