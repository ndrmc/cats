using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Services.Common;

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
            ApplicationSetting apset = new ApplicationSetting {SettingName = name, SettingValue = value};
            AddApplicationSetting(apset);

        }

        private int getIntValue(string name)
        {
            int val = 0;
            try
            {
                val = Int32.Parse(FindValue(name));
            }
            catch (Exception e)
            {
            }
            return val;
        }

        public int getPSNPWorkflow()
        {
            return getIntValue( ApplicationSettings.Default.PSNPWorkflow);
        }

        public int getPaymentRequestWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.TransporterPaymentRequestWorkflow);
        }

        public int getRegionalRequestWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.RegionalRequestWorkflow);
        }

        public int getTransporterChequeWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.TransporterChequeWorkflow);
        }

        public int getBidWinnerWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.BidWinnerWorkflow);
        }

        public int getDefaultRation()
        {
            return getIntValue(ApplicationSettings.Default.DefaultRation);
        }

        public int getNeedAssessmentPlanWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.NeedAssessmentPlanWorkflow);
        }

        public int getNeedAssessmentWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.NeedAssessmentWorkflow);
        }

        public int getReliefRequisitionWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.ReliefRequisitionWorkflow);
        }

        public int getTransportOrderWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.TransportOrderWorkflow);
        }


        public int GetDispatchWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.DispatchWorkflow);
        }
        public int getHRDWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.HRDWorkflow);
        }

        public int getReciptPlanForLoanWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.ReciptPlanForLoanWorkflow);
        }

        public int GetLocalPurchaseReceiptPlanWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow);
        }

        public int GetSwapWrokflow()
        {
            return getIntValue(ApplicationSettings.Default.SwapWorkflow);
        }

        public int getTransportRequisitionWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.TransportRequisitionWorkflow);
        }

        public int getDonationPlanHeaderWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.DonationPlanHeaderWorkflow);
        }


        public int getGiftCertificateWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.GiftCertificateWorkflow);
        }

        public int getBidPlanWorkflow()
        {
            return getIntValue(Cats.Services.Common.ApplicationSettings.Default.BidPlanWorkflow);
        }

        public int getBidPlanDeatailWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.BidPlanDetailActionWorkflow);
        }

        public int getTransferReceiptPlanWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.TranferReceiptPlanWorkflow);
        }

        public int getReceiveHubWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.ReceiveHubWorkflow);
    }

        public int getGlobalWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.GlobalWorkflow);
        }
        public int getDeliveryWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.DeliveryWorkflow);
        }

        public int GetDeliveryWorkflow()
        {
            return getIntValue(ApplicationSettings.Default.DeliveryWorkflow);
        }
    }
}
