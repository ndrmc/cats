using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.EarlyWarning.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Dashboard;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Cats.ViewModelBinder;

namespace Cats.Areas.EarlyWarning.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /EarlyWarning/
        private readonly IHRDService _hrdService;
        private readonly IHRDDetailService _hrdDetailService;
        private readonly IRationDetailService _rationDetailService;
        private readonly IRegionalRequestService _regionalRequestService;
        private IUserAccountService _userAccountService;
        private readonly IEWDashboardService _eWDashboardService;

        public HomeController(IHRDService hrdService, IHRDDetailService hrdDetailService,
            IRationDetailService rationDetailService, IRegionalRequestService regionalRequestService,
            IUserAccountService userAccountService, IEWDashboardService ewDashboardService)
        {
            _hrdService = hrdService;
            _hrdDetailService = hrdDetailService;
            _rationDetailService = rationDetailService;
            _regionalRequestService = regionalRequestService;
            _userAccountService = userAccountService;
            _eWDashboardService = ewDashboardService;
        }

        public ActionResult Index()
        {
            ViewBag.HRDList = new SelectList(_hrdService.GetHrds(), "HRDID", "HRDName");
            //ModelState.AddModelError("Success", "Sample Error Message. Use in Your Controller: ModelState.AddModelError('Errors', 'Your Error Message.')");
            var hrd = _hrdService.FindBy(m => m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published").FirstOrDefault();
            if (hrd == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlanID = hrd.PlanID;
            ViewBag.HRDName = hrd.Plan.PlanName;
            var summary = GetHRDSummary(hrd.HRDID);
            var rounds = GetRounds();
            if (rounds != null) ViewBag.Rounds = rounds;
            else ViewBag.Rounds = new List<int>();
            ViewBag.RecentRequestRounds = GetRecentRequestRounds();
            return View(summary);
        }

        public ActionResult EWMaps()
        {
            return View();
        }

        private DataTable GetHRDSummary(int id)
        {
            var weightPref = _userAccountService.GetUserInfo(HttpContext.User.Identity.Name).PreferedWeightMeasurment;
            var hrd = _hrdService.FindById(id);
            var hrdDetails =
                _hrdDetailService.Get(t => t.HRDID == id, null,
                    "AdminUnit,AdminUnit.AdminUnit2,AdminUnit.AdminUnit2.AdminUnit2").ToList();
            var rationDetails = _rationDetailService.Get(t => t.RationID == hrd.RationID, null, "Commodity");
            var dt = HRDViewModelBinder.TransposeDataSummary(hrdDetails, rationDetails, weightPref);
            return dt;
        }

        public ActionResult HRDSummaryJson()
        {
            //ModelState.AddModelError("Success", "Sample Error Message. Use in Your Controller: ModelState.AddModelError('Errors', 'Your Error Message.')");
            var hrd = _hrdService.FindBy(m => m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published").FirstOrDefault();
            if (hrd == null)
            {
                return HttpNotFound();
            }
            // ViewBag.PlanID = hrd.PlanID;
            // ViewBag.HRDName = hrd.Plan.PlanName;
            var summary = GetHRDSummary(hrd.HRDID);
            return Json(summary, JsonRequestBehavior.AllowGet);
        }

        public List<int?> GetRounds()
        {
            var currentHrd = GetCurrentHrd();
            var requests =
                _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID)
                    .OrderByDescending(m => m.RegionalRequestID);
            var reliefRequisitions = _eWDashboardService.GetAllReliefRequisition();
            return (from reliefRequisition in reliefRequisitions
                from request in requests
                where reliefRequisition.RegionalRequestID == request.RegionalRequestID
                      && reliefRequisition.Round.HasValue
                select reliefRequisition.Round).Distinct().ToList();
        }

        public List<int?> GetRecentRequestRounds()
        {
            var currentHrd = GetCurrentHrd();
            var requests =
                _eWDashboardService.FindByRequest(m => m.PlanID == currentHrd.PlanID && m.ProgramId == 1)
                    .OrderByDescending(m => m.RegionalRequestID);
            var rounds = (from regionalRequest in requests
                where regionalRequest.ProgramId == 1
                select regionalRequest.Round).Distinct().ToList();
            return rounds;
        }

        public HRD GetCurrentHrd()
        {
            var currentHrd =
                _eWDashboardService.FindByHrd(
                    m => m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published")
                    .FirstOrDefault();
            return currentHrd;
        }
    }
}

