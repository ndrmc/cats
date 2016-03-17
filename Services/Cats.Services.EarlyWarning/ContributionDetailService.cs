using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public class ContributionDetailService:IContributionDetailService
   {
       private readonly IUnitOfWork _unitOfWork;
       public ContributionDetailService(IUnitOfWork unitOfWork)
       {
           _unitOfWork = unitOfWork;
       }
        public bool AddContributionDetail(ContributionDetail contributionDetail)
        {
            _unitOfWork.ContributionDetailRepository.Add(contributionDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteContributionDetail(ContributionDetail contributionDetail)
        {
            if (contributionDetail == null) return false;
            _unitOfWork.ContributionDetailRepository.Delete(contributionDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.ContributionDetailRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.ContributionDetailRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditContributionDetail(ContributionDetail contributionDetail)
        {
            _unitOfWork.ContributionDetailRepository.Edit(contributionDetail);
            _unitOfWork.Save();
            return true;
        }

        public ContributionDetail FindById(int id)
        {
            return _unitOfWork.ContributionDetailRepository.FindById(id);
        }

        public List<ContributionDetail> GetAllContributionDetail()
        {
            return _unitOfWork.ContributionDetailRepository.GetAll();
        }

        public List<ContributionDetail> FindBy(Expression<Func<ContributionDetail, bool>> predicate)
        {
            return _unitOfWork.ContributionDetailRepository.FindBy(predicate);
        }

        public IEnumerable<ContributionDetail> Get(Expression<Func<ContributionDetail, bool>> filter = null, Func<IQueryable<ContributionDetail>, IOrderedQueryable<ContributionDetail>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.ContributionDetailRepository.Get(filter, orderBy, includeProperties);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
