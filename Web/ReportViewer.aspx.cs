using System;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Drawing.Printing;
using Cats.Helpers;
using Cats.Library;

namespace SSRS_Portal
{
    public partial class ReportViewer : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        private string _reportName;
        private string _sessionPDFFileName;
        protected string iFrameURL;

        protected void Page_Load(object sender, EventArgs e)
        {
            _reportName = "ReportName";
            _sessionPDFFileName = Session.SessionID.ToString() + ".pdf";
            //Attach pdf to the iframe
            string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + _sessionPDFFileName;
            iFrameURL = url;
            RegisterClientsCallBackReference();
            RegisterClientCallBackAfterPrint();
            this.Page.Title = _reportName;
            if (!Page.IsPostBack)
            {
                /*rvREXReport.ProcessingMode = ProcessingMode.Remote;
                var userName = System.Configuration.ConfigurationManager.AppSettings["CatsReportUserName"];
                var password = System.Configuration.ConfigurationManager.AppSettings["CatsReportPassword"];
                var url = System.Configuration.ConfigurationManager.AppSettings["CatsReportServerURL"];
                var credential = new CatsReportServerCredentials(userName, password);
                rvREXReport.ServerReport.ReportServerCredentials = credential;
                rvREXReport.ServerReport.ReportServerUrl = new Uri(url);
                rvREXReport.ServerReport.ReportPath = Request["path"];
                rvREXReport.ServerReport.Refresh();*/
                //ReportViewer1.ShowPrintButton = CanPrintReport;
                //ReportViewer1.ShowExportControls = CanExportReport; 

                // Set the report server URL and report path
                string _reportServerUrl = ConfigurationManager.AppSettings["CatsReportServerURL"];
                string _reportPath = ConfigurationManager.AppSettings["ReportPath"];
                string _userName = ConfigurationManager.AppSettings["CatsReportUserName"];
                string _pasword = ConfigurationManager.AppSettings["CatsReportPassword"];
                // Set the processing mode for the ReportViewer to Remote
                rvREXReport.ProcessingMode = ProcessingMode.Remote;
                ServerReport serverReport = rvREXReport.ServerReport;
                rvREXReport.ServerReport.ReportServerCredentials = new ReportViewerCredentials(_userName, _pasword);
                serverReport.ReportServerUrl = new Uri(_reportServerUrl);
                serverReport.ReportPath = Request["path"];
                serverReport.Refresh();
                var currentUser = UserAccountHelper.GetCurrentUser();
                if (currentUser.RegionalUser)
                {
                    string regionId = currentUser.RegionID.ToString();
                    List<ReportParameter> parameters = new List<ReportParameter> { new ReportParameter("Region", regionId) };
                    rvREXReport.ServerReport.SetParameters(parameters);
                }
                rvREXReport.ShowPrintButton = true;
                rvREXReport.ShowRefreshButton = true;
                rvREXReport.ShowZoomControl = true;
                rvREXReport.ShowToolBar = true;
            }

        }
        protected void ShowPrintButton()
        {

            string script = "<SCRIPT LANGUAGE='JavaScript'> ";
            script += "showPrintButton()";
            script += "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowStatus", "javascript:showPrintButton();showDatePicker();", true);

        }

        protected void SavePDF()
        {
            string _reportPath = Request["path"];
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            rvREXReport.LocalReport.ReportPath = _reportPath + _reportName;

            byte[] bytes = rvREXReport.ServerReport.Render("PDF", "", out mimeType, out encoding, out extension, out streamids, out warnings);
            //save the pdf byte to the folder
            if (!File.Exists(Server.MapPath(_sessionPDFFileName)))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(Server.MapPath(_sessionPDFFileName))))
                {
                    sw.Write("");
                }
            }
            FileStream fs = new FileStream(Server.MapPath(_sessionPDFFileName), FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        protected void rrvREXReport_ReportRefresh(object sender, CancelEventArgs e)
        {
            ShowPrintButton();
        }

        public string AjaxCall(string name)
        {
            //var ajaxcall = Request.Form["ajaxcall"];
            if (name != null)
            {
                if (File.Exists(Server.MapPath(_sessionPDFFileName)))
                {
                    frmPrint.Attributes["src"] = "";
                    File.Delete(Server.MapPath(_sessionPDFFileName));
                }
                else
                {
                    SavePDF();
                }
            }
            return name;
        }


        string returnValue;
        string ICallbackEventHandler.GetCallbackResult()
        {
            return returnValue;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            returnValue = AjaxCall(eventArgument);
        }

        private void RegisterClientsCallBackReference()
        {

            String myClientsCallBack = Page.ClientScript.GetCallbackEventReference(this, "arg", "ServerCallSucceeded", "context", "ServerCallFailed", true);
            //Could also call this wtihout the callback succeeded or failed methods:
            String myCompleteClientFunction = @"function CallServerMethodBeforePrint(arg, context)
                                      { " +
                                            myClientsCallBack + @"; 
                                      }";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TheScriptToCallServer", myCompleteClientFunction, true);

        }

        private void RegisterClientCallBackAfterPrint()
        {
            String myClientAfterPrintCallBack = Page.ClientScript.GetCallbackEventReference(this, "arg", "ServerCallSucceededAfterPrint", "context", "ServerCallFailed", true);

            String myAfterPrintCompleteClientFunction = @"function CallServerAfterPrint(arg, context)
                                      { " +
                                            myClientAfterPrintCallBack + @"; 
                                      }";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TheScriptToCallServerAfterPrint", myAfterPrintCompleteClientFunction, true);
        }
    }
    
}