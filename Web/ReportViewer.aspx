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

            

            $(function() {
                showDatePicker();
                showPrintButton();
                $("select").on("focusout", function () {
                    showDatePicker();
                    console.log("Show date picker: Select");
                    //showPrintButton();
                    //alert();
                });
                $("input[type=checkbox]").on("focusout", function () {
                    //setTimeout(showDatePicker, 1000);
                    showDatePicker();
                    console.log("Show date picker: Input");
                    //showPrintButton();
                });

                var repeater;

                function doWork() {
                    $('#more').load('exp1.php');
                    repeater = setTimeout(doWork, 1000);
                }

                doWork();
            });
        });

        var messageElem = 'AlertMessage';
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                var errorMessage = args.get_error().message;
                args.set_errorHandled(true);
                $get(messageElem).innerHTML = errorMessage;
            }
            showDatePicker();
            console.log("Show date picker: Input");
        }

        function showDatePicker() {
            
            var parameterRow = $("#ParametersRowrvREXReport");
            var innerTable = $(parameterRow).find("table").find("table");
            var span = innerTable.find("span:contains('Date')");

            $("span:contains('Date')").each(function (index) {
                var dateInputRow = $(this).parent().next();
                var dateInput = dateInputRow.find("input[type=text]");
                var calendar = "<%= GetCalendarPreference() %>";
                var date = new Date();
                
                if (calendar == "EC") {
                    if (typeof Metronic === 'undefined') {
                        $(dateInput).ethcal_datepicker();
                    } else {
                        $(dateInput).ethcal_datepicker({
                            rtl: Metronic.isRTL(),
                            orientation: "left",
                            autoclose: true
                        });
                    }
                }
                else {
                    if (typeof Metronic === 'undefined') {
                        $(dateInput).datepicker();
                    } else {
                        $(dateInput).datepicker({
                            rtl: Metronic.isRTL(),
                            orientation: "left",
                            autoclose: true
                        });
                    }
                }
                $(dateInput).each(function () {
                    if ($(this).val()) {
                        date = new Date(Date.parse($(this).val()));
                    }
                    $(this).val(date.toLocaleDateString());
                    $(this).change(function () {
                        var date2 = new Date(Date.parse($(this).val()));
                        //alert(date2.toLocaleDateString() +" * " + $(this).val());
                        if (date2.toLocaleDateString() != $(this).val()) {
                            var today = new Date();
                            //$(this).val(today.toLocaleDateString());
                            //$(this).tooltip('show');
                        }
                        console.log("Date Changed", date2);
                    });
                }).tooltip({ trigger: "hover manual", title: "mm/dd/yyyy" }).attr("placeholder", "mm/dd/yyyy");
                
            });

            if (span) {
                <%--var innerRow = $(span).parent().next();
                var innerCell = innerRow.find("td").eq(1);
                var textFrom = innerCell.find("input[type=text]");
                innerCell = innerRow.find("td").eq(4);
                var textTo = innerCell.find("input[type=text]");

                var calendar = "<%= GetCalendarPreference() %>";
                var date = new Date();

                $(textFrom).each(function () {
                    if ($(this).val()) {
                        date = new Date(Date.parse($(this).val()));
                    }
                    $(this).val(date.toLocaleDateString());
                    $(this).change(function () {
                        var date2 = new Date(Date.parse($(this).val()));
                        if (date2.toLocaleDateString() != $(this).val()) {
                            var today = new Date();
                            $(this).val(today.toLocaleDateString());
                            $(this).tooltip('show');
                        }
                        console.log("Date Changed", date2);
                    });
                }).tooltip({ trigger: "hover manual", title: "mm/dd/yyyy" }).attr("placeholder", "mm/dd/yyyy");
                if (calendar == "EC") {
                    $(textFrom).ethcal_datepicker();
                }
                else {
                    if (typeof Metronic === 'undefined') {
                        $(textFrom).datepicker();
                    } else {
                        $(textFrom).datepicker({
                            rtl: Metronic.isRTL(),
                            orientation: "left",
                            autoclose: true
                        });
                    }
                }
                $(textTo).each(function () {
                    if ($(this).val()) {
                        date = new Date(Date.parse($(this).val()));
                    }
                    $(this).val(date.toLocaleDateString());
                    $(this).change(function () {
                        var date2 = new Date(Date.parse($(this).val()));
                        if (date2.toLocaleDateString() != $(this).val()) {
                            var today = new Date();
                            $(this).val(today.toLocaleDateString());
                            $(this).tooltip('show');
                        }
                        console.log("Date Changed", date2);
                    });
                }).tooltip({ trigger: "hover manual", title: "mm/dd/yyyy" }).attr("placeholder", "mm/dd/yyyy");
                if (calendar == "EC") {
                    $(textTo).ethcal_datepicker();
                }
                else {
                    if (typeof Metronic === 'undefined') {
                        $(textTo).datepicker();
                    } else {
                        $(textTo).datepicker({
                            rtl: Metronic.isRTL(),
                            orientation: "left",
                            autoclose: true
                        });
                    }
                }--%>
                //$(textFrom).datepicker({
                //    defaultDate: "+1w",
                //    dateFormat: 'dd/mm/yy',
                //    changeMonth: true,
                //    numberOfMonths: 1,
                //    onClose: function(selectedDate) {
                //        $(textTo).datepicker("option", "minDate", selectedDate);
                //    }
                //});
                //$(textFrom).focus(function(e) {
                //    e.preventDefault();
                //    $(textFrom).datepicker("show");
                //});
                //$(textTo).datepicker({
                //    defaultDate: "+1w",
                //    dateFormat: 'dd/mm/yy',
                //    changeMonth: true,
                //    numberOfMonths: 1,
                //    onClose: function(selectedDate) {
                //        $(textFrom).datepicker("option", "maxDate", selectedDate);
                //    }
                //});
                //$(textTo).focus(function() {
                //    $(textTo).datepicker("show");
                //});
            }
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
    </script>

</body>

</html>
