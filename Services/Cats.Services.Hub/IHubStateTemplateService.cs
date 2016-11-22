using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IHubStateTemplateService
    {
        bool Add(StateTemplate item);
        bool Update(StateTemplate item);
        bool Delete(StateTemplate item);
        bool DeleteById(int id);
        StateTemplate FindById(int id);
        List<StateTemplate> GetAll();
        List<StateTemplate> FindBy(Expression<Func<StateTemplate, bool>> predicate);
    }
}