using Cats.Areas.WorkflowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Data.Shared.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.Shared.DashBoardModels;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Workflows;
using Cats.Services.Workflows.Config;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowManagerController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        //private readonly IWorkflowActivityService _workflowActivityService;
        private readonly Cats.Services.Workflows.Config.IApplicationSettingService _applicationSettingService;
        private readonly IStateTemplateService _stateTemplateService;

        public WorkflowManagerController(IUserProfileService userProfileService,
            //IWorkflowActivityService workflowActivityService,
            Cats.Services.Workflows.Config.IApplicationSettingService applicationSettingService,
            IStateTemplateService stateTemplateService)
        {
            _userProfileService = userProfileService;
            //_workflowActivityService = workflowActivityService;
            _applicationSettingService = applicationSettingService;
            _stateTemplateService = stateTemplateService;

            DashboardMapping.RunConfig();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetWorkflowActivity(string pageName, string filter = null)
        {
            string[] users = GetAllTeamUsers(pageName);
            List<DashboardFilterModel> dashboardFilterUser = users.Select(user => new DashboardFilterModel { Name = user }).ToList();

            string[] workflows = GetAllWorkflows(pageName);
            List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow }).ToList();

            string[] activities = GetAllStateTemplate(workflows);
            List<DashboardFilterModel> dashboardFilterActivity = activities.Select(activity => new DashboardFilterModel { Name = activity }).ToList();


            return null;
        }

        public JsonResult GetAllListOfFilterObjects()
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCaseTeamUsers(string pageName)
        {
            return Json(GetAllTeamUsers(pageName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllWorkflowActivities(string pageName)
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataEntryStat(DateTime startDate, DateTime endDate, List<string> workflowDefs,
            List<string> wfusers, List<string> activities)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            WorkflowActivityService wfa = new WorkflowActivityService(unitOfWork);
            // Sample data seed
            //List<string> workflowDefs = new List<string> { "PaymentRequestWorkflow", "TransporterChequeWorkflow" };
            //List<string> wfusers = new List<string> { "AbebaB", "admin" };
            //List<string> activities = new List<string> { "Cheque Collected", "Cheque Issued", "Approved by finance", "Closed", "Request Verified" };

            List<DashboardDataEntry> result = wfa.GetWorkflowActivityAgg(startDate, endDate, workflowDefs, wfusers,
                activities);

            List<DashboarDataEntryModel> dashboarDataEntryModels = (from user in wfusers
                                                                    let dashboardDataEntries = result.Where(u => u.PerformedBy == user).ToList()
                                                                    select new DashboarDataEntryModel
                                                                    {
                                                                        Name = user,
                                                                        DashboardDataEntries = dashboardDataEntries
                                                                    }).ToList();

            return Json(dashboarDataEntryModels, JsonRequestBehavior.AllowGet);
        }

        private string[] GetAllTeamUsers(string pageName)
        {
            UserType.CASETEAM caseteam;

            List<UserProfile> userProfiles = new List<UserProfile>();
            Enum.TryParse(pageName, out caseteam);

            switch (caseteam)
            {
                case UserType.CASETEAM.FINANCE:
                    {
                        userProfiles = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.FINANCE);

                        break;
                    }
                case UserType.CASETEAM.EARLYWARNING:
                    {
                        userProfiles = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.EARLYWARNING);

                        break;
                    }
                case UserType.CASETEAM.LOGISTICS:
                    {
                        userProfiles = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.LOGISTICS);

                        break;
                    }
                case UserType.CASETEAM.PROCUREMENT:
                    {
                        userProfiles = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PROCUREMENT);

                        break;
                    }
                case UserType.CASETEAM.PSNP:
                    {
                        userProfiles = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PSNP);

                        break;
                    }
            }

            List<string> users = userProfiles.Select(u => u.UserName).ToList();

            return users.ToArray();
        }
        private string[] GetAllWorkflows(string pageName)
        {
            List<string> workflowDefinitions = new List<string>();

            workflowDefinitions.AddRange(DashboardMapping.PageNameToWorkflowMappingsList.Where(w => w.Item2 == pageName).Select(w => w.Item3));

            return workflowDefinitions.ToArray();
        }
        private string[] GetAllStateTemplates(string workflowName)
        {
            var applicationSetting = _applicationSettingService.GetAllApplicationSetting()
                .FirstOrDefault(w => w.SettingName == workflowName);

            if (applicationSetting != null)
            {
                string settingValue = applicationSetting.SettingValue;
                int processId;
                int.TryParse(settingValue, out processId);

                return _stateTemplateService.GetAll().Where(p => p.ParentProcessTemplateID == processId).Select(p => p.Name).ToArray();
            }

            return null;
        }

        private string[] GetAllStateTemplate(string[] workflows)
        {
            List<string> stateTemplates = new List<string>();

            foreach (string workflow in workflows)
            {
                var applicationSetting = _applicationSettingService.GetAllApplicationSetting().FirstOrDefault(w => w.SettingName == workflow);

                if (applicationSetting != null)
                {
                    string settingValue = applicationSetting.SettingValue;
                    int processId;

                    int.TryParse(settingValue, out processId);

                    stateTemplates.AddRange(_stateTemplateService.GetAll().Where(p => p.ParentProcessTemplateID == processId).Select(p => p.Name));
                }
            }

            return stateTemplates.Distinct().ToArray();
        }
    }
}