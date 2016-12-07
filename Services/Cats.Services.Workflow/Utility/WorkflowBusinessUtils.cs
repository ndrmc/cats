using Cats.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Services.Workflows
{
  public  class WorkflowBusinessUtils
    {

      public static String Promote(int businessProcessId, int promoteToState,String comment, String attachment =null)
        {
            
            String finalWorkflowStateName = "Approve";
            return finalWorkflowStateName;

            //WorkflowBusinessUtils.Promote(2354, WorkflowState.PaymentRequest., Alert.AlertMessage.Workflow_DefaultCreate);
            //WorkflowBusine;ssUtils.Demote(1, WorkflowState.PaymentRequest.RequestVerified, Alert.AlertMessage.Workflow_DefaultCreate);

        }

        public static String Demote(int businessProcessId, int promoteToState, String comment, String attachment=null)
        {

            String finalWorkflowStateName = "Approve";
            return finalWorkflowStateName;
        }
    }
}
