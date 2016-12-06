
using System;
using System.Collections.Generic;
using Cats.Models;
using Workflow = Cats.Models.Constant.WORKFLOW;

namespace Cats.Services.EarlyWarning
{
    public interface IWorkflowStatusService : IDisposable
    {
        string GetStatusName(Workflow workflow, int statusId);
        List<WorkflowStatus> GetStatus(Workflow workflow);

    }
}