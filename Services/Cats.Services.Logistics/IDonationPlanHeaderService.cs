﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models;


namespace Cats.Services.Logistics
{
    public interface IDonationPlanHeaderService
    {

        bool AddDonationPlanHeader(DonationPlanHeader donationPlanHeader);
        bool DeleteDonationPlanHeader(DonationPlanHeader donationPlanHeader);
        bool DeleteById(int id);
        bool EditDonationPlanHeader(DonationPlanHeader donationPlanHeader);
        DonationPlanHeader FindById(int id);
        List<DonationPlanHeader> GetAllDonationPlanHeader();
        List<DonationPlanHeader> FindBy(Expression<Func<DonationPlanHeader, bool>> predicate);
        bool DeleteDonation(DonationPlanHeader donationPlanHeader);
        bool DeleteReceiptAllocation(DonationPlanHeader donationPlanHeader);

        IEnumerable<DonationPlanHeader> Get(Expression<Func<DonationPlanHeader, bool>> filter = null,
            Func<IQueryable<DonationPlanHeader>, IOrderedQueryable<DonationPlanHeader>> orderBy = null,
            string includeProperties = "");
        void Dispose();

    }
}


