using Cats.Models;
using Cats.Services.EarlyWarning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                //if (_businessProcessService == null)
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
        public static void ShowHistory(int? businessProcessId)
        {
            try
            {
                String logMsg = String.Empty;

                BusinessProcess item = BusinessProcessService.FindById(businessProcessId??0);
                if (item == null || item.BusinessProcessStates == null || !item.BusinessProcessStates.Any())
                {
                    logMsg = "*------------ NO DATA ENTRY LOGS FOR =>" + businessProcessId + "------------------*";
                    Debug.WriteLine(logMsg);
                    WriteLog(logMsg);


                }
                else
                {
                    logMsg = "*------------WORKFLOW DATA ENTRY LOGS FOR BP ID =>" + businessProcessId + "------------------*";
                    Debug.WriteLine(logMsg);
                    WriteLog(logMsg);

                    int index = 1;
                    foreach (var businessprocessstate in item.BusinessProcessStates)
                    {

                        logMsg = (index++) + " >  " + businessprocessstate.DatePerformed.ToShortDateString() + " " + businessprocessstate.DatePerformed.ToLongTimeString() + "==>" + businessprocessstate.Comment;
                        WriteLog(logMsg);
                        Debug.WriteLine(logMsg);
                    }


                }
            }
            catch 
            {

            }

        }

        public static void WriteLog(string strLog)
        {
            try
            {
                StreamWriter log;
                FileStream fileStream = null;
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo;

                string logFilePath = "C:\\Workflow Logs\\";
                logFilePath = logFilePath + "Workflow_Log.txt";// + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
                logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                log = new StreamWriter(fileStream);
                log.WriteLine(strLog);
                log.Close();
            }
            catch 
            {

            }
        }


    }
}