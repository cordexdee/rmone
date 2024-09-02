<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardPreview.ascx.cs" Inherits="uGovernIT.Web.DashboardPreview" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">    

    var globalFilters = "";

    $(function () {


        $(".globalfilterdaterangeButton").bind("click", function (e) {
            $(this).parent().parent().find('select').val("");
            callGlobalFilter();
        });

        $(".globalfilterlistbox").bind("change", function (e) {

            if ($(this).parent().find(".to").length > 0) {
                $(this).parent().find(".to").val("");
                $(this).parent().find(".from").val("");
                setWaterMark($(this).parent().find(".to").get(0));
                setWaterMark($(this).parent().find(".from").get(0));
            }
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

        function setWaterMark(obj) {
            $(obj).addClass("removemark");
            if ($(obj).val().toLowerCase() == $(obj).attr("watermark").toLowerCase() || $.trim($(obj).val().toLowerCase()) == "") {
                $(obj).removeClass("removemark");
                $(obj).val($(obj).attr("watermark"));
            }
        }

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


        $("iframe").each(function () {
            var src = $(this).attr('isrc');
            $(this).attr('src', src);
        });

        $(".dashboard-panel").each(function (i, item) {
            if ($(item).attr('theme') == 'Transparent') {
                if ($(item).find(".messageboard-ul").length > 0) {
                    $(item).find(".messageboard-ul").css('background', 'transparent');
                }
                else if ($(item).find("#tdMain").length > 0) {
                    $(item).find("#tdMain").css('background', 'transparent');
                }
            }
        });


    });


    function callGlobalFilter() {

        var globalFiltersString = "";
        var filters = $(".globalfilterlistbox");
        var isFilterExist = false;
        var viewID = "0";
        filters.each(function (index, item) {
            viewID = $(item).attr("viewID");
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
                    $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "block");
                    if ($.trim(startDate.val()) != "" && startDate.val() != startDate.attr("watermark")) {
                        rangeValue += startDate.val();
                    }
                    rangeValue += "to";
                    if ($.trim(endDate.val()) != "" && endDate.val() != endDate.attr("watermark")) {
                        rangeValue += endDate.val();
                    }
                    globalFiltersString += $(item).attr("globalfilterid") + "~" + rangeValue + ";#";
                }
            }
            else {
                $(item).parents(".custompanelbox").find(".globalfilterbutton").css("display", "none");
            }
        });

        if (isFilterExist) {
            $(".removeallglobalfilterbutton").unbind();
            $(".removeallglobalfilterbutton").removeClass("faddedlink");
            $(".removeallglobalfilterbutton").bind("click", function (e) {
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
        panelDivs.each(function (index, item) {

            var panelKey = $(item).attr("panelInstanceID");
            var dashboard = ugitDashboardData(panelKey);
            dashboard.GlobalFilter = escape(globalFiltersString);

            if (panelKey && dashboard.Type == 'chart') {

                updateCharts(panelKey, 5);
            }
        });
    }

    function OpenLink(url, navigationType) {
        if (navigationType == 2) {
            javascript: window.parent.UgitOpenPopupDialog(unescape(url), '', '', '85', '90');
        }
        else if (navigationType == 1) {
            window.open(unescape(url), '_blank');
        }
        else {
            window.location.href = unescape(url);
        }
    }

    function loadDashboard(obj) {
        var borderStyle = ''
        $(document).ready(function () {
            if (obj.BorderStyle != undefined) {
                borderStyle = obj.BorderStyle;
            }
            configurDashboardUrls("<%=HttpContext.Current.Request.Url %>", "<%= filterTicketsPageUrl %>", "<%= dasbhoardViewPage %>");
            ugitDashboardData(obj.PanelID.toString(), { ViewID: obj.ViewID.toString(), PanelID: obj.PanelID.toString(), Sidebar: "false", Width: obj.Width, Height: obj.Height, viewType: "0", Type: obj.Type, LocalFilter: obj.LocalFilter, DimensionFilter: "", ExpressionFilter: "", GlobalFilter: "", BorderStyle: borderStyle, Description: obj.Description });

            if (obj.Type == "chart") {
                //ChartsPreview(obj);
                updateCharts(obj.PanelID.toString());
            }
            else {
                updateKPIs(obj.PanelID.toString());
                //kpisPreview(obj);
            }
        });
    }



</script>
<%--<script type="text/javascript" src="/_layouts/SP.UI.Dialog.js"></script>--%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    body #s4-titlerow {
        display: none;
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

    .dashboardpanels {
        float: left;
        width: 100%; /*padding-top:5px;*/
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

    .dashboard-panel-main, .dashboard-panel-main-mini, .dashboard-panel-main-notmove {
        float: left;
        width: auto;
    }

    .dashboard-panel-main-mini {
        padding-top: 0px;
        padding-right: 0px
    }

    .panel-content-header {
        position: relative;
        margin-right: 5px;
        font-size: 13px;
    }

    .dashboard-panel-main a, .dashboard-panel-main a:hover, dashboard-panel-main-mini a, dashboard-panel-main-mini a:hover, dashboard-panel-main-notmove a, dashboard-panel-main-notmove a:hover {
        text-decoration: none !important;
    }

    .panel-content {
        position: relative;
    }

    .panel-content-main {
        float: left;
        width: 100%;
    }

    .dashboard-desc {
        font-weight: normal;
        float: left;
        padding-left: 4px;
        font-size: 11px;
    }

    .dashboardkpi-main {
        margin-bottom: 5px;
        display: block;
    }

    .dashboardkpi-main-min {
        margin-bottom: 1px;
        display: block;
    }
    /*.dashboardkpi-txt{}*/
    /*.dashboardkpi-txt:hover{color:#000;}*/
    .dashboardkpi-td {
        padding: 0px 2px;
    }

    .dashboardkpi-a {
        font-size: 12px;
    }

    .dashboardkpi-a-min {
        font-size: 10px;
    }

    .dashboardaction-icon {
        float: right;
        padding-left: 3px;
        position: relative;
        right: -14px;
        top: 2px;
    }

    .fleft {
        float: left;
    }

    .drilldownback {
        width: 16px;
        height: 16px;
        float: left;
        padding-left: 2px;
        position: absolute;
        top: 3px;
        left: 3px;
        z-index: 100;
    }

    .chartbreadcrumb {
        padding: 0px;
        margin: 0px;
        float: left;
        border: none;
        font-size: 9px;
        text-align: center;
        position: absolute;
        z-index: 100;
        left: 3px;
        top: -3px;
    }

    .helptext-cont {
        padding-top: 0px !important;
    }

    .labeltooltip {
        font-weight: bold;
    }
    .dashboardpanelcontainer .associate-Card .dx-tileview-wrapper{
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(215px, 0.1fr));
        grid-auto-flow: dense;
        padding-top: 15px;
        justify-items:center;
    }
    .dashboardpanelcontainer .associate-Card .dx-scrollview-content::after, .dashboardpanelcontainer .associate-Card .dx-scrollview-content::before {
        display: none;
        content: "";
        line-height: 0;
    }
    .dashboard-panel-main {
    border-radius: 0px !important;
    box-shadow: rgba(14, 30, 37, 0.12) 0px 2px 4px 0px, rgba(14, 30, 37, 0.32) 0px 2px 16px 0px;    }
    .dashboard-panel {
    border: none !important;
    padding: 15px;
    z-index: auto;
}
</style>

<%-- <SharePoint:SPSecurityTrimmedControl ID="MySetting" runat="server" Permissions="AddAndCustomizePages">
    <style type="text/css">
     .ms-cui-tts, .ms-cui-tts-scale-1, .ms-cui-tts-scale-2{display:block !important;}
     .ms-cui-TabRowLeft, .ms-cui-jewel-container{display:block !important;}
    </style>
 </SharePoint:SPSecurityTrimmedControl>--%>

<div id="divContainer" runat="server">
    <div style="float: left; position: relative;">
        <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
            width="100%">
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
                            <td id="dashboardPanelsContainer" runat="server" class="dashboardpanels" style="position: relative;">

                                <asp:Panel ID="dvDashBoard" runat="server">
                                    <asp:Panel ID="dvDashboardBG" runat="server">
                                    </asp:Panel>
                                </asp:Panel>

                                <div id="dvDashboards" runat="server">
                                </div>
                            </td>
                            <td id="globalFilterContainer" runat="server"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>

