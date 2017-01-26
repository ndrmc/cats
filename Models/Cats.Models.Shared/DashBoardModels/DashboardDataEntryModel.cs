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
        public int ProcessTemplateID { get; set; }
        public int StateTemplateID { get; set; }
        public int BusinessProcessID { get; set; }
        public Object Detail { get; set; }

        //public String Comment { get; set; }
        public DateTime DatePerformed { get; set; }
    }
}
