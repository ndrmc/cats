﻿using System;
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
            _IWorkflowActivityService.EnterEditWorkflow(giftCertificate);

            _unitOfWork.GiftCertificateRepository.Delete(giftCertificate);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.GiftCertificateRepository.FindById(id);
            if (entity == null) return false;

            _IWorkflowActivityService.EnterEditWorkflow(entity);

            _unitOfWork.GiftCertificateRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<GiftCertificate> GetAllGiftCertificate()
        {
            return _unitOfWork.GiftCertificateRepository.GetAll();
        }
        public GiftCertificate FindById(int id)
        {
            return _unitOfWork.GiftCertificateRepository.FindById(id);
        }
        public List<GiftCertificate> FindBy(Expression<Func<GiftCertificate, bool>> predicate)
        {
            return _unitOfWork.GiftCertificateRepository.FindBy(predicate);
        }
        #endregion


      

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }



        public GiftCertificate FindBySINumber(string siNumber)
        {
            return _unitOfWork.GiftCertificateRepository.FindBy(t => t.ShippingInstruction.Value == siNumber).FirstOrDefault();
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
            return _unitOfWork.GiftCertificateDetailRepository.Get(p => p.BillOfLoading == billOfLoading).Any();
        }
        public IEnumerable<GiftCertificate> Get(Expression<Func<GiftCertificate, bool>> filter = null, Func<IQueryable<GiftCertificate>, IOrderedQueryable<GiftCertificate>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.GiftCertificateRepository.Get(filter, orderBy, includeProperties);
        }
        
      
    }
}

 
      
