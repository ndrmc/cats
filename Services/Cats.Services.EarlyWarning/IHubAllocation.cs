
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IHubAllocationService
    {

        bool AddHubAllocation(HubAllocation hubAllocation);

        void AddHubAllocations(IEnumerable<Allocation> allocations, int userProfileId);
        bool DeleteHubAllocation(HubAllocation hubAllocation);
        bool DeleteById(int id);
        bool EditHubAllocation(HubAllocation hubAllocation);
        HubAllocation FindById(int id);
        List<HubAllocation> GetAllHubAllocation();
        List<HubAllocation> FindBy(Expression<Func<HubAllocation, bool>> predicate);
        HubAllocation GetAllocatedHubByRequisitionNo(int requisitionNo);
        string GetAllocatedHub(int id);
        int GetAllocatedHubId(int id);

    }
}


      
