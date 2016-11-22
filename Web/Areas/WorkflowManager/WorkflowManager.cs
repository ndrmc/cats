﻿using Cats.Alert;
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


namespace Cats.Areas
{
    public class WorkflowCommon : Controller
    {
        private static IBusinessProcessService _businessProcessService;
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

        public void GetUserName()
        {
            UserName = HttpContext.User.Identity.Name;



        }

        public static BusinessProcess GetNewInstance(string description)
        {
            BusinessProcess bp = new BusinessProcess();
            int BP_PR = ApplicationSettingService.getGlobalWorkflow();

            if (BP_PR != 0)
            {
                var createdstate = new BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = UserName,
                    Comment = description
                };

                bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,"Created", createdstate);

            }
            return bp;
        }

        #region Enter workflow Methods


        public static Boolean EnterCreateWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultCreate", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int CreateId = BusinessProcessService.GetGlobalCreatedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, CreateId, description, fileName);
        }
        private static Boolean EnterCreateWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultCreate", String fileName = "")
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

            EnterWorkflow(businessProcessID, finalStateID, fileName, msg);


            return true;
        }


        public static Boolean EnterEditWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultEdit", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

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
            if (documentBusinessProcess == null) return false;

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
            if (documentBusinessProcess == null) return false;

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

        public static Boolean EnterExportWorkflow(BusinessProcess documentBusinessProcess, String description = "Workflow_DefaultExport", String fileName = "")
        {
            if (documentBusinessProcess == null) return false;

            int ExportId = BusinessProcessService.GetGlobalExportedStateTempId();

            return EnterPrintWorkflow(documentBusinessProcess.BusinessProcessID, ExportId, description, fileName);
        }
        private static Boolean EnterExportWorkflow(int businessProcessID, int finalStateID, String description = "Workflow_DefaultExport", String fileName = "")
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
        #endregion


    }
}
