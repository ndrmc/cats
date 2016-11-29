using Cats.Models;
using Cats.Models.Hubs;

namespace Cats.Services.Workflows
{
    public interface IWorkflowActivityService
    {
    

        bool EnterCreateWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultCreate", string fileName = "");
        bool EnterCreateWorkflow(IWorkflowHub workflowImplementer, string description = "Workflow_DefaultCreate", string fileName = "");
        bool EnterCreateWorkflow(IWorkflow workflowImplementer, string description = "Workflow_DefaultCreate", string fileName = "");
        bool EnterCreateWorkflow(Models.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultCreate", string fileName = "");
        bool EnterCreateWorkflow(int? businessProcessID, int finalStateID, string description = "Workflow_DefaultCreate", string fileName = "", bool isHub = false);
        bool EnterDeleteWorkflow(int? businessProcessID, int finalStateID, string description = "Workflow_DefaultDelete", string fileName = "", bool isHub = false);
        bool EnterDeleteWorkflow(IWorkflowHub workflowImplementer, string description = "Workflow_DefaultDelete", string fileName = "");
        bool EnterDeleteWorkflow(IWorkflow workflowImplementer, string description = "Workflow_DefaultDelete", string fileName = "");
        bool EnterDeleteWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultDelete", string fileName = "");
        bool EnterDeleteWorkflow(Models.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultDelete", string fileName = "");
        bool EnterEditWorkflow(IWorkflowHub workflowImplementer, string description = "Workflow_DefaultEdit", string fileName = "");
        bool EnterEditWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultEdit", string fileName = "");
        bool EnterEditWorkflow(IWorkflow workflowImplementer, string description = "Workflow_DefaultEdit", string fileName = "");
        bool EnterEditWorkflow(Models.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultEdit", string fileName = "");
        bool EnterEditWorkflow(int? businessProcessID, int finalStateID, string description = "Workflow_DefaultEdit", string fileName = "", bool isHub = false);
        bool EnterExportWorkflow(IWorkflow workflowImplementer, string description = "Workflow_DefaultExport", string fileName = "");
        bool EnterExportWorkflow(IWorkflowHub workflowImplementer, string description = "Workflow_DefaultExport", string fileName = "");
        bool EnterExportWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultExport", string fileName = "");
        bool EnterExportWorkflow(Models.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultExport", string fileName = "");
        bool EnterExportWorkflow(int? businessProcessID, int finalStateID, string description = "Workflow_DefaultExport", string fileName = "", bool isHub = false);
        bool EnterPrintWorkflow(IWorkflowHub workflowImplementer, string description = "Workflow_DefaultPrint", string NameofInitialStateFlowTempl = "Print", string fileName = "");
        bool EnterPrintWorkflow(IWorkflow workflowImplementer, string description = "Workflow_DefaultPrint", string NameofInitialStateFlowTempl = "Print", string fileName = "");
        bool EnterPrintWorkflow(Models.Hubs.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultPrint", string NameofInitialStateFlowTempl = "Print", string fileName = "");
        bool EnterPrintWorkflow(Models.BusinessProcess documentBusinessProcess, string description = "Workflow_DefaultPrint", string NameofInitialStateFlowTempl = "Print", string fileName = "");
        bool EnterPrintWorkflow(int? businessProcessID, int finalStateID, string description = "Workflow_DefaultPrint", string fileName = "", bool isHub = false);
        Models.BusinessProcess GetNewInstance(string description);
        Models.Hubs.BusinessProcess GetNewInstanceHub(string description);
        void InitializeWorkflow(IWorkflowHub workflowImplementer, string instanceDescription = null);
        void InitializeWorkflow(IWorkflow workflowImplementer, string instanceDescription = null);
        Models.BusinessProcess GetBusinessProcess(int businessProcessId);
        Models.Hubs.BusinessProcess GetBusinessProcessHub(int businessProcessId);

    }
}