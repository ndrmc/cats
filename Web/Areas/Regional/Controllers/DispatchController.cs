using System.Linq;
using System.Web.Mvc;
using Cats.Helpers;
using Cats.Services.Dashboard;
using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using Cats.Web.Hub;
using IAdminUnitService = Cats.Services.Hub.IAdminUnitService;

namespace Cats.Areas.Regional.Controllers
{
    [Authorize]
    public class DispatchController : BaseController
    {

        private readonly IReliefRequisitionService _reliefRequisitionService;

        private readonly IRegionalDashboard _regionalDashboard;
        private readonly IUserProfileService _userProfileService;
        private readonly IAdminUnitService _adminUnitService;
        public DispatchController(
       IUserProfileService userProfileService, IRegionalDashboard regionalDashboard, IAdminUnitService adminUnitService)
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
            var result = _regionalDashboard.GetAllRecentDispatches(regionID).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}