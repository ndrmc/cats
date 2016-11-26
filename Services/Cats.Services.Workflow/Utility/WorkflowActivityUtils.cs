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
    public class WorkflowActivityUtil 
    {
        #region  Privte Declarations

        private static IBusinessProcessService _businessProcessService;

        private static IHubBusinessProcessService _hubBusinessProcessService;
        private static IBusinessProcessService BusinessProcessService

        {
            get
            {
                //if (_businessProcessService == null)
                {

                    _businessProcessService =
                        (IBusinessProcessService)
                            DependencyResolver.Current.GetService(typeof(IBusinessProcessService));


                }
                return _businessProcessService;

            }

            set { _businessProcessService = value; }
        }
        private static IHubBusinessProcessService HubBusinessProcessService

        {
            get
            {
                //if (_hubBusinessProcessService == null)
                {

                    _hubBusinessProcessService =
                        (IHubBusinessProcessService)
                            DependencyResolver.Current.GetService(typeof(IHubBusinessProcessService));


                }
                return _hubBusinessProcessService;

            }

            set { _hubBusinessProcessService = value; }
        }
        private static IApplicationSettingService ApplicationSettingService

        {
            get
            {
                //if (_applicationSettingService == null)
                {

                    _applicationSettingService =
                        (IApplicationSettingService)
                            DependencyResolver.Current.GetService(typeof(IApplicationSettingService));


                }
                return _applicationSettingService;

            }

            set { _applicationSettingService = value; }
        }

        private static String userName;

        private static IApplicationSettingService _applicationSettingService;
        public static string UserName
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

        public static Models.BusinessProcess GetNewInstance(string description)
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
        public static Models.Hubs.BusinessProcess GetNewInstanceHub(string description)
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
        public static void InitializeWorkflow(IWorkflow workflowImplementer, String instanceDescription = null)
        {
            if (workflowImplementer.BusinessProcessId == 0)
            {
                workflowImplementer.BusinessProcess = GetNewInstance(instanceDescription);
                workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
            }
        }
        public static void InitializeWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String instanceDescription = null)
        {
            if (workflowImplementer.BusinessProcessId == 0)
            {
                workflowImplementer.BusinessProcess = GetNewInstanceHub(instanceDescription);
                workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
            }
        }

        #endregion

        #region  public Static Methods

        public static Boolean EnterCreateWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;

           
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);
            

            int editId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
        public static Boolean EnterCreateWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;

         
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);
        

            int editId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName,true);
        }
        public static Boolean EnterEditWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
        public static Boolean EnterPrintWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);


            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName);
        }
        public static Boolean EnterPrintWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName, true);

        }
        public static Boolean EnterDelteteWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName);


        }
        public static Boolean EnterDelteteWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName, true);



        }
        public static Boolean EnterExportWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName, true);

        }
        public static Boolean EnterExportWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;


            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);


            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName);
        }
        public static Boolean EnterEditWorkflow(Models.Hubs.IWorkflowHub workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);


            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName, true);

        }

        #endregion

        #region Private Static Methods
        public static Boolean EnterCreateWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        public static Boolean EnterCreateWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterCreateWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        public static Boolean EnterCreateWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultCreate", String fileName = "", bool isHub = false)
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
        public static Boolean EnterEditWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        public static Boolean EnterEditWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        public static Boolean EnterEditWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultEdit", String fileName = "", bool isHub = false)
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
        public static Boolean EnterPrintWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, PrintId, description, fileName);
        }
        public static Boolean EnterPrintWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, PrintId, description, fileName);
        }
        public static Boolean EnterPrintWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultPrint", String fileName = "", bool isHub = false)
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
        public static Boolean EnterDelteteWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(documentBusinessProcess.BusinessProcessID, deleteId, description, fileName);


        }
        public static Boolean EnterDelteteWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(documentBusinessProcess.BusinessProcessID, deleteId, description, fileName);


        }
        public static Boolean EnterDeleteWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultDelete", String fileName = "", bool isHub = false)
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
        public static Boolean EnterExportWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        public static Boolean EnterExportWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterEditWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        public static Boolean EnterExportWorkflow(int? businessProcessID, int finalStateID, String description = "Workflow_DefaultExport", String fileName = "", bool isHub = false)
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
        public static void EnterWorkflow(int? businessProcessID, int finalStateID, string fileName, bool isHub, string msg)
        {
            if (!isHub)
                EnterWorkflow(businessProcessID, finalStateID, fileName, msg);
            else
                EnterWorkflowHub(businessProcessID, finalStateID, fileName, msg);
        }
        public static void EnterWorkflow(int? businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName, 
                ParentBusinessProcessID = businessProcessID??0
            };


            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            BusinessProcessService.PromotWorkflow_WoutUpdatingCurrentStatus(businessProcessState);


            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

        }
        public static void EnterWorkflowHub(int? businessProcessID, int finalStateID, string fileName, string msg)
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
        #endregion

        

    }
}
