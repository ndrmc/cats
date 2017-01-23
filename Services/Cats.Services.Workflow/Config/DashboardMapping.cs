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
        // int, string, string => UniqueKey, PageName, Workflow-Definition
        public static List<Tuple<int, string, string>> PageNameToWorkflowMappingsList = new List<Tuple<int, string, string>>();
        // int, string, string, Type, string, string => uniqueKey, PageName, Workflow-Definition, Entity, SearchField, and link Address - Uri
        public static List<Tuple<int, string, string, Type, string, string>> WorkflowToObjectMappings = new List<Tuple<int, string, string, Type, string, string>>();
        private static readonly Random Random = new Random(int.MinValue);
        /// <summary>
        /// These are lists of workflow types that were implemented using the old way of adding states to the flow template.
        /// the reason for keeping this workflows is that we need not to process Global workflow activities for the document bc activity operations are implemented as if they are normal wf.
        /// </summary>
        public static List<String> GlobalWorkflowExcluded = new List<string>();

        // This static method has to called or run always on application startup
        public static void AddDashboarMapping(string pageName, string workflowImplementer)
        {
            PageNameToWorkflowMappingsList.Add(new Tuple<int, string, string>(Random.Next(), pageName, workflowImplementer));
        }
        // This static method has to called or run always on application startup
        public static void AddWorkflowToObjectMapping(string pageName, string workflowDefinition, Type type, string searchField, string uri)
        {
            WorkflowToObjectMappings.Add(new Tuple<int, string, string, Type, string, string>(Random.Next(), pageName, workflowDefinition, type, searchField, uri));
            WorkflowToObjectMappings.Add(new Tuple<int, string, string, Type, string, string>(Random.Next(), pageName, workflowDefinition, type, searchField, uri));
        }

        private static Boolean alreadyInitialized=false;
        public static void InitializeNonGloablWorkflowList()
        {

            if (alreadyInitialized) return;

            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.DeliveryWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.TranferReceiptPlanWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.ReciptPlanForLoanWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.DonationPlanHeaderWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.TransportOrderWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.SwapWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.RegionalRequestWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.ReceiveHubWorkflow);

            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.HRDWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.BidPlanWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.NeedAssessmentPlanWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.NeedAssessmentWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.BidWinnerWorkflow);
            GlobalWorkflowExcluded.Add(ApplicationSettings.Default.PSNPWorkflow);

            alreadyInitialized = true;
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
            AddDashboarMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
            // Workflows that are in Regional
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.FDPReceiptWorkflow);
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.DistributionWorkflow);
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
           
            // Workflows that are in Hub
            AddDashboarMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.ReceiveHubWorkflow);
            AddDashboarMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.DispatchWorkflow);
            AddDashboarMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
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
            AddDashboarMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
            // Workflows that are in Psnp
            AddDashboarMapping(Constants.PsnpPage + Constants.Dashbord, ApplicationSettings.Default.PSNPWorkflow);
            AddDashboarMapping(Constants.PsnpPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
            // Workflows that are in Procurement
            AddDashboarMapping(Constants.ProcurementPage + Constants.Dashbord, ApplicationSettings.Default.BidWinnerWorkflow);
            AddDashboarMapping(Constants.ProcurementPage + Constants.Dashbord, ApplicationSettings.Default.TransporterWorkflow);
            AddDashboarMapping(Constants.ProcurementPage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanWorkflow);
            AddDashboarMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanDetailActionWorkflow);
            // Workflows that are in Finance
            AddDashboarMapping(Constants.FinancePage + Constants.Dashbord, ApplicationSettings.Default.TransporterChequeWorkflow);
            AddDashboarMapping(Constants.FinancePage + Constants.Dashbord, ApplicationSettings.Default.GlobalWorkflow); // Unique, GlobalWorkflow
        }

        //public static void WorkflowToObject()
        //{
        //    //
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.GiftCertificateWorkflow, typeof(GiftCertificate), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.ReliefRequisitionWorkflow, typeof(ReliefRequisition), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.NeedAssessmentWorkflow, typeof(NeedAssessment), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.NeedAssessmentPlanWorkflow, typeof(NeedAssessmentHeader), string.Empty, string.Empty); // ?
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.HRDWorkflow, typeof(HRD), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.EarlywarningPage, ApplicationSettings.Default.RegionalRequestWorkflow, typeof(RegionalRequest), string.Empty, string.Empty);

        //    AddWorkflowToObjectMapping(Constants.RegionalPage, ApplicationSettings.Default.NeedAssessmentWorkflow, typeof(NeedAssessment), string.Empty, string.Empty); // ?
        //    AddWorkflowToObjectMapping(Constants.RegionalPage, ApplicationSettings.Default.DistributionWorkflow, typeof(WoredaStockDistribution), string.Empty, string.Empty); // ?
        //    AddWorkflowToObjectMapping(Constants.RegionalPage, ApplicationSettings.Default.FDPReceiptWorkflow, typeof(DeliveryReconcile), string.Empty, string.Empty); // ?

        //    AddWorkflowToObjectMapping(Constants.HubPage, ApplicationSettings.Default.ReceiveHubWorkflow, typeof(Receive), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.HubPage, ApplicationSettings.Default.DispatchWorkflow, typeof(Models.Dispatch), string.Empty, string.Empty); // ?

        //    //AddWorkflowToObjectMapping(ApplicationSettings.Default.DispatchWorkflow, typeof(Models.Hubs.Dispatch)); // ?

        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransportOrderWorkflow, typeof(TransportOrder), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransportRequisitionWorkflow, typeof(TransportRequisition), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransporterChequeWorkflow, typeof(TransporterCheque), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TransporterPaymentRequestWorkflow, typeof(TransporterPaymentRequest), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.TranferReceiptPlanWorkflow, typeof(Transfer), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.SwapWorkflow, typeof(Transfer), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow, typeof(LocalPurchase), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.ReciptPlanForLoanWorkflow, typeof(ReceiptPlan), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.DonationPlanHeaderWorkflow, typeof(DonationPlanHeader), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.DeliveryWorkflow, typeof(Delivery), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.BidPlanDetailActionWorkflow, typeof(TransportBidPlan), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.LogisticsPage, ApplicationSettings.Default.BidPlanWorkflow, typeof(TransportBidPlanDetail), string.Empty, string.Empty);

        //    AddWorkflowToObjectMapping(Constants.PsnpPage, ApplicationSettings.Default.PSNPWorkflow, typeof(RegionalPSNPPlan), string.Empty, string.Empty);

        //    AddWorkflowToObjectMapping(Constants.ProcurementPage, ApplicationSettings.Default.BidWinnerWorkflow, typeof(BidWinner), string.Empty, string.Empty);
        //    AddWorkflowToObjectMapping(Constants.ProcurementPage, ApplicationSettings.Default.TransporterWorkflow, typeof(Models.Transporter), string.Empty, string.Empty);

        //    AddWorkflowToObjectMapping(Constants.FinancePage, ApplicationSettings.Default.TransporterPaymentRequestWorkflow, typeof(TransporterPaymentRequest), string.Empty, string.Empty);

        //    AddWorkflowToObjectMapping(Constants.FinancePage, ApplicationSettings.Default.TransporterChequeWorkflow, typeof(TransporterCheque), string.Empty, string.Empty);
        //}
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
