using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;

namespace Cats.Services.Dashboard
{
    public interface ILgDashboardService: IDisposable
    {
        List<DashboardDispAlloRequisition> DispatchAllocatedRequisitions(DateTime? startDate, DateTime? endDate, int round);
        List<int?> GetRounds();

        List<DashboardBidPlan> BidPlanEntryStatus(DateTime? startDate, DateTime? endDate);
    }
}
