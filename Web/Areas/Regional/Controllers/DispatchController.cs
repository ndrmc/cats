using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Cats.Models.Hubs;
using Cats.Models.ViewModels;
using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using Cats.Services.Common;
using Cats.Services.Dashboard;
using Cats.ViewModelBinder;
using Cats.Web.Hub;
using Cats.Web.Hub.Helpers;
using LanguageHelpers.Localization;
using Newtonsoft.Json;
using Telerik.Web.Mvc;
using System;
using Cats.Helpers;


using Cats.Models.Hubs;
using Cats.Models.Hubs.ViewModels;
using Cats.Models.Hubs.ViewModels.Common;
using Cats.Models.Hubs.ViewModels.Dispatch;
using Cats.Services.Dashboard;
using Cats.Services.Logistics;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using IAdminUnitService = Cats.Services.Hub.IAdminUnitService;
using ICommodityService = Cats.Services.Hub.ICommodityService;
using ICommodityTypeService = Cats.Services.Hub.ICommodityTypeService;
using IFDPService = Cats.Services.Hub.IFDPService;
using IHubService = Cats.Services.Hubs.IHubService;
using IProgramService = Cats.Services.Hub.IProgramService;
using IProjectCodeService = Cats.Services.Hub.IProjectCodeService;
using IShippingInstructionService = Cats.Services.Hub.IShippingInstructionService;
using IUnitService = Cats.Services.Hub.IUnitService;
using UserAccountHelper = Cats.Helpers.UserAccountHelper;

namespace Cats.Areas.Regional.Controllers
{
    [Authorize]
    public class DispatchController: BaseController
    {
        
        private readonly IReliefRequisitionService _reliefRequisitionService;
       
        private readonly IRegionalDashboard _regionalDashboard;
        private readonly IUserProfileService _userProfileService;
        private readonly IAdminUnitService _adminUnitService;
        public DispatchController(
       IUserProfileService userProfileService, IRegionalDashboard regionalDashboard,IAdminUnitService adminUnitService)
       : base(userProfileService)
        {         
            _regionalDashboard = regionalDashboard;
            _userProfileService = userProfileService;
            _adminUnitService = adminUnitService;
        }
        public ViewResult Index()
        {
            var currentUser = UserAccountHelper.GetUser(HttpContext.User.Identity.Name);

            if (!currentUser.RegionalUser || currentUser.RegionID == null)
            {
                ModelState.AddModelError("Errors", @"You are not assigned to any region");
            }

            ViewBag.RegionID = currentUser.RegionID;
            ViewBag.RegionName = currentUser.RegionID != null ? _adminUnitService.FindById(currentUser.RegionID ?? 0).Name : "[region not set for user]";
            return View();
        }
        public JsonResult Dispatches(int regionID)
        {
            var result = _regionalDashboard.GetAllRecentDispatches(regionID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}