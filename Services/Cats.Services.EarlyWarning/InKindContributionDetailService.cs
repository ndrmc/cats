using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public class InKindContributionDetailService:IInkindContributionDetailService
   {
       private UnitOfWork _unitOfWork;
       public InKindContributionDetailService(UnitOfWork unitOfWork)
       {
           _unitOfWork = unitOfWork;
       } 
       public bool AddInKindContributionDetail(InKindContributionDetail inKindContributionDetail)
       {
           _unitOfWork.InKindContributionDetailRepository.Add(inKindContributionDetail);
           _unitOfWork.Save();
           return true;
       }

        public bool DeleteInKindContributionDetail(InKindContributionDetail inKindContributionDetail)
        {
            if (inKindContributionDetail == null) return false;
            _unitOfWork.InKindContributionDetailRepository.Delete(inKindContributionDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.InKindContributionDetailRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.InKindContributionDetailRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditInKindContributionDetail(InKindContributionDetail inKindContributionDetail)
        {
            _unitOfWork.InKindContributionDetailRepository.Edit(inKindContributionDetail);
            _unitOfWork.Save();
            return true;
        }

        public InKindContributionDetail FindById(int id)
        {
            return _unitOfWork.InKindContributionDetailRepository.FindById(id);
        }

        public List<InKindContributionDetail> GetAllInKindContributionDetail()
        {
            return _unitOfWork.InKindContributionDetailRepository.GetAll();
        }

        public List<InKindContributionDetail> FindBy(Expression<Func<InKindContributionDetail, bool>> predicate)
        {
            return _unitOfWork.InKindContributionDetailRepository.FindBy(predicate);
        }

        public IEnumerable<InKindContributionDetail> Get(Expression<Func<InKindContributionDetail, bool>> filter = null, Func<IQueryable<InKindContributionDetail>, IOrderedQueryable<InKindContributionDetail>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.InKindContributionDetailRepository.Get(filter, orderBy, includeProperties);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
