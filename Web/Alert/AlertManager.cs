using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Alert
{
    public static class AlertManager
    {
     
        public static String GetWorkflowMessage_Print(String documentName)
        {
            //TODO:get current culture
            //if amharic use amharic ressource file
            //else if English
            return String.Format(AlertMessage.Workflow_PrintwithParam1, documentName);
        }
        public static String GetWorkflowMessage_Edit(String documentName)
        {
            //TODO:get current culture
            //if amharic use amharic ressource file
            //else if English
            return String.Format(AlertMessage.Workflow_EditwithParam1, documentName);
        }
        public static String GetWorkflowMessage_Delete(String documentName)
        {

            return String.Format(AlertMessage.Workflow_DeleteWithParam1, documentName);
        }
        public static String GetWorkflowMessage_ReliefReqDetail(String _for,String from,String to)
        {

            return String.Format(AlertMessage.Workflow_ReliefReqDetailParam3, _for, from,to);
        }

    }
}