using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.ViewModels
{
    public class FlowTemplateViewModel
    {
        public int FlowTemplateID { get; set; }
        //Fields

        [Display(Name = "Name")]
        public string Name { get; set; }


        //Relationships
        [Display(Name = "Process")]
        public string ParentProcessTemplate { get; set; }

        [Display(Name = "Process ID")]
        public int ParentProcessTemplateID { get; set; }

        [Display(Name = "Initial State")]
        public string InitialState { get; set; }

        [Display(Name = "Initial State ID")]
        public int InitialStateID { get; set; }

        [Display(Name = "Final State")]
        public string FinalState { get; set; }

        [Display(Name = "Final State ID")]
        public int FinalStateID { get; set; }
    }
}
