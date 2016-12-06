using Cats.Areas.WorkflowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Data.Shared.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.Shared.DashBoardModels;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Workflows;
using Cats.Services.Workflows.Config;
using IUnitOfWork = Cats.Data.UnitWork.IUnitOfWork;
using UnitOfWork = Cats.Data.UnitWork.UnitOfWork;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowManagerController : Controller
    {
        public JsonResult GetAllListOfFilterObjects(string pageName)
        {
            Random random = new Random(Int32.MinValue);
            
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

            return Json(filterObjects, JsonRequestBehavior.AllowGet);
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

            return users.ToArray();
        }
        private string[] GetAllWorkflows(string pageName)
        {
            List<string> workflowDefinitions = new List<string>();

            if (DashboardMapping.PageNameToWorkflowMappingsList.Count == 0) { DashboardMapping.RunConfig(); }

            workflowDefinitions.AddRange(DashboardMapping.PageNameToWorkflowMappingsList.Where(w => w.Item2 == pageName).Select(w => w.Item3));

            return workflowDefinitions.ToArray();
        }
        private string[] GetAllStateTemplates(string workflowName)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ApplicationSettingService applicationSettingService = new ApplicationSettingService(unitOfWork);
            StateTemplateService stateTemplateService = new StateTemplateService(unitOfWork);

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
        private string[] GetAllStateTemplate(string[] workflows)
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

            return stateTemplates.Distinct().ToArray();
        }
        public dynamic GetDataEntryStat(DateTime startDate, DateTime endDate, List<string> workflowDefs,
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

            return dashboarDataEntryModels;
        }
    }
}