using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Alert
{
    public static class AlertManager
    {
        public static String GetWorkflowEdifFDPReceipt(String documentName)
        {
            //TODO:get current culture
            //if amharic use amharic ressource file
            //else if English
            return String.Format(AlertMessage.Workflow_FDPReceipt_EditedParam1, documentName);
        }

        public static String GetWorkflowPrintMessage(String documentName)
        {
            //TODO:get current culture
            //if amharic use amharic ressource file
            //else if English
            return String.Format(AlertMessage.Workflow_PrintwithParam1, documentName);
        }

    }
}