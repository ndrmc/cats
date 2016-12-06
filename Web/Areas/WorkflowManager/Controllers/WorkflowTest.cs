using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Areas.WorkflowManager.Models;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.EarlyWarning;
using Cats.Services.Workflows.Config;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowTestController : ApiController
    {
        private readonly IUserProfileService _userProfileService;
        //private readonly IWorkflowActivityService _workflowActivityService;
        private readonly Cats.Services.Workflows.Config.IApplicationSettingService _applicationSettingService;
        private readonly IStateTemplateService _stateTemplateService;
        public WorkflowTestController()
        { }

        public WorkflowTestController(IUserProfileService userProfileService,
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
            string[] users = GetAllTeamUsers(pageName);
            List<DashboardFilterModel> dashboardFilterUser = users.Select(user => new DashboardFilterModel { Name = user }).ToList();

            string[] workflows = GetAllWorkflows(pageName);
            List<DashboardFilterModel> dashboardFilterWorkflow = workflows.Select(workflow => new DashboardFilterModel { Name = workflow }).ToList();

            string[] activities = GetAllStateTemplate(workflows);
            List<DashboardFilterModel> dashboardFilterActivity = activities.Select(activity => new DashboardFilterModel { Name = activity }).ToList();

            dynamic[] filterObjects = { users, workflows, activities };

            return filterObjects;
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