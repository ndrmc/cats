using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Services.Workflows
{  
    //TODO: Use T4 Template Engine to Generate Enum with the 
        //following struct for all workflow statesfrom the db 

        /// <summary>
        /// Description:Workflow for payment processing from logistics to finance 
        /// </summary>
     public class WorkflowState
    {
     public static PaymentRequest PaymentRequest;

    }
  public  class PaymentRequest
    {
        /// <summary>
        /// <para>Id => 2013</para>
        /// 
        /// From-----------------
        /// <para>RequestVerified(2354)=>PaymentRequested(2013) = Request Payment</para>
        /// <para>RequestVerified(2045)=>PaymentRequested(2013) = Request Payment</para>
        /// 
        /// 
        /// To------------------
        /// <para>PaymentRequested=>RequestVerified = Verify a Payment</para>
        /// <para>PaymentRequested=>RequestVerified = Verify a Payment</para>
        /// <para>PaymentRequested=>RequestVerified = Verify a Payment</para>
        /// 
        /// </summary>
        /// 

        public int PaymentRequested = 2013;

        /// <summary>
        /// From-----------------<para>
        /// PaymentRequested=>RequestVerified = Request Payment</para>
        /// 
        /// To------------------<para>
        /// RequestVerified=>SubmittedtoFinance = Submit To Financ</para>
        /// 
        /// </summary>
        public int RequestVerified = 2015;
        public  int SubmittedtoFinance = 2016;
        public  int Approvedbyfinance = 2017;
        public  int Closed = 2021;


    }

    public struct Constants  // this will help identify where a specific document is on
    {
        public static string Dashbord = string.Empty; // "Dashbord";
        public static string Workflow = "Workflow";
        public static string EarlywarningPage = "Earlywarning";
        public static string LogisticsPage = "Logistics";
        public static string HubPage = "Hub";
        public static string PsnpPage = "Psnp";
        public static string FinancePage = "Finance";
        public static string RegionalPage = "Regional";
        public static string ProcurementPage = "Procurement";
    }

}
