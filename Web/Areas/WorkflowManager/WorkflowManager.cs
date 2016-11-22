using Cats.Alert;
using Cats.Models;
using Cats.Services.EarlyWarning;
using Cats.Services.Security;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cats.Areas
{
    public class WorkflowCommon : Controller
    {
        private static IBusinessProcessService _businessProcessService;

        private static String UserName = String.Empty;


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
    

 
        public WorkflowCommon()
        {
            int editId =  BusinessProcessService.GetGlobalEditStateTempId();

            UserName = HttpContext.User.Identity.Name;



        }
        public static Boolean EnterEditWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            int editId = BusinessProcessService.GetGlobalEditStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, editId, description, fileName);
        }
        private static Boolean EnterEditWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultEdit", String fileName = "")
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

            EnterWorkflow(businessProcessID, finalStateID, fileName, msg);


            return true;
        }
        public static Boolean EnterPrintWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultPrint", String NameofInitialStateFlowTempl = "Print", String fileName = "")
        {

            int PrintId = BusinessProcessService.GetGlobalPrintStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, PrintId, description, fileName);
        }

        private static Boolean EnterPrintWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultPrint", String fileName = "")
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

            EnterWorkflow(businessProcessID, finalStateID, fileName, msg);


            return true;
        }

        public static Boolean EnterDelteteWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultDelete", String fileName = "")
        {
            int deleteId = BusinessProcessService.GetGlobalDeleteStateTempId();

            return EnterDeleteWorkflow(documentBusinessProcess.BusinessProcessID, deleteId, description, fileName);


        }

        private static Boolean EnterDeleteWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultDelete", String fileName = "")
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

            EnterWorkflow(businessProcessID, finalStateID, fileName, msg);

            return true;
        }

        private static void EnterWorkflow(int businessProcessID, int finalStateID, string fileName, string msg)
        {

            var businessProcessState = new BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = UserName,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID
            };


            BusinessProcessService.PromotWorkflow_WoutUpdatingCurrentStatus(businessProcessState);
        }
    }
}
