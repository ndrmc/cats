using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public class HrdDonorCoverageDetailService:IHrdDonorCoverageDetailService
   {
       private readonly IUnitOfWork _unitOfWork;

       public HrdDonorCoverageDetailService(IUnitOfWork unitOfWork)
       {
           _unitOfWork = unitOfWork;
       }
       public bool AddHrdDonorCoverageDetail(HrdDonorCoverageDetail hrdDonorCoverageDetail)
       {
           _unitOfWork.HrdDonorCoverageDetailRepository.Add(hrdDonorCoverageDetail);
           _unitOfWork.Save();
           return true;
       }

        public bool DeleteHrdDonorCoverageDetail(HrdDonorCoverageDetail hrdDonorCoverageDetail)
        {
            if (hrdDonorCoverageDetail == null) return false;
            _unitOfWork.HrdDonorCoverageDetailRepository.Delete(hrdDonorCoverageDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.HrdDonorCoverageDetailRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.HrdDonorCoverageDetailRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditHrdDonorCoverageDetail(HrdDonorCoverageDetail hrdDonorCoverageDetail)
        {
            _unitOfWork.HrdDonorCoverageDetailRepository.Edit(hrdDonorCoverageDetail);
            _unitOfWork.Save();
            return true;
        }

        public HrdDonorCoverageDetail FindById(int id)
        {
            return _unitOfWork.HrdDonorCoverageDetailRepository.FindById(id);
        }

        public List<HrdDonorCoverageDetail> GetAllHrdDonorCoverageDetail()
        {
            return _unitOfWork.HrdDonorCoverageDetailRepository.GetAll();
        }

        public List<HrdDonorCoverageDetail> FindBy(Expression<Func<HrdDonorCoverageDetail, bool>> predicate)
        {
            return _unitOfWork.HrdDonorCoverageDetailRepository.FindBy(predicate);
        }

        public IEnumerable<HrdDonorCoverageDetail> Get(Expression<Func<HrdDonorCoverageDetail, bool>> filter = null, Func<IQueryable<HrdDonorCoverageDetail>, IOrderedQueryable<HrdDonorCoverageDetail>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.HrdDonorCoverageDetailRepository.Get(filter, orderBy, includeProperties);
        }
        public bool AddWoredas(HrdDonorCoverageDetail hrdDonorCoverageDetail)
        {
            var woredaExists =
                _unitOfWork.HrdDonorCoverageDetailRepository.FindBy(
                    m => m.HRDDonorCoverageID == hrdDonorCoverageDetail.HRDDonorCoverageID &&
                         m.WoredaID == hrdDonorCoverageDetail.WoredaID).Count;
            if (woredaExists==0)
            {
                _unitOfWork.HrdDonorCoverageDetailRepository.Add(hrdDonorCoverageDetail);
                _unitOfWork.Save();
            }
            
            return true;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
