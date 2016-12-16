

using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.Hub;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;
using Cats.Services.Workflows;

namespace Cats.Services.Hub
{

    public class GiftCertificateService : IGiftCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowActivityService _IWorkflowActivityService;



        public GiftCertificateService(IUnitOfWork unitOfWork, IWorkflowActivityService iWorkflowActivityService)
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
            var allRecords = _unitOfWork.GiftCertificateRepository.GetAll().Cast<Models.Hubs.IWorkflowHub>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();


        }
        public GiftCertificate FindById(int id)
        {
            List<IWorkflowHub> lst = new List<IWorkflowHub>();

            lst.Add(_unitOfWork.GiftCertificateRepository.FindById(id));

            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(lst).Cast<GiftCertificate>().FirstOrDefault<GiftCertificate>();

        }
        public List<GiftCertificate> FindBy(Expression<Func<GiftCertificate, bool>> predicate)
        {
            var allRecords = _unitOfWork.GiftCertificateRepository.FindBy(predicate).Cast<IWorkflowHub>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();


          
        }
        #endregion
        /// <summary>
        /// Gets the monthly summary.
        /// </summary>
        /// <returns></returns>
        public ObjectResult<RPT_MonthlyGiftSummary_Result> GetMonthlySummary()
        {
            return _unitOfWork.ReportRepository.GetMonthlyGiftSummary();

        }


        /// <summary>
        /// Updates the specified gift certificate model.
        /// </summary>
        /// <param name="giftCertificateModel">The gift certificate model.</param>
        /// <param name="inserted">The inserted gift certificate detail.</param>
        /// <param name="updated">The updated gift certificate detail.</param>
        /// <param name="deleted">The deleted gift certificate detail.</param>
        public void Update(GiftCertificate giftCertificateModel, List<GiftCertificateDetail> inserted,
            List<GiftCertificateDetail> updated, List<GiftCertificateDetail> deleted)
        {

            // DRMFSSEntities1 db = new DRMFSSEntities1();
            GiftCertificate orginal = _unitOfWork.GiftCertificateRepository.Get(p => p.GiftCertificateID == giftCertificateModel.GiftCertificateID).SingleOrDefault();
            if (orginal != null)
            {

                orginal.GiftDate = giftCertificateModel.GiftDate;
                orginal.DonorID = giftCertificateModel.DonorID;
                orginal.ShippingInstructionID = giftCertificateModel.ShippingInstructionID;
                orginal.ReferenceNo = giftCertificateModel.ReferenceNo;
                orginal.Vessel = giftCertificateModel.Vessel;
                orginal.ETA = giftCertificateModel.ETA;
                orginal.ProgramID = giftCertificateModel.ProgramID;
                orginal.PortName = giftCertificateModel.PortName;
                orginal.DModeOfTransport = giftCertificateModel.DModeOfTransport;

                foreach (GiftCertificateDetail insert in inserted)
                {
                    orginal.GiftCertificateDetails.Add(insert);
                }

                foreach (GiftCertificateDetail delete in deleted)
                {
                    GiftCertificateDetail deletedGiftDetails = _unitOfWork.GiftCertificateDetailRepository.FindBy(p => p.GiftCertificateDetailID == delete.GiftCertificateDetailID).SingleOrDefault();
                    if (deletedGiftDetails != null)
                    {
                        _unitOfWork.GiftCertificateDetailRepository.Delete(deletedGiftDetails);
                    }
                }

                foreach (GiftCertificateDetail update in updated)
                {
                    GiftCertificateDetail updatedGiftDetails = _unitOfWork.GiftCertificateDetailRepository.Get(p => p.GiftCertificateDetailID == update.GiftCertificateDetailID).SingleOrDefault();
                    if (updatedGiftDetails != null)
                    {
                        updatedGiftDetails.CommodityID = update.CommodityID;
                        updatedGiftDetails.BillOfLoading = update.BillOfLoading;
                        updatedGiftDetails.YearPurchased = update.YearPurchased;
                        updatedGiftDetails.AccountNumber = update.AccountNumber;
                        updatedGiftDetails.WeightInMT = update.WeightInMT;
                        updatedGiftDetails.EstimatedPrice = update.EstimatedPrice;
                        updatedGiftDetails.EstimatedTax = update.EstimatedTax;
                        updatedGiftDetails.DCurrencyID = update.DCurrencyID;
                        updatedGiftDetails.DFundSourceID = update.DFundSourceID;
                        updatedGiftDetails.DBudgetTypeID = update.DBudgetTypeID;
                        updatedGiftDetails.ExpiryDate = update.ExpiryDate;
                    }
                }
                _unitOfWork.Save();
            }

        }


        /// <summary>
        /// Gets the monthly summary ETA.
        /// </summary>
        /// <returns></returns>
        public ObjectResult<RPT_MonthlyGiftSummary_Result> GetMonthlySummaryETA()
        {
            return _unitOfWork.ReportRepository.GetMonthlyGiftSummaryETA();
        }


        /// <summary>
        /// Finds the by SI number.
        /// </summary>
        /// <param name="SINumber">The SI number.</param>
        /// <returns></returns>
        public GiftCertificate FindBySINumber(int SINumber)
        {
            List<IWorkflowHub> lst = new List<IWorkflowHub>();

            lst.Add(_unitOfWork.GiftCertificateRepository.Get(p => p.ShippingInstructionID == SINumber).FirstOrDefault());

            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(lst).Cast<GiftCertificate>().FirstOrDefault<GiftCertificate>();

            
        }

        public GiftCertificate FindBySINumber(string SINumber)
        {
            List<IWorkflowHub> lst = new List<IWorkflowHub>();

            lst.Add(_unitOfWork.GiftCertificateRepository.Get(p => p.ShippingInstruction.Value == SINumber).FirstOrDefault());

            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(lst).Cast<GiftCertificate>().FirstOrDefault<GiftCertificate>();

 
        }
        /// <summary>
        /// Gets the SI balances.
        /// </summary>
        /// <returns></returns>
        public List<SIBalance> GetSIBalances()
        {
            var tempGiftCertificateDetails = _unitOfWork.GiftCertificateDetailRepository.GetAll();
            var list = (from GCD in tempGiftCertificateDetails
                        group GCD by GCD.GiftCertificate.ShippingInstruction.Value into si
                        select new SIBalance() { SINumber = si.Key, AvailableBalance = si.Sum(p => p.WeightInMT) }).ToList();

            return list;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }



        public IEnumerable<GiftCertificate> Get(Expression<Func<GiftCertificate, bool>> filter = null, Func<IQueryable<GiftCertificate>, IOrderedQueryable<GiftCertificate>> orderBy = null, string includeProperties = "")
        {
            var allRecords = _unitOfWork.GiftCertificateRepository.Get(filter, orderBy, includeProperties).Cast<IWorkflowHub>().ToList();

            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(allRecords).Cast<GiftCertificate>().ToList<GiftCertificate>();

          
        }
    }
}


