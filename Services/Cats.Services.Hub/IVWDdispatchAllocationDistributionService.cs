using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IVWDdispatchAllocationDistributionService
    {
        List<VWDdispatchAllocationDistribution> GetAllDispatchAllocationDistribution();
    }
}
