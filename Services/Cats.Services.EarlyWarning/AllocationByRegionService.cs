using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public class AllocationByRegionService:IAllocationByRegionService
    {

         private readonly IUnitOfWork _unitOfWork;

        public AllocationByRegionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public List<AllocationByRegion> GetAllAllocations()
         {
            return _unitOfWork.AllocationByRegionRepository.GetAll();
        }
        public AllocationByRegion FindById(int id)
        {
            return _unitOfWork.AllocationByRegionRepository.FindById(id);
        }
        public List<AllocationByRegion> FindBy(Expression<Func<AllocationByRegion, bool>> predicate)
        {
            return _unitOfWork.AllocationByRegionRepository.FindBy(predicate);
        }
    }
}
