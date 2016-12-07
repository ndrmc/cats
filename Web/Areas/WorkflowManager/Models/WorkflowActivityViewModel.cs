﻿using Cats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Areas.WorkflowManager.Models
{
    public class WorkflowActivityViewModel
    {

        public int BusinessProcessStateID { get; set; }
    
        public int StateID { get; set; }
         
        public string PerformedBy { get; set; }
 
        public DateTime DatePerformed { get; set; }

        
        public string Comment { get; set; }

       
        public string AttachmentFile { get; set; }

        
        public virtual BusinessProcess ParentBusinessProcess { get; set; }

        public int ParentBusinessProcessID { get; set; }

        public virtual ICollection<BusinessProcess> CurrentStateBusinessProcesss { get; set; }
        public Object TargetObject { get; set; }

        public String TargetObjectReferenceId { get; set; }

        public Type TargetObjectType { get; set; }

        public String TargetObjectJsonData { get; set; }

        public String TargetObjectLink { get; set; }

        public Dictionary<String, String> PersonToWorkflow { get; set; }

    }


}


namespace Cats.Areas.WorkflowManager.Models.Mapper
{
    public class WorkflowActivityMap
    {

    public WorkflowActivity WorkflowActivityMapper(WorkflowActivityViewModel from)
    {

        return null;
    }

        public WorkflowActivityViewModel WorkflowActivityMapper(WorkflowActivity from)
        {

            return null;
        }
    }
}