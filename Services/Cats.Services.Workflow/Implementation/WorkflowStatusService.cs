using System.Collections.Generic;
using System.Linq;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;

namespace Cats.Services.EarlyWarning
{
    public class WorkflowStatusService : IWorkflowStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkflowStatusService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public string GetStatusName(WORKFLOW workflow, int statusId)
        {
            var workflowStatus =
                _unitOfWork.WorkflowStatusRepository.Get(t => t.WorkflowID == (int)workflow && t.StatusID == statusId).
                    FirstOrDefault();
            return workflowStatus != null ? workflowStatus.Description :
             string.Empty;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }


        public List<WorkflowStatus> GetStatus(WORKFLOW workflow)
        {
          return   _unitOfWork.WorkflowStatusRepository.Get(t => t.WorkflowID == (int) workflow).ToList();
        }
    }
}
