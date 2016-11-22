using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IHubFlowTemplateService
    {
        bool Add(FlowTemplate item);
        bool Update(FlowTemplate item);
        bool Delete(FlowTemplate item);
        bool DeleteById(int id);
        FlowTemplate FindById(int id);
        List<FlowTemplate> GetAll();
        List<FlowTemplate> FindBy(Expression<Func<FlowTemplate, bool>> predicate);
    }
}