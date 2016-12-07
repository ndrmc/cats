﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models.Hubs
{
    public class ProcessTemplate
    {
        public ProcessTemplate()
        {
            ParentProcessTemplateStateTemplates = new List<StateTemplate>();
        }
        public int ProcessTemplateID { get; set; }
        //Fields
        
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Graphic Data")]
        public string GraphicsData { get; set; }


        public virtual ICollection<StateTemplate> ParentProcessTemplateStateTemplates { get; set; }
        public virtual ICollection<FlowTemplate> ParentProcessTemplateFlowTemplates { get; set; }
        //Relationships
        public virtual ICollection<BusinessProcess> ProcessTypeBusinessProcesss { get; set; }

       /* public StateTemplate StartingState { 
            get {
                ParentProcessTemplateStateTemplates.Where(s => s.StateType == 0).Select();
                return new StateTemplate(); 
            } }*/
    }
    public class ProcessTemplatePOCO
    {
        public int ProcessTemplateID { get; set; }
        //Fields

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Graphic Data")]
        public string GraphicsData { get; set; }

        public IEnumerable<StateTemplatePOCO> StateTemplates { get; set; }
        //Relationships
        public IEnumerable<FlowTemplatePOCO> FlowTemplates { get; set; }

    }
   
}