
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IProgramService
    {

        bool AddProgram(Program program);
        bool DeleteProgram(Program program);
        bool DeleteById(int id);
        bool EditProgram(Program program);
        Program FindById(int id);
        List<Program> GetAllProgram();
        List<Program> FindBy(Expression<Func<Program, bool>> predicate);


    }
}


