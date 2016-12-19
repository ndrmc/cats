using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Services.Workflows;

namespace Cats.Services.EarlyWarning
{

    public class GiftCertificateDetailService : IGiftCertificateDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowActivityService _IWorkflowActivityService;



        public GiftCertificateDetailService(UnitOfWork unitOfWork, IWorkflowActivityService iWorkflowActivityService)
        {
            this._unitOfWork = unitOfWork;
            this._IWorkflowActivityService = iWorkflowActivityService;



        }
        #region Default Service Implementation
        public bool AddGiftCertificateDetail(GiftCertificateDetail giftCertificateDetail)
        {
            _unitOfWork.GiftCertificateDetailRepository.Add(giftCertificateDetail);

            _IWorkflowActivityService.EnterEditWorkflow(giftCertificateDetail.GiftCertificate, "Child Gift Certificate Have Been Added");




            _unitOfWork.Save();
            return true;

        }
        public bool EditGiftCertificateDetail(GiftCertificateDetail giftCertificateDetail)
        {
            _unitOfWork.GiftCertificateDetailRepository.Edit(giftCertificateDetail);


            _IWorkflowActivityService.EnterEditWorkflow(giftCertificateDetail.GiftCertificate, "Child Gift Certificate Have Been Edited");


            _unitOfWork.Save();
            return true;

        }
        public bool DeleteGiftCertificateDetail(GiftCertificateDetail giftCertificateDetail)
        {
            if (giftCertificateDetail == null) return false;
            _unitOfWork.GiftCertificateDetailRepository.Delete(giftCertificateDetail);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.GiftCertificateDetailRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.GiftCertificateDetailRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<GiftCertificateDetail> GetAllGiftCertificateDetail()
        {
            return _unitOfWork.GiftCertificateDetailRepository.GetAll();
        }
        public GiftCertificateDetail FindById(int id)
        {
            return _unitOfWork.GiftCertificateDetailRepository.FindById(id);
        }
        public List<GiftCertificateDetail> FindBy(Expression<Func<GiftCertificateDetail, bool>> predicate)
        {
            return _unitOfWork.GiftCertificateDetailRepository.FindBy(predicate);
        }
        #endregion

        public IEnumerable<GiftCertificateDetail> Get(
         Expression<Func<GiftCertificateDetail, bool>> filter = null,
         Func<IQueryable<GiftCertificateDetail>, IOrderedQueryable<GiftCertificateDetail>> orderBy = null,
         string includeProperties = "")
        {
            return _unitOfWork.GiftCertificateDetailRepository.Get(filter, orderBy, includeProperties);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

    }
}


