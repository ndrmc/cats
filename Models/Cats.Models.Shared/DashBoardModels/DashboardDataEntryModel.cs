using System;
using System.Collections.Generic;

namespace Cats.Models.Shared.DashBoardModels
{
    public class DashboardDataEntry
    {
        public string UserName { get; set; }        
        public string ActivityName { get; set; }
        public int ActivityCount { get; set; }
    }
}
