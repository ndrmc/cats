﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Cats.Data.Hub;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;
using Cats.Models.Hubs.ViewModels;
using Cats.Models.Hubs.ViewModels.Report;
using Cats.Models.Hubs.ViewModels.Report.Data;
using Cats.Models.Hubs;
using Cats.Data.UnitWork;
using daModel = Cats.Models;
using Ledger = Cats.Models.Ledger;
using Cats.Services.Workflows;

namespace Cats.Services.Hub
{
    public class TransactionService : ITransactionService
    {
        private readonly Data.UnitWork.IUnitOfWork _unitOfWorkNew;
        private readonly Data.Hub.UnitWork.IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private readonly IProjectCodeService _projectCodeService;
        private readonly IWorkflowActivityService _IWorkflowActivityService;


        public TransactionService(Data.Hub.UnitWork.IUnitOfWork unitOfWork, Data.UnitWork.IUnitOfWork unitOfWorkNew, IAccountService accountService, IShippingInstructionService shippingInstructionService, IProjectCodeService projectCodeService, IWorkflowActivityService iWorkflowActivityService)
        {

            this._IWorkflowActivityService = iWorkflowActivityService;
            this._unitOfWork = unitOfWork;
            this._unitOfWorkNew = unitOfWorkNew;
            this._accountService = accountService;
            this._shippingInstructionService = shippingInstructionService;
            this._projectCodeService = projectCodeService;

        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _accountService.Dispose();
            _projectCodeService.Dispose();
            _shippingInstructionService.Dispose();
        }


        /// <summary>
        /// Gets the active accounts for ledger.
        /// </summary>
        /// <param name="LedgerID">The ledger ID.</param>
        /// <returns></returns>
        ///

        public List<Account> GetActiveAccountsForLedger(int LedgerID)
        {
            var accounts =
                _unitOfWork.TransactionRepository.FindBy(t => t.LedgerID == LedgerID).Select(t => t.Account).ToList();

            return accounts;
        }


        /// <summary>
        /// Gets the transactions for ledger.
        /// </summary>
        /// <param name="LedgerID">The ledger ID.</param>
        /// <param name="AccountID">The account ID.</param>
        /// <param name="Commodity">The commodity.</param>
        /// <returns></returns>
        public List<Transaction> GetTransactionsForLedger(int LedgerID, int AccountID, int Commodity)
        {

            var transactions =
                _unitOfWork.TransactionRepository.FindBy(
                    t =>
                    (t.LedgerID == LedgerID && t.AccountID == AccountID) &&
                    (t.CommodityID == Commodity || t.ParentCommodityID == Commodity));

            return transactions;
        }


        /// <summary>
        /// Gets the total receipt allocation.
        /// </summary>
        /// <param name="siNumber">The SI number.</param>
        /// <param name="commodityId"></param>
        /// <param name="hubId">The hub id.</param>
        /// <returns></returns>
        public decimal GetTotalReceiptAllocation(int siNumber, int commodityId, int hubId)
        {
            decimal totalAllocation = 0;
            var commodity = _unitOfWork.CommodityRepository.FindById(commodityId);
            if (commodity.ParentID != null)
            {
                commodityId = commodity.ParentID.Value;
            }
            var allocationSum = _unitOfWork.TransactionRepository.FindBy(
                t => t.ShippingInstructionID == siNumber
                     && t.HubID == hubId
                     && (t.ParentCommodityID == commodityId || t.CommodityID == commodityId)
                     && t.LedgerID == Ledger.Constants.GIFT_CERTIFICATE
                     && t.QuantityInMT > 0
                ).Select(t => t.QuantityInMT).ToList();



            if (allocationSum.Any())
            {
                totalAllocation = allocationSum.Sum();
            }
            return totalAllocation;
        }

        /// <summary>
        /// Gets the total received from receipt allocation.
        /// </summary>
        /// <param name="siNumber">The SI number.</param>
        /// <param name="hubId">The hub id.</param>
        /// <returns></returns>
        public decimal GetTotalReceivedFromReceiptAllocation(int siNumber, int commodityId, int hubId)
        {
            decimal totalAllocation = 0;
            var commodity = _unitOfWork.CommodityRepository.FindById(commodityId);
            if (commodity.ParentID != null)
            {
                commodityId = commodity.ParentID.Value;
            }
            var allocationSum = _unitOfWork.TransactionRepository.FindBy(t => t.ShippingInstructionID == siNumber
                                         && t.HubID == hubId
                                       && t.ParentCommodityID == commodityId
                                       && t.LedgerID == Ledger.Constants.GOODS_ON_HAND
                                       && t.QuantityInMT > 0).Select(t => t.QuantityInMT).ToList();


            if (allocationSum.Any())
            {
                totalAllocation = allocationSum.Sum();
            }
            return totalAllocation;
        }

        /// <summary>
        /// Gets the commodity balance for store.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="parentCommodityId">The parent commodity id.</param>
        /// <param name="si">The SI.</param>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public decimal GetCommodityBalanceForStore(int storeId, int commodityId, int si, int project)
        {
            var balance = _unitOfWork.TransactionRepository.FindBy(t =>
                                                                   t.StoreID == storeId &&
                                                                   t.CommodityID == commodityId &&
                                                                   t.ShippingInstructionID == si &&
                                                                   t.ProjectCodeID == project &&
                                                                   t.LedgerID ==
                                                                  Ledger.Constants.GOODS_ON_HAND

                ).Select(t => t.QuantityInMT).ToList();

            if (balance.Any())
            {
                return balance.Sum();
            }
            return 0;
        }


        /// <summary>
        /// Gets the commodity balance for stack.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="stack">The stack.</param>
        /// <param name="parentCommodityId">The parent commodity id.</param>
        /// <param name="si">The SI.</param>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public decimal GetCommodityBalanceForStack(int storeId, int stack, int CommodityId, int si, int project)
        {
            var balance = _unitOfWork.TransactionRepository.FindBy(t =>
                                                                   t.StoreID == storeId &&
                                                                   t.CommodityID == CommodityId &&
                                                                   t.ShippingInstructionID == si &&
                                                                   t.ProjectCodeID == project && t.Stack == stack &&
                                                                   t.LedgerID ==
                                                                   Ledger.Constants.GOODS_ON_HAND

                ).Select(t => t.QuantityInMT).ToList();

            if (balance.Any())
            {
                return balance.Sum();
            }

            return 0;
        }

        public decimal GetCommodityBalanceForStack2(int storeId, int parentCommodityId, int si, int project)
        {
            var balance = _unitOfWork.TransactionRepository.FindBy(t =>
                                                                   t.StoreID == storeId &&
                                                                   t.ParentCommodityID == parentCommodityId &&
                                                                   t.ShippingInstructionID == si &&
                                                                   t.ProjectCodeID == project &&
                                                                   t.LedgerID ==
                                                                   Ledger.Constants.GOODS_ON_HAND

                ).Select(t => t.QuantityInMT).ToList();

            if (balance.Any())
            {
                return balance.Sum();
            }

            return 0;
        }



        /// <summary>
        /// Saves the receipt transaction.
        /// </summary>
        /// <param name="receiveModels">The receive models.</param>
        /// <param name="user">The user.</param>
        public Boolean SaveReceiptTransaction(ReceiveViewModel receiveModels, UserProfile user, Boolean reverse = false)
        {
            // Populate more details of the reciept object
            // Save it when you are done.
            int transactionsign = reverse ? -1 : 1;
            Receive receive;

            if (receiveModels.ReceiveID != null)
            {
                receive = _unitOfWork.ReceiveRepository.FindById(receiveModels.ReceiveID.GetValueOrDefault());
            }
            else
            {
                receive = new Receive();
                receive = receiveModels.GenerateReceive();
            }

            receive.CreatedDate = DateTime.Now;
            receive.HubID = user.DefaultHubObj.HubID;
            receive.UserProfileID = user.UserProfileID;

            int? donorId = receive.SourceDonorID;
            var commType = _unitOfWork.CommodityTypeRepository.FindById(receiveModels.CommodityTypeID);

            // var comms = GenerateReceiveDetail(commodities);


            var transactionGroupId = Guid.NewGuid();

            receive.ReceiveDetails.Clear();

            foreach (ReceiveDetailViewModel c in receiveModels.ReceiveDetails)
            {
                if (commType.CommodityTypeID == 2)//if it's a non food
                {
                    c.ReceivedQuantityInMT = 0;
                    c.SentQuantityInMT = 0;
                }

                TransactionGroup tgroup = new TransactionGroup();
                tgroup.TransactionGroupID = transactionGroupId;
                var receiveDetail = new ReceiveDetail()
                {
                    CommodityID = c.CommodityID,
                    Description = c.Description,
                    SentQuantityInMT = c.SentQuantityInMT.Value,
                    SentQuantityInUnit = c.SentQuantityInUnit.Value,
                    UnitID = c.UnitID,
                    ReceiveID = receive.ReceiveID,
                    ReceiveDetailID = Guid.NewGuid()
                };
                //if (c.ReceiveDetailID.HasValue&&!reverse)
                //{
                //    receiveDetail.ReceiveDetailID = c.ReceiveDetailID.Value;
                //}

                receiveDetail.TransactionGroupID = tgroup.TransactionGroupID;
                receiveDetail.TransactionGroup = tgroup;
                receive.ReceiveDetails.Add(receiveDetail);



                //transaction for goods on hand // previously it was GOODS_ON_HAND_UNCOMMITED
                var transaction = new Transaction();
                transaction.TransactionID = Guid.NewGuid();
                transaction.TransactionGroupID = transactionGroupId;
                transaction.TransactionDate = DateTime.Now;
                transaction.ParentCommodityID = _unitOfWork.CommodityRepository.FindById(c.CommodityID).ParentID ?? c.CommodityID;
                transaction.CommodityID = c.CommodityID;
                transaction.LedgerID = Ledger.Constants.GOODS_ON_HAND;
                transaction.HubOwnerID = user.DefaultHubObj.HubOwnerID;

                transaction.DonorID = donorId;

                transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID);
                transaction.ShippingInstructionID = _shippingInstructionService.GetSINumberIdWithCreate(receiveModels.SINumber).ShippingInstructionID;

                transaction.ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(receiveModels.ProjectNumber).ProjectCodeID;
                transaction.HubID = user.DefaultHubObj.HubID;
                transaction.UnitID = c.UnitID;
                if (c.ReceivedQuantityInMT != null) transaction.QuantityInMT = transactionsign * c.ReceivedQuantityInMT.Value;
                if (c.ReceivedQuantityInUnit != null) transaction.QuantityInUnit = transactionsign * c.ReceivedQuantityInUnit.Value;
                if (c.CommodityGradeID != null) transaction.CommodityGradeID = c.CommodityGradeID.Value;

                transaction.ProgramID = receiveModels.ProgramID;
                transaction.StoreID = receiveModels.StoreID;
                transaction.Stack = receiveModels.StackNumber;
                transaction.TransactionGroupID = tgroup.TransactionGroupID;
                tgroup.Transactions.Add(transaction);



                var transaction2 = new Transaction();
                transaction2.TransactionID = Guid.NewGuid();
                transaction2.TransactionGroupID = transactionGroupId;
                transaction2.TransactionDate = DateTime.Now;

                transaction2.ParentCommodityID = transaction.ParentCommodityID;
                transaction2.CommodityID = c.CommodityID;
                transaction2.HubOwnerID = user.DefaultHubObj.HubOwnerID;

                transaction2.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                if (receive.ResponsibleDonorID != null)
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR, receive.ResponsibleDonorID.Value);

                //Decide from where the -ve side of the transaction comes from
                //it is either from the allocated stock
                // or it is from goods under care.

                // this means that this receipt is done without having gone through the gift certificate process.

                if (receiveModels.CommoditySourceID == CommoditySource.Constants.DONATION || receiveModels.CommoditySourceID == CommoditySource.Constants.LOCALPURCHASE)
                {
                    transaction2.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR, receive.ResponsibleDonorID.Value);
                }
                else if (receiveModels.CommoditySourceID == CommoditySource.Constants.REPAYMENT)
                {
                    transaction2.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receiveModels.SourceHubID.Value);
                }
                else
                {
                    transaction2.LedgerID = Ledger.Constants.LIABILITIES;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receiveModels.SourceHubID.Value);
                }

                transaction2.DonorID = donorId;

                transaction2.ShippingInstructionID = _shippingInstructionService.GetSINumberIdWithCreate(receiveModels.SINumber).ShippingInstructionID;
                transaction2.ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(receiveModels.ProjectNumber).ProjectCodeID;
                transaction2.HubID = user.DefaultHubObj.HubID;
                transaction2.UnitID = c.UnitID;
                // this is the credit part, so make it Negative
                if (c.ReceivedQuantityInMT != null) transaction2.QuantityInMT = transactionsign * (-c.ReceivedQuantityInMT.Value);
                if (c.ReceivedQuantityInUnit != null) transaction2.QuantityInUnit = transactionsign * (-c.ReceivedQuantityInUnit.Value);
                if (c.CommodityGradeID != null) transaction2.CommodityGradeID = c.CommodityGradeID.Value;

                transaction2.ProgramID = receiveModels.ProgramID;
                transaction2.StoreID = receiveModels.StoreID;
                transaction2.Stack = receiveModels.StackNumber;
                transaction2.TransactionGroupID = tgroup.TransactionGroupID;
                tgroup.Transactions.Add(transaction2);


                #region plan side of the transaction
                //transaction for statistics
                transaction = new Transaction();
                transaction.TransactionID = Guid.NewGuid();
                transaction.TransactionGroupID = transactionGroupId;
                transaction.TransactionDate = DateTime.Now;
                transaction.ParentCommodityID = _unitOfWork.CommodityRepository.FindById(c.CommodityID).ParentID ?? c.CommodityID;
                transaction.CommodityID = c.CommodityID;
                transaction.DonorID = donorId;
                transaction.LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK;
                transaction.HubOwnerID = user.DefaultHubObj.HubOwnerID;

                transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID);
                transaction.ShippingInstructionID = _shippingInstructionService.GetSINumberIdWithCreate(receiveModels.SINumber).ShippingInstructionID;

                transaction.ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(receiveModels.ProjectNumber).ProjectCodeID;
                transaction.HubID = user.DefaultHubObj.HubID;
                transaction.UnitID = c.UnitID;
                if (c.ReceivedQuantityInMT != null) transaction.QuantityInMT = transactionsign * c.ReceivedQuantityInMT.Value;
                if (c.ReceivedQuantityInUnit != null) transaction.QuantityInUnit = transactionsign * c.ReceivedQuantityInUnit.Value;
                if (c.CommodityGradeID != null) transaction.CommodityGradeID = c.CommodityGradeID.Value;

                transaction.ProgramID = receiveModels.ProgramID;
                transaction.StoreID = receiveModels.StoreID;
                transaction.Stack = receiveModels.StackNumber;
                transaction.TransactionGroupID = tgroup.TransactionGroupID;
                tgroup.Transactions.Add(transaction);


                // transaction for Receivable
                transaction2 = new Transaction();
                transaction2.TransactionID = Guid.NewGuid();
                transaction2.TransactionGroupID = transactionGroupId;
                transaction2.TransactionDate = DateTime.Now;
                //TAKEs the PARENT FROM THE FIRST TRANSACTION
                transaction2.ParentCommodityID = transaction.ParentCommodityID;
                transaction2.CommodityID = c.CommodityID;
                transaction2.DonorID = donorId;
                transaction2.HubOwnerID = user.DefaultHubObj.HubOwnerID;

                transaction2.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                if (receive.ResponsibleDonorID != null)
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR, receive.ResponsibleDonorID.Value);

                //Decide from where the -ve side of the transaction comes from
                //it is either from the allocated stock
                // or it is from goods under care.

                // this means that this receipt is done without having gone through the gift certificate process.

                #region "commented out"
                if (receiveModels.CommoditySourceID == CommoditySource.Constants.DONATION || receiveModels.CommoditySourceID == CommoditySource.Constants.LOCALPURCHASE)
                {
                    transaction2.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR, receive.ResponsibleDonorID.Value);
                }
                else if (receiveModels.CommoditySourceID == CommoditySource.Constants.REPAYMENT)
                {
                    transaction2.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receiveModels.SourceHubID.Value);
                }
                else
                {
                    transaction2.LedgerID = Ledger.Constants.LIABILITIES;
                    transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receiveModels.SourceHubID.Value);
                }
                #endregion



                transaction2.ShippingInstructionID = _shippingInstructionService.GetSINumberIdWithCreate(receiveModels.SINumber).ShippingInstructionID;
                transaction2.ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(receiveModels.ProjectNumber).ProjectCodeID;
                transaction2.HubID = user.DefaultHubObj.HubID;
                transaction2.UnitID = c.UnitID;
                // this is the credit part, so make it Negative
                if (c.ReceivedQuantityInMT != null) transaction2.QuantityInMT = transactionsign * (-c.ReceivedQuantityInMT.Value);
                if (c.ReceivedQuantityInUnit != null) transaction2.QuantityInUnit = transactionsign * (-c.ReceivedQuantityInUnit.Value);
                if (c.CommodityGradeID != null) transaction2.CommodityGradeID = c.CommodityGradeID.Value;

                transaction2.ProgramID = receiveModels.ProgramID;
                transaction2.StoreID = receiveModels.StoreID;
                transaction2.Stack = receiveModels.StackNumber;
                transaction2.TransactionGroupID = tgroup.TransactionGroupID;
                // hack to get past same key object in context error
                //repository.Transaction = new TransactionRepository();
                tgroup.Transactions.Add(transaction2);

                #endregion
            }


            try
            {
                if (!reverse)
                {
                    if (receiveModels.ReceiveID == null)
                    {
                        _unitOfWork.ReceiveRepository.Add(receive);
                    }
                    else
                    {
                        _unitOfWork.ReceiveRepository.Edit(receive);
                    }

                }


                _unitOfWork.Save();
                return true;
            }
            catch (Exception exp)
            {
                //TODO: Save the exception somewhere
                throw new Exception("The Receipt Transaction Cannot be saved. <br />Detail Message :" + exp.StackTrace);

            }

        }


        public bool ReceiptTransactionForLoanFromNGOs(ReceiveNewViewModel viewModel, Boolean reverse = false)
        {
            //Todo: Construct Receive from the viewModel .... refactor
            int transactionsign = reverse ? -1 : 1;

            #region BindReceiveFromViewModel

            Receive receive;

            if (viewModel.ReceiveId == Guid.Empty)
            {
                receive = new Receive();
                receive.ReceiveID = Guid.NewGuid();

            }
            else
            {
                receive = _unitOfWork.ReceiveRepository.FindById(viewModel.ReceiveId);
            }



            receive.GRN = viewModel.Grn;
            receive.CommodityTypeID = viewModel.CommodityTypeId;

            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;

            receive.TransporterID = viewModel.TransporterId > 0 ? viewModel.TransporterId : 1;
            receive.PlateNo_Prime = viewModel.PlateNoPrime;
            receive.PlateNo_Trailer = viewModel.PlateNoTrailer;
            receive.DriverName = viewModel.DriverName;
            receive.WeightBridgeTicketNumber = viewModel.WeightBridgeTicketNumber;
            receive.WeightBeforeUnloading = viewModel.WeightBeforeUnloading;
            receive.WeightAfterUnloading = viewModel.WeightAfterUnloading;

            receive.VesselName = viewModel.VesselName;
            receive.PortName = viewModel.PortName;

            receive.ReceiptDate = viewModel.ReceiptDate;
            receive.CreatedDate = DateTime.Now;
            receive.WayBillNo = viewModel.WayBillNo;
            receive.CommoditySourceID = viewModel.CommoditySourceTypeId;
            receive.ReceivedByStoreMan = viewModel.ReceivedByStoreMan;

            receive.PurchaseOrder = viewModel.PurchaseOrder;
            receive.SupplierName = viewModel.SupplierName;

            receive.Remark = viewModel.Remark;

            receive.ReceiptAllocationID = viewModel.ReceiptAllocationId;
            receive.HubID = viewModel.CurrentHub;
            receive.UserProfileID = viewModel.UserProfileId;
            receive.StoreId = viewModel.StoreId;
            receive.StackNumber = viewModel.StackNumber;
            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;



            #endregion

            //Todo: Construct ReceiveDetail from the viewModel Transaction

            var transactionGroup = new TransactionGroup { TransactionGroupID = Guid.NewGuid() };

            #region transaction for receiveDetail

            //foreach (var receiveDetailNewViewModel in viewModel.ReceiveDetailNewViewModels)
            //{
            //    ReceiveSingleTransaction(viewModel, receiveDetailNewViewModel, receive, transactionGroup);
            //}

            //Tem implantation for one Receive

            //check for non food

            #region

            if (viewModel.CommodityTypeId == 2)
            {
                viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt = 0;
                viewModel.ReceiveDetailNewViewModel.SentQuantityInMt = 0;
            }

            #endregion

            //Construct receive detail from viewModel

            #region

            var receiveDetail = new ReceiveDetail
            {
                ReceiveDetailID = Guid.NewGuid(), //Todo: if there is existing id dont give new one

                CommodityID = viewModel.ReceiveDetailNewViewModel.CommodityId,
                CommodityChildID = viewModel.ReceiveDetailNewViewModel.CommodityChildID,
                Description = viewModel.ReceiveDetailNewViewModel.Description,
                SentQuantityInMT = viewModel.ReceiveDetailNewViewModel.SentQuantityInMt,
                SentQuantityInUnit = viewModel.ReceiveDetailNewViewModel.SentQuantityInUnit,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                ReceiveID = receive.ReceiveID,
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionGroup = transactionGroup,

            };


            #endregion

            //add to receive
            receive.ReceiveDetails.Clear();
            receive.ReceiveDetails.Add(receiveDetail);

            var parentCommodityId =
                _unitOfWork.CommodityRepository.FindById(viewModel.ReceiveDetailNewViewModel.CommodityId).ParentID ??
                viewModel.ReceiveDetailNewViewModel.CommodityId;



            //get transactionGroup from a a loaned commodity so that we can deduct commodity amount using this transactionGroup.

            Guid? transactionGroupIdForLoan = _unitOfWork.ReceiveDetailRepository.FindBy(r => r.ReceiveID == viewModel.SelectedGRN).Select(
                   t => t.TransactionGroupID).FirstOrDefault();



            //physical stock movement

            #region

            //transaction for goods on hand

            #region On Positive Side

            var transactionOne = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.GOODS_ON_HAND,
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt,
                QuantityInUnit = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit,

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };
            transactionGroup.Transactions.Add(transactionOne);

            #endregion

            // transaction for goods under care, receivable, liabilities

            #region Negative Side

            var transactionTwo = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroupIdForLoan,// transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.GOODS_ON_HAND,//Ledger.Constants.GOODS_UNDER_CARE,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                        viewModel.SourceHubId.GetValueOrDefault(0)),
                //HubOwnerID =
                DonorID = receive.SourceDonorID, //

                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt),
                QuantityInUnit = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit),

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = true
            };

            //switch (viewModel.CommoditySourceTypeId)
            //{
            //    case CommoditySource.Constants.LOCALPURCHASE:
            //    case CommoditySource.Constants.DONATION:
            //        transactionTwo.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
            //        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
            //            receive.ResponsibleDonorID.GetValueOrDefault(0));
            //        break;
            //    case CommoditySource.Constants.REPAYMENT:
            //        transactionTwo.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
            //        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
            //            viewModel.SourceHubId.GetValueOrDefault(0));
            //        break;
            //    default:
            //        transactionTwo.LedgerID = Ledger.Constants.LIABILITIES;
            //        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
            //            viewModel.SourceHubId.GetValueOrDefault(0));
            //        break;
            //}

            transactionGroup.Transactions.Add(transactionTwo);

            #endregion

            #endregion

            // plan side

            #region

            #region Positive Side

            //statstics free

            var transactionThree = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK,
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt,
                QuantityInUnit = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit,

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };

            transactionGroup.Transactions.Add(transactionThree);

            #endregion

            #region Negative Side

            var transactionFour = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroupIdForLoan,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                    viewModel.SourceHubId.GetValueOrDefault(0)),
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt),
                QuantityInUnit = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit),

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = true// viewModel.IsFalseGRN
            };

            //if (transactionFour.CommoditySourceID == CommoditySource.Constants.DONATION ||
            //    viewModel.CommoditySourceTypeId == CommoditySource.Constants.LOCALPURCHASE)
            //{
            //    transactionFour.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
            //    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
            //        receive.ResponsibleDonorID.GetValueOrDefault(0));
            //}
            //else if (transactionFour.CommoditySourceID == CommoditySource.Constants.REPAYMENT)
            //{
            //    transactionFour.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
            //    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
            //        viewModel.SourceHubId.GetValueOrDefault(0));
            //}
            //else
            //{
            //    transactionFour.LedgerID = Ledger.Constants.LIABILITIES;
            //    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
            //        viewModel.SourceHubId.GetValueOrDefault(0));
            //}

            transactionGroup.Transactions.Add(transactionFour);

            #endregion

            #endregion

            #endregion

            //Todo: Save Receive

            try
            {
                if (!reverse)
                {
                    if (viewModel.ReceiveId == Guid.Empty)
                    {
                        _unitOfWork.ReceiveRepository.Add(receive);
                    }
                    else
                    {
                        _unitOfWork.ReceiveRepository.Edit(receive);
                    }

                }


                _unitOfWork.Save();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool ReceiptDetailsTransaction(ReceiveNewViewModel viewModel, Boolean reverse = false, bool isOnEdit = false)
        {

            //Todo: Construct Receive from the viewModel .... refactor
            int transactionsign = reverse ? -1 : 1;

            #region BindReceiveFromViewModel

            Receive receive;

            if (viewModel.ReceiveId == Guid.Empty)
            {
                receive = new Receive();
                receive.ReceiveID = Guid.NewGuid();

            }
            else
            {
                receive = _unitOfWork.ReceiveRepository.FindById(viewModel.ReceiveId);
            }



            receive.GRN = viewModel.Grn;
            receive.CommodityTypeID = viewModel.CommodityTypeId;

            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;

            receive.TransporterID = viewModel.TransporterId > 0 ? viewModel.TransporterId : 1;
            receive.PlateNo_Prime = viewModel.PlateNoPrime;
            receive.PlateNo_Trailer = viewModel.PlateNoTrailer;
            receive.DriverName = viewModel.DriverName;
            receive.WeightBridgeTicketNumber = viewModel.WeightBridgeTicketNumber;
            receive.WeightBeforeUnloading = viewModel.WeightBeforeUnloading;
            receive.WeightAfterUnloading = viewModel.WeightAfterUnloading;

            receive.VesselName = viewModel.VesselName;
            receive.PortName = viewModel.PortName;

            receive.ReceiptDate = viewModel.ReceiptDate;
            receive.CreatedDate = DateTime.Now;
            receive.WayBillNo = viewModel.WayBillNo;
            receive.CommoditySourceID = viewModel.CommoditySourceTypeId;
            receive.ReceivedByStoreMan = viewModel.ReceivedByStoreMan;

            receive.PurchaseOrder = viewModel.PurchaseOrder;
            receive.SupplierName = viewModel.SupplierName;

            receive.Remark = viewModel.Remark;

            receive.ReceiptAllocationID = viewModel.ReceiptAllocationId;
            receive.HubID = viewModel.CurrentHub;
            receive.UserProfileID = viewModel.UserProfileId;
            receive.StoreId = viewModel.StoreId;
            receive.StackNumber = viewModel.StackNumber;
            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;

            if (isOnEdit)
            {
                var receiveDetailsId = viewModel.ReceiveDetailsViewModels[0].ReceiveDetailsId;
                if (receiveDetailsId != null)
                {
                    var firstOrDefault = receive.ReceiveDetails.FirstOrDefault(rd => rd.ReceiveDetailID == receiveDetailsId);
                    if (firstOrDefault != null)
                    {
                        var transactionGroupId = firstOrDefault.TransactionGroupID;
                        if (transactionGroupId != null)
                        {
                            var transactionGroupIds = (Guid)transactionGroupId;
                            var tansacationIds = Get(t => t.TransactionGroupID == transactionGroupIds).Select(t => t.TransactionID).ToList();
                            foreach (var tansacationId in tansacationIds)
                            {
                                DeleteById(tansacationId);
                            }
                        }
                    }
                }
            }

            #endregion

            //Todo: Construct ReceiveDetail from the viewModel Transaction

            TransactionGroup transactionGroup = new TransactionGroup { TransactionGroupID = Guid.NewGuid() };

            #region transaction for receiveDetail

            //foreach (var receiveDetailNewViewModel in viewModel.ReceiveDetailNewViewModels)
            //{
            //    ReceiveSingleTransaction(viewModel, receiveDetailNewViewModel, receive, transactionGroup);
            //}

            //Tem implantation for one Receive

            //check for non food

            #region

            foreach (var receiveDetailsViewModel in viewModel.ReceiveDetailsViewModels)
            {
                if (viewModel.CommodityTypeId == 2)
                {
                    receiveDetailsViewModel.ReceivedQuantityInMt = 0;
                    receiveDetailsViewModel.SentQuantityInMt = 0;
                }
                ReceiveDetail receiveDetail;
                if (receiveDetailsViewModel.ReceiveDetailsId.HasValue)
                {
                    var model = receiveDetailsViewModel;
                    receiveDetail = receive.ReceiveDetails.FirstOrDefault(r => r.ReceiveDetailID == model.ReceiveDetailsId);
                    receive.ReceiveDetails.Remove(receiveDetail);

                }

                receiveDetail = new ReceiveDetail
                {
                    //, //Todo: if there is existing id dont give new one

                    ReceiveDetailID = Guid.NewGuid(),
                    CommodityID = receiveDetailsViewModel.CommodityId,
                    CommodityChildID = receiveDetailsViewModel.CommodityChildID,
                    Description = receiveDetailsViewModel.Description,
                    SentQuantityInMT = receiveDetailsViewModel.SentQuantityInMt,
                    SentQuantityInUnit = receiveDetailsViewModel.SentQuantityInUnit,
                    UnitID = receiveDetailsViewModel.UnitId,
                    ReceiveID = receive.ReceiveID,
                    TransactionGroupID = transactionGroup.TransactionGroupID,
                    TransactionGroup = transactionGroup,
                    SiNumber = receiveDetailsViewModel.SiNumber ?? 0

                };
                //add to receive
                receive.ReceiveDetails.Add(receiveDetail);


                //var parentCommodityId =
                //    _unitOfWork.CommodityRepository.FindById(viewModel.ReceiveDetailNewViewModel.CommodityId).ParentID ??
                //    viewModel.ReceiveDetailNewViewModel.CommodityId;

                //physical stock movement



                //transaction for goods on hand

                #region On Positive Side

                var siNumber = "";
                var firstOrDefault = _unitOfWork.ReceiveRepository.Get(r => r.ReceiveID == receiveDetail.ReceiveID, null, "ReceiptAllocation").FirstOrDefault();
                if (
                    firstOrDefault != null)
                {
                    siNumber = firstOrDefault.ReceiptAllocation.SINumber;
                }
                var transactionOne = new Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup.TransactionGroupID,
                    TransactionDate = DateTime.Now,
                    ParentCommodityID = null,
                    CommodityID = receiveDetail.CommodityID,
                    CommodityChildID = receiveDetail.CommodityChildID,
                    LedgerID = Ledger.Constants.GOODS_ON_HAND,
                    //HubOwnerID =
                    DonorID = receive.SourceDonorID,
                    AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                    ShippingInstructionID =
                        _shippingInstructionService.GetSINumberIdWithCreate(siNumber)
                            .ShippingInstructionID,
                    ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                    HubID = viewModel.CurrentHub,
                    UnitID = receiveDetailsViewModel.UnitId,
                    QuantityInMT = transactionsign * receiveDetailsViewModel.ReceivedQuantityInMt,
                    QuantityInUnit = transactionsign * receiveDetailsViewModel.ReceivedQuantityInUnit,

                    //CommodityGradeID =
                    ProgramID = viewModel.ProgramId,
                    StoreID = viewModel.StoreId,
                    Stack = viewModel.StackNumber,
                    IsFalseGRN = viewModel.IsFalseGRN
                };
                transactionGroup.Transactions.Add(transactionOne);

                #endregion

                // transaction for goods under care, receivable, liabilities

                #region Negative Side

                var transactionTwo = new Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup.TransactionGroupID,
                    TransactionDate = DateTime.Now,
                    ParentCommodityID = null,
                    CommodityID = receiveDetail.CommodityID,
                    CommodityChildID = receiveDetail.CommodityChildID,
                    LedgerID = Ledger.Constants.GOODS_UNDER_CARE,

                    //HubOwnerID =
                    DonorID = receive.SourceDonorID, //

                    ShippingInstructionID =
                        _shippingInstructionService.GetSINumberIdWithCreate(siNumber)
                            .ShippingInstructionID,
                    ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                    HubID = viewModel.CurrentHub,
                    UnitID = receiveDetailsViewModel.UnitId,
                    QuantityInMT = transactionsign * (-receiveDetailsViewModel.ReceivedQuantityInMt),
                    QuantityInUnit = transactionsign * (-receiveDetailsViewModel.ReceivedQuantityInUnit),

                    //CommodityGradeID =
                    ProgramID = viewModel.ProgramId,
                    StoreID = viewModel.StoreId,
                    Stack = viewModel.StackNumber,
                    IsFalseGRN = viewModel.IsFalseGRN
                };

                switch (viewModel.CommoditySourceTypeId)
                {
                    case CommoditySource.Constants.LOCALPURCHASE:
                    case CommoditySource.Constants.DONATION:
                        transactionTwo.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
                            receive.ResponsibleDonorID.GetValueOrDefault(0));
                        break;
                    case CommoditySource.Constants.REPAYMENT:
                        transactionTwo.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                            viewModel.SourceHubId.GetValueOrDefault(0));
                        break;
                    default:
                        transactionTwo.LedgerID = Ledger.Constants.LIABILITIES;
                        transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                            viewModel.SourceHubId.GetValueOrDefault(0));
                        break;
                }

                transactionGroup.Transactions.Add(transactionTwo);

                #endregion

                #endregion

                // plan side

                #region

                #region Positive Side

                //statstics free

                var transactionThree = new Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup.TransactionGroupID,
                    TransactionDate = DateTime.Now,
                    ParentCommodityID = null,
                    CommodityID = receiveDetail.CommodityID,
                    CommodityChildID = receiveDetail.CommodityChildID,
                    LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK,
                    //HubOwnerID =
                    DonorID = receive.SourceDonorID,
                    AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                    ShippingInstructionID =
                        _shippingInstructionService.GetSINumberIdWithCreate(siNumber)
                            .ShippingInstructionID,
                    ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                    HubID = viewModel.CurrentHub,
                    UnitID = receiveDetailsViewModel.UnitId,
                    QuantityInMT = transactionsign * receiveDetailsViewModel.ReceivedQuantityInMt,
                    QuantityInUnit = transactionsign * receiveDetailsViewModel.ReceivedQuantityInUnit,

                    //CommodityGradeID =
                    ProgramID = viewModel.ProgramId,
                    StoreID = viewModel.StoreId,
                    Stack = viewModel.StackNumber,
                    IsFalseGRN = viewModel.IsFalseGRN
                };

                transactionGroup.Transactions.Add(transactionThree);

                #endregion

                #region Negative Side

                var transactionFour = new Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup.TransactionGroupID,
                    TransactionDate = DateTime.Now,
                    ParentCommodityID = null,
                    CommodityID = receiveDetail.CommodityID,
                    CommodityChildID = receiveDetail.CommodityChildID,
                    //HubOwnerID =
                    DonorID = receive.SourceDonorID,
                    ShippingInstructionID =
                        _shippingInstructionService.GetSINumberIdWithCreate(siNumber)
                            .ShippingInstructionID,
                    ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                    HubID = viewModel.CurrentHub,
                    UnitID = receiveDetailsViewModel.UnitId,
                    QuantityInMT = transactionsign * (-receiveDetailsViewModel.ReceivedQuantityInMt),
                    QuantityInUnit = transactionsign * (-receiveDetailsViewModel.ReceivedQuantityInUnit),

                    //CommodityGradeID =
                    ProgramID = viewModel.ProgramId,
                    StoreID = viewModel.StoreId,
                    Stack = viewModel.StackNumber,
                    IsFalseGRN = viewModel.IsFalseGRN
                };

                if (transactionFour.CommoditySourceID == CommoditySource.Constants.DONATION ||
                    viewModel.CommoditySourceTypeId == CommoditySource.Constants.LOCALPURCHASE)
                {
                    transactionFour.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
                        receive.ResponsibleDonorID.GetValueOrDefault(0));
                }
                else if (transactionFour.CommoditySourceID == CommoditySource.Constants.REPAYMENT)
                {
                    transactionFour.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                        viewModel.SourceHubId.GetValueOrDefault(0));
                }
                else
                {
                    transactionFour.LedgerID = Ledger.Constants.LIABILITIES;
                    transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                        viewModel.SourceHubId.GetValueOrDefault(0));
                }

                transactionGroup.Transactions.Add(transactionFour);

                #endregion

                #endregion

                #endregion

                //Todo: Save Receive

                try
                {
                    if (!reverse)
                    {
                        if (viewModel.ReceiveId == Guid.Empty)
                        {
                            _unitOfWork.ReceiveRepository.Add(receive);
                        }
                        else
                        {
                            _unitOfWork.ReceiveRepository.Edit(receive);
                        }

                    }


                    _unitOfWork.Save();
                    return true;
                }
                catch (Exception exception)
                {
                    throw exception;
                }

            }
            return true;

        }
        public bool ReceiptTransaction(ReceiveNewViewModel viewModel, Boolean reverse = false)
        {
            //Todo: Construct Receive from the viewModel .... refactor
            int transactionsign = reverse ? -1 : 1;
            bool canCreateTransaction = false;
            bool existed = false;
            if (!reverse)
            {
                canCreateTransaction = CanCreateTransaction(viewModel);
            }
            #region BindReceiveFromViewModel

            Receive receive = new Receive();

            if (viewModel.ReceiveId == Guid.Empty)
            {
                receive.ReceiveID = Guid.NewGuid();
            }
            else
            {
                receive = _unitOfWork.ReceiveRepository.FindById(viewModel.ReceiveId);
                if (receive != null) existed = true;
            }
            if (receive == null) receive = new Receive();

            receive.ReceiveID = viewModel.ReceiveId;
            receive.GRN = viewModel.Grn;
            receive.CommodityTypeID = viewModel.CommodityTypeId;

            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;

            receive.TransporterID = viewModel.TransporterId > 0 ? viewModel.TransporterId : 1;
            receive.PlateNo_Prime = viewModel.PlateNoPrime;
            receive.PlateNo_Trailer = viewModel.PlateNoTrailer;
            receive.DriverName = viewModel.DriverName;
            receive.WeightBridgeTicketNumber = viewModel.WeightBridgeTicketNumber;
            receive.WeightBeforeUnloading = viewModel.WeightBeforeUnloading;
            receive.WeightAfterUnloading = viewModel.WeightAfterUnloading;

            receive.VesselName = viewModel.VesselName;
            receive.PortName = viewModel.PortName;

            receive.ReceiptDate = viewModel.ReceiptDate;
            receive.CreatedDate = DateTime.Now;
            receive.WayBillNo = viewModel.WayBillNo;
            receive.CommoditySourceID = viewModel.CommoditySourceTypeId;
            receive.ReceivedByStoreMan = viewModel.ReceivedByStoreMan;

            receive.PurchaseOrder = viewModel.PurchaseOrder;
            receive.SupplierName = viewModel.SupplierName;

            receive.Remark = viewModel.Remark;

            receive.ReceiptAllocationID = viewModel.ReceiptAllocationId;
            receive.HubID = viewModel.CurrentHub;
            receive.UserProfileID = viewModel.UserProfileId;
            receive.StoreId = viewModel.StoreId;
            receive.StackNumber = viewModel.StackNumber;
            receive.SourceDonorID = viewModel.SourceDonorId;
            receive.ResponsibleDonorID = viewModel.ResponsibleDonorId;
            receive.BusinessProcessID = viewModel.BusinessProcessID;



            #endregion
            #region

            if (viewModel.CommodityTypeId == 2)
            {
                viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt = 0;
                viewModel.ReceiveDetailNewViewModel.SentQuantityInMt = 0;
            }

            #endregion

            //receive.ReceiveDetails.Clear();

            if (!existed) //viewModel.ReceiveId == Guid.Empty
            {

                if (viewModel.ReceiveDetailNewViewModel.ReceiveDetailId != Guid.Empty) //||     viewModel.ReceiveDetailNewViewModel.ReceiveDetailId != null

                {
                    var receiveDetail = new ReceiveDetail
                    {
                        ReceiveDetailID = viewModel.ReceiveDetailNewViewModel.ReceiveDetailId,
                        //Guid.NewGuid(), //Todo: if there is existing id dont give new one

                        CommodityID = viewModel.ReceiveDetailNewViewModel.CommodityId,
                        CommodityChildID = viewModel.ReceiveDetailNewViewModel.CommodityChildID,
                        Description = viewModel.ReceiveDetailNewViewModel.Description,
                        SentQuantityInMT = viewModel.ReceiveDetailNewViewModel.SentQuantityInMt,
                        SentQuantityInUnit = viewModel.ReceiveDetailNewViewModel.SentQuantityInUnit,
                        UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                        ReceiveID = receive.ReceiveID,


                    };

                    CreateTransaction(viewModel, transactionsign, receive, receiveDetail);

                    //add to receive
                    receive.ReceiveDetails.Clear();
                    receive.ReceiveDetails.Add(receiveDetail);
                }
            }
            else
            {
                foreach (var receiveDetailModel in viewModel.ReceiveDetailsViewModels)
                {
                    //var receiveDetail = reciev

                    if (receiveDetailModel.ReceiveDetailsId == null) continue;

                    var receiveDetail = new ReceiveDetail
                    {
                        ReceiveDetailID = (Guid)receiveDetailModel.ReceiveDetailsId,

                        CommodityID = receiveDetailModel.CommodityId,
                        CommodityChildID = receiveDetailModel.CommodityChildID,
                        Description = receiveDetailModel.Description,
                        SentQuantityInMT = receiveDetailModel.SentQuantityInMt,
                        SentQuantityInUnit = receiveDetailModel.SentQuantityInUnit,
                        UnitID = receiveDetailModel.UnitId,
                        ReceiveID = receive.ReceiveID

                    };



                    try
                    {
                        if (reverse)
                            CreateTransaction(viewModel, transactionsign, receive, receiveDetail);

                        if (!reverse)
                        {
                            if (viewModel.ReceiveId == Guid.Empty)
                            {
                                CreateTransaction(viewModel, transactionsign, receive, receiveDetail);

                            }
                            else
                            {
                                //CHECK IF FIELDS THAT AFFECT TRANSACTION TBL ARE CHANGED , IF CHANGED CREATE NEW TRANSACTION GROUP
                                if (canCreateTransaction)
                                {
                                    CreateTransaction(viewModel, transactionsign, receive, receiveDetail);
                                }
                            }

                        }



                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }

                }
            }

            try
            {

                if (!reverse)
                {
                    //var dbRecieve = _unitOfWork.ReceiveRepository.FindById(viewModel.ReceiveId);
                    if (!existed)
                    {
                        _unitOfWork.ReceiveRepository.Add(receive);
                    }
                    else
                    {
                        _unitOfWork.ReceiveRepository.Edit(receive);
                    }

                }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool RemoveExistingDetails(List<ReceiveDetail> detailsList)
        {
            foreach (var datail in detailsList)
            {
                var recieveDetail = _unitOfWork.ReceiveDetailRepository.FindById(datail.ReceiveDetailID);
                _unitOfWork.ReceiveDetailRepository.Delete(recieveDetail);
            }
            _unitOfWork.Save();
            return true;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="ediedReceive">Edited receive object</param>
        /// <returns></returns>
        private bool CanCreateTransaction(ReceiveNewViewModel editedReceiveNewViewModel)
        {

            if (editedReceiveNewViewModel.ReceiveId == Guid.Empty)
            {
                return true;
            }
            var origionalReceive = _unitOfWork.ReceiveRepository.FindById(editedReceiveNewViewModel.ReceiveId);
            if (origionalReceive == null) return true;
            //compare
            // return true;

            bool result = false;

            //if store is changed Create transaction
            if (editedReceiveNewViewModel.StoreId != origionalReceive.StoreId)
            {
                result = true;
                return result;

            }

            //if stack is changed create transaction
            if (editedReceiveNewViewModel.StackNumber != origionalReceive.StackNumber)
            {
                result = true;
                return result;

            }

            //if transporter is changed
            if (editedReceiveNewViewModel.TransporterId != origionalReceive.TransporterID)
            {
                result = true;
                return result;

            }

            //If unit is changed

            if (editedReceiveNewViewModel.CurrentHub != origionalReceive.HubID)
            {
                result = true;
                return result;

            }
            //    editedReceiveNewViewModel.ReceiveDetailNewViewModels = new List<ReceiveDetailNewViewModel>();

            //if (editedReceiveNewViewModel.ReceiveDetailNewViewModel != null)
            foreach (var editedRecDetails in editedReceiveNewViewModel.ReceiveDetailsViewModels)
            {
                // ReceiveDetailNewViewModel editedRecDetails = editedReceiveNewViewModel.ReceiveDetailNewViewModel;


                var recDetailCommodities =
                    origionalReceive.ReceiveDetails.Where(
                        u =>
                            u.CommodityID == editedRecDetails.CommodityId &&
                            u.CommodityChildID == editedRecDetails.CommodityChildID);

                //there is no comodity id and CommodityChildID
                if (recDetailCommodities.Any() && !origionalReceive.ReceiveDetails.Any()) { result = true; return result; }
                if (!recDetailCommodities.Any() && origionalReceive.ReceiveDetails.Any()) { result = true; return result; }



                if (editedRecDetails.UnitId != recDetailCommodities.FirstOrDefault().UnitID)
                {
                    result = true;
                    return result;

                }
                if (editedRecDetails.ReceivedQuantityInMt != recDetailCommodities.FirstOrDefault().QuantityInMT)
                {
                    result = true;
                    return result;

                }
                if (editedRecDetails.ReceivedQuantityInUnit != recDetailCommodities.FirstOrDefault().QuantityInUnit)
                {
                    result = true;
                    return result;

                }

            }



            return result;
        }


        private void CreateTransaction(ReceiveNewViewModel viewModel, int transactionsign, Receive receive, ReceiveDetail receiveDetail)
        {
            #region transaction for receiveDetail

            //Construct receive detail from viewModel
            var transactionGroup = new TransactionGroup { TransactionGroupID = Guid.NewGuid() };


            receiveDetail.TransactionGroupID = transactionGroup.TransactionGroupID;
            receiveDetail.TransactionGroup = transactionGroup;

            //physical stock movement

            #region

            //transaction for goods on hand

            #region On Positive Side

            var transactionOne = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.GOODS_ON_HAND,
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt,
                QuantityInUnit = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit,

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };
            transactionGroup.Transactions.Add(transactionOne);

            #endregion

            // transaction for goods under care, receivable, liabilities

            #region Negative Side

            var transactionTwo = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.GOODS_UNDER_CARE,

                //HubOwnerID =
                DonorID = receive.SourceDonorID, //

                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt),
                QuantityInUnit = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit),

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };

            switch (viewModel.CommoditySourceTypeId)
            {
                case CommoditySource.Constants.LOCALPURCHASE:
                case CommoditySource.Constants.DONATION:
                    transactionTwo.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                    transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
                        receive.ResponsibleDonorID.GetValueOrDefault(0));
                    break;
                case CommoditySource.Constants.REPAYMENT:
                    transactionTwo.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                    transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                        viewModel.SourceHubId.GetValueOrDefault(0));
                    break;
                default:
                    transactionTwo.LedgerID = Ledger.Constants.LIABILITIES;
                    transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                        viewModel.SourceHubId.GetValueOrDefault(0));
                    break;
            }

            transactionGroup.Transactions.Add(transactionTwo);

            #endregion

            #endregion

            // plan side

            #region

            #region Positive Side

            //statstics free

            var transactionThree = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK,
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, receive.HubID),
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt,
                QuantityInUnit = transactionsign * viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit,

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };

            transactionGroup.Transactions.Add(transactionThree);

            #endregion

            #region Negative Side

            var transactionFour = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                TransactionGroupID = transactionGroup.TransactionGroupID,
                TransactionDate = DateTime.Now,
                ParentCommodityID = null,
                CommodityID = receiveDetail.CommodityID,
                CommodityChildID = receiveDetail.CommodityChildID,
                //HubOwnerID =
                DonorID = receive.SourceDonorID,
                ShippingInstructionID =
                    _shippingInstructionService.GetSINumberIdWithCreate(viewModel.SiNumber).ShippingInstructionID,
                ProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(viewModel.ProjectCode).ProjectCodeID,
                HubID = viewModel.CurrentHub,
                UnitID = viewModel.ReceiveDetailNewViewModel.UnitId,
                QuantityInMT = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt),
                QuantityInUnit = transactionsign * (-viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit),

                //CommodityGradeID =
                ProgramID = viewModel.ProgramId,
                StoreID = viewModel.StoreId,
                Stack = viewModel.StackNumber,
                IsFalseGRN = viewModel.IsFalseGRN
            };

            if (transactionFour.CommoditySourceID == CommoditySource.Constants.DONATION ||
                viewModel.CommoditySourceTypeId == CommoditySource.Constants.LOCALPURCHASE)
            {
                transactionFour.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
                transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR,
                    receive.ResponsibleDonorID.GetValueOrDefault(0));
            }
            else if (transactionFour.CommoditySourceID == CommoditySource.Constants.REPAYMENT)
            {
                transactionFour.LedgerID = Ledger.Constants.GOODS_RECIEVABLE;
                transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                    viewModel.SourceHubId.GetValueOrDefault(0));
            }
            else
            {
                transactionFour.LedgerID = Ledger.Constants.LIABILITIES;
                transactionFour.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB,
                    viewModel.SourceHubId.GetValueOrDefault(0));
            }

            transactionGroup.Transactions.Add(transactionFour);

            #endregion

            #endregion

            #endregion
        }

        /// <summary>
        /// Gets the transportation reports.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public List<TransporationReport> GetTransportationReports(OperationMode mode, DateTime? fromDate, DateTime? toDate)
        {
            int ledgerId = (mode == OperationMode.Dispatch) ? Ledger.Constants.GOODS_IN_TRANSIT : Ledger.Constants.GOODS_ON_HAND;
            var list = _unitOfWork.TransactionRepository.Get(item =>
                        (item.LedgerID == ledgerId && (item.QuantityInMT > 0 || item.QuantityInUnit > 0))
                              &&
                              (item.TransactionGroup.DispatchDetails.Any() || item.TransactionGroup.ReceiveDetails.Any())
                              &&
                              (!item.TransactionGroup.InternalMovements.Any() || !item.TransactionGroup.Adjustments.Any())
                       );

            if (fromDate.HasValue)
            {
                list = list.Where(p => p.TransactionDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                list = list.Where(p => p.TransactionDate <= toDate.Value);
            }

            return (from t in list
                    group t by new { t.Commodity, t.Program } into tgroup
                    select new TransporationReport()
                    {
                        Commodity = tgroup.Key.Commodity.Name,
                        Program = tgroup.Key.Program.Name,
                        NoOfTrucks = tgroup.Count(),
                        QuantityInMT = tgroup.Sum(p => p.QuantityInMT),
                        QuantityInUnit = tgroup.Sum(p => p.QuantityInUnit)
                    }).ToList();

        }

        /// <summary>
        /// Gets the grouped transportation reports.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public List<GroupedTransportation> GetGroupedTransportationReports(OperationMode mode, DateTime? fromDate, DateTime? toDate)
        {
            var list = (from tr in GetTransportationReports(mode, fromDate, toDate)
                        group tr by tr.Program into trg
                        select new GroupedTransportation()
                        {
                            Program = trg.Key,
                            Transportations = trg.ToList()
                        });
            return list.ToList(); ;
        }


        /// <summary>
        /// Gets the available allocations.
        /// </summary>
        /// <param name="hubId">The hub id.</param>
        /// <returns></returns>
        public List<ReceiptAllocation> GetAvailableAllocations(int hubId)
        {
            var avaliableRAll =
                _unitOfWork.ReceiptAllocationRepository.FindBy(
                    t => t.QuantityInMT >= (_unitOfWork.TransactionRepository.FindBy(v =>
                                                                                      v.ShippingInstruction.Value ==
                                                                                      t.SINumber
                                                                                      && v.HubID == hubId
                                                                                      &&
                                                                                      v.LedgerID ==
                                                                                      Ledger.Constants.
                                                                                          GOODS_ON_HAND_UNCOMMITED
                                                                                      && v.QuantityInMT > 0).Select(
                                                                                          v => v.QuantityInMT).Sum()));
            //var avaliableRAll = (from rAll in db.ReceiptAllocations
            //                     where rAll.QuantityInMT >= (from v in db.Transactions
            //                                                 where v.ShippingInstruction.Value == rAll.SINumber
            //                                                       && v.HubID == hubId
            //                                                       && v.LedgerID == Ledger.Constants.GOODS_ON_HAND_UNCOMMITED
            //                                                       && v.QuantityInMT > 0
            //                                                 select v.QuantityInMT).Sum()
            //                     select rAll);

            return avaliableRAll.ToList();
        }


        /// <summary>
        /// Saves the dispatch transaction.
        /// </summary>
        /// <param name="dispatchModel">The dispatch model.</param>
        /// <param name="user">The user.</param>
        public bool SaveDispatchTransaction(DispatchModel dispatchModel, UserProfile user) //used to return void
        {
            Dispatch dispatch = dispatchModel.GenerateDipatch(user);
            dispatch.DispatchID = Guid.NewGuid();
            dispatch.HubID = user.DefaultHub.Value;
            dispatch.UserProfileID = user.UserProfileID;
            dispatch.DispatchAllocationID = dispatchModel.DispatchAllocationID;
            dispatch.OtherDispatchAllocationID = dispatchModel.OtherDispatchAllocationID;
            CommodityType commType = _unitOfWork.CommodityTypeRepository.FindById(dispatchModel.CommodityTypeID);

            foreach (DispatchDetailModel detail in dispatchModel.DispatchDetails)
            {

                if (commType.CommodityTypeID == 2)//if it's a non food
                {
                    detail.DispatchedQuantityMT = 0;
                    detail.RequestedQuantityMT = 0;
                }

                TransactionGroup group = new TransactionGroup();
                group.TransactionGroupID = Guid.NewGuid();
                if (dispatchModel.Type == 1)
                {
                    Transaction transaction = GetGoodsOnHandFDPTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction);

                    Transaction transaction2 = GetGoodsInTransitFDPTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction2);

                    Transaction transaction3 = GetStatisticsFDPTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction3);

                    Transaction transaction4 = GetCommitedToFDPTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction4);
                }
                else
                {
                    Transaction transaction = GetGoodsOnHandHUBTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction);

                    Transaction transaction2 = GetGoodInTransitHUBTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction2);

                    Transaction transaction3 = GetStatisticsHUBTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction3);

                    Transaction transaction4 = GetCommittedToFDPHUBTransaction(dispatchModel, dispatch, detail);
                    group.Transactions.Add(transaction4);
                }

                DispatchDetail dispatchDetail = GenerateDispatchDetail(detail);
                dispatchDetail.DispatchDetailID = Guid.NewGuid();
                dispatchDetail.TransactionGroup = group;


                dispatch.DispatchDetails.Add(dispatchDetail);

            }
            // Try to save this transaction
            //    db.Database.Connection.Open();
            //  DbTransaction dbTransaction = db.Database.Connection.BeginTransaction();
            try
            {
                _unitOfWork.DispatchRepository.Add(dispatch);

                _IWorkflowActivityService.EnterCreateWorkflow(dispatch);

                _unitOfWork.Save();
                return true;
                //repository.Dispatch.Add(dispatch);
                //dbTransaction.Commit();
            }
            catch (Exception exp)
            {
                // dbTransaction.Rollback();
                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Dispatch Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }

            if (dispatch.Type == 1)
            {
                string sms = dispatch.GetSMSText();
                //SMS.SendSMS(dispatch.FDPID.Value, sms);
            }
            return false;
        }
        public bool PostSIAllocation(int requisitionID, int siPCAllocationID)
        {
            var allocationDetails = _unitOfWorkNew.SIPCAllocationRepository.Get(t => t.ReliefRequisitionDetail.RequisitionID == requisitionID, null, "ReliefRequisitionDetail");
            allocationDetails = allocationDetails.Where(t => t.SIPCAllocationID == siPCAllocationID);
            if (allocationDetails == null) return false;
            var d = new daModel.TransactionGroup();
            var transactionGroup = Guid.NewGuid();
            var transactionDate = DateTime.Now;
            _unitOfWorkNew.TransactionGroupRepository.Add(new daModel.TransactionGroup { PartitionID = 0, TransactionGroupID = transactionGroup });

            //ProjectCodeID	ShippingInstructionID ProgramID QuantityInMT	QuantityInUnit	UnitID	TransactionDate	RegionID	Month	Round	DonorID	CommoditySourceID	GiftTypeID	FDP

            foreach (var allocationDetail in allocationDetails)
            {
                if (allocationDetail.TransactionGroup == null && allocationDetail.ReliefRequisitionDetail != null)
                {
                    var transaction = new Models.Transaction
                    {
                        TransactionID = Guid.NewGuid(),
                        TransactionGroupID = transactionGroup,
                        TransactionDate = transactionDate,
                        UnitID = 1
                    };

                    var allocation = allocationDetail;




                    transaction.QuantityInMT = -allocationDetail.AllocatedAmount;
                    transaction.QuantityInUnit = -allocationDetail.AllocatedAmount;
                    transaction.LedgerID = Ledger.Constants.COMMITED_TO_FDP;
                    if (allocationDetail.ReliefRequisitionDetail.CommodityID != 0)
                        transaction.CommodityID = allocationDetail.ReliefRequisitionDetail.CommodityID;
                    transaction.ParentCommodityID = allocationDetail.ReliefRequisitionDetail.Commodity.ParentID;
                    transaction.FDPID = allocationDetail.ReliefRequisitionDetail.FDPID;
                    transaction.ProgramID = (int)allocationDetail.ReliefRequisitionDetail.ReliefRequisition.ProgramID;
                    transaction.RegionID = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionID;
                    transaction.PlanId = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionalRequest.PlanID;
                    transaction.Round = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.Round;

                    //int hubID1 = 0;
                    if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                    {
                        transaction.ShippingInstructionID = allocationDetail.Code;
                        //hubID1 =
                        //    (int)
                        //        _unitOfWorkNew.TransactionRepository.FindBy(
                        //            m =>
                        //                m.ShippingInstructionID == allocationDetail.Code &&
                        //                m.LedgerID == Ledger.Constants.GOODS_ON_HAND).Select(m => m.HubID).FirstOrDefault();
                    }
                    else
                    {
                        transaction.ProjectCodeID = allocationDetail.Code;
                        //hubID1 =
                        //    (int)
                        //        _unitOfWorkNew.TransactionRepository.FindBy(
                        //            m =>
                        //                m.ProjectCodeID == allocationDetail.Code &&
                        //                m.LedgerID == Ledger.Constants.GOODS_ON_HAND).Select(m => m.HubID).FirstOrDefault();

                    }

                    // I see some logical error here
                    // what happens when hub x was selected and the allocation was made from hub y?
                    //TOFIX:
                    // Hub is required for this transaction
                    // Try catch is danger!! Either throw the exception or use conditional statement.

                    //if (hubID1 != 0)
                    //{
                    //    transaction.HubID = hubID1;
                    //}
                    //else
                    //{
                    //    transaction.HubID =
                    //        _unitOfWorkNew.HubAllocationRepository.FindBy(
                    //            r => r.RequisitionID == allocation.ReliefRequisitionDetail.RequisitionID).Select(
                    //                h => h.HubID).FirstOrDefault();
                    //}
                    transaction.HubID = allocationDetail.HubID;



                    _unitOfWorkNew.TransactionRepository.Add(transaction);
                    // result.Add(transaction);

                    /*post Debit-Pledged To FDP*/
                    var transaction2 = new Models.Transaction
                    {
                        TransactionID = Guid.NewGuid(),
                        TransactionGroupID = transactionGroup,
                        TransactionDate = transactionDate,
                        UnitID = 1
                    };



                    transaction2.QuantityInMT = allocationDetail.AllocatedAmount;
                    transaction2.QuantityInUnit = allocationDetail.AllocatedAmount;
                    transaction2.LedgerID = Ledger.Constants.PLEDGED_TO_FDP;
                    transaction2.CommodityID = allocationDetail.ReliefRequisitionDetail.CommodityID;
                    transaction2.ParentCommodityID = allocationDetail.ReliefRequisitionDetail.Commodity.ParentID;
                    transaction2.FDPID = allocationDetail.ReliefRequisitionDetail.FDPID;
                    transaction2.ProgramID = (int)allocationDetail.ReliefRequisitionDetail.ReliefRequisition.ProgramID;
                    transaction2.RegionID = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionID;
                    transaction2.PlanId = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionalRequest.PlanID;
                    transaction2.Round = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.Round;

                    //int hubID2 = 0;
                    //if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                    //{
                    //    var siCode = allocationDetail.Code.ToString();
                    //    var shippingInstruction =
                    //        _unitOfWorkNew.ShippingInstructionRepository.Get(t => t.Value == siCode).
                    //            FirstOrDefault();
                    //    if (shippingInstruction != null)
                    //        transaction.ShippingInstructionID = shippingInstruction.ShippingInstructionID;

                    //    //hubID2 =
                    //    //    (int)
                    //    //        _unitOfWorkNew.TransactionRepository.FindBy(
                    //    //            m => m.ShippingInstructionID == allocationDetail.Code &&
                    //    //                 m.LedgerID == Ledger.Constants.GOODS_ON_HAND).Select(m => m.HubID).FirstOrDefault();


                    //}
                    //else
                    //{
                    //    var detail = allocationDetail;
                    //    var code = detail.Code.ToString();
                    //    var projectCode =
                    //        _unitOfWork.ProjectCodeRepository.Get(t => t.Value == code).
                    //            FirstOrDefault();
                    //    if (projectCode != null) transaction.ProjectCodeID = projectCode.ProjectCodeID;

                    //    //hubID2 =
                    //    //    (int)_unitOfWork.TransactionRepository.FindBy(m => m.ProjectCodeID == allocationDetail.Code &&
                    //    //                                                       m.LedgerID == Ledger.Constants.GOODS_ON_HAND)
                    //    //        .Select(m => m.HubID)
                    //    //        .FirstOrDefault();

                    //}
                    if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                    {
                        transaction2.ShippingInstructionID = allocationDetail.Code;
                    }
                    else
                    {
                        transaction2.ProjectCodeID = allocationDetail.Code;
                    }

                    //if (hubID2 != 0)
                    //{
                    //    transaction2.HubID = hubID2;
                    //}

                    //else
                    //{
                    //    transaction2.HubID =
                    //        _unitOfWorkNew.HubAllocationRepository.FindBy(
                    //            r => r.RequisitionID == allocation.ReliefRequisitionDetail.RequisitionID).Select(
                    //                h => h.HubID).FirstOrDefault();

                    //}

                    transaction2.HubID = allocationDetail.HubID;

                    _unitOfWorkNew.TransactionRepository.Add(transaction2);
                    allocationDetail.TransactionGroupID = transactionGroup;
                    _unitOfWorkNew.SIPCAllocationRepository.Edit(allocationDetail);
                    //result.Add(transaction);

                }
            }
            var requisition = _unitOfWorkNew.ReliefRequisitionRepository.FindById(requisitionID);
            requisition.Status = 4;
            _unitOfWorkNew.ReliefRequisitionRepository.Edit(requisition);
            _unitOfWorkNew.Save();
            //return result;
            return true;
        }
        public void SaveDispatchTransaction(DispatchViewModel dispatchViewModel, Boolean reverse = false)
        {
            int transactionsign = reverse ? -1 : 1;
            Dispatch dispatch;
            DispatchAllocation tempDispatchAllocation =
                _unitOfWork.DispatchAllocationRepository.FindById(dispatchViewModel.DispatchAllocationID);
            if (dispatchViewModel.ShippingInstructionID != tempDispatchAllocation.ShippingInstructionID)
            {
                DispatchAllocation newDispatchAllocation = new DispatchAllocation();
                // newDispatchAllocation = tempDispatchAllocation;
                newDispatchAllocation.ShippingInstructionID = dispatchViewModel.ShippingInstructionID;
                newDispatchAllocation.Amount = dispatchViewModel.Quantity;
                newDispatchAllocation.Beneficiery = tempDispatchAllocation.Beneficiery;
                newDispatchAllocation.BidRefNo = tempDispatchAllocation.BidRefNo;
                newDispatchAllocation.CommodityID = tempDispatchAllocation.CommodityID;

                // This keeps track of DispatchAllocations newly created during SI change under their parent Dispatch Allocation
                newDispatchAllocation.ParentDispatchAllocationID = tempDispatchAllocation.DispatchAllocationID;

                newDispatchAllocation.DonorID = tempDispatchAllocation.DonorID;
                newDispatchAllocation.FDPID = tempDispatchAllocation.FDPID;
                newDispatchAllocation.HubID = tempDispatchAllocation.HubID;
                newDispatchAllocation.Month = tempDispatchAllocation.Month;
                newDispatchAllocation.PartitionId = tempDispatchAllocation.PartitionId;
                newDispatchAllocation.ProgramID = tempDispatchAllocation.ProgramID;
                newDispatchAllocation.ProjectCodeID = tempDispatchAllocation.ProjectCodeID;
                newDispatchAllocation.RequisitionNo = tempDispatchAllocation.RequisitionNo;
                newDispatchAllocation.Round = tempDispatchAllocation.Round;
                newDispatchAllocation.RequisitionId = tempDispatchAllocation.RequisitionId;
                newDispatchAllocation.TransporterID = tempDispatchAllocation.TransporterID;
                newDispatchAllocation.Unit = tempDispatchAllocation.Unit;
                newDispatchAllocation.Year = tempDispatchAllocation.Year;
                newDispatchAllocation.TransportOrderID = tempDispatchAllocation.TransportOrderID;
                //newDispatchAllocation.DispatchAllocationID = Guid.Empty;
                newDispatchAllocation.DispatchAllocationID = Guid.NewGuid();
                _unitOfWork.DispatchAllocationRepository.Add(newDispatchAllocation);
                tempDispatchAllocation.Amount -= dispatchViewModel.Quantity;
                _unitOfWork.DispatchAllocationRepository.Edit(tempDispatchAllocation);
                _unitOfWork.Save();
                dispatchViewModel.DispatchAllocationID = newDispatchAllocation.DispatchAllocationID;
                //int siPCAllocationID = 0;
                //var allocationFirst = _unitOfWorkNew.SIPCAllocationRepository.Get(t => t.ReliefRequisitionDetail.RequisitionID == newDispatchAllocation.RequisitionId.Value).FirstOrDefault();
                int siPCAllocationID = 0;
                var requisitionDetail =
                  _unitOfWorkNew.ReliefRequisitionDetailRepository.Get(
                      t => t.RequisitionID == newDispatchAllocation.RequisitionId && t.FDPID == newDispatchAllocation.FDPID).FirstOrDefault();
                var allocationFirst = _unitOfWorkNew.SIPCAllocationRepository.FindBy(
                      t => t.RequisitionDetailID == requisitionDetail.RequisitionDetailID && t.Code == tempDispatchAllocation.ShippingInstructionID).FirstOrDefault();

                if (allocationFirst != null)
                {
                    daModel.SIPCAllocation allocation = new daModel.SIPCAllocation();

                    allocation.Code = newDispatchAllocation.ShippingInstructionID.Value;
                    allocation.AllocatedAmount = dispatchViewModel.Quantity;
                    allocation.AllocationType = allocationFirst.AllocationType;
                    allocation.RequisitionDetailID = allocationFirst.RequisitionDetailID;
                    allocation.HubID = allocationFirst.HubID;
                    allocationFirst.AllocatedAmount -= allocation.AllocatedAmount;
                    _unitOfWorkNew.SIPCAllocationRepository.Add(allocation);
                    _unitOfWorkNew.SIPCAllocationRepository.Edit(allocationFirst);
                    _unitOfWorkNew.Save();

                    siPCAllocationID = allocation.SIPCAllocationID;
                    PostSIAllocationUncommit(allocationFirst.RequisitionDetailID, allocationFirst.Code, allocation.Code);
                }


                if (newDispatchAllocation.RequisitionId.HasValue)
                    PostSIAllocation(newDispatchAllocation.RequisitionId.Value, siPCAllocationID);

            }

            if (dispatchViewModel.DispatchID != null)
                dispatch = _unitOfWork.DispatchRepository.FindById(dispatchViewModel.DispatchID.GetValueOrDefault());
            else
            {
                dispatch = new Dispatch();
                dispatch.DispatchID = Guid.NewGuid();
            }




            dispatch.BidNumber = dispatchViewModel.BidNumber;
            dispatch.CreatedDate = dispatchViewModel.CreatedDate;
            dispatch.DispatchAllocationID = dispatchViewModel.DispatchAllocationID;
            dispatch.DispatchDate = dispatchViewModel.DispatchDate;

            dispatch.DispatchedByStoreMan = dispatchViewModel.DispatchedByStoreMan;
            dispatch.DriverName = dispatchViewModel.DriverName;
            dispatch.FDPID = dispatchViewModel.FDPID;
            dispatch.GIN = dispatchViewModel.GIN;
            dispatch.HubID = dispatchViewModel.HubID;
            dispatch.PeriodMonth = dispatchViewModel.Month;
            dispatch.PeriodYear = dispatchViewModel.Year;
            dispatch.PlateNo_Prime = dispatchViewModel.PlateNo_Prime;
            dispatch.PlateNo_Trailer = dispatchViewModel.PlateNo_Trailer;
            dispatch.Remark = dispatchViewModel.Remark;
            dispatch.RequisitionNo = dispatchViewModel.RequisitionNo;
            dispatch.Round = dispatchViewModel.Round;
            dispatch.TransporterID = dispatchViewModel.TransporterID;
            dispatch.UserProfileID = dispatchViewModel.UserProfileID;
            dispatch.WeighBridgeTicketNumber = dispatchViewModel.WeighBridgeTicketNumber;
            dispatch.ShippingInstructionID = dispatchViewModel.ShippingInstructionID.Value;


            //dispatch.Type = dispatchViewModel.Type;

            var group = new TransactionGroup();
            group.TransactionGroupID = Guid.NewGuid();

            var dispatchDetail = new DispatchDetail
            {
                DispatchID = dispatch.DispatchID,
                CommodityID = dispatchViewModel.CommodityID,
                CommodityChildID = dispatchViewModel.CommodityChildID,
                Description = dispatchViewModel.Commodity,
                DispatchDetailID = Guid.NewGuid(),
                RequestedQuantityInMT = dispatchViewModel.Quantity,
                RequestedQunatityInUnit = dispatchViewModel.QuantityInUnit,
                QuantityPerUnit = dispatchViewModel.QuantityPerUnit,

                UnitID = dispatchViewModel.UnitID,
                TransactionGroupID = @group.TransactionGroupID
            };

            //var parentCommodityId =
            //    _unitOfWork.CommodityRepository.FindById(dispatchViewModel.CommodityChildID).ParentID ??
            //    dispatchViewModel.CommodityID;
            // Physical movement of stock
            var transactionInTransit = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatchViewModel.FDPID),
                ProgramID = dispatchViewModel.ProgramID,
                ParentCommodityID = null,
                CommodityID = dispatchViewModel.CommodityID,
                CommodityChildID = dispatchViewModel.CommodityChildID,
                FDPID = dispatchViewModel.FDPID,
                HubID = dispatchViewModel.HubID,
                HubOwnerID = _unitOfWork.HubRepository.FindById(dispatchViewModel.HubID).HubOwnerID,
                LedgerID = Models.Ledger.Constants.GOODS_IN_TRANSIT,
                QuantityInMT = transactionsign * (+dispatchViewModel.Quantity),
                QuantityInUnit = transactionsign * (+dispatchViewModel.QuantityInUnit),
                ShippingInstructionID = dispatchViewModel.ShippingInstructionID,
                ProjectCodeID = dispatchViewModel.ProjectCodeID,
                Round = dispatchViewModel.Round,
                PlanId = dispatchViewModel.PlanId,
                TransactionDate = DateTime.Now,
                UnitID = dispatchViewModel.UnitID,
                TransactionGroupID = @group.TransactionGroupID
            };
            //transaction2.Stack = dispatchModel.StackNumber;
            //transaction2.StoreID = dispatchModel.StoreID;
            //group.Transactions.Add(transaction2);



            var transactionGoh = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatchViewModel.FDPID),
                ProgramID = dispatchViewModel.ProgramID,
                ParentCommodityID = null,
                CommodityID = dispatchViewModel.CommodityID,
                CommodityChildID = dispatchViewModel.CommodityChildID,
                FDPID = dispatchViewModel.FDPID,
                HubID = dispatchViewModel.HubID,
                HubOwnerID = _unitOfWork.HubRepository.FindById(dispatchViewModel.HubID).HubOwnerID,
                LedgerID = Models.Ledger.Constants.GOODS_ON_HAND,
                QuantityInMT = transactionsign * (-dispatchViewModel.Quantity),
                QuantityInUnit = transactionsign * (-dispatchViewModel.QuantityInUnit),
                ShippingInstructionID = dispatchViewModel.ShippingInstructionID,
                ProjectCodeID = dispatchViewModel.ProjectCodeID,
                Round = dispatchViewModel.Round,
                PlanId = dispatchViewModel.PlanId,
                TransactionDate = DateTime.Now,
                UnitID = dispatchViewModel.UnitID,
                TransactionGroupID = @group.TransactionGroupID
            };
            //transaction.Stack = dispatch.StackNumber;
            //transaction.StoreID = dispatch.StoreID;


            // plan side of the transaction (Red Border)

            var transactionComitedToFdp = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatchViewModel.FDPID),
                ProgramID = dispatchViewModel.ProgramID,
                ParentCommodityID = null,
                CommodityID = dispatchViewModel.CommodityID,
                CommodityChildID = dispatchViewModel.CommodityChildID,
                FDPID = dispatchViewModel.FDPID,
                HubID = dispatchViewModel.HubID,
                HubOwnerID = _unitOfWork.HubRepository.FindById(dispatchViewModel.HubID).HubOwnerID,
                LedgerID = Models.Ledger.Constants.COMMITED_TO_FDP,
                QuantityInMT = transactionsign * (+dispatchViewModel.Quantity),
                QuantityInUnit = transactionsign * (+dispatchViewModel.QuantityInUnit),
                ShippingInstructionID = dispatchViewModel.ShippingInstructionID,
                ProjectCodeID = dispatchViewModel.ProjectCodeID,
                Round = dispatchViewModel.Round,
                PlanId = dispatchViewModel.PlanId,
                TransactionDate = DateTime.Now,
                UnitID = dispatchViewModel.UnitID,
                TransactionGroupID = @group.TransactionGroupID
            };
            //transaction2.Stack = dispatchModel.StackNumber;
            //transaction2.StoreID = dispatchModel.StoreID;
            //group.Transactions.Add(transaction2);





            var transactionInTansitFreeStock = new Transaction
            {
                TransactionID = Guid.NewGuid(),
                AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatchViewModel.FDPID),
                ProgramID = dispatchViewModel.ProgramID,
                ParentCommodityID = null,
                CommodityID = dispatchViewModel.CommodityID,
                CommodityChildID = dispatchViewModel.CommodityChildID,
                FDPID = dispatchViewModel.FDPID,
                HubID = dispatchViewModel.HubID,
                HubOwnerID = _unitOfWork.HubRepository.FindById(dispatchViewModel.HubID).HubOwnerID,
                LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK,
                QuantityInMT = transactionsign * (-dispatchViewModel.Quantity),
                QuantityInUnit = transactionsign * (-dispatchViewModel.QuantityInUnit),
                ShippingInstructionID = dispatchViewModel.ShippingInstructionID,
                ProjectCodeID = dispatchViewModel.ProjectCodeID,
                Round = dispatchViewModel.Round,
                PlanId = dispatchViewModel.PlanId,
                TransactionDate = DateTime.Now,
                UnitID = dispatchViewModel.UnitID,
                TransactionGroupID = @group.TransactionGroupID
            };
            //transaction.Stack = dispatch.StackNumber;
            //transaction.StoreID = dispatch.StoreID;
            dispatch.DispatchDetails.Clear();
            dispatch.DispatchDetails.Add(dispatchDetail);


            try
            {
                _unitOfWork.TransactionGroupRepository.Add(group);
                _unitOfWork.TransactionRepository.Add(transactionInTransit);
                _unitOfWork.TransactionRepository.Add(transactionGoh);
                _unitOfWork.TransactionRepository.Add(transactionInTansitFreeStock);
                _unitOfWork.TransactionRepository.Add(transactionComitedToFdp);
                if (!reverse)
                {
                    if (dispatchViewModel.DispatchID == null)
                    {
                        _unitOfWork.DispatchRepository.Add(dispatch);
                        _IWorkflowActivityService.EnterCreateWorkflow(dispatch);

                    }

                    else
                    {
                        _unitOfWork.DispatchRepository.Edit(dispatch);
                        _IWorkflowActivityService.EnterEditWorkflow(dispatch);

                    }
                }


                _unitOfWork.Save();

            }

            catch (Exception exp)
            {
                // dbTransaction.Rollback();
                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Dispatch Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }

        }

        public decimal GetGoodsInTransit(int shippinginstructionId, int hubId, int programId, int donorId)
        {
            return
                _unitOfWork.TransactionRepository.Get(
                    t =>
                        t.ProgramID == programId && t.HubID == hubId // && t.DonorID == donorId 
                        &&
                        t.ShippingInstructionID == shippinginstructionId &&
                        t.LedgerID == Models.Ledger.Constants.GOODS_IN_TRANSIT).Select(s => s.QuantityInMT).Sum();
        }
        public bool PostSIAllocationUncommit(int requisitionDetailID, int siID, int newSiIDd)
        {
            var allocationDetail = _unitOfWorkNew.SIPCAllocationRepository.Get(t => t.RequisitionDetailID == requisitionDetailID && t.Code == siID).FirstOrDefault();
            var allocationDetailNew = _unitOfWorkNew.SIPCAllocationRepository.Get(t => t.RequisitionDetailID == requisitionDetailID && t.Code == newSiIDd).FirstOrDefault();
            if (allocationDetail == null || allocationDetailNew == null) return false;

            if (allocationDetail.TransactionGroupID != null)
            {
                var transactionGroup = (Guid)allocationDetail.TransactionGroupID;
                var transactionDate = DateTime.Now;
                //_unitOfWorkNew.TransactionGroupRepository.Add(new daModel.TransactionGroup { PartitionID = 0, TransactionGroupID = transactionGroup });
                //ProjectCodeID	ShippingInstructionID ProgramID QuantityInMT	QuantityInUnit	UnitID	TransactionDate	RegionID	Month	Round	DonorID	CommoditySourceID	GiftTypeID	FDP

                var transaction = new Models.Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup,
                    TransactionDate = transactionDate,
                    UnitID = 1
                };

                var allocation = allocationDetail;




                transaction.QuantityInMT = allocationDetailNew.AllocatedAmount;
                transaction.QuantityInUnit = allocationDetailNew.AllocatedAmount;
                transaction.LedgerID = Ledger.Constants.COMMITED_TO_FDP;
                transaction.CommodityID = allocationDetail.ReliefRequisitionDetail.CommodityID;
                transaction.ParentCommodityID = allocationDetail.ReliefRequisitionDetail.Commodity.ParentID;
                transaction.FDPID = allocationDetail.ReliefRequisitionDetail.FDPID;
                transaction.ProgramID = (int)allocationDetail.ReliefRequisitionDetail.ReliefRequisition.ProgramID;
                transaction.RegionID = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionID;
                transaction.PlanId = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionalRequest.PlanID;
                transaction.Round = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.Round;


                if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                {
                    transaction.ShippingInstructionID = allocationDetail.Code;
                }
                else
                {
                    transaction.ProjectCodeID = allocationDetail.Code;
                }

                transaction.HubID = allocationDetail.HubID;

                // I see some logical error here
                // what happens when hub x was selected and the allocation was made from hub y?
                //TOFIX:
                // Hub is required for this transaction
                // Try catch is danger!! Either throw the exception or use conditional statement.

                //if (hubID1!=0)
                //{
                //    transaction.HubID = hubID1;
                //}
                //else
                //{
                //      transaction.HubID =
                //                        _unitOfWorkNew.HubAllocationRepository.FindBy(r => r.RequisitionID == allocation.ReliefRequisitionDetail.RequisitionID).Select(
                //                                h => h.HubID).FirstOrDefault();
                //}




                _unitOfWorkNew.TransactionRepository.Add(transaction);
                // result.Add(transaction);

                /*post Debit-Pledged To FDP*/
                var transaction2 = new Models.Transaction
                {
                    TransactionID = Guid.NewGuid(),
                    TransactionGroupID = transactionGroup,
                    TransactionDate = transactionDate,
                    UnitID = 1
                };



                transaction2.QuantityInMT = -allocationDetailNew.AllocatedAmount;
                transaction2.QuantityInUnit = -allocationDetailNew.AllocatedAmount;
                transaction2.LedgerID = Ledger.Constants.PLEDGED_TO_FDP;
                transaction2.CommodityID = allocationDetail.ReliefRequisitionDetail.CommodityID;
                transaction2.ParentCommodityID = allocationDetail.ReliefRequisitionDetail.Commodity.ParentID;
                transaction2.FDPID = allocationDetail.ReliefRequisitionDetail.FDPID;
                transaction2.ProgramID = (int)allocationDetail.ReliefRequisitionDetail.ReliefRequisition.ProgramID;
                transaction2.RegionID = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionID;
                transaction2.PlanId = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.RegionalRequest.PlanID;
                transaction2.Round = allocationDetail.ReliefRequisitionDetail.ReliefRequisition.Round;

                int hubID2 = 0;
                //if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                //{
                //    var siCode = allocationDetail.Code.ToString();
                //    var shippingInstruction =
                //        _unitOfWorkNew.ShippingInstructionRepository.Get(t => t.Value == siCode).
                //            FirstOrDefault();
                //    if (shippingInstruction != null) transaction.ShippingInstructionID = shippingInstruction.ShippingInstructionID;

                //    //hubID2=(int) _unitOfWorkNew.TransactionRepository.FindBy(m => m.ShippingInstructionID == allocationDetail.Code &&
                //    //       m.LedgerID == Ledger.Constants.GOODS_ON_HAND).Select(m => m.HubID).FirstOrDefault();


                //}
                //else
                //{
                //    var detail = allocationDetail;
                //    var code = detail.Code.ToString();
                //    var projectCode =
                //        _unitOfWorkNew.ProjectCodeRepository.Get(t => t.Value == code).
                //            FirstOrDefault();
                //    if (projectCode != null) transaction.ProjectCodeID = projectCode.ProjectCodeID;

                //    //hubID2 = (int)_unitOfWorkNew.TransactionRepository.FindBy(m => m.ProjectCodeID == allocationDetail.Code &&
                //    //           m.LedgerID == Ledger.Constants.GOODS_ON_HAND).Select(m => m.HubID).FirstOrDefault();

                //}
                if (allocationDetail.AllocationType == daModel.TransactionConstants.Constants.SHIPPNG_INSTRUCTION)
                {
                    transaction2.ShippingInstructionID = allocationDetail.Code;
                }
                else
                {
                    transaction2.ProjectCodeID = allocationDetail.Code;
                }
                transaction2.HubID = allocationDetail.HubID;

                //if (hubID2!=0)
                //{
                //    transaction2.HubID = hubID2;
                //}

                //else
                //{
                //    transaction2.HubID =
                //                       _unitOfWorkNew.HubAllocationRepository.FindBy(r => r.RequisitionID == allocation.ReliefRequisitionDetail.RequisitionID).Select(
                //                               h => h.HubID).FirstOrDefault();

                //}

                _unitOfWorkNew.TransactionRepository.Add(transaction2);
                allocationDetail.TransactionGroupID = transactionGroup;

                _unitOfWorkNew.SIPCAllocationRepository.Edit(allocationDetail);
                //result.Add(transaction);

                //var requisition = _unitOfWorkNew.ReliefRequisitionDetailRepository.FindBy(t=>t.RequisitionDetailID == requisitionDetailID).Select(t=>t.ReliefRequisition).FirstOrDefault();
                //if (requisition != null)
                //{
                //    requisition.Status = 4;
                //    _unitOfWorkNew.ReliefRequisitionRepository.Edit(requisition);
                //}
                _unitOfWorkNew.Save();
                //return result;
                return true;
            }
            return false;
        }

        #region dispatch transaction helpers
        //TODO: this section has to be cleaned
        /// <summary>
        /// Gets the positive FDP transaction.
        /// </summary>
        /// <param name="dispatchModel">The dispatch model.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <param name="detail">The detail.</param>
        /// <returns></returns>
        private Transaction GetGoodsInTransitFDPTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction2 = new Transaction();
            transaction2.TransactionID = Guid.NewGuid();
            transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatchModel.FDPID.Value);
            transaction2.ProgramID = dispatchModel.ProgramID;
            transaction2.ParentCommodityID = detail.CommodityID;
            transaction2.CommodityID = detail.CommodityID;
            transaction2.HubID = dispatch.HubID;
            transaction2.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction2.LedgerID = Ledger.Constants.GOODS_IN_TRANSIT;
            transaction2.QuantityInMT = +detail.DispatchedQuantityMT.Value;
            transaction2.QuantityInUnit = +detail.DispatchedQuantity.Value;
            transaction2.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            transaction2.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction2.Stack = dispatchModel.StackNumber;
            transaction2.StoreID = dispatchModel.StoreID;
            transaction2.TransactionDate = DateTime.Now;
            transaction2.UnitID = detail.Unit;
            return transaction2;
        }

        /// <summary>
        /// Gets the negative FDP transaction.
        /// </summary>
        /// <param name="dispatchModel">The dispatch model.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <param name="detail">The detail.</param>
        /// <returns></returns>
        private Transaction GetGoodsOnHandFDPTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionID = Guid.NewGuid();
            transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatch.FDPID.Value);
            transaction.ProgramID = dispatchModel.ProgramID;
            transaction.ParentCommodityID = detail.CommodityID;
            transaction.CommodityID = detail.CommodityID;
            transaction.HubID = dispatch.HubID;
            transaction.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction.LedgerID = Ledger.Constants.GOODS_ON_HAND; //previously GOODS_ON_HAND_UNCOMMITED
            transaction.QuantityInMT = -detail.DispatchedQuantityMT.Value;
            transaction.QuantityInUnit = -detail.DispatchedQuantity.Value;
            transaction.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            transaction.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction.Stack = dispatchModel.StackNumber;
            transaction.StoreID = dispatchModel.StoreID;
            transaction.TransactionDate = DateTime.Now;
            transaction.UnitID = detail.Unit;
            return transaction;
        }

        private Transaction GetStatisticsFDPTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionID = Guid.NewGuid();
            transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatch.FDPID.Value);
            transaction.ProgramID = dispatchModel.ProgramID;
            transaction.ParentCommodityID = detail.CommodityID;
            transaction.CommodityID = detail.CommodityID;
            transaction.HubID = dispatch.HubID;
            transaction.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction.LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK;
            transaction.QuantityInMT = -detail.DispatchedQuantityMT.Value;
            transaction.QuantityInUnit = -detail.DispatchedQuantity.Value;
            transaction.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            transaction.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction.Stack = dispatchModel.StackNumber;
            transaction.StoreID = dispatchModel.StoreID;
            transaction.TransactionDate = DateTime.Now;
            transaction.UnitID = detail.Unit;
            return transaction;
        }

        private Transaction GetCommitedToFDPTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionID = Guid.NewGuid();
            transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.FDP, dispatch.FDPID.Value);
            transaction.ProgramID = dispatchModel.ProgramID;
            transaction.ParentCommodityID = detail.CommodityID;
            transaction.CommodityID = detail.CommodityID;
            transaction.HubID = dispatch.HubID;
            transaction.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction.LedgerID = Ledger.Constants.COMMITED_TO_FDP;
            transaction.QuantityInMT = +detail.DispatchedQuantityMT.Value;
            transaction.QuantityInUnit = +detail.DispatchedQuantity.Value;
            transaction.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            transaction.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction.Stack = dispatchModel.StackNumber;
            transaction.StoreID = dispatchModel.StoreID;
            transaction.TransactionDate = DateTime.Now;
            transaction.UnitID = detail.Unit;
            return transaction;
        }

        /// <summary>
        /// Gets the positive HUB transaction.
        /// </summary>
        /// <param name="dispatchModel">The dispatch model.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <param name="detail">The detail.</param>
        /// <returns></returns>
        private Transaction GetGoodInTransitHUBTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction2 = new Transaction();
            transaction2.TransactionID = Guid.NewGuid();
            transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, dispatchModel.ToHubID.Value);
            transaction2.ProgramID = dispatchModel.ProgramID;
            transaction2.ParentCommodityID = detail.CommodityID;
            transaction2.CommodityID = detail.CommodityID;
            transaction2.HubID = dispatch.HubID;
            transaction2.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction2.LedgerID = Ledger.Constants.GOODS_IN_TRANSIT;
            transaction2.QuantityInMT = +detail.DispatchedQuantityMT.Value;
            transaction2.QuantityInUnit = +detail.DispatchedQuantity.Value;
            transaction2.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);

            transaction2.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction2.Stack = dispatchModel.StackNumber;
            transaction2.StoreID = dispatchModel.StoreID;
            transaction2.TransactionDate = DateTime.Now;
            transaction2.UnitID = detail.Unit;
            return transaction2;
        }

        private Transaction GetGoodsOnHandHUBTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction2 = new Transaction();
            transaction2.TransactionID = Guid.NewGuid();
            transaction2.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, dispatchModel.ToHubID.Value);
            transaction2.ProgramID = dispatchModel.ProgramID;
            transaction2.ParentCommodityID = detail.CommodityID;
            transaction2.CommodityID = detail.CommodityID;
            transaction2.HubID = dispatch.HubID;
            transaction2.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction2.LedgerID = Ledger.Constants.GOODS_ON_HAND; //Previously GOODS_ON_HAND_UNCOMMITED
            transaction2.QuantityInMT = -detail.DispatchedQuantityMT.Value;
            transaction2.QuantityInUnit = -detail.DispatchedQuantity.Value;
            transaction2.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);

            transaction2.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction2.Stack = dispatchModel.StackNumber;
            transaction2.StoreID = dispatchModel.StoreID;
            transaction2.TransactionDate = DateTime.Now;
            transaction2.UnitID = detail.Unit;
            return transaction2;
        }

        /// <summary>
        /// Gets the negative HUB Transaction.
        /// </summary>
        /// <param name="dispatchModel">The dispatch model.</param>
        /// <param name="dispatch">The dispatch.</param>
        /// <param name="detail">The detail.</param>
        /// <returns></returns>
        private Transaction GetStatisticsHUBTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionID = Guid.NewGuid();
            transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, dispatch.HubID);
            transaction.ProgramID = dispatchModel.ProgramID;
            transaction.ParentCommodityID = detail.CommodityID;
            transaction.CommodityID = detail.CommodityID;
            transaction.HubID = dispatch.HubID;
            transaction.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction.LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK;
            transaction.QuantityInMT = -detail.DispatchedQuantityMT.Value;
            transaction.QuantityInUnit = -detail.DispatchedQuantity.Value;
            transaction.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            //transaction.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction.Stack = dispatchModel.StackNumber;
            transaction.StoreID = dispatchModel.StoreID;
            transaction.TransactionDate = DateTime.Now;
            transaction.UnitID = detail.Unit;
            return transaction;
        }

        private Transaction GetCommittedToFDPHUBTransaction(DispatchModel dispatchModel, Dispatch dispatch, DispatchDetailModel detail)
        {
            Transaction transaction = new Transaction();
            transaction.TransactionID = Guid.NewGuid();
            transaction.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, dispatch.HubID);
            transaction.ProgramID = dispatchModel.ProgramID;
            transaction.ParentCommodityID = detail.CommodityID;
            transaction.CommodityID = detail.CommodityID;
            transaction.HubID = dispatch.HubID;
            transaction.HubOwnerID = _unitOfWork.HubRepository.FindById(dispatch.HubID).HubOwnerID;
            transaction.LedgerID = Ledger.Constants.STATISTICS_FREE_STOCK;
            transaction.QuantityInMT = -detail.DispatchedQuantityMT.Value;
            transaction.QuantityInUnit = -detail.DispatchedQuantity.Value;
            transaction.ShippingInstructionID = _shippingInstructionService.GetShipingInstructionId(dispatchModel.SINumber);
            //transaction.ProjectCodeID = _projectCodeService.GetProjectCodeId(dispatchModel.ProjectNumber);
            transaction.Stack = dispatchModel.StackNumber;
            transaction.StoreID = dispatchModel.StoreID;
            transaction.TransactionDate = DateTime.Now;
            transaction.UnitID = detail.Unit;
            return transaction;
        }

        /// <summary>
        /// Generates the dispatch detail.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private DispatchDetail GenerateDispatchDetail(DispatchDetailModel c)
        {
            if (c != null)
            {
                DispatchDetail dispatchDetail = new DispatchDetail()
                {
                    CommodityID = c.CommodityID,
                    Description = c.Description,
                    // DispatchDetailID = c.Id,
                    RequestedQuantityInMT = c.RequestedQuantityMT.Value,
                    //DispatchedQuantityInMT = c.DispatchedQuantityMT,
                    //DispatchedQuantityInUnit = c.DispatchedQuantity,
                    RequestedQunatityInUnit = c.RequestedQuantity.Value,
                    UnitID = c.Unit
                };
                if (c.Id.HasValue)
                {
                    dispatchDetail.DispatchDetailID = c.Id.Value;
                }

                return dispatchDetail;
            }
            else
            {
                return null;
            }
        }

        #endregion


        /// <summary>
        /// Finds the by transaction group ID.
        /// </summary>
        /// <param name="partition">The partition.</param>
        /// <param name="transactionGroupID">The transaction group ID.</param>
        /// <returns></returns>
        public Transaction FindByTransactionGroupID(Guid transactionGroupID)
        {
            return _unitOfWork.TransactionRepository.Get(tr => tr.TransactionGroupID == transactionGroupID).FirstOrDefault();
        }



        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="user"></param>
        /// <exception cref="System.Exception"></exception>
        public void SaveInternalMovementTrasnsaction(InternalMovementViewModel viewModel, UserProfile user)
        {
            InternalMovement internalMovement = new InternalMovement();
            TransactionGroup transactionGroup = new TransactionGroup();
            Transaction transactionFromStore = new Transaction();
            var transactionGroupId = Guid.NewGuid();



            Commodity commodity = _unitOfWork.CommodityRepository.FindById(viewModel.CommodityId);

            transactionFromStore.TransactionID = Guid.NewGuid();
            transactionFromStore.TransactionGroupID = transactionGroupId;
            transactionFromStore.LedgerID = 2;
            transactionFromStore.HubOwnerID = user.DefaultHubObj.HubOwner.HubOwnerID;
            //trasaction.AccountID
            transactionFromStore.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionFromStore.HubID = user.DefaultHub.Value;
            transactionFromStore.StoreID = viewModel.FromStoreId;  //
            transactionFromStore.Stack = viewModel.FromStackId; //
            transactionFromStore.ProjectCodeID = viewModel.ProjectCodeId;
            transactionFromStore.ShippingInstructionID = viewModel.ShippingInstructionId;
            transactionFromStore.ProgramID = viewModel.ProgramId;
            transactionFromStore.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionFromStore.CommodityID = viewModel.CommodityId;
            transactionFromStore.CommodityGradeID = null; // How did I get this value ?
            transactionFromStore.QuantityInMT = 0 - viewModel.QuantityInMt;
            transactionFromStore.QuantityInUnit = 0 - viewModel.QuantityInUnit;
            transactionFromStore.UnitID = viewModel.UnitId;
            transactionFromStore.TransactionDate = DateTime.Now;



            Transaction transactionToStore = new Transaction();

            transactionToStore.TransactionID = Guid.NewGuid();
            transactionToStore.TransactionGroupID = transactionGroupId;
            transactionToStore.LedgerID = 2;
            transactionToStore.HubOwnerID = user.DefaultHubObj.HubOwner.HubOwnerID;
            //transactionToStore.AccountID
            transactionToStore.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionToStore.HubID = user.DefaultHub.Value;
            transactionToStore.StoreID = viewModel.ToStoreId;  //
            transactionToStore.Stack = viewModel.ToStackId; //
            transactionToStore.ProjectCodeID = viewModel.ProjectCodeId;
            transactionToStore.ShippingInstructionID = viewModel.ShippingInstructionId;
            transactionToStore.ProgramID = viewModel.ProgramId;

            transactionToStore.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionToStore.CommodityID = viewModel.CommodityId;
            transactionToStore.CommodityGradeID = null; // How did I get this value ?
            transactionToStore.QuantityInMT = viewModel.QuantityInMt;
            transactionToStore.QuantityInUnit = viewModel.QuantityInUnit;
            transactionToStore.UnitID = viewModel.UnitId;
            transactionToStore.TransactionDate = DateTime.Now;

            transactionGroup.TransactionGroupID = transactionGroupId;
            transactionGroup.Transactions.Add(transactionFromStore);
            transactionGroup.Transactions.Add(transactionToStore);
            transactionGroup.PartitionId = 0;

            internalMovement.InternalMovementID = Guid.NewGuid();
            internalMovement.PartitionId = 0;
            internalMovement.TransactionGroupID = transactionGroupId;
            internalMovement.TransactionGroup = transactionGroup;
            internalMovement.TransferDate = viewModel.SelectedDate;
            internalMovement.DReason = viewModel.ReasonId;
            internalMovement.Notes = viewModel.Note;
            internalMovement.ApprovedBy = viewModel.ApprovedBy;
            internalMovement.ReferenceNumber = viewModel.ReferenceNumber;
            internalMovement.HubID = user.DefaultHub.Value;



            // Try to save this transaction

            try
            {
                _unitOfWork.InternalMovementRepository.Add(internalMovement);
                _unitOfWork.Save();
            }
            catch (Exception exp)
            {
                //dbTransaction.Rollback();
                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Internal Movement Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="user"></param>
        /// <exception cref="System.Exception"></exception>
        public void SaveLossTrasnsaction(LossesAndAdjustmentsViewModel viewModel, UserProfile user)
        {
            Commodity commodity = _unitOfWork.CommodityRepository.FindById(viewModel.CommodityId);



            Adjustment lossAndAdjustment = new Adjustment();
            TransactionGroup transactionGroup = new TransactionGroup();
            Transaction transactionOne = new Transaction();

            var transactionGroupId = Guid.NewGuid();

            transactionOne.TransactionID = Guid.NewGuid();
            transactionOne.TransactionGroupID = transactionGroupId;
            transactionOne.LedgerID = Ledger.Constants.GOODS_ON_HAND;// 2;
            transactionOne.HubOwnerID = user.DefaultHubObj.HubOwner.HubOwnerID;
            transactionOne.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionOne.HubID = user.DefaultHub.Value;
            transactionOne.StoreID = viewModel.StoreId;  //
            transactionOne.ProjectCodeID = viewModel.ProjectCodeId;
            transactionOne.ShippingInstructionID = viewModel.ShippingInstructionId;

            transactionOne.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionOne.CommodityID = viewModel.CommodityId;
            transactionOne.ProgramID = viewModel.ProgramId;
            transactionOne.CommodityGradeID = null; // How did I get this value ?
            transactionOne.QuantityInMT = 0 - viewModel.QuantityInMt;
            transactionOne.QuantityInUnit = 0 - viewModel.QuantityInUint;
            transactionOne.UnitID = viewModel.UnitId;
            transactionOne.TransactionDate = DateTime.Now;



            Transaction transactionTwo = new Transaction();

            transactionTwo.TransactionID = Guid.NewGuid();
            transactionTwo.TransactionGroupID = transactionGroupId;
            transactionTwo.LedgerID = Ledger.Constants.LOSS_IN_TRANSIT;// 14;
            transactionTwo.HubOwnerID = user.DefaultHubObj.HubOwnerID;
            transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionTwo.HubID = user.DefaultHub.Value;
            transactionTwo.StoreID = viewModel.StoreId;  //
            transactionTwo.ProjectCodeID = viewModel.ProjectCodeId;
            transactionTwo.ShippingInstructionID = viewModel.ShippingInstructionId;
            transactionTwo.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionTwo.CommodityID = viewModel.CommodityId;
            transactionTwo.ProgramID = viewModel.ProgramId;
            transactionTwo.CommodityGradeID = null; // How did I get this value ?
            transactionTwo.QuantityInMT = viewModel.QuantityInMt;
            transactionTwo.QuantityInUnit = viewModel.QuantityInUint;
            transactionTwo.UnitID = viewModel.UnitId;
            transactionTwo.TransactionDate = DateTime.Now;

            transactionGroup.TransactionGroupID = transactionGroupId;
            transactionGroup.Transactions.Add(transactionOne);
            transactionGroup.Transactions.Add(transactionTwo);


            lossAndAdjustment.PartitionId = 0;
            lossAndAdjustment.AdjustmentID = Guid.NewGuid();
            lossAndAdjustment.TransactionGroupID = transactionGroupId;
            lossAndAdjustment.TransactionGroup = transactionGroup;
            lossAndAdjustment.HubID = user.DefaultHub.Value;
            lossAndAdjustment.AdjustmentReasonID = viewModel.ReasonId;
            lossAndAdjustment.AdjustmentDirection = "L";
            lossAndAdjustment.AdjustmentDate = viewModel.SelectedDate;
            lossAndAdjustment.ApprovedBy = viewModel.ApprovedBy;
            lossAndAdjustment.Remarks = viewModel.Description;
            lossAndAdjustment.UserProfileID = user.UserProfileID;
            lossAndAdjustment.ReferenceNumber = viewModel.MemoNumber;
            lossAndAdjustment.StoreManName = viewModel.StoreMan;



            // Try to save this transaction
            try
            {
                _unitOfWork.AdjustmentRepository.Add(lossAndAdjustment);
                _IWorkflowActivityService.EnterCreateWorkflow(lossAndAdjustment);

                _unitOfWork.Save();
            }
            catch (Exception exp)
            {
                // dbTransaction.Rollback();
                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Internal Movement Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="user"></param>

        public void SaveAdjustmentTrasnsaction(LossesAndAdjustmentsViewModel viewModel, UserProfile user)
        {


            Adjustment lossAndAdjustment = new Adjustment();
            TransactionGroup transactionGroup = new TransactionGroup();
            Transaction transactionOne = new Transaction();
            var transactionGroupId = Guid.NewGuid();


            Commodity commodity = _unitOfWork.CommodityRepository.FindById(viewModel.CommodityId);
            transactionOne.TransactionID = Guid.NewGuid();
            transactionOne.TransactionGroupID = transactionGroupId;
            transactionOne.LedgerID = Ledger.Constants.LOSS_IN_TRANSIT;//14;
            transactionOne.HubOwnerID = user.DefaultHubObj.HubOwner.HubOwnerID;
            transactionOne.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionOne.HubID = user.DefaultHub.Value;
            transactionOne.StoreID = viewModel.StoreId;  //
            transactionOne.ProjectCodeID = viewModel.ProjectCodeId;
            transactionOne.ShippingInstructionID = viewModel.ShippingInstructionId;
            transactionOne.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionOne.CommodityID = viewModel.CommodityId;
            transactionOne.ProgramID = viewModel.ProgramId;
            transactionOne.CommodityGradeID = null; // How did I get this value ?
            transactionOne.QuantityInMT = 0 - viewModel.QuantityInMt;
            transactionOne.QuantityInUnit = 0 - viewModel.QuantityInUint;
            transactionOne.UnitID = viewModel.UnitId;
            transactionOne.TransactionDate = DateTime.Now;



            Transaction transactionTwo = new Transaction();

            transactionTwo.TransactionID = Guid.NewGuid();
            transactionTwo.TransactionGroupID = transactionGroupId;
            transactionTwo.LedgerID = Ledger.Constants.GOODS_ON_HAND;// 2;
            transactionTwo.HubOwnerID = user.DefaultHubObj.HubOwnerID;
            transactionTwo.AccountID = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); //
            transactionTwo.HubID = user.DefaultHub;
            transactionTwo.StoreID = viewModel.StoreId;  //
            transactionTwo.ProjectCodeID = viewModel.ProjectCodeId;
            transactionTwo.ShippingInstructionID = viewModel.ShippingInstructionId;
            transactionTwo.ParentCommodityID = (commodity.ParentID == null)
                                                       ? commodity.CommodityID
                                                       : commodity.ParentID.Value;
            transactionTwo.CommodityID = viewModel.CommodityId;
            transactionTwo.ProgramID = viewModel.ProgramId;
            transactionTwo.CommodityGradeID = null; // How did I get this value ?
            transactionTwo.QuantityInMT = viewModel.QuantityInMt;
            transactionTwo.QuantityInUnit = viewModel.QuantityInUint;
            transactionTwo.UnitID = viewModel.UnitId;
            transactionTwo.TransactionDate = DateTime.Now;

            transactionGroup.TransactionGroupID = transactionGroupId;
            transactionGroup.Transactions.Add(transactionOne);
            transactionGroup.Transactions.Add(transactionTwo);

            lossAndAdjustment.TransactionGroupID = transactionGroupId;
            lossAndAdjustment.AdjustmentID = Guid.NewGuid();
            lossAndAdjustment.PartitionId = 0;
            lossAndAdjustment.TransactionGroup = transactionGroup;
            lossAndAdjustment.HubID = user.DefaultHub.Value;
            lossAndAdjustment.AdjustmentReasonID = viewModel.ReasonId;
            lossAndAdjustment.AdjustmentDirection = "A";
            lossAndAdjustment.AdjustmentDate = viewModel.SelectedDate;
            lossAndAdjustment.ApprovedBy = viewModel.ApprovedBy;
            lossAndAdjustment.Remarks = viewModel.Description;
            lossAndAdjustment.UserProfileID = user.UserProfileID;
            lossAndAdjustment.ReferenceNumber = viewModel.MemoNumber;
            lossAndAdjustment.StoreManName = viewModel.StoreMan;

            // Try to save this transaction
            try
            {
                _unitOfWork.AdjustmentRepository.Add(lossAndAdjustment);
                _IWorkflowActivityService.EnterCreateWorkflow(lossAndAdjustment);
                _unitOfWork.Save();
            }
            catch (Exception exp)
            {
                //dbTransaction.Rollback();
                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Loss / Adjustment Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }
        }

        /// <summary>
        /// Saves the loss adjustment transaction.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="user">The user.</param>
        public void SaveLossAdjustmentTransaction(LossesAndAdjustmentsViewModel viewModel, UserProfile user)
        {
            if (viewModel.IsLoss == true)
            {
                SaveLossTrasnsaction(viewModel, user);
            }
            else
            {
                SaveAdjustmentTrasnsaction(viewModel, user);
            }
        }


        /// <summary>
        /// Gets the total received from receipt allocation.
        /// </summary>
        /// <param name="siNumber">The si number.</param>
        /// <param name="commodityId">The commodity id.</param>
        /// <param name="hubId">The hub id.</param>
        /// <returns></returns>
        public decimal GetTotalReceivedFromReceiptAllocation(string siNumber, int commodityId, int hubId)
        {
            return GetTotalReceivedFromReceiptAllocation(
                _shippingInstructionService.GetShipingInstructionId(siNumber), commodityId, hubId);
        }


        /// <summary>
        /// Gets the commodity balance for hub.
        /// </summary>
        /// <param name="HubId">The hub id.</param>
        /// <param name="parentCommodityId">The parent commodity id.</param>
        /// <param name="si">The si.</param>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public decimal GetCommodityBalanceForHub(int HubId, int parentCommodityId, int si, int project)
        {
            var balance = _unitOfWork.TransactionRepository.Get(v =>
                                                                v.HubID == HubId &&
                                                                v.ParentCommodityID == parentCommodityId &&
                                                                v.ShippingInstructionID == si &&
                                                                v.ProjectCodeID == project &&
                                                                v.LedgerID == Ledger.Constants.GOODS_ON_HAND_UNCOMMITED)
                .Select(v => v.QuantityInMT).ToList();
            if (balance.Any())
            {
                return balance.Sum();
            }
            return 0;
        }


        /// <summary>
        /// Saves the starting balance transaction.
        /// </summary>
        /// <param name="startingBalance">The starting balance.</param>
        /// <param name="user">The user.</param>
        /// <exception cref="System.Exception"></exception>
        public void SaveStartingBalanceTransaction(StartingBalanceViewModel startingBalance, UserProfile user)
        {
            int repositoryAccountGetAccountIDWithCreateNegative = _accountService.GetAccountIdWithCreate(Account.Constants.DONOR, startingBalance.DonorID); ;

            int repositoryProjectCodeGetProjectCodeIdWIthCreateProjectCodeID = _projectCodeService.GetProjectCodeIdWIthCreate(startingBalance.ProjectNumber).ProjectCodeID; ;
            int repositoryShippingInstructionGetSINumberIdWithCreateShippingInstructionID = _shippingInstructionService.GetSINumberIdWithCreate(startingBalance.SINumber).ShippingInstructionID; ;
            int repositoryAccountGetAccountIDWithCreatePosetive = _accountService.GetAccountIdWithCreate(Account.Constants.HUB, user.DefaultHub.Value); ;

            TransactionGroup transactionGroup = new TransactionGroup();

            Transaction transactionOne = new Transaction();

            var transactionGroupId = Guid.NewGuid();

            transactionOne.TransactionID = Guid.NewGuid();
            transactionOne.TransactionGroupID = transactionGroupId;
            transactionOne.PartitionId = 0;
            transactionOne.LedgerID = Ledger.Constants.GOODS_UNDER_CARE;
            transactionOne.HubOwnerID = user.DefaultHubObj.HubOwner.HubOwnerID;
            transactionOne.AccountID = repositoryAccountGetAccountIDWithCreateNegative;
            transactionOne.HubID = user.DefaultHub.Value;
            transactionOne.StoreID = startingBalance.StoreID;
            transactionOne.Stack = startingBalance.StackNumber;
            transactionOne.ProjectCodeID = repositoryProjectCodeGetProjectCodeIdWIthCreateProjectCodeID;
            transactionOne.ShippingInstructionID = repositoryShippingInstructionGetSINumberIdWithCreateShippingInstructionID;
            transactionOne.ProgramID = startingBalance.ProgramID;
            var comm = _unitOfWork.CommodityRepository.FindById(startingBalance.CommodityID);

            transactionOne.CommodityID = (comm.ParentID != null)
                                                      ? comm.ParentID.Value
                                                      : comm.CommodityID;

            //transactionOne.ParentCommodityID = (comm.ParentID != null)
            //                                           ? comm.ParentID.Value
            //                                           : comm.CommodityID;
            transactionOne.CommodityChildID = startingBalance.CommodityID;
            transactionOne.CommodityGradeID = null;
            transactionOne.QuantityInMT = 0 - startingBalance.QuantityInMT;
            transactionOne.QuantityInUnit = 0 - startingBalance.QuantityInUnit;
            transactionOne.UnitID = startingBalance.UnitID;
            transactionOne.TransactionDate = DateTime.Now;

            Transaction transactionTwo = new Transaction();

            transactionTwo.TransactionID = Guid.NewGuid();
            transactionTwo.TransactionGroupID = transactionGroupId;
            transactionTwo.PartitionId = 0;
            transactionTwo.LedgerID = Ledger.Constants.GOODS_ON_HAND;
            transactionTwo.HubOwnerID = user.DefaultHubObj.HubOwnerID;
            transactionTwo.AccountID = repositoryAccountGetAccountIDWithCreatePosetive;
            transactionTwo.HubID = user.DefaultHub.Value;
            transactionTwo.StoreID = startingBalance.StoreID;
            transactionTwo.Stack = startingBalance.StackNumber;
            transactionTwo.ProjectCodeID = repositoryProjectCodeGetProjectCodeIdWIthCreateProjectCodeID;
            transactionTwo.ShippingInstructionID = repositoryShippingInstructionGetSINumberIdWithCreateShippingInstructionID;
            transactionTwo.ProgramID = startingBalance.ProgramID;

            transactionTwo.CommodityID = (comm.ParentID != null)
                                                      ? comm.ParentID.Value
                                                      : comm.CommodityID;

            //transactionTwo.ParentCommodityID = (comm.ParentID != null)
            //                                           ? comm.ParentID.Value
            //                                           : comm.CommodityID;
            transactionTwo.CommodityChildID = startingBalance.CommodityID;
            transactionTwo.CommodityGradeID = null; // How did I get this value ?
            transactionTwo.QuantityInMT = startingBalance.QuantityInMT;
            transactionTwo.QuantityInUnit = startingBalance.QuantityInUnit;
            transactionTwo.UnitID = startingBalance.UnitID;
            transactionTwo.TransactionDate = DateTime.Now;

            transactionGroup.PartitionId = 0;

            try
            {
                transactionGroup.TransactionGroupID = transactionGroupId;
                transactionGroup.Transactions.Add(transactionOne);
                transactionGroup.Transactions.Add(transactionTwo);
                _unitOfWork.TransactionGroupRepository.Add(transactionGroup);
                _unitOfWork.Save();

            }
            catch (Exception exp)
            {

                //TODO: Save the detail of this exception somewhere
                throw new Exception("The Starting Balance Transaction Cannot be saved. <br />Detail Message :" + exp.Message);
            }
        }


        /// <summary>
        /// Gets the list of starting balances.
        /// </summary>
        /// <param name="hubID">The hub ID.</param>
        /// <returns></returns>
        public List<StartingBalanceViewModelDto> GetListOfStartingBalances(int hubID)
        {


            return (from t in _unitOfWork.TransactionRepository.Get(t => t.Account != null, null, "ProjectCode,Program,Commodity,Account,TransactionGroup.ReceiveDetails,TransactionGroup.DispatchDetails,TransactionGroup.InternalMovements,TransactionGroup.Adjustments")
                    where
                    !t.TransactionGroup.ReceiveDetails.Any()
                    &&
                    !t.TransactionGroup.DispatchDetails.Any()
                    &&
                    !t.TransactionGroup.InternalMovements.Any()
                    &&
                    !t.TransactionGroup.Adjustments.Any()
                    &&
                    t.HubID == hubID

                    join d in _unitOfWork.DonorRepository.Get() on t.Account.EntityID equals d.DonorID
                    where t.Account.EntityType == "Donor"
                    let firstOrDefault = _unitOfWork.CommodityRepository.FindBy(c => c.CommodityID == t.CommodityChildID).FirstOrDefault()
                    where firstOrDefault != null
                    select new StartingBalanceViewModelDto()
                    {
                        CommodityName = t.Commodity.Name,
                        ChildCommodity = firstOrDefault.Name,
                        SINumber = t.ShippingInstruction.Value,
                        ProgramName = t.Program.Name,
                        ProjectCode = t.ProjectCode.Value,
                        QuantityInMT = Math.Abs(t.QuantityInMT),
                        QuantityInUnit = Math.Abs(t.QuantityInUnit),
                        StackNumber = (t.Stack.HasValue) ? t.Stack.Value : 0,
                        StoreName = t.Store.Name,
                        UnitName = t.Unit.Name,
                        DonorName = d.Name,
                    }).ToList();
        }


        /// <summary>
        /// Gets the offloading report.
        /// </summary>
        /// <param name="hubID">The hub ID.</param>
        /// <param name="dispatchesViewModel">The dispatches view model.</param>
        /// <returns></returns>
        public List<OffloadingReport> GetOffloadingReport(int hubID, DispatchesViewModel dispatchesViewModel)
        {
            DateTime sTime = DateTime.Now;
            DateTime eTime = DateTime.Now;

            if (!dispatchesViewModel.PeriodId.HasValue || dispatchesViewModel.PeriodId == 6)
            {
                //filter it to only the current week
                //sTime = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                eTime = sTime.AddDays(7).Date;
            }
            else
            {
                //start end date filters
                if (dispatchesViewModel.PeriodId == 8)
                {
                    sTime = dispatchesViewModel.StartDate ?? sTime;
                    eTime = dispatchesViewModel.EndDate ?? eTime;
                }
                //allocation round
                else if (dispatchesViewModel.PeriodId == 9)
                {
                }
                //allocation year + month
                else if (dispatchesViewModel.PeriodId == 9)
                {
                }
            }

            string StartTime = sTime.ToShortDateString();
            string EndTime = eTime.ToShortDateString();
            // string HUbName = repository.Hub.FindById(hubID).HubNameWithOwner;
            var dbGetOffloadingReport = _unitOfWork.ReportRepository.RPT_Offloading(hubID, sTime, eTime).ToList();

            if (dispatchesViewModel.ProgramId.HasValue && dispatchesViewModel.ProgramId != 0)
            {
                dbGetOffloadingReport = dbGetOffloadingReport.Where(p => p.ProgramID == dispatchesViewModel.ProgramId).ToList();
            }
            if (dispatchesViewModel.AreaId.HasValue && dispatchesViewModel.AreaId != 0)
            {
                dbGetOffloadingReport = dbGetOffloadingReport.Where(p => p.RegionID == dispatchesViewModel.AreaId).ToList();
            }
            if (dispatchesViewModel.bidRefId != null)
            {
                dbGetOffloadingReport = dbGetOffloadingReport.Where(p => p.BidRefNo == dispatchesViewModel.bidRefId).ToList();
            }


            return (from t in dbGetOffloadingReport
                    group t by new { t.BidRefNo, t.ProgramName, t.Round, t.PeriodMonth, t.PeriodYear, t.RegionName }
                        into b
                    select new OffloadingReport()
                    {
                        ContractNumber = b.Key.BidRefNo,
                        EndDate = EndTime,
                        StartDate = StartTime,
                        Month = Convert.ToString(b.Key.PeriodMonth),
                        Round = Convert.ToString(b.Key.Round),
                        Year = b.Key.PeriodYear,//??0, modified Banty 23_5_13
                        Region = b.Key.RegionName,
                        Program = b.Key.ProgramName,
                        OffloadingDetails = b.Select(t1 => new OffloadingDetail()
                        {
                            RequisitionNumber = t1.RequisitionNo,
                            Product = t1.CommodityName,
                            Zone = t1.ZoneName,
                            Woreda = t1.WoredaName,
                            Destination = t1.FDPName,
                            Allocation = t1.AllocatedInMT ?? 0,
                            Dispatched = t1.DispatchedQuantity ?? 0,
                            Remaining = t1.RemainingAmount ?? 0,
                            Transporter = t1.TransaporterName,
                            Donor = t1.DonorName
                        }).ToList()

                    }).ToList();
        }


        /// <summary>
        /// Gets the receive report.
        /// </summary>
        /// <param name="hubID">The hub ID.</param>
        /// <param name="receiptsViewModel">The receipts view model.</param>
        /// <returns></returns>
        public List<ReceiveReport> GetReceiveReport(int hubID, ReceiptsViewModel receiptsViewModel)
        {
            DateTime sTime = DateTime.Now;
            DateTime eTime = DateTime.Now;

            if (!receiptsViewModel.PeriodId.HasValue)
            {
                sTime = new DateTime(1979, 01, 01, 00, 00, 00, 000);
            }
            else
            {
                //start end date filters
                if (receiptsViewModel.PeriodId == 8)
                {
                    sTime = receiptsViewModel.StartDate ?? sTime;
                    eTime = receiptsViewModel.EndDate ?? eTime;
                }
                //allocation round
                else if (receiptsViewModel.PeriodId == 6)
                {
                    //filter it to only the current week
                    // sTime = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                    eTime = sTime.AddDays(7).Date;
                }
                //allocation year + month
                else if (receiptsViewModel.PeriodId == 9)
                {
                }
            }

            string StartTime = sTime.ToShortDateString();
            string EndTime = eTime.ToShortDateString();
            // string HUbName = repository.Hub.FindById(hubID).HubNameWithOwner;
            var dbGetReceiptReport = _unitOfWork.ReportRepository.RPT_ReceiptReport(hubID, sTime, eTime).ToList();

            if (receiptsViewModel.ProgramId.HasValue && receiptsViewModel.ProgramId != 0)
            {
                dbGetReceiptReport = dbGetReceiptReport.Where(p => p.ProgramID == receiptsViewModel.ProgramId).ToList();
            }
            if (receiptsViewModel.CommodityTypeId.HasValue && receiptsViewModel.CommodityTypeId != 0)
            {
                dbGetReceiptReport = dbGetReceiptReport.Where(p => p.CommodityTypeID == receiptsViewModel.CommodityTypeId).ToList();
            }

            return (from t in dbGetReceiptReport
                    group t by new { t.BudgetYear }
                        into b
                    select new ReceiveReport()
                    {
                        BudgetYear = b.Key.BudgetYear.Value,
                        rows = b.Select(t1 => new ReceiveRow()
                        {
                            Product = t1.CommodityName,
                            Program = t1.ProgramName,
                            Quantity = t1.BalanceInMt.Value,
                            Quarter = t1.Quarter.Value
                            /*MeasurementUnit = t1.BalanceInUnit.Value*/

                        }).ToList()

                    }).ToList();
        }


        /// <summary>
        /// Gets the distribution report.
        /// </summary>
        /// <param name="hubID">The hub ID.</param>
        /// <param name="distributionViewModel">The distribution view model.</param>
        /// <returns></returns>
        public List<DistributionRows> GetDistributionReport(int hubID, DistributionViewModel distributionViewModel)
        {

            var dbDistributionReport = _unitOfWork.ReportRepository.RPT_Distribution(hubID).ToList();

            if (distributionViewModel.PeriodId.HasValue && distributionViewModel.PeriodId != 0)
            {
                dbDistributionReport = dbDistributionReport.Where(p => p.Quarter == distributionViewModel.PeriodId.Value).ToList();
            }
            if (distributionViewModel.ProgramId.HasValue && distributionViewModel.ProgramId != 0)
            {
                dbDistributionReport = dbDistributionReport.Where(p => p.ProgramID == distributionViewModel.ProgramId.Value).ToList();
            }
            if (distributionViewModel.AreaId.HasValue && distributionViewModel.AreaId != 0)
            {
                dbDistributionReport = dbDistributionReport.Where(p => p.RegionID == distributionViewModel.AreaId.Value).ToList();
            }

            return (from t in dbDistributionReport
                        //  group t by new { t.BudgetYear }
                        //      into b
                    select new DistributionRows()
                    {
                        BudgetYear = t.PeriodYear,
                        Program = t.ProgramName,
                        DistributedAmount = t.DispatchedQuantity.Value,
                        Month = Convert.ToString(t.PeriodMonth),
                        Quarter = t.Quarter.Value,
                        Region = t.RegionName
                    }).ToList();
        }

        public IEnumerable<Transaction> getTransactionsAsof(DateTime date)
        {
            return _unitOfWork.TransactionRepository.FindBy(d => d.TransactionDate <= date);
        }

        public bool DeleteById(System.Guid id)
        {
            var original = FindById(id);
            if (original == null) return false;
            _unitOfWork.TransactionRepository.Delete(original);
            _unitOfWork.Save();
            return true;
        }

        public Transaction FindById(System.Guid id)
        {
            return _unitOfWork.TransactionRepository.Get(t => t.TransactionID == id).FirstOrDefault();
        }

        public IEnumerable<Transaction> Get(System.Linq.Expressions.Expression<Func<Transaction, bool>> filter = null, Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.TransactionRepository.Get(filter, orderBy, includeProperties);
        }
    }
}
