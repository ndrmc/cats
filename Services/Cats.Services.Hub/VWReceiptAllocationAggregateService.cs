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
    public class VWReceiptAllocationAggregateService : IVWReceiptAllocationAggregateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VWReceiptAllocationAggregateService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool AddVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate)
        {
            _unitOfWork.VWReceiptAllocationAggregateRepository.Add(vwReceiptAllocationAggregate);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate)
        {
            if (vwReceiptAllocationAggregate == null) return false;
            _unitOfWork.VWReceiptAllocationAggregateRepository.Delete(vwReceiptAllocationAggregate);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.VWReceiptAllocationAggregateRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.VWReceiptAllocationAggregateRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditVWReceiptAllocationAggregate(VWReceiptAllocationAggregate vwReceiptAllocationAggregate)
        {
            _unitOfWork.VWReceiptAllocationAggregateRepository.Edit(vwReceiptAllocationAggregate);
            _unitOfWork.Save();
            return true;
        }

        public VWReceiptAllocationAggregate FindById(int id)
        {
            return _unitOfWork.VWReceiptAllocationAggregateRepository.FindById(id);
        }

        public List<VWReceiptAllocationAggregate> GetAllVWReceiptAllocationAggregate()
        {
            return _unitOfWork.VWReceiptAllocationAggregateRepository.GetAll();
        }

        public List<VWReceiptAllocationAggregate> FindBy(Expression<Func<VWReceiptAllocationAggregate, bool>> predicate)
        {
            return _unitOfWork.VWReceiptAllocationAggregateRepository.FindBy(predicate);
        }

        public IEnumerable<VWReceiptAllocationAggregate> Get(Expression<Func<VWReceiptAllocationAggregate, bool>> filter = null, Func<IQueryable<VWReceiptAllocationAggregate>, IOrderedQueryable<VWReceiptAllocationAggregate>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.VWReceiptAllocationAggregateRepository.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<VWReceiptAllocationAggregate> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _unitOfWork.Database.SqlQuery<VWReceiptAllocationAggregate>(query, parameters);
        }

        public List<VWReceiptAllocationAggregate> GetAllRAAggr(int hubId, int commoditySoureType, bool? closedToo, string weightMeasurmentCode, int? CommodityType, bool? receivable, string grns)
        {
            var hubID = new SqlParameter("hubID", SqlDbType.Int) { Value = hubId };
            var isClosed = new SqlParameter("isClosed", SqlDbType.Bit) { Value = closedToo };
            var isFalseGRN = new SqlParameter("isFalseGRN", SqlDbType.Bit) { Value = receivable };
            var commodityTypeID = new SqlParameter("commodityTypeID", SqlDbType.Int) { Value = CommodityType };
            var commoditySourceID = new SqlParameter("commoditySourceID", SqlDbType.Int) { Value = commoditySoureType };
            var grn = new SqlParameter("grn", SqlDbType.VarChar) { Value = grns };
            
            var result = this.ExecWithStoreProcedure("EXEC [dbo].[SPReceiveAllocationAggr] @hubID, @isClosed, @isFalseGRN, @commodityTypeID, @commoditySourceID, @grn",
                hubID, isClosed, isFalseGRN, commodityTypeID, commoditySourceID, grn);
            return result.ToList();
        } 
    }
}
