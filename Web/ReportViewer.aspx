<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="SSRS_Portal.ReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CATS: Commodity Allocation & Tracking System</title>
    <%--<script src="/Scripts/kendo/2013.1.319/jquery.min.js"></script>
    <link href="/Content/themes/default/bootstrap.min.css" rel="stylesheet"/>
    <link href="Content/jquery-ui.min.css" rel="stylesheet"/>--%>
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/Beka.EthDate/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/Beka.EthDate/calendar.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .spinner {
	        position: fixed;
	        top: 50%;
	        left: 50%;
	        margin-left: -50px; /* half width of the spinner gif */
	        margin-top: -50px; /* half height of the spinner gif */
	        text-align:center;
	        z-index:1234;
	        overflow: auto;
	        width: 150px; /* width of the spinner gif */
	        height: 62px; /*height of the spinner gif +2px to fix IE8 issue */
	        background-color:#E1E1D7; 
	        border:1px solid black;
        }

        .HighlightDiv img{
        background-color: transparent; 
        border-top-width: 1px; 
        border-right-width: 1px; 
        border-bottom-width: 1px; 
        border-left-width: 1px; 
        border-top-color: transparent; 
        border-right-color: transparent; 
        border-bottom-color: transparent; 
        border-left-color: transparent; 
        border-top-style: solid; 
        border-right-style: solid; 
        border-bottom-style: solid; 
        border-left-style: solid; 
        cursor: default; 
        }

        .HighlightDiv:hover img{
        background-color: #DDEEF7; 
        border-top-width: 1px; 
        border-right-width: 1px; 
        border-bottom-width: 1px; 
        border-left-width: 1px; 
        border-top-color: #336699; 
        border-right-color: #336699; 
        border-bottom-color: #336699; 
        border-left-color: #336699; 
        border-top-style: solid; 
        border-right-style: solid; 
        border-bottom-style: solid; 
        border-left-style: solid; 
        cursor:pointer; 
        }
     </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1"  EnablePageMethods="true" 
                EnablePartialRendering="true" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="rvREXReport" runat="server" Width="100%" Height="798px"
            Style="display: table !important; margin: 0px; overflow: auto !important;" 
            ShowBackButton="true" onreportrefresh="rrvREXReport_ReportRefresh">
            </rsweb:ReportViewer> 
            <iframe id="frmPrint" name="frmPrint" runat="server" style = "display:none"></iframe>
            <div id="spinner" class="spinner" style="display:none;">
                <table align="center" valign="middle" style="height:100%;width:100%">
                    <tr>
                        <td><img id="img-spinner" src="Content/images/loading.gif" alt="Printing"/></td>
                        <td><span style="font-family:Verdana; font-weight:bold;font-size:10pt;width:86px;">Printing...</span></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <%--<script >
        $(document).ready(function() {
            //alert("hello report");
        });
        $("#ReportViewer1_ctl04").css("background", "none");
        $("#ParameterTable_ReportViewer1_ctl04").css("background", "none");
    </script>--%>
    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="Scripts/Beka.EthDate/Beka.EthDate.js"> </script>
    <script type="text/javascript" src="Scripts/Beka.EthDate/jquery.Beka.EthCalDatePicker.js"> </script>
    <script type="text/javascript" src="Scripts/js/CatsUI.js"> </script>
    <script>
        $(document).ready(function () {
            
            $("#spinner").bind("ajaxSend", function() {
                $(this).show();
            }).bind("ajaxStop", function() {
                $(this).hide();
            }).bind("ajaxError", function() {
                $(this).hide();
            });

            var browser = get_browser_info();
            
            $(function () {
                if (browser.name.includes("IE")) {
                    //alert(browser.name);
                }
                else {
                    
                    //if (IsPrefCalendarEthiopian())
                    //    {
                    ////alert(window.navigator.userAgent);
                    //showDatePicker();
                    ////showPrintButton();
                    //$("select").on("focusout", function () {
                    //    showDatePicker();
                    //    console.log("Show date picker: Select");
                    //    //showPrintButton();
                    //    //alert();
                    //});
                    //$("input[type=checkbox]").on("focusout", function () {
                    //    //setTimeout(showDatePicker, 1000);
                    //    showDatePicker();
                    //    console.log("Show date picker: Input");
                    //    //showPrintButton();
                    //});
                    }
                
                

            });
        });

        var browser = get_browser_info();

        var messageElem = 'AlertMessage';
        if (browser.name.includes("IE")) {
            //alert(browser.name);
        } else {
            // Register a page load event on asynchronuous requests to bind the showDatePicker event to date fields 
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoadHandler);
            // Register an end request event on asynchronuous requests to enable the ASP Disabled date fields 
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function PageLoadHandler(sender, args) {
            showDatePicker();

            console.log("Show date picker: Input");
        }
        function EndRequestHandler(sender, args) {

            $("input[type=text]").css("background-color", "#fff");
            $("input[type=text]").prop('disabled', true);
            $("input[type=text]").removeAttr('disabled');
            console.log("Enable date picker: Input");
            showDatePicker();

        }

           function showDatePicker() {
                    
			if (!IsPrefCalendarEthiopian()) {
                $("td>span:contains('Date')").each(function (index) {

                    var dateInputRow = $(this).parent().next();

                    var Control = dateInputRow.find("input[type=text][placeholder!='dd/mm/yyyy']");


                    $(Control).datepicker({ dateFormat: 'mm-dd-yy' });

                });
                return;
            }
			
            var parameterRow = $("#ParametersRowrvREXReport");
            var innerTable = $(parameterRow).find("table").find("table");
            var span = innerTable.find("span:contains('Date')");
			
			var containLabel=true;
			
			var dateContainers=$("label>span:contains('Date')");
            if(dateContainers.length==0)
               { 
			   dateContainers=$("td>span:contains('Date')");
			   containLabel=false;
			   }

            if (dateContainers.length > 0)
                {
            $(dateContainers).each(function (index) {
			
			var dateInputRow;
		
				if(containLabel)
               {  dateInputRow = $(this).parent().parent().next();}
			   else
			   { dateInputRow = $(this).parent().next();
			   }
				
                var gcControl = dateInputRow.find("input[type=text][placeholder!='dd/mm/yyyy']");
				
                if (gcControl == NaN)//this is ethiopian cal control skip
                    return;//same as continue
                 //CREATE ETH CONTROL, MAKE INVISIBLE GC
                CreateEthiopianCalField(gcControl);
                //CONVERT GC DATE AND ASSIGN TO THE VISIBLE ETH CONTROL
                AssignEthDate(gcControl);
                //CHECK IF CALENDAR PICTRUE EXISTS OR NOT(NOT ON SOME VERSIONS OFCHROME)
                HideCalendarImage(dateInputRow);
                
                
            });
			
			}
        }

        function HideCalendarImage(container, gcControl)
        {
            var calIcon = container.find("input[type='image']");

            
            if(calIcon)
            {

                calIcon.css("display", "none");
            }


        }


        function IsPrefCalendarEthiopian()
        {
            var calendar = "<%= GetCalendarPreference() %>";

            if (calendar == "EC") return true;
            else return false;

        }

        function CreateEthiopianCalField(gcCalendarField)
        {

            if (IsPrefCalendarEthiopian()) {
                $(gcCalendarField).ethcal_datepicker();
            }
            else {
                if (typeof Metronic === 'undefined') { //TODO: check what the heck this is
                    $(gcCalendarField).datepicker();
                } else {
                    $(gcCalendarField).datepicker({
                        rtl: Metronic.isRTL(),
                        orientation: "left",
                        autoclose: true
                    });
                }
            }
        }
        function AssignEthDate(gcControl)
        {
            //get ec control
            var ecControl = FindECControl(gcControl);

            var gcDate = gcControl.attr("value");

            if (gcDate) gcDate = new Date(Date.parse(gcDate));
            else {
                ecControl.attr("Value", "");
                return
            }   
            
            //Convert to Ethiopian Date
            var ecDate = new EthDate().fromGregStr(gcDate.toLocaleDateString());

            ecControl.attr("Value", ecDate);


        }
        function FindECControl(gcControl)
        {
            var ecControl = gcControl.next();
            return ecControl;
        }

        //EVENT LISTENER TO ADJUST THE LOCATION OF THE DATE PICKER
        $(document).on("aboutToOpenPicker", function (event) {

            var pickerControl = event.PickerControl;

            var inputField = pickerControl.data("input");

            AdjustEthCalLocation(event.InputControl, pickerControl);


        });

        //ADJUST THE LOCATION OF THE PICKER RELATIVE TO THE INPUTFIELD

        function AdjustEthCalLocation(inputField,pickerControl)
        {

            var offsetTop = 0;
            var offsetleft = 0;
            try {
                var bodyRect = document.body.getBoundingClientRect(),
                 elemRect = inputField.getBoundingClientRect(),
                 offsetTop = elemRect.top,
                 offsetleft = elemRect.left - bodyRect.left;

                offsetTop = offsetTop + elemRect.height;
            } catch (err) { }

            pickerControl.css({ top: offsetTop, left: offsetleft, position: 'fixed', 'z-index': '1' });

        }


        //Function that is called on Successful AJAX method call.  These are referenced in the "CallServerMethodBeforePrint" function that is created from code behind and will exist in the final rendering of the page.
        function ServerCallSucceeded(result, context) {
            var iFrameURL = "<%=iFrameURL%>";
            window.frames['frmPrint'].document.location.href = iFrameURL;
            window.frames['frmPrint'].focus();
            var timeout = window.setTimeout("window.frames[\"frmPrint\"].focus();window.frames[\"frmPrint\"].print();", 500);
            window.setTimeout("ServerCallAfterPrint(this)", 2000);
        }

        function ServerCallSucceededAfterPrint(result, context) {
        }

        //Function that is called on failure or error in AJAX method call. These are referenced in the "CallServerMethodBeforePrint" function that is created from code behind and will exist in the final rendering of the page.
        function ServerCallFailed(result, context) {
        }

        function ServerCallBeforePrint(btn) {
            $('#spinner').show();
            var context = new Object();
            //example of passing multiple args
            context.flag = new Array('Today', 'Tomorrow');
            //This "CallServerMethodBeforePrint" function is created from code behind and will exist in the final rendering of the page
            CallServerMethodBeforePrint(context.flag, context);
        }

        function ServerCallAfterPrint(btn) {
            var context = new Object();
            //example of passing multiple args
            context.flag = new Array('Today', 'Tomorrow');
            //This "CallServerAfterPrint" function is created from code behind and will exist in the final rendering of the page
            CallServerAfterPrint(context.flag, context);
            $('#spinner').hide();
        }

        function printPDF(btn) {
            ServerCallBeforePrint(btn);
        }

        function showPrintButton() {

            var parameterRow = $("#ParametersRowrvREXReport");
            var innerTable = $(parameterRow).find("table").find("table");
            var input = innerTable.find("input[type=submit]");
            var innerRow = $(input).parent();

            var table = $("table[title='Refresh']");
            var parentTable = $(table).parents('table');
            var parentDiv = $(parentTable).parents('div').parents('div').first();
            var btnPrint = $("<input type='button' id='btnPrint' name='btnPrint' value='Print' style=\"font-family:Verdana;font-size:8pt;width:86px\"/>");
            var btnClose = $("<input type='button' id='btnClose' name='btnClose'value='Close' style=\"font-family:Verdana;font-size:8pt;width:86px\"/>");
            btnPrint.click(function() {
                printPDF(this);
            });
            btnClose.click(function() {
                window.close();
            });
            if (parentDiv.find("input[value='Print']").length == 0) {
                innerRow.append('<table cellpadding="0" cellspacing="0" toolbarspacer="true" style="display:inline-block;width:6px;"><tbody><tr><td></td></tr></tbody></table>');
                innerRow.append('<div id="customDiv" class=" " style="display:inline-block;font-family:Verdana;font-size:8pt;vertical-align:inherit;"><table cellpadding="0" cellspacing="0"><tbody><tr><td><span style="cursor:pointer;" class="HighlightDiv" onclick="javascript:printPDF(this);" ><img src="Content/images/printer.jpg" alt="Print Report" title="Print Report" width="18px" height="18px" style="margin-top:4px"/></span></td></tr></tbody></table></div>');
                innerRow.append('<table cellpadding="0" cellspacing="0" toolbarspacer="true" style="display:inline-block;width:10px;"><tbody><tr><td></td></tr></tbody></table>');
                innerRow.append('<div id="customDiv" class=" " style="display:inline-block;font-family:Verdana;font-size:8pt;vertical-align:inherit;"><table cellpadding="0" cellspacing="0" style="display:inline;"><tbody><tr><td><span style="cursor:pointer;" class="HighlightDiv" onclick="javascript:window.close();"><img src="Content/images/close.jpg" alt="Close Report" title="Close Report" width="18px" height="18px" style="margin-top:4px"/></span></td></tr></tbody></table></div>');
            }
        }
        function cfnReportsViewer_ViewReport(selectedTreeKeyGuidValue, ReportName, VenueExamCounter) {
            var windowWidth = 1000;
            var windowHeight = 800;
            var left = (screen.width / 2) - (windowWidth / 2);
            var top = (screen.height / 2) - (windowHeight / 2);
            var myForm = document.getElementById("frmReportViewer");
            if (myForm) {
                myForm.target = "PopupReport";
            }
            $("#hfAccessObjectGuid").val(selectedTreeKeyGuidValue);
            $("#hfReportName").val(ReportName);
            $("#hfVenueExamCounter").val(VenueExamCounter);
            var thePopup = window.open("about:blank", "PopupReport", 'scrollbars=yes,status=yes,toolbar=yes,menubar=no,location=no,resizable=no,fullscreen=yes, width=' + windowWidth + ', height=' + windowHeight + ', top=' + top + ', left=' + left);
            window.setTimeout(document.getElementById("frmReportViewer").submit(), 500);
            return false;
        }
        function get_browser_info(){
            var ua=navigator.userAgent,tem,M=ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || []; 
            if(/trident/i.test(M[1])){
                tem=/\brv[ :]+(\d+)/g.exec(ua) || []; 
                return {name:'IE ',version:(tem[1]||'')};
            }   
            if(M[1]==='Chrome'){
                tem=ua.match(/\bOPR\/(\d+)/)
                if(tem!=null)   {return {name:'Opera', version:tem[1]};}
            }   
            M=M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
            if((tem=ua.match(/version\/(\d+)/i))!=null) {M.splice(1,1,tem[1]);}
            return {
                name: M[0],
                version: M[1]
            };
        }
    </script>

</body>

</html>
