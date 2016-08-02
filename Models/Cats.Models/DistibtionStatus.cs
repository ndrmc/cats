using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
   public class DistibtionStatus
    {
       
       public string PlanName { get; set; }
       public int WoredaID { get; set; }
       
       //public int FDPID { get; set; }
       public int FdpCount { get; set; }
       public string RegionName { get; set; }
       public string WoredaName { get; set; }
       public int Status { get; set; }
       public int RegionID { get; set; }
       public DateTime DistributionDate { get; set; }

    }
}
