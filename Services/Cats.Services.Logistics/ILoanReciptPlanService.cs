﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;
using System.Linq;

namespace Cats.Services.Logistics
{
   public  interface ILoanReciptPlanService:IDisposable
   {
       
       bool AddLoanReciptPlan(LoanReciptPlan loanReciptPlan);
       bool DeleteLoanReciptPlan(LoanReciptPlan loanReciptPlan);
       bool DeleteById(int id);
       bool EditLoanReciptPlan(LoanReciptPlan loanReciptPlan);
       LoanReciptPlan FindById(int id);
       List<LoanReciptPlan> GetAllLoanReciptPlan();
       List<LoanReciptPlan> FindBy(Expression<Func<LoanReciptPlan, bool>> predicate);
       bool ApproveRecieptPlan(LoanReciptPlan loanReciptPlan);
       bool DeleteLoanReciptAllocation(LoanReciptPlan loanReciptPlan);
       bool DeleteLoanWithDetail(LoanReciptPlan loanReciptPlan);

        IEnumerable<LoanReciptPlan> Get(
           Expression<Func<LoanReciptPlan, bool>> filter = null,
           Func<IQueryable<LoanReciptPlan>, IOrderedQueryable<LoanReciptPlan>> orderBy = null,
           string includeProperties = "");

    }
}


      