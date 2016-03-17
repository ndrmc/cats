using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IRequestDetailCommodityService
    {
        List<RequestDetailCommodity> FindBy(Expression<Func<RequestDetailCommodity, bool>> predicate);
    }
}
