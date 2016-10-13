using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats.Models.ViewModels.Dashboard;

namespace Cats.Services.Dashboard
{
    public interface IRegionalDashboard:IDisposable
    {
        List<RecentRequests> GetRecentRequests(int regionID);
        List<RecentRequisitions> GetRecentRequisitions(int regionID);
        List<Object> RequisitionsPercentage(int regionID);
        List<Object> GetRecentDispatches(int regionID);
        List<Object> GetAllRecentDispatches(int regionID);
        List<RegionalRequestAllocationChange> GetAllocationChange(int regionID);
        List<DistibtionStatusView> GetDistributions(int regionID);
        decimal GetRegionalNotReconcileDispatchAmount(int regionId);
        IEnumerable<decimal> ExecWithStoreProcedure(string query, params object[] parameters);
    }
}