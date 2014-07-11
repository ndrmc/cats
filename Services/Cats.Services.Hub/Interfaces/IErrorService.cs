﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models.Hubs;

namespace Cats.Services.Hub.Interfaces
{
    public interface IErrorLogService : IDisposable
    {

        bool AddErrorLog(ErrorLog Entity);
        bool DeleteErrorLog(ErrorLog Entity);
        bool DeleteById(int id);
        bool EditErrorLog(ErrorLog Entity);
        ErrorLog FindById(int id);
        List<ErrorLog> GetAllErrorLog();
        List<ErrorLog> FindBy(Expression<Func<ErrorLog, bool>> predicate);

        IEnumerable<ErrorLog> Get(
                   Expression<Func<ErrorLog, bool>> filter = null,
                   Func<IQueryable<ErrorLog>, IOrderedQueryable<ErrorLog>> orderBy = null,
                   string includeProperties = "");
    }
}


