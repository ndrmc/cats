using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    public class HubStoreViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Hub/store")]
        public string Name { get; set; }


    }
}
