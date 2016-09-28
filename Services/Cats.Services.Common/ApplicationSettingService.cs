using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.Common
{
    public class ApplicationSettingService : IApplicationSettingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationSettingService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool AddApplicationSetting(ApplicationSetting item)
        {
            _unitOfWork.ApplicationSettingRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool UpdateApplicationSetting(ApplicationSetting item)
        {
            if (item == null) return false;
            _unitOfWork.ApplicationSettingRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteApplicationSetting(ApplicationSetting item)
        {
            if (item == null) return false;
            _unitOfWork.ApplicationSettingRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.ApplicationSettingRepository.FindById(id);
            return DeleteApplicationSetting(item);
        }
        public ApplicationSetting FindById(int id)
        {
            return _unitOfWork.ApplicationSettingRepository.FindById(id);
        }
        public List<ApplicationSetting> GetAllApplicationSetting()
        {
            return _unitOfWork.ApplicationSettingRepository.GetAll();

        }
        public List<ApplicationSetting> FindBy(Expression<Func<ApplicationSetting, bool>> predicate)
        {
            return _unitOfWork.ApplicationSettingRepository.FindBy(predicate);

        }
        public string FindValue(string name)
        {
            List<ApplicationSetting> ret = FindBy(t => t.SettingName == name);
            if (ret.Count == 1)
            {
                return ret[0].SettingValue;
            }
            return "";

        }
        public void SetValue(string name, string value)
        {
            List<ApplicationSetting> ret = FindBy(t => t.SettingName == name);
            if (ret.Count == 1)
            {
                ret[0].SettingValue = value;
                UpdateApplicationSetting(ret[0]);
                return;
            }
            ApplicationSetting apset = new ApplicationSetting { SettingName = name, SettingValue = value };
            AddApplicationSetting(apset);

        }
        private int getIntValue(string name)
        {
            int val = 0;
            try
            {
                val = Int32.Parse(FindValue(name));
            }
            catch (Exception e) { }
            return val;
        }
        public int getPSNPWorkflow()
        {
            return getIntValue("PSNPWorkflow");
        }
        public int getPaymentRequestWorkflow()
        {
            return getIntValue("TransporterPaymentRequestWorkflow");
        }
        public int getRegionalRequestWorkflow()
        {
            return getIntValue("RegionalRequestWorkflow");
        }
        public int getTransporterChequeWorkflow()
        {
            return getIntValue("TransporterChequeWorkflow");
        }
        public int getBidWinnerWorkflow()
        {
            return getIntValue("BidWinnerWorkflow");
        }

        public int getDefaultRation()
        {
            return getIntValue("DefaultRation");
        }

        public int getNeedAssessmentPlanWorkflow()
        {
            return getIntValue("NeedAssessmentPlanWorkflow");
        }
        public int getNeedAssessmentWorkflow()
        {
            return getIntValue("NeedAssessmentWorkflow");
        }
        public int getReliefRequisitionWorkflow()
        {
            return getIntValue("ReliefRequisitionWorkflow");
        }
        public int getTransportOrderWorkflow()
        {
            return getIntValue("TransportOrderWorkflow");
        }
        public int getHRDWorkflow()
        {
            return getIntValue("HRDWorkflow");
        }
        public int getReciptPlanForLoanWorkflow()
        {
            return getIntValue("ReciptPlanForLoanWorkflow");
        }
        public int GetLocalPurchaseReceiptPlanWorkflow()
        {
            return getIntValue("LocalPurchaseReceiptPlanWorkflow");
        }

        public int GetSwapWrokflow()
        {
            return getIntValue("SwapWorkflow");
        }

        public int getTransportRequisitionWorkflow()
        {
            return getIntValue("TransportRequisitionWorkflow");
        }

        public int getDonationPlanHeaderWorkflow()
        {
            return getIntValue("DonationPlanHeaderWorkflow");
        }

        public int getBidPlanWorkflow()
        {
            return getIntValue("BidWinnerWorkflow");
        }
    }  
}
