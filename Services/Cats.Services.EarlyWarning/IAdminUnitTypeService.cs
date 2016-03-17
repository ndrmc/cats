
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IAdminUnitTypeService
    {

        bool AddAdminUnitType(AdminUnitType adminUnitType);
        bool DeleteAdminUnitType(AdminUnitType adminUnitType);
        bool DeleteById(int id);
        bool EditAdminUnitType(AdminUnitType adminUnitType);
        AdminUnitType FindById(int id);
        List<AdminUnitType> GetAllAdminUnitType();
        List<AdminUnitType> FindBy(Expression<Func<AdminUnitType, bool>> predicate);


    }
}


