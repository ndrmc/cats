﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cats.Services.Hub;
using Cats.Models.Hubs.ViewModels.Report;

namespace Cats.Services.Hub
{
    public class ConstantsService 
    {
        public static List<Models.Hubs.ViewModels.Report.CodesViewModel> GetAllCodes()
        {
            List<Models.Hubs.ViewModels.Report.CodesViewModel> codes = new List<Models.Hubs.ViewModels.Report.CodesViewModel>();
            codes.Add(new Models.Hubs.ViewModels.Report.CodesViewModel { CodesId = 0, CodesName = "All Codes" });
            codes.Add(new Models.Hubs.ViewModels.Report.CodesViewModel { CodesId = 1, CodesName = "Particular SI" });
            codes.Add(new Models.Hubs.ViewModels.Report.CodesViewModel { CodesId = 2, CodesName = "Particular PC" });
            return codes;
        }


        public static List<Models.Hubs.ViewModels.Report.TypeViewModel> GetAllTypes()
        {
            List<Models.Hubs.ViewModels.Report.TypeViewModel> types = new List<Models.Hubs.ViewModels.Report.TypeViewModel>();
            types.Add(new Models.Hubs.ViewModels.Report.TypeViewModel { TypeId = 0, TypeName = "All Types" });
            types.Add(new Models.Hubs.ViewModels.Report.TypeViewModel { TypeId = 1, TypeName = "FDP Dispatch" });
            types.Add(new Models.Hubs.ViewModels.Report.TypeViewModel { TypeId = 2, TypeName = "Transfer To Other Hub" });
            return types;
        }
    }
}
