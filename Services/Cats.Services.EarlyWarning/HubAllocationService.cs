﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{

    public class HubAllocationService : IHubAllocationService
    {
        private readonly IUnitOfWork _unitOfWork;


        public HubAllocationService(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #region Default Service Implementation
        public bool AddHubAllocation(HubAllocation hubAllocation)
        {
            _unitOfWork.HubAllocationRepository.Add(hubAllocation);

            //var requisition = _unitOfWork.ReliefRequisitionRepository.FindBy(r => r.RequisitionID == hubAllocation.RequisitionID).Single();
            //requisition.Status = 3;

            _unitOfWork.Save();
            return true;
        }

        public void AddHubAllocations(IEnumerable<Allocation> allocations, int userProfileId)
        {
            foreach (var appRequisition in allocations)
            {
                var newHubAllocation = new HubAllocation
                {
                    AllocatedBy = userProfileId,
                    RequisitionID = appRequisition.ReqId,
                    AllocationDate = DateTime.Now.Date,
                    ReferenceNo = "001",
                    HubID = appRequisition.HubId,

                   // StoreId = appRequisition.StoreId,

                    SatelliteWarehouseID= appRequisition.SatelliteWarehouseID

                };
                AddHubAllocation(newHubAllocation);
            }
        }

        public bool EditHubAllocation(HubAllocation hubAllocation)
        {
            _unitOfWork.HubAllocationRepository.Edit(hubAllocation);
            //var requisition = _unitOfWork.ReliefRequisitionRepository.FindBy(r => r.RequisitionID == hubAllocation.RequisitionID).Single();
            //requisition.Status = 3;
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteHubAllocation(HubAllocation hubAllocation)
        {
            if (hubAllocation == null) return false;
            _unitOfWork.HubAllocationRepository.Delete(hubAllocation);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.HubAllocationRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.HubAllocationRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<HubAllocation> GetAllHubAllocation()
        {
            return _unitOfWork.HubAllocationRepository.GetAll();
        }
        public HubAllocation FindById(int id)
        {
            return _unitOfWork.HubAllocationRepository.FindById(id);
        }
        public List<HubAllocation> FindBy(Expression<Func<HubAllocation, bool>> predicate)
        {
            return _unitOfWork.HubAllocationRepository.FindBy(predicate);
        }
        #endregion

       

        public string GetAllocatedHub(int id)
        {
            var HubAllocatedRequest = _unitOfWork.HubAllocationRepository.Get(r => r.RequisitionID == id).SingleOrDefault();
            if (HubAllocatedRequest == null) return null;
            return HubAllocatedRequest.Hub.Name;
        }

        public int GetAllocatedHubId(int id)
        {
            try
            {
                var HubAllocatedRequest = _unitOfWork.HubAllocationRepository.Get(r => r.RequisitionID == id).First();
                if (HubAllocatedRequest == null) return 0;

                return HubAllocatedRequest.Hub.HubID;
            }
            catch (Exception e)
            {

            }
            finally
            {
                
            }
            return 0;
        }

        public HubAllocation GetAllocatedHubByRequisitionNo(int requisitionNo)
        {
            var hubAllocatedRequest = _unitOfWork.HubAllocationRepository.Get(r => r.RequisitionID == requisitionNo).SingleOrDefault();
            if (hubAllocatedRequest == null) return null;
            return hubAllocatedRequest;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

    }
}


