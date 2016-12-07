﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Services.Workflows;

namespace Cats.Services.Finance
{

    public class TransporterChequeService : ITransporterChequeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowActivityService _workflowActivityService;

        private int PartitionId = 1;


        public TransporterChequeService(IUnitOfWork unitOfWork,IWorkflowActivityService workflowActivityService)
        {
            this._unitOfWork = unitOfWork;
            this._workflowActivityService = workflowActivityService;
        }
        #region Default Service Implementation
        public bool AddTransporterCheque(TransporterCheque transporterCheque)
        {

            _unitOfWork.TransporterChequeRepository.Add(transporterCheque);

            _unitOfWork.Save();
            return true;

        }
        public bool EditTransporterCheque(TransporterCheque transporterCheque)
        {
        

            _unitOfWork.TransporterChequeRepository.Edit(transporterCheque);

            _workflowActivityService.EnterEditWorkflow(transporterCheque);

            _unitOfWork.Save();


            return true;

        }
        public bool DeleteTransporterCheque(TransporterCheque transporterCheque)
        {
            if (transporterCheque == null) return false;
            _unitOfWork.TransporterChequeRepository.Delete(transporterCheque);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.TransporterChequeRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.TransporterChequeRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<TransporterCheque> GetAllTransporterCheque()
        {
            return _unitOfWork.TransporterChequeRepository.GetAll();
        }
        public TransporterCheque FindById(int id)
        {
            return _unitOfWork.TransporterChequeRepository.FindById(id);
        }
        public List<TransporterCheque> FindBy(Expression<Func<TransporterCheque, bool>> predicate)
        {
            return _unitOfWork.TransporterChequeRepository.FindBy(predicate);
        }
        public IEnumerable<TransporterCheque> Get(System.Linq.Expressions.Expression<Func<TransporterCheque, bool>> filter = null,
                                    Func<IQueryable<TransporterCheque>, IOrderedQueryable<TransporterCheque>> orderBy = null,
                                    string includeProperties = "")
        {
            return _unitOfWork.TransporterChequeRepository.Get(filter, orderBy, includeProperties);
        }
        #endregion

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

    }
}

 
      
