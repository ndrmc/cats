using Cats.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Services.Workflows.Config
{
   public class DashboardMapping
    {
        public static Dictionary<String, List<Type>> PageNameToWorkflowMapping = new Dictionary<String, List<Type>>();

        public static void MappingConfig()
        {
            AddMapping("EarlyWarningDashboard", typeof(GiftCertificate));
            AddMapping("EarlyWarningDashboard", typeof(Transporter));
            AddMapping("RegionalDashboard", typeof(Dispatch));

        }
        public static void AddMapping(String pageName,Type workflowImplementer )
        {
            

        }
    }
}
