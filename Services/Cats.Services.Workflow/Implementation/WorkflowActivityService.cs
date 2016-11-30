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

namespace Cats.Services.Workflows
{
    public class WorkflowActivityService : IWorkflowActivityService
    {

        #region Constructor
        public WorkflowActivityService(IBusinessProcessService businessProcessService, IApplicationSettingService applicationSettingService, IHubBusinessProcessService hubBusinessProcessService)
        {
            _businessProcessService = businessProcessService;
            _applicationSettingService = applicationSettingService;
            _hubBusinessProcessService = hubBusinessProcessService;
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

        private String userName;
        public string UserName
        {
            get
            {
                if (String.IsNullOrEmpty(userName))
                    new WorkflowActivityUtil().GetUserName();

                return userName;
            }

            set
            {
                userName = value;
            }
        }
        public String GetUserName()
        {

            UserInfo info = (UserInfo)System.Web.HttpContext.Current.Session["USER_INFO"];

            if (info != null)
                UserName = info.UserName;

            return UserName;


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
                workflowImplementer.BusinessProcess = GetNewInstance(instanceDescription);
                workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
            }
        }
        public void InitializeWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String instanceDescription = null)
        {
            if (workflowImplementer.BusinessProcessId == 0)
            {
                workflowImplementer.BusinessProcess = GetNewInstanceHub(instanceDescription);
                workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
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

            EnterWorkflow(businessProcessID, finalStateID, fileName, isHub, msg);


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

        public List<WorkflowActivity> GetWorkflowActivity(string pageName, string filter = null)
        {

            
            List<WorkflowActivity> mocks = new List<WorkflowActivity>();

            WorkflowActivity mock1 = new WorkflowActivity()
            {
                BusinessProcessStateID = 1,
                PerformedBy = "Ayele",
                Comment = Alert.AlertMessage.Workflow_DefaultCreate,
                //TargetObject = get gift certificate from unit of work
                TargetObjectJsonData = "ID:14 , Name ='GiftCert1'",
                TargetObjectReferenceId = "14",
                TargetObjectType = typeof(Models.Hubs.GiftCertificate)

            };

            WorkflowActivity mock2 = new WorkflowActivity()
            {
                BusinessProcessStateID = 1,
                PerformedBy = "Kebede",
                Comment = Alert.AlertMessage.Workflow_DefaultCreate,
                //TargetObject = get gift certificate from unit of work
                TargetObjectJsonData = "ID:15 , Name ='GiftCert1'",
                TargetObjectReferenceId = "15",
                TargetObjectType = typeof(Models.Hubs.GiftCertificate)

            };

            mocks.Add(mock1);
            mocks.Add(mock2);

            return mocks;


        }


        //public List<WorkflowActivityAgg> GetWorkflowActivityAgg(string pageName, string filter = null)

        //{

        //    return null;
        //}


        #endregion

    }
}
