using Cats.Models;
using Cats.Services.EarlyWarning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cats
{
    public class DebbuggingTools
    {

        /// <summary>
        /// ADD THIS LINE TO A METHOD TO KNOW WHO CALLED THE METHOD, CHECK THE DEBBUGER
        /// </summary>
        /// <param name="message"></param>
      public  static void ShowMethodCaller(int frameNo = 1,String message="")
        {

            try
            {
                StackFrame frame = new StackFrame(frameNo, true);
                var method = frame.GetMethod();
                var fileName = frame.GetFileName();
                var lineNumber = frame.GetFileLineNumber();

                Debug.Flush();
                Debug.WriteLine("*------------METHOD CALLER------------------*");

                Debug.WriteLine("{0}({1}):{2} - {3}", fileName, lineNumber, method.Name, message);
            }
            catch 
            {

            }
        }

        private static IBusinessProcessService _businessProcessService;

        private static IBusinessProcessService BusinessProcessService

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

        /// <summary>
        /// SHOW BUSINESS PROCESS STATE HISTORY FOR BUSINESS PROCESS
        /// </summary>
        /// <param name="businessProcessId"></param>
        public static void ShowHistory(int businessProcessId)
        {
            try
            {

                BusinessProcess item = BusinessProcessService.FindById(businessProcessId);
                if (item == null || item.BusinessProcessStates == null || !item.BusinessProcessStates.Any())
                {
                    Debug.Flush();
                    Debug.WriteLine("*------------ NO DATA ENTRY LOGS FOR =>" + businessProcessId + "------------------*");

                }
                else
                {

                    Debug.Flush();
                    Debug.WriteLine("*------------WORKFLOW DATA ENTRY LOGS FOR BP ID =>" + businessProcessId + "------------------*");

                    int index = 1;
                    foreach (var businessprocessstate in item.BusinessProcessStates)
                    {

                        Debug.WriteLine((index++) + " >  " + businessprocessstate.DatePerformed.ToShortDateString() + " " + businessprocessstate.DatePerformed.ToLongTimeString() + "==>" + businessprocessstate.Comment);
                    }


                }
            }
            catch 
            {

            }

        }


    }
}