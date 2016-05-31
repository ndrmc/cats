using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public interface IReasonService:IDisposable
    {
        bool AddReason(Reason reason);
        bool DeleteReason(Reason reason);
        bool DeleteById(int id);
        bool EditReason(Reason reason);
        Reason FindById(int id);
        List<Reason> GetAllReason();
        List<Reason> FindBy(Expression<Func<Reason, bool>> predicate);

        IEnumerable<Reason> Get(
             Expression<Func<Reason, bool>> filter = null,
             Func<IQueryable<Reason>, IOrderedQueryable<Reason>> orderBy = null,
             string includeProperties = "");

  
    }
}