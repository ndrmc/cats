﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Data.UnitWork;
using Cats.Models;
using System.Linq;

namespace Cats.Services.EarlyWarning
{
    public class ProjectCodeAllocationService : IProjectCodeAllocationService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ProjectCodeAllocationService()
        {
            this._unitOfWork = new UnitOfWork();
        }
      

        #region Implementation of Service
        public bool AddProjectCodeAllocation(ProjectCodeAllocation _ProjectCodeAllocationDetail,int requisitionId,bool IsLastAssignment)
        {
            if (IsLastAssignment)
            {
                var requisition = _unitOfWork.ReliefRequisitionRepository.FindBy(r => r.RequisitionID == requisitionId).Single();
                requisition.Status = 4;
            }
            

            _unitOfWork.ProjectCodeAllocationRepository.Add(_ProjectCodeAllocationDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool AddProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail)
        {
            

            _unitOfWork.ProjectCodeAllocationRepository.Add(_ProjectCodeAllocationDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool EditProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail)
        {
            _unitOfWork.ProjectCodeAllocationRepository.Edit(_ProjectCodeAllocationDetail);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteProjectCodeAllocationDetail(ProjectCodeAllocation _ProjectCodeAllocationDetail)
        {
            if (_ProjectCodeAllocationDetail == null) return false;
            _unitOfWork.ProjectCodeAllocationRepository.Delete(_ProjectCodeAllocationDetail);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.ProjectCodeAllocationRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.ProjectCodeAllocationRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public bool Save()
        {
            
            _unitOfWork.Save();
            return true;
        }
        public bool Save(ProjectCodeAllocation _projectAllocation)
        {
            _unitOfWork.ProjectCodeAllocationRepository.Add(_projectAllocation);
            var hubAllocation = _unitOfWork.HubAllocationRepository.FindById(_projectAllocation.HubAllocationID);
            var requisition = _unitOfWork.ReliefRequisitionRepository.FindById(hubAllocation.RequisitionID);
            requisition.Status = 4;
            _unitOfWork.Save();
            
            return true;
        }
        public IEnumerable<ProjectCodeAllocation> GetAllProjectCodeAllocationDetail()
        {
            return _unitOfWork.ProjectCodeAllocationRepository.GetAll();
        }
        public ProjectCodeAllocation FindById(int id)
        {
            return _unitOfWork.ProjectCodeAllocationRepository.FindById(id);
        }
        public IEnumerable<ProjectCodeAllocation> FindBy(Expression<Func<ProjectCodeAllocation, bool>> predicate)
        {
            return _unitOfWork.ProjectCodeAllocationRepository.FindBy(predicate);
        }
 
        #endregion


        public void Dispose()
        {
            _unitOfWork.Dispose();

        }


        //public bool SaveProjectCodeAllocation(IEnumerable<ProjectCodeAllocation> projectCodeAlloation)
        //{
        //    try{
        //        foreach (var item in projectCodeAlloation)
        //            {
        //                var tempProjectCodeAllocation=FindById(item.HubAllocationID);
        //                    tempProjectCodeAllocation.ProjectCodeID=item.ProjectCodeID;
        //                    tempProjectCodeAllocation.ShippingInstructionID = item.ShippingInstructionID;
        //                    this.EditProjectCodeAllocationDetail(tempProjectCodeAllocation);
        //            }
        //            return true;
        //        }
        //        catch
        //            {
        //                return false;
        //            }
            
        //}

        public List<HubAllocation> GetHubAllocation(Expression<Func<HubAllocation, bool>> predicate)
        {
            return _unitOfWork.HubAllocationRepository.FindBy(predicate);
        }

        public List<HubAllocation> GetAllRequisitionsInHubAllocation()
        {
            return _unitOfWork.HubAllocationRepository.GetAll();
        }

        public IEnumerable<HubAllocation> Get(
          Expression<Func<HubAllocation, bool>> filter = null,
          Func<IQueryable<HubAllocation>, IOrderedQueryable<HubAllocation>> orderBy = null,
          string includeProperties = "")
        {
            return _unitOfWork.HubAllocationRepository.Get(filter, orderBy, includeProperties);
        }

        //public IEnumerable<ReliefRequisition> GetReliefRequisition(
        //  Expression<Func<ReliefRequisition, bool>> filter = null,
        //  Func<IQueryable<ReliefRequisition>, IOrderedQueryable<ReliefRequisition>> orderBy = null,
        //  string includeProperties = "")
        //{
        //    return _unitOfWork.ReliefRequisitionRepository.Get(filter, orderBy, includeProperties);
        //}

        public List<ProjectCodeAllocation> GetHubAllocationByHubID(int status)
        {
            return _unitOfWork.ProjectCodeAllocationRepository.Get(t => t.HubAllocationID == status).ToList();
        }
        public List<ProjectCodeAllocation> GetHubAllocationByID(int hubID)
        {
            return _unitOfWork.ProjectCodeAllocationRepository.Get(h=>h.HubAllocationID == hubID).ToList();
        } 
        
    }
}
