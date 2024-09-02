<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomFilteredTickets.ascx.cs" Inherits="uGovernIT.Web.CustomFilteredTickets" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/Shared/CustomTicketRelationShip.ascx" TagPrefix="ugit" TagName="CustomTicketRelationShip" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<script type="text/javascript" src="https://vadikom.com/demos/poshytip/src/jquery.poshytip.js"></script>
<link rel="stylesheet" href="https://vadikom.com/demos/poshytip/src/tip-skyblue/tip-skyblue.css" type="text/css" />--%>


<% if (!defaultBanding)
    { %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .homeGrid_dataRow {
        background-color: #FFF;
        background: #fff;
    }
    .dxgvDataRowAlt_UGITNavyBlueDevEx {
        background-color: #F9F9F9 !important;
    }
    .homeGrid_dataRow td.dxgv {
        padding: 0px 10px !important;
        /*border-bottom: 1px solid #f6f7fb !important;*/
    }

    .dxgvSelectedRow_UGITNavyBlueDevEx {
        background-color: #ebebeb !important;
        color: black;
    }

    .dxgvEditFormDisplayRow_UGITNavyBlueDevEx td.dxgv,
    .dxgvDataRow_UGITNavyBlueDevEx td.dxgv,
    .dxgvDataRowAlt_UGITNavyBlueDevEx td.dxgv,
    .dxgvSelectedRow_UGITNavyBlueDevEx td.dxgv,
    .dxgvFocusedRow_UGITNavyBlueDevEx td.dxgv
    {
        border-top: 2px solid #d9dae0;
        font-size:13px;
        border-bottom: 0px solid #d9dae0;
    }
    .dxgvAdaptiveDetailRow_UGITNavyBlueDevEx td.dxgv {
        vertical-align:middle;
    }
   .aspxPopupHoldTooltipContent {
        line-height: 1.3;
    }

    .btnAddNew .dx-icon {
        filter: brightness(10);
    }

    .dxlpLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxcaLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx .dxlp-loadingImage, .dxeImage_UGITNavyBlueDevEx.dxe-loadingImage {
        background-image: url(/Content/Images/ajaxloader.gif);
        height: 32px;
        width: 32px;
    }


</style>
<% } %>

<% if (!ShowBandedrows)
    { %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .homeGrid_dataRow td.dxgv {
    padding: 0px 10px !important;
    background-color: #fff !important;
}
</style>
<% } %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxgvDataRowHover_UGITNavyBlueDevEx td.dxgv, .dxgvFocusedRow_UGITNavyBlueDevEx td.dxgv {
        color: #4a6ee2 !important;
    }
</style>
<asp:PlaceHolder runat="server">
    <%= UGITUtility.LoadStyleSheet("/Content/UGITCommonTheme.css") %>
    <%= UGITUtility.LoadStyleSheet("/Content/UGITNewTheme.css") %>
</asp:PlaceHolder>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function setClassValue() {
        $(".commonLayout tr").addClass("commonLayouttr");
        $(".commonLayout").parent().parent().addClass("commonLayout-parent");
        $(".cardticket").each(function () {
            $(this).closest(".cardouterbox").prepend("<div class='cardticket'>" + $(this).text() + "</div>");
            $(this).closest(".commonLayout-parent").hide();
        });
        $(".cardtitle").each(function () {
            $(this).closest(".cardouterbox").prepend("<div class='cardtitle'>" + $(this).text() + "</div>");
            $(this).closest(".commonLayout-parent").hide();
        });
        $(".cardouterbox").each(function () {
            var elem = $(this);
            var attr = elem.attr("totalAllocation");
            var attrClick = elem.attr("titleClick");
            var attrClass = elem.attr("allocationClass");
            var aatrTitleURL = elem.attr("TitleURL");
            if (typeof attr !== 'undefined' && attr !== false
                && typeof attrClass !== 'undefined' && attrClass !== false) {
                elem.prepend(`<div class="cardallocation ${attrClass}" onclick=\"${attrClick}\">${attr}</div>`);
                //elem.prepend("<div class='cardallocation " + attrClass + "' onclick=" + attrClick + ">" + attr + "</div>")
            }
            if (elem.hasClass("preconClass")) {
                elem.find(".cardtitle").css("background", "#52bed9");
            }
            if (elem.hasClass("constClass")) {
                elem.find(".cardtitle").css("background", "#005c9b");
            }
            if (elem.hasClass("constClass-1")) {
                elem.find(".cardtitle").css("background", "#351b82");
            }
            if (elem.hasClass("clousOutClass")) {
                elem.find(".cardtitle").css("background", "#d9d9d9");
                elem.find(".cardtitle").css("color", "black");
            }
            elem.find(".cardtitle").attr("onclick", aatrTitleURL);
        });
    }
    function ChangeColorForHold() {
        let holdStr = "On Hold";
        $('.dx-vam:contains("On Hold")', document.body).each(function () {
            $(this).html($(this).html().replace(
                new RegExp(holdStr, 'g'), '<span style=color:red !important>' + holdStr + '</span>'
            ));
        });
    }
    $(document).ready(function () {
        ChangeColorForHold();
        //debugger
        document.getElementById("<%=grid.ClientID%>").onSortChanged = function (e) {
            e.preventDefault();
        }
    });
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">        
    var moduleName = '<%=ModuleName%>';
    var reportUrl = '<%=reportUrl%>';
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var iframePopupId;
    var hidenext = false;
    var hideprevious = false;
    function callModulePaggeNextPrevious(navtype, frameid) {
        var length = GetVisibleRowsOnPage();
        var index = GetkeysindexOf();
        var startindex = length * GetPageIndex();
        var endindex = (length - 1) * (GetPageIndex() + 1);

        var ticketURLPath = '<%=TicketNavigationURL%>';
        if (index < length) {
            var ticketid = "";
            var nxtIndex = index;
            if (navtype == "next") {
                ticketid = GetKeys(index + 1);
                nxtIndex = eval(index + 1);
                if (nxtIndex == GetLengthOfKeys())
                    MoveToNextPage();
                else if (GetPageIndex() == 0 && nxtIndex == endindex)
                    MoveToNextPage();
                else if (nxtIndex >= endindex)
                    $.cookie("hidenext", true);
                else
                    $.cookie("hidenext", false);
                $.cookie("hideprevious", false);
            }
            else {
                if (index >= 0)
                    ticketid = GetKeys(index - 1);
                else {
                    ticketid = GetKeys(endindex);
                    index = endindex;
                }
                nxtIndex = eval(index - 1);
                if (index == 0 && startindex > 0 && GetPageIndex() > 0)
                    MoveToPrevPage();
                else if (nxtIndex <= 0 && startindex <= 0)
                    $.cookie("hideprevious", true);
                else
                    $.cookie("hideprevious", false);
                $.cookie("hidenext", false);
            }

            if (ticketid) {
                ticketURLPath = ticketURLPath + "?TicketId=" + ticketid + "&pageTitle=" + ticketid + "&source=" + "/pages/tsr";
                //gridClientInstance.UnselectAllRowsOnPage();
                //gridClientInstance.SelectRowsByKey(ticketid);
                UnselectAllRowsOnPage();
                SelectRowsByKey(ticketid);

                var callbackindex = eval(startindex + nxtIndex);
                var returnIframe = document.getElementById(frameid);
                iframePopupId = frameid.replace("iframe_", "");

                //gridClientInstance.GetRowValues(callbackindex, "Title;TicketId", getGridTitleValue);
                GetRowValues(callbackindex, "Title;TicketId", getGridTitleValue);

                returnIframe.src = ticketURLPath;
            }
        }
    }



    function getGridTitleValue(selectedValues) {

        if (selectedValues.length > 1) {
            var title = selectedValues[0];
            var ticketid = selectedValues[1];
            var titlestr = ticketid;
            if (title != null && title != undefined && title != '')
                titlestr = titlestr + ": " + title;

            $("#" + iframePopupId).dialog('option', 'title', titlestr);
        }
    }

    function onSearchKeyPress(s, e) {
        if (e.htmlEvent.keyCode == 13) {
            startLocalSearch(s);
        }
    }
    function adjustControlSize() {
        setTimeout(function () {
            gridClientInstance.AdjustControl();
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }






    try {
        var controlLength = customUGITGridConfig.length;
    }
    catch (ex) {
        customUGITGridConfig = {}
    }
    customUGITGridConfig["<%= grid.ClientID%>"] = { "showfilter": "<%= ShowClearFilter%>", "customMessageContainer": "<%= customMessageContainer.ClientID%>" };


    function reportItemMouseOver(obj) {
        $(obj).removeClass("ugitlinkbg");
        $(obj).addClass("ugitsellinkbg");
    }

    function reportItemMouseOut(obj) {
        $(obj).removeClass("ugitsellinkbg");
        $(obj).addClass("ugitlinkbg");
    }

    function setTooltip() {
        $.each($(".monitoricon"), function (i, item) {
            var desc = $.trim(unescape($(item).find(".info").html()).replace(/\+/g, " "));
            //$(item).poshytip({ className: 'tip-violet', alignX: 'right', alignY: 'center', offsetX: 20, bgImageFrameSize: 9, content: desc });
            $(item).poshytip({ className: 'tip-violet', bgImageFrameSize: 9, content: desc });
        });
    }

    function HideWinLossRPopup() {
        ASPxPopupControl1.Hide();
        // document.getElementById("dvWinLossReport").style.display = "none";
    }

    //ASPxPopupControl1.Hide();

    function GetWinLossRPopup(obj) {
        var path = "<%=WinAndLossReportURL%>";
        var dtWinLossRStart1 = dtWinLossRStart.GetDate();
        var dtWinLossREnd1 = dtWinLossREnd.GetDate();
        var startDate = dtWinLossRStart1 != null ? ((dtWinLossRStart1.getMonth() + 1) + '/' + dtWinLossRStart1.getDate() + '/' + dtWinLossRStart1.getFullYear()) : '';
        var endDate = dtWinLossREnd1 != null ? ((dtWinLossREnd1.getMonth() + 1) + '/' + dtWinLossREnd1.getDate() + '/' + dtWinLossREnd1.getFullYear()) : '';
        var params = "dtStart=" + startDate + "&dtEnd=" + endDate;
        //window.parent.UgitOpenPopupDialog(path, params, $('#spWinLossReport').html(), 90, 90, false, escape(window.location.href));
        window.parent.UgitOpenPopupDialog(path, params, ASPxPopupControl1.GetHeaderText(), 90, 90, false, escape(window.location.href));
        // document.getElementById("dvWinLossReport").style.display = "none";
        ASPxPopupControl1.Hide();
        return false;

    }

    function ShowtWinLossRPopup() {

        ASPxPopupControl1.Show();
        <%--var datecontrolStart = document.getElementById("<%=dtWinLossRStart.ClientID %>_dtWinLossRStartDate");
        var datecontrolEnd = document.getElementById("<%=dtWinLossREnd.ClientID %>_dtWinLossREndDate");
        if (datecontrolStart && Date.parse(datecontrolStart.value) != "NaN") {
            datecontrolStart.value = "";
        }
        if (datecontrolEnd && Date.parse(datecontrolEnd.value) != "NaN") {
            datecontrolEnd.value = "";
        }--%>

        //$("#dvWinLossReport").css({ 'top': 0 + 'px', 'display': 'block', '': ($(".imgReport").position().left - $("#dvWinLossReport").width()) + 'px' });
        return false;
    }


    var subTicketElement = null;
    function LoadSubTicket(control, TicketId) {
        if (subTicketElement && subTicketElement != control) {
            subTicketElement.setAttribute("Max", "true");
            subTicketElement.src = "/Content/Images/plus.gif";
        }
        var url = "/Layouts/uGovernIT/DelegateControl.aspx?";

        // hfTicketInfo.SetValue("");
        //ticketPanelControl.innerHTML = "";

        if (control.getAttribute("Max") == "false") {
            control.setAttribute("Max", "true");
            control.src = "/Content/images/plus.png";
        }
        else {
            control.setAttribute("Max", "false");
            control.src = "/Content/images/minus.png";
            subTicketElement = control;
            var controlPostion = $(control).position();

            var position = "absolute";
            var left = (controlPostion.left + 10) + "px";
            var top = (controlPostion.top + 15) + "px";
            var background = "#f0f0f0";
            if ("<%= ModuleName%>" == "CMDB") {
                window.parent.UgitOpenPopupDialog(url, "control=AssetRelatedWithAssets&TicketId=" + TicketId + "&moduleName=" + "<%= ModuleName%>", "Related Ticket", '70', '60', 0);
            }
            else {
                window.parent.UgitOpenPopupDialog(url, "control=CustomTicketRelationship&TicketId=" + TicketId + "&moduleName=" + "<%= ModuleName%>", "Related Ticket", '50', '50', 0);
            }

        }

    }

    function hideSubTicket() {
        //popupSubTicket.Hide();
       <%-- var ticketControl = document.getElementById("<%=hfTicketInfo.ClientID %>");
        ticketControl.value = "";
        var ticketPanelControl = $("#<%=subTicketPanel.ClientID %>");
        ticketPanelControl.html("");--%>
        var subticketaction = $(".subticketaction");
        subticketaction.attr("max", "true");
        subticketaction.attr("src", "/Content/images/plus.png");
    }

    function saveTabStageForPanel(val) {
        if (typeof gridClientInstance !== "undefined") {
            var headerFilter = gridClientInstance.GetHeaderFilterPopup();
            if (headerFilter != undefined) {
                headerFilter.SetContentUrl('');
            }
        }
        var oldVal = "";
        var tabType = $("#<%= tabType.ClientID%>");
        if (tabType.length > 0) {

            //alert(tabType.val());
            oldVal = tabType;
            tabType.val(val);

            //document.getElementById('ctl00_ctl00_MainContent_ContentPlaceHolderContainer_customFilter_ASPxCallbackPanel1_lblcustomfilterselectedvalue').innerHTML = val;
            //tabType.val(x);
        }

        if (oldVal != val && (val == "allclosedtickets" || oldVal == "allclosedtickets")) {
            __doPostBack();
            filterTicketLoading.Show();
        }
        else {
            ASPxCallbackPanel1.PerformCallback("");
            filterTicketLoading.Show();
        }

        //ASPxCallbackPanel1.PerformCallback("");
        //filterTicketLoading.Show();
    }

    function setTabValue(val) {
        document.getElementById("ctl00_ctl00_MainContent_ContentPlaceHolderContainer_customFilter_lblcustomfilterselectedvalue").textContent = val;
    }

    function setvalue(s, e) {
        var x = s.GetSelectedItem().value;
        document.getElementById("ctl00_ctl00_MainContent_ContentPlaceHolderContainer_customFilter_lblcustomfilterselectedvalue").textContent = x;
    }
    
    function saveTabStage(s, e) {
        var val = s.GetValue().toString();
        if (typeof gridClientInstance !== "undefined") {
            var headerFilter = gridClientInstance.GetHeaderFilterPopup();
            if (headerFilter != undefined) {
                headerFilter.SetContentUrl('');
            }
        }
        var oldVal = "";
        var tabType = $("#<%= tabType.ClientID%>");
        if (tabType.length > 0) {

            //alert(tabType.val());
            oldVal = tabType;
            tabType.val(val);

            //document.getElementById('ctl00_ctl00_MainContent_ContentPlaceHolderContainer_customFilter_ASPxCallbackPanel1_lblcustomfilterselectedvalue').innerHTML = val;
            //tabType.val(x);
        }

        if (oldVal != val && (val == "allclosedtickets" || oldVal == "allclosedtickets")) {
            __doPostBack();
            filterTicketLoading.Show();
        }
        else {
            ASPxCallbackPanel1.PerformCallback("");
            filterTicketLoading.Show();
        }

        //ASPxCallbackPanel1.PerformCallback("");
        //filterTicketLoading.Show();
    }

    var prevSelectedRow;
    var prevSelectedClass;
    function SaveTicketIdInHiddenVariable(obj) {
        var selectedTicketIdHidden = document.getElementById("<%= selectedTicketIdHidden.ClientID%>");
        if (prevSelectedRow != null) {
            prevSelectedRow.setAttribute("class", prevSelectedClass);
        }

        if (selectedTicketIdHidden != null) {
            selectedTicketIdHidden.value = obj.getAttribute("ticketId");
            prevSelectedClass = obj.getAttribute("class");
            obj.setAttribute("class", "ugitlight1darkest");
            prevSelectedRow = obj;
        }
    }

    function WatermarkFocus(txtElem, strWatermark) {
        var txtwatermark = document.getElementById("<%=txtWildCard.ClientID %>");
        if (txtElem.value == strWatermark) {
            txtElem.value = '';
            txtwatermark.className = "searchTextBox";
        }
    }

    function WatermarkBlur(txtElem, strWatermark) {
        var txtwatermark = document.getElementById("<%=txtWildCard.ClientID %>");
        if (txtElem.value == '') {
            txtElem.value = strWatermark;
            txtwatermark.className = "WaterMarkClass";
        }

    }


    function watermarkFromDateField() {
        var datecontroll = document.getElementById("<%=dtFrom.ClientID %>_dtFromDate");
        datecontroll.className = "GlobalWaterMarkClass";
        var strWatermark = "From date";
        datecontroll.value = "From date";
        datecontroll.setAttribute("onfocus", "WaterMarkOnFromDateControllFocus(this,'" + strWatermark + "')");
        datecontroll.setAttribute("onblur", "WatermarkOnFromDateBlur(this,'" + strWatermark + "')");
    }
    function ClearDates() {

        var datecontrolTo = document.getElementById("<%=dtTo.ClientID %>_dtToDate");
        var datecontrolFrom = document.getElementById("<%=dtFrom.ClientID %>_dtFromDate");
        if (datecontrolTo && Date.parse(datecontrolTo.value) != "NaN") {
            datecontrolTo.value = "";
        }
        if (datecontrolFrom && Date.parse(datecontrolFrom.value) != "NaN") {
            datecontrolFrom.value = "";
        }
    }
    function watermarkToDateField() {

        var datecontroll = document.getElementById("<%=dtTo.ClientID %>_dtToDate");
        datecontroll.className = "GlobalWaterMarkClass";
        var strWatermark = "To date";
        datecontroll.value = "To date";

        datecontroll.setAttribute("onfocus", "WaterMarkOnToDateControllFocus(this,'" + strWatermark + "')");
        datecontroll.setAttribute("onblur", "WatermarkOnToDateBlur(this,'" + strWatermark + "')");


    }
    function WaterMarkOnFromDateControllFocus(txtElem, strWatermark) {
        if (txtElem.value == strWatermark) {
            txtElem.value = '';
            txtElem.className = "globalSearchTextBox";
        }
    }

    function WaterMarkOnToDateControllFocus(txtElem, strWatermark) {
        if (txtElem.value == strWatermark) {
            txtElem.value = '';
            txtElem.className = "globalSearchTextBox";
        }
    }

    function WatermarkOnToDateBlur(txtElem, strWatermark) {
        if (txtElem.value == '') {
            txtElem.value = strWatermark;
            txtElem.className = "GlobalWaterMarkClass";
        }
    }

    function WatermarkOnFromDateBlur(txtElem, strWatermark) {
        if (txtElem.value == '') {
            txtElem.value = strWatermark;
            txtElem.className = "GlobalWaterMarkClass";
        }
    }

    function classOnFromDate() {
        var datecontrol = document.getElementById("<%=dtFrom.ClientID %>_dtFromDate");
        datecontrol.className = "globalSearchTextBox";
    }

<%--    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        
        var ticketid = params.split('=')[1];

         <%--set_cookie('UseManageStateCookies', 'true', null, "<%= SPContext.Current.Web.ServerRelativeUrl %>");
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }--%>


    function startLocalSearch(obj) {
        event.preventDefault();
        event.stopPropagation();
        var searchString = obj.GetText();
        obj.SetText(searchString.replace("'", "''"))
        ASPxCallbackPanel1.PerformCallback("");
        filterTicketLoading.Show();
        return false;
    }
    function clearFilterFromList() {
        //gridClientInstance.ClearFilter();
        if (typeof gridClientInstance !== "undefined")
            gridClientInstance.ClearFilter();
        else if (typeof cardClientInstance !== "undefined")
            cardClientInstance.ClearFilter();

        var ModuleName = moduleName;


        if ('<%=IsDashboard%>' == 'True') {
            $.cookie('Dashboard_Grid_Cookies', '', { path: "/" });
        }
        else if (ModuleName != '') {
            $.cookie(ModuleName + "_Grid_Cookies", '', { path: "/" });
            //_aspxDelCookie(ModuleName + "_Grid_Cookies", "")
        }
        else {
            $.cookie("Home_" + "<%=MTicketStatus.ToString()%>" + "_Grid_Cookies", '', { path: "/" });

        }

        if (window.location.href.indexOf('globalSearchString') > -1) {
            $.cookie('GlobalSearch_Grid_Cookies', '', { path: "/" });
        }


        if ($("[id$='hndClearFilter']").length > 1) {
            // Case when showing both ticket panel & waiting on me in home page
            // Below code is base on assumption that while displaying both tabs their display sequence must be 1) than waiting on me 2)  ticket panel.


            if ($("[id$='cMyTicketLinkspanel']").is(':hidden')) {
                $($("[id$='hndClearFilter']")[0]).val('__ClearFilter__');
            }
            else {
                $($("[id$='hndClearFilter']")[1]).val('__ClearFilter__');
            }
        }
        else {
            $('#<%=hndClearFilter.ClientID%>').val('__ClearFilter__');
        }

        ASPxCallbackPanel1.PerformCallback();
        //filterTicketLoading.Show();

        //__doPostBack();


    }

    function initializeDataAfterCallback(s, e) {
        setJquerytoolTip();
    }
    function setFilterMode(obj) {
        var filterMode = '<%=FilterMode%>';
        if (filterMode == 'True') {
            $('#<%=hndFilterStatus.ClientID%>').val('__HideFilterMode__');
            __doPostBack();
        }
        else {
            $('#<%=hndFilterStatus.ClientID%>').val('__ShowFilterMode__');
            __doPostBack();
        }

    }


    function showClearFilterValue() {
        var status = '<%=ShowClearFilter%>';
        if (status == 'True') {
            $('#<%=customMessageContainer.ClientID%>').show();
        }
        else {
            $('#<%=customMessageContainer.ClientID%>').hide();
        }
    }

    function showSummary() {

        //pcTSKSummary.Show();
        window.parent.parent.UgitOpenPopupDialog("<%= summaryUrl %>", "", "<%= summaryPopupTitle %>", 80, 100, 1, "");
    }

    function NewContactbyCompany() {
        //
        //filterTicketLoading.Show();
        $('#<%= hdnPopupControlName.ClientID%>').val("NewContactFromCompany");
        $('#<%= hdnPopupControl.ClientID%>').val("NewContactFromCompany");
        //ClientPopupControl.PerformCallback("NewContactFromCompany");
        ClientPopupControl.width = 1500;
        ClientPopupControl.height = 650;
        ClientPopupControl.SetHeaderText('Companies')
        ClientPopupControl.Show();
        ClientPopupControl.PerformCallback("NewContactFromCompany");
        //filterTicketLoading.hide();
        return false;
    }

    function triggerRefresh() {
        //Refresh cutom grid and side bar if refreshdataOnly
        refreshTabCounts(true);
    }
    function item_mousehover(obj, ticketID) {
        showTasksActions(obj);
        showHoldDetail(obj, ticketID);
    }
    function item_mouseout(obj, ticketID) {
        hideTasksActions(obj);
        aspxPopupHoldTooltip.Hide();
    }
    var holdDataList = {};
    function showHoldDetail(trObj, ticketID) {
        var holdObj = $(trObj).find(".progressbar-hold");
        if (holdObj.length > 0) {
            var holdData = holdDataList[ticketID.toString()];
            var moduleName = ticketID.split("-")[0];

            if (holdData) {
                $(".aspxPopupHoldTooltipContent").html(holdData.message);
            }
            else {
                $(".aspxPopupHoldTooltipContent").html("Loading..");
            }

            aspxPopupHoldTooltip.ShowAtElement(holdObj.get(0));
            var jsonData = { "name": moduleName, "id": ticketID };

            $.ajax({
                type: "POST",
                url: "<%=AccountControllerURL %>/GetTicketHoldReason",
                data: JSON.stringify(jsonData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    try {                        
                        var json = $.parseJSON(message);
                        var htmlData = [];
                        htmlData.push("<div><b>On Hold Till: </b><span>" + json.holdtilldate + "</span></div>");
                        htmlData.push("<div><b>Reason: </b><span>" + json.holdreason + "</span></div>");
                        if (json.holdcomment != "")
                            htmlData.push("<div><b>Comment: </b><span>" + json.holdcomment + "</span></div>");
                        holdDataList[ticketID.toString()] = { message: htmlData.join(""), json: json };
                        $(".aspxPopupHoldTooltipContent").html(htmlData.join(""));
                    } catch (ex) {
                        $(".aspxPopupHoldTooltipContent").html("No Data Available");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

    $(function () {
        setTooltip();
        startExportDataIfTriggered();
        clearInterval(refreshInterval);

        autoRefreshListFrequency = parseInt('<%=autoRefreshListFrequency%>');
        moduleName = '<%=ModuleName%>';
        if (autoRefreshListFrequency > 0 && moduleName != '') {
            setTimeout(function () { refreshTabCounts(true); }, (autoRefreshListFrequency * 60000));
        }

        setJquerytoolTip();
    });
    function setJquerytoolTip() {
        try {
            $(".jqtooltip").tooltip({
                hide: { duration: 4000, effect: "fadeOut", easing: "easeInExpo" },
                content: function () {
                    var title = $(this).attr("title");
                    if (title)
                        return title.replace(/\n/g, "<br/>");
                }
            });
            $(".jqtooltip span").tooltip({
                hide: { duration: 4000, easing: "easeInExpo" },
                content: function () {
                    var title = $(this).attr("title");
                    if (title)
                        return title.replace(/\n/g, "<br/>");
                }
            });
           
        }
        catch (ex) {
        }
    }
    var refreshInterval;
    function refreshTabCounts(forceGridRefresh) {
        //moduleName = '<%=ModuleName%>';
        //var jsonData = '{' + '"moduleName":"' + moduleName + '","moduleLastModifiedOn":"' + lastmodifiedon + '"}';
        //var pageURL = "<%=ajaxHelperPage %>/GetTicketCount";

        //below code to fix too many requests are made to server
        ASPxCallbackPanel1.PerformCallback("");
        // refreshInterval = setInterval(function () { ASPxCallbackPanel1.PerformCallback(""); }, (autoRefreshListFrequency * 60000))

        //$.ajax({
        //    type: "POST",
        //    contentType: "application/json",
        //    url: pageURL,
        //    data: jsonData,
        //    dataType: 'json',
        //    success: function (result) {
        //        try {
        //            if (result != null && result.d != "") {
        //                var data = JSON.parse(result.d);
        //                lastmodifiedon = data.lastModified;
        //                if (forceGridRefresh || data.wasUpdated == "True")
        //                    ASPxCallbackPanel1.PerformCallback("");
        //            }

        //            if (autoRefreshListFrequency > 0 && moduleName != '') {
        //                setTimeout(function () { refreshTabCounts(); }, (autoRefreshListFrequency * 60000));
        //            }
        //        }
        //        catch (ex) {


        //        }
        //    },
        //    error: function (xhr, ajaxOptions, thrownError) { //Add these parameters to display the required response

        //        if (autoRefreshListFrequency > 0 && moduleName != '') {
        //            setTimeout(function () { refreshTabCounts(); }, (autoRefreshListFrequency * 60000));
        //        }
        //    }
        //});

    }

    function grid_Init(s, e) {

        $.globalEval($("#filterticketlist").find("script").html());
        setFilterEnableSettings();

        if (customUGITGridConfig[s.name].showfilter.toLowerCase() == "true") {
            $('#' + customUGITGridConfig[s.name].customMessageContainer).show();
        }
        //else {
        //    $('#' + customUGITGridConfig[s.name].customMessageContainer).hide();
        //}

        //
        //console.log('Grid Init');
        <%if (EnableCustomizeColumns){ %>
        if (moduleName !== '') {
            if ($.cookie(moduleName + 'Columns') != null && $.cookie(moduleName + 'Columns') != undefined) {
                //columnMoved.Set(moduleName + 'Columns', $.cookie(moduleName + 'Columns'));
                var values = $.cookie(moduleName + 'Columns').split(';#');
                values = values.filter(function (i) { return i != ''; });
                if (values.length > 1)
                    gridClientInstance.ShowCustomizationWindow();
            }
        }
        <%}%>
    }

    function printPDF(obj) {

        var url = window.location.href + "&print=true";
        window.open(url);
    }

    $(document).ready(function () {
        $("#export-options").hide();
        $(".copyToclip_popup_container").parent().addClass("copyToclip_parent");
    });

    function showhideExportOptions(obj) {
        if ($(obj).hasClass("selected-export")) {
            $("#export-options").hide();
            $("#<%= exportAction.ClientID %>").removeClass("selected-export");
        }
        else {
            $("#export-options").show();
            $("#<%= exportAction.ClientID %>").addClass("selected-export");
        }
    }

    function downloadPDF(obj) {
        var exportUrl = $("#<%= exportURL.ClientID %>").val();
        exportUrl += "&initiateExport=true&exportType=pdf";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }
    function downloadImage(obj) {
        var exportUrl = $("#<%= exportURL.ClientID %>").val();
        exportUrl += "&initiateExport=true&exportType=image";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }
    function downloadExcel(obj) {
        var status = document.getElementById("ctl00_ctl00_MainContent_ContentPlaceHolderContainer_customFilter_lblcustomfilterselectedvalue").innerText;
        var exportUrl = $("#<%= exportURL.ClientID %>").val();
        exportUrl += "&initiateExport=true&exportType=excel&status=" + status;
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    //For Excel Import
    <%--function OpenImportExcel() {
       
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', "Module=CMDB", 'Import Assets', '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }--%>

    function startPrint(obj) {
        var exportUrl = $("#<%= exportURL.ClientID %>").val();
        exportUrl += "&initiateExport=true&exportType=print";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function startExportDataIfTriggered() {
        var exportEnable = $("#enableExport").val();
        var exportPath = $("#<%= exportURL.ClientID %>").val();

        if (exportEnable.toLowerCase() == "true") {

            $.each($("img"), function (i, item) {
                var src = $(item).attr("src");
                if (src.indexOf("sortup.gif") > 0 || src.indexOf("filter.gif") > 0 || src.indexOf("menudark.gif") > 0) {
                    $(item).parent().remove();
                }
            });

            $(".customfitler-message").remove();

            var type = '<%= Request["exportType"] %>';
            if (type.toLowerCase() == "print") {
                $(":hidden").remove();
                $("#MSO_ContentTable").css("margin", "5px");
                window.print();
            }
        }
    }

    function showReportPopup(dashboardObj) {
        var moduleName = "<%= ModuleName %>";
        var path = "<%= reportPath %>";
        if (dashboardObj != null) {

            var res = dashboardObj.split(";#");

            window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName + "&dashboardId=" + res[1], res[0], 90, 90, false, escape(window.location.href));
        }
        else
            window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Reports", 90, 90, false, escape(window.location.href));

        return false;
    }


    function getGanttChart(obj) {
        HideGanttPopup();
        var params = "";
        var ModuleName = moduleName;
        var GanttType = "1";  // Constant for project gantt chart.
        var openProject = $("#<%=chkOpenProjectOnly.ClientID%>").is(':checked');
        var GroupBy = $("#<%=ddlGroupBy.ClientID%>").val();

        var url = '<%=ganttReportUrl %>' + "&GanttType=" + GanttType + "&GroupBy=" + GroupBy + "&OpenProjectOnly=" + openProject + "&Module=" + ModuleName;
        var title = 'Gantt Chart';
        if (openProject == true) {
            title += ' - Open Projects';
        }
        else {
            title += ' - All Projects';
        }

        if (GroupBy > 0) {
            if (GroupBy == 1) {
                title += ' by Priority';
            }
            else if (GroupBy == 2) {
                title += ' by Project Type';
            }
            else if (GroupBy == 3) {
                title += ' by Project Initiative';
            }
        }

        window.parent.UgitOpenPopupDialog(url, params, title, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function ShowGanttPopup() {


        $("#<%=ddlGroupBy.ClientID%>").val(0);
        $("#<%=chkOpenProjectOnly.ClientID%>").attr('checked', 'checked');
        $("#ganntPopup").css({ 'top': 0 + 'px', 'display': 'block', 'left': ($(".imgReport").position().left - $("#ganntPopup").width()) + 'px' });
        return false;
    }

    function ShowProjectPopup() {
        var params = "";
        var moduleName = moduleName;
        var url = "<%= projectReportUrl %>" + "&Module=" + moduleName;
        window.parent.UgitOpenPopupDialog(url, params, 'Project Status Report', '100', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }


    function ShowProjectSummaryPopup() {
        var params = "";
        <%--var moduleName = "<%= ModuleName %>";--%>
        var reportType = "ProjectReport";
        var url = reportUrl + "?reportName=" + reportType + "&Module=" + moduleName;
        var title = '';
        if (moduleName != '') {
            if (moduleName == "NPR") {
               // url = "<%= NPRprojectReportSummaryUrl %>" + "&Module=" + moduleName;
                title = 'NPR Project Summary Report'
            }
            else {
                //url = "<%= projectReportSummaryUrl %>" + "&Module=" + moduleName;
                title = 'Project Summary Report';
            }
            window.parent.UgitOpenPopupDialog(url, params, title, '100', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        return false;
    }

    function HideGanttPopup() {
        document.getElementById("ganntPopup").style.display = "none";
    }

    function showhideReportMenu(obj) {
        if ($("#report-options").hasClass("selected-export")) {
            $("#report-options").hide(300);
            $("#report-options").removeClass("selected-export");
        }
        else {
            $("#report-options").show(300);
            $("#report-options").addClass("selected-export");
        }
    }

    function DisplayCalendarView() {

        var url = '<%=delegateControl%>' + "?control=calendarview";
        window.parent.UgitOpenPopupDialog(url, '', 'Calendar View', '70', '80', 0, '');
    }

    $(document).ready(function () {
        $("#<%=chkSelectAll.ClientID%>").click(function (event) {
            if (this.checked) {
                $("[id*=cblModules] input:checkbox").prop('checked', true);
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("[id*=cblModules] input:checkbox").prop('checked', false);
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');
            }
        });

        $('[id*=cblModules] input[type=checkbox]').change(function () {
            if ($('#<%=cblModules.ClientID%> :checkbox:checked').length > 1) {
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "block");
                $("#<%=chkSortByModule.ClientID%>").attr('checked', 'checked');
            }
            else {
                $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
                $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');

                $("#<%=chkSelectAll.ClientID%>").removeAttr('checked');
            }

            if ($('#<%=cblModules.ClientID%> :checkbox:checked').length != this.length) {
                $("#<%=chkSelectAll.ClientID%>").removeAttr('checked');
            }
        });

    });
    function ShowTicketReportsPopup() {
        var moduleName = "<%= ModuleName %>";
        $("#<%=cblModules.ClientID%> input:checkbox[value=" + moduleName + "]").prop('checked', true);

        $('#<%=rdbtnLstFilterCriteria.ClientID %> input[value=Open]').attr('checked', 'checked');
        $('#<%=rdSortCriteria.ClientID %> input[value=oldesttonewest]').attr('checked', 'checked');
        $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
        $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');
        var datecontrolTo = document.getElementById("<%=dtToTicketSummary.ClientID %>_dtToTicketSummaryDate");
        var datecontrolFrom = document.getElementById("<%=dtFromTicketSummary.ClientID %>_dtFromTicketSummaryDate");
        if (datecontrolTo && Date.parse(datecontrolTo.value) != "NaN") {
            datecontrolTo.value = "";
        }
        if (datecontrolFrom && Date.parse(datecontrolFrom.value) != "NaN") {
            datecontrolFrom.value = "";
        }
        $("#divTicketsReportsPopup").css({ 'top': 0 + 'px', 'display': 'block', 'left': ($(".imgReport").position().left - $("#divTicketsReportsPopup").width()) + 'px' });
        return false;
    }
    function GetTicketsReportPopup(obj) {
        if ($('#<%=cblModules.ClientID%> :checkbox:checked').length < 1) {
            alert("Please select a Module.");
            return false;
        }

        HideTicketsReportPopup();
        var params = "";

        var selectedModules = [];
        $("[id*=<%=cblModules.ClientID%>] input:checked").each(function () {
            selectedModules.push($(this).val());
        });

        var SelectedType = $('#<%=rdbtnLstFilterCriteria.ClientID %> input:checked').val();
        var SortType = $('#<%=rdSortCriteria.ClientID %> input:checked').val();
        var isModuleSort = $("#<%=chkSortByModule.ClientID%>").is(':checked');

        var datecontrolTo = document.getElementById("<%=dtToTicketSummary.ClientID %>_dtToTicketSummaryDate").value;
        var datecontrolFrom = document.getElementById("<%=dtFromTicketSummary.ClientID %>_dtFromTicketSummaryDate").value;

        var url = '<%=delegateControl%>' + "?control=openticketreportviewer&SelectedModule=" + selectedModules + "&SelectedType=" + SelectedType + "&SortType=" + SortType + "&IsModuleSort=" + isModuleSort + "&DateFrom=" + datecontrolFrom + "&DateTo=" + datecontrolTo;
        var popupHeader = SelectedType + " Tickets Summary";
        window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
    function ShowReportViewer(s, reportType, reportTitle) {
        var moduleName = "<%= ModuleName %>"; 
        if (moduleName == 'CPR' && reportType == 'TicketSummary')
            reportTitle = 'Project Summary';

        var popupHeader = reportTitle;
        var params = "";
        var prefix = "Viewer";
        var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + "&title=" + reportTitle + '&userId=<%= HttpContext.Current.CurrentUser().Id %>';

        window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function showSummary(s, reportType, reportTitle) {
        var moduleName = "<%= ModuleName %>"; 
        var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + "&Filter=Filter";
        window.parent.UgitOpenPopupDialog(url, "", reportTitle, '80', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function HideTicketsReportPopup() {
        document.getElementById("divTicketsReportsPopup").style.display = "none";
    }


    function ShowApplicationFilterPopUp(s, reportType, reportTitle) {
        var params = ""; 
        var moduleName = "<%= ModuleName %>";
        var url = "<%= reportUrl %>" + "?reportName=" + reportType + "&Module=" + moduleName + "&title=" + reportTitle + "&Filter=Filter";
        window.parent.UgitOpenPopupDialog(url, params, 'Application Report', '1050px', '740px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }


    //function DisplayTaskCalendarView(obj) {
    //    var url = '<%=delegateControl%>' + "?control=taskcalender";
    //    window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '70', '100', 0, '');
    //   return false;
    // }

    function customFilterGridViewCallBack(s, e) {
        initializeDefaults();
        if (typeof (s.cpShowClearFilter) != 'undefined') {
            if (s.cpShowClearFilter == true) {

                $('#' + s.cpClientID).show();
            }
            else {
                $('#' + s.cpClientID).hide();
            }
        }
        if (typeof (s.cpAssignToMe) != 'undefined') {
            if (s.cpAssignToMe.length > 0) {
                alert(s.cpAssignToMe);
                s.cpAssignToMe = '';
            }
        }
        //$("html, body").animate({ scrollTop: $(".customgridview").scrollTop() }, 1200);

        //
        //console.log('Grid End');
        if (moduleName !== '') {
            var cols = moduleName + ';#';
            for (var i = 0; i < gridClientInstance.columns.length; i++) {
                if (gridClientInstance.columns[i]["visible"] === false)
                    cols = cols + gridClientInstance.columns[i]["fieldName"] + ',';
            }
            columnMoved.Set(moduleName + 'Columns', cols);
            $.cookie(moduleName + 'Columns', cols, { path: "/" });
        }
    }

    function initializeDefaults() {
        hideSubTicket();
        setTooltip();
    }

    var OpenTaskCount = 0;
    function OnSelectionChanged(s, e) {
        //
        if (e.isSelected) {
            <%--var key = s.GetRowKey(e.visibleIndex);

            var params = "TemplateId=" + key;
            var popupTitle = 'New Ticket';

            openTicketDialog('<%=ticketURL %>', params, popupTitle, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");--%>

            var key = s.GetRowKey(e.visibleIndex);
            var params = "TemplateId=" + key;
            var moduleFrom = $('#<%= hndType.ClientID%>').val();
            if (moduleFrom == "LEM") {
                params = "LeadId=" + key;
            }
            else if (moduleFrom == "COM") {
                params = "CompanyId=" + key;
            }
            else if (moduleFrom == "OPM") {
                params = "OpportunityId=" + key;
            }
            var popupTitle = $('#<%= hndNewTicketTitle.ClientID%>').val();
            var url = '<%=ticketURL %>';
            var updatedUrl = s["cpUrl_" + key];
            if (updatedUrl && updatedUrl != '')
                url = updatedUrl;

            ClientPopupControl.Hide();
            if (moduleFrom == "LEM" || moduleFrom == "OPM") {
                GetOpenTasksCount(moduleFrom, key);
                if (OpenTaskCount > 0) {
                    var customDialog = DevExpress.ui.dialog.custom({
                        title: "Open Tasks Alert",
                        message: "You have some pending Tasks. Please select one of the options below.",
                        buttons: [
                            { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                            { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                            { text: "Cancel", onClick: function () { return "Cancel" } }
                        ]
                    });
                    customDialog.show().done(function (dialogResult) {

                        if (dialogResult == "CloseTasks") {
                            params = params + "&CompleteTasks=true";
                        }
                        else if (dialogResult == "KeepTasksOpen") {
                            params = params + "&CompleteTasks=false";
                        }
                        else
                            return false;

                        openTicketDialog(url, params, popupTitle, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
                    });
                }
                else {
                    params = params + "&CompleteTasks=false";
                    openTicketDialog(url, params, popupTitle, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
                }

            }
            else {
                openTicketDialog(url, params, popupTitle, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            }
        }
    }

    function GetOpenTasksCount(moduleFrom, key) {
        var jsonData = { "ModuleNameLookUp": moduleFrom, "ID": key };
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperPage %>/GetOpenTasksCountById",
            data: JSON.stringify(jsonData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {

                try {
                    OpenTaskCount = $.parseJSON(message.d);
                } catch (ex) {
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function GetOpenTasksCount(obj) {
        var jsonData = { "ticketID": obj };
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperPage %>/GetOpenTasksCount",
            data: JSON.stringify(jsonData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {

                try {
                    OpenTaskCount = $.parseJSON(message.d);
                } catch (ex) {
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        ClientPopupControl.Hide();
        var ticketid = params.split('=')[1];
        openTicketDialogCommand.Set("openTicketDialogCommand", true);

        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }



    function OnEndTicketTypePopupCallback(s, e) {
        pcChangeTicketType.Hide();
        if (typeof (s.cpTicketUrl) != 'undefined') {
            ticketUrl = s.cpTicketUrl;
        }

        // params = "TicketId=0&SourceTicketId=" + ticketTypeChangeTicketId + "&ParentId=" + ticketTypeChangeTicketId + "&NewModuleId=" + $("#<%=ddlModuleListItems.ClientID %> option:selected").val() 
        params = "TicketId=0&OldTicketId=" + ticketTypeChangeTicketId + "&NewModuleId=" + $("#<%=ddlModuleListItems.ClientID %> option:selected").val()
        window.parent.parent.UgitOpenPopupDialog(ticketUrl, params, "Change Item Type and Archive Original Item", '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function OpneChangeTicketTypeDialog() {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetRowSelectedFieldValues);
        GetSelectedFieldValues('TicketId', OnGetRowSelectedFieldValues);
    }

    var ticketTypeChangeTicketId;
    function OnGetRowSelectedFieldValues(selectedValues) {
        if (selectedValues.length != 1) {
            alert("Please select a single item."); //BTS-22-000880
            return;
        }
        ticketTypeChangeTicketId = selectedValues[0];
        pcChangeTicketType.PerformCallback($("#<%=ddlModuleListItems.ClientID %> option:selected").val());
        //pcChangeTicketType.PerformCallback(($("#<%=ddlModuleListItems.ClientID %> option:selected").Text()),selectedValues[0]);
        return false;
    }

    function OnbtnTicketTypeChangeClick(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValues);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValues);
    }

    function OnGetSelectedFieldValues(selectedValues) {
        if (selectedValues.length != 1) {
            alert("Please select a single item.");
            return;
        }
        pcChangeTicketType.Show();
        return false;
    }

    function OnbtnAllowBatchEditClick(s, e) {

        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForBatchEdit);
        //OnGetSelectedFieldValuesForBatchEdit();
    }

    function OnGetSelectedFieldValuesForBatchEdit(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }

        params = "TicketId=" + selectedValues[0] + "&BatchEditing=true" + "&AllTickets=" + selectedValues;
        window.parent.parent.UgitOpenPopupDialog('<%=TicketURlBatchEditing%>', params, "Batch Editing", '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        return false;
    }

    function OnbtnBatchCreateClick(s, e) {
        params = "moduleName=" + '<%=ModuleName%>';
        window.parent.parent.UgitOpenPopupDialog('<%=batchCreateURL%>', params, "Batch Create", '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function OnbtnProjectSimilarityClick(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForProjectSimilarity);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForProjectSimilarity);
    }

    function OnGetSelectedFieldValuesForProjectSimilarity(selectedValues) {
        if (selectedValues.length < 2) {
            alert("Please select at least two items.");
            return;
        }
        var param = "ids=" + selectedValues;
        window.parent.UgitOpenPopupDialog('<%= ProjectSimilarityUrl %>', param, 'Project Comparison', '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }
    function OnbtnProjectPlanClick(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForProjectPlan);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForProjectPlan);
    }
    function OnGetSelectedFieldValuesForProjectPlan(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var param = "ids=" + selectedValues;
        window.parent.UgitOpenPopupDialog('<%= ProjectPlanUrl %>', param, 'Project Plan Report', '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }


    function OnbtnBatchReassignClick(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForBatchReassign);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForBatchReassign);
    }
    function OnBatchReassignToMe(s, e) {
        if (typeof gridClientInstance !== "undefined")
            gridClientInstance.PerformCallback("AssignToMe");
    }
    function OnbtnBatchEscalationClick(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForManualEscalation);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForManualEscalation);
    }

    function OnGetSelectedFieldValuesForBatchReassign(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }

        var param = "ids=" + selectedValues;
        window.parent.UgitOpenPopupDialog('<%= TicketReAssignmentUrl %>', param, 'Item (Re)assignment', '550px', '440px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }

    function OnGetSelectedFieldValuesForManualEscalation(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var param = "ids=" + selectedValues + "&ModuleName=" + '<%=ModuleName%>';
        window.parent.UgitOpenPopupDialog('<%= TicketManualEscalationUrl %>', param, 'Send Email', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }

    function OnbtnBatchCloseOrReject(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForCloseOrReject);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForCloseOrReject);
    }

    function OnGetSelectedFieldValuesForCloseOrReject(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var param = "ids=" + selectedValues;
        window.parent.UgitOpenPopupDialog('<%= TicketCloseOrRejectUrl %>', param, 'Quick Close', '550px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OnbtnBatchComments(s, e) {
        //gridClientInstance.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForComment);
        GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForComment);
    }
    function OnbtnReOpenClosedTicket(s, e) {
        //gridClientInstance.GetSelectedFieldValues("TicketId;Title", OnGetSelectedFieldValuesForReOpenCloseTicket);
        GetSelectedFieldValues('TicketId;Title', OnGetSelectedFieldValuesForReOpenCloseTicket);
    }
    //Re-open close ticket handel for custom filter Action button ::start
    function OnGetSelectedFieldValuesForReOpenCloseTicket(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }

        var title = "Re-Open";
        var ids = new Array();
        if (selectedValues.length == 1) {
            title = selectedValues[0][0] + ": " + selectedValues[0][1];
            ids.push(selectedValues[0][0]);

            if (title.length > 45)
                title = title.substring(0, 50) + "...";
        }
        else {
            for (var i = 0; i < selectedValues.length; i++) {
                ids.push(selectedValues[i][0]);
            }
        }


        var param = "ids=" + ids.join(",");
        window.parent.UgitOpenPopupDialog('<%= TicketReOpenUrl %>', param, title, '550px', '250px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
    //Re-open close ticket for custom filter Action button ::end


    //Re-open close ticket handel for home custom filter  ::start
    function ReopenticketPopupCall(s, ticketId, title) {
        var selectedTicketid = ticketId;
        var param = "ids=" + selectedTicketid;
        window.parent.UgitOpenPopupDialog('<%= TicketReOpenUrl %>', param, selectedTicketid + ': ' + title, '550px', '250px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showTasksActions(trObj) {

        $(trObj).find(".action-container").show();
    }

    function hideTasksActions(trObj) {
        $(trObj).find(".action-container").hide();
    }

    //Re-open close ticket handel for home custom filter  ::end

    function OnGetSelectedFieldValuesForComment(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }

        var param = "ids=" + selectedValues + "&comment=true";
        window.parent.UgitOpenPopupDialog('<%= TicketCloseOrRejectUrl %>', param, 'Comment', '550px', '250px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function ShowProjectSummaryPopup() {
        var params = ""; 
        var moduleName = "<%= ModuleName %>";
        var reportType = "ProjectReport";
        var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName;
        var title = '';
        if (moduleName != '') {
            if (moduleName == "NPR") {
                title = 'NPR Project Summary Report'
            }
            else {
                title = 'Project Summary Report';
            }
            window.parent.UgitOpenPopupDialog(url, params, title, '100', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        return false;
    }

    function GetSelectedProjects(values) {
        
        var params = "";
        if (values.length > 0)
            params = "alltickets=" + values;
        var moduleName = "<%= ModuleName %>";
        var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName;
        var title = '';
        switch (reportType) {

            case "ProjectReport":
                if (moduleName != '') {
                    if (moduleName == "NPR") {
                        title = 'NPR Project Summary Report'
                    }
                    else {
                        title = 'Project Summary Report';
                    }

                    window.parent.UgitOpenPopupDialog(url, params, title, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                }
                break;
            case "ProjectStatusReport":
                var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= HttpContext.Current.CurrentUser().Id %>' + "&" + params;
                var popupHeader = "Project Status Report";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            case "OnePagerReport":
                var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= HttpContext.Current.CurrentUser().Id %>' + "&" + params;
                var popupHeader = "1-Pager Project Report";
                window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                break;
            default:
                break;
        }

        return false;
    }

    var reportType = '';
    function popupMenuReportItemClick(s, e) {
        if (e.item.name === "ProjectSummary") {
            reportType = 'ProjectReport';
            //gridClientInstance.GetSelectedFieldValues('TicketId', GetSelectedProjects);
            GetSelectedFieldValues('TicketId', GetSelectedProjects);
        }
        else if (e.item.name === "TaskReport")
            ShowReportViewer(s, "TSKProjectReport", "Project Task Report");
        // ShowProjectPopup();
        else if (e.item.name == "ProjectReport") {
            reportType = 'ProjectStatusReport';
            //gridClientInstance.GetSelectedFieldValues('TicketId', GetSelectedProjects);
            GetSelectedFieldValues('TicketId', GetSelectedProjects);
        }
        else if (e.item.name == "TaskSummary")
            showSummary(s, "TaskSummary", "Task Summary Report");
        else if (e.item.name == "GanttView")
            //ShowGanttPopup();
            ShowReportViewer(s, "GanttView", "Gantt View");
        else if (e.item.name === "CalendarView")
            DisplayCalendarView();
        else if (e.item.name === "ApplicationReport")
            ShowApplicationFilterPopUp(s, "ApplicationReport", "Application Access Report");
        else if (e.item.name === "TicketSummary")
            ShowReportViewer(s, "TicketSummary", "Item Summary");
        else if (e.item.name === "QueryReport")
            showReportPopup(null);
        else if (e.item.name === "BottleNeckChart")
            showBottleNeckChartPopup();
        else if (e.item.name === "GroupScoreCard")
            showGroupScoreCard();
        else if (e.item.name == "TicketSummaryByPRP")
            ShowReportViewer(s, "SummaryByTechnician", "Item Summary By Technician");
        else if (e.item.name == "HelpDeskPerformance")
            ShowReportViewer(s, "NonPeakHoursPerformance", "Non-Peak Hours Performance");
        else if (e.item.name === "WeeklyTeamPerformance")
            ShowReportViewer(s, "WeeklyTeamReport", "Weekly Team Performance");
        else if (e.item.name === "ProCompactReport") {
            reportType = 'OnePagerReport';
            //gridClientInstance.GetSelectedFieldValues('TicketId', GetSelectedProjects);
            GetSelectedFieldValues('TicketId', GetSelectedProjects);
        }
        else if (e.item.name === "BusinessStrategyCompletionDate")
            ShowReportViewer(s, "ProjectByDueDate", "Project By DueDate");
        else if (e.item.name === "BusinessStrategy")
            ShowReportViewer(s, "BusinessStrategy", "Business Strategy");
        else if (e.item.name == "SVCProjectTask")
            showSVCProjectTaskPopup();
        else if (e.item.name == "TrackProjectStageHistory") {
            GetTrackProjectStageReport()
        }
        else if (e.item.name == "ExecutiveSummary") {
            //ShowCoreServiceReportPopup();
            ShowReportViewer(s, "ExecutiveSummary", "Executive Summary Report");
        }

        else if (e.item.name == "CombinedLostJobReport") {
            $('#spCLJR').html('Combined Lost Job Report');
            //ShowtCLJRPopup();
            ShowReportViewer(s, "CombinedLostJobReport", "Combined Lost Job Report");
        }
        else if (e.item.name == "CombinedJobReport") {
            $('#spCLJR').html('Combined Job Report');
            //ShowtCLJRPopup();
            ShowReportViewer(s, "CombinedLostJobReport", "Combined Job Report");

        }
        else if (e.item.name == "BusinessUnitDistributionReport") {
            //$('#spBUDReport').html('Business Unit Distribution Report');
            //ShowtBusinessUnitDistributionReportPopup();
            ShowReportViewer(s, "BusinessUnitDistribution", "Division Distribution Report");
        }
        else if (e.item.name == "StudioSpecificReport") {
            ShowReportViewer(s, "StudioSpecific", "Studio Specific Report");
        }
        else if (e.item.name == "ResourceSchedulerReport") {
            ShowReportViewer(s, "ResourceSchedulerReport", "Resource Scheduler Report");
        }
        else if (e.item.name == "ClientContractSummary")
            ShowClientContractSummaryReport();
        else if (e.item.name == "OPMWinsAndLosses") {
            ShowtWinLossRPopup();
        }
        else {
            showReportPopup(e.item.name);
        }
    }

    function GetTrackProjectStageReport() {
        var trackProjectStageUrl = '<%=TrackProjectStageUrl%>&AllProject=true';
        window.parent.UgitOpenPopupDialog(trackProjectStageUrl, '', 'Project Stage History', '1070px', '530px');
    }

    function showGroupScoreCard() {
        var path = "<%= groupScorecardPath %>";
        window.parent.UgitOpenPopupDialog(path, "", "Group Score Card", 90, 90, false, escape(window.location.href));
        return false;
    }

    function OpenImportExcel() {
        var title = '<%=importTitle%>';
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', '', title, '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function showBottleNeckChartPopup() {
        var moduleName = "<%= ModuleName %>";
        var path = "<%= bottelNeckChartPath %>";
        var moduletitle = "<%= moduleRowTitle %>"

        window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName + "&moduletitle=" + moduletitle, "Bottleneck Chart", 90, 90, false, escape(window.location.href));
        return false;
    }

    function showSVCProjectTaskPopup() {
        var moduleName = moduleName;
        var path = "<%= svcprojectTaskListPath %>";

        window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Pending Service Tasks", '90', '90', false, escape(window.location.href));
        return false;
    }

    //$(document).on("click", "div.listItem", function (e) {
    //    var path = "/Layouts/uGovernIT/DelegateControl.aspx?control=importallocationtemplate&ProjectID=" + arrOfSelectedTickets[0];
    //    window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Select Allocation Template", '50', '50', false, escape(window.location.href));
    //});

    function OpenCreateTemplatePopup(callback) {
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select at least one item.");
            return false;
        }

        else if (arrOfSelectedTickets.length > 1) {
            alert("Please select one item.");
            return false;
        }
        $.get("/api/rmone/GetProjectDates?TicketId=" + arrOfSelectedTickets[0], function (data, status) {
            let preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : data.PreconStart;
            let preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : data.PreconEnd;
            let constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : data.ConstStart;
            let constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : data.ConstEnd;
            if (preconStartDate == '' || preconEndDate == ''
                || constStartDate == '' || constEndDate == '') {
                if (callback == undefined) {
                    openDateAgent(arrOfSelectedTickets[0]);
                    //DevExpress.ui.dialog.alert("Please Enter Valid Phases Dates On Selected Project.", "Error!");
                }
            }
            else {
                if (new Date(constStartDate) <= new Date(preconEndDate)
                    && new Date(constEndDate) >= new Date(preconStartDate)) {
                    if (callback == undefined) {
                        openDateAgent(arrOfSelectedTickets[0]);
                        //DevExpress.ui.dialog.alert("Please Enter Valid Phases Dates On Selected Project - Phases precon and construction phase have overlapping schedules", "Error!");
                    }
                }
                else
                {
                    $('#<%= hdnticketId.ClientID%>').val(arrOfSelectedTickets[0]);

                    pcSaveAsTemplate.Show();
                    pcSaveAsTemplate.PerformCallback();

                    //var path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&control=saveallocationastemplate&isreadonly=true&ticketId=" + arrOfSelectedTickets[0] + "&module=" + moduleName;
                    //window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Save Allocation as Template(" + arrOfSelectedTickets[0] + ")", '50', '60', false, escape(window.location.href));
                }
            }
        });

    }

    function ShowImportAllocations() {
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        var d = gridClientInstance.GetRowValues(gridClientInstance.GetFocusedRowIndex(), 'Title');
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select at least one item.");
            return false;
        }
        else if (arrOfSelectedTickets.length > 1) {
            $("#popupContainer").dxPopup({
                title: "Please select one item.",
                visible: true,
                selectionMode: "single",
                maxWidth: "350px",
                maxHeight: "300px",
                contentTemplate: function (container) {
                    container.append(
                        $("<div class='listItem' />").dxList({
                            dataSource: arrOfSelectedTickets,
                            searchEnabled: false,
                            searchMode: 'startswith',
                            onItemClick: function (item) {
                                var path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + item.itemData + "&module=CPR";
                                window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Project Allocation: " + item.itemData, '85', '85', false, escape(window.location.href));
                            }
                        })
                    ).appendTo(container);

                }

            });
            return false;
        }
        else {
            var path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + arrOfSelectedTickets[0] + "&module=CPR";
            window.parent.UgitOpenPopupDialog(path, "moduleName=" + moduleName, "Project Allocation: " + arrOfSelectedTickets[0], '85', '85', false, escape(window.location.href));
        }

        return false;
    }

    function NewContactFromCompany() {
        //
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select at least one item.");
            return false;
        }

        else if (arrOfSelectedTickets.length > 1) {
            alert("Please select one item.");
            return false;
        }

        var params = "hpac=1&TicketId=0&CompanyTicketId=" + arrOfSelectedTickets[0];
        window.parent.parent.UgitOpenPopupDialog('<%=NewContactUrl%>', params, 'New Contact', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function popupMenu(s, e) {
        if (e.isSelected) {
            if (e.visibleIndex == 0)
                $.cookie("hideprevious", true);
            else
                $.cookie("hideprevious", false);
            if (s.cpRowCount == e.visibleIndex + 1)
                $.cookie("hidenext", true);
            else
                $.cookie("hidenext", false);

            if (s.pageRowCount == 1 && e.visibleIndex == 0) {
                $.cookie("hideprevious", true);
                $.cookie("hidenext", true);
            }
        }

        if ("<%= enableModuleAgent%>" == "True") {
            s.GetSelectedFieldValues('TicketId;Status', OnGetSelectedFieldValuesForGrid)
        }
    }


    function OnGetSelectedFieldValuesForGrid(selectedValues, d) {

        try {

            var ids = "";
            var ticketTitle = "";
            var params = ""
            var obj = ASPxPopupCustomFilterActionMenu.GetItemByName("Agent");
            if (obj != null)
                var itemCount = obj.GetItemCount();
            if (itemCount != undefined)
                itemCount = parseInt(itemCount);


            var agentBtns = $(".btn-agents");
            if (agentBtns != undefined && agentBtns != null) {
                $.each(agentBtns, function (i, item) {

                    var agentTab = $(item).attr("Id");
                    if (agentTab != "") {
                        if (selectedValues.length == 0) {
                            $(item).hide();
                        }
                        else {
                            var ticketStage = selectedValues[0][1];
                            for (var i = 0; i < selectedValues.length; i++) {
                                if (ticketStage == selectedValues[i][1]) {
                                    var agentStages = agentTab.split("~")[2];
                                    var agentID = agentTab.split("~")[1];
                                    if (agentStages.indexOf(ticketStage) == -1) {
                                        $(item).hide();
                                    }
                                    else
                                        $(item).show();
                                }
                                else
                                    $(item).hide();
                            }

                        }

                    }
                });
            }

            if (selectedValues.length == 0) {
                for (var i = 0; i < itemCount; i++) {
                    obj.items[i].SetVisible(false);
                    obj.SetVisible(false);
                }

            }
            else {
                var ticketStage = selectedValues[0][1];
                for (var y = 0; y < selectedValues.length; y++) {
                    if (ticketStage == selectedValues[y][1]) {
                        for (var x = 0; x < itemCount; x++) {
                            var agentSubItemName = obj.items[x].name;

                            var agentStages = (agentSubItemName.split('#')[3]);
                            if (agentSubItemName.indexOf(ticketStage) != -1) {
                                obj.SetVisible(true);
                                obj.items[x].SetVisible(true);


                            }
                            else {
                                obj.items[x].SetVisible(false);

                            }

                        }
                        ticketStage = selectedValues[y][1];
                    }
                    else {
                        for (var i = 0; i < itemCount; i++) {
                            obj.items[i].SetVisible(false);
                            obj.SetVisible(false);
                        }

                    }
                }
            }
        }
        catch (ex) {
        }
    }

    function popupMenuCustomMenuItemClick(s, e) {
        if (e.item.name == "NewProjectFromOpportunity") {
            var params = "SelectionMode=OpenOpportunity";

            window.parent.parent.UgitOpenPopupDialog('<%=NewOPMWizardPageUrl%>', params, 'Open Opportunity', '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

                <%--//filterTicketLoading.Show();
                $('#<% hdnPopupControlName.ClientID>').val("NewProjectFromOpportunity");
                $('#<% hdnPopupControl.ClientID%>').val("NewProjectFromOpportunity");
                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewProjectFromOpportunity");--%>
        }
        if (e.item.name == "NewCompany") {
            NewCompany();
        }
        if (e.item.name == "NewContact") {
            NewContact();
        }
        if (e.item.name == "NewProjectFromTemplate") {
            return showTicketTemplate();
        }
        else if (e.item.name == "NewRequest") {
            var elem = $("#<%=btNewbutton.ClientID%>");
            if (elem) {
                elem.click();
            }
        }
        else if (e.item.name == "NewContactbyCompany") {
            NewContactbyCompany();
        }
        else if (e.item.name == "AdvancedLead") {
            var params = "SelectionMode=Lead&module=OPM";

            window.parent.parent.UgitOpenPopupDialog('<%=NewOPMWizardPageUrl%>', params, 'Open Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
        else if (e.item.name == "NewOpportunity") {
            window.parent.UgitOpenPopupDialog('<%=NewOPMUrl%>', '', 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            //var params = "SelectionMode=NewOpportunity";

            //window.parent.parent.UgitOpenPopupDialog('<NewOPMWizardPageUrl%>', params, 'Open Opportunity', '90', '90', false, "<Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
        else if (e.item.name == "OpenOpportunity") {
            var params = "SelectionMode=OpenOpportunity&module=CPR";

            window.parent.parent.UgitOpenPopupDialog('<%=NewOPMWizardPageUrl%>', params, 'Open Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
    }
    function CreateNewProject() {
        var elem = $("#<%=btNewbutton.ClientID%>");
        if (elem) {
            elem.click();
        }
    }
    function popupMenuCustomFilterActionMenuItemClick(s, e) {
        if (e.item.name == "ChangeTicketType")
            OnbtnTicketTypeChangeClick();
        else if (e.item.name == "BatchEdit")
            OnbtnAllowBatchEditClick();
        else if (e.item.name == "BatchCreate")
            OnbtnBatchCreateClick();
        else if (e.item.name == "AssignToMe")
            OnBatchReassignToMe();
        else if (e.item.name == "Reassign")
            OnbtnBatchReassignClick();
        else if (e.item.name == "Escalation")
            OnbtnBatchEscalationClick();
        else if (e.item.name == "QuickClose")
            OnbtnBatchCloseOrReject();
        else if (e.item.name == "Comment")
            OnbtnBatchComments();
        else if (e.item.name.indexOf("SendAgentLink") != -1)
            OnbtnSendAgentLink(e.item.name);
        else if (e.item.name.indexOf("AgentSubItem") != -1)
            ShowAgents(e.item.name);
        else if (e.item.name == "Reopen")
            OnbtnReOpenClosedTicket();
        else if (e.item.name == "CopyLinktoClipboard")
            copyToClipboard();
        else if (e.item.name == "Duplicate")
            DoDuplicateTicket();
        else if (e.item.name == "Putonhold") {
            OnbtnPutOnHoldClick();
        }
        else if (e.item.name == "PutonUnhold") {
            OnbtnPutOnUnHoldClick();
        }
        else if (e.item.name == "ProjectSimilarity")
            OnbtnProjectSimilarityClick();
        else if (e.item.name == "ProjectPlan") {
            OnbtnProjectPlanClick();
        }
        else if (e.item.name == "ExcelImport") {
            OpenImportExcel();
        }
        else if (e.item.name == "ProCompactReport") {
            GetProjectCompactReport();
        }

        else if (e.item.name == "TrackProjectStageHistory") {
            GetTrackProjectStageReport()
        }

        else if (e.item.name == "SVCProjectTask") {
            showSVCProjectTaskPopup();
        }
        else if (e.item.name == "ImportAllocations") {
            ShowImportAllocations();
        }
        else if (e.item.name == "CreateTemplate") {
            OpenCreateTemplatePopup();
        }
        else if (e.item.name == "NewOPMWizard") {
            ShowNewOPMWizard();
        }
        else if (e.item.name == "UpdateTags") {
            var arrOfSelectedTickets = GetSelectedKeysOnPage();
            if (arrOfSelectedTickets.length < 1) {
                alert("Please select at least one item.");
                return false;
            }

            var params = "ProcessSelectedRecords=True" + "&TicketIDs=" + arrOfSelectedTickets.join();
            window.parent.parent.UgitOpenPopupDialog('<%= CreateProjectTagsUrl %>', params, "Update Project Tags", '700px', '210px', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
        else if (e.item.name == "UpdateStatistics") {
            var arrOfSelectedTickets = GetSelectedKeysOnPage();
            if (arrOfSelectedTickets.length < 1) {
                alert("Please select at least one item.");
                return false;
            }

            busyLoader.Show();
            var params = "Action=UpdateStatistics" + "&TicketIDs=" + arrOfSelectedTickets.join();
            ASPxCallbackPanel_Actions.PerformCallback(params);
            
        } 
        else {
            
            if (e.item.name == "NewLeadFromCompany") {
              
                $('#<%= hdnPopupControlName.ClientID%>').val("NewLeadFromCompany");
                $('#<%= hdnPopupControl.ClientID%>').val("NewLeadFromCompany");
                
                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewLeadFromCompany");
            }

            if (e.item.name == "NewOpportunityFromLead") {

                $('#<%= hdnPopupControlName.ClientID%>').val("NewOpportunityFromLead");
                $('#<%= hdnPopupControl.ClientID%>').val("NewOpportunityFromLead");
                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewOpportunityFromLead");
            }

            if (e.item.name == "NewProjectFromOpportunity") {

                var params = "SelectionMode=OpenOpportunity";

                window.parent.parent.UgitOpenPopupDialog('<%=NewOPMWizardPageUrl%>', params, 'Open Opportunity', '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

                <%--//old code
                $('#<% hdnPopupControlName.ClientID%>').val("NewProjectFromOpportunity");
                $('#<% hdnPopupControl.ClientID%>').val("NewProjectFromOpportunity");
                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewProjectFromOpportunity");--%>
            }

            if (e.item.name == "NewServiceFromOpportunity") {

                $('#<%= hdnPopupControlName.ClientID%>').val("NewServiceFromOpportunity");
                $('#<%= hdnPopupControl.ClientID%>').val("NewServiceFromOpportunity");
                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewServiceFromOpportunity");
            }

            if (e.item.name == "NewContactFromCompany") {

                $('#<%= hdnPopupControlName.ClientID%>').val("NewContactFromCompany");
                $('#<%= hdnPopupControl.ClientID%>').val("NewContactFromCompany");

                ClientPopupControl.Show();
                ClientPopupControl.PerformCallback("NewContactFromCompany");
                //NewContactFromCompany();
            }

            if (e.item.name == "NewLead")
                OnBtnNewLeadClick();

            if (e.item.name == "NewOpportunity")
                OnBtnNewOpportunityClick();

            if (e.item.name == "NewProject")
                OnBtnNewProjectClick();

            if (e.item.name == "NewCPR")
                OnBtnNewProjectFormLeadClick();

            if (e.item.name == "NewCompany") {
                NewCompany();
            }

            if (e.item.name == "NewContact") {
                NewContact();
            }

            if (e.item.name == "ProjectCardView")
                OnGetSelectedFieldValuesForProjectCard();

            else if (e.item.name == "ProjectSimilarity")
                OnbtnProjectSimilarityClick();

            else if (e.item.name == "ProjectPlan")
                OnbtnProjectPlanClick();

            else if (e.item.name == "ExcelImport") {
                OpenImportExcel();
            }
        }
    }

    function OpenImportExcel() {
        var title = '<%=importTitle%>';
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', '', title, '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }
    function popupMenuExportItemClick(s, e) {
        if (e.item.name == "Excel") {
            downloadExcel();
            e.processOnServer = false;
        }
        else if (e.item.name == "email") {
            OnbtnBatchEscalationClick();
            e.processOnServer = false; 
        }
        else if (e.item.name == "CopyLinktoClipboard") {
            copyToClipboard();
            e.processOnServer = false; 
        }
        
        //else if (e.item.name == "Pdf")
        //    downloadPDF();

    }

    function DoDuplicateTicket() {
        CreateDuplicateTicket();
    }

    function CreateDuplicateTicket() {
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select at least one item.");
            return false;
        }
        else if (arrOfSelectedTickets.length > 1) {
            alert("You can not select more than one item.");
            return false;
        }

        var url = '<%=createDuplicateUrl%>';
        if (url != '') {
            url = url + '?TicketId=0' + '&SourceTicketId=' + arrOfSelectedTickets[0] + '&Duplicate=true';
            window.parent.UgitOpenPopupDialog(url, "", "New " + '<%=this.ModuleName%>' + " Item", "90", "90", false, escape(window.location.href));
        }

    }
    function OnbtnPutOnHoldClick(s, e) {
        var prefixcolumn = "<%=uGovernIT.Manager.uHelper.getAltTicketIdField(_context, this.ModuleName) %>";
        //gridClientInstance.GetSelectedFieldValues('TicketId;OnHold;StageStep', OnGetSelectedFieldValuesForPutOnHold);
        GetSelectedFieldValues('TicketId;OnHold;StageStep;Title;' + prefixcolumn, OnGetSelectedFieldValuesForPutOnHold);
    }
    function OnGetSelectedFieldValuesForPutOnHold(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var holdMaxStage = parseInt('<%= this.holdMaxStage%>');
        var selectedTicketIds = "";
        var onHoldTicketIds = "";
        var onHoldMaxStageTicketIds = "";
        var titleText = "";
        for (var i = 0; i < selectedValues.length; i++) {
            if ((selectedValues[i][1] != null && selectedValues[i][1] == "1")) {
                if (selectedValues.length == 1)
                    onHoldTicketIds = selectedValues[i][0];
                else
                    onHoldTicketIds += selectedValues[i][0] + ";";
            }
            else if (parseInt(selectedValues[i][2]) > holdMaxStage) {
                if (selectedValues.length == 1)
                    onHoldMaxStageTicketIds = selectedValues[i][0];
                else
                    onHoldMaxStageTicketIds += selectedValues[i][0] + ";";
            }
            else {
                if (selectedValues.length == 1) {
                    selectedTicketIds = selectedValues[i][0];
                    titleText = selectedValues[i][4] + ": " + selectedValues[i][3];
                }
                else {
                    selectedTicketIds += selectedValues[i][0] + ";";
                    titleText += (titleText == "" ? "" : ', ') + selectedValues[i][4] + ": " + selectedValues[i][3];
                }
            }
        }
        if (selectedTicketIds == "") {
            alert("Please select at least one item which is not on hold, or whose current stage is less than the maximum allowed hold stage.");
            return;
        }
        var showPopUp = true;
        var confirmMsg = "";
        if (onHoldMaxStageTicketIds != "") {
            confirmMsg = "One or more of the selected items have stage greater than the maximum allow hold stage";
            showPopUp = false;
        }

        if (onHoldTicketIds != "") {
            confirmMsg += "\nOne or more of the selected items are already on hold";
            showPopUp = false;
        }

        if (confirmMsg != "") {
            confirmMsg += "\nDo you want to continue?";
            if (confirm(confirmMsg)) {
                showPopUp = true;
            }
            else
                showPopUp = false;
        }

        if (showPopUp) {
            params = "moduleName=" + '<%=ModuleName%>' + "&ids=" + selectedTicketIds + "&stagestep=" + selectedValues[2] + "&onhold=" + selectedTicketIds[1] + "&titleText=" + titleText + "&Action=Hold";
            window.parent.parent.UgitOpenPopupDialog('<%=TicketHoldUnHoldUrl%>', params, "Put On Hold", '700px', '350px', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
        return false;
    }
    function OnbtnPutOnUnHoldClick(s, e) {
        var prefixcolumn = "<%=uGovernIT.Manager.uHelper.getAltTicketIdField(_context, this.ModuleName) %>";

        //gridClientInstance.GetSelectedFieldValues('TicketId;OnHold', OnGetSelectedFieldValuesForPutOnUnHold);
        GetSelectedFieldValues('TicketId;OnHold;Title;' + prefixcolumn, OnGetSelectedFieldValuesForPutOnUnHold);
    }
    function OnGetSelectedFieldValuesForPutOnUnHold(selectedValues) {

        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var titleText = "";
        var selectedTicketIds = "";
        var onHoldTicketIds = "";
        for (var i = 0; i < selectedValues.length; i++) {
            if (selectedValues[i][1] != null && selectedValues[i][1] == "1") {

                if (selectedValues.length == 1) {
                    onHoldTicketIds = selectedValues[i][0];
                    titleText = selectedValues[i][3] + ": " + selectedValues[i][2];
                }
                else {
                    onHoldTicketIds += selectedValues[i][0] + ";";
                    titleText += (titleText == "" ? "" : ', ') + selectedValues[i][3] + ": " + selectedValues[i][2];
                }

            }
            else {
                if (selectedValues.length == 1)
                    selectedTicketIds = selectedValues[i][0];
                else 
                    selectedTicketIds += selectedValues[i][0] + ";";
            }
        }

        if (onHoldTicketIds == "") {
            alert("Please select at least one item which is already on Hold.");
            return;
        }
        var showPopUp = false;
        if (selectedTicketIds != "") {
            var confirmMsg = "One or more of the selected items are already not on hold, this will remove hold from items which are on hold. \nDo you want to continue?";
            if (confirm(confirmMsg)) {
                showPopUp = true;
            }
        }
        else
            showPopUp = true;
        if (showPopUp) {
            //params = "ids=" + selectedValues[0] + "&BatchEditing=true" + "&AllTickets=" + selectedValues;
            params = "moduleName=" + '<%=ModuleName%>' + "&ids=" + onHoldTicketIds + "&titleText=" + titleText + "&Action=UnHold";
            window.parent.parent.UgitOpenPopupDialog('<%=TicketHoldUnHoldUrl%>', params, "Remove Hold", '700px', '340px', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }

        return false;
    }

    function copyToClipboard() {
        getTicketUrlForCopyToClipboard();
    }
    function getTicketUrlForCopyToClipboard() {
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select at least one item.");
            return false;
        }
        else if (arrOfSelectedTickets.length > 1) {
            alert("You cannot select more than one item.");
            return false;
        }
        var url = '<%=clipboardUrl%>';
        var moduleName = '<%=ModuleName%>';
        if (moduleName != null && moduleName != "undefined" && moduleName != "") {
            var completeurl = url + 'TicketId=' + arrOfSelectedTickets[0] + '&ModuleName=' + moduleName;
            $('#lblticketUrl').val(completeurl);
            if (navigator.appVersion.indexOf("Mac") != -1)
                aspxPopupCopyToClipboard.SetHeaderText("Press <span style='font-size:16px;position:relative;top:2px;'>&#x2318;</span>-C to copy");

            aspxPopupCopyToClipboard.Show();
        }
        else {
            alert("No module is associated with this item.");
            return false;
        }
    }

    function SetSelectedTab(SelectedTab) {
        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        var selectedValue = SelectedTab.value;
        var completeurl = '';

        var url = '<%=clipboardUrl%>';
        var moduleName = '<%=ModuleName%>';
        if (moduleName != null && moduleName != "undefined" && moduleName != "") {
            if (selectedValue !== '0') {
                completeurl = url + 'TicketId=' + arrOfSelectedTickets[0] + '&ModuleName=' + moduleName + '&showTab=' + selectedValue;
            }
            else {
                completeurl = url + 'TicketId=' + arrOfSelectedTickets[0] + '&ModuleName=' + moduleName
            }

            $('#lblticketUrl').val(completeurl);
        }
    }


    function autoSelect() {
        $('#lblticketUrl').trigger('click');
    }
    var agentSendLink = "";
    function OnbtnSendAgentLink(agentlink) {

        agentSendLink = agentlink;
        //gridClientInstance.GetSelectedFieldValues('TicketId;Title', OnGetSelectedValuesForSendAgentLink);
        GetSelectedFieldValues('TicketId;Title', OnGetSelectedValuesForSendAgentLink);
    }
    function setAgentLink(agentName, agentID) {

        agentNames = agentName;
        agentId = agentID;
        //gridClientInstance.GetSelectedFieldValues('TicketId;Title', OnGetSelectedFieldValuesForShowAgents);
        GetSelectedFieldValues('TicketId;Title', OnGetSelectedFieldValuesForShowAgents);
    }

    function setAgentButtonColor(s, e) {

        var tab = s.GetTabElement(e.tab.index)
        $(tab).removeClass();
        $(tab).addClass("dxtc-tab")
    }
    function OnGetSelectedValuesForSendAgentLink(selectedValues) {

        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return false;
        }
        var ids = "";
        var ticketTitle = "";
        var params = "";
        var moduleName = '<%=ModuleName%>';
        if (selectedValues.length > 1) {
            for (var i = 0; i < selectedValues.length; i++)
                ids += selectedValues[i][0] + ";";
        }
        else if (selectedValues.length == 1) {
            ids += selectedValues[0][0];
        }
        var agentID = agentSendLink.split('#')[1];
        var agentName = agentSendLink.split('#')[2];
        var agentLink = '<%=TicketManualEscalationUrl%>';
        params = "sendagentlink=1&ids=" + ids + "&ModuleName=" + moduleName + "&agentID=" + agentID + "";
        window.parent.UgitOpenPopupDialog(agentLink, params, "Agent: " + agentName + "", "700px", "700px", false, escape(window.location.href));
    }

    var agentNames = "";
    var agentId = "";
    function ShowAgents(agentProp) {
        agentNames = agentProp.split("#")[2];
        agentId = agentProp.split("#")[1];
        //gridClientInstance.GetSelectedFieldValues('TicketId;Title', OnGetSelectedFieldValuesForShowAgents);
        GetSelectedFieldValues('TicketId;Title', OnGetSelectedFieldValuesForShowAgents);
        return false;
    }
    function OnGetSelectedFieldValuesForShowAgents(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return false;
        }
        var ids = "";
        var ticketTitle = "";
        var params = "";

        if (selectedValues.length > 1) {
            for (var i = 0; i < selectedValues.length; i++)
                ids += selectedValues[i][0] + ";";
        }
        else if (selectedValues.length == 1) {
            ids += selectedValues[0][0];

        }

        var moduleName = '<%=ModuleName%>';
        var agentPath = '<%=ServiceURL%>' + agentId;
        params = "TicketId=" + ids + "&ModuleName=" + moduleName + "";

        var agentName = agentNames;
        window.parent.UgitOpenPopupDialog(agentPath, params, "Agent: " + agentName + "", "90", "90", false, escape(window.location.href));
        return true;
    }
    $(function () {
        try {
            var tabName = '<%=HomeTabName%>';
            window.parent.RefreshTabCount(tabName, parseInt('<%=tabCount%>'));
        } catch (ex) {
        }
    });

    function OnBtnNewLeadClick() {
        OnGetSelectedFieldValuesForNewLead();

    }

    function OnBtnNewOpportunityClick() {
        OnGetSelectedFieldValuesForNewOpportunity();
    }


    function OnBtnNewProjectFormLeadClick() {

        var arrOfSelectedTickets = GetSelectedKeysOnPage();

        if (arrOfSelectedTickets.length < 1) {
            alert("Please select one item.");
            return false;
        }

        else if (arrOfSelectedTickets.length > 1) {
            alert("Please select only one item.");
            return false;
        }

        var params = "LeadTicketId=" + arrOfSelectedTickets[0];

        GetOpenTasksCount(arrOfSelectedTickets[0]);
        if (OpenTaskCount > 0) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Open Tasks Alert",
                message: "You have some pending Tasks. Please select one of the options below.",
                buttons: [
                    { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                    { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {

                if (dialogResult == "CloseTasks") {
                    params = params + "&CompleteTasks=true";
                }
                else if (dialogResult == "KeepTasksOpen") {
                    params = params + "&CompleteTasks=false";
                }
                else
                    return false;

                window.parent.UgitOpenPopupDialog('<%=NewProjectUrl%>', params, 'New Project', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            });
        }
        else {
            params = params + "&CompleteTasks=false";
            window.parent.UgitOpenPopupDialog('<%=NewProjectUrl%>', params, 'New Project', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
        //window.parent.parent.UgitOpenPopupDialog('<%=NewProjectUrl%>', params, 'New Project', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

        return false;
    }

    function OnGetSelectedFieldValuesForNewLead() {
        //if (selectedValue.length < 1) {
        //    alert("Please select atleast one item.");
        //    return;
        //}

        var arrOfSelectedTickets = GetSelectedKeysOnPage();
        if (arrOfSelectedTickets.length < 1) {
            alert("Please select one item.");
            return false;
        }
        else if (arrOfSelectedTickets.length > 1) {
            alert("Please select only one item.");
            return false;
        }

        var params = "CompanyTicketId=" + arrOfSelectedTickets;
        window.parent.parent.UgitOpenPopupDialog('<%=NewLeadUrl%>', params, 'New Lead', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        return false;
    }

    function OnGetSelectedFieldValuesForProjectCard() {

        var arrOfSelectedTickets = GetSelectedKeysOnPage();

        if (arrOfSelectedTickets.length < 1) {

            alert("Please select at least one item.");

            return;
        }

        var params = "CompanyTicketId=" + arrOfSelectedTickets;

        window.parent.parent.UgitOpenPopupDialog('<%=ProjectCardViewUrl%>', params, 'Project View', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

        return false;
    }




    function OnGetSelectedFieldValuesForNewOpportunity() {

        var arrOfSelectedTickets = GetSelectedKeysOnPage();

        if (arrOfSelectedTickets.length < 1) {
            alert("Please select one item.");
            return;
        }

        else if (arrOfSelectedTickets.length > 1) {
            alert("Please select only one item.");
            return false;
        }

        var params = "LeadTicketId=" + arrOfSelectedTickets[0];

        GetOpenTasksCount(arrOfSelectedTickets[0]);
        if (OpenTaskCount > 0) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Open Tasks Alert",
                message: "You have some pending Tasks. Please select one of the options below.",
                buttons: [
                    { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                    { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {

                if (dialogResult == "CloseTasks") {
                    params = params + "&CompleteTasks=true";
                }
                else if (dialogResult == "KeepTasksOpen") {
                    params = params + "&CompleteTasks=false";
                }
                else
                    return false;

                window.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', params, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            });
        }
        else {
            params = params + "&CompleteTasks=false";
            window.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', params, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }

        //window.parent.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', params, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

        return false;
    }

    function NewCompany() {

        var params = "hpac=1&TicketId=0";

        window.parent.parent.UgitOpenPopupDialog('<%=NewCompanyUrl%>', params, 'New Company', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function NewContact() {

        var params = "hpac=1&TicketId=0";

        window.parent.parent.UgitOpenPopupDialog('<%=NewContactUrl%>', params, 'New Contact', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }


    function createNewTicket() {
        controlsPopup.Show();
    }
    function OnCloseUp(s, e) {
        //gridClientInstance.Refresh();
        if (typeof gridClientInstance !== "undefined")
            gridClientInstance.Refresh();
        else if (typeof cardClientInstance !== "undefined")
            cardClientInstance.Refresh();

        controlsPopup.RefreshContentUrl();
    }

    function PopupCloseBeforeOpenUgit(url, tickedId, tickedIdVal, width, height, refresh, urlVal) {
        $("#NewTicketDialog").remove();
        window.parent.UgitOpenPopupDialog(url, tickedId, tickedIdVal, width, height, refresh, urlVal);

    }
    function OnBeginCallback(s, e) {
        callbackCommand.Set("command", e.command);
    }

    function OnColumnMoving(s, e) {
        
        if (moduleName !== '') {
            if (e.destinationColumn === null) {
                var cols = moduleName + ';#';
                for (var i = 0; i < gridClientInstance.columns.length; i++) {
                    if (gridClientInstance.columns[i]["visible"] === false)
                        cols = cols + gridClientInstance.columns[i]["fieldName"] + ',';
                }
                cols = cols + e.sourceColumn.fieldName;
                columnMoved.Set(moduleName + 'Columns', cols);
                $.cookie(moduleName + 'Columns', cols, { path: "/" });

                //console.log("Selected Columns: " + cols);
            }
        }
        else {
            if (e.destinationColumn === null) {
                columnMoved.Set('NonModuleColumns', "true");
            }
        }
    }

    //function OnColumnStartDragging(s, e) {
    //    //
    //    console.log("\ne.column.fieldName: " + e.column.fieldName);
    //    console.log("e.column.destinationColumn: " + e.column.destinationColumn);
    //}

    //function OnCustomizationWindowCloseUp(s, e) {
    //    console.log("Customization Window Close");
    //}


    // Below two functions added, to work for both Grid & Card Views.
    function GetSelectedKeysOnPage() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.GetSelectedKeysOnPage();
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.GetSelectedKeysOnPage();
        else
            return null;
    }

    function GetSelectedFieldValues(field, functionName) {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.GetSelectedFieldValues(field, functionName);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.GetSelectedFieldValues(field, functionName);
        else
            return null;
    }

    function GetPageIndex() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.pageIndex;
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.pageIndex;
        else
            return 0;
    }

    function GetKeys(index) {

        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.keys[index];
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.keys[index];
        else
            return 0;
    }

    function GetVisibleRowsOnPage() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.GetVisibleRowsOnPage();
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.GetVisibleCardsOnPage();
        else
            return null;
    }

    function GetRowValues(callbackindex, field, functionName) {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.GetRowValues(callbackindex, "Title;TicketId", getGridTitleValue);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.GetRowValues(callbackindex, "Title;TicketId", getGridTitleValue);
        else
            return null;
    }

    function SelectRowsByKey(field) {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.SelectRowsByKey(field);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.SelectRowsByKey(field);
        else
            return null;
    }

    function UnselectAllRowsOnPage() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.UnselectAllRowsOnPage();
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.UnselectAllRowsOnPage();
        else
            return null;
    }

    function GetkeysindexOf() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.keys.indexOf(GetSelectedKeysOnPage()[0]);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.keys.indexOf(GetSelectedKeysOnPage()[0]);
        else
            return 0;
    }

    function MoveToNextPage() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.NextPage(true);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.NextPage(true);
        else
            return null;
    }

    function MoveToPrevPage() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.PrevPage(true);
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.PrevPage(true);
        else
            return null;
    }

    function GetLengthOfKeys() {
        if (typeof gridClientInstance !== "undefined")
            return gridClientInstance.keys.length;
        else if (typeof cardClientInstance !== "undefined")
            return cardClientInstance.keys.length;
        else
            return 0;
    }

    function ShowNewOPMWizard() {
        var params = "";
        window.parent.UgitOpenPopupDialog("<%=NewOPMWizardPageUrl%>", params, "New Opportunity", '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        if (typeof gridClientInstance !== "undefined") {
            gridClientInstance.SetHeight(0);

            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridClientInstance.SetHeight(containerHeight);
        }
    }

    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    var title = "";
    var dataModel = {
    preconStartDate: "",
    preconEndDate: "",
    constStartDate: "",
    constEndDate: "",
    closeoutStartDate: "",
    closeoutEndDate: "",
    onHold: false,
    preconDuration: "",
    constDuration: "",
    closeOutDuration: ""
};
var Model = {
    RecordId: "",
    Fields: [{
        FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconStartDate%>",
        Value: dataModel.preconStartDate
    }, {
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconEndDate%>",
            Value: dataModel.preconEndDate
        }, {
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionStart%>",
            Value: dataModel.constStartDate
        }, {
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionEnd%>",
            Value: dataModel.constEndDate
        }, {
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.CloseoutDate%>",
            Value: dataModel.closeoutEndDate
        }, {
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.TicketOnHold%>",
            Value: dataModel.onHold
        }]
};

    $(function () {
        $('#popup').dxPopup({
            visible: false,
            hideOnOutsideClick: true,
            showTitle: true,
            showCloseButton: true,
            title: "Please Enter Valid Dates On Project.",
            width: "auto",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            contentTemplate: () => {
                const content = $("<div />");
                content.append(
                    $("<div id='form' />").dxForm({
                        formData: dataModel,
                        title: 'Update Dates',
                        items: [{
                            itemType: 'group',
                            name: 'group',
                            caption: '',
                            colCount: 3,
                            items: [{
                                dataField: 'preconStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy",
                                    onValueChanged(e) {
                                        var enteredPreconStartDate = e.value;
                                        let newdate = new Date(enteredPreconStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }

                                        if (enteredPreconStartDate != null) {
                                            if (String(enteredPreconStartDate).startsWith("00")) {
                                                enteredPreconStartDate = enteredPreconStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconStartDate;
                                                dataModel.preconStartDate = enteredPreconStartDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                            {
                                dataField: 'preconDuration',
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    value: undefined,
                                    onFocusOut(e) {
                                        if (e.component.option('value') != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.preconStartDate, e.component.option('value'));
                                            $("#form").dxForm("instance").updateData({ 'preconEndDate': new Date(dataModel.preconEndDate).toLocaleDateString('en-US') });
                                        }
                                    },
                                },
                            },
                            {
                                dataField: 'preconEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy",
                                    onValueChanged(e) {
                                        var enteredPreconEndDate = e.value;
                                        let newdate = new Date(enteredPreconEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredPreconEndDate != null) {
                                            if (String(enteredPreconEndDate).startsWith("00")) {
                                                enteredPreconEndDate = enteredPreconEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconEndDate;
                                                dataModel.preconEndDate = enteredPreconEndDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('PreCon End'),
                                },
                            },
                            {
                                dataField: 'constStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.constStartDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy",
                                    onValueChanged(e) {
                                        var enteredConstStartDate = e.value;
                                        let newdate = new Date(enteredConstStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredConstStartDate != null) {
                                            if (String(enteredConstStartDate).startsWith("00")) {
                                                enteredConstStartDate = enteredConstStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredConstStartDate;
                                                dataModel.constStartDate = enteredConstStartDate;
                                            }
                                        }
                                        if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                            dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                            $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('Const Start'),
                                },
                            },
                            {
                                dataField: 'constDuration',
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    value: undefined,
                                    onFocusOut(e) {
                                        if (e.component.option('value') != '' && dataModel.constStartDate != '') {
                                            dataModel.constEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.constStartDate, e.component.option('value'));
                                            $("#form").dxForm("instance").updateData({ 'constEndDate': new Date(dataModel.constEndDate).toLocaleDateString('en-US') });
                                        }
                                    },
                                },
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                            {
                                dataField: 'constEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.constEndDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy",
                                    onValueChanged(e) {
                                        var enteredConstEndDate = e.value;
                                        let newdate = new Date(enteredConstEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredConstEndDate != null) {
                                            if (String(enteredConstEndDate).startsWith("00")) {
                                                enteredConstEndDate = enteredConstEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredConstEndDate;
                                            }
                                            if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                                dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                                $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                            }
                                            if (enteredConstEndDate != null && enteredConstEndDate != e.previousValue) {
                                                $.ajax({
                                                    type: "GET",
                                                    url: "<%= ajaxPageURL %>/GetNextWorkingDateAndTime?dateString=" + new Date(e.value).toLocaleDateString('en-US'),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    async: false,
                                                    success: function (message) {
                                                        dataModel.closeoutStartDate = new Date(message).toLocaleDateString('en-US');
                                                        dataModel.closeoutEndDate = new Date(GetEndDateByWorkingDays(ajaxHelperPage, message, "<%=CloseOutPeriod%>")).toISOString();
                                                        dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                        $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US'), 'closeoutStartDate': dataModel.closeoutStartDate, 'closeOutDuration': dataModel.closeOutDuration });
                                                    },
                                                    error: function (xhr, ajaxOptions, thrownError) {

                                                    }
                                                });
                                            }
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('Const End'),
                                },
                            },
                            {
                                dataField: 'closeoutStartDate',
                                editorOptions: {
                                    value: dataModel.closeoutStartDate,
                                    format: 'MMM d, yyyy',
                                    readOnly: true,
                                },
                                label: {
                                    template: labelTemplate('Close Out'),
                                },
                            },
                            {
                                dataField: 'closeOutDuration',
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    value: undefined,
                                    onFocusOut(e) {
                                        if (e.component.option('value') != '' && dataModel.closeoutStartDate != '') {
                                            dataModel.closeoutEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.closeoutStartDate, e.component.option('value'));
                                            $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US') });
                                        }
                                    },
                                },
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                            {
                                dataField: 'closeoutEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.closeoutEndDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy",
                                    onValueChanged(e) {
                                        var enteredCloseoutEndDate = e.value;
                                        let newdate = new Date(enteredCloseoutEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredCloseoutEndDate != null) {
                                            if (String(enteredCloseoutEndDate).startsWith("00")) {
                                                enteredCloseoutEndDate = enteredCloseoutEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredCloseoutEndDate;
                                                dataModel.closeoutEndDate = enteredCloseoutEndDate;
                                            }
                                        }
                                        if (dataModel.closeoutEndDate != '' && dataModel.closeoutStartDate != '') {
                                            dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                            $("#form").dxForm("instance").updateData({ 'closeOutDuration': dataModel.closeOutDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('Close Out End'),
                                },
                            },
                            {
                                dataField: 'onHold',
                                editorType: 'dxSwitch',
                                visible: false,
                                editorOptions: {
                                    width: 100,
                                    value: dataModel.onHold,
                                    switchedOffText: "OFF HOLD",
                                    switchedOnText: "ON HOLD",
                                },
                                label: {
                                    template: labelTemplate(''),
                                },
                            }
                            ]
                        }],
                        onContentReady: function (data) {
                            data.element.find("label[for$='preconDuration']").text("Precon Duration(Weeks)");
                            data.element.find("label[for$='constDuration']").text("Const Duration(Weeks)");
                            data.element.find("label[for$='closeOutDuration']").text("Closeout Duration(Weeks)");
                        }
                    }),
                    $("#saveButton").dxButton({
                        text: 'Save',
                        icon: '/content/Images/saveFile_icon.png',
                        onClick: function (e) {
                            UpdateRecord();
                        }
                    })
                );
                return content;
            },
            onDisposing: function (e) {
                dataModel.preconStartDate = "";
                dataModel.preconEndDate = "";
                dataModel.constStartDate = "";
                dataModel.constEndDate = "";
                dataModel.closeoutEndDate = "";
                dataModel.onHold = false;
            }
        });

        $("#toast").dxToast({
            message: "Record Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
    });

    function openDateAgent(ticketid) {
        Model.RecordId = ticketid;
        $.get("/api/rmone/GetProjectDates?TicketId=" + ticketid, function (data, status) {
            dataModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : data.PreconStart;
            dataModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : data.PreconEnd;
            dataModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : data.ConstStart;
            dataModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : data.ConstEnd;
            dataModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : new Date(data.CloseoutStart).toLocaleDateString('en-US');
            dataModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : data.Closeout;
            dataModel.onHold = data.OnHold;
            dataModel.preconDuration = parseInt(data.PreconDuration) > 0 ? data.PreconDuration : "";
            dataModel.constDuration = parseInt(data.ConstDuration) > 0 ? data.ConstDuration : "";
            dataModel.closeOutDuration = parseInt(data.CloseOutDuration) > 0 ? data.CloseOutDuration : "";
            const popup = $("#popup").dxPopup("instance");
            popup.show();
            let form = $("#form").dxForm("instance");
            form.option("formData", dataModel);
            //form.itemOption("group", "caption", title);
        });
    }
    
    function openProjectSummaryPage(obj) {
        var ticketid = $(obj).attr("ticketid");
        var ticketitle = $(obj).attr("ticketTitle");
        var IsSummary = $(obj).attr("IsSummary");
        var params = "TicketId=" + ticketid + "&tickettitle=" + ticketitle + "&IsSummary=" + IsSummary;
        window.parent.UgitOpenPopupDialog('<%=NewProjectSummaryPageUrl%>', params, ticketitle, '95', '95', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

    }

        function labelTemplate(iconName) {
            return (data) => $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
        }

    function UpdateRecord() {

        if ((dataModel.preconEndDate != null && dataModel.preconEndDate != "") && (dataModel.preconStartDate == null || dataModel.preconStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Precon Start Date is required.", "Error!");
            return;
        }
        if (dataModel.preconEndDate != null) {
            if (new Date(dataModel.preconStartDate) > new Date(dataModel.preconEndDate)) {
                DevExpress.ui.dialog.alert("Precon End Date should be after the Precon Start Date.", "Error!");
                return;
            }
        }

        if ((dataModel.constEndDate != null && dataModel.constEndDate != "") && (dataModel.constStartDate == null || dataModel.constStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Construction Start Date is required.", "Error!");
            return;
        }
        if (new Date(dataModel.constStartDate) > new Date(dataModel.constEndDate)) {

            if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
                DevExpress.ui.dialog.alert("Entry of Construction End Date is required.", "Error!");
                return;
            }
            DevExpress.ui.dialog.alert("Construction End Date should be after the Construction Start Date.", "Error!");
            return;
        }

        if (new Date(dataModel.constEndDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout Date should be after the Construction End Date.", "Error!");
            return;
        }
        if (new Date(dataModel.closeoutStartDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout End Date should be after the Closeout Start Date.", "Error!");
            return;
        }

        var arrDates = [
            ['Precon Start Date', dataModel.preconStartDate == null ? "" : dataModel.preconStartDate],
            ['Precon End Date', dataModel.preconEndDate == null ? "" : dataModel.preconEndDate],
            ['Construction Start Date', dataModel.constStartDate == null ? "" : dataModel.constStartDate],
            ['Construction End Date', dataModel.constEndDate == null ? "" : dataModel.constEndDate],
            ['Closeout End Date', dataModel.closeoutEndDate == null ? "" : dataModel.closeoutEndDate]
        ];
        for (let i = 0; i < arrDates.length; i++) {
            let newdate = new Date(arrDates[i][1]);
            if (arrDates[i][1] != "") {
                if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                    DevExpress.ui.dialog.alert("Please enter a valid " + arrDates[i][0]);
                    return;
                }
                Model.Fields[i].Value = newdate.toLocaleDateString('en-US');
            }
        }

        $("#loadpanel").dxLoadPanel("show");
        Model.Fields[5].Value = dataModel.onHold == true ? '1' : '0';
        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>UpdateRecord",
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                if (message.Status == true) {
                    OpenCreateTemplatePopup(true);
                    const popup = $("#popup").dxPopup("instance");
                    popup.hide();

                    $("#loadpanel").dxLoadPanel("hide");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
</script>
<div style="display: none;">
    <dx:ASPxDateEdit ID="DateTimeControl1" runat="server" cssclasstextbox="inputTextBox datetimectr111" Visible="false"></dx:ASPxDateEdit>
</div>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    $(document).ready(function () {        
        $(".selectTemplateForQuickTicket_popUp").parent().addClass("quick-ticket-parent");
        $(".homeDb_gridContainer").parent().addClass("grid-mainContainer");

        var url = window.location.href;
        url = url.split('?');
        if (url != null && url.length == 2) {
            sid = url[1].split('&');
            var istrailuserdir = "<%= IsTrailUserDir%>";
            var serviceid = "<%= ServiceID%>";
            istrailuserdir = sid[0];
            if (istrailuserdir == "ftu") {
                window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ServicesWizard&serviceID=' + sid[1], '', 'Service: Employee on-boarding', 90, 90, 0, '%2flayouts%2fugovernit%2fdelegatecontrol.aspx');
                newUrl = url[0];
                history.pushState({}, null, newUrl);
            }

        }


    })

    $(document).ready(function () {
        var tsr = $.cookie("TSRClicked");
        var acr = $.cookie("ACRClicked");
        var drq = $.cookie("DRQClicked");
        var svc = $.cookie("SVCClicked");
        if (tsr == "True") {
            $('#<%=TSR.ClientID%>').toggleClass("slectedBtn");
        } else if (acr == "True") {
            $('#<%=ACR.ClientID%>').toggleClass("slectedBtn");
        } else if (drq == "True") {
            $('#<%=DRQ.ClientID%>').toggleClass("slectedBtn");
        } else if (svc == "True") {
            $('#<%=SVC.ClientID%>').toggleClass("slectedBtn");
        } else {
            $('#<%=TSR.ClientID%>').toggleClass("slectedBtn");
        }
        delete_cookie("TSRClicked");
        delete_cookie("ACRClicked");
        delete_cookie("DRQClicked");
        delete_cookie("SVCClicked");

    });
    //function OnColumnSorting(s, e) {
    //
    //    alert(1);
    //    if (e.column.fieldName == "StageActionUsersUser")
    //        e.cancel = true;
    //} 
    function ChangeToGanttView(s, e) {
        window.parent.UgitOpenPopupDialog('<%=allocationGanttUrl %>', "", "Director View", "95", "855px");
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12">

    <div class="row">
        <%--    <button type="button" class="btn btn-md ITSMButton-secondary" id="TSR">Incidents</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="SVC">Services</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="DRQ">Change Management</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="ACR">Applications</button>--%>
        <asp:Button ID="TSR" class="btn btn-md ITSMButton-secondary" runat="server" Visible="false" OnClick="TSR_Click" Text="Incidents" />
        <asp:Button ID="SVC" class="btn btn-md ITSMButton-secondary" runat="server" Visible="false" OnClick="SVC_Click" Text="Services" />
        <asp:Button ID="DRQ" class="btn btn-md ITSMButton-secondary" runat="server" Visible="false" OnClick="DRQ_Click" Text="Change Management" />
        <asp:Button ID="ACR" class="btn btn-md ITSMButton-secondary" runat="server" Visible="false" OnClick="ACR_Click" Text="Applications" />
    </div>
</div>

<asp:HiddenField ID="hdnPopupControlName" runat="server" />
<asp:HiddenField ID="hdnPopupControl" runat="server" />
<dx:ASPxPopupControl ID="pcSaveAsTemplate" runat="server" Modal="True" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcSaveAsTemplate" AllowDragging="true"
    HeaderText="Save Allocation as Template" CloseOnEscape="true" OnWindowCallback="pcSaveAsTemplate_WindowCallback" CloseAction="CloseButton" PopupAnimationType="None" EnableViewState="False" CssClass="unsaved_popUp">
    <SettingsAdaptivity Mode="Always" VerticalAlign="WindowCenter" MaxWidth="450px"  />
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccRequestTypeChange" runat="server" Visible="false">
            <asp:HiddenField ID="hdnticketId" runat="server" EnableViewState="true" />
            <ugit:SaveAllocationAsTemplate runat="server" ID="ctrSaveAllocationAsTemplate" />
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px" />
    </ContentStyle>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="aspxPopupHoldTooltip" runat="server" CloseAction="CloseButton"
    PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
    ShowFooter="false" ShowHeader="false" Width="300px" Height="50px" HeaderText="Hold Details" ClientInstanceName="aspxPopupHoldTooltip">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
            <div style="vertical-align: middle" class="aspxPopupHoldTooltipContent"></div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton" AllowDragging="true"
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" PopupElementID="gridTemplate"
    ShowFooter="False" Width="460px" Height="150px" CssClass="selectTemplateForQuickTicket_popUp" HeaderText="Select template to create new item"
    ClientInstanceName="ClientPopupControl" OnWindowCallback="PopupControl_WindowCallback">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
            <div style="vertical-align: middle">
                <asp:HiddenField ID="hndType" runat="server" EnableViewState="true" />
                <asp:HiddenField ID="hndNewTicketTitle" runat="server" EnableViewState="true" />
                <ugit:ASPxGridView ID="gridTemplate" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="gridTemplate" Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid SVCHomeGrid"
                    OnHtmlDataCellPrepared="gridTemplate_HtmlDataCellPrepared" OnAfterPerformCallback="grid_AfterPerformCallback" >
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Template" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" FieldName="Title" Settings-AllowSort="False" CellStyle-Cursor="pointer">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TemplateType" VisibleIndex="3"  GroupIndex="0">  
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowSelectByRowClick="false" AutoExpandAllGroups="false" />
                    <Settings VerticalScrollableHeight="150" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFooter="false" ShowGroupPanel="false" />
                    <ClientSideEvents SelectionChanged="OnSelectionChanged" />
                </ugit:ASPxGridView>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="aspxPopupCopyToClipboard" ClientInstanceName="aspxPopupCopyToClipboard" Width="500" Height="500" Modal="true"
    ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Press Ctrl-C To Copy" SettingsAdaptivity-Mode="Always"
    HeaderStyle-CssClass="modal-header" runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    EnableHierarchyRecreation="True">

    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                <div class="ms-formtable accomp-popup ">
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Choose Service</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:DropDownList runat="server" ID="ddlSelectTab" CssClass="aspxDropDownList itsmDropDownList" onchange="SetSelectedTab(this)"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Comments</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <textarea id="lblticketUrl" rows="5" onclick="this.focus();this.select();" readonly></textarea>
                        </div>
                    </div>
                    <div class="row addEditPopup-btnWrap">
                        <dx:ASPxButton ID="HyperLink2" CssClass="secondary-cancelBtn" runat="server" Visible="true" ToolTip="Close" Text="Cancel"
                            AutoPostBack="false">
                            <ClientSideEvents Click="function(s,e){aspxPopupCopyToClipboard.Hide();}" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents PopUp="function(s,e){autoSelect();}" />
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="pcChangeTicketType" runat="server" CloseAction="CloseButton" Height="250px" Width="370px" OnWindowCallback="pcChangeTicketType_WindowCallback"
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ClientInstanceName="pcChangeTicketType" SettingsAdaptivity-Mode="Always"
    HeaderText="Choose New Ticket Type" CssClass="aspxPopup" HeaderStyle-CssClass="modal-header"
    AllowDragging="false" ShowFooter="false" ShowHeader="true">
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccChangeTicketType" runat="server">
            <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnChangeTicketTypeSave">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                            <div id="trModulelistItem" runat="server" class="row" style="padding-bottom: 20px;">
                                <div class="ms-formlabel ">
                                    <h3 class="ms-standardheader budget_fieldLabel">Module: </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlModuleListItems" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" id="trModuleListItemMessage" runat="server" visible="false">
                                <div>No Target Modules Configured. </div>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btChangeTicketTypeCancel" runat="server" ClientInstanceName="btChangeTicketTypeCancel" Text="Cancel"
                                    ToolTip="Cancel" AutoPostBack="false" CausesValidation="false" CssClass="secondary-cancelBtn" Visible="true">
                                    <ClientSideEvents Click="function(s, e) { pcChangeTicketType.Hide(); }" />
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btnChangeTicketTypeSave" ClientInstanceName="btnChangeTicketTypeSave" runat="server" AutoPostBack="false"
                                    Text="Proceed" CssClass="primary-blueBtn" ToolTip="Proceed">
                                    <ClientSideEvents Click="OpneChangeTicketTypeDialog" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents EndCallback="OnEndTicketTypePopupCallback" />
</dx:ASPxPopupControl>



<div id="rootDiv" runat="server" class="ticket_contentWrap table-responsive grid-Wrap col-md-12 col-sm-12 col-xs-12">
    <asp:Label ID="lblcustomfilterselectedvalue" runat="server" Text="labelText" Style="display: none"></asp:Label>

    <dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" Height="100%"
        ClientInstanceName="ASPxCallbackPanel1" Enabled="true" OnCallback="ASPxCallbackPanel1_Callback">
        <SettingsLoadingPanel Enabled="false" Text="Loading..." />
        <ClientSideEvents EndCallback="function(s,e){initializeDataAfterCallback(s,e);}" />
        <PanelCollection>
            <dx:PanelContent runat="server">
                <dx:ASPxLoadingPanel ID="filterTicketLoading" CssClass="customeLoader" ClientInstanceName="filterTicketLoading" Modal="True" runat="server" Image-Url="~/Content/IMAGES/AjaxLoader.gif" ImagePosition="Top" ContainerElementID="filterticketlist"  Text="Loading...">
<Image Url="~/Content/IMAGES/AjaxLoader.gif"></Image>
                </dx:ASPxLoadingPanel>
                <div class="svcContent_container" id="filterticketlist">
                    <div class="row">
                        <div>
                            <script type="text/javascript" data-v="">
                                function setFilterEnableSettings() {
                                    customUGITGridConfig["<%= grid.ClientID%>"].showfilter = "<%= ShowClearFilter%>";
                                }
                                function showTicketTemplate() {
                                    ClientPopupControl.Show();
                                    ClientPopupControl.PerformCallback();
                                    return false;
                                }
                            </script>

                            <dx:ASPxHiddenField ID="hdnFilter" runat="server" ClientInstanceName="hdnFilterClient"></dx:ASPxHiddenField>

                            <asp:HiddenField ID="tabType" runat="server" EnableViewState="true" />
                            <asp:HiddenField ID="hndClearFilter" runat="server" EnableViewState="true" />
                            <asp:HiddenField ID="hndFilterStatus" runat="server" />
                            <asp:HiddenField ID="selectedTicketIdHidden" runat="server" EnableViewState="true" />

                            <asp:HiddenField ID="hfTicketDelete" runat="server" />

                            <div>
                                <input type="hidden" id="exportPath" value="" />
                                <input type="hidden" id="enableExport" value="" />
                                <div>
                                </div>
                                <%-- //start--%>
                                <div class="mt-0 row svcDashboardContent_mainContainer">
                                    <div id="cModuleDetailPanel" runat="server" class="">
                                        <div class="svcDashboardContent_mainWrap col-md-12 col-sm-12 col-xs-12 noPadding">
                                            <div class="row">
                                                <%--<div class="moduleimgtd" id="cModuleImgPanel" runat="server" valign="top" style="padding-bottom: 8px;">
                                                        <a id="moduleLogoLink" runat="server" href="javascript:">
                                                            <asp:Image runat="server" ID="moduleLogo" Width="32" Height="32" />
                                                        </a>
                                                    </div>--%>
                                                <div class="col-md-6 col-sm-4 col-xs-12 noPadding">
                                                    <div class="moduleimgtd svcAddTicket_icon" id="cModuleImgPanel" runat="server">
                                                        <a id="moduleLogoLink" runat="server" href="javascript:">
                                                            <asp:Image runat="server" ID="moduleLogo" Width="32" Height="32" />
                                                        </a>
                                                    </div>
                                                    <div id="cModuleDescriptionPanel" runat="server" class="row" style="margin-top:10px;color:black;">
                                                        <div class="moduledesciptiontd svcModule_title">
                                                            <asp:Literal runat="server" ID="moduleDescription" Text=""></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                               <%-- <div class="col-md-4 col-sm-4 col-xs-12 px-0 ShiftChanges pt-2">
                                                        
                                                    </div>--%>
                                                <div class="col-md-6 col-sm-8 col-xs-12 noPadding" id="iconContainer" runat="server" visible="false">
                                                    <asp:HiddenField ID="hdInitiateExport" runat="server" />
                                                    <asp:HiddenField ID="hdExportType" runat="server" />
                                                    <asp:HiddenField ID="exportURL" runat="server" />
                                                    <asp:Button ID="btExportAction" runat="server" CssClass="dnode" />
                                                    <div class="exportImport_btnsWrap my-2" style="">
                                                        <span class="fright" id="helpLinkContainer" runat="server" visible="false" style="padding-left: 3px">
                                                            <a id="btHelpLink" runat="server">
                                                                <img style="cursor: help; border: none;" src="/Content/images/help_22x22.png" width="16px" alt="Help" title="Help" />
                                                            </a>
                                                        </span>

                                                        <%--Report Menu Options--%>
                                                        <span id="exportAction1" style="padding-left: 3px" class="svcDashboard_reportIcon">
                                                            <img id="imgReport" runat="server" src="/Content/images/reports-Black.png" alt="Reports" title="Reports" style="cursor: pointer;width:20px;" class="imgReport" /><dx:ASPxPopupMenu ID="ASPxPopupMenuReportList" OnLoad="ASPxPopupMenuReportList_Load" runat="server" PopupElementID="imgReport" ShowPopOutImages="True" CloseAction="MouseOut" ItemSpacing="0"
                                                                ClientInstanceName="ASPxPopupMenuReportList" PopupHorizontalAlign="OutsideLeft" PopupVerticalAlign="TopSides" PopupAction="LeftMouseClick" CssClass="ddlActionMenu ddlPopupMenuReportList">
                                                                <Items>
                                                                </Items>
                                                                <ClientSideEvents ItemClick="function(s, e) { popupMenuReportItemClick(s,e);}" />
                                                                <ItemStyle Width="180px"></ItemStyle>
                                                            </dx:ASPxPopupMenu>
                                                        </span>

                                                        <span class="fright" runat="server" visible="true">
                                                            <dx:ASPxButton ID="btnAllocationtimeline" runat="server" Image-Height="20"  AutoPostBack="false" CssClass="d-none"
                                                                Image-Width="20" Image-Url="/Content/Images/ganttBlackNew.png" RenderMode="Link" ToolTip="Show Gantt View">
                                                                <ClientSideEvents Click="ChangeToGanttView" />

<Image Height="20px" Width="20px" Url="/Content/Images/ganttBlackNew.png"></Image>
                                                            </dx:ASPxButton>
                                                        </span>

                                                        <%-- Export Options--%>
                                                        <span id="exportAction" class="svcDashboard_exportIcon" runat="server" visible="false">
                                                            <img id="imgExportAction" class="export-icon-imgs-1" runat="server" src="/Content/images/export-Black.png" style="cursor: pointer;width:20px;" alt="Export" title="Export" />
                                                        </span>
                                                        
                                                        <dx:ASPxButton ID="ImportExcel" runat="server" AutoPostBack="False" RenderMode="Link" CssClass="importExcelClass" Visible="False">
                                                                <Image Url="/Content/images/icons/upload-icon.png" width="22"></Image>
                                                                <ClientSideEvents Click="function(s,e){OpenImportExcel();}" />
                                                        </dx:ASPxButton>
                                                        <dx:ASPxPopupMenu ID="ExportPopupMenu" runat="server" PopupElementID="exportAction" CloseAction="MouseOut" ItemSpacing="0"  ItemImage-Width="18px"
                                                            PopupHorizontalAlign="OutsideLeft" PopupVerticalAlign="TopSides" PopupAction="LeftMouseClick" CssClass="ddlActionMenu"
                                                            OnItemClick="ExportPopupMenu_ItemClick" OnLoad="ExportPopupMenu_Load" >
                                                            <Items>
                                                            </Items>
                                                            <ClientSideEvents ItemClick="function(s, e) {  popupMenuExportItemClick(s,e); }" />

<ItemImage Width="18px"></ItemImage>

                                                            <ItemStyle Width="25px"></ItemStyle>
                                                        </dx:ASPxPopupMenu>
                                                        <span class="filterImg_wrap">
                                                            <img id="imgAdvanceMode" class="homeDb_filterImg" runat="server" onclick="setFilterMode(this)" /></span><div id="cModuleGlobalFilterPanel" runat="server">
                                                            <asp:Panel ID="pGlobalFilters" runat="server">
                                                                <div id="pGlobalFiltersTable" runat="server">
                                                                    <div class="fright mr-3">
                                                                        <div class="FromTo_container">
                                                                            <div class="selectFrom_container xsColPadding">
                                                                                <b class="selectFromTo_label">From:</b>
                                                                                <div class="selectFromTo_field_new">
                                                                                    <dx:ASPxDateEdit ID="dtFrom" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                                                                                        CssClass="CRMDueDate_inputField homeDB-dateInput" runat="server" Visible="true">
<DropDownButton>
<Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
</DropDownButton>
                                                                                    </dx:ASPxDateEdit>
                                                                                </div>
                                                                            </div>
                                                                            <div class="selectTo_container xsColPadding">
                                                                                <b class="selectFromTo_label">To:</b>
                                                                                <div class="selectFromTo_field_new">
                                                                                    <dx:ASPxDateEdit ID="dtTo" runat="server" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                                                                                        CssClass="CRMDueDate_inputField homeDB-dateInput" Visible="true">
                                                                                        <DropDownButton>
                                                                                        <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                                                                        </DropDownButton>
                                                                                    </dx:ASPxDateEdit>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="searchFilter">
                                                                            <div class="searchFilter_searchWrap ml-2">

                                                                                <dx:ASPxButtonEdit ID="txtWildCard" runat="server" CssClass="txtWildCard" NullText="Search String" Width="150">
                                                                                    <ClientSideEvents ButtonClick="function(s,e){startLocalSearch(s,e);}" KeyPress="function(s,e){onSearchKeyPress(s,e)}" />
                                                                                    <Buttons>
                                                                                        <dx:EditButton>
                                                                                            <Image Url="/Content/images/searchNew.png" Width="14" ToolTip="Search">

                                                                                            </Image>
                                                                                        </dx:EditButton>
                                                                                    </Buttons>
                                                                                </dx:ASPxButtonEdit>
                                                                            </div>
                                                                        </div>
                                                                        <span id="globalfilterByModule" runat="server"><b style="padding-top: 2px; float: left; font-weight: normal;">Module:</b>

                                                                            <span>
                                                                                <asp:DropDownList ID="ddlModuleName" runat="server" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged" AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                            </span>
                                                                            <b style="padding-top: 2px; padding-left: 2px; float: left; font-weight: normal;">Field:</b>
                                                                            <span>
                                                                                <asp:DropDownList Width="100" ID="lstFilteredFields" runat="server">
                                                                                </asp:DropDownList>
                                                                            </span>
                                                                        </span>


                                                                        <%-- <asp:TextBox ID="txtWildCard" Style="width: 100px;" runat="server" CssClass="txtWildCard"></asp:TextBox>
                                                                                    <dfn class="s4-clust" style="height: 13px; width: 13px; right: 3px; top: 3px; position: absolute; display: inline-block; overflow: hidden;">
                                                                                        <asp:ImageButton ToolTip="Search" Style="border: 0pt none; position: absolute; left: 0px ! important; top: -218px ! important;"
                                                                                            ID="imagego" runat="server" ImageUrl="/Content/images/fgimg.png" OnClientClick="return startLocalSearch(this);" />
                                                                                    </dfn>--%>

                                                                        <%--<span>
                                                                                    <dfn class="s4-clust" style="height: 13px; width: 16px; position: relative; overflow: hidden; top: 1px;">
                                                                                        <img onclick="clearFilterFromList()" alt="*" src="/Content/images/fgimg.png" style="border: 0pt none; position: absolute; left: 0px ! important; top: -645px ! important;"
                                                                                            title="Clear search / filter(s)" />
                                                                                    </dfn>
                                                                                </span>--%>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                        <span id="onlyExcelImport" style="padding-left: 2px" class="fright" runat="server" visible="false">
                                                            <img src="/Content/images/import.png" style="cursor: pointer;" title="Import Excel" alt="Import Excel" onclick="return OpenImportExcel();" />
                                                        </span>
                                                        <span id="onlyExcelExport" style="padding-left: 2px" class="fright" runat="server" visible="true">
                                                            <img src="/Content/images/excel-icon.png" alt="Excel Export" title="Excel Export" onclick="downloadExcel(this);" style="cursor: pointer;" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row bottomBorder">
                                                <div id="cModuleFilteredLinksPanel" runat="server" class="">
                                                    <div class="customddl-button waitOnmeClass col-sm-12 col-md-12 col-xs-12 left-side-filtered-item flex-wrap">
                                                        <div class="smNoPadding noPaddingLeft">
                                                            <dx:ASPxComboBox ClientInstanceName="ddlCustomFilterHome" EncodeHtml="false" ID="ddlCustomFilterHome" runat="server"
                                                                CssClass="svcDashboard_dropdown homeGrid_dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {saveTabStage(s,e); setvalue(s,e);} " />

<ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                                            </dx:ASPxComboBox>
                                                          
                                                            <asp:Panel ID="pFilteredData" runat="server" Visible="true" CssClass="dxtcLite_UGITNavyBlueDevEx">
                                                                    <dx:ASPxTabControl ID="filterTab" AutoPostBack="False" runat="server" Theme="Default">
                                                                        <Tabs>
                                                                        </Tabs>
                                                                        <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px" CssClass="customFilteredTabStyle">
                                                                            <Paddings PaddingLeft="10px" PaddingRight="10px"></Paddings>
                                                                        </TabStyle>
                                                                        <ClientSideEvents EndCallback="ChangeColorForHold()" TabClick="function(s, e){saveTabStageForPanel(e.tab.name);setTabValue(e.tab.name);}"/>
                                                                    </dx:ASPxTabControl>
                                                                </asp:Panel>
                                                              
                                                        </div>
                                                        <div class="waitOnmeClass svcDashboard_btnWrap ml-2 py-1">
                                                            <div class="svcDashboard_drpDownWrap ml-0">
                                                                <div style="display:none;">
                                                                <dx:ASPxButton ID="btQuickTicket" runat="server" AutoPostBack="false" EnableTheming="false" CssClass="btn btn-sm quickTicket svcDashboard_quickTicketBtn">
                                                                    <Image Url="/Content/Images/quico ticket.png" Align="left" Width="14px" />
                                                                    <ClientSideEvents Click="function(s,e){return showTicketTemplate();}" />
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton ID="btNewbutton" Visible="false" AutoPostBack="false" runat="server" Text="&nbsp;&nbsp;New &nbsp;&nbsp;" ToolTip="New Request" ImagePosition="Right" EnableTheming="false"
                                                                    CssClass="btn btn-sm db-quickTicket svcDashboard_addTicketBtn">
                                                                    <Image Url="/Content/Images/Puzzle.png" Align="left" Width="14px" />
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton ID="btnNewContact" Visible="false" AutoPostBack="false" runat="server" Text="New" ToolTip="New Contact" ImagePosition="Right" EnableTheming="false"
                                                                    CssClass="btn btn-sm db-quickTicket svcDashboard_addTicketBtn">
                                                                    <ClientSideEvents Click="function(s,e){NewContactbyCompany();}" />
                                                                    <Image Url="/Content/Images/Puzzle.png" Align="left" />
                                                                </dx:ASPxButton>
                                                                    </div>
                                                                <div class="svcDashboard_actionBtnWrap" id="dvMenuItems" runat="server" >
                                                                    <dx:ASPxButton ID="lnkbtnCustomMenuItem" runat="server" Text="New" ForeColor="White" AutoPostBack="false" ImagePosition="Right" EnableTheming="false"
                                                                        CssClass="btn btn-sm svcDashboard_actionBtn-1 ml-0" Width="75px">
                                                                        <Image Url="/Content/images/icon-arrow-down-b.png" IconID="imgActionMenu" AlternateText="Reports"></Image>
                                                                        <ClientSideEvents Click="CreateNewProject" />
                                                                    </dx:ASPxButton>
                                                                </div>
                                                                <div class="svcDashboard_actionBtnWrap" id="dvActions" runat="server">
                                                                    <dx:ASPxButton ID="lnkbtnActionMenu" runat="server" Text="Actions" ForeColor="White" AutoPostBack="false" ImagePosition="Right" EnableTheming="false"
                                                                        CssClass="btn btn-sm svcDashboard_actionBtn-1" Width="100px">
                                                                        <Image Url="/Content/images/icon-arrow-down-b.png" IconID="imgActionMenu" AlternateText="Reports"></Image>
                                                                    </dx:ASPxButton>
                                                                </div>
                                                                <div class="svcDashboard_actionBtnWrap">
                                                                    <dx:ASPxButton Image-Url="/content/Images/gridBlackNew.png" Image-Height="30" ID="btnGridView" runat="server" Visible="false" OnClick="btnGridView_Click" RenderMode="Link" ToolTip="Grid View" CssClass="svcDashboard_actionBtn" Width="40">
<Image Height="30px" Url="/content/Images/gridBlackNew.png"></Image>
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton Image-Url="/content/Images/cardViewBlack-new.png" Image-Height="30" ID="btnCardView" runat="server" RenderMode="Link" OnClick="btnCardView_Click" ToolTip="Card View" CssClass="svcDashboard_actionBtn" Width="40">
<Image Height="30px" Url="/content/Images/cardViewBlack-new.png"></Image>
                                                                    </dx:ASPxButton>
                                                                </div>
                                                            </div>

                                                            <script data-v="<%=UGITUtility.AssemblyVersion %>">

                                    $(document).ready(function () {
                                        $('.agentDropDown_option').parent().addClass('actionBtn_agentOption_dropDownWrap');
                                                                });
                                                            </script>
                                                            <dx:ASPxPopupMenu ID="ASPxPopupCustomFilterActionMenu" OnLoad="ASPxPopupCustomFilterActionMenu_Load" runat="server" PopupElementID="lnkbtnActionMenu" CloseAction="MouseOut" ItemSpacing="0"
                                                                ClientInstanceName="ASPxPopupCustomFilterActionMenu" PopupHorizontalAlign="OutsideLeft" PopupVerticalAlign="TopSides" PopupAction="LeftMouseClick" CssClass="ddlActionMenu" SubMenuStyle-CssClass="agentDropDown_option" SubMenuItemStyle-DropDownButtonStyle-CssClass="agent_dropDownIcon">
                                                                <Items>
                                                                </Items>
                                                                <ClientSideEvents ItemClick="function(s, e) { popupMenuCustomFilterActionMenuItemClick(s,e);}" />
                                                                <ItemStyle>
                                                                    <HoverStyle>
                                                                    </HoverStyle>

                                                                </ItemStyle>
                                                                <SubMenuItemStyle>
                                                                    <HoverStyle>
                                                                    </HoverStyle>
<DropDownButtonStyle CssClass="agent_dropDownIcon"></DropDownButtonStyle>
                                                                </SubMenuItemStyle>

<SubMenuStyle CssClass="agentDropDown_option"></SubMenuStyle>
                                                            </dx:ASPxPopupMenu>
                                                            <dx:ASPxPopupMenu ID="ASPxPopupCustomMenuItem" OnLoad="ASPxPopupCustomMenuItem_Load" runat="server" PopupElementID="lnkbtnCustomMenuItem"  CloseAction="MouseOut" ItemSpacing="0"
                                                                ClientInstanceName="ASPxPopupCustomMenuItem" PopupHorizontalAlign="OutsideLeft" PopupVerticalAlign="TopSides" PopupAction="MouseOver" CssClass="ddlActionMenu ddlCustomFilterActionMenu" SubMenuStyle-CssClass="agentDropDown_option" SubMenuItemStyle-DropDownButtonStyle-CssClass="agent_dropDownIcon">
                                                                <Items>
                                                                </Items>
                                                                <ClientSideEvents ItemClick="function(s, e) { popupMenuCustomMenuItemClick(s,e);}" />
                                                                <ItemStyle>
                                                                    <HoverStyle>
                                                                    </HoverStyle>

                                                                </ItemStyle>
                                                                <SubMenuItemStyle>
                                                                    <HoverStyle>
                                                                    </HoverStyle>
<DropDownButtonStyle CssClass="agent_dropDownIcon"></DropDownButtonStyle>
                                                                </SubMenuItemStyle>

<SubMenuStyle CssClass="agentDropDown_option"></SubMenuStyle>
                                                            </dx:ASPxPopupMenu>
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                    <div id="customMessageContainer" runat="server" style="display: none;" class="row">
                                        <div class="ugitlight1lighter" style="padding: 2px 0px;" align="center">
                                            <span id="customMessage" class="customfitler-message">Displaying Filtered View, <a href="javascript:void(0)" onclick="clearFilterFromList();">Click Here to Reset Filter</a></span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div>
                                            <div>
                                                <div>
                                                    <div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <dx:ASPxHiddenField ID="callbackCommand" ClientInstanceName="callbackCommand" runat="server"></dx:ASPxHiddenField>
                                                    <dx:ASPxHiddenField ID="openTicketDialogCommand" ClientInstanceName="openTicketDialogCommand" runat="server"></dx:ASPxHiddenField>
                                                    <dx:ASPxHiddenField ID="columnMoved" ClientInstanceName="columnMoved" runat="server"></dx:ASPxHiddenField>
                                                    <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsPager-PageSize="10" Images-HeaderActiveFilter-Url="/Content/Images/Filter_Red_24.png"
                                                        OnHtmlRowPrepared="grid_HtmlRowPrepared" OnCustomCallback="grid_CustomCallback"
                                                        OnHeaderFilterFillItems="grid_HeaderFilterFillItems"
                                                        OnAfterPerformCallback="grid_AfterPerformCallback"
                                                        OnDataBinding="grid_DataBinding"
                                                        ClientInstanceName="gridClientInstance"
                                                        UseFixedTableLayout="false" SettingsText-CustomizationWindowCaption="Drag Columns to Hide"
                                                        ShowHorizontalScrollBar="true"
                                                        Width="100%" KeyFieldName="TicketId" Images-CustomizationWindowClose-Url="~/Content/Images/close-red.png"
                                                        CssClass="customgridview homeGrid SVCHomeGrid" EnableRowsCache="true">
                                                        <%--<clientsideevents begincallback="OnBeginCallback" />--%>

<SettingsText CustomizationWindowCaption="Drag Columns to Hide"></SettingsText>
                                                        <Toolbars>
                                                            <dx:GridViewToolbar Name="CustomizeColumns" Position="Top" ItemAlign="Right">
                                                                <%--<SettingsAdaptivity Enabled="true"  />--%>
                                                                <Items>
                                                                    <dx:GridViewToolbarItem Command="ShowCustomizationWindow" DisplayMode="Image" Checked="true"
                                                                        Text="Manage Columns" Image-Url="../../Content/Images/manageColum.png" Image-Width="18" EnableScrolling="true" ItemStyle-CssClass="column_choser" >
<Image Width="18px" Url="../../Content/Images/manageColum.png"></Image>

<ItemStyle CssClass="column_choser"></ItemStyle>
                                                                    </dx:GridViewToolbarItem>
                                                                </Items>
                                                            </dx:GridViewToolbar>
                                                        </Toolbars>
                                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                                        <Columns>
                                                        </Columns>
                                                        <SettingsExport PaperKind="A4Rotated" ExcelExportMode="DataAware" EnableClientSideExportAPI="true" />
                                                        <SettingsCommandButton>
                                                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn" >
<Styles>
<Style CssClass="homeGrid_openBTn"></Style>
</Styles>
                                                            </ShowAdaptiveDetailButton>
                                                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn">
<Styles>
<Style CssClass="homeGrid_closeBTn"></Style>
</Styles>
                                                            </HideAdaptiveDetailButton>
                                                        </SettingsCommandButton>
                                                        <SettingsPopup>
                                                            <HeaderFilter Height="200" />
                                                            <CustomizationWindow MinHeight="250" MinWidth="300" AllowResize="true" CloseOnEscape="True" />

<FilterControl AutoUpdatePosition="False"></FilterControl>
                                                        </SettingsPopup>
                                                        <SettingsPager>
                                                            <PageSizeItemSettings Position="Right" Visible="true" Items="5,10,15,20,25,50,100,200,500,1000"></PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Styles>
                                                            <Row CssClass="homeGrid_dataRow"></Row>
                                                            <Header CssClass="custom-background-color"></Header>
                                                        </Styles>
                                                        <SettingsCookies Enabled="true" StoreGroupingAndSorting="true" StoreColumnsWidth="false" StoreFiltering="true" Version="0" StorePaging="true" CookiesID="grid_Cookies" />
                                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                                        <SettingsBehavior AllowSort="true" AllowSelectByRowClick="false" EnableRowHotTrack="false" EnableCustomizationWindow="true" AllowFocusedRow="true"/>
                                                        <ClientSideEvents SelectionChanged="function(s, e) {   popupMenu(s,e);} " Init="grid_Init" EndCallback="function(s, e) {   customFilterGridViewCallBack(s,e); }"  ColumnMoving="function(s, e) { OnColumnMoving(s,e); }" />
                                                        <Images>
                                                            <HeaderActiveFilter Url="/Content/images/Filter_Red_24.png"></HeaderActiveFilter>

<CustomizationWindowClose Url="~/Content/Images/close-red.png"></CustomizationWindowClose>
                                                        </Images>
                                                    </dx:ASPxGridView>
                                                    <script type="text/javascript">
                                    try {
                                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                            UpdateGridHeight();
                                        });
                                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                        UpdateGridHeight();
                                                            });
                                                        } catch (e) {
                                    }
                                                    </script>
                                                </div>
                                                <br />
                                                <div>
                                                    <dx:ASPxCardView ID="CardView" ClientInstanceName="cardClientInstance" runat="server" KeyFieldName="TicketId"
                                                        Width="100%" AutoGenerateColumns="false" OnHtmlCardPrepared="CardView_HtmlCardPrepared"
                                                        OnCustomColumnDisplayText="CardView_CustomColumnDisplayText" CssClass="devexpress-projectCard"
                                                        OnHeaderFilterFillItems="CardView_HeaderFilterFillItems" Visible="false">
                                                         <%--[+][SANKET][20-10-2023][Added for grey loader]--%>

<SettingsExport ExportSelectedCardsOnly="False"></SettingsExport>

<StylesExport>
<Card BorderSize="1" BorderSides="All"></Card>

<Group BorderSize="1" BorderSides="All"></Group>

<TabbedGroup BorderSize="1" BorderSides="All"></TabbedGroup>

<Tab BorderSize="1"></Tab>
</StylesExport>

                                                        <Images>
                                                            <LoadingPanel Url="~/Content/Images/AjaxLoader.gif">
                                                            </LoadingPanel>
                                                        </Images>
                                                        <Settings ShowHeaderPanel="true" ShowHeaderFilterButton="false" />
                                                        <SettingsPopup>
                                                            <HeaderFilter Height="200" />
                                                            <CustomizationWindow MinHeight="250" MinWidth="300" AllowResize="true" CloseOnEscape="True" />

<FilterControl AutoUpdatePosition="False"></FilterControl>
                                                        </SettingsPopup>
                                                        <SettingsPager PageSizeItemSettings-Visible="true" SettingsTableLayout-ColumnCount ="4" SettingsTableLayout-RowsPerPage="5">
<SettingsTableLayout ColumnCount="4" RowsPerPage="5"></SettingsTableLayout>

<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Columns>
                                                        </Columns>
                                                        <ClientSideEvents Init="setClassValue" EndCallback="setClassValue"/>
                                                    </dx:ASPxCardView>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div style="height: 2px; display: block">
                                                    <div style="padding-top: 12px; min-height: 12px; max-height: 40px;">
                                                        <asp:Panel ID="lnkNewTicket" runat="server" Style="float: left; margin-right: 8px;" Visible="false">
                                                            <dx:ASPxButton CssClass="btn btn-sm  svcDashboard_addTicketBtn dbNew-btn" Height="23" AutoPostBack="false" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                                                                ID="lnkNewTicket1" ClientIDMode="Static" ClientInstanceName="lnkNewTicket1" runat="server" Text="New">
                                                                <Image Url="/Content/images/caret-down.png" Width="10px"></Image>
                                                                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                                                            </dx:ASPxButton>
                                                        </asp:Panel>
                                                        <div style="margin-right: 5px; float: left;">
                                                            <dx:ASPxButton CssClass="btn btn-sm db-quickTicket svcDashboard_quickTicketBtn" Height="23" Visible="false" AutoPostBack="false" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover" ID="lnkQuickTicket" runat="server" Text="Quick">
                                                                <Image Url="/Content/Images/quico ticket.png"></Image>
                                                                <ClientSideEvents Click="function(s,e){showTicketTemplate();}" />

                                                                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                                                            </dx:ASPxButton>
                                                        </div>
                                                        <div style="float: left; width: 100%;" id="cTicketPreviewPanel" runat="server">
                                                            <span style="float: left;">
                                                                <asp:Literal ID="previewListHeading" runat="server"></asp:Literal>
                                                            </span>
                                                            <span style="float: right; padding: 5px 2px 2px 0px;"><strong>
                                                                <asp:LinkButton ID="moreTicketLink" runat="server" Text="More >>" Style="font-family: 'Poppins', sans-serif;"></asp:LinkButton></strong>
                                                            </span>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                            <div id="mModuleNewTicketPanel" runat="server" style="display: none" class="row">
                                                <div>
                                                    <%--<div class="ugit-stick-bottom" style="height: 40px;">--%>
                                                    <div>
                                                        <div style="float: left; padding-top: 10px;" id="mModuleActionsContainer" runat="server">
                                                            <%--<dx:ASPxButton  ID="btNewbutton" Visible="false"  AutoPostBack="false" runat="server" Text="&nbsp;&nbsp;New&nbsp;&nbsp;" ToolTip="New Request" ImagePosition="Right">
                                                    <Image Url="/Content/images/add_icon.png"  />
                                                </dx:ASPxButton>
                                                 <dx:ASPxButton ID="btQuickTicket" runat="server" Text="Quick Ticket" AutoPostBack="false">
                                                     <Image Url="/Content/images/add_icon.png"  />
                                                     <ClientSideEvents Click="function(s,e){return showTicketTemplate();}" />
                                                </dx:ASPxButton>--%>
                                                        </div>
                                                        <div style="float: left; padding-top: 6px;">
                                                            <div runat="server" style="padding: 4px; display: none">
                                                                <%--  <dx:ASPxButton ID="lnkbtnActionMenu" runat="server" Text="Actions" ForeColor="White"  AutoPostBack="false" ImagePosition="Right">
                                                           <Image Url="/Content/images/icon-arrow-down-b.png" IconID="imgActionMenu" AlternateText="Reports"></Image>
                                                        </dx:ASPxButton>--%>

                                                                <dx:ASPxButton ID="btVendors" runat="server" Text="Vendors" AutoPostBack="false" ToolTip="Vendors" Visible="false">
                                                                </dx:ASPxButton>
                                                                <dx:ASPxButton ID="btAssetModels" runat="server" Text="Asset Models" AutoPostBack="false" ToolTip="Asset Models" Visible="false">
                                                                </dx:ASPxButton>
                                                            </div>

                                                            <%--<dx:ASPxPopupMenu ID="ASPxPopupCustomFilterActionMenu" OnLoad="ASPxPopupCustomFilterActionMenu_Load" runat="server" PopupElementID="lnkbtnActionMenu" CloseAction="MouseOut" ItemSpacing="0"
                                                    ClientInstanceName="ASPxPopupCustomFilterActionMenu" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick">
                                                    <Items>
                                                    </Items>
                                                    <ClientSideEvents ItemClick="function(s, e) { popupMenuCustomFilterActionMenuItemClick(s,e);}" />
                                                    <ItemStyle>
                                                        <HoverStyle>
                                                            
                                                        </HoverStyle>

                                                       
                                                    </ItemStyle>
                                                    <SubMenuItemStyle>
                                                        <HoverStyle>
                                                        
                                                        </HoverStyle>

                               
                                                    </SubMenuItemStyle>
                                                </dx:ASPxPopupMenu>--%>
                                                        </div>
                                                        <div style="float: right; padding-top: 10px; width: 80%; text-align: right">

                                                            <dx:ASPxPanel ID="pnlAgentBtns" runat="server" ClientInstanceName="pnlAgentBtns">

                                                                <PanelCollection>
                                                                    <dx:PanelContent runat="server"></dx:PanelContent>
                                                                </PanelCollection>

                                                            </dx:ASPxPanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- end--%>
                                        <%--<asp:UpdatePanel ID="subTicketUpdatePanel" runat="server">
                                    <Triggers>
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Button ID="btSubTicketButton" CssClass="hide" runat="server" OnClick="BtSubTicketButton_Click" />
                                        <asp:HiddenField ID="hfTicketInfo" runat="server" />
                                        <asp:Panel ID="subTicketPanel" runat="server">
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                        <%--start2--%>
                                        <div id="ganntPopup" style="display: none; height: auto; width: 260px;" class="ModuleBlock table-responsive">
                                            <fieldset>
                                                <legend>Build Gantt Chart</legend>
                                                <div>
                                                    <div class="row">
                                                        <div>
                                                            <asp:Label ID="lblGanttType" runat="server" Text="Group By" Font-Bold="true"> </asp:Label>
                                                        </div>
                                                        <div>
                                                            <asp:DropDownList ID="ddlGroupBy" runat="server">
                                                                <asp:ListItem Text="--None--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Priority" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Project Type" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Project Initiative" Value="3"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>&nbsp;</div>
                                                        <div>
                                                            <asp:CheckBox ID="chkOpenProjectOnly" runat="server" Text="Open Projects Only" Checked="true" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>
                                                            &nbsp;
                                                        </div>
                                                        <div style="text-align: right;">
                                                            <div class="first_tier_nav">
                                                                <ul>
                                                                    <li runat="server" id="Li12" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="float: right;">
                                                                        <asp:LinkButton ID="btnCancel" runat="server" Style="color: white" Visible="true" Text="Cancel" CssClass="cancelwhite" OnClientClick="HideGanttPopup();return false;" />
                                                                    </li>
                                                                    <li runat="server" id="Li11" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="float: right;">
                                                                        <asp:LinkButton runat="server" ID="btnBuildGantt" Style="color: white" Text="Build Gantt" CssClass="ganttImg" OnClientClick="javascript:return getGanttChart(this); return false;" />
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <%--end2--%>

                                        <%--start3--%>
                                        <div id="divTicketsReportsPopup" style="display: none; height: auto; width: 450px;" class="ModuleBlock">
                                            <fieldset>
                                                <legend>Item Summary Options</legend>
                                                <div class="table" style="padding-top: 10px;">
                                                    <div class="row">
                                                        <div style="vertical-align: middle; width: 100px">
                                                            <asp:Label ID="lbl" runat="server" Text="Modules" Font-Bold="true"> </asp:Label>
                                                        </div>
                                                        <div style="margin: 5px 0 5px 8px;">
                                                            <!-- select all boxes -->

                                                            <div style="width: 300px; overflow: auto; max-height: 150px; border: 1px solid black">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" Checked="false" Text="Select All" Style="margin-left: 3px; font-weight: 600;" />
                                                                <asp:CheckBoxList ID="cblModules" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                            </div>
                                                            <asp:CheckBox ID="chkSortByModule" runat="server" Checked="false" Style="display: none; margin-left: 3px; font-weight: 600;" Text="Sort By Module First" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>
                                                            <asp:Label ID="lblFilterCriteria" runat="server" Text="Item Status" Font-Bold="true"> </asp:Label>
                                                        </div>
                                                        <div>
                                                            <asp:RadioButtonList ID="rdbtnLstFilterCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria">
                                                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                                                <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>
                                                            <asp:Label ID="Label1" runat="server" Text="Sort By" Font-Bold="true"> </asp:Label>
                                                        </div>
                                                        <div>
                                                            <asp:RadioButtonList ID="rdSortCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria">
                                                                <asp:ListItem Text="Oldest First" Value="oldesttonewest" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Newest First" Value="newesttooldest"></asp:ListItem>
                                                                <asp:ListItem Text="Waiting On" Value="waitingon"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>
                                                            <asp:Label ID="Label2" runat="server" Text="Date Created" Font-Bold="true"> </asp:Label>
                                                        </div>
                                                        <div>
                                                            <div class="top_right_nav" style="left: 0px; float: left; margin-top: 6px;">
                                                                <b style="padding-top: 4px; float: left; font-weight: normal;">From:</b>
                                                                <span>
                                                                    <dx:ASPxDateEdit ID="dtFromTicketSummary" CssClassTextBox="inputTextBox datetimectr111" runat="server"></dx:ASPxDateEdit>

                                                                </span>

                                                                <b style="padding-top: 4px; float: left; font-weight: normal;">To:</b>
                                                                <span>
                                                                    <dx:ASPxDateEdit ID="dtToTicketSummary" CssClassTextBox="inputTextBox datetimectr111" runat="server"></dx:ASPxDateEdit>

                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div>
                                                            &nbsp;
                                                        </div>
                                                        <div style="text-align: right;">
                                                            <div class="first_tier_nav">
                                                                <ul style="margin-top: 5px; margin-bottom: 5px; background-color: #ECE8D3; border-top: 0px;">
                                                                    <li runat="server" id="Li1" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="float: right;">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" Style="color: white" Visible="true" Text="Cancel" CssClass="cancelwhite" OnClientClick="HideTicketsReportPopup();return false;" />
                                                                    </li>
                                                                    <li runat="server" id="Li2" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="float: right; margin-right: 10px; margin-left: 10px;">
                                                                        <asp:LinkButton runat="server" ID="LinkButton2" Style="color: white" Text="Build Report" CssClass="ganttImg" OnClientClick="javascript:return GetTicketsReportPopup(this);" />
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <%--end3--%>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <%--opm win and loss start--%>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" CloseAction="CloseButton" CloseOnEscape="true" EncodeHtml="true" HeaderText="OPM Wins and Losses Report"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="ASPxPopupControl1"
        AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AllowResize="true" Width="100%" SettingsAdaptivity-Mode="Always" HeaderStyle-CssClass="modal-header reportOPm-popupHeader"
        CloseButtonImage-Url="~/Content/Images/close-red-big.png" CssClass="departmentPopup_new copyToclip_popup_container opmWinLoss-roprtPopup">
        <ClientSideEvents />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel runat="server" DefaultButton="btnOK" ScrollBars="Vertical">
                    <PanelCollection>
                        <dx:PanelContent>
                            <fieldset>
                                <%--<legend class="winLosses-Header"><span id="spWinLossReport">OPM Wins and Losses Report</span></legend>--%>
                                <div class="tblReports col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="Label3" runat="server" Text="Date" CssClass="summary-reportLabel"> </asp:Label>
                                        </div>
                                        <div>
                                            <div class="reportDate-fieldWrap">
                                                <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">From:</b>
                                                <div>
                                                    <dx:ASPxDateEdit ID="ASPxDateEdit1" ClientInstanceName="dtWinLossRStart"
                                                        EnableClientSideAPI="true" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy"
                                                        NullText="MM/dd/yyyy" DateOnly="true" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                                                        DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="reportDate-fieldWrap1">
                                                <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">To:</b>
                                                <div>
                                                    <dx:ASPxDateEdit ID="ASPxDateEdit2" ClientInstanceName="dtWinLossREnd"
                                                        EnableClientSideAPI="true" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy"
                                                        NullText="MM/dd/yyyy" DateOnly="true" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                                                        DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row opmReport-btnWrap">
                                        <div class="summaryReport-btnContainer">
                                            <ul class="summaryReport-ul">
                                                <li runat="server" id="Li3" onmouseover="this.className='tabhover'"
                                                    onmouseout="this.className=''">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" Visible="true" Text="Cancel" CssClass="cancelReport-btn opmReport-btn"
                                                        OnClientClick="HideWinLossRPopup();return false;" />
                                                </li>
                                                <li runat="server" id="Li4" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                                    <asp:LinkButton runat="server" ID="LinkButton4" Text="Build Report" CssClass="buildReport-btn opmBuild-reportBtn"
                                                        OnClientClick="javascript:return GetWinLossRPopup(this);" />
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <%-- opm win and loss end--%>

    <dx:ASPxPopupControl ID="popupInsertUpdateMsg" runat="server" CloseAction="CloseButton" CloseOnEscape="true" EncodeHtml="true"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupInsertUpdateMsg"
        AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AllowResize="true">
        <ClientSideEvents CloseUp="OnCloseUp" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel runat="server" DefaultButton="btnOK" ScrollBars="Vertical">
                    <PanelCollection>
                        <dx:PanelContent>
                            <dx:ASPxLabel ID="lblInsertUpdateMsg" ClientInstanceName="lblInsertUpdateMsg" runat="server"></dx:ASPxLabel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxPopupMenu ID="AspxPopupMenuNewTicket" OnLoad="AspxPopupMenuNewTicket_Load" runat="server" PopupElementID="lnkNewTicket1" AutoPostBack="false" CloseAction="MouseOut" ItemSpacing="0"
        ClientInstanceName="AspxPopupMenuNewTicket" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick" Theme="Default" CssClass="dbNewBtn-popupList-wrap">
        <Items>
        </Items>
        <ItemStyle>
            <HoverStyle BackColor="#ccd4e1" ForeColor="White"></HoverStyle>
        </ItemStyle>
        <SubMenuItemStyle VerticalAlign="Middle">
            <HoverStyle>
            </HoverStyle>
        </SubMenuItemStyle>
    </dx:ASPxPopupMenu>
</div>

<div id="saveButton" class="btnAddNew mb-3" style="float:right;font-size:14px;margin-right:-5px;">
</div>
<div id="popup"></div>
<div id="toast"></div>
<div id="popupContactDetail"></div>
<div id="popupEmailDetail"></div>

<%--Callback to run Statistics--%>
 <dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel_Actions" Height="100%" ClientInstanceName="ASPxCallbackPanel_Actions" Enabled="true" OnCallback="ASPxCallbackPanel_Actions_Callback">
    <SettingsLoadingPanel Enabled="false" Text="Loading..." />
    <PanelCollection>
        <dx:PanelContent runat="server">
            <dx:ASPxLoadingPanel ID="busyLoader" CssClass="customeLoader" ClientInstanceName="busyLoader" Modal="True" runat="server" Image-Url="~/Content/IMAGES/AjaxLoader.gif" 
                ImagePosition="Top" ContainerElementID="filterticketlist" Text="Loading...">
                <Image Url="~/Content/IMAGES/AjaxLoader.gif"></Image>
            </dx:ASPxLoadingPanel>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

