using System.Collections.Generic;
using Cats.Models.Shared.DashBoardModels;

namespace Cats.Areas.WorkflowManager.Models
{
    public class DashboarDataEntryModel
    {
        public string Name { get; set; }
        public List<DashboardDataEntry> DashboardDataEntries { get; set; } 
    }

    public class DashboardFilterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}