using Cats.Alert;
using Cats.Models;
using Cats.Services.EarlyWarning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cats.Areas.Workflow
{
    public  class WorkflowManager 
    {
        private static  IBusinessProcessService _businessProcessService;

        public WorkflowManager(IBusinessProcessService businessProcessService)
        {
            _businessProcessService = businessProcessService;


        }

        public static Boolean EnterEditWorkflow(int businessProcessID, int finalStateID, String performedBy, String fileName="",String description="Workflow_DefaultEdit")
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

            EnterWorkflow(businessProcessID, finalStateID, performedBy, fileName, msg);


            return true;
        }


        public static Boolean EnterPrintWorkflow(int businessProcessID, int finalStateID, String performedBy, String fileName = "", String description = "Workflow_DefaultPrint")
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

            EnterWorkflow(businessProcessID, finalStateID, performedBy, fileName, msg);


            return true;
        }

        public static Boolean EnterDeleteWorkflow(int businessProcessID, int finalStateID, String performedBy, String fileName = "", String description = "Workflow_DefaultDelete")
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

            EnterWorkflow(businessProcessID, finalStateID,  performedBy, fileName, msg);

            return true;
        }

        private static void EnterWorkflow(int businessProcessID, int finalStateID, String performedBy,string fileName, string msg)
        {
            var businessProcessState = new BusinessProcessState()
            {
                StateID = finalStateID,
                PerformedBy = performedBy,
                DatePerformed = DateTime.Now,
                Comment = msg,
                AttachmentFile = fileName,
                ParentBusinessProcessID = businessProcessID
            };


            _businessProcessService.PromotWorkflow(businessProcessState);
        }
    }
}