using System;
using System.Collections.Generic;
using Cats.Models;
using Cats.Models.Hubs;
using Cats.Services.Common;
using GiftCertificate = Cats.Models.GiftCertificate;

namespace Cats.Services.Workflows.Config
{
    public class DashboardMapping
    {
        // int, string, string => UniqueKey, PageName, WorkflowDefinition
        public static List<Tuple<int, string, string>> PageNameToWorkflowMappingsList = new List<Tuple<int, string, string>>();
        public static List<Tuple<int, string, string, Type>> WorkflowToObjectMappings = new List<Tuple<int, string, string, Type>>(); 
        private static readonly Random Random = new Random(int.MinValue);

        public static void AddDashboarMapping(string pageName, string workflowImplementer)
        {
            PageNameToWorkflowMappingsList.Add(new Tuple<int, string, string>(Random.Next(), pageName, workflowImplementer));
        }
        public static void AddWorkflowToObjectMapping(string pageName, string workflowDefinition, Type type)
        {
            WorkflowToObjectMappings.Add(new Tuple<int, string, string, Type>(Random.Next(), pageName, workflowDefinition, type));
        }
        public static void RegisterDashboardPage()
        {
            // Workflows that are in Earlywarning
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.GiftCertificateWorkflow);
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.ReliefRequisitionWorkflow);
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentPlanWorkflow);
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.HRDWorkflow);
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.RegionalRequestWorkflow);
            // Workflows that are in Regional
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);
            //AddMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);

            // Workflows that are in Hub
            AddDashboarMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.ReceiveHubWorkflow);
            AddDashboarMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.DispatchWorkflow);
            // Workflows that are in Logistics
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransportOrderWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransportRequisitionWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransporterChequeWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransporterPaymentRequestWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TranferReceiptPlanWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.SwapWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.ReciptPlanForLoanWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.DonationPlanHeaderWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.DeliveryWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanDetailActionWorkflow);
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanWorkflow);
            // Workflows that are in Psnp
            AddDashboarMapping(Constants.PsnpPage + Constants.Dashbord, ApplicationSettings.Default.PSNPWorkflow);
            // Workflows that are in Procurement
            AddDashboarMapping(Constants.ProcurementPage + Constants.Dashbord, ApplicationSettings.Default.BidWinnerWorkflow);
            // Workflows that are in Finance
            AddDashboarMapping(Constants.FinancePage + Constants.Dashbord, ApplicationSettings.Default.TransporterChequeWorkflow);
        }

        public static void WorkflowToObject()
        {
            // 
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.GiftCertificateWorkflow, typeof(GiftCertificate));
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.ReliefRequisitionWorkflow, typeof(ReliefRequisition));
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.NeedAssessmentWorkflow, typeof(NeedAssessment));
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.NeedAssessmentPlanWorkflow, typeof(NeedAssessmentHeader)); // ?
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.HRDWorkflow, typeof(HRD));
            AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.RegionalRequestWorkflow, typeof(RegionalRequest));
            
            AddWorkflowToObjectMapping(Constants.RegionalPage, ApplicationSettings.Default.NeedAssessmentWorkflow, typeof(NeedAssessment)); // ?
            
            AddWorkflowToObjectMapping(Constants.HubPage, ApplicationSettings.Default.ReceiveHubWorkflow, typeof(Receive));
            AddWorkflowToObjectMapping(Constants.HubPage, ApplicationSettings.Default.DispatchWorkflow, typeof(Models.Dispatch)); // ?

            //AddWorkflowToObjectMapping(ApplicationSettings.Default.DispatchWorkflow, typeof(Models.Hubs.Dispatch)); // ?

            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransportOrderWorkflow, typeof(TransportOrder));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransportRequisitionWorkflow, typeof(TransportRequisition));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransporterChequeWorkflow, typeof(TransporterCheque));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransporterPaymentRequestWorkflow, typeof(TransporterPaymentRequest));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TranferReceiptPlanWorkflow, typeof(Transfer));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.SwapWorkflow, typeof(Transfer));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow, typeof(LocalPurchase));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.ReciptPlanForLoanWorkflow, typeof(ReceiptPlan));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.DonationPlanHeaderWorkflow, typeof(DonationPlanHeader));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.DeliveryWorkflow, typeof(Delivery));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.BidPlanDetailActionWorkflow, typeof(TransportBidPlan));
            AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.BidPlanWorkflow, typeof(TransportBidPlanDetail));
            
            AddWorkflowToObjectMapping(Constants.PsnpPage, ApplicationSettings.Default.PSNPWorkflow, typeof(RegionalPSNPPlan));
            
            AddWorkflowToObjectMapping(Constants.ProcurementPage, ApplicationSettings.Default.BidWinnerWorkflow, typeof(BidWinner));
            
            AddWorkflowToObjectMapping(Constants.FinancePage, ApplicationSettings.Default.TransporterChequeWorkflow, typeof(TransporterCheque));
        }
        //private static void StartupConfiguration_StartWorkflow(string pageName, string workflowName)
        //{

        //}
        //private static void StartupConfiguration_TakeTop(string pageName, int topX)
        //{
        //    StartupConfiguration_TakeTop("EarlyWarningDashboard", 100);
        //    StartupConfiguration_StartWorkflow("EarlyWarningDashboard", "GiftCertificate");
        //}
        public static void AddMapping(string pageName,Type workflowImplementer )
        {

        }
    }
}
