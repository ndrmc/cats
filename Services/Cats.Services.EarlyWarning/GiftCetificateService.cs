using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Workflows;

namespace Cats.Services.EarlyWarning
{

    public class GiftCertificateService : IGiftCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowActivityService _IWorkflowActivityService;

        public GiftCertificateService(UnitOfWork unitOfWork, IWorkflowActivityService iWorkflowActivityService)
        {
            this._unitOfWork = unitOfWork;
            this._IWorkflowActivityService = iWorkflowActivityService;



        }
        #region Default Service Implementation
        public bool AddGiftCertificate(GiftCertificate giftCertificate)
        {
            _unitOfWork.GiftCertificateRepository.Add(giftCertificate);

            _IWorkflowActivityService.EnterCreateWorkflow(giftCertificate);

            _unitOfWork.Save();
            return true;

        }
        public bool EditGiftCertificate(GiftCertificate giftCertificate)
        {
            _unitOfWork.GiftCertificateRepository.Edit(giftCertificate);

            _IWorkflowActivityService.EnterEditWorkflow(giftCertificate);

            _unitOfWork.Save();
            return true;

        }
        public bool DeleteGiftCertificate(GiftCertificate giftCertificate)
        {
            if (giftCertificate == null) return false;
   
            _IWorkflowActivityService.EnterDeleteWorkflow(giftCertificate);

            _unitOfWork.GiftCertificateRepository.Edit(giftCertificate);

            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.GiftCertificateRepository.FindById(id);
            if (entity == null) return false;

            _IWorkflowActivityService.EnterEditWorkflow(entity);

            DeleteGiftCertificate(entity);
             
            return true;
        }
        public List<GiftCertificate> GetAllGiftCertificate()
        {
            var allRecords = _unitOfWork.GiftCertificateRepository.GetAll().Cast<IWorkflow>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();

         
        }
        public GiftCertificate FindById(int id)
        {
            List<IWorkflow> lst = new List<IWorkflow>();

            lst.Add(_unitOfWork.GiftCertificateRepository.FindById(id));

            return _IWorkflowActivityService.ExcludeDeletedRecords(lst).Cast<GiftCertificate>().FirstOrDefault<GiftCertificate>();

           
        }
        public List<GiftCertificate> FindBy(Expression<Func<GiftCertificate, bool>> predicate)
        {
            var allRecords = _unitOfWork.GiftCertificateRepository.FindBy(predicate).Cast<IWorkflow>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();

           
        }
        #endregion


      

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }



        public GiftCertificate FindBySINumber(string siNumber)
        {
            List<IWorkflow> lst = new List<IWorkflow>();

            lst.Add(_unitOfWork.GiftCertificateRepository.FindBy(t => t.ShippingInstruction.Value == siNumber).FirstOrDefault());

            return _IWorkflowActivityService.ExcludeDeletedRecords(lst).Cast<GiftCertificate>().FirstOrDefault<GiftCertificate>();

          
        }

        public bool IsSINumberNewOrEdit(string siNumber, int giftCertificateID)
        {
            var gift = FindBySINumber(siNumber);
            bool inReceiptAllocation =
                _unitOfWork.ReceiptAllocationReository.Get(
                    t => t.SINumber == siNumber && t.CommoditySourceID == CommoditySourceConst.Constants.LOCALPURCHASE).Any();

            return ((gift == null || (gift.GiftCertificateID == giftCertificateID)) && !(inReceiptAllocation)) ;// new one or edit no problem 
        }

        public bool IsBillOfLoadingDuplicate(string billOfLoading)
        {
            var allRecords= _unitOfWork.GiftCertificateDetailRepository.Get(p => p.BillOfLoading == billOfLoading).Cast<IWorkflow>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Any();
 
        }
        public IEnumerable<GiftCertificate> Get(Expression<Func<GiftCertificate, bool>> filter = null, Func<IQueryable<GiftCertificate>, IOrderedQueryable<GiftCertificate>> orderBy = null, string includeProperties = "")
        {
            var allRecords = _unitOfWork.GiftCertificateRepository.Get(filter, orderBy, includeProperties).Cast<IWorkflow>().ToList();

            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();
        }
        
      
    }
}

 
      
