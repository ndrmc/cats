using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Data;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hubs
{
    public class HubBusinessProcessStateService :IHubBusinessProcessStateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HubBusinessProcessStateService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool Add(BusinessProcessState item)
        {
            _unitOfWork.HubBusinessProcessStateRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(BusinessProcessState item)
        {
            if (item == null) return false;
            _unitOfWork.HubBusinessProcessStateRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(BusinessProcessState item)
        {
            if (item == null) return false;
            _unitOfWork.HubBusinessProcessStateRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.HubBusinessProcessStateRepository.FindById(id);
            return Delete(item);
        }
        public BusinessProcessState FindById(int id)
        {
            return _unitOfWork.HubBusinessProcessStateRepository.FindById(id);
        }
        public List<BusinessProcessState> GetAll()
        {
            return _unitOfWork.HubBusinessProcessStateRepository.GetAll();

        }
        public List<BusinessProcessState> FindBy(Expression<Func<BusinessProcessState, bool>> predicate)
        {
            return _unitOfWork.HubBusinessProcessStateRepository.FindBy(predicate);

        }
   }
 }