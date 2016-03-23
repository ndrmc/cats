using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IDonorService
    {
        bool AddDonor(Donor donor);
        bool DeleteDonor(Donor donor);
        bool DeleteById(int id);
        bool EditDonor(Donor donor);
        Donor FindById(int id);
        List<Donor> GetAllDonor();
        List<Donor> FindBy(Expression<Func<Donor, bool>> predicate);
    }
}
