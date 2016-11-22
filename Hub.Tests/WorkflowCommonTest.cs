

using Cats.Alert;
using Cats.Areas;

using Cats.Models;
using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hub.Tests
{
    class WorkflowCommonTest
    {
        public static void main()
        {

        }


        public void RunTest()
        {
            String unique =Convert.ToInt16( GetUniqueNo()).ToString();
            int id = Convert.ToInt16( GetUniqueNo());
            
            //Create New Gift Cert 
            GiftCertificate newGiftCert = new GiftCertificate();
            newGiftCert.GiftCertificateID = id;
            //Get the gift Cert 

            GiftCertificateService.AddGiftCertificate(newGiftCert);

            GiftCertificate giftCert = GiftCertificateService.FindById(id);
            //Use all methods of workflow common

            Console.WriteLine("Starting to create workflows");

            WorkflowCommon.EnterCreateWorkflow(newGiftCert,AlertMessage.Workflow_DefaultCreate+unique);
            Console.WriteLine(AlertMessage.Workflow_DefaultCreate);

            WorkflowCommon.EnterEditWorkflow(newGiftCert, AlertMessage.Workflow_DefaultEdit + unique);
            Console.WriteLine(AlertMessage.Workflow_DefaultEdit);

            WorkflowCommon.EnterPrintWorkflow(newGiftCert, AlertMessage.Workflow_DefaultPrint + unique);
            Console.WriteLine(AlertMessage.Workflow_DefaultPrint);

            WorkflowCommon.EnterExportWorkflow(newGiftCert, AlertMessage.Workflow_DefaultExport + unique);
            Console.WriteLine(AlertMessage.Workflow_DefaultExport);

            WorkflowCommon.EnterDelteteWorkflow(newGiftCert, AlertMessage.Workflow_DefaultDelete + unique);
            Console.WriteLine(AlertMessage.Workflow_DefaultDelete);
            //Call History method


            Cats.Models.BusinessProcess item = BusinessProcessService.FindById(newGiftCert.BusinessProcessId);

            ShowHistory(item);
        }

        private void ShowHistory(Cats.Models.BusinessProcess item)
        {
            Console.WriteLine("HISTORY");

            foreach(Cats.Models.BusinessProcessState state in item.BusinessProcessStates)
            {

                Console.WriteLine(state.Comment);
            }

        }


        private static String userName;

        private static Cats.Services.EarlyWarning.IGiftCertificateService _giftCertificateService;


        public static Cats.Services.EarlyWarning.IGiftCertificateService GiftCertificateService

        {
            get
            {
                if (_giftCertificateService == null)
                {

                    _giftCertificateService =
                        (Cats.Services.EarlyWarning.IGiftCertificateService)
                            DependencyResolver.Current.GetService(typeof(Cats.Services.EarlyWarning.IGiftCertificateService));


                }
                return _giftCertificateService;

            }

            set { _giftCertificateService = value; }
        }


        private static IBusinessProcessService _businessProcessService;


        public static IBusinessProcessService BusinessProcessService

        {
            get
            {
                if (_businessProcessService == null)
                {

                    _businessProcessService =
                        (IBusinessProcessService)
                            DependencyResolver.Current.GetService(typeof(IBusinessProcessService));


                }
                return _businessProcessService;

            }

            set { _businessProcessService = value; }
        }



        private long GetUniqueNo()
        {
            
            return DateTime.Now.Ticks;
        }
    }
}
