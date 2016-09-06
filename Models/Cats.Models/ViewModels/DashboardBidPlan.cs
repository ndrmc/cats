using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.ViewModels
{
    public class DashboardBidPlan
    {
        public int RegionId { get; set; }

        public string Region { get; set; }

        public int NumberOfWoredas { get; set; }

        public int NumberOfHubAssignedWoredas { get; set; }

    }
}
