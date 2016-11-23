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
using Cats.Services.Hub;
using System.Diagnostics;

namespace Cats.Areas
{
    public class WorkflowCommon : Controller
    {
        private static IBusinessProcessService _businessProcessService;
        private static IHubBusinessProcessService _hubBusinessProcessService;
        private static IApplicationSettingService _applicationSettingService;

        private static String userName ;


        public static IBusinessProcessService BusinessProcessService

        {
            get
            {
                if (_businessProcessService == null)
                {

                    _businessProcessService =
                        (IBusinessProcessService)
                            DependencyResolver.Current.GetService(typeof (IBusinessProcessService));


                }
                return _businessProcessService;

            }

            set { _businessProcessService = value; }
        }

        public static IHubBusinessProcessService HubBusinessProcessService

        {
            get
            {
                if (_hubBusinessProcessService == null)
                {

                    _hubBusinessProcessService =
                        (IHubBusinessProcessService)
                            DependencyResolver.Current.GetService(typeof(IHubBusinessProcessService));


                }
                return _hubBusinessProcessService;

            }

            set { _hubBusinessProcessService = value; }
        }


        public static IApplicationSettingService ApplicationSettingService

        {
            get
            {
                if (_applicationSettingService == null)
                {

                    _applicationSettingService =
                        (IApplicationSettingService)
                            DependencyResolver.Current.GetService(typeof(IApplicationSettingService));


                }
                return _applicationSettingService;

            }

            set { _applicationSettingService = value; }
        }

        public  string UserName
        {
            get
            {
                if(String.IsNullOrEmpty(userName))
                new WorkflowCommon().GetUserName();

                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public String GetUserName()
        {
            UserName = HttpContext.User.Identity.Name;
            return UserName;


        }

        public static Models.BusinessProcess GetNewInstance(string description)
        {
            Models.BusinessProcess bp = new Models.BusinessProcess();
            int BP_PR = ApplicationSettingService.getGlobalWorkflow();

            if (BP_PR != 0)
            {
                var createdstate = new Models.BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = userName,
                    Comment = description
                };

                bp = BusinessProcessService.CreateBusinessProcessWithOutStateEntry(BP_PR, 0,"Created");

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
                    PerformedBy = userName,
                    Comment = description
                };

                bp = HubBusinessProcessService.CreateBusinessProcessWithOutStateEntry(BP_PR, 0, "Created");

            }
            return bp;
        }

        #region Enter workflow Methods

        public static Boolean EnterCreateWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);
            }

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
        public static Boolean EnterCreateWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultCreate);
            }

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }
       
        private static void InitializeWorkflow(IWorkflow workflowImplementer ,String instanceDescription )
        {
            workflowImplementer.BusinessProcess = GetNewInstance(instanceDescription);
            workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
        }

        private static void InitializeWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String instanceDescription)
        {
            workflowImplementer.BusinessProcess = GetNewInstanceHub(instanceDescription);
            workflowImplementer.BusinessProcessId = workflowImplementer.BusinessProcess.BusinessProcessID;
        }

        public static Boolean EnterCreateWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }

        public static Boolean EnterCreateWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        private static Boolean EnterCreateWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultCreate", String fileName = "", bool isHub = false)
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


        public static Boolean EnterEditWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                if (workflowImplementer.BusinessProcess == null)
                {
                    InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);
                }
            }

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }


        public static Boolean EnterEditWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                if (workflowImplementer.BusinessProcess == null)
                {
                    InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultEdit);
                }
            }

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, editId, description, fileName);
        }


        public static Boolean EnterEditWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }

        public static Boolean EnterEditWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        private static Boolean EnterEditWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultEdit", String fileName = "", bool isHub = false)
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
        public static Boolean EnterPrintWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);
            }

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName);
        }

        public static Boolean EnterPrintWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultPrint);
            }

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, PrintId, description, fileName);
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
        private static Boolean EnterPrintWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultPrint", String fileName = "", bool isHub = false)
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
        public static Boolean EnterDelteteWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);
            }
            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName);


        }
        public static Boolean EnterDelteteWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultDelete);
            }
            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(workflowImplementer.BusinessProcessId, deleteId, description, fileName);


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
        private static Boolean EnterDeleteWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultDelete", String fileName = "", bool isHub = false)
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

        public static Boolean EnterExportWorkflow(Models.Hubs.IWorkflowHub  workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);
            }

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName);
        }
        public static Boolean EnterExportWorkflow(IWorkflow workflowImplementer, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (workflowImplementer == null) return false;

            if (workflowImplementer.BusinessProcess == null)
            {
                InitializeWorkflow(workflowImplementer, AlertMessage.Workflow_DefaultExport);
            }
        
            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterPrintWorkflow(workflowImplementer.BusinessProcessId, ExportId, description, fileName);
        }
        public static Boolean EnterExportWorkflow(Models.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        public static Boolean EnterExportWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        private static Boolean EnterExportWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultExport", String fileName = "", bool isHub = false)
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

        private static void EnterWorkflow(int businessProcessID, int finalStateID, string fileName, bool isHub, string msg)
        {
            if (!isHub)
                EnterWorkflow(businessProcessID, finalStateID, fileName, msg);
            else
                EnterWorkflowHub(businessProcessID, finalStateID, fileName, msg);
        }

        private static void EnterWorkflow(int businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = userName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName, 
                ParentBusinessProcessID = businessProcessID
            };


            Debug.WriteLine("*-----------WORKFLOW COMMEN BEFORE ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

            BusinessProcessService.PromotWorkflow_WoutUpdatingCurrentStatus(businessProcessState);


            Debug.WriteLine("*-----------WORKFLOW COMMEN AFTER ENTERING LOG");
            DebbuggingTools.ShowHistory(businessProcessID);

        }

        private static void EnterWorkflowHub(int businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new Models.Hubs.BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = userName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID
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
