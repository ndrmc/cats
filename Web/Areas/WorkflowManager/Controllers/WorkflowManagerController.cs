using Cats.Areas.WorkflowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.Shared.DashBoardModels;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Workflows;
using Cats.Services.Workflows.Config;
using IUnitOfWork = Cats.Data.UnitWork.IUnitOfWork;
using UnitOfWork = Cats.Data.UnitWork.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowManagerController : Controller
    {
        public WorkflowManagerController()
        {
            if (DashboardMapping.PageNameToWorkflowMappingsList.Count == 0) { DashboardMapping.RegisterDashboardPage(); }
        }
        public dynamic GetAllListOfFilterObjects(string pageName)
        {
            //Random random = new Random(int.MinValue);

            ////string pageName = Constants.FinancePage;
            //string constantPageName = GetFormalPageName(pageName);
            //string[] workflows = GetAllWorkflows(constantPageName);
            //List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow, Id = random.Next() }).ToList();

            //random = new Random(int.MinValue);

            //string[] activities = GetAllStateTemplate(workflows);
            //List<DashboardFilterModel> dashboardFilterActivity = activities.Select(activity => new DashboardFilterModel { Name = activity, Id = random.Next() }).ToList();

            ////dynamic[] filterObjects = { users, workflows, activities };
            //dynamic[] filterObjects = { GetJsonResult(dashboardFilterUser), GetJsonResult(dashboardFilterWorkflow), GetJsonResult(dashboardFilterActivity) };


            //return filterObjects;
            ////return Json(filterObjects, JsonRequestBehavior.AllowGet);
            return null;
        }
        private static string GetFormalPageName(string pageName)
        {
            string constantPageName = string.Empty;

            if (pageName.Equals(Constants.FinancePage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.FinancePage;
            }
            else if (pageName.Equals(Constants.EarlywarningPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.EarlywarningPage;
            }
            else if (pageName.Equals(Constants.PsnpPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.PsnpPage;
            }
            else if (pageName.Equals(Constants.LogisticsPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.LogisticsPage;
            }
            else if (pageName.Equals(Constants.ProcurementPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.ProcurementPage;
            }
            else if (pageName.Equals(Constants.HubPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.HubPage;
            }
            else if (pageName.Equals(Constants.RegionalPage, StringComparison.InvariantCultureIgnoreCase))
            {
                constantPageName = Constants.RegionalPage;
            }
 
            return constantPageName;
        }
        private ContentResult GetJsonResult(object list)
        {
            JsonSerializerSettings camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ContentResult jsonResult = new ContentResult
            {
                Content = JsonConvert.SerializeObject(list, camelCaseFormatter),
                ContentType = "application/json"
            };

            return jsonResult;
        }
        public ContentResult GetAllTeamUsers(string pageName)
        {
            UserType.CASETEAM caseteam;
            Enum.TryParse(GetFormalPageName(pageName).ToUpper(), out caseteam);

            List<UserProfile> userProfiles = new List<UserProfile>();
            IUnitOfWork unitOfWork = new UnitOfWork();
            UserProfileService userProfileService = new UserProfileService(unitOfWork);

            switch (caseteam)
            {
                case UserType.CASETEAM.FINANCE: // 1
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.FINANCE);

                        break;
                    }
                case UserType.CASETEAM.EARLYWARNING: // 2
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.EARLYWARNING);

                        break;
                    }
                case UserType.CASETEAM.LOGISTICS:  // 3
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.LOGISTICS);

                        break;
                    }
                case UserType.CASETEAM.PROCUREMENT: // 4
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PROCUREMENT);

                        break;
                    }
                case UserType.CASETEAM.PSNP: // 5
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PSNP);

                        break;
                    }
                case UserType.CASETEAM.HUB: // 6
                    {
                        userProfiles = userProfileService.FindBy(c => c.DefaultHub != null);

                        break;
                    }
                case UserType.CASETEAM.REGIONAL: // 7
                    {
                        userProfiles = userProfileService.FindBy(c => c.RegionalUser);

                        break;
                    }
                case UserType.CASETEAM.ADMIN: // 8
                    {
                        userProfiles = userProfileService.FindBy(null);

                        break;
                    }
            }

            List<string> users = userProfiles.Select(u => u.UserName).ToList();
            Random random = new Random(int.MinValue);

            List<DashboardFilterModel> dashboardFilterUser =
                users. Select(user => new DashboardFilterModel
                {
                    Name = user,
                    Id = random.Next()
                }).OrderBy(o => o.Name).ToList();

            return GetJsonResult(dashboardFilterUser);
        }
        public ContentResult GetAllWorkflows(string pageName)
        {
            pageName = GetFormalPageName(pageName);
            Random random = new Random(int.MinValue);
            List<string> workflowDefinitions = new List<string>();

            workflowDefinitions.AddRange(DashboardMapping.PageNameToWorkflowMappingsList.Where(w => w.Item2 == pageName).Select(w => w.Item3));
            if (workflowDefinitions.Contains(Constants.GlobalWorkflow)) workflowDefinitions.Remove(Constants.GlobalWorkflow);

            List<DashboardFilterModel> dashboardFilterWorkflow =
                workflowDefinitions.Select(workflow => new DashboardFilterModel
                {
                    Name = workflow,
                    Id = random.Next()
                }).OrderBy(o => o.Name).ToList();

            return GetJsonResult(dashboardFilterWorkflow);
        }
        public ContentResult GetAllStateTemplate(string workflowName)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ApplicationSettingService applicationSettingService = new ApplicationSettingService(unitOfWork);
            StateTemplateService stateTemplateService = new StateTemplateService(unitOfWork);
            Random random = new Random(int.MinValue);

            var applicationSetting = applicationSettingService.GetAllApplicationSetting()
                .FirstOrDefault(w => w.SettingName == workflowName);
            var globalApplicationSetting = applicationSettingService.GetAllApplicationSetting()
                .FirstOrDefault(w => w.SettingName == Constants.GlobalWorkflow);

            if (applicationSetting != null || globalApplicationSetting != null)
            {
                string settingValue = applicationSetting.SettingValue;
                int processId;
                int.TryParse(settingValue, out processId);
                string globalSettingValue = globalApplicationSetting.SettingValue;
                int globalProcessId;
                int.TryParse(globalSettingValue, out globalProcessId);

                var d = stateTemplateService.GetAll().Where(p => p.ParentProcessTemplateID == processId ||
                                p.ParentProcessTemplateID == globalProcessId).Select(p => p.Name).ToArray();
                List<DashboardFilterModel> dashboardFilterActivity = d.Distinct().Select(activity => new DashboardFilterModel
                {
                    Name = activity,
                    Id = random.Next()
                }).OrderBy(o => o.Name).ToList();

                return GetJsonResult(dashboardFilterActivity);
            }

            return null;
        }
        public ContentResult GetAllStateTemplates(string[] workflows)
        {
            List<string> stateTemplates = new List<string>();
            Random random = new Random(int.MinValue);
            IUnitOfWork unitOfWork = new UnitOfWork();
            ApplicationSettingService applicationSettingService = new ApplicationSettingService(unitOfWork);
            StateTemplateService stateTemplateService = new StateTemplateService(unitOfWork);

            foreach (string workflow in workflows)
            {
                var applicationSetting = applicationSettingService.GetAllApplicationSetting().FirstOrDefault(w => w.SettingName == workflow);

                if (applicationSetting != null)
                {
                    string settingValue = applicationSetting.SettingValue;
                    int processId;
                    int.TryParse(settingValue, out processId);

                    stateTemplates.AddRange(stateTemplateService.GetAll().Where(p => p.ParentProcessTemplateID == processId).Select(p => p.Name));
                }
            }

            List<DashboardFilterModel> dashboardFilterActivity =
                stateTemplates.Distinct().Select(activity => new DashboardFilterModel
                {
                    Name = activity,
                    Id = random.Next()
                }).OrderBy(o => o.Name).ToList();

            return GetJsonResult(dashboardFilterActivity);
        }
        public ContentResult GetDataEntryStat(DateTime startDate, DateTime endDate, List<string> workflowDefs,
         List<string> wfusers, List<string> activities)
        {
            if (!workflowDefs.Contains(Constants.GlobalWorkflow)) workflowDefs.Add(Constants.GlobalWorkflow);

            Data.Shared.UnitWork.IUnitOfWork unitOfWork = new Data.Shared.UnitWork.UnitOfWork();
            WorkflowActivityService wfa = new WorkflowActivityService(unitOfWork);

            // Get aggregated data
            List<DashboardDataEntry> result = wfa.GetWorkflowActivityAgg(startDate, endDate, workflowDefs, wfusers,
                activities);

            List<DashboarDataEntryModel> dashboarDataEntryModels = (from user in wfusers
                                                                    let dashboardDataEntries = result.Where(u => u.PerformedBy == user).ToList()
                                                                    select new DashboarDataEntryModel
                                                                    {
                                                                        Name = user,
                                                                        DashboardDataEntries = dashboardDataEntries
                                                                    }).OrderBy(o => o.Name).ToList();

            return GetJsonResult(dashboarDataEntryModels);
        }

        public dynamic GetObjectList(DateTime startDate, DateTime endDate, List<string> workflows, string userName, List<string> activities)
        {
            return null;
        }

        #region Helper Methods

        IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
        {
            List<int> candidates = new List<int>();
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                candidates.Add(i);
            }
            Random rnd = new Random();

            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
            }
        }

        #endregion
    }
}