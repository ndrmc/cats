using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Data;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public class HubStateTemplateService : IHubStateTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HubStateTemplateService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool Add(StateTemplate item)
        {
            _unitOfWork.HubStateTemplateRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(StateTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubStateTemplateRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(StateTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubStateTemplateRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.HubStateTemplateRepository.FindById(id);
            return Delete(item);
        }
        public StateTemplate FindById(int id)
        {
            return _unitOfWork.HubStateTemplateRepository.FindById(id);
        }
        public List<StateTemplate> GetAll()
        {
            return _unitOfWork.HubStateTemplateRepository.GetAll();

        }
        public List<StateTemplate> FindBy(Expression<Func<StateTemplate, bool>> predicate)
        {
            return _unitOfWork.HubStateTemplateRepository.FindBy(predicate);

        }

    }
}