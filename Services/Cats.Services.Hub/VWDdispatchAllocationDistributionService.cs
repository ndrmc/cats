using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public class VWDdispatchAllocationDistributionService : IVWDdispatchAllocationDistributionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VWDdispatchAllocationDistributionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        
        public List<VWDdispatchAllocationDistribution> GetAllDispatchAllocationDistribution()
        {
            return _unitOfWork.VwDispatchAllocationDistribuition.GetAll();
        }
    }
}
