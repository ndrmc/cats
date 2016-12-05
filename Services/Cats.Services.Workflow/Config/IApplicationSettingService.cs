using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.Workflows.Config
{
    public interface IApplicationSettingService
    {
        bool AddApplicationSetting(ApplicationSetting item);
        bool UpdateApplicationSetting(ApplicationSetting item);

        bool DeleteApplicationSetting(ApplicationSetting item);
        bool DeleteById(int id);
        int getPSNPWorkflow();
        int getPaymentRequestWorkflow();
        int getTransporterChequeWorkflow();
        int getDefaultRation();
        int getBidWinnerWorkflow();
        int getRegionalRequestWorkflow();
        ApplicationSetting FindById(int id);
        List<ApplicationSetting> GetAllApplicationSetting();
        List<ApplicationSetting> FindBy(Expression<Func<ApplicationSetting, bool>> predicate);
        void SetValue(string name, string value);
        string FindValue(string name);
        int getTransportOrderWorkflow();
        int getReciptPlanForLoanWorkflow();
        int getNeedAssessmentPlanWorkflow();
        int getNeedAssessmentWorkflow();
        int getReliefRequisitionWorkflow();
        int getHRDWorkflow();
        int GetLocalPurchaseReceiptPlanWorkflow();
        int GetSwapWrokflow();
        int getTransportRequisitionWorkflow();
        int getDonationPlanHeaderWorkflow();

        int getGiftCertificateWorkflow();
        int getBidPlanWorkflow();
        int getBidPlanDeatailWorkflow();
        int getTransferReceiptPlanWorkflow();
        int getGlobalWorkflow();
        int getReceiveHubWorkflow();
        int GetDispatchWorkflow();
        int GetDeliveryWorkflow();
    }
}