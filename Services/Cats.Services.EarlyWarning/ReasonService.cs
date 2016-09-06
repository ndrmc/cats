

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{

    public class ReasonService : IReasonService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReasonService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        
        #region Default Service Implementation
        public bool AddReason(Reason reason)
        {
            _unitOfWork.ReasonRepository.Add(reason);
            _unitOfWork.Save();
            return true;

        }
        public bool EditReason(Reason reason)
        {
            _unitOfWork.ReasonRepository.Edit(reason);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteReason(Reason reason)
        {
            if (reason == null) return false;
            _unitOfWork.ReasonRepository.Delete(reason);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.ReasonRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.ReasonRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<Reason> GetAllReason()
        {
            //return _unitOfWork.ReasonRepository.FindBy(c => c.ParentID != null);
            return _unitOfWork.ReasonRepository.GetAll();
        }

        public List<Reason> GetCommonReason()
        {
            return _unitOfWork.ReasonRepository.FindBy(m=>m.ReasonID==1 && m.ReasonID==2 && m.ReasonID==4 && m.ReasonID==8);
        }

        public Reason FindById(int id)
        {
            return _unitOfWork.ReasonRepository.FindById(id);
        }
        public List<Reason> FindBy(Expression<Func<Reason, bool>> predicate)
        {
            return _unitOfWork.ReasonRepository.FindBy(predicate);
        }
        #endregion

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

        public IEnumerable<Reason> Get(Expression<Func<Reason, bool>> filter = null,
           Func<IQueryable<Reason>, IOrderedQueryable<Reason>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.ReasonRepository.Get(filter, orderBy, includeProperties);
        }

      


        public IEnumerable<Unit> GetAllUnit()
        {
            return _unitOfWork.UnitRepository.GetAll();
        }
    }
}


