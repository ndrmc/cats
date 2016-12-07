﻿using Cats.Models;
using System;
using System.Collections.Generic;
using Cats.Services.Common;

namespace Cats.Services.Workflows.Config
{
    public class DashboardMapping
    {
        // int, string, string => UniqueKey, PageName, WorkflowDefinition
        public static List<Tuple<int, string, string>> PageNameToWorkflowMappingsList = new List<Tuple<int, string, string>>();
        private static readonly Random Random = new Random(Int32.MinValue);

        public static void AddMapping(string pageName, string workflowImplementer)
        {
            PageNameToWorkflowMappingsList.Add(new Tuple<int, string, string>(Random.Next(), pageName, workflowImplementer));
        }
        public static void RegisterDashboardPage()
        {
            // Workflows that are in Earlywarning
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.GiftCertificateWorkflow);
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.ReliefRequisitionWorkflow);
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentPlanWorkflow);
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.HRDWorkflow);
            AddMapping(Constants.EarlywarningPage + Constants.Dashbord, ApplicationSettings.Default.RegionalRequestWorkflow);
            // Workflows that are in Regional
            AddMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);
            //AddMapping(Constants.RegionalPage + Constants.Dashbord, ApplicationSettings.Default.NeedAssessmentWorkflow);

            // Workflows that are in Hub
            AddMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.ReceiveHubWorkflow);
            AddMapping(Constants.HubPage + Constants.Dashbord, ApplicationSettings.Default.DispatchWorkflow);
            // Workflows that are in Logistics
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransportOrderWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransportRequisitionWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransporterChequeWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TransporterPaymentRequestWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.TranferReceiptPlanWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.SwapWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.LocalPurchaseReceiptPlanWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.ReciptPlanForLoanWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.DonationPlanHeaderWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.DeliveryWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanDetailActionWorkflow);
            AddMapping(Constants.LogisticsPage + Constants.Dashbord, ApplicationSettings.Default.BidPlanWorkflow);
            // Workflows that are in Psnp
            AddMapping(Constants.PsnpPage + Constants.Dashbord, ApplicationSettings.Default.PSNPWorkflow);
            // Workflows that are in Procurement
            AddMapping(Constants.ProcurementPage + Constants.Dashbord, ApplicationSettings.Default.BidWinnerWorkflow);
            // Workflows that are in Finance
            AddMapping(Constants.FinancePage + Constants.Dashbord, ApplicationSettings.Default.TransporterChequeWorkflow);
        }

        private static void StartupConfiguration_StartWorkflow(string pageName, String workflowName)
        {

        }
        //private static void StartupConfiguration_TakeTop(string pageName, int topX)
        //{
        //    StartupConfiguration_TakeTop("EarlyWarningDashboard", 100);
        //    StartupConfiguration_StartWorkflow("EarlyWarningDashboard", "GiftCertificate");
        //}
        public static void AddMapping(String pageName,Type workflowImplementer )
        {

        }
    }
}
