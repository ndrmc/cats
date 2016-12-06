using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cats.Areas.WorkflowManager.Models;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Workflows;
using Cats.Services.Workflows.Config;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowTestController : ApiController
    {
        //private readonly IUserProfileService _userProfileService;
        ////private readonly IWorkflowActivityService _workflowActivityService;
        //private readonly IApplicationSettingService _applicationSettingService;
        //private readonly IStateTemplateService _stateTemplateService;
        public WorkflowTestController()
        { }
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public dynamic GetAllListOfFilterObjects(string pageName)
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
            //List<DashboardFilterModel> dashboardFilterUser = users.Select(user => new DashboardFilterModel { Name = user }).ToList();

            string[] workflows = GetAllWorkflows(constantPageName);
            //List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow }).ToList();

            string[] activities = GetAllStateTemplate(workflows);
            //List<DashboardFilterModel> dashboardFilterActivity = activities.Select(activity => new DashboardFilterModel { Name = activity }).ToList();

            dynamic[] filterObjects = { users, workflows, activities };

            return filterObjects;
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
    }
}