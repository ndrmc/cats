using Cats.Areas.WorkflowManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Data.Shared.UnitWork;
using Cats.Models.Constant;
using Cats.Services.Administration;
using Cats.Services.Workflows;

namespace Cats.Areas.WorkflowManager.Controllers
{
    public class WorkflowManager : Controller
    {
        private readonly IUserProfileService _userProfileService;
        public WorkflowManager(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        public List<WorkflowActivityViewModel> GetWorkflowActivity(string pageName, string filter = null)
        {

            return null;
        }

        public JsonResult GetAllListOfFilterObjects()
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCaseTeamUsers(string pageName)
        {
            var usersPerCaseTeam = _userProfileService.FindBy(c => c.CaseTeam == (int)UserType.CASETEAM.FINANCE);
            var users = usersPerCaseTeam.Select(u => u.UserName);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllWorkflowActivities()
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataEntryStat(DateTime startdDate, DateTime enDateTime, List<string> workflowDefs, List<string> wfusers, List<string> activities)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            WorkflowActivityService wfa = new WorkflowActivityService(unitOfWork);
            // Sample data seed
            //List<string> workflowDefs = new List<string> { "PaymentRequestWorkflow", "TransporterChequeWorkflow" };
            //List<string> wfusers = new List<string> { "AbebaB", "admin" };
            //List<string> activities = new List<string> { "Cheque Collected", "Cheque Issued", "Approved by finance", "Closed", "Request Verified" };

            var result = wfa.GetWorkflowActivityAgg(startdDate, enDateTime, workflowDefs, wfusers, activities);

            //
            // TODO: needs (result) to be structured in 
            //

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}