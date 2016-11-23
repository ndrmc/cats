using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public class HubHubFlowTemplateService : IHubFlowTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HubHubFlowTemplateService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool Add(FlowTemplate item)
        {
            _unitOfWork.HubFlowTemplateRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(FlowTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubFlowTemplateRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(FlowTemplate item)
        {
            if (item == null) return false;
            _unitOfWork.HubFlowTemplateRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.HubFlowTemplateRepository.FindById(id);
            return Delete(item);
        }
        public FlowTemplate FindById(int id)
        {
            return _unitOfWork.HubFlowTemplateRepository.FindById(id);
        }
        public List<FlowTemplate> GetAll()
        {
            return _unitOfWork.HubFlowTemplateRepository.GetAll();

        }
        public List<FlowTemplate> FindBy(Expression<Func<FlowTemplate, bool>> predicate)
        {
            return _unitOfWork.HubFlowTemplateRepository.FindBy(predicate);

        }
    }
}