using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public interface IHubBusinessProcessStateService
    {
         bool Add(BusinessProcessState item);
         bool Update(BusinessProcessState item);
         bool Delete(BusinessProcessState item);
         bool DeleteById(int id);
         BusinessProcessState FindById(int id);
         List<BusinessProcessState> GetAll();
         List<BusinessProcessState> FindBy(Expression<Func<BusinessProcessState, bool>> predicate);
    }
}