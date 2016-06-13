using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.ViewModels
{
    public class DashboardDispAlloRequisition
    {
        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public string HrdName { get; set; }

        public int NumberOfEstimatedRequisitions { get; set; }

        public int NumberOfApprovedRequisitions { get; set; }

        public int NumberOfHubAllocatedRequisitions { get; set; }

        public int NumberOfSipcAlloRequisitions { get; set; }

        public int NumberOfCommitedRequisitions { get; set; }


    }
}
