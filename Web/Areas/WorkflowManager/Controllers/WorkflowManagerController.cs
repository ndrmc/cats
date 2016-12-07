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
        public dynamic GetAllListOfFilterObjects(string pageName)
        {
            //Random random = new Random(Int32.MinValue);

            ////string pageName = Constants.FinancePage;
            //string constantPageName = GetFormalPageName(pageName);
            //string[] workflows = GetAllWorkflows(constantPageName);
            //List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow, Id = random.Next() }).ToList();

            //random = new Random(Int32.MinValue);

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

            string[] users = GetAllTeamUsers(pageName);
            List<DashboardFilterModel> dashboardFilterUser = users.Select(user => new DashboardFilterModel { Name = user, Id = random.Next() }).ToList();

            random = new Random(Int32.MinValue);

            string[] workflows = GetAllWorkflows(constantPageName);
            List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow, Id = random.Next() }).ToList();

            random = new Random(Int32.MinValue);

            string[] activities = GetAllStateTemplate(workflows);
            List<DashboardFilterModel> dashboardFilterActivity = activities.Select(activity => new DashboardFilterModel { Name = activity, Id = random.Next() }).ToList();

            //dynamic[] filterObjects = { users, workflows, activities };
            dynamic[] filterObjects = { dashboardFilterUser, dashboardFilterWorkflow, dashboardFilterActivity };

        }
        private string[] GetAllTeamUsers(string pageName)
        {
            UserType.CASETEAM caseteam;

            List<UserProfile> userProfiles = new List<UserProfile>();
            Enum.TryParse(pageName.ToUpper(), out caseteam);

            IUnitOfWork unitOfWork = new UnitOfWork();
            UserProfileService userProfileService = new UserProfileService(unitOfWork);

            switch (caseteam)
            {
                case UserType.CASETEAM.FINANCE:
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.FINANCE);

                        break;
                    }
                case UserType.CASETEAM.EARLYWARNING:
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.EARLYWARNING);

                        break;
                    }
                case UserType.CASETEAM.LOGISTICS:
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.LOGISTICS);

                        break;
                    }
                case UserType.CASETEAM.PROCUREMENT:
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PROCUREMENT);

                        break;
                    }
                case UserType.CASETEAM.PSNP:
                    {
                        userProfiles = userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.PSNP);

                        break;
                    }
            }

            List<string> users = userProfiles.Select(u => u.UserName).ToList();

            return GetJsonResult(dashboardFilterUser);
        }
        private string[] GetAllWorkflows(string pageName)
        {
            List<string> workflowDefinitions = new List<string>();

            if (DashboardMapping.PageNameToWorkflowMappingsList.Count == 0) { DashboardMapping.RegisterDashboardPage(); }

            workflowDefinitions.AddRange(DashboardMapping.PageNameToWorkflowMappingsList.Where(w => w.Item2 == pageName).Select(w => w.Item3));

            List<DashboardFilterModel> dashboardFilterWorkflow = workflowDefinitions.Select(workflow => new DashboardFilterModel { Name = workflow, Id = random.Next() }).ToList();

            return GetJsonResult(dashboardFilterWorkflow);
        }
        public ContentResult GetAllStateTemplate(string workflowName)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ApplicationSettingService applicationSettingService = new ApplicationSettingService(unitOfWork);
            StateTemplateService stateTemplateService = new StateTemplateService(unitOfWork);
            var random = new Random(Int32.MinValue);

            var applicationSetting = applicationSettingService.GetAllApplicationSetting()
                .FirstOrDefault(w => w.SettingName == workflowName);

            if (applicationSetting != null)
            {
                string settingValue = applicationSetting.SettingValue;
                int processId;
                int.TryParse(settingValue, out processId);

                return stateTemplateService.GetAll().Where(p => p.ParentProcessTemplateID == processId).Select(p => p.Name).ToArray();
            }

            return null;
        }
        public ContentResult GetAllStateTemplates(string[] workflows)
        {
            List<string> stateTemplates = new List<string>();

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
            List<DashboardFilterModel> dashboardFilterActivity = stateTemplates.Distinct().Select(activity => new DashboardFilterModel { Name = activity, Id = random.Next() }).ToList();

            return GetJsonResult(dashboardFilterActivity);
        }
        public ContentResult GetDataEntryStat(DateTime startDate, DateTime endDate, List<string> workflowDefs,
         List<string> wfusers, List<string> activities)
        {
            Data.Shared.UnitWork.IUnitOfWork unitOfWork = new Data.Shared.UnitWork.UnitOfWork();
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

            return GetJsonResult(dashboarDataEntryModels);

        }
    }
}
