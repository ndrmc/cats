using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models.Hubs;

namespace Cats.Services.Hubs
{
    public interface IHubProcessTemplateService
    {
        bool Add(ProcessTemplate item);
        bool Update(ProcessTemplate item);
        bool Delete(ProcessTemplate item);
        bool DeleteById(int id);
        ProcessTemplate FindById(int id);
        List<ProcessTemplate> GetAll();
        List<ProcessTemplate> FindBy(Expression<Func<ProcessTemplate, bool>> predicate);
    }
}