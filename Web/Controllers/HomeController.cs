using System.Globalization;
using Cats.Helpers;
using Cats.Models;
using Cats.Services.EarlyWarning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Cats.Data.UnitWork;
using Cats.Services.Common;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Cats.Models.ViewModels;
using Cats.Services.Security;
using UserProfile = Cats.Models.Security.UserProfile;

namespace Cats.Controllers
{
    public class HomeController : Controller
    {
        private IRegionalRequestService _regionalRequestService;
        private IReliefRequisitionService _reliefRequistionService;
        private IUnitOfWork _unitOfWork;
        private IUserDashboardPreferenceService _userDashboardPreferenceService;
        private IDashboardWidgetService _dashboardWidgetService;
        private IUserAccountService userService;
        private readonly IDashboardService _IDashboardService;
        private readonly IUserAccountService _userAccountService;
        private readonly INotificationService _notificationService;
        public HomeController(IUserDashboardPreferenceService userDashboardPreferenceService,
            IDashboardWidgetService dashboardWidgetService,
            IUserAccountService _userService,
            IUnitOfWork unitOfWork,
            IRegionalRequestService regionalRequestService,
            IReliefRequisitionService reliefRequisitionService, IDashboardService iDashboardService, IUserAccountService userAccountService, INotificationService notificationService)
        {
            _regionalRequestService = regionalRequestService;
            _reliefRequistionService = reliefRequisitionService;
            _IDashboardService = iDashboardService;
            this._userAccountService = userAccountService;
            _notificationService = notificationService;
            _userDashboardPreferenceService = userDashboardPreferenceService;
            _dashboardWidgetService = dashboardWidgetService;
            this.userService = _userService;
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /Home/
        //     [Authorize]
        public ActionResult Index()
        {
            //var req = _reliefRequistionService.FindBy(t => t.RegionID == regionId);
            //var req = _regionalRequestService.FindBy(t => t.RegionID == regionId);
            //ViewBag.Requests = req;

            var currentUser = UserAccountHelper.GetUser(HttpContext.User.Identity.Name);

            var userID = currentUser.UserProfileID;
            if (currentUser.IsAdmin) {
                return RedirectToAction("Index", "AdminDashboard", new { Area = "Settings" });
            }
			if (currentUser.DefaultHub!= null) {
                return RedirectToAction("Index", "Home", new { Area = "Hub" });
            }
            if (currentUser.RegionalUser)
            {
                ViewBag.RegionID = currentUser.RegionID;
                return RedirectToAction("Index", "Home", new { Area = "Regional" });
            }
                switch (currentUser.CaseTeam)
                {
                    case 1:
                        return RedirectToAction("Index", "Home", new { Area = "EarlyWarning" });
                        break;
                    case 2:
                        return RedirectToAction("Index", "Home", new { Area = "PSNP" });
                        break;
                    case 3:
                        return RedirectToAction("Index", "Home", new { Area = "Logistics" });
                        break;
                    case 4:
                        return RedirectToAction("Index", "Home", new { Area = "Procurement" });
                        break;
                    case 5:
                        return RedirectToAction("Index", "Home", new { Area = "Finance" });
                        break;

                }

            
            // If the user is not niether regional nor caseteam user return this default page
            var userDashboardPreferences = _userDashboardPreferenceService.Get(t => t.UserID == userID).OrderBy(m => m.OrderNo);
            var dashboardWidgets = userDashboardPreferences.Select(userDashboardPreference =>
                                    _dashboardWidgetService.FindById(userDashboardPreference.DashboardWidgetID)).ToList();
            return View(dashboardWidgets);

            //return Json(req, JsonRequestBehavior.AllowGet);
            //return Json(req, JsonRequestBehavior.AllowGet);
            //var widgets = new List<DashboardWidget>();
            //return View(widgets);
        }

        public ActionResult Preference()
        {
            if (TempData["PreferenceUpdateSuccessMsg"] != null)
                ModelState.AddModelError("Success", TempData["PreferenceUpdateSuccessMsg"].ToString());
            if (TempData["PreferenceUpdateErrorMsg"] != null)
                ModelState.AddModelError("Errors", TempData["PreferenceUpdateErrorMsg"].ToString());

            var userID = UserAccountHelper.GetUser(HttpContext.User.Identity.Name).UserProfileID;
            var userDashboardPreferences = _userDashboardPreferenceService.Get(t => t.UserID == userID).OrderBy(m => m.OrderNo);
            var selectedDashboardWidgets = userDashboardPreferences.Select(userDashboardPreference =>
                                    _dashboardWidgetService.FindById(userDashboardPreference.DashboardWidgetID)).ToList();
            ViewBag.SelectedDashboards = selectedDashboardWidgets;
            var allDashboardWidgets = _dashboardWidgetService.GetAllDashboardWidget();
            var unselectedDashbaords = allDashboardWidgets.Where(dashboardWidget => !selectedDashboardWidgets.Contains(dashboardWidget)).ToList();
            ViewBag.UnselectedDashbaords = unselectedDashbaords;

            var user = userService.GetUserDetail(HttpContext.User.Identity.Name);
            var userPreferenceViewModel = new UserPreferenceViewModel(user);
            ViewBag.Languages = new SelectList(userPreferenceViewModel.Languages, "StringID", "Name", userPreferenceViewModel.Language);
            ViewBag.DateFormatPreference = new SelectList(userPreferenceViewModel.DateFormatPreferences, "StringID", "Name", userPreferenceViewModel.DateFormatPreference);
            ViewBag.WeightPrefernce = new SelectList(userPreferenceViewModel.WeightPerferences, "StringID", "Name", userPreferenceViewModel.WeightPrefernce.Trim());
            ViewBag.KeyboardLanguage = new SelectList(userPreferenceViewModel.KeyboardLanguages, "StringID", "Name", userPreferenceViewModel.KeyboardLanguage);
            ViewBag.ThemePreference = new SelectList(userPreferenceViewModel.ThemePreferences, "StringID", "Name", userPreferenceViewModel.ThemePreference);

            return View(userPreferenceViewModel);
        }

        public JsonResult SavePreference([DataSourceRequest] DataSourceRequest request, List<int> selectedDashboardIDs)
        {
            var userID = UserAccountHelper.GetUser(HttpContext.User.Identity.Name).UserProfileID;
            var userDashboardPreferences = _userDashboardPreferenceService.Get(t => t.UserID == userID).OrderBy(m => m.OrderNo);
            var selectedDashboardWidgets = userDashboardPreferences.Select(userDashboardPreference =>
                                    _dashboardWidgetService.FindById(userDashboardPreference.DashboardWidgetID)).ToList();
            var selectedDashboardWidgetIDs = selectedDashboardWidgets.Select(selectedDashboardWidget => selectedDashboardWidget.DashboardWidgetID).ToList();

            //Create the newly selected dashboards in user preference
            var order = 1;
            foreach (var selectedDashboardID in selectedDashboardIDs)
            {
                if (!selectedDashboardWidgetIDs.Contains(selectedDashboardID))
                {
                    var userDashboardPreference = new UserDashboardPreference
                        {
                            DashboardWidgetID = selectedDashboardID,
                            UserID = userID,
                            OrderNo = order
                        };
                    _userDashboardPreferenceService.AddUserDashboardPreference(userDashboardPreference);
                }
                order++;
            }

            //Delete the removed dashboards from the user preference
            foreach (var userDashboardPreferenceObj in selectedDashboardWidgetIDs.Where(selectedDashboardWidgetID => !selectedDashboardIDs.Contains(selectedDashboardWidgetID)).Select(id => _userDashboardPreferenceService.Get(
                t => t.DashboardWidgetID == id && t.UserID == userID).FirstOrDefault()))
            {
                _userDashboardPreferenceService.DeleteUserDashboardPreference(userDashboardPreferenceObj);
            }

            //Edit the selected dashboards order
            var orderNo = 1;
            foreach (var userDashboardPreferenceObj in selectedDashboardIDs.Select(id => _userDashboardPreferenceService.Get(
                t => t.DashboardWidgetID == id && t.UserID == userID).FirstOrDefault()))
            {
                userDashboardPreferenceObj.OrderNo = orderNo;
                _userDashboardPreferenceService.EditUserDashboardPreference(userDashboardPreferenceObj);
                orderNo++;
            }

            ModelState.AddModelError("Success", @"Preference Saved.");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegionalMonthlyRequests()
        {
            var request = _unitOfWork.RegionalRequestRepository.FindById(2);
            return Json(request, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Redirect2Hub()
        {
            return Redirect("/hub");
        }

        private static string GetApplication(string user)
        {

            var currentUser = UserAccountHelper.GetUser(user);
            var userID = currentUser.UserProfileID;
            if (currentUser.IsAdmin)
            {
                return Cats.Models.Constant.Application.EARLY_WARNING;
            }
            if (currentUser.DefaultHub != null)
            {

                return Cats.Models.Constant.Application.HUB;

            }
            if (currentUser.RegionalUser)
            {
                return Cats.Models.Constant.Application.REGIONAL;
            }
            switch (currentUser.CaseTeam)
            {
                case 1://EarlyWarning
                    return Cats.Models.Constant.Application.EARLY_WARNING;
                    break;
                case 2://PSNP
                    return Cats.Models.Constant.Application.PSNP;
                    break;
                case 3://Logistics
                    return Cats.Models.Constant.Application.LOGISTICS;
                    break;
                case 4://Procurement
                    return Cats.Models.Constant.Application.PROCUREMENT;
                    break;
                case 5://Finance
                    return Cats.Models.Constant.Application.FINANCE;
                    break;
                default:
                    return "";

            }

        }
        public ActionResult GetUnreadNotification([DataSourceRequest] DataSourceRequest request)
        {

            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));

            List<Cats.Models.Notification> totallUnread = null;
            var currentUser = UserAccountHelper.GetUser(user);
            var app = GetApplication(user);

            if (app == Models.Constant.Application.HUB)
            {
                totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.DefaultHub && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
            }
            else if (app == Models.Constant.Application.REGIONAL)
            {
                totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.RegionID && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
            }
            else
            {
                totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
            }

            //var user = System.Web.HttpContext.Current.User.Identity.Name;
            //var roles = _userAccountService.GetUserPermissions(user).Select(a => a.Roles).ToList();
            //var allUserRollsInAllApplications = new List<string>();

            //foreach (var app in roles)
            //{
            //    allUserRollsInAllApplications.AddRange(app.Select(role => role.RoleName));
            //}

            //var totalUnread = _notificationService.GetAllNotification().Where(n => n.IsRead == false && allUserRollsInAllApplications.Contains(n.Application)).ToList();
            foreach (var nai in totallUnread)
            {
                nai.Url = GetRelativeURL(nai.Url);

            }
            var notificationViewModel = Cats.ViewModelBinder.NotificationViewModelBinder.ReturnNotificationViewModel(totallUnread.ToList());
            return Json(notificationViewModel.ToDataSourceResult(request));
        }

        public static IEnumerable<string> MatchingStrings(string haystack, IEnumerable<string> needles)
        {
            return needles.Where(haystack.Contains);
        } 
        public static string GetRelativeURL(string AbsURL)
        {

            // return empity string if the url fetched is empity 
            if (AbsURL.IsEmpty()) return "";

            // URL fetched from the database 
            // Example: 1. http://172.16.1.5//Logistics/DispatchAllocation/IndexFromNotification?paramRegionId=10&recordId=18402
            //          2. http://cats.dppc.gov.et//Logistics/DispatchAllocation/IndexFromNotification?paramRegionId=2&recordId=5421
            //          3. http://cats.dppc.gov.et//EarlyWarning/Request/IndexFromNotification?recordId=17078
            //          4. http://172.16.1.5/CATSV6/Hub/TransportOrder/NotificationIndex?recordId=10952
            // We have to search for the caseteams from the absolute URL, since that is the where the relative path begins
            // Get a caseteam that exists on the URL path
            var matchCaseTeams = MatchingStrings(AbsURL, new [] {"Logistics", "Hub", "EarlyWarning", "Procurement", "Finance", "PSNP", "Regional", "Home"}).ToList();
            // check of a case team exists on the absolute URL
            if (matchCaseTeams.Any())
            {
                // search for the matching caseteam on the absolute path and get the index
                var caseteamIndex = AbsURL.IndexOf(matchCaseTeams[0], 0, StringComparison.Ordinal);
                // get the string starting from the caseteam to the end
                var relativeUrl = AbsURL.Substring(caseteamIndex);
                // get the subdirectory and host address
                var originalUrl = System.Web.HttpContext.Current.Request.Url.OriginalString;
                var appPathUrl = originalUrl.Substring(0,
                    (originalUrl.IndexOf(System.Web.HttpContext.Current.Request.Url.LocalPath)+1));
                
                // return the relative url
                return appPathUrl + relativeUrl;
            }
            else
            {
                return "";
            }
        }
        public ActionResult GetUnreadNotificationDetail([DataSourceRequest] DataSourceRequest request)
        {
            return View();
        }

        public ActionResult ReportListing(string id="")
        {
            ViewBag.caseTeam = id;
            return View();
        }

        [HttpGet]
        public string GetAmharicReport(string caseTeam = "", string report = "")
        {
            try
            {
                var html = string.Empty;

                var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" 
                              + System.Web.HttpContext.Current.Request.Url.Authority 
                              + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                html += "" + baseUrl + "AmharicReportViewer.aspx?path=/" + caseTeam + "/AM" + "/" + report;

                return html;
            }
            catch (Exception)
            {
                return "<h3>Error in showing the report!</h3>";
            }
        }
        [HttpGet]
        public string  GetReport(string caseTeam="", string report="")
        {
            try
            {
                var html = string.Empty;

                var baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

                html += "" + baseUrl + "ReportViewer.aspx?path=/" + caseTeam + "/" + report;

                return html;
            }
            catch (Exception)
            {
                return "<h3>Error in showing the report!</h3>";
            }
        }

    }
}
