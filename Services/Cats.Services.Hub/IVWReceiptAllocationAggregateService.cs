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
    public interface IVWReceiptAllocationAggregateService
    {
        bool AddVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate);
        bool DeleteVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate);
        bool DeleteById(int id);
        bool EditVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate);
        VWReceiptAllocationAggregate FindById(int id);
        List<VWReceiptAllocationAggregate> GetAllVWReceiptAllocationAggregate();
        List<VWReceiptAllocationAggregate> FindBy(Expression<Func<VWReceiptAllocationAggregate, bool>> predicate);
        IEnumerable<VWReceiptAllocationAggregate> Get(
                   Expression<Func<VWReceiptAllocationAggregate, bool>> filter = null,
                   Func<IQueryable<VWReceiptAllocationAggregate>, IOrderedQueryable<VWReceiptAllocationAggregate>> orderBy = null,
                   string includeProperties = "");

        List<VWReceiptAllocationAggregate> GetAllRAAggr(int hubId, int commoditySoureType, bool? closedToo,
            string weightMeasurmentCode, int? CommodityType, bool? receivable, string grn);
    }
}
