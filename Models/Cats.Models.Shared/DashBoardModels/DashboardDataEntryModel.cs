using System;
using System.Collections.Generic;

namespace Cats.Models.Shared.DashBoardModels
{
    public class DashboardDataEntry
    {
        //public Int32 Row { get; set; }
        public string PerformedBy { get; set; }
        public string SettingName { get; set; }
        public string ActivityName { get; set; }
        public int ActivityCount { get; set; } // Frequency count for state types
    }
}
