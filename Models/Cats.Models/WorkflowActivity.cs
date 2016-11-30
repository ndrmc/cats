using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
  public  class WorkflowActivity : BusinessProcessState
    {
        public Object TargetObject { get; set; }

        public String TargetObjectReferenceId { get; set; }

        public Type TargetObjectType { get; set; }

        public String TargetObjectJsonData { get; set; }

        public String TargetObjectLink { get; set; }

        public Dictionary<String,String> PersonToWorkflow { get; set; }


    }
}
