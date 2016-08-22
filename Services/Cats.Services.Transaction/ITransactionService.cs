﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;

namespace Cats.Services.Transaction
{
    public interface ITransactionService:IDisposable
    {
        bool AddTransaction(Models.Transaction item);
        bool UpdateTransaction(Models.Transaction item);

        bool DeleteTransaction(Models.Transaction item);
        bool DeleteById(Guid id);

        Models.Transaction FindById(Guid id);
        List<Models.Transaction> GetAllTransaction();
        List<Models.Transaction> FindBy(Expression<Func<Models.Transaction, bool>> predicate);
        List<Models.Transaction> PostPSNPPlan(RegionalPSNPPlan plan, Ration ration);
        List<Models.Transaction> PostHRDPlan(HRD plan, Ration ration);
        bool PostGiftCertificate(int giftCertificateId);
        bool PostDeliveryReconcileReceipt(int deliveryID);

        bool RevertGiftCertificate(int giftCertificateId);
        bool PrintedGiftCertificate(int giftCertificateId);
        bool PostRequestAllocation(int requestId);
        bool PostSIAllocation(int requisitionID);
        bool PostSIAllocationUncommit(int requisitionID);
        bool PostDonationPlan(DonationPlanHeader donationPlanDetail);
        bool RevertDonationPlan(DonationPlanHeader donationPlanDetail);
        bool PostDistribution(int distributionId);
    }
}