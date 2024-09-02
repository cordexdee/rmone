<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowDashboardView.ascx.cs" Inherits="uGovernIT.Web.ShowDashboardView" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="~/CONTROLTEMPLATES/uGovernIT/CustomPanelBox.ascx" TagPrefix="uGovernIT" TagName="CustomPanel" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    body #s4-titlerow {
        display: none;
    }

    .disabledbutton {
        pointer-events: none;
        opacity: 0.4;
    }

    .marggin {
        margin-right: 4px !important;
    }
    body #s4-leftpanel {
        display: none;
    }

    body #MSO_ContentTable {
        margin-left: 0px !important;
    }

    .ms-rte-layoutszone-inner {
        padding: 0px;
        width: 100%;
        height: 100%;
    }

    .ms-wikicontent {
        padding-right: 0px !important;
    }

    .s4-ca {
        background: transparent !important;
    }

    /*.ms-cui-tts, .ms-cui-tts-scale-1, .ms-cui-tts-scale-2{display:none !important;}
    .ms-cui-TabRowLeft, .ms-cui-jewel-container{display:none !important;}*/

    .boxborder {
        border: 1px solid black;
    }

    .searchpanelprority {
        float: left;
        width: 100%;
    }

    .searchpanelpriority_sub {
        float: left;
        width: 100%;
    }

    .searchpanelowner {
        float: left;
    }

    .searchpanelowner_sub {
        float: left;
        width: 100%;
        width: 100%;
    }

    .searchpanelprp {
        float: left;
        width: 100%;
    }

    .searchpanelprp_sub {
        float: left;
        width: 100%;
        width: 100%;
    }

    .selected {
        background: Yellow;
    }

    .globalfilterlistbox {
        max-height: 100%;
        min-height: 100px;
        width: 99%;
        border: none;
        min-width: 140px;
    }

    .ownerslist {
        max-height: 100%;
        min-height: 100px;
        width: 99%;
        border: none;
    }

    .prpslist {
        max-height: 100%;
        min-height: 100px;
        width: 99%;
        border: none;
    }

    .mainchartscontainer {
        width: 100%;
        float: left;
    }

    .chartboxdiv {
        float: left;
        width: auto;
    }

    .filterheader {
        float: left;
        padding: 5px;
        font-size: 14px;
        font-weight: bold;
    }

    .filterheadertd {
        padding-right: 3px;
        padding-top: 5px;
        padding-bottom: 10px;
    }

    .filterlink {
        border: 1px solid !important;
        float: right;
        font-weight: bold;
        padding: 5px;
        text-decoration: none !important;
    }

    .dashboarpanels {
        display: flex;
        flex-wrap: wrap;
    }

    .cursor {
        cursor: pointer;
    }

    .faddedlink {
        opacity: 0.6;
        filter: alpha(opacity=60)
    }

    .watermark {
        color: Gray;
    }

    .removemark {
        color: Black !important;
    }

    .help-container {
        position: absolute;
        right: 10px;
    }
</style>

<%-- <SharePoint:SPSecurityTrimmedControl ID="MySetting" runat="server" Permissions="AddAndCustomizePages">
    <style type="text/css">
     .ms-cui-tts, .ms-cui-tts-scale-1, .ms-cui-tts-scale-2{display:block !important;}
     .ms-cui-TabRowLeft, .ms-cui-jewel-container{display:block !important;}
    </style>
 </SharePoint:SPSecurityTrimmedControl>--%>
<div id="exeDashboard" runat="server" class="col-md-12" style="background-color:white;border-radius:7px;padding:5px;margin:15px 0px 0px 9px;">
    <div class="col-md-4"></div>
    <div class="col-md-4" style="text-align:center;">
        <asp:Label runat="server" ID="lblExecDash" Style="font-size: 24px;font-weight: 600;font-family: 'Roboto', sans-serif !important;"></asp:Label>
    </div>
</div>
<div class="homeDashboard_chartSec_container">    
    <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse" width="100%">
        <tr id="helpTextRow" runat="server">
            <td class="paddingleft8 helptext-cont" align="right">
                <asp:Panel ID="helpTextContainer" runat="server" CssClass="help-container">                    
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="dashobardPanelsContainer" runat="server" class="dashboarpanels"></td>
                        <td id="globalFilterContainer" runat="server" class="clsglobalFilterContainer"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var globalFilters = "";
    var globalFiltersAndViewID = "";
    $(function () {
        $(".globalfilterdaterangeButton").bind("click", function (e) {
            $(this).parent().parent().find('select').val("");
            callGlobalFilter();
        });

        //$(".globalfilterlistbox").bind("change", function (e) {   
        //    debugger;
        //    if ($(this).parent().find(".to").length > 0) {
        //        $(this).parent().find(".to").val("");
        //        $(this).parent().find(".from").val("");
        //        setWaterMark($(this).parent().find(".to").get(0));
        //        setWaterMark($(this).parent().find(".from").get(0));
        //    }
        //    callGlobalFilter();
        //});
        //Commented Above click event for all dynamic listbox to avoid spearte call and '/applyglobalfilterbutton' this will help to create one call only
        $(".applyglobalfilterbutton").click(function (event) {
            //$("#_Box").children().prop('disabled', true);
            $(".custompanelbox").addClass("disabledbutton");
            event.preventDefault();
            callGlobalFilter();
        });

        $(".globalfilterbutton").bind("click", function (e) {

            $(this).parents(".custompanelbox").find(".globalfilterlistbox").val("");

            if ($(this).parents(".custompanelbox").find(".to").length > 0) {

                $(this).parents(".custompanelbox").find(".to").val("");
                $(this).parents(".custompanelbox").find(".from").val("");
                $.each($(this).parents(".custompanelbox").find(".to"), function () {
                    setWaterMark(this);
                });
                $.each($(this).parents(".custompanelbox").find(".from"), function () {
                    setWaterMark(this);
                });
            }

            callGlobalFilter();
        });



        $(".watermark").bind("blur", function (e) {
            setWaterMark(this);
        });

        $(".watermark").bind("focus", function (e) {
            $(this).addClass("removemark");
            if ($(this).val() == $(this).attr("watermark")) {
                $(this).val("");
                $(this).removeClass("removemark");
            }
        });

        //callGlobalFilter();
    });
    function setWaterMark(obj) {
        $(obj).addClass("removemark");
        if ($(obj).val().toLowerCase() == $(obj).attr("watermark").toLowerCase() || $.trim($(obj).val().toLowerCase()) == "") {
            $(obj).removeClass("removemark");
            $(obj).val($(obj).attr("watermark"));
        }
    }
    function callGlobalFilter() {

        var globalFiltersString = "";
        var filters = $(".globalfilterlistbox");
        var isFilterExist = false;
        var viewID = "0";
        filters.each(function (index, item) {
            viewID = $(item).attr("viewID");
            $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "none");
            if ($(item).val() != null && $(item).val() != "") {
                $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "block");
                globalFiltersString += $(item).attr("globalfilterid") + "~" + $(item).val() + ";#";

                isFilterExist = true;
            }
            else if ($(item).attr("dataType") == "DateTime") {
                var startDate = $(item).parent().find(".to");
                var endDate = $(item).parent().find(".from");
                var rangeValue = "";
                if (($.trim(startDate.val()) != "" && startDate.val() != startDate.attr("watermark")) || ($.trim(endDate.val()) != "" && endDate.val() != endDate.attr("watermark"))) {

                    if ($.trim(startDate.val()) != "" && startDate.val() != startDate.attr("watermark")) {
                        $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "block");
                        rangeValue += startDate.val();
                    }
                    rangeValue += "to";
                    if ($.trim(endDate.val()) != "" && endDate.val() != endDate.attr("watermark")) {
                        $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "block");
                        rangeValue += endDate.val();
                    }
                    globalFiltersString += $(item).attr("globalfilterid") + "~" + rangeValue + ";#";
                }
            }
            else {
                $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "none");
            }
        });

        //alert(globalFiltersString + "ViewID~" + viewID);
        globalFiltersAndViewID = globalFiltersString + "ViewID~" + viewID;
        //alert(globalFiltersAndViewID);

        if (isFilterExist) {
            $(".removeallglobalfilterbutton").unbind();
            $(".removeallglobalfilterbutton").removeClass("faddedlink");
            $(".removeallglobalfilterbutton").bind("click", function (e) {
                $(".custompanelbox").removeClass("disabledbutton");
                $(".globalfilterlistbox").val("");
                $(this).addClass("faddedlink");
                callGlobalFilter();
            });
        }
        else {
            $(".removeallglobalfilterbutton").addClass("faddedlink");
            $(".removeallglobalfilterbutton").unbind();
        }

        var cgFitlerID = "0";
       

        UpdateGlobalFilterData(viewID, globalFiltersString);
        var panelDivs = $(".dashboardpanelcontainer").filter("[panelInstanceID]");
        (function myLoop(i) {
            setTimeout(function () {
                var a = i - 1;
                var panelKey = $('#' + panelDivs[a].id).attr("panelInstanceID");
                var dashboard = ugitDashboardData(panelKey);
                if (dashboard) {
                    dashboard.GlobalFilter = escape(globalFiltersString);
                    if (panelKey && dashboard.Type == 'chart') {
                        //$.when(updateCharts(panelKey)).done(function (a) {
                        //});
                        setTimeout(function () { updateCharts(panelKey); }, 3000);
                    }
                    else if (panelKey && dashboard.Type == 'panel') {

                        updateKPIs(panelKey);
                    }
                }
                //updateCharts(panelKey);
                //console.log(i); //  your code here                
                if (--i) myLoop(i);   //  decrement i and call myLoop again if i > 0
                if (i == 0) {
                    $(".custompanelbox").removeClass("disabledbutton");
                }
            }, 3000)
        })(panelDivs.length);
       

        //panelDivs.each(function (index, item) {
        //    debugger;
        //    var panelKey = $(item).attr("panelInstanceID");
        //    var dashboard = ugitDashboardData(panelKey);
        //    if (dashboard) {
        //        dashboard.GlobalFilter = escape(globalFiltersString);
        //        if (panelKey && dashboard.Type == 'chart') {
        //            //$.when(updateCharts(panelKey)).done(function (a) {
        //            //});
        //            setTimeout(function () { updateCharts(panelKey); }, 10000);
        //        }
        //        else if (panelKey && dashboard.Type == 'panel') {

        //            updateKPIs(panelKey);
        //        }
        //    }
        //});
      


    }
    function openPanelLink(obj, href, viewType, panelTitle) {
        var extraParam = $(obj).attr("extraParam");

        if (extraParam != null && extraParam != undefined && extraParam != "") {
            href = href + extraParam;
        }
        if (viewType == "1") {
            window.parent.UgitOpenPopupDialog(href, "showalldetail=true&showFilterTabs=false", panelTitle, 90, 90, 0);
        }
        else if (viewType == "2") {
            window.open(href, '_blank');
        }
        else {
            window.location.href = href;
        }
    }
</script>

