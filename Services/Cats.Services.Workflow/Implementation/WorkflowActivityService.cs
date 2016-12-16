using Cats.Alert;
using Cats.Models;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Services.Common;
using Cats.Models.Hubs;
using Cats.Services;
using System.Diagnostics;
using Cats.Models.Security;
using Cats.Services.Workflows.Alert;
using Cats.Services.Hubs;
using System.Data;
using Cats.Models.Shared.DashBoardModels;
using Cats.Data.Shared;
using Cats.Data.Shared.UnitWork;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using IApplicationSettingService = Cats.Services.Workflows.Config.IApplicationSettingService;

namespace Cats.Services.Workflows
{
    public class WorkflowActivityService : IWorkflowActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        #region Constructor
        public WorkflowActivityService(IBusinessProcessService businessProcessService, IApplicationSettingService applicationSettingService, IHubBusinessProcessService hubBusinessProcessService)
        {
            _businessProcessService = businessProcessService;
            _applicationSettingService = applicationSettingService;
            _hubBusinessProcessService = hubBusinessProcessService;
        }
        public WorkflowActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region  Privte Declarations


        private IBusinessProcessService _businessProcessService;

        private IHubBusinessProcessService _hubBusinessProcessService;
        private IBusinessProcessService BusinessProcessService

        {
            get
            {

                return _businessProcessService;

            }

            set { _businessProcessService = value; }
        }
        private IHubBusinessProcessService HubBusinessProcessService

        {
            get
            {

                return _hubBusinessProcessService;

            }

            set { _hubBusinessProcessService = value; }
        }
        private IApplicationSettingService ApplicationSettingService

        {
            get
            {

                return _applicationSettingService;

            }

            set { _applicationSettingService = value; }
        }

        private IApplicationSettingService _applicationSettingService;

        private String userName_;
        public string UserName
        {
            get
            {
                if (String.IsNullOrEmpty(userName_))
                    userName_ = new WorkflowActivityUtil().GetUserName();

                return userName_;
            }

            set
            {
                userName_ = value;
            }
        }
        public String GetUserName()
        {

            UserInfo info = (UserInfo)System.Web.HttpContext.Current.Session["USER_INFO"];

            if (info != null)
                userName_ = info.UserName;

            return userName_;


        }

        #endregion

        #region Business Process Initializers

        public Models.BusinessProcess GetNewInstance(string description)
        {
            Models.BusinessProcess bp = new Models.BusinessProcess();
            int BP_PR = ApplicationSettingService.getGlobalWorkflow();

            if (BP_PR != 0)
            {
                var createdstate = new Models.BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = UserName,
                    Comment = description
                };

                bp = BusinessProcessService.CreateBusinessProcessWithOutStateEntry(BP_PR, 0, "Created");

            }
            return bp;
        }
        public Models.Hubs.BusinessProcess GetNewInstanceHub(string description)
        {
            Models.Hubs.BusinessProcess bp = new Models.Hubs.BusinessProcess();
            int BP_PR = ApplicationSettingService.getGlobalWorkflow();

            if (BP_PR != 0)
            {
                var createdstate = new Models.Hubs.BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = UserName,
                    Comment = description
                };

                bp = HubBusinessProcessService.CreateBusinessProcessWithOutStateEntry(BP_PR, 0, "Created");

            }
            return bp;
        }
        public void InitializeWorkflow(IWorkflow workflowImplementer, String instanceDescription = null)
        {
            if (workflowImplementer.BusinessProcessId == 0)
            {
                //workflowImplementer.BusinessProcess = GetNewInstance(instanceDescription);
                workflowImplementer.BusinessProcessId = GetNewInstance(instanceDescription).BusinessProcessID;
            }
        }
        public void InitializeWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String instanceDescription = null)
        {
            if (workflowImplementer.BusinessProcessId == 0)
            {
                //workflowImplementer.BusinessProcess = GetNewInstanceHub(instanceDescription);
                workflowImplementer.BusinessProcessId = GetNewInstanceHub(instanceDescription).BusinessProcessID;
            }
        }

        #endregion

        #region  public  Methods

        public Boolean EnterCreateWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);


            int editId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
        public Boolean EnterCreateWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);


            int editId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName, true);
        }
        public Boolean EnterEditWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
        public Boolean EnterPrintWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);


            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName);
        }
        public Boolean EnterPrintWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName, true);

        }
        public Boolean EnterDeleteWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName);


        }
        public Boolean EnterDeleteWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName, true);



        }
        public Boolean EnterExportWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName, true);

        }
        public Boolean EnterExportWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);


            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName);
        }
        public Boolean EnterEditWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);


            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName, true);

        }
        public Boolean EnterCreateWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        public Boolean EnterCreateWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        public Boolean EnterCreateWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultCreate", String fileName = "", bool isHub = false)
        {
            if (businessProcessID <= 0) return false;//Throw invalid param exception
            if (finalStateID < 0) return false;//Throw invalid param exception

            String msg = String.Empty;

            if (String.IsNullOrEmpty(description))
                msg = String.Empty;
            else if (description == "Workflow_DefaultCreate")
                msg = AlertMessage.Workflow_DefaultCreate;
            else
                msg = description;

            EnterWorkflow(businessProcessID, finalStateID, fileName, isHub, msg);



            return true;
        }
        public Boolean EnterEditWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        public Boolean EnterEditWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        public Boolean EnterEditWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultEdit", String fileName = "", bool isHub = false)
        {
            if (businessProcessID <= 0) return false;//Throw invalid param exception
            if (finalStateID < 0) return false;//Throw invalid param exception

            String msg = String.Empty;

            if (String.IsNullOrEmpty(description))
                msg = String.Empty;
            else if (description == "Workflow_DefaultEdit")
                msg = AlertMessage.Workflow_DefaultEdit;
            else
                msg = description;

            EnterWorkflow(businessProcessID, finalStateID, fileName, isHub, msg);



            return true;
        }
        public Boolean EnterPrintWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, PrintId, description, fileName);
        }
        public Boolean EnterPrintWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, PrintId, description, fileName);
        }
        public Boolean EnterPrintWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultPrint", String fileName = "", bool isHub = false)
        {
            if (businessProcessID <= 0) return false;//Throw invalid param exception
            if (finalStateID < 0) return false;//Throw invalid param exception

            String msg = String.Empty;

            if (String.IsNullOrEmpty(description))
                msg = String.Empty;
            else if (description == "Workflow_DefaultPrint")
                msg = AlertMessage.Workflow_DefaultPrint;
            else
                msg = description;

            EnterWorkflow(businessProcessID, finalStateID, fileName, isHub, msg);



            return true;
        }
        public Boolean EnterDeleteWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(documentBusinessProcess.BusinessProcessID, deleteId, description, fileName);


        }
        public Boolean EnterDeleteWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(documentBusinessProcess.BusinessProcessID, deleteId, description, fileName);


        }
        public Boolean EnterDeleteWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultDelete", String fileName = "", bool isHub = false)
        {
            if (businessProcessID <= 0) return false;//Throw invalid param exception
            if (finalStateID < 0) return false;//Throw invalid param exception

            String msg = String.Empty;

            if (String.IsNullOrEmpty(description))
                msg = String.Empty;
            else if (description == "Workflow_DefaultDelete")
                msg = AlertMessage.Workflow_DefaultDelete;
            else
                msg = description;

            EnterWorkflowDelete(businessProcessID, finalStateID, fileName, isHub, msg);


            return true;
        }
        public Boolean EnterExportWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        public Boolean EnterExportWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        public Boolean EnterExportWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultExport", String fileName = "", bool isHub = false)
        {
            if (businessProcessID <= 0) return false;//Throw invalid param exception
            if (finalStateID < 0) return false;//Throw invalid param exception

            String msg = String.Empty;

            if (String.IsNullOrEmpty(description))
                msg = String.Empty;
            else if (description == "Workflow_DefaultExport")
                msg = AlertMessage.Workflow_DefaultExport;
            else
                msg = description;

            EnterWorkflow(businessProcessID, finalStateID, fileName, isHub, msg);

            return true;
        }
        public void EnterWorkflow(int? businessProcessID, int finalStateID, string fileName, bool isHub, string msg)
        {
            if (!isHub)
                EnterWorkflow(businessProcessID, finalStateID, fileName, msg);
            else
                EnterWorkflowHub(businessProcessID, finalStateID, fileName, msg);
        }

        public void EnterWorkflowDelete(int? businessProcessID, int finalStateID, string fileName, bool isHub, string msg)
        {
            if (!isHub)
                EnterWorkflowDelete(businessProcessID, finalStateID, fileName, msg);
            else
                EnterWorkflowHubDelete(businessProcessID, finalStateID, fileName, msg);
        }
        public void EnterWorkflow(int? businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID ?? 0
            };


            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            BusinessProcessService.PromotWorkflow_WoutUpdatingCurrentStatus(businessProcessState);


            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

        }

        public void EnterWorkflowDelete(int? businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID ?? 0
            };


            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            BusinessProcessService.PromotWorkflow(businessProcessState);


            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

        }
        public void EnterWorkflowHub(int? businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.Hubs.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID ?? 0
            };

            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            HubBusinessProcessService.PromotWorkflow_WoutUpdatingCurrentStatus(businessProcessState);

            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);


        }

        public void EnterWorkflowHubDelete(int? businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.Hubs.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID ?? 0
            };

            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            HubBusinessProcessService.PromotWorkflow(businessProcessState);

            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);


        }
        Models.Hubs.BusinessProcess IWorkflowActivityService.GetBusinessProcessHub(int businessProcessId)
        {
            if (businessProcessId <= 0) return null;

            return HubBusinessProcessService.FindById(businessProcessId);
        }

        Models.BusinessProcess IWorkflowActivityService.GetBusinessProcess(int businessProcessId)
        {

            if (businessProcessId <= 0) return null;

            return BusinessProcessService.FindById(businessProcessId);
        }

        public List<IWorkflow> ExcludeDeletedRecords(List<IWorkflow> records)
        {
            if (records == null || !records.Any()) return new List<IWorkflow>();

            int deletedId = _businessProcessService.GetGlobalDeleteStateTempId();

            List<IWorkflow> result = new List<IWorkflow>();

            foreach (IWorkflow workflow in records)
            {
                if (workflow == null) continue;
                if (workflow.BusinessProcess == null && workflow.BusinessProcessId != 0)
                    workflow.BusinessProcess = _businessProcessService.FindById(workflow.BusinessProcessId);
                else
                {

                    result.Add(workflow);

                    continue;
                }
                if (workflow.BusinessProcess.CurrentState != null)
                {
                    if (workflow.BusinessProcess.CurrentState.StateID != deletedId)
                        result.Add(workflow);
                }
                else
                    result.Add(workflow);

            }

            return result.ToList();


        }

        public List<IWorkflowHub> ExcludeDeletedRecordsHub(List<IWorkflowHub> records)
        {
            if (records == null || !records.Any()) return new List<IWorkflowHub>();

            int deletedId = _businessProcessService.GetGlobalDeleteStateTempId();

            List<IWorkflowHub> result = new List<IWorkflowHub>();

            foreach (IWorkflowHub workflow in records)
            {
                if (workflow == null) continue;
                if (workflow.BusinessProcess == null && workflow.BusinessProcessId != 0)
                    workflow.BusinessProcess = _hubBusinessProcessService.FindById(workflow.BusinessProcessId);
                else
                {

                    result.Add(workflow);

                    continue;
                }
                if (workflow.BusinessProcess.CurrentState != null)
                {
                    if (workflow.BusinessProcess.CurrentState.StateID != deletedId)
                        result.Add(workflow);
                }
                else
                    result.Add(workflow);

            }

            return result.ToList();


        }


        public List<WorkflowActivity> GetWorkflowActivity(string pageName, string filter = null)
        {


            List<WorkflowActivity> mocks = new List<WorkflowActivity>();

            WorkflowActivity mock1 = new WorkflowActivity()
            {
                BusinessProcessStateID = 1,
                PerformedBy = "Ayele",
                Comment = Alert.AlertMessage.Workflow_DefaultCreate
                //TargetObject = get gift certificate from unit of work

            };

            WorkflowActivity mock2 = new WorkflowActivity()
            {
                BusinessProcessStateID = 1,
                PerformedBy = "Kebede",
                Comment = Alert.AlertMessage.Workflow_DefaultCreate
                //TargetObject = get gift certificate from unit of work

            };

            mocks.Add(mock1);
            mocks.Add(mock2);

            return mocks;


        }


        //public List<WorkflowActivityAgg> GetWorkflowActivityAgg(string pageName, string filter = null)

        //{

        //    return null;
        //}

        public List<DashboardDataEntry> GetWorkflowActivityAgg(DateTime startDate, DateTime endDate, List<string> workflowDefinitions, List<string> users, List<string> activities)
        {
            try
            {
                // Workflow filter collection
                FilterCollection filterWorkflowDefinitions = new FilterCollection();
                filterWorkflowDefinitions.AddRange(workflowDefinitions.Select(filterName => new Filter { FilterName = filterName }));

                // User filter collection
                FilterCollection filterUsers = new FilterCollection();
                filterUsers.AddRange(users.Select(filterName => new Filter { FilterName = filterName }));

                // Activity filter collection
                FilterCollection filterActivities = new FilterCollection();
                filterActivities.AddRange(activities.Select(filterName => new Filter { FilterName = filterName }));

                // Into Param object
                SqlParameter filterStartDate = new SqlParameter("StartDate", SqlDbType.Date) { Value = startDate.Date.ToString("d") };
                SqlParameter filterEndDate = new SqlParameter("EndDate", SqlDbType.Date) { Value = endDate.Date.ToString("d") };
                SqlParameter paramWorkflow = new SqlParameter
                {
                    ParameterName = "@WorkflowName_Array",  // proc def
                    SqlDbType = SqlDbType.Structured,
                    Value = filterWorkflowDefinitions,
                    Direction = ParameterDirection.Input,
                    TypeName = "dbo.FilterArray"
                };
                SqlParameter paramUser = new SqlParameter
                {
                    ParameterName = "@User_Array", // proc def
                    SqlDbType = SqlDbType.Structured,
                    Value = filterUsers,
                    Direction = ParameterDirection.Input,
                    TypeName = "dbo.FilterArray"
                };
                SqlParameter paramActivity = new SqlParameter
                {
                    ParameterName = "@Activity_Array", // proc def
                    SqlDbType = SqlDbType.Structured,
                    Value = filterActivities,
                    Direction = ParameterDirection.Input,
                    TypeName = "dbo.FilterArray"
                };

                var result = ExecWithStoreProcedure("EXEC [dbo].[GenericDashboardDataProvider] " +
                                                    "@StartDate, @EndDate, @WorkflowName_Array, @User_Array, @Activity_Array",
                    filterStartDate, filterEndDate, paramWorkflow, paramUser, paramActivity);

                //Debug.Assert(startDate != null, "_sc.From_Date != null");
                //string shortFromDate = startDate.ToString("yyyy-MM-dd");
                ////Debug.Assert(_sc.To_Date != null, "_sc.To_Date != null");
                //string shortToDate = endDate.ToString("yyyy-MM-dd");

                //DateTime fromdDate = Convert.ToDateTime(shortFromDate);
                //DateTime toDate = Convert.ToDateTime(shortToDate);              

                var dashboardDataEntries = (from dashEntries in result
                                            where dashEntries.DatePerformed >= startDate && dashEntries.DatePerformed <= endDate
                                            group dashEntries by new
                                            {
                                                dashEntries.ProcessTemplateID,
                                                dashEntries.StateTemplateID,
                                                dashEntries.PerformedBy,
                                                dashEntries.ActivityName,
                                                dashEntries.SettingName,
                                                //dashEntries.BusinessProcessID
                                                //dashEntries.DatePerformed
                                            }
                                            into gTrnsRqst
                                            select new
                                            {
                                                gTrnsRqst.Key.PerformedBy,
                                                gTrnsRqst.Key.SettingName,
                                                gTrnsRqst.Key.ActivityName,
                                                ActivityCount = gTrnsRqst.Count(),
                                                //gTrnsRqst.Key.StateTemplateID,
                                                //gTrnsRqst.Key.ProcessTemplateID,
                                                //gTrnsRqst.Key.BusinessProcessID
                                                //gTrnsRqst.Key.DatePerformed
                                            }).ToList();

                return dashboardDataEntries.Select(dataEntry => new DashboardDataEntry
                {
                    PerformedBy = dataEntry.PerformedBy,
                    ActivityCount = dataEntry.ActivityCount,
                    SettingName = dataEntry.SettingName,
                    ActivityName = dataEntry.ActivityName
                }).ToList();
            }
            catch (Exception exception)
            {
                return null;
            }
        }
        public void GetMainObject()
        {

        }
        public IEnumerable<DashboardDataEntry> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _unitOfWork.Database.SqlQuery<DashboardDataEntry>(query, parameters);
        }
        public class FilterCollection : List<Filter>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlRow = new SqlDataRecord(new SqlMetaData("Filter", SqlDbType.VarChar, 50));

                foreach (Filter filter in this)
                {
                    sqlRow.SetString(0, filter.FilterName);

                    yield return sqlRow;
                }
            }
        }

        public class Filter
        {
            public string FilterName { get; set; }
        }

        #endregion
    }
}
