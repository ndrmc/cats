using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.Hub;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public class HubProcessTemplateService : IHubProcessTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HubProcessTemplateService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool Add(ProcessTemplate item)
        {
            _unitOfWork.HubProcessTemplateRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(ProcessTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubProcessTemplateRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(ProcessTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubProcessTemplateRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.HubProcessTemplateRepository.FindById(id);
            return Delete(item);
        }
        public ProcessTemplate FindById(int id)
        {
            return _unitOfWork.HubProcessTemplateRepository.FindById(id);
        }
        public List<ProcessTemplate> GetAll()
        {
            return _unitOfWork.HubProcessTemplateRepository.GetAll();

        }
        public List<ProcessTemplate> FindBy(Expression<Func<ProcessTemplate, bool>> predicate)
        {
            return _unitOfWork.HubProcessTemplateRepository.FindBy(predicate);

        }
        public StateTemplate GetStartingState(int id)
        {
            return _unitOfWork.HubStateTemplateRepository.FindBy(s=>s.ParentProcessTemplateID==id && s.StateType==0).Single();

        }

    }
}